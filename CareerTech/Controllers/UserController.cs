using CareerTech.Models;
using CareerTech.Services;
using CareerTech.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareerTech.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService<UserService> _userService;

        public UserController(IUserService<UserService> userService)
        {
            _userService = userService;
        }

        public ActionResult Portfolio(string id)
        {
            ViewBag.id = id;
            return View();
        }

        // GET: User
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: User
        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult LoadPortfolio()
        {
            var user = Session[SessionConstant.USER_MODEL] as ApplicationUser;
            ViewBag.listPortfolio = _userService.GetPortfolioByUser(user.Id);
            return View("PortfolioManager");
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult CreatePortfolio(string portfolioName)
        {
            var user = Session[SessionConstant.USER_MODEL] as ApplicationUser;
            if (_userService.GetPortfolioByNameAndUser(portfolioName, user.Id).Count > 0)
            {
                ViewBag.listPortfolio = _userService.GetPortfolioByUser(user.Id);
                ViewBag.ErrorMessage = MessageConstant.DUPLICATE_NAME;
                return View("PortfolioManager");
            }
            else
            {
                Portfolio portfolio = new Portfolio();
                portfolio.ID = Guid.NewGuid().ToString();
                portfolio.UserID = user.Id;
                portfolio.Name = portfolioName;
                portfolio.PublicStatus = false;
                portfolio.MainStatus = false;
                portfolio.Url_Domain = "/User/Portfolio/" + portfolio.ID;
                _userService.InsertPortfolio(portfolio);
                ViewBag.listPortfolio = _userService.GetPortfolioByUser(user.Id);
                return View("PortfolioManager");
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult DeletePortfolio(string id)
        {
            if (_userService.GetPortfolioByID(id) == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                return View("Error");
            }
            _userService.DeletePortfolio(id);
            var user = Session[SessionConstant.USER_MODEL] as ApplicationUser;
            ViewBag.listPortfolio = _userService.GetPortfolioByUser(user.Id);
            return View("PortfolioManager");
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult EditProfilePortfolio(string id)
        {
            if (_userService.GetPortfolioByID(id) == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                return View("Error");
            }
            ViewBag.Portfolio = _userService.GetPortfolioByID(id);
            ViewBag.Profile = _userService.GetProfileByPortfolioID(id);
            ViewBag.CurrentPage = PageConstant.PROFILE_PORTFOLIO;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult EditProfilePortfolio(Profile model, string desc)
        {
            var portfolio = _userService.GetPortfolioByID(model.PortfolioID);
            var profile = _userService.GetProfileByPortfolioID(model.PortfolioID);
            model.Desc = desc;
            if (ModelState.IsValid)
            {
                if (profile == null)
                {
                    _userService.CreateProfile(model);
                }
                else
                {
                    _userService.EditProfile(model);
                }

            }
            ViewBag.Portfolio = portfolio;
            ViewBag.Profile = profile;
            return View(model);
        }
        //==========LOAD SKILL ======================
        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult LoadSkill(string id)
        {
            if (_userService.GetPortfolioByID(id) == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                return View("Error");
            }
            ViewBag.Portfolio = _userService.GetPortfolioByID(id);
            ViewBag.ListSkill = _userService.GetSkillByPortfolioID(id);
            ViewBag.CurrentPage = PageConstant.SKILL_PORTFOLIO;
            return View("SkillManager");
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult AddSkill(Skill model)
        {
            if (_userService.GetPortfolioByID(model.PortfolioID) == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                return View("Error");
            }
            if (ModelState.IsValid)
            {
                if (_userService.GetSkillByNameAndPortfolioID(model.SkillName, model.PortfolioID) != null)
                {
                    ViewBag.ErrorMessage = MessageConstant.DUPLICATE_NAME;
                }
                else
                {
                    _userService.AddSkill(model);
                }
            }
            ViewBag.Portfolio = _userService.GetPortfolioByID(model.PortfolioID);
            ViewBag.ListSkill = _userService.GetSkillByPortfolioID(model.PortfolioID);
            ViewBag.CurrentPage = PageConstant.SKILL_PORTFOLIO;
            return View("SkillManager");
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult EditSkill(string skillID)
        {
            Skill skill = _userService.GetSkillByID(skillID);
            if (skill == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_SKILL;
                return View("Error");
            }
            ViewBag.Skill = skill;
            ViewBag.Portfolio = _userService.GetPortfolioByID(skill.PortfolioID);
            ViewBag.CurrentPage = PageConstant.SKILL_PORTFOLIO;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult EditSkill(string skillID, Skill model)
        {
            var skill = _userService.GetSkillByID(skillID);
            if (skill == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_SKILL;
                return View("Error");
            }
            else
            {
                ViewBag.Portfolio = _userService.GetPortfolioByID(model.PortfolioID);
                ViewBag.CurrentPage = PageConstant.SKILL_PORTFOLIO;
                if (ModelState.IsValid)
                {
                    if (_userService.GetSkillByNameAndPortfolioID(model.SkillName, model.PortfolioID) != null && model.SkillName != _userService.GetSkillByID(skillID).SkillName)
                    {
                        ViewBag.ErrorMessage = MessageConstant.DUPLICATE_NAME;
                        ViewBag.Skill = _userService.GetSkillByID(skillID);
                        return View();
                    }
                    _userService.EditSkill(skillID, model);
                }
                ViewBag.ListSkill = _userService.GetSkillByPortfolioID(model.PortfolioID);
                return View("SkillManager");
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult DeleteSkill(string skillID)
        {
            var skill = _userService.GetSkillByID(skillID);
            if (skill == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_SKILL;
                return View("Error");
            }
            else
            {
                string portfolioID = _userService.GetSkillByID(skillID).PortfolioID;
                ViewBag.Portfolio = _userService.GetPortfolioByID(portfolioID);
                ViewBag.CurrentPage = PageConstant.SKILL_PORTFOLIO;
                _userService.DeleteSkill(skillID);
                ViewBag.ListSkill = _userService.GetSkillByPortfolioID(portfolioID);
                return View("SkillManager");
            }
        }

        //==========LOAD EDU ======================
        [HttpGet]
        //[Authorize(Roles = "User")]
        public ActionResult LoadEducation(string id)
        {
            if (_userService.GetPortfolioByID(id) == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                return View("Error");
            }
            ViewBag.Portfolio = _userService.GetPortfolioByID(id);
            ViewBag.CurrentPage = PageConstant.EDUCATION_PORTFOLIO;
            ViewBag.ListEducation = _userService.GetEducationByPortfolioID(id);
            return View("EducationManager");
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateInput(false)]
        public ActionResult AddEducation(Education model)
        {
            if (_userService.GetPortfolioByID(model.PortfolioID) == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                return View("Error");
            }
            _userService.AddEducation(model);
            ViewBag.Portfolio = _userService.GetPortfolioByID(model.PortfolioID);
            ViewBag.ListEducation = _userService.GetEducationByPortfolioID(model.PortfolioID);
            ViewBag.CurrentPage = PageConstant.EDUCATION_PORTFOLIO;
            return View("EducationManager");
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult EditEducation(string educationID)
        {
            Education education = _userService.GetEducationByID(educationID);
            if (education == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_EDUCATION;
                return View("Error");
            }
            ViewBag.Education = education;
            ViewBag.Portfolio = _userService.GetPortfolioByID(education.PortfolioID);
            ViewBag.CurrentPage = PageConstant.EDUCATION_PORTFOLIO;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateInput(false)]
        public ActionResult EditEducation(string educationID, Education model, FormCollection collection)
        {
            var education = _userService.GetEducationByID(educationID);
            if (education == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_EDUCATION;
                return View("Error");
            }
            else
            {
                ViewBag.Portfolio = _userService.GetPortfolioByID(model.PortfolioID);
                ViewBag.CurrentPage = PageConstant.EDUCATION_PORTFOLIO;
                string detail = collection["Detail"];
                model.Detail = detail;
                _userService.EditEducation(educationID, model);
                ViewBag.ListEducation = _userService.GetEducationByPortfolioID(model.PortfolioID);
                return View("EducationManager");
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult DeleteEducation(string educationID)
        {
            var education = _userService.GetEducationByID(educationID);
            if (education == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_EDUCATION;
                return View("Error");
            } else
            {
                string portfolioID = education.PortfolioID;
                ViewBag.Portfolio = _userService.GetPortfolioByID(portfolioID);
                ViewBag.CurrentPage = PageConstant.EDUCATION_PORTFOLIO;
                _userService.DeleteEducation(educationID);
                ViewBag.ListEducation = _userService.GetEducationByPortfolioID(portfolioID);
                return View("EducationManager");
            }
        }

        //==========LOAD EXP ======================
        [HttpGet]
        //[Authorize(Roles = "User")]
        public ActionResult LoadExperience(string id)
        {
            if (_userService.GetPortfolioByID(id) == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                return View("Error");
            }
            ViewBag.Portfolio = _userService.GetPortfolioByID(id);
            ViewBag.CurrentPage = PageConstant.EXPERIENCE_PORTFOLIO;
            ViewBag.ListExperience = _userService.GetExperienceByPortfolioID(id);
            return View("ExperienceManager");
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateInput(false)]
        public ActionResult AddExperience(Experience model)
        {
            if (_userService.GetPortfolioByID(model.PortfolioID) == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                return View("Error");
            }
            _userService.AddExperience(model);
            ViewBag.Portfolio = _userService.GetPortfolioByID(model.PortfolioID);
            ViewBag.ListExperience = _userService.GetExperienceByPortfolioID(model.PortfolioID);
            ViewBag.CurrentPage = PageConstant.EXPERIENCE_PORTFOLIO;
            return View("ExperienceManager");
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult EditExperience(string experienceID)
        {
            Experience experience = _userService.GetExperienceByID(experienceID);
            if (experience == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_EXPERIENCE;
                return View("Error");
            }
            ViewBag.Experience = experience;
            ViewBag.Portfolio = _userService.GetPortfolioByID(experience.PortfolioID);
            ViewBag.CurrentPage = PageConstant.EXPERIENCE_PORTFOLIO;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateInput(false)]
        public ActionResult EditExperience(string experienceID, Experience model, FormCollection collection)
        {
            var experience = _userService.GetExperienceByID(experienceID);
            if (experience == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_EXPERIENCE;
                return View("Error");
            }
            else
            {
                ViewBag.Portfolio = _userService.GetPortfolioByID(model.PortfolioID);
                ViewBag.CurrentPage = PageConstant.EDUCATION_PORTFOLIO;
                string detail = collection["Detail"];
                model.Detail = detail;
                _userService.EditExperience(experienceID, model);
                ViewBag.ListExperience = _userService.GetExperienceByPortfolioID(model.PortfolioID);
                return View("ExperienceManager");
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult DeleteExperience(string experienceID)
        {
            var experience = _userService.GetExperienceByID(experienceID);
            if (experience == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_EXPERIENCE;
                return View("Error");
            }
            else
            {
                string portfolioID = experience.PortfolioID;
                ViewBag.Portfolio = _userService.GetPortfolioByID(portfolioID);
                ViewBag.CurrentPage = PageConstant.EDUCATION_PORTFOLIO;
                _userService.DeleteExperience(experienceID);
                ViewBag.ListExperience = _userService.GetExperienceByPortfolioID(portfolioID);
                return View("ExperienceManager");
            }
        }
        //==========LOAD PRODUCT ======================
        [HttpGet]
        //[Authorize(Roles = "User")]
        public ActionResult LoadProduct(string id)
        {
            if (_userService.GetPortfolioByID(id) == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                return View("Error");
            }
            ViewBag.Portfolio = _userService.GetPortfolioByID(id);
            ViewBag.CurrentPage = PageConstant.PRODUCT_PORTFOLIO;
            ViewBag.ListProduct = _userService.GetProductByPortfolioID(id);
            return View("ProductManager");
        }
    }
}