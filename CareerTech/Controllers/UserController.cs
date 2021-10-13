using CareerTech.Models;
using CareerTech.Services;
using CareerTech.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace CareerTech.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService<UserService> _userService;

        public UserController(IUserService<UserService> userService)
        {
            _userService = userService;
        }

        // GET: User
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {
            return View();
        }
        #region UserProfile
        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult UserProfile()
        {
            ApplicationUser user = Session[SessionConstant.USER_MODEL] as ApplicationUser;
            return View(user);
        }

        [HttpPost]
        public ActionResult EditUserProfile()
        {
            return View();
        }
        #endregion

        #region PortfolioPage
        [HttpGet]
        public ActionResult Portfolio(string id)
        {
            if (_userService.GetPortfolioByID(id) == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                return View("Error");
            }
            ViewBag.Profile = _userService.GetProfileByPortfolioID(id);
            ViewBag.Skill = _userService.GetSkillByPortfolioID(id);
            ViewBag.Education = _userService.GetEducationByPortfolioID(id);
            ViewBag.Experience = _userService.GetExperienceByPortfolioID(id);
            ViewBag.Product = _userService.GetProductByPortfolioID(id);
            return View();
        }

        public ActionResult DownloadPortfolio(string id)
        {
            var PDF = IronPdf.ChromePdfRenderer.StaticRenderUrlAsPdf(new Uri("https://localhost:44396/User/Portfolio/" + id));
            return File(PDF.BinaryData, "application/pdf", "Portfolio.Pdf");
        }
        #endregion

        #region ImageProcess
        private async Task<string> GetUrlImageByFileBase(HttpPostedFileBase fileBase, int Height, int width)
        {
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
            return urlImage;
        }
        #endregion

        #region PortfolioManagement
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
                ViewBag.ErrorMessage = MessageConstant.DUPLICATE_NAME;
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
            }
            ViewBag.listPortfolio = _userService.GetPortfolioByUser(user.Id);
            return View("PortfolioManager");
        }

        [HttpPost]
        public JsonResult ChangeStatus(string id)
        {
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
            if (_userService.GetPortfolioByID(id) == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PORTFOLIO;
                return View("Error");
            }
            _userService.DeletePortfolio(id);
            return RedirectToAction("LoadPortfolio");
        }
        #endregion

        #region ProfileManagement
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
        public async Task<ActionResult> EditProfilePortfolioAsync(Profile model, string desc, HttpPostedFileBase url_avatar)
        {
            var portfolio = _userService.GetPortfolioByID(model.PortfolioID);
            var profile = _userService.GetProfileByPortfolioID(model.PortfolioID);
            model.Desc = desc;
            if (url_avatar != null)
            {
                model.Url_avatar = await GetUrlImageByFileBase(url_avatar, 350, 280);
            }
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
            ViewBag.CurrentPage = PageConstant.PROFILE_PORTFOLIO;
            return View("EditProfilePortfolio", model);
        }
        #endregion

        #region SkillManagement
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
                    ModelState.AddModelError(model.SkillName, "Lặp rồi!!!!!");
                    //   ViewBag.ErrorMessage = MessageConstant.DUPLICATE_NAME;
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
                else
                {
                    return View("EditSkill");
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
        #endregion

        #region EducationManagement
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
            }
            else
            {
                string portfolioID = education.PortfolioID;
                ViewBag.Portfolio = _userService.GetPortfolioByID(portfolioID);
                ViewBag.CurrentPage = PageConstant.EDUCATION_PORTFOLIO;
                _userService.DeleteEducation(educationID);
                ViewBag.ListEducation = _userService.GetEducationByPortfolioID(portfolioID);
                return View("EducationManager");
            }
        }
        #endregion

        #region ExperienceManagement
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
        #endregion

        #region ProductManagement
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

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> AddProductAsync(Product model, HttpPostedFileBase url_image)
        {
            if (_userService.GetPortfolioByID(model.PortfolioID) == null)
            {
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
                    ViewBag.ErrorMessage = MessageConstant.DUPLICATE_NAME;
                }
                else
                {
                    _userService.AddProduct(model);
                }
            }
            ViewBag.Portfolio = _userService.GetPortfolioByID(model.PortfolioID);
            ViewBag.ListProduct = _userService.GetProductByPortfolioID(model.PortfolioID);
            ViewBag.CurrentPage = PageConstant.PRODUCT_PORTFOLIO;
            return View("ProductManager");
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult EditProduct(string productID)
        {
            Product product = _userService.GetProductByID(productID);
            if (product == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PRODUCT;
                return View("Error");
            }
            ViewBag.Product = product;
            ViewBag.Portfolio = _userService.GetPortfolioByID(product.PortfolioID);
            ViewBag.CurrentPage = PageConstant.PRODUCT_PORTFOLIO;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> EditProductAsync(string productID, Product model, HttpPostedFileBase url_image)
        {
            var product = _userService.GetProductByID(productID);
            if (product == null)
            {
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
                        ViewBag.ErrorMessage = MessageConstant.DUPLICATE_NAME;
                        ViewBag.Product = _userService.GetProductByID(productID);
                        return View();
                    }
                    _userService.EditProduct(productID, model);
                }
                else
                {
                    return View("EditProduct");
                }
                ViewBag.ListProduct = _userService.GetProductByPortfolioID(model.PortfolioID);
                return View("ProductManager");
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult DeleteProduct(string productID)
        {
            var product = _userService.GetProductByID(productID);
            if (product == null)
            {
                ViewBag.ErrorMessage = MessageConstant.NOT_FOUND_PRODUCT;
                return View("Error");
            }
            else
            {
                string portfolioID = _userService.GetProductByID(productID).PortfolioID;
                ViewBag.Portfolio = _userService.GetPortfolioByID(portfolioID);
                ViewBag.CurrentPage = PageConstant.PRODUCT_PORTFOLIO;
                _userService.DeleteProduct(productID);
                ViewBag.ListProduct = _userService.GetProductByPortfolioID(portfolioID);
                return View("ProductManager");
            }
        }
        #endregion
    }
}