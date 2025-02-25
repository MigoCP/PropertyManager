using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Property_Manager_WebApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

public class AppointmentsController : Controller
{
    private readonly PropertyRentalManagementContext _context;
    private readonly ILogger<AppointmentsController> _logger;

    public AppointmentsController(PropertyRentalManagementContext context, ILogger<AppointmentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Create()
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        var userId = HttpContext.Session.GetString("UserId");

        if (string.IsNullOrEmpty(userRole) || string.IsNullOrEmpty(userId))
        {
            // Redirect to login if session data is missing
            return RedirectToAction("Login", "Account");
        }

        if (userRole == "Tenant" && int.TryParse(userId, out int tenantId))
        {
            var tenant = _context.Users.FirstOrDefault(u => u.UserId == tenantId);
            ViewBag.IsTenant = true;
            ViewBag.TenantName = tenant?.FullName;
            ViewBag.TenantIdFixed = tenantId; // Separate key for fixed TenantId
        }
        else if (userRole == "Manager")
        {
            ViewBag.IsManager = true;
            ViewBag.ManagerId = int.Parse(userId);
        }
        else if (userRole == "Owner")
        {
            ViewBag.IsOwner = true;
        }

        // Populate dropdowns for tenants and managers
        ViewBag.TenantDropdown = new SelectList(_context.Users.Where(u => u.Role == "Tenant"), "UserId", "FullName");
        ViewBag.ManagerId = new SelectList(_context.Users.Where(u => u.Role == "Manager"), "UserId", "FullName");
        ViewBag.ApartmentId = new SelectList(Enumerable.Empty<SelectListItem>());

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TenantId,ManagerId,ApartmentId,Status")] Appointment appointment, string selectedDay, string selectedTime)
    {
        _logger.LogInformation("POST Create Method Called");

        var userRole = HttpContext.Session.GetString("UserRole");
        var userId = HttpContext.Session.GetString("UserId");
        if (userRole == "Tenant" && int.TryParse(userId, out int tenantId))
        {
            appointment.TenantId = tenantId;
            _logger.LogInformation($"Enforced TenantId: {tenantId}");
        }

        if (!string.IsNullOrEmpty(selectedDay) && !string.IsNullOrEmpty(selectedTime))
        {
            try
            {
                var targetDay = Enum.Parse<DayOfWeek>(selectedDay, true);
                var today = DateTime.Now.Date;
                var targetDate = today.AddDays(((int)targetDay - (int)today.DayOfWeek + 7) % 7);

                if (TimeSpan.TryParse(selectedTime, out var time))
                {
                    appointment.ScheduledDate = targetDate.Add(time);
                    _logger.LogInformation($"ScheduledDate Set: {appointment.ScheduledDate}");
                }
                else
                {
                    throw new Exception("Invalid time format");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Invalid day or time selected: {ex.Message}");
                ModelState.AddModelError("ScheduledDate", "Invalid day or time selected.");
            }
        }
        else
        {
            ModelState.AddModelError("ScheduledDate", "Day and time are required.");
        }

        if (!ModelState.IsValid)
        {
            PopulateDropdowns(appointment);
            return View(appointment);
        }

        try
        {
            _context.Add(appointment);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Appointment created successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unexpected error: {ex.Message}");
            ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
        }

        PopulateDropdowns(appointment);
        return View(appointment);
    }


    private void PopulateDropdowns(Appointment appointment)
    {
        ViewBag.ManagerId = new SelectList(_context.Users.Where(u => u.Role == "Manager"), "UserId", "FullName", appointment.ManagerId);

        ViewBag.ApartmentId = new SelectList(
            _context.Apartments
                .Include(a => a.Building)
                .Where(a => a.Building.ManagerId == appointment.ManagerId && a.Status == "Available"),
            "ApartmentId", "Address", appointment.ApartmentId
        );

        var userRole = HttpContext.Session.GetString("UserRole");
        if (userRole != "Tenant")
        {
            ViewBag.TenantDropdown = new SelectList(_context.Users.Where(u => u.Role == "Tenant"), "UserId", "FullName", appointment.TenantId);
        }
        else
        {
            var tenantId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
            ViewBag.TenantIdFixed = tenantId;
            ViewBag.TenantName = _context.Users.FirstOrDefault(u => u.UserId == tenantId)?.FullName;
        }
    }




    // AJAX: Get apartments by manager
    [HttpGet]
    public JsonResult GetApartmentsByManager(int managerId)
    {
        var apartments = _context.Apartments
            .Include(a => a.Building)
            .Where(a => a.Building.ManagerId == managerId && a.Status == "Available")
            .Select(a => new { a.ApartmentId, a.Address })
            .ToList();

        return Json(apartments);
    }

    // AJAX: Get available days for a selected manager
    [HttpGet]
    public JsonResult GetAvailableDays(int managerId)
    {
        var availableDays = _context.Schedules
            .Where(s => s.ManagerId == managerId)
            .Select(s => s.DayOfWeek)
            .Distinct()
            .ToList();

        return Json(availableDays);
    }

    // AJAX: Get available times for a selected manager and day
    [HttpGet]
    public JsonResult GetAvailableTimes(int managerId, string dayOfWeek)
    {
        // Parse dayOfWeek to DayOfWeek enum
        if (!Enum.TryParse<DayOfWeek>(dayOfWeek, true, out var dayOfWeekEnum))
        {
            return Json(new { error = "Invalid day of the week." });
        }

        // Fetch schedule for the manager
        var schedule = _context.Schedules
            .Where(s => s.ManagerId == managerId && s.DayOfWeek == dayOfWeek)
            .FirstOrDefault();

        if (schedule == null)
        {
            return Json(new List<string>());
        }

        // Filter booked slots from database
        var allAppointments = _context.Appointments
            .Where(a => a.ManagerId == managerId)
            .AsEnumerable() // Switch to in-memory processing
            .Where(a => a.ScheduledDate.DayOfWeek == dayOfWeekEnum) // Apply DayOfWeek filter
            .Select(a => a.ScheduledDate.TimeOfDay)
            .ToList();

        // Generate available time slots
        var availableSlots = new List<string>();
        for (var time = schedule.StartTime; time < schedule.EndTime; time = time.Add(TimeSpan.FromMinutes(30)))
        {
            if (!allAppointments.Contains(time))
            {
                availableSlots.Add(time.ToString(@"hh\:mm"));
            }
        }

        return Json(availableSlots);
    }

    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetString("UserId");
        var userRole = HttpContext.Session.GetString("UserRole");

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userRole))
        {
            // Redirect to login if session data is missing
            return RedirectToAction("Login", "Account");
        }

        IQueryable<Appointment> appointmentsQuery = _context.Appointments
            .Include(a => a.Tenant)
            .Include(a => a.Manager)
            .Include(a => a.Apartment);

        // Filter appointments based on user role
        if (userRole == "Tenant" && int.TryParse(userId, out int tenantId))
        {
            appointmentsQuery = appointmentsQuery.Where(a => a.TenantId == tenantId);
        }
        else if (userRole == "Manager" && int.TryParse(userId, out int managerId))
        {
            appointmentsQuery = appointmentsQuery.Where(a => a.ManagerId == managerId);
        }
        else if (userRole == "Owner")
        {
            // Owners can see all appointments or you can add specific filters for Owners here
        }
        else
        {
            // Redirect unauthorized users
            return RedirectToAction("Login", "Account");
        }

        var appointments = await appointmentsQuery.ToListAsync();
        return View(appointments);
    }



}
