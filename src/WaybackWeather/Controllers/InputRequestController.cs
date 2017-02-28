using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WaybackWeather.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WaybackWeather.Controllers
{
    public class InputRequestController : Controller
    {
        // GET: /<controller>/
        public IActionResult NewRequest()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RequestPartial(InputRequest input, string dateString)
        {
            long date = new DateTimeOffset(DateTime.Parse(dateString)).ToUnixTimeSeconds();
            input.Date = date;
            try
            {
                input.GetLatLong();
            }
            catch
            {
                return View("InvalidLocation");
            }
            ViewBag.Weather = input.GetWeather();
            if (ViewBag.Weather["summary"] == null)
            {
                return View("InvalidDate");
            }

            return View();
        }


    }
}
