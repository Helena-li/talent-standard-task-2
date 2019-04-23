using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.App.WebApp.Models;

namespace Talent.App.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(bool VerificationModal=false)
        {
            ViewBag.id = VerificationModal;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult TalentProfile()
        {
            return View();
        }

        public IActionResult EmployerProfile()
        {
            return View();
        }

        public IActionResult TalentFeed()
        {
            return View();
        }

        public IActionResult TalentDetail(string id)
        {
            ViewBag.id = id;
            return View();
        }

        public IActionResult TalentMatching()
        {
            return View();
        }

        public IActionResult EmployerFeed()
        {
            return View();
        }

        public IActionResult EmployerJobPost(string id=null)
        {
            ViewBag.id = id;
            return View();
        }

        public IActionResult ManageJobs()
        {
            return View();
        }
        public IActionResult UserSettings(string pageType,string token)
        {
            ViewBag.pageType = pageType;
            ViewBag.token = token;
            return View();
        }

        public ActionResult ResetPassword([FromQuery] string o, [FromQuery] string p)
        {
            try
            {
                string email = o != null ? o : throw new ArgumentException("Missing Email");
                string token = p != null ? p : throw new ArgumentException("Invalid Token");

                Regex rx = new Regex(
                    @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
                bool isEmail = rx.IsMatch(email);

                if (isEmail)
                {
                    return View("~/Views/Home/ResetPassword.cshtml");
                }
                throw new ArgumentException("Invalid Email");

            }
            catch (ArgumentException e)
            {
                return Redirect("/Home");
            }
        }

        public IActionResult UserAccountSetting()
        {
            return View();
        }


        public IActionResult TalentWatchlist()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
