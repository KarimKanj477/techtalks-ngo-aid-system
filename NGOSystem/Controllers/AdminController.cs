using Microsoft.AspNetCore.Mvc;
using NGOSystem.Data;

namespace NGOSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Dashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public IActionResult ManageRequests()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var requests = _context.AidRequests.ToList();

            return View(requests);
        }

        public IActionResult ManageUsers()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var users = _context.Users.ToList();

            return View(users);
        }
        public IActionResult Approve(int id)
        {
            var request = _context.AidRequests.FirstOrDefault(r => r.Id == id);

            if (request != null)
            {
                request.Status = "Approved";
                _context.SaveChanges();
            }

            return RedirectToAction("ManageRequests");
        }

        public IActionResult Reject(int id)
        {
            var request = _context.AidRequests.FirstOrDefault(r => r.Id == id);

            if (request != null)
            {
                request.Status = "Rejected";
                _context.SaveChanges();
            }

            return RedirectToAction("ManageRequests");
        }
    }
}