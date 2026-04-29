using Microsoft.AspNetCore.Mvc;
using NGOSystem.Data;
using NGOSystem.Models;

public class AidRequestController : Controller
{
    private readonly AppDbContext _context;

    public AidRequestController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(AidRequest request)
    {
        request.Status = "Pending";
        request.UserEmail = HttpContext.Session.GetString("UserEmail");

        _context.AidRequests.Add(request);
        _context.SaveChanges();

        return RedirectToAction("MyRequests");
    }

    public IActionResult MyRequests()
    {
        var email = HttpContext.Session.GetString("UserEmail");

        var requests = _context.AidRequests
            .Where(r => r.UserEmail == email)
            .ToList();

        return View(requests);
    }
}