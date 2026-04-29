using Microsoft.AspNetCore.Mvc;

namespace NGOSystem.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Dashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "User")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
    }
}