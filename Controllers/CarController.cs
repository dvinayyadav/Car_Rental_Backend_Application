using Microsoft.AspNetCore.Mvc;

namespace Car_Rental_Backend_Application.Controllers
{
    public class CarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
