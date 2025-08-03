using Care_Pulse.Data;
using Care_Pulse.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Care_Pulse.Controllers
{
    public class ContentController : Controller
    {
        private readonly CarePulseContext _context;

        public ContentController(CarePulseContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/Content/ViewPosts")]
        public IActionResult GetPosts()
        {
            try
            {
                var posts = _context.Contents
                    .Include(c => c.UploadedBy)
                    .Where(c => c.IsActive)
                    .OrderByDescending(c => c.CreatedAt)
                    .Select(c => new PostViewModel 
                    {
                        Title = c.Title,
                        Description = c.Description,
                        FileUrl = c.FileUrl,
                        Author = c.UploadedBy.Name,
                        CreatedAt = c.CreatedAt
                    })
                    .ToList();

                return View("ViewPosts", posts);
            }
            catch (Exception ex)
            {
                return View("ViewPosts", new List<PostViewModel>());
            }
        }
    }
}