using Care_Pulse.Data;
using Microsoft.AspNetCore.Mvc;

namespace Care_Pulse.Controllers
{
    public class HomeController : Controller
    {
        private readonly CarePulseContext _context;

        public HomeController(CarePulseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");
            return View();
        }
    }
}