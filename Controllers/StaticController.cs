using Microsoft.AspNetCore.Mvc;

namespace Care_Pulse.Controllers
{
    public class StaticController : Controller 
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Articles()
        {
            return View();
        }

       
        public IActionResult About()
        {
            return View();
        }
        public IActionResult ExerciseByMuscle()
        {
            return View();
        }
        public IActionResult Nutrition()
        {
            return View();
        }
        public IActionResult WaterTracker()
        {
            return View();
        }
        public IActionResult VEGAN()
        {
            return View();
        }
        public IActionResult GENERALWELLNESSS()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

    }
}