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
    public class ReportsController : Controller
    {
        private readonly PropertyRentalManagementContext _context;

        public ReportsController(PropertyRentalManagementContext context)
        {
            _context = context;
        }

        // GET: Reports/ManagerIndex
        public async Task<IActionResult> ManagerIndex()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            int managerId = int.Parse(userId);

            var managerReports = _context.Reports
                .Include(r => r.Manager)
                .Include(r => r.Owner)
                .Where(r => r.ManagerId == managerId);

            return View(await managerReports.ToListAsync());
        }


        // GET: Reports/OwnerIndex
        public async Task<IActionResult> OwnerIndex()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            int ownerId = int.Parse(userId);

            var ownerReports = _context.Reports
                .Include(r => r.Manager)
                .Include(r => r.Owner)
                .Where(r => r.OwnerId == ownerId);

            return View(await ownerReports.ToListAsync());
        }



        // GET: Reports
        public async Task<IActionResult> Index()
        {
            var propertyRentalManagementContext = _context.Reports.Include(r => r.Manager).Include(r => r.Owner);
            return View(await propertyRentalManagementContext.ToListAsync());
        }

        // GET: Reports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.Manager)
                .Include(r => r.Owner)
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // GET: Reports/Create
        public IActionResult Create()
        {
            ViewData["ManagerId"] = new SelectList(
                _context.Users.Where(u => u.Role == "Manager"),
                "UserId",
                "FullName"
            );

            ViewData["OwnerId"] = new SelectList(
                _context.Users.Where(u => u.Role == "Owner"),
                "UserId",
                "FullName"
            );

            return View();
        }


        // POST: Reports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportId,ManagerId,OwnerId,Title,Description,ReportDate,Status")] Report report)
        {
            if (ModelState.IsValid)
            {
                _context.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerId"] = new SelectList(_context.Users, "UserId", "UserId", report.ManagerId);
            ViewData["OwnerId"] = new SelectList(_context.Users, "UserId", "UserId", report.OwnerId);
            return View(report);
        }

        // GET: Reports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            ViewData["ManagerId"] = new SelectList(_context.Users, "UserId", "UserId", report.ManagerId);
            ViewData["OwnerId"] = new SelectList(_context.Users, "UserId", "UserId", report.OwnerId);
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportId,ManagerId,OwnerId,Title,Description,ReportDate,Status")] Report report)
        {
            if (id != report.ReportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.ReportId))
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
            ViewData["ManagerId"] = new SelectList(_context.Users, "UserId", "UserId", report.ManagerId);
            ViewData["OwnerId"] = new SelectList(_context.Users, "UserId", "UserId", report.OwnerId);
            return View(report);
        }

        // GET: Reports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.Manager)
                .Include(r => r.Owner)
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report != null)
            {
                _context.Reports.Remove(report);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.ReportId == id);
        }
    }
}
