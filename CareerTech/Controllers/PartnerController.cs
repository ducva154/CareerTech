using CareerTech.Models;
using CareerTech.Services;
using CareerTech.Services.Implement;
using CareerTech.Utils;
using System;
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

        // GET CompanyProfile Page 

        [HttpGet]
        public ActionResult CompanyProfile()
        {
            CompanyProfile company = partnerService.GetCompanyProfileByPartnerId(Session[SessionConstant.USER_ID].ToString());
            if (company != null)
            {
                return View(company);
            }
            return View();
        }

        [HttpGet]
        public ActionResult CreateCompany()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> EditCompany(HttpPostedFileBase img_avatar, HttpPostedFileBase img_cover, FormCollection data)
        {
            CompanyProfile company = partnerService.GetCompanyProfileByPartnerId(Session[SessionConstant.USER_ID].ToString());
            string name = data["companyName"];
            string address = data["address"];
            string intro = data["introCompany"];
            string urlAvatar = await GetUrlImageByFileBase(img_avatar, 200, 200);
            string urlCover = await GetUrlImageByFileBase(img_cover, 281, 783);
            
            string message;
            string type;
            if (string.IsNullOrEmpty(name)
                || string.IsNullOrEmpty(address)
                || string.IsNullOrEmpty(intro))
            {
                message = MessageConstant.DATA_NOT_EMPTY;
                type = CommonConstants.DANGER;
            }
            else
            {
                company.CompanyName = name;
                company.Address = address;
                company.Desc = intro;
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
                message = MessageConstant.UPDATE_SUCCESS;
                type = CommonConstants.SUCCESS;
            }
            ViewBag.message = message;
            ViewBag.type = type;
            return View("CompanyProfile", company);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> CreateCompanyProfile(HttpPostedFileBase img_avatar, HttpPostedFileBase img_cover, FormCollection data)
        {
            string Id = Guid.NewGuid().ToString();
            string userId = Session[SessionConstant.USER_ID].ToString();
            string name = data["companyName"];
            string address = data["address"];
            string intro = data["introCompany"];
            string urlAvatar = await GetUrlImageByFileBase(img_avatar, 200, 200);
            string urlCover = await GetUrlImageByFileBase(img_cover, 281, 783);
            string message;
            string type;
            if (string.IsNullOrEmpty(name)
                || string.IsNullOrEmpty(address)
                || string.IsNullOrEmpty(intro))
            {
                message = MessageConstant.DATA_NOT_EMPTY;
                type = CommonConstants.DANGER;
                ViewBag.message = message;
                ViewBag.type = type;
                return View("CreateCompany");
            }
            else
            {
                CompanyProfile company = new CompanyProfile()
                {
                    ID = Id,
                    UserID = userId,
                    CompanyName = name,
                    Address = address,
                    Desc = intro,
                    Url_Avatar = urlAvatar,
                    Url_Background = urlCover,
                    Phone = "123456789",
                    Email = "fixcungthoima"
                };
                //add to DB 
                partnerService.CreateProfileCompany(company);
                message = MessageConstant.CREATE_SUCCESS;
                type = CommonConstants.SUCCESS;
                ViewBag.message = message;
                ViewBag.type = type;
                return View("CompanyProfile", company);
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

       
        // GET Recruitment Page 
        public ActionResult CreateRecruitment()
        {
            return View();
        }



        // GET Recruitment manage Page 
        public ActionResult RecruitmentManage()
        {
            return View();
        }

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