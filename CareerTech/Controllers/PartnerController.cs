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

    public class PartnerController : Controller
    {

        private readonly IPartnerService<PartnerService> _partnerService = null;
        private readonly IUserService<UserService> _userService = null;

        public PartnerController(IPartnerService<PartnerService> partnerService, IUserService<UserService> userService)
        {
            _partnerService = partnerService;
            _userService = userService;
        }

        [Authorize(Roles = "Partner")]
        // GET Dashboard page of Partner 
        public ActionResult Index()
        {
            CompanyProfile company = _partnerService.GetCompanyProfileByPartnerId(Session[SessionConstant.USER_ID].ToString());
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
        [Authorize(Roles = "Partner")]
        public ActionResult PartnerProfile()
        {
            ApplicationUser partner = Session[SessionConstant.USER_MODEL] as ApplicationUser;
            return View(partner);
        }

        [Authorize(Roles = "Partner")]
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
            else if (!_partnerService.CheckEmailExist(email, Session[SessionConstant.USER_ID].ToString()))
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
                _partnerService.UpdatePartnerProfile(partner);
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
        [Authorize(Roles = "Partner")]
        [HttpGet]
        public ActionResult CompanyProfile()
        {     
            ViewBag.ListAddress = new SelectList(CommonService.GetAddresses());
            CompanyProfile company = _partnerService.GetCompanyProfileByPartnerId(Session[SessionConstant.USER_ID].ToString());
            if (company != null)
            {
                if (!company.Status.Equals("Approved"))
                {
                    ViewBag.approveStatus = "The company is not Approved";
                }
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

        [Authorize(Roles = "Partner")]
        [HttpGet]
        public ActionResult CreateCompany()
        {
            ViewBag.ListAddress = new SelectList(CommonService.GetAddresses());
            return View();
        }

        [Authorize(Roles = "Partner")]
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> EditCompany(HttpPostedFileBase imgAvatar, HttpPostedFileBase imgCover, CompanyProfileViewModel model)
        {
            ViewBag.ListAddress = new SelectList(CommonService.GetAddresses());
            if (ModelState.IsValid)
            {
                CompanyProfile company = _partnerService.GetCompanyProfileByPartnerId(Session[SessionConstant.USER_ID].ToString());
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
                _partnerService.UpdateCompany(company);
                ViewBag.message = MessageConstant.UPDATE_SUCCESS;
                ViewBag.type = CommonConstants.SUCCESS;
            }
            return View("CompanyProfile", model);
        }

        [Authorize(Roles = "Partner")]
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> CreateCompanyProfile(HttpPostedFileBase imgAvatar, HttpPostedFileBase imgCover, CompanyProfileViewModel model)
        {
            ViewBag.ListAddress = new SelectList(CommonService.GetAddresses());
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
                    Email = model.Email,
                    Status = "Pending"
                };
                //add to DB 
                _partnerService.CreateProfileCompany(company);
                ViewBag.message = MessageConstant.CREATE_SUCCESS;
                ViewBag.type = CommonConstants.SUCCESS;
                Session[SessionConstant.COMPANY_ID] = company.ID;
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
        [AllowAnonymous]
        [HandleError]
        public ActionResult IntroductionCompany(string id)
        {
            var company = _partnerService.GetCompanyProfileById(id);
            return View(company);
        }

        #endregion
        /// <summary>
        /// About Recruitment
        /// </summary>
        #region Recruitment
        // GET Recruitment manage Page 
        [Authorize(Roles = "Partner")]
        public ActionResult RecruitmentManage()
        {
            if (Session[SessionConstant.COMPANY_ID] == null)
            {
                return RedirectToAction("CreateCompany", "Partner");
            }
            else
            {
                var company = _partnerService.GetCompanyProfileById(Session[SessionConstant.COMPANY_ID].ToString());
                if (!company.Status.Equals("Approved"))
                {
                    return RedirectToAction("CompanyProfile", "Partner");
                }

                ViewBag.ListAddress = new SelectList(CommonService.GetAddresses());
                ViewBag.ListJobCategory = new SelectList(_partnerService.GetAllJobCategory(), "ID", "JobName");
                ViewBag.ListRecruitment = _partnerService.GetListRecruitmentByCompanyID(Session[SessionConstant.COMPANY_ID].ToString());
                return View();
            }
        }


        [Authorize(Roles = "Partner")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateRecruitment(RecruitmentViewModel model)
        {

            if (Session[SessionConstant.COMPANY_ID] == null)
            {
                return RedirectToAction("CreateCompany", "Partner");
            }
            else
            {
                var company = _partnerService.GetCompanyProfileById(Session[SessionConstant.COMPANY_ID].ToString());
                if (!company.Status.Equals("Approved"))
                {
                    return RedirectToAction("CompanyProfile", "Partner");
                }
            }

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
                _partnerService.AddRecruitment(recruitment);
                ViewBag.message = MessageConstant.CREATE_SUCCESS;
                ViewBag.type = CommonConstants.SUCCESS;

            }
            ViewBag.ListAddress = new SelectList(CommonService.GetAddresses());
            ViewBag.ListJobCategory = new SelectList(_partnerService.GetAllJobCategory(), "ID", "JobName");
            ViewBag.ListRecruitment = _partnerService.GetListRecruitmentByCompanyID(Session[SessionConstant.COMPANY_ID].ToString());
            return View("RecruitmentManage", model);
        }


        [Authorize(Roles = "Partner")]
        [HttpGet]
        public ActionResult EditRecruitment(string id)
        {

            if (Session[SessionConstant.COMPANY_ID] == null)
            {
                return RedirectToAction("CreateCompany", "Partner");
            }
            else
            {
                var company = _partnerService.GetCompanyProfileById(Session[SessionConstant.COMPANY_ID].ToString());
                if (!company.Status.Equals("Approved"))
                {
                    return RedirectToAction("CompanyProfile", "Partner");
                }

            }
            Recruitment recruitment = _partnerService.GetRecruitmentById(id);
            RecruitmentViewModel model = new RecruitmentViewModel()
            {
                RecId = recruitment.ID,
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
            ViewBag.ListAddress = new SelectList(CommonService.GetAddresses());
            ViewBag.ListJobCategory = new SelectList(_partnerService.GetAllJobCategory(), "ID", "JobName");
            ViewBag.ListRecruitment = _partnerService.GetListRecruitmentByCompanyID(Session[SessionConstant.COMPANY_ID].ToString());
            return View(model);
        }

        [Authorize(Roles = "Partner")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditRecruitment(RecruitmentViewModel model, string recId)
        {
            if (ModelState.IsValid)
            {
                Recruitment recruitment = _partnerService.GetRecruitmentById(recId);
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
                _partnerService.UpdateRecruitment(recruitment);
                ViewBag.message = MessageConstant.UPDATE_SUCCESS;
                ViewBag.type = CommonConstants.SUCCESS;
            }
            ViewBag.ListAddress = new SelectList(CommonService.GetAddresses());
            ViewBag.ListJobCategory = new SelectList(_partnerService.GetAllJobCategory(), "ID", "JobName");
            //   ViewBag.ListRecruitment = partnerService.GetAllRecruitment();
            return View(model);
        }

        [Authorize(Roles = "Partner")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DeleteRecruitment(string recruitmentId)
        {
            if (recruitmentId != null)
            {
                _partnerService.DeleteRecruitmentByID(recruitmentId);
            }
            //ViewBag.ListJobCategory = new SelectList(partnerService.GetAddJobCategory(), "ID", "JobName");
            //ViewBag.ListRecruitment = partnerService.GetAllRecruitment();
            //return View("RecruitmentManage");
            return RedirectToAction("RecruitmentManage", "Partner");
        }

        [AllowAnonymous]
        [HandleError]
        public ActionResult JobDetail(string id, string message)
        {
            ViewBag.Message = message;
            var recruitment = _partnerService.GetRecruitmentById(id);
            var company = _partnerService.GetCompanyProfileById(recruitment.CompanyProfileID);
            if (!company.Status.Equals(CommonConstants.APPROVED_STATUS))
            {
                ViewBag.ErrorMessage = "The recruitment not found";
                return View("Error");
            }
            ViewBag.company = company;
            return View(recruitment);
        }
        #endregion



        #region Candidate
        [HttpGet]
        [Authorize(Roles = "Partner")]
        [HandleError]
        public ActionResult CandidateManage()
        {
            if (Session[SessionConstant.COMPANY_ID] == null)
            {
                return RedirectToAction("CreateCompany", "Partner");
            }
            else
            {
                var company = _partnerService.GetCompanyProfileById(Session[SessionConstant.COMPANY_ID].ToString());
                if (!company.Status.Equals("Approved"))
                {
                    return RedirectToAction("CompanyProfile", "Partner");
                }
            }
            string companyId = Session[SessionConstant.COMPANY_ID].ToString();
            List<CandidateViewModel> listCandidate = _partnerService.GetListCandidateByCompanyID(companyId);
            return View(listCandidate);
        }

        [HttpGet]
        [Authorize(Roles = "Partner")]
        [HandleError]
        public ActionResult SearchCandidate()
        {

            if (Session[SessionConstant.COMPANY_ID] == null)
            {
                return RedirectToAction("CreateCompany", "Partner");
            }
            else
            {
                var company = _partnerService.GetCompanyProfileById(Session[SessionConstant.COMPANY_ID].ToString());
                if (!company.Status.Equals("Approved"))
                {
                    return RedirectToAction("CompanyProfile", "Partner");
                }
            }
            ViewBag.ListAddress = new SelectList(CommonService.GetAddresses());
            ViewBag.ListJobCategory = new SelectList(_partnerService.GetAllJobCategory(), "ID", "JobName");
            return View();
        }

        [Authorize(Roles = "Partner")]
        [HandleError]
        [HttpPost]
        public ActionResult RejectCandidate(string id)
        {
            if (Session[SessionConstant.COMPANY_ID] == null)
            {
                return RedirectToAction("CreateCompany", "Partner");
            }
            else
            {
                var company = _partnerService.GetCompanyProfileById(Session[SessionConstant.COMPANY_ID].ToString());
                if (!company.Status.Equals("Approved"))
                {
                    return RedirectToAction("CompanyProfile", "Partner");
                }
            }
            _partnerService.DeleteCandidateById(id);
            string companyId = Session[SessionConstant.COMPANY_ID].ToString();
            List<CandidateViewModel> listCandidate = _partnerService.GetListCandidateByCompanyID(companyId);
            return View("CandidateManage", listCandidate);
        }


        [Authorize(Roles = "Partner")]
        [HandleError]
        [HttpPost]
        public ActionResult ApproveCandidate(string candidateID, string portfolioID, string toEmail, string recruitmentID)
        {
            if (Session[SessionConstant.COMPANY_ID] == null)
            {
                return RedirectToAction("CreateCompany", "Partner");
            }
         
            string companyId = Session[SessionConstant.COMPANY_ID].ToString();
            var profile = _userService.GetProfileByPortfolioID(portfolioID);
            var company = _partnerService.GetCompanyProfileById(companyId);
   
            if (!company.Status.Equals("Approved"))
            {
                return RedirectToAction("CompanyProfile", "Partner");
            }

            ApplicationUser partner = null;
            if (Session[SessionConstant.USER_MODEL] != null)
            {
                partner = Session[SessionConstant.USER_MODEL] as ApplicationUser;
            }

            string smtpUserName = "MockCareerTech@gmail.com";
            string smtpPassword = "mockproject@123";
            string smtpHost = "smtp.gmail.com";
            int smtpPort = 587;
            string emailTo = toEmail;

            string subject = "Bạn vừa nhận được liên hê từ CareerTech";
            string body = System.IO.File.ReadAllText(Server.MapPath("~/Content/EmailTemplate/Approve.html"));
            body = body.Replace("{{CandidateName}}", profile.Name);
            body = body.Replace("{{CompanyName}}", company.CompanyName);
            body = body.Replace("{{PartnerName}}", partner.FullName);
            body = body.Replace("{{Phone}}", partner.PhoneNumber);
            body = body.Replace("{{Email}}", partner.Email);
            body = body.Replace("{{RecruitmentID}}", recruitmentID);
            bool result = CommonService.Send(smtpUserName, smtpPassword, smtpHost, smtpPort, emailTo, subject, body);
            string message;
            if (result == true)
            {
                _partnerService.UpdateCandidateByID(candidateID);
                message = "Approve success";
            }
            else
            {
                message = "Candidate information was wrong!";
            }
            ViewBag.message = message;
            List<CandidateViewModel> listCandidate = _partnerService.GetListCandidateByCompanyID(companyId);
            return View("CandidateManage", listCandidate);
        }
        #endregion
    }
}