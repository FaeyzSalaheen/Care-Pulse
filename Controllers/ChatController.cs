using Care_Pulse.Data;
using Care_Pulse.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Care_Pulse.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly CarePulseContext _context;

        public ChatController(CarePulseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var currentUserId = GetCurrentUserId();
            var otherUser = _context.Users.FirstOrDefault(u => u.Id != currentUserId);
            return View(otherUser);
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}