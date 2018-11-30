using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DemoApp1.Models;

namespace DemoApp1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() {
//            HttpContext.Request.Query.TryGetValue("token", out var tok);
//            if (tok.Count>0) {
//                var token = tok.ToString();
//                Response.Cookies.Append("siteurl1", token);
//            }

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
