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

        public HomeController(ISubscriptionManagementService<SubscriptionManagementService> subscriptionManagementService,
            ISolutionManagementService<SolutionManagementService> solutionManagementService,
            IContentService<ContentService> contentService)
        {
            _subscriptionManagementService = subscriptionManagementService;
            _solutionManagementService = solutionManagementService;
            _contentService = contentService;
        }
        public ActionResult Index()
        {
            var subscription = _subscriptionManagementService.GetSubscriptions();
            ViewBag.Subs = subscription;
            var solution = _solutionManagementService.GetSolutions();
            ViewBag.Sol = solution;
            var intro = _contentService.GetIntroduction();
            ViewBag.Intro = intro;
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