using Microsoft.AspNetCore.Mvc;

namespace Hackathon_TEK.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public virtual IActionResult HomeView()
        {
            return PartialView("_HomePartial");
        }
    }
}