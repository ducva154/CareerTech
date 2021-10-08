using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareerTech.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //string rolename = accountService.GetRoleByEmail("cuongnm@gmail.com").Name;
            //ViewBag.rolename = rolename;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}