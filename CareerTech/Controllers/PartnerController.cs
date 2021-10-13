using CareerTech.Models;
using CareerTech.Services;
using CareerTech.Services.Implement;
using CareerTech.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CareerTech.Controllers
{
    [Authorize(Roles = "Partner")]
    public class PartnerController : BaseController
    {

        private readonly IPartnerService<PartnerService> partnerService = null;

        public PartnerController(IPartnerService<PartnerService> partnerService)
        {
            this.partnerService = partnerService;
        }

        // GET Dashboard page of Partner 
        public ActionResult Index()
        {
            CompanyProfile company = partnerService.GetCompanyProfileByPartnerId(Session[SessionConstant.USER_ID].ToString());
            if (company != null)
            {
                Session[SessionConstant.COMPANY_ID] = company.ID;
            }
            return View();
        }


        /// <summary>
        /// About profile parner
        /// </summary>
        /// 
        #region Profile partner
        public ActionResult PartnerProfile()
        {
            ApplicationUser partner = Session[SessionConstant.USER_MODEL] as ApplicationUser;
            return View(partner);
        }

        [HttpPost]
        public ActionResult UpdateProfile(FormCollection data)
        {
            string fullname = data["fullname"];
            string phone = data["phone"];
            string email = data["email"];
            ApplicationUser partner = Session[SessionConstant.USER_MODEL] as ApplicationUser;
            string message;
            string type;
            if (string.IsNullOrEmpty(fullname)
                || string.IsNullOrEmpty(phone)
                || string.IsNullOrEmpty(email))
            {
                message = MessageConstant.DATA_NOT_EMPTY;
                type = CommonConstants.DANGER;
            }
            else if (!partnerService.CheckEmailExist(email, Session[SessionConstant.USER_ID].ToString()))
            {
                message = MessageConstant.EMAIL_EXIST;
                type = CommonConstants.DANGER;
            }
            else
            {
                partner.FullName = fullname;
                partner.Email = email;
                partner.PhoneNumber = phone;
                //Update profile
                partnerService.UpdatePartnerProfile(partner);
                //Update information in session
                Session[SessionConstant.USER_MODEL] = partner;

                message = MessageConstant.UPDATE_SUCCESS;
                type = CommonConstants.SUCCESS;
            }
            ViewBag.message = message;
            ViewBag.type = type;
            return View("PartnerProfile", partner);
        }

        #endregion 



        /// <summary>
        /// About company
        /// </summary>
        #region Company
        // GET CompanyProfile Page 
        [HttpGet]
        public ActionResult CompanyProfile()
        {
            CompanyProfile company = partnerService.GetCompanyProfileByPartnerId(Session[SessionConstant.USER_ID].ToString());
            if (company != null)
            {
                CompanyProfileViewModel companyProfileView = new CompanyProfileViewModel()
                {
                    CompanyName = company.CompanyName,
                    Address = company.Address,
                    Desc = company.Desc,
                    Url_Avatar = company.Url_Avatar,
                    Url_Background = company.Url_Background,
                    Phone = company.Phone,
                    Email = company.Email
                };
                return View(companyProfileView);
            }
            return RedirectToAction("Index", "Partner");
        }

        [HttpGet]
        public ActionResult CreateCompany()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> EditCompany(HttpPostedFileBase imgAvatar, HttpPostedFileBase imgCover, CompanyProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                CompanyProfile company = partnerService.GetCompanyProfileByPartnerId(Session[SessionConstant.USER_ID].ToString());
                string urlAvatar = await GetUrlImageByFileBase(imgAvatar, 200, 200);
                string urlCover = await GetUrlImageByFileBase(imgCover, 281, 783);
                company.CompanyName = model.CompanyName;
                company.Address = model.Address;
                company.Desc = model.Desc;
                company.Phone = model.Phone;
                company.Email = model.Email;
                if (!string.IsNullOrEmpty(urlAvatar))
                {
                    company.Url_Avatar = urlAvatar;
                }
                if (!string.IsNullOrEmpty(urlCover))
                {
                    company.Url_Background = urlCover;
                }
                //update company in DB 
                partnerService.UpdateCompany(company);
                ViewBag.message = MessageConstant.UPDATE_SUCCESS;
                ViewBag.type = CommonConstants.SUCCESS;
            }
            return View("CompanyProfile", model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> CreateCompanyProfile(HttpPostedFileBase imgAvatar, HttpPostedFileBase imgCover, CompanyProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                string Id = Guid.NewGuid().ToString();
                string userId = Session[SessionConstant.USER_ID].ToString();
                string urlAvatar = await GetUrlImageByFileBase(imgAvatar, 200, 200);
                string urlCover = await GetUrlImageByFileBase(imgCover, 281, 783);
                if (string.IsNullOrEmpty(urlAvatar))
                {
                    urlAvatar = CommonConstants.URL_AVATAR_DEFAULT;
                }
                if (string.IsNullOrEmpty(urlCover))
                {
                    urlCover = CommonConstants.URL_COVER_DEFAULT;
                }
                CompanyProfile company = new CompanyProfile()
                {
                    ID = Id,
                    UserID = userId,
                    CompanyName = model.CompanyName,
                    Address = model.Address,
                    Desc = model.Desc,
                    Url_Avatar = urlAvatar,
                    Url_Background = urlCover,
                    Phone = model.Phone,
                    Email = model.Email
                };
                //add to DB 
                partnerService.CreateProfileCompany(company);
                ViewBag.message = MessageConstant.CREATE_SUCCESS;
                ViewBag.type = CommonConstants.SUCCESS;
                return View("CompanyProfile", model);
            }
            else
            {
                return View("CreateCompany", model);
            }
        }

        // get url image with value configuration
        private async Task<string> GetUrlImageByFileBase(HttpPostedFileBase fileBase, int Height, int width)
        {
            if (fileBase != null && fileBase.ContentLength > 0)
            {
                string path = Path.Combine(Server.MapPath("~/UploadFiles"), Path.GetFileName(fileBase.FileName));
                fileBase.SaveAs(path);
                // Call API to get Url image
                string urlImage = await CloudDiaryService.CloudinaryUpload(path, Height, width);
                if (!string.IsNullOrEmpty(path))
                {
                    System.IO.File.Delete(path);
                }
                return urlImage;
            }
            return "";
        }


        // GET Introduction company  Page 
        public ActionResult IntroductionCompany(string id)
        {
            var company = partnerService.GetCompanyProfileById(id);
            return View(company);
        }

        #endregion
        /// <summary>
        /// About Recruitment
        /// </summary>
        #region Recruitment
        // GET Recruitment manage Page 
        public ActionResult RecruitmentManage()
        {
            ViewBag.ListJobCategory = new SelectList(partnerService.GetAddJobCategory(), "ID", "JobName");
            ViewBag.ListRecruitment = partnerService.GetAllRecruitment();
            return View();
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateRecruitment(RecruitmentViewModel model)
        {

            if (ModelState.IsValid)
            {
                Recruitment recruitment = new Recruitment()
                {
                    ID = Guid.NewGuid().ToString(),
                    CompanyProfileID = Session[SessionConstant.COMPANY_ID].ToString(),
                    JobID = model.JobID,
                    Title = model.Title,
                    Address = model.Address,
                    Salary = model.Salary,
                    Workingform = model.Workingform,
                    Amount = model.Amount,
                    Position = model.Position,
                    Experience = model.Experience,
                    Gender = model.Gender,
                    EndDate = model.EndDate,
                    DetailDesc = model.DetailDesc.ToString()

                };
                partnerService.AddRecruitment(recruitment);
                ViewBag.message = MessageConstant.CREATE_SUCCESS;
                ViewBag.type = CommonConstants.SUCCESS;

            }

            ViewBag.ListJobCategory = new SelectList(partnerService.GetAddJobCategory(), "ID", "JobName");
            ViewBag.ListRecruitment = partnerService.GetAllRecruitment();
            return View("RecruitmentManage", model);
        }



        [HttpGet]
        public ActionResult EditRecruitment(string id)
        {
            Recruitment recruitment = partnerService.GetRecruitmentById(id);
            RecruitmentViewModel model = new RecruitmentViewModel()
            {
                JobID = recruitment.JobID,
                Title = recruitment.Title,
                Address = recruitment.Address,
                Salary = recruitment.Salary,
                Workingform = recruitment.Workingform,
                Amount = recruitment.Amount,
                Position = recruitment.Position,
                Experience = recruitment.Experience,
                Gender = recruitment.Gender,
                EndDate = recruitment.EndDate,
                DetailDesc = recruitment.DetailDesc.ToString()

            };
            ViewBag.ListJobCategory = new SelectList(partnerService.GetAddJobCategory(), "ID", "JobName");
            ViewBag.ListRecruitment = partnerService.GetAllRecruitment();
            ViewBag.recruitmentId = recruitment.ID;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditRecruitment(RecruitmentViewModel model, string recruitmentId)
        {
            if (ModelState.IsValid)
            {
                Recruitment recruitment = partnerService.GetRecruitmentById(recruitmentId);
                recruitment.JobID = model.JobID;
                recruitment.Title = model.Title;
                recruitment.Address = model.Address;
                recruitment.Salary = model.Salary;
                recruitment.Workingform = model.Workingform;
                recruitment.Amount = model.Amount;
                recruitment.Position = model.Position;
                recruitment.Experience = model.Experience;
                recruitment.Gender = model.Gender;
                recruitment.EndDate = model.EndDate;
                recruitment.DetailDesc = model.DetailDesc.ToString();
                partnerService.UpdateRecruitment(recruitment);
                ViewBag.message = MessageConstant.UPDATE_SUCCESS;
                ViewBag.type = CommonConstants.SUCCESS;
            }
            ViewBag.ListJobCategory = new SelectList(partnerService.GetAddJobCategory(), "ID", "JobName");
            ViewBag.ListRecruitment = partnerService.GetAllRecruitment();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DeleteRecruitment(string recruitmentId)
        {
            if (recruitmentId != null)
            {
                partnerService.DeleteRecruitmentByID(recruitmentId);
            }
            //ViewBag.ListJobCategory = new SelectList(partnerService.GetAddJobCategory(), "ID", "JobName");
            //ViewBag.ListRecruitment = partnerService.GetAllRecruitment();
            //return View("RecruitmentManage");
            return RedirectToAction("RecruitmentManage", "Partner");
        }

        public ActionResult JobDetail(string id)
        {
            var recruitment = partnerService.GetRecruitmentById(id);
            if (string.IsNullOrEmpty(id)||recruitment==null)
            {
                ViewBag.message = "This Recruitment is not exist!";
                return Redirect("Error"); 
            }
            else
            {
                ViewBag.company = partnerService.GetCompanyProfileById(recruitment.CompanyProfileID);
                return View(recruitment);
            }
           
        }

        //[HttpGet]
        //public ActionResult GetRecruitment()
        //{
        //    var results = partnerService.GetAllRecruitment();
        //    return Json(new { Data = results, TotalItems = results.Count }, JsonRequestBehavior.AllowGet);
        //}



        #endregion


        #region Candidate
        public ActionResult CandidateManage()
        {

            return View();
        }

        public ActionResult SearchCandidate()
        {
            return View();
        }
        #endregion
    }
}