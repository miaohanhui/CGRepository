using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CGBlockMarket.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Policy = "Permission")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
