using Microsoft.AspNetCore.Mvc;

namespace Cinemart.Controllers
{
    public class AccountController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
