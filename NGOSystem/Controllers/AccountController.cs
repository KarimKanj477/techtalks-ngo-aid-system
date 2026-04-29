using NGOSystem.Data;
using NGOSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace NGOSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);

                if (existingUser != null)
                {
                    ViewBag.Error = "Email already exists";
                    return View(user);
                }

                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            var dbUser = _context.Users
                .FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);

            if (dbUser != null)
            {
                HttpContext.Session.SetString("Username", dbUser.Username);
                HttpContext.Session.SetString("UserEmail", dbUser.Email);
                HttpContext.Session.SetString("UserRole", dbUser.Role);

                if (dbUser.Role == "Admin")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }

                return RedirectToAction("Dashboard", "User");
            }

            ViewBag.Error = "Invalid email or password";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}