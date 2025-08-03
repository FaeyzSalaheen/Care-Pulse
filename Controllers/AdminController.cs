using Care_Pulse.Data;
using Care_Pulse.Filters;
using Care_Pulse.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Care_Pulse.Controllers
{
    [TypeFilter(typeof(RoleFilter), Arguments = new object[] { 1 })]
    public class AdminController : Controller
    {
        private readonly CarePulseContext _context;

        public AdminController(CarePulseContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Posts()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var posts = _context.Contents
                .Where(c => c.UploadedById == userId)
                .ToList();
            return View(posts);
        }

        public IActionResult Chat()
        {
            var users = _context.Users.Where(u => u.Role == 2).ToList();
            return View(users);
        }

        [HttpPost("/Admin/Posts")]
        public async Task<IActionResult> Posts([FromForm] PostRequest request)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (!userId.HasValue)
                {
                    return Unauthorized(new { success = false, message = "يجب تسجيل الدخول أولاً" });
                }

                string fileUrl = null;

                
                if (request.ImageFile != null && request.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + request.ImageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.ImageFile.CopyToAsync(fileStream);
                    }

                    fileUrl = "/uploads/" + uniqueFileName;
                }
                else if (!string.IsNullOrEmpty(request.FileUrl))
                {
                    fileUrl = request.FileUrl;
                }

                var newPost = new Content
                {
                    Title = request.Title,
                    Description = request.Description,
                    FileUrl = fileUrl,
                    UploadedById = userId.Value,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };

                _context.Contents.Add(newPost);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "تم إنشاء المنشور بنجاح",
                    postId = newPost.Id,
                    fileUrl = fileUrl
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء إنشاء المنشور",
                    error = ex.Message
                });
            }
        }

       
        public class PostRequest
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public IFormFile? ImageFile { get; set; } 
            public string? FileUrl { get; set; }
        }





    }
}
    
