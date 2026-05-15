using Microsoft.AspNetCore.Mvc;
using NGOSystem.Data;
using NGOSystem.Models;

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

            ViewBag.TotalRequests = _context.AidRequests.Count();

            ViewBag.PendingRequests =
                _context.AidRequests.Count(r => r.Status == "Pending");

            ViewBag.ApprovedRequests =
                _context.AidRequests.Count(r => r.Status == "Approved");

            ViewBag.RejectedRequests =
                _context.AidRequests.Count(r => r.Status == "Rejected");

            return View();
        }

        public IActionResult ManageRequests(string status, string search)
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var requests = _context.AidRequests.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                requests = requests.Where(r => r.Status == status);
            }

            if (!string.IsNullOrEmpty(search))
            {
                requests = requests.Where(r =>
                    r.Title.Contains(search) ||
                    r.Location.Contains(search) ||
                    r.UserEmail.Contains(search));
            }

            ViewBag.SelectedStatus = status;
            ViewBag.Search = search;

            return View(requests.ToList());
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
        public IActionResult Delete(int id)
        {
            var request = _context.AidRequests.FirstOrDefault(r => r.Id == id);

            if (request != null)
            {
                _context.AidRequests.Remove(request);
                _context.SaveChanges();
            }

            return RedirectToAction("ManageRequests");
        }
        public IActionResult Edit(int id)
        {
            var request = _context.AidRequests.Find(id);

            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        [HttpPost]
        public IActionResult Edit(AidRequest request)
        {
            _context.AidRequests.Update(request);
            _context.SaveChanges();

            return RedirectToAction("ManageRequests");
        }
        public IActionResult Details(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            } 
            var request = _context.AidRequests.FirstOrDefault(r => r.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }
    }
}