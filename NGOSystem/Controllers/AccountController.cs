using Microsoft.AspNetCore.Mvc;
using NGOSystem.Models;

namespace NGOSystem.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}