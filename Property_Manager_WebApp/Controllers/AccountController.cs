using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Property_Manager_WebApp.Models;

namespace Property_Manager_WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly PropertyRentalManagementContext _context;

        public AccountController(PropertyRentalManagementContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Check if the user exists with the given email and password
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                // Set error message
                ViewBag.Error = "Invalid email or password.";
                return View(); // Return to the Login view with the error message
            }

            // Store user information in Session
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserRole", user.Role);
            HttpContext.Session.SetString("UserId", user.UserId.ToString()); // Add UserId to session

            // Redirect based on user role
            return user.Role switch
            {
                "Owner" => RedirectToAction("OwnerIndex", "Home"),
                "Manager" => RedirectToAction("ManagerIndex", "Home"), // Updated for Manager
                "Tenant" => RedirectToAction("TenantIndex", "Home"),  // Updated for Tenant
                _ => RedirectToAction("Index", "Home")
            };

        }


        // GET: SignUp
        public IActionResult SignUp()
        {
            return View();
        }

        // POST: SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(string fullName, string email, string password, string role)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
            {
                ViewBag.Error = "Email is already registered.";
                return View();
            }

            if (role != "Owner" && role != "Manager" && role != "Tenant")
            {
                ViewBag.Error = "Invalid role selected.";
                return View();
            }

            var newUser = new User
            {
                FullName = fullName,
                Email = email,
                Password = password,
                Role = role
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            TempData["UserName"] = newUser.FullName;
            TempData["UserRole"] = newUser.Role;

            return RedirectToAction("Index", "Home");
        }

        // GET: Logout
        public IActionResult Logout()
        {
            // Clear session data
            HttpContext.Session.Clear();

            // Redirect to the general Home page
            return RedirectToAction("Index", "Home");
        }

    }
}
