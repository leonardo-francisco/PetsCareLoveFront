using Microsoft.AspNetCore.Mvc;

namespace PetsCareLove.Web.Controllers
{
    public class TrainerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
