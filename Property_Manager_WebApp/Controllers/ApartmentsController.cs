using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Property_Manager_WebApp.Models;

namespace Property_Manager_WebApp.Controllers
{
    public class ApartmentsController : Controller
    {
        private readonly PropertyRentalManagementContext _context;
        private readonly ILogger<AppointmentsController> _logger;


        public ApartmentsController(PropertyRentalManagementContext context, ILogger<AppointmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Apartments/Index
        public async Task<IActionResult> Index()
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userRole)) // Guest user
            {
                // Return only the top 5 apartments for guests
                var apartments = await _context.Apartments
                    .Include(a => a.Building)
                    .Take(5)
                    .ToListAsync();

                ViewBag.IsGuest = true; // Pass flag to the view
                return View(apartments);
            }

            // Return all apartments for authenticated users
            var allApartments = await _context.Apartments
                .Include(a => a.Building)
                .ToListAsync();

            ViewBag.IsGuest = false; // Pass flag to the view
            return View(allApartments);
        }


        // GET: TenantIndex - Show all apartments
        public async Task<IActionResult> TenantIndex()
        {
            var apartments = await _context.Apartments
                .Include(a => a.Building)
                .ToListAsync();
            return View(apartments);
        }

        // GET: ManagerIndex - Show apartments for manager's buildings
        [HttpGet]
        public async Task<IActionResult> ManagerIndex()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            int managerId = int.Parse(userId);

            var apartments = await _context.Apartments
                .Include(a => a.Building)
                .Where(a => a.Building.ManagerId == managerId)
                .ToListAsync();

            // Render the view from /Views/Apartments/ManagerIndex.cshtml
            return View(apartments);
        }

        public async Task<IActionResult> GuestIndex()
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            // If not a guest, redirect to login or appropriate action
            if (!string.IsNullOrEmpty(userRole))
            {
                return RedirectToAction("Login", "Account");
            }

            // Limit to 5 apartments for guest users
            var apartments = await _context.Apartments
                .Include(a => a.Building)
                .Take(5) // Restrict to 5 records
                .ToListAsync();

            return View("GuestIndex", apartments);
        }



        // GET: Apartments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartments
                .Include(a => a.Building)
                .FirstOrDefaultAsync(m => m.ApartmentId == id);
            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        // GET: Apartments/Create
        public IActionResult Create()
        {
            // Populate dropdown with BuildingId as the value and Building.Name as the display
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "BuildingId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BuildingId,Address,Description,Status,NumberOfRooms")] Apartment apartment, IFormFile? ApartmentImage)
        {
            if (!ModelState.IsValid)
            {
                ViewData["BuildingId"] = new SelectList(_context.Buildings, "BuildingId", "Name", apartment.BuildingId);
                return View(apartment);
            }

            try
            {
                // Get the selected building to ensure the address matches
                var building = await _context.Buildings.FirstOrDefaultAsync(b => b.BuildingId == apartment.BuildingId);
                if (building == null)
                {
                    ModelState.AddModelError("BuildingId", "The selected building does not exist.");
                    ViewData["BuildingId"] = new SelectList(_context.Buildings, "BuildingId", "Name", apartment.BuildingId);
                    return View(apartment);
                }

                // Validate that the apartment address matches the building address
                if (!apartment.Address.StartsWith(building.Address))
                {
                    ModelState.AddModelError("Address", "The apartment address must match the building's address.");
                    ViewData["BuildingId"] = new SelectList(_context.Buildings, "BuildingId", "Name", apartment.BuildingId);
                    return View(apartment);
                }

                // Add the apartment to the database
                _context.Add(apartment);
                await _context.SaveChangesAsync();

                // Handle image upload
                if (ApartmentImage != null && ApartmentImage.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/apartments");

                    // Ensure the folder exists
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Generate the file path with the apartment ID as the file name and .png extension
                    string filePath = Path.Combine(uploadsFolder, $"{apartment.ApartmentId}.png");

                    // Save the file as .png using System.Drawing.Common or SkiaSharp
                    using (var stream = ApartmentImage.OpenReadStream())
                    {
                        var image = System.Drawing.Image.FromStream(stream);

                        // Save the image as .png
                        image.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating apartment: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while creating the apartment.");
                ViewData["BuildingId"] = new SelectList(_context.Buildings, "BuildingId", "Name", apartment.BuildingId);
                return View(apartment);
            }
        }




        // GET: Apartments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartments.FindAsync(id);
            if (apartment == null)
            {
                return NotFound();
            }
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "BuildingId", "BuildingId", apartment.BuildingId);
            return View(apartment);
        }

        // POST: Apartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ApartmentId,BuildingId,UnitNumber,Description,Status,NumberOfRooms")] Apartment apartment)
        {
            if (id != apartment.ApartmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apartment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApartmentExists(apartment.ApartmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "BuildingId", "Name", apartment.BuildingId);
            return View(apartment);
        }

        // GET: Apartments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartments
                .Include(a => a.Building)
                .FirstOrDefaultAsync(m => m.ApartmentId == id);
            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apartment = await _context.Apartments.FindAsync(id);
            if (apartment != null)
            {
                _context.Apartments.Remove(apartment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApartmentExists(int id)
        {
            return _context.Apartments.Any(e => e.ApartmentId == id);
        }

        // AJAX: Get Building Address
        [HttpGet]
        public JsonResult GetBuildingAddress(int buildingId)
        {
            var building = _context.Buildings
                .FirstOrDefault(b => b.BuildingId == buildingId);

            if (building == null)
            {
                return Json(new { error = "Building not found" });
            }

            return Json(new { address = building.Address });
        }

    }
}
