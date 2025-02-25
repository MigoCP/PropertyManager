using Microsoft.AspNetCore.Mvc;
using Property_Manager_WebApp.Models;
using System.Diagnostics;

namespace Property_Manager_WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OwnerIndex()
        {
            return View();
        }

        public IActionResult ManagerIndex()
        {
            return View();
        }

        public IActionResult TenantIndex()
        {
            return View();
        }

        // Optionally: A stub action for reporting events (Manager functionality)
        public IActionResult ReportEvent()
        {
            // This would lead to a form where managers can report events
            return View();
        }
    }
}