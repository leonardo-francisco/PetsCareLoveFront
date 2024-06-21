using Microsoft.AspNetCore.Mvc;

namespace PetsCareLove.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
