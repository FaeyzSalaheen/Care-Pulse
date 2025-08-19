using Care_Pulse.Data;
using Care_Pulse.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Care_Pulse.Controllers
{
    
    public class ChatController : Controller
    {    
        public IActionResult Index()
        {
            return View();
        }
    }
}