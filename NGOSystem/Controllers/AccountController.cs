using NGOSystem.Data;
using NGOSystem.Models;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        if (ModelState.IsValid)
        {
            // check if email already exists
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
            
            if(dbUser.Role=="Admin")
            {
                return RedirectToAction("AdminDashboard","Home");
            }
            else
            {
                return RedirectToAction("UserDashboard","Home");
            }
        }

        ViewBag.Error = "Invalid email or password";
        return View();
    }
}
