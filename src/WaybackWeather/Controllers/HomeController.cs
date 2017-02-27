using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WaybackWeather.Models;

namespace WaybackWeather.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewRequest()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewRequest(InputRequest input, string dateString)
        {
            long date = new DateTimeOffset(DateTime.Parse(dateString)).ToUnixTimeSeconds();
            input.Date = date;
            return RedirectToAction("Index");
        }
    }
}
