using CareerTech.Models;
using CareerTech.Services;
using CareerTech.Services.Implement;
using CareerTech.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CareerTech.Controllers
{
    [Authorize(Roles = "Partner")]
    public class PartnerController : Controller
    {

        private IPartnerService<PartnerService> partnerService = null;

        public PartnerController(IPartnerService<PartnerService> partnerService)
        {
            this.partnerService = partnerService;
        }

        // GET Dashboard page of Partner 
        public ActionResult Index()
        {
            return View();
        }

        // GET CompanyProfile Page 
        
        public ActionResult CompanyProfile()
        {
            return View();
        }


        // GET Recruitment Page 
        public ActionResult Recruitment()
        {
            return View();
        }

        public ActionResult PartnerProfile()
        {
           ApplicationUser partner = partnerService.GetPartnerByID(Session[SessionConstant.USER_ID].ToString());
            return View(partner);
        }

        [HttpPost]
        public ActionResult PartnerProfile(ProfileViewModels model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            return View();
        }


        public ActionResult CandidateManage()
        {
            return View();
        }

        public ActionResult SearchCandidate()
        {
            return View();
        }

    }
}