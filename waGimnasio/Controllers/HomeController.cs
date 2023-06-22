using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using waGimnasio.Models;

namespace waGimnasio.Controllers
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
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            return View();
        }

        public IActionResult Privacy()
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}