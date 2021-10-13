using CareerTech.Models;
using CareerTech.Services;
using CareerTech.Services.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareerTech.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISubscriptionManagementService<SubscriptionManagementService> _subscriptionManagementService;
        private readonly ISolutionManagementService<SolutionManagementService> _solutionManagementService;
        private readonly IContentService<ContentService> _contentService;
        private readonly IAboutManagement<AboutService> _aboutManagement;

        public HomeController(ISubscriptionManagementService<SubscriptionManagementService> subscriptionManagementService,
            ISolutionManagementService<SolutionManagementService> solutionManagementService,
            IContentService<ContentService> contentService, IAboutManagement<AboutService> aboutManagement)
        {
            _subscriptionManagementService = subscriptionManagementService;
            _solutionManagementService = solutionManagementService;
            _contentService = contentService;
            _aboutManagement = aboutManagement;
        }
        public ActionResult Index()
        {
            var subscription = _subscriptionManagementService.GetSubscriptions();
            ViewBag.Subs = subscription;
            var solution = _solutionManagementService.GetSolutions();
            ViewBag.Sol = solution;
            var intro = _contentService.GetPublicIntroduction();
            if (intro != null)
            {
                ViewBag.Intro = intro;
            }
            else
            {
                ViewBag.Intro = _contentService.GetIntroduction();
            }

            return View();
        }

        public ActionResult About()
        {
            var about = _aboutManagement.getMainAbout();
            if (about != null)
            {
                ViewBag.About = about;
            }
            else
            {
                ViewBag.About = _aboutManagement.getAbout();
            }
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}