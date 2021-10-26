using CareerTech.Models;
using CareerTech.Services;
using CareerTech.Services.Implement;
using CareerTech.Utils;
using log4net;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CareerTech.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService<UserService> _userService;
        private readonly IPartnerService<PartnerService> _partnerService;
        private readonly IAccountService<AccountService> _accountService;
        private readonly ILog log = LogManager.GetLogger(typeof(UserController));

        public UserController(IUserService<UserService> userService, IPartnerService<PartnerService> partnerService, IAccountService<AccountService> accountService)
        {
            _userService = userService;
            _partnerService = partnerService;
            _accountService = accountService;
        }

        #region UserDashboard
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {
            try
            {
                log.Info(LogConstants.LOG_USER_DASHBOARD);
                ApplicationUser user = Session[SessionConstant.USER_MODEL] as ApplicationUser;
                ViewBag.NumberRecruitmentPending = _userService.CountCandidateByUserIDAndStatus(user.Id, CommonConstants.PENDING_STATUS);
                ViewBag.NumberRecruitmentApproved = _userService.CountCandidateByUserIDAndStatus(user.Id, CommonConstants.APPROVED_STATUS);
                ViewBag.ListRecruitmentApplied = _userService.GetDashboardRecruitmentByUserIDAndStatus(user.Id);
                return View();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }
        #endregion

        #region SearchRecruitment
        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult SearchRecruitment()
        {
            try
            {
                log.Info(LogConstants.LOG_SEARCH_RECRUITMENT);
                ViewBag.ListAddress = new SelectList(CommonService.GetAddresses());
                ViewBag.ListJob = new SelectList(_partnerService.GetAllJobCategory(), "ID", "JobName");
                ViewBag.ListRecruitment = _userService.SearchAllRecruiment();
                return View();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult SearchRecruitment(string address, string job)
        {
            try
            {
                log.Info(LogConstants.LOG_SEARCH_RECRUITMENT);
                ViewBag.ListAddress = new SelectList(CommonService.GetAddresses(), address);
                ViewBag.ListJob = new SelectList(_partnerService.GetAllJobCategory(), "ID", "JobName", job);
                ViewBag.ListRecruitment = _userService.SearchRecruimentByFilter(address, job);
                return View();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult ApplyRecruitment(string id)
        {
            try
            {
                log.Info(LogConstants.LOG_APPLY_RECRUITMENT);
                string message = "";
                ApplicationUser user = Session[SessionConstant.USER_MODEL] as ApplicationUser;
                if (_userService.GetMainPortfolioByUser(user.Id) == null)
                {
                    log.Info(LogConstants.LOG_NULL_MAIN_PORTFOLIO);
                    message = MessageConstant.NULL_MAIN_PORTFOLIO;
                }
                else
                {
                    _userService.GetCandidateByUserIDAndRecruitmentID(user.Id, id);
                    if (_userService.GetCandidateByUserIDAndRecruitmentID(user.Id, id) != null)
                    {
                        log.Info(LogConstants.LOG_APPLY_ALREADY);
                        message = MessageConstant.APPLY_ALREADY;
                    }
                    else
                    {
                        Candidate candidate = new Candidate();
                        candidate.ID = Guid.NewGuid().ToString();
                        candidate.RecruitmentID = id;
                        candidate.UserID = user.Id;
                        candidate.DateApply = DateTime.Now;
                        candidate.Status = CommonConstants.PENDING_STATUS;
                        int checkSuccess = _userService.ApplyRecruitment(candidate);
                        if (checkSuccess > 0)
                        {
                            log.Info(LogConstants.LOG_SUCCESS);
                            message = MessageConstant.APPLY_SUCCESS;
                        }
                        else
                        {
                            log.Info(LogConstants.LOG_FAIL);
                            message = MessageConstant.APPLY_FAIL;
                        }
                    }
                }
                return RedirectToAction("JobDetail", "Partner", new { id = id, message = message });
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }
        #endregion

        #region UserProfile
        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult UserProfile()
        {
            try
            {
                log.Info(LogConstants.LOG_USER_PROFILE);
                ApplicationUser user = Session[SessionConstant.USER_MODEL] as ApplicationUser;
                SelectList list = new SelectList(_userService.GetPublicPortfolioByUser(user.Id), "ID", "Name");
                Portfolio mainPortfolio = _userService.GetMainPortfolioByUser(user.Id);
                if (mainPortfolio != null)
                {
                    list = new SelectList(_userService.GetPublicPortfolioByUser(user.Id), "ID", "Name", mainPortfolio.ID);
                }
                ViewBag.ListPublicPortfolio = list;
                return View(user);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult EditUserProfile(ApplicationUser model, string mainPortfoio)
        {
            try
            {
                log.Info(LogConstants.LOG_EDIT_USER_PROFILE);
                ApplicationUser user = Session[SessionConstant.USER_MODEL] as ApplicationUser;
                int checkSuccess = _userService.EditUserProfile(user.Id, model);
                if (checkSuccess >= 0)
                {
                    if (_userService.EditMainStatus(mainPortfoio, user.Id) >= 0)
                    {
                        log.Info(LogConstants.LOG_SUCCESS);
                        ViewBag.Type = CommonConstants.SUCCESS;
                        ViewBag.Message = MessageConstant.UPDATE_SUCCESS;
                        Session[SessionConstant.USER_MODEL] = _accountService.GetUserByEmail(user.Email); ;
                    }
                    else
                    {
                        log.Info(LogConstants.LOG_FAIL);
                        ViewBag.Type = CommonConstants.DANGER;
                        ViewBag.Message = MessageConstant.UPDATE_FAIL;
                    }
                }
                else
                {
                    log.Info(LogConstants.LOG_FAIL);
                    ViewBag.Type = CommonConstants.DANGER;
                    ViewBag.Message = MessageConstant.UPDATE_FAIL;
                }
                SelectList list = new SelectList(_userService.GetPublicPortfolioByUser(user.Id), "ID", "Name");
                Portfolio mainPortfolio = _userService.GetMainPortfolioByUser(user.Id);
                if (mainPortfolio != null)
                {
                    list = new SelectList(_userService.GetPublicPortfolioByUser(user.Id), "ID", "Name", mainPortfolio.ID);
                }
                ViewBag.ListPublicPortfolio = list;
                return View("UserProfile");
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }
        #endregion

        #region PortfolioPage
        [HttpGet]
        public ActionResult Portfolio(string id)
        {
            try
            {
                log.Info(LogConstants.LOG_PORTFOLIO_PAGE);
                var user = Session[SessionConstant.USER_MODEL] as ApplicationUser;
                Portfolio portfolio = _userService.GetPortfolioByID(id);
                if (portfolio == null)
                {
                    log.Error(LogConstants.LOG_PORTFOLIO_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                    return View("Error");
                }
                if (portfolio.PublicStatus == false && (user == null || user.Id != portfolio.UserID))
                {
                    log.Error(LogConstants.LOG_PORTFOLIO_NOT_PUBLIC);
                    ViewBag.ErrorMessage = MessageConstant.NOT_PUBLIC_PORTFOLIO;
                    return View("Error");
                }
                Profile profile = _userService.GetProfileByPortfolioID(id);
                if (profile == null)
                {
                    log.Error(LogConstants.LOG_NULL_PROFILE_PORTFOLIO);
                    //ViewBag.ErrorMessage = MessageConstant.NULL_PROFILE_PORTFOLIO;
                    //return View("Error");
                    return RedirectToAction("EditProfilePortfolio","User", new { id = id});
                }
                ViewBag.Profile = _userService.GetProfileByPortfolioID(id);
                ViewBag.Skill = _userService.GetSkillByPortfolioID(id);
                ViewBag.Education = _userService.GetEducationByPortfolioID(id);
                ViewBag.Experience = _userService.GetExperienceByPortfolioID(id);
                ViewBag.Product = _userService.GetProductByPortfolioID(id);
                return View();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        public ActionResult DownloadPortfolio(string id)
        {
            log.Info(LogConstants.LOG_DOWNLOAD_PORTFOLIO);
            var portfolio = _userService.GetPortfolioByID(id);
            var PDF = IronPdf.ChromePdfRenderer.StaticRenderUrlAsPdf(new Uri("https://localhost:44396/User/Portfolio/" + id));
            return File(PDF.BinaryData, CommonConstants.PDF_TYPE, portfolio.Name + ".pdf");
        }
        #endregion

        #region ImageProcess
        private async Task<string> GetUrlImageByFileBase(HttpPostedFileBase fileBase, int Height, int width)
        {
            log.Info(LogConstants.LOG_GET_IMAGE_URL);
            string urlImage = "";
            if (fileBase != null && fileBase.ContentLength > 0)
            {
                string path = Path.Combine(Server.MapPath("~/UploadFiles"), Path.GetFileName(fileBase.FileName));
                fileBase.SaveAs(path);
                // Call API to get Url image
                urlImage = await CloudDiaryService.CloudinaryUpload(path, Height, width);
                if (!string.IsNullOrEmpty(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            log.Info(urlImage);
            return urlImage;
        }
        #endregion

        #region PortfolioManagement
        // GET: User
        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult LoadPortfolio()
        {
            try
            {
                log.Info(LogConstants.LOG_PORTFOLIO_MANAGER);
                var user = Session[SessionConstant.USER_MODEL] as ApplicationUser;
                ViewBag.listPortfolio = _userService.GetPortfolioByUser(user.Id);
                return View("PortfolioManager");
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult CreatePortfolio(string portfolioName)
        {
            try
            {
                log.Info(LogConstants.LOG_CREATE_PORTFOLIO);
                var user = Session[SessionConstant.USER_MODEL] as ApplicationUser;
                if (_userService.GetPortfolioByNameAndUser(portfolioName, user.Id).Count > 0)
                {
                    log.Error(LogConstants.LOG_DUPLICATE_NAME);
                    ViewBag.Type = CommonConstants.DANGER;
                    ViewBag.Message = MessageConstant.DUPLICATE_NAME;
                }
                else
                {
                    Portfolio portfolio = new Portfolio();
                    portfolio.ID = Guid.NewGuid().ToString();
                    portfolio.UserID = user.Id;
                    portfolio.Name = portfolioName;
                    portfolio.PublicStatus = false;
                    portfolio.MainStatus = false;
                    portfolio.Url_Domain = "/portfolio/" + portfolio.ID;
                    int checkSuccess = _userService.InsertPortfolio(portfolio);
                    if (checkSuccess > 0)
                    {
                        log.Info(LogConstants.LOG_SUCCESS);
                        ViewBag.Type = CommonConstants.SUCCESS;
                        ViewBag.Message = MessageConstant.CREATE_SUCCESS;
                    }
                    else
                    {
                        log.Info(LogConstants.LOG_FAIL);
                        ViewBag.Type = CommonConstants.DANGER;
                        ViewBag.Message = MessageConstant.CREATE_FAIL;
                    }
                }
                ViewBag.listPortfolio = _userService.GetPortfolioByUser(user.Id);
                return View("PortfolioManager");
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public JsonResult ChangeStatus(string id)
        {
            log.Info(LogConstants.LOG_CHANGE_PUBLIC_STATUS);
            _userService.ChangePortfolioStatus(id);
            return Json(new
            {
                status = _userService.GetPortfolioByID(id).PublicStatus
            });
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult DeletePortfolio(string id)
        {
            try
            {
                log.Info(LogConstants.LOG_DELETE_PORTFOLIO);
                if (_userService.GetPortfolioByID(id) == null)
                {
                    log.Error(LogConstants.LOG_PORTFOLIO_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                    return View("Error");
                }
                int checkSuccess = _userService.DeletePortfolio(id);
                if (checkSuccess > 0)
                {
                    log.Info(LogConstants.LOG_SUCCESS);
                    ViewBag.Type = CommonConstants.SUCCESS;
                    ViewBag.Message = MessageConstant.DELETE_SUCCESS;
                }
                else
                {
                    log.Info(LogConstants.LOG_FAIL);
                    ViewBag.Type = CommonConstants.DANGER;
                    ViewBag.Message = MessageConstant.DELETE_FAIL;
                }
                var user = Session[SessionConstant.USER_MODEL] as ApplicationUser;
                ViewBag.listPortfolio = _userService.GetPortfolioByUser(user.Id);
                return View("PortfolioManager");
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }
        #endregion

        #region ProfileManagement
        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult EditProfilePortfolio(string id)
        {
            try
            {
                log.Info(LogConstants.LOG_PROFILE_PORTFOLIO);
                if (_userService.GetPortfolioByID(id) == null)
                {
                    log.Error(LogConstants.LOG_PORTFOLIO_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                    return View("Error");
                }
                ViewBag.ListAddress = new SelectList(CommonService.GetAddresses());
                ViewBag.Portfolio = _userService.GetPortfolioByID(id);
                Profile profile = _userService.GetProfileByPortfolioID(id);
                ViewBag.CurrentPage = PageConstant.PROFILE_PORTFOLIO;
                return View(profile);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> EditProfilePortfolioAsync(Profile model, HttpPostedFileBase url_avatar)
        {
            try
            {
                var profile = _userService.GetProfileByPortfolioID(model.PortfolioID);
                if (url_avatar != null)
                {
                    model.Url_avatar = await GetUrlImageByFileBase(url_avatar, 350, 280);
                }
                if (ModelState.IsValid)
                {
                    if (profile == null)
                    {
                        int checkSuccess = _userService.CreateProfile(model);
                        if (checkSuccess > 0)
                        {
                            log.Info(LogConstants.LOG_SUCCESS);
                            ViewBag.Type = CommonConstants.SUCCESS;
                            ViewBag.Message = MessageConstant.CREATE_SUCCESS;
                        }
                        else
                        {
                            log.Info(LogConstants.LOG_FAIL);
                            ViewBag.Type = CommonConstants.DANGER;
                            ViewBag.Message = MessageConstant.CREATE_FAIL;
                        }
                    }
                    else
                    {
                        int checkSuccess = _userService.EditProfile(model);
                        if (checkSuccess >= 0)
                        {
                            log.Info(LogConstants.LOG_SUCCESS);
                            ViewBag.Type = CommonConstants.SUCCESS;
                            ViewBag.Message = MessageConstant.UPDATE_SUCCESS;
                        }
                        else
                        {
                            log.Info(LogConstants.LOG_FAIL);
                            ViewBag.Type = CommonConstants.DANGER;
                            ViewBag.Message = MessageConstant.UPDATE_FAIL;
                        }
                    }
                }
                ViewBag.ListAddress = new SelectList(CommonService.GetAddresses());
                ViewBag.Portfolio = _userService.GetPortfolioByID(model.PortfolioID); ;
                ViewBag.CurrentPage = PageConstant.PROFILE_PORTFOLIO;
                return View("EditProfilePortfolio", model);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }
        #endregion

        #region SkillManagement
        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult LoadSkill(string id)
        {
            try
            {
                log.Info(LogConstants.LOG_SKILL_MANAGER);
                if (_userService.GetPortfolioByID(id) == null)
                {
                    log.Error(LogConstants.LOG_PORTFOLIO_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                    return View("Error");
                }
                ViewBag.Portfolio = _userService.GetPortfolioByID(id);
                ViewBag.ListSkill = _userService.GetSkillByPortfolioID(id);
                ViewBag.CurrentPage = PageConstant.SKILL_PORTFOLIO;
                return View("SkillManager");
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult AddSkill(Skill model)
        {
            try
            {
                log.Info(LogConstants.LOG_ADD_SKILL);
                if (_userService.GetPortfolioByID(model.PortfolioID) == null)
                {
                    log.Error(LogConstants.LOG_PORTFOLIO_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                    return View("Error");
                }
                if (ModelState.IsValid)
                {
                    if (_userService.GetSkillByNameAndPortfolioID(model.SkillName, model.PortfolioID) != null)
                    {
                        log.Error(LogConstants.LOG_DUPLICATE_NAME);
                        ViewBag.Type = CommonConstants.DANGER;
                        ViewBag.Message = MessageConstant.DUPLICATE_NAME;
                    }
                    else
                    {
                        int checkSuccess = _userService.AddSkill(model);
                        if (checkSuccess > 0)
                        {
                            log.Info(LogConstants.LOG_SUCCESS);
                            ViewBag.Type = CommonConstants.SUCCESS;
                            ViewBag.Message = MessageConstant.CREATE_SUCCESS;
                        }
                        else
                        {
                            log.Info(LogConstants.LOG_FAIL);
                            ViewBag.Type = CommonConstants.DANGER;
                            ViewBag.Message = MessageConstant.CREATE_FAIL;
                        }
                    }
                }
                ViewBag.Portfolio = _userService.GetPortfolioByID(model.PortfolioID);
                ViewBag.ListSkill = _userService.GetSkillByPortfolioID(model.PortfolioID);
                ViewBag.CurrentPage = PageConstant.SKILL_PORTFOLIO;
                return View("SkillManager");
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult EditSkill(string skillID)
        {
            try
            {
                log.Info(LogConstants.LOG_EDIT_SKILL);
                Skill skill = _userService.GetSkillByID(skillID);
                if (skill == null)
                {
                    log.Error(LogConstants.LOG_SKILL_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_SKILL;
                    return View("Error");
                }
                ViewBag.Portfolio = _userService.GetPortfolioByID(skill.PortfolioID);
                ViewBag.CurrentPage = PageConstant.SKILL_PORTFOLIO;
                return View(skill);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult EditSkill(string skillID, Skill model)
        {
            try
            {
                var skill = _userService.GetSkillByID(skillID);
                if (skill == null)
                {
                    log.Error(LogConstants.LOG_SKILL_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_SKILL;
                    return View("Error");
                }
                else
                {
                    ViewBag.Portfolio = _userService.GetPortfolioByID(model.PortfolioID);
                    ViewBag.CurrentPage = PageConstant.SKILL_PORTFOLIO;
                    model.ID = skillID;
                    if (ModelState.IsValid)
                    {
                        if (_userService.GetSkillByNameAndPortfolioID(model.SkillName, model.PortfolioID) != null && model.SkillName != _userService.GetSkillByID(skillID).SkillName)
                        {
                            log.Error(LogConstants.LOG_DUPLICATE_NAME);
                            ViewBag.Type = CommonConstants.DANGER;
                            ViewBag.Message = MessageConstant.DUPLICATE_NAME;
                        }
                        else
                        {
                            int checkSuccess = _userService.EditSkill(skillID, model);
                            if (checkSuccess >= 0)
                            {
                                log.Info(LogConstants.LOG_SUCCESS);
                                ViewBag.Type = CommonConstants.SUCCESS;
                                ViewBag.Message = MessageConstant.UPDATE_SUCCESS;
                                ViewBag.ListSkill = _userService.GetSkillByPortfolioID(model.PortfolioID);
                                return View("SkillManager");
                            }
                            else
                            {
                                log.Info(LogConstants.LOG_FAIL);
                                ViewBag.Type = CommonConstants.DANGER;
                                ViewBag.Message = MessageConstant.UPDATE_FAIL;
                            }
                        }
                    }
                    return View("EditSkill", model);
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult DeleteSkill(string skillID)
        {
            try
            {
                log.Info(LogConstants.LOG_DELETE_SKILL);
                var skill = _userService.GetSkillByID(skillID);
                string portfolioID = skill.PortfolioID;
                if (skill == null)
                {
                    log.Error(LogConstants.LOG_SKILL_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_SKILL;
                    return View("Error");
                }
                else
                {
                    int checkSuccess = _userService.DeleteSkill(skillID);
                    if (checkSuccess > 0)
                    {
                        log.Info(LogConstants.LOG_SUCCESS);
                        ViewBag.Type = CommonConstants.SUCCESS;
                        ViewBag.Message = MessageConstant.DELETE_SUCCESS;
                    }
                    else
                    {
                        log.Info(LogConstants.LOG_FAIL);
                        ViewBag.Type = CommonConstants.DANGER;
                        ViewBag.Message = MessageConstant.DELETE_FAIL;
                    }
                    ViewBag.Portfolio = _userService.GetPortfolioByID(portfolioID);
                    ViewBag.CurrentPage = PageConstant.SKILL_PORTFOLIO;
                    ViewBag.ListSkill = _userService.GetSkillByPortfolioID(portfolioID);
                    return View("SkillManager");
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }
        #endregion

        #region EducationManagement
        [HttpGet]
        //[Authorize(Roles = "User")]
        public ActionResult LoadEducation(string id)
        {
            try
            {
                log.Info(LogConstants.LOG_EDUCATION_MANAGER);
                if (_userService.GetPortfolioByID(id) == null)
                {
                    log.Error(LogConstants.LOG_PORTFOLIO_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                    return View("Error");
                }
                ViewBag.Portfolio = _userService.GetPortfolioByID(id);
                ViewBag.CurrentPage = PageConstant.EDUCATION_PORTFOLIO;
                ViewBag.ListEducation = _userService.GetEducationByPortfolioID(id);
                return View("EducationManager");
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateInput(false)]
        public ActionResult AddEducation(Education model)
        {
            try
            {
                log.Info(LogConstants.LOG_ADD_EDUCATION);
                if (_userService.GetPortfolioByID(model.PortfolioID) == null)
                {
                    log.Error(LogConstants.LOG_PORTFOLIO_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                    return View("Error");
                }
                int checkSuccess = _userService.AddEducation(model);
                if (checkSuccess > 0)
                {
                    log.Info(LogConstants.LOG_SUCCESS);
                    ViewBag.Type = CommonConstants.SUCCESS;
                    ViewBag.Message = MessageConstant.CREATE_SUCCESS;
                }
                else
                {
                    log.Info(LogConstants.LOG_FAIL);
                    ViewBag.Type = CommonConstants.DANGER;
                    ViewBag.Message = MessageConstant.CREATE_FAIL;
                }
                ViewBag.Portfolio = _userService.GetPortfolioByID(model.PortfolioID);
                ViewBag.ListEducation = _userService.GetEducationByPortfolioID(model.PortfolioID);
                ViewBag.CurrentPage = PageConstant.EDUCATION_PORTFOLIO;
                return View("EducationManager");
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult EditEducation(string educationID)
        {
            try
            {
                log.Info(LogConstants.LOG_EDIT_EDUCATION);
                Education education = _userService.GetEducationByID(educationID);
                if (education == null)
                {
                    log.Error(LogConstants.LOG_EDUCATION_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_EDUCATION;
                    return View("Error");
                }
                ViewBag.Education = education;
                ViewBag.Portfolio = _userService.GetPortfolioByID(education.PortfolioID);
                ViewBag.CurrentPage = PageConstant.EDUCATION_PORTFOLIO;
                return View();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateInput(false)]
        public ActionResult EditEducation(string educationID, Education model, FormCollection collection)
        {
            try
            {
                var education = _userService.GetEducationByID(educationID);
                if (education == null)
                {
                    log.Error(LogConstants.LOG_EDUCATION_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_EDUCATION;
                    return View("Error");
                }
                else
                {
                    ViewBag.Portfolio = _userService.GetPortfolioByID(model.PortfolioID);
                    ViewBag.CurrentPage = PageConstant.EDUCATION_PORTFOLIO;
                    string detail = collection["Detail"];
                    model.Detail = detail;
                    int checkSuccess = _userService.EditEducation(educationID, model);
                    if (checkSuccess >= 0)
                    {
                        log.Info(LogConstants.LOG_SUCCESS);
                        ViewBag.Type = CommonConstants.SUCCESS;
                        ViewBag.Message = MessageConstant.UPDATE_SUCCESS;
                    }
                    else
                    {
                        log.Info(LogConstants.LOG_FAIL);
                        ViewBag.Type = CommonConstants.DANGER;
                        ViewBag.Message = MessageConstant.UPDATE_FAIL;
                        ViewBag.Education = education;
                        return View("EditEducation");
                    }
                    ViewBag.ListEducation = _userService.GetEducationByPortfolioID(model.PortfolioID);
                    return View("EducationManager");
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult DeleteEducation(string educationID)
        {
            try
            {
                log.Info(LogConstants.LOG_DELETE_EDUCATION);
                var education = _userService.GetEducationByID(educationID);
                if (education == null)
                {
                    log.Error(LogConstants.LOG_EDUCATION_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_EDUCATION;
                    return View("Error");
                }
                else
                {
                    string portfolioID = education.PortfolioID;
                    ViewBag.Portfolio = _userService.GetPortfolioByID(portfolioID);
                    ViewBag.CurrentPage = PageConstant.EDUCATION_PORTFOLIO;
                    int checkSuccess = _userService.DeleteEducation(educationID);
                    if (checkSuccess > 0)
                    {
                        log.Info(LogConstants.LOG_SUCCESS);
                        ViewBag.Type = CommonConstants.SUCCESS;
                        ViewBag.Message = MessageConstant.DELETE_SUCCESS;
                    }
                    else
                    {
                        log.Info(LogConstants.LOG_FAIL);
                        ViewBag.Type = CommonConstants.DANGER;
                        ViewBag.Message = MessageConstant.DELETE_FAIL;
                    }
                    ViewBag.ListEducation = _userService.GetEducationByPortfolioID(portfolioID);
                    return View("EducationManager");
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }
        #endregion

        #region ExperienceManagement
        [HttpGet]
        //[Authorize(Roles = "User")]
        public ActionResult LoadExperience(string id)
        {
            try
            {
                log.Info(LogConstants.LOG_EXPERIENCE_MANAGER);
                if (_userService.GetPortfolioByID(id) == null)
                {
                    log.Error(LogConstants.LOG_PORTFOLIO_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                    return View("Error");
                }
                ViewBag.Portfolio = _userService.GetPortfolioByID(id);
                ViewBag.CurrentPage = PageConstant.EXPERIENCE_PORTFOLIO;
                ViewBag.ListExperience = _userService.GetExperienceByPortfolioID(id);
                return View("ExperienceManager");
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateInput(false)]
        public ActionResult AddExperience(Experience model)
        {
            try
            {
                log.Info(LogConstants.LOG_ADD_EXPERIENCE);
                if (_userService.GetPortfolioByID(model.PortfolioID) == null)
                {
                    log.Error(LogConstants.LOG_PORTFOLIO_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                    return View("Error");
                }
                int checkSuccess = _userService.AddExperience(model);
                if (checkSuccess > 0)
                {
                    log.Info(LogConstants.LOG_SUCCESS);
                    ViewBag.Type = CommonConstants.SUCCESS;
                    ViewBag.Message = MessageConstant.CREATE_SUCCESS;
                }
                else
                {
                    log.Info(LogConstants.LOG_FAIL);
                    ViewBag.Type = CommonConstants.DANGER;
                    ViewBag.Message = MessageConstant.CREATE_FAIL;
                }
                ViewBag.Portfolio = _userService.GetPortfolioByID(model.PortfolioID);
                ViewBag.ListExperience = _userService.GetExperienceByPortfolioID(model.PortfolioID);
                ViewBag.CurrentPage = PageConstant.EXPERIENCE_PORTFOLIO;
                return View("ExperienceManager");
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult EditExperience(string experienceID)
        {
            try
            {
                log.Info(LogConstants.LOG_EDIT_EXPERIENCE);
                Experience experience = _userService.GetExperienceByID(experienceID);
                if (experience == null)
                {
                    log.Error(LogConstants.LOG_EXPERIENCE_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_EXPERIENCE;
                    return View("Error");
                }
                ViewBag.Experience = experience;
                ViewBag.Portfolio = _userService.GetPortfolioByID(experience.PortfolioID);
                ViewBag.CurrentPage = PageConstant.EXPERIENCE_PORTFOLIO;
                return View();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateInput(false)]
        public ActionResult EditExperience(string experienceID, Experience model, FormCollection collection)
        {
            try
            {
                var experience = _userService.GetExperienceByID(experienceID);
                if (experience == null)
                {
                    log.Error(LogConstants.LOG_EXPERIENCE_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_EXPERIENCE;
                    return View("Error");
                }
                else
                {
                    ViewBag.Portfolio = _userService.GetPortfolioByID(model.PortfolioID);
                    ViewBag.CurrentPage = PageConstant.EDUCATION_PORTFOLIO;
                    string detail = collection["Detail"];
                    model.Detail = detail;
                    int checkSuccess = _userService.EditExperience(experienceID, model);
                    if (checkSuccess >= 0)
                    {
                        log.Info(LogConstants.LOG_SUCCESS);
                        ViewBag.Type = CommonConstants.SUCCESS;
                        ViewBag.Message = MessageConstant.UPDATE_SUCCESS;
                    }
                    else
                    {
                        log.Info(LogConstants.LOG_FAIL);
                        ViewBag.Type = CommonConstants.DANGER;
                        ViewBag.Message = MessageConstant.UPDATE_FAIL;
                        ViewBag.Experience = experience;
                        return View("EditExperience");
                    }
                    ViewBag.ListExperience = _userService.GetExperienceByPortfolioID(model.PortfolioID);
                    return View("ExperienceManager");
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult DeleteExperience(string experienceID)
        {
            try
            {
                log.Info(LogConstants.LOG_DELETE_EXPERIENCE);
                var experience = _userService.GetExperienceByID(experienceID);
                if (experience == null)
                {
                    log.Error(LogConstants.LOG_EXPERIENCE_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_EXPERIENCE;
                    return View("Error");
                }
                else
                {
                    string portfolioID = experience.PortfolioID;
                    ViewBag.Portfolio = _userService.GetPortfolioByID(portfolioID);
                    ViewBag.CurrentPage = PageConstant.EDUCATION_PORTFOLIO;
                    int checkSuccess = _userService.DeleteExperience(experienceID);
                    if (checkSuccess > 0)
                    {
                        log.Info(LogConstants.LOG_SUCCESS);
                        ViewBag.Type = CommonConstants.SUCCESS;
                        ViewBag.Message = MessageConstant.DELETE_SUCCESS;
                    }
                    else
                    {
                        log.Info(LogConstants.LOG_FAIL);
                        ViewBag.Type = CommonConstants.DANGER;
                        ViewBag.Message = MessageConstant.DELETE_FAIL;
                    }
                    ViewBag.ListExperience = _userService.GetExperienceByPortfolioID(portfolioID);
                    return View("ExperienceManager");
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }
        #endregion

        #region ProductManagement
        [HttpGet]
        //[Authorize(Roles = "User")]
        public ActionResult LoadProduct(string id)
        {
            try
            {
                log.Info(LogConstants.LOG_PRODUCT_MANAGER);
                if (_userService.GetPortfolioByID(id) == null)
                {
                    log.Error(LogConstants.LOG_PORTFOLIO_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                    return View("Error");
                }
                ViewBag.Portfolio = _userService.GetPortfolioByID(id);
                ViewBag.CurrentPage = PageConstant.PRODUCT_PORTFOLIO;
                ViewBag.ListProduct = _userService.GetProductByPortfolioID(id);
                return View("ProductManager");
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> AddProductAsync(Product model, HttpPostedFileBase url_image)
        {
            try
            {
                log.Info(LogConstants.LOG_ADD_PRODUCT);
                if (_userService.GetPortfolioByID(model.PortfolioID) == null)
                {
                    log.Error(LogConstants.LOG_PORTFOLIO_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                    return View("Error");
                }
                if (url_image != null)
                {
                    model.Url_Image = await GetUrlImageByFileBase(url_image, 253, 338);
                }
                if (ModelState.IsValid)
                {
                    if (_userService.GetProductByNameAndPortfolioID(model.Name, model.PortfolioID) != null)
                    {
                        log.Info(LogConstants.LOG_DUPLICATE_NAME);
                        ViewBag.Type = CommonConstants.DANGER;
                        ViewBag.Message = MessageConstant.DUPLICATE_NAME;
                    }
                    else
                    {
                        int checkSuccess = _userService.AddProduct(model);
                        if (checkSuccess > 0)
                        {
                            log.Info(LogConstants.LOG_SUCCESS);
                            ViewBag.Type = CommonConstants.SUCCESS;
                            ViewBag.Message = MessageConstant.CREATE_SUCCESS;
                        }
                        else
                        {
                            log.Info(LogConstants.LOG_FAIL);
                            ViewBag.Type = CommonConstants.DANGER;
                            ViewBag.Message = MessageConstant.CREATE_FAIL;
                        }
                    }
                }
                ViewBag.Portfolio = _userService.GetPortfolioByID(model.PortfolioID);
                ViewBag.ListProduct = _userService.GetProductByPortfolioID(model.PortfolioID);
                ViewBag.CurrentPage = PageConstant.PRODUCT_PORTFOLIO;
                return View("ProductManager");
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult EditProduct(string productID)
        {
            try
            {
                log.Info(LogConstants.LOG_EDIT_PRODUCT);
                Product product = _userService.GetProductByID(productID);
                if (product == null)
                {
                    log.Error(LogConstants.LOG_PRODUCT_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PRODUCT;
                    return View("Error");
                }
                ViewBag.Product = product;
                ViewBag.Portfolio = _userService.GetPortfolioByID(product.PortfolioID);
                ViewBag.CurrentPage = PageConstant.PRODUCT_PORTFOLIO;
                return View();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> EditProductAsync(string productID, Product model, HttpPostedFileBase url_image)
        {
            try
            {
                var product = _userService.GetProductByID(productID);
                if (product == null)
                {
                    log.Error(LogConstants.LOG_PRODUCT_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PRODUCT;
                    return View("Error");
                }
                else
                {
                    if (url_image != null)
                    {
                        model.Url_Image = await GetUrlImageByFileBase(url_image, 253, 338);
                    }
                    ViewBag.Portfolio = _userService.GetPortfolioByID(model.PortfolioID);
                    ViewBag.CurrentPage = PageConstant.PRODUCT_PORTFOLIO;
                    if (ModelState.IsValid)
                    {
                        if (_userService.GetProductByNameAndPortfolioID(model.Name, model.PortfolioID) != null && model.Name != _userService.GetProductByID(productID).Name)
                        {
                            log.Error(LogConstants.LOG_DUPLICATE_NAME);
                            ViewBag.Type = CommonConstants.DANGER;
                            ViewBag.Message = MessageConstant.DUPLICATE_NAME;
                            ViewBag.Product = _userService.GetProductByID(productID);
                            return View();
                        }
                        int checkSuccess = _userService.EditProduct(productID, model);
                        if (checkSuccess >= 0)
                        {
                            log.Info(LogConstants.LOG_SUCCESS);
                            ViewBag.Type = CommonConstants.SUCCESS;
                            ViewBag.Message = MessageConstant.UPDATE_SUCCESS;
                            ViewBag.ListProduct = _userService.GetProductByPortfolioID(model.PortfolioID);
                            return View("ProductManager");
                        }
                        else
                        {
                            log.Info(LogConstants.LOG_FAIL);
                            ViewBag.Type = CommonConstants.DANGER;
                            ViewBag.Message = MessageConstant.UPDATE_FAIL;
                        }
                    }
                    model.ID = productID;
                    ViewBag.Product = model;
                    return View("EditProduct");
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult DeleteProduct(string productID)
        {
            try
            {
                log.Info(LogConstants.LOG_DELETE_PRODUCT);
                var product = _userService.GetProductByID(productID);
                if (product == null)
                {
                    log.Error(LogConstants.LOG_PRODUCT_NOT_FOUND);
                    ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PRODUCT;
                    return View("Error");
                }
                else
                {
                    string portfolioID = _userService.GetProductByID(productID).PortfolioID;
                    ViewBag.Portfolio = _userService.GetPortfolioByID(portfolioID);
                    ViewBag.CurrentPage = PageConstant.PRODUCT_PORTFOLIO;
                    int checkSuccess = _userService.DeleteProduct(productID);
                    if (checkSuccess > 0)
                    {
                        log.Info(LogConstants.LOG_SUCCESS);
                        ViewBag.Type = CommonConstants.SUCCESS;
                        ViewBag.Message = MessageConstant.DELETE_SUCCESS;
                    }
                    else
                    {
                        log.Info(LogConstants.LOG_FAIL);
                        ViewBag.Type = CommonConstants.DANGER;
                        ViewBag.Message = MessageConstant.DELETE_FAIL;
                    }
                    ViewBag.ListProduct = _userService.GetProductByPortfolioID(portfolioID);
                    return View("ProductManager");
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }
        #endregion
    }
}