using Antemis.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Антемис.Models;

namespace Антемис.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            UsersRepository.CurrentUsersInn = null;
            UsersRepository.CurrentUser = null;
            HotelsRepository.CurrentHotel = null;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
