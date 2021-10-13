using CareerTech.Services;
using CareerTech.Services.Implement;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
namespace CareerTech.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserManagmentService<UserManagementService> _UserManagmentService;
        private readonly IPartnerManagementService<PartnerManagementService> _PartnerManagementService;
        private readonly ISubscriptionManagementService<SubscriptionManagementService> _subscriptionManagementService;
        private readonly ISolutionManagementService<SolutionManagementService> _solutionManagementService;
        private readonly IContentService<ContentService> _contentManagement;
        private readonly IAboutManagement<AboutService> _aboutManagement;
        public AdminController(IUserManagmentService<UserManagementService> UserManagmentService,
            IPartnerManagementService<PartnerManagementService> PartnerManagementService,
            ISubscriptionManagementService<SubscriptionManagementService> subscriptionManagementService,
            ISolutionManagementService<SolutionManagementService> solutionManagementService,
            IContentService<ContentService> contentManagement, IAboutManagement<AboutService> aboutManagement)
        {
            _UserManagmentService = UserManagmentService;
            _PartnerManagementService = PartnerManagementService;
            _subscriptionManagementService = subscriptionManagementService;
            _solutionManagementService = solutionManagementService;
            _contentManagement = contentManagement;
            _aboutManagement = aboutManagement;
        }

        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.NOUser = _UserManagmentService.countNumerOfUser();
            ViewBag.NOPartner = _PartnerManagementService.CountNoOfPartners();
            return View();
        }
        #region UserManagement
        public ActionResult UserManagement(string mess)
        {
            var users = _UserManagmentService.getAllUsers();
            ViewBag.User = users;
            ViewBag.Mess = mess;
            return View("UserManagement");
        }
        public ActionResult deleteUser(string userID)
        {
            string mess = string.Empty;
            try
            {
                var del = _UserManagmentService.deleteUser(userID);
                if (del > 0)
                {
                    mess = "Delete Successfully";
                }
                else
                {
                    mess = "Delete Fail";
                }
            }
            catch (Exception e)
            {
                mess = e.Message;
            }
            return UserManagement(mess);
        }
        public void ExportUserData()
        {
            try
            {
                ExcelPackage excelPackage = new ExcelPackage();
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("UserInformation");
                worksheet.Cells["A1"].Value = "Career";
                worksheet.Cells["B1"].Value = "Tech";
                worksheet.Cells["A2"].Value = "User";
                worksheet.Cells["B2"].Value = "Information";
                worksheet.Cells["A3"].Value = "Download Date:";
                worksheet.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

                worksheet.Cells["A6"].Value = "UserID";
                worksheet.Cells["B6"].Value = "FullName";
                worksheet.Cells["C6"].Value = "Email";
                worksheet.Cells["D6"].Value = "PhoneNumber";
                worksheet.Cells["E6"].Value = "Portfolio Domain";
                var userInfo = _UserManagmentService.getAllUsers();
                int rowStart = 7;
                foreach (var user in userInfo)
                {
                    worksheet.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("pink")));
                    var portfolioLink = user.Portfolios.Where(p => p.MainStatus == true).FirstOrDefault();
                    worksheet.Cells[string.Format("A{0}", rowStart)].Value = user.Id;
                    worksheet.Cells[string.Format("B{0}", rowStart)].Value = user.FullName;
                    worksheet.Cells[string.Format("C{0}", rowStart)].Value = user.Email;
                    worksheet.Cells[string.Format("D{0}", rowStart)].Value = user.PhoneNumber;
                    if (portfolioLink != null)
                    {
                        worksheet.Cells[string.Format("E{0}", rowStart)].Value = portfolioLink.Url_Domain;

                    }
                    else
                    {
                        worksheet.Cells[string.Format("E{0}", rowStart)].Value = "No data";

                    }
                    rowStart++;
                }
                worksheet.Cells["A:AZ"].AutoFitColumns();
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "UserInfor.xlxs");
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.End();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        #endregion
        #region PaymentManagement
        public ActionResult PaymentManagement()
        {
            return View();
        }
        #endregion
        #region PartnerManagement
        public ActionResult PartnerManagement()
        {
            var partners = _PartnerManagementService.getAllPartners();
            ViewBag.Partners = partners;
            return View();
        }
        #endregion
        #region SubscriptionManagement
        [Authorize(Roles = "Admin")]
        public ActionResult SubscriptionManagement(string mess)
        {
            var subs = _subscriptionManagementService.GetSubscriptions();
            ViewBag.Sub = subs;
            ViewBag.Mess = mess;
            return View("SubscriptionManagement");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AddnewSubscription()
        {
            string mess = string.Empty;
            try
            {
                Guid id = Guid.NewGuid();
                string Name = Request.Form["SubName"].ToString();
                if (!String.IsNullOrEmpty(Name.Trim()))
                {
                    string Type = Request.Form["type"].ToString();
                    if (!String.IsNullOrEmpty(Type.Trim()))
                    {
                        string txtPrice = Request.Form["price"].ToString();
                        if (!String.IsNullOrEmpty(txtPrice.Trim()))
                        {
                            float Price = float.Parse(txtPrice);
                            /*string Period = Request.Form["period"].ToString();
                            DateTime period = DateTime.Parse(Period);*/
                            int add = _subscriptionManagementService.addNewSubscription(id, Name, Price, Type);
                            if (add > 0)
                            {
                                mess = "Add Successfully";
                            }
                            else
                            {
                                mess = "Add Failed";
                            }
                        }
                        else
                        {
                            mess = "Add Failed! Price can not be empty";
                        }
                    }
                    else
                    {
                        mess = "Add Failed! Type can not be empty";
                    }
                }
                else
                {
                    mess = "Add Failed! Name can not be empty";
                }
            }
            catch (Exception e)
            {
                mess = e.Message;
            }
            return SubscriptionManagement(mess);
        }
        [HttpGet]
        public ActionResult EditSubscription(string subID)
        {
            var subscription = _subscriptionManagementService.GetSubscriptionByID(subID);
            ViewBag.SubInfo = subscription;
            return View();

        }
        [HttpPost]
        public ActionResult EditSubscription(string ID, string Name, float Price, string Type)
        {
            string mess = string.Empty;
            try
            {
                ID = Request.Form["id"].ToString();
                Name = Request.Form["SubName"].ToString();
                if (!String.IsNullOrEmpty(Name))
                {
                    Type = Request.Form["type"].ToString();
                    if (!String.IsNullOrEmpty(Type))
                    {
                        string txtPrice = Request.Form["price"].ToString();
                        if (!String.IsNullOrEmpty(txtPrice))
                        {
                            Price = float.Parse(txtPrice);
                            /*string Period = Request.Form["period"].ToString();
                            DateTime period = DateTime.Parse(Period);*/
                            int edit = _subscriptionManagementService.UpdateSubscriptionByID(ID, Name, Price, Type);
                            if (edit > 0)
                            {
                                mess = "Update Successfully";
                            }
                            else
                            {
                                mess = "Update Failed";
                            }
                        }
                        else
                        {
                            mess = "Update Failed! Price can not be empty";
                        }
                    }
                    else
                    {
                        mess = "Update Failed! Type can not be empty";
                    }
                }
                else
                {
                    mess = "Update Failed! Name can not be empty";
                }
            }
            catch (Exception e)
            {
                mess = e.Message;
            }
            return SubscriptionManagement(mess);
        }
        public ActionResult deleteSub(string subID)
        {
            string mess = string.Empty;
            var del = _subscriptionManagementService.DeleteSubscriptionByID(subID);
            if (del > 0)
            {
                mess = "Delete Successfully";
            }
            else
            {
                mess = "Delete Fail";
            }
            return SubscriptionManagement(mess);
        }
        #endregion
        #region ContentManagement
        public ActionResult ContentManagement(string mess)
        {
            var intro = _contentManagement.GetAllIntroductions();
            ViewBag.Intro = intro;
            ViewBag.Mess = mess;
            return View("ContentManagement");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddContent(HttpPostedFileBase file, FormCollection collection)
        {
            string mess = string.Empty;
            try
            {
                Guid id = Guid.NewGuid();
                string uID = User.Identity.GetUserId();
                string Title = collection["Title"].ToString();
                bool status;
                if (!String.IsNullOrEmpty(Title.Trim()))
                {
                    string Detail = collection["Des"].ToString();
                    if (!String.IsNullOrEmpty(Detail.Trim()))
                    {
                        string url = "";
                        string txtStatus = collection["Status"];
                        if (!String.IsNullOrEmpty(txtStatus))
                        {

                            if (txtStatus.Equals("Public"))
                            {
                                bool checkMainExisted = _contentManagement.CheckMainExisted();
                                if (checkMainExisted)
                                {
                                    mess = "Add Failed! Main Content Existed";
                                    return ContentManagement(mess);
                                }
                                else
                                {
                                    status = true;
                                }
                            }
                            else
                            {
                                status = false;
                            }
                            if (file == null)
                            {
                                mess = "Add Failed! Please choose image for your content";
                            }
                            else
                            {
                                string _FileName = Path.GetFileName(file.FileName);
                                string _path = Path.Combine(Server.MapPath("~/Content/Home/img"), _FileName);
                                file.SaveAs(_path);
                                url = CloudinaryUpload(_path, _FileName);
                                int result = _contentManagement.addContent(id, uID, Title, Detail, url, status);
                                if (result > 0)
                                {
                                    mess = "Add Successfully";
                                }
                                else
                                {
                                    mess = "Add failed";
                                }
                            }

                        }
                        else
                        {
                            mess = "Add failed! Please choose status for content";
                        }
                    }
                    else
                    {
                        mess = "Add failed! Detail can not be empty";
                    }
                }
                else
                {
                    mess = "Add failed! Title can not be Empty";
                }
            }
            catch (Exception e)
            {
                mess = e.Message;
            }
            return ContentManagement(mess);

        }
        [HttpGet]
        public ActionResult EditContent(string contentID, string mess)
        {
            var intro = _contentManagement.GetIntroductionByID(contentID);
            ViewBag.Content = intro;
            ViewBag.Mess = mess;
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditContent(HttpPostedFileBase file, FormCollection collection)
        {
            string mess = string.Empty;
            string contentID = collection["id"];
            try
            {
                string Title = collection["title"];
                string url = string.Empty;
                bool status = false;
                if (!string.IsNullOrEmpty(Title.Trim()))
                {
                    string Detail = collection["des"];
                    if (!string.IsNullOrEmpty(Detail.Trim()))
                    {
                        string txtStatus = collection["Status"];
                        if (txtStatus.Equals("Public"))
                        {
                            bool checkMainExisted = _contentManagement.CheckMainExisted();
                            if (checkMainExisted)
                            {
                                mess = "Save Changes Failed!! Main Content Existed";
                                return EditContent(contentID, mess);
                            }
                            else
                            {
                                status = true;
                            }
                        }
                        else
                        {
                            status = false;
                        }
                        if (file == null)
                        {
                            var content = _contentManagement.GetIntroductionByID(contentID);
                            url = content.Url_Image;
                        }
                        else
                        {
                            string _FileName = Path.GetFileName(file.FileName);
                            string _path = Path.Combine(Server.MapPath("~/Content/Home/img"), _FileName);
                            file.SaveAs(_path);
                            url = CloudinaryUpload(_path, _FileName);
                        }

                        int result = _contentManagement.updateContent(contentID, Title, Detail, url, status);
                        if (result > 0)
                        {
                            mess = "Save Changes Successfully";
                        }
                        else
                        {
                            mess = "Save Changes failed";
                        }
                    }
                    else
                    {
                        mess = "Save changes failed! Detail can not be empty";
                    }
                }
                else
                {
                    mess = "Save changes failed! Title can not be empty";
                }
            }
            catch (Exception e)
            {
                mess = e.Message;
               
            }
            return EditContent(contentID, mess);
        }
        public ActionResult deleteContent(string contentID)
        {
            string mess = string.Empty;
            try
            {
                int result = _contentManagement.deleteIntroductionByID(contentID);
                if (result > 0)
                {
                    mess = "Delete Sucessfully";
                }
                else
                {
                    mess = "Delete Failed";
                }
            }
            catch (Exception e)
            {
                mess = e.Message;
            }
            return ContentManagement(mess);
        }
        #endregion
        #region SolutionManangement
        public ActionResult SolutionManagement(string mess)
        {
            var sol = _solutionManagementService.GetSolutions();
            ViewBag.Mess = mess;
            ViewBag.Sol = sol;

            return View("SolutionManagement");
        }
        public ActionResult AddSolution(HttpPostedFileBase file)
        {
            string mess = string.Empty;
            try
            {
                Guid id = Guid.NewGuid();
                string Name = Request.Form["SolName"].ToString();
                if (!String.IsNullOrEmpty(Name.Trim()))
                {
                    string Des = Request.Form["des"].ToString();
                    if (!String.IsNullOrEmpty(Des.Trim()))
                    {
                        string url = "";
                        if (file == null)
                        {
                            mess = "Add Failed! Please add image for your solution";
                        }
                        else
                        {
                            string _FileName = Path.GetFileName(file.FileName);
                            string _path = Path.Combine(Server.MapPath("~/Content/Home/img/services"), _FileName);
                            file.SaveAs(_path);
                            url = CloudinaryUpload(_path, _FileName);
                            string uID = User.Identity.GetUserId();
                            int result = _solutionManagementService.AddSolution(id, uID, Name, Des, url);
                            if (result > 0)
                            {
                                mess = "Add Successfully";
                            }
                            else
                            {
                                mess = "Add failed";
                            }
                        }
                    }
                    else
                    {
                        mess = "Add failed! Detail can not be empty";
                    }
                }
                else
                {
                    mess = "Add failed! name can not be empty";
                }
            }
            catch (Exception e)
            {
                mess = e.Message;
            }
            return SolutionManagement(mess);
        }

        [HttpGet]
        public ActionResult EditSolution(string solID, string mess)
        {
            var solution = _solutionManagementService.GetSolutionByID(solID);
            ViewBag.SolInfo = solution;
            ViewBag.Mess = mess;
            return View();

        }
        [HttpPost]
        public ActionResult EditSolution(string solID, string Title, string Detail, string url_img, HttpPostedFileBase file)
        {
            string mess = string.Empty;
            try
            {
                solID = Request.Form["id"].ToString();
                Title = Request.Form["SolName"].ToString();
                if (!String.IsNullOrEmpty(Title.Trim()))
                {
                    Detail = Request.Form["des"].ToString();
                    if (!String.IsNullOrEmpty(Detail.Trim()))
                    {
                        if (file == null)
                        {
                            var solution = _solutionManagementService.GetSolutionByID(solID);
                            url_img = solution.Url_image;
                        }
                        else
                        {
                            string _FileName = Path.GetFileName(file.FileName);
                            string _path = Path.Combine(Server.MapPath("~/Content/Home/img/services"), _FileName);
                            file.SaveAs(_path);
                            url_img = CloudinaryUpload(_path, _FileName);
                        }

                        int result = _solutionManagementService.UpdateSolutionByID(solID, Title, Detail, url_img);
                        if (result > 0)
                        {
                            mess = "Save changes Successfully";
                            return SolutionManagement(mess);
                        }
                        else
                        {
                            mess = "Save changes failed";
                        }
                    }
                    else
                    {
                        mess = "Save changes failed! Detail can not be empty";
                    }
                }
                else
                {
                    mess = "Save changes failed! Title can not be empty";
                }
            }
            catch (Exception e)
            {
                mess = e.Message;

            }
            return EditSolution(solID, mess);
        }
        public ActionResult deleteSolution(string solID)
        {
            string mess = string.Empty;
            try
            {
                int result = _solutionManagementService.DeleteSolutionByID(solID);
                if (result > 0)
                {
                    mess = "Delete Successfully";
                }
                else
                {
                    mess = "Delete failed";
                }
            }
            catch (Exception e)
            {
                mess = e.Message;
            }
            return SolutionManagement(mess);
        }
        #endregion
        #region About Management
        public ActionResult AboutManagement(string mess)
        {
            var abouts = _aboutManagement.getAllAbouts();
            ViewBag.About = abouts;
            ViewBag.Mess = mess;
            return View("AboutManagement");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult AddAbout(FormCollection collection)
        {
            string mess = string.Empty;
            try
            {
                Guid id = Guid.NewGuid();
                string uID = User.Identity.GetUserId();
                string Title = collection["Title"].ToString();
                bool status;
                if (!String.IsNullOrEmpty(Title.Trim()))
                {
                    string Detail = collection["detail"].ToString();
                    if (!String.IsNullOrEmpty(Detail.Trim()))
                    {
                        string Description = collection["Des"];
                        if (!String.IsNullOrEmpty(Description.Trim()))
                        {
                            string txtStatus = collection["Status"];
                            if (!String.IsNullOrEmpty(txtStatus))
                            {

                                if (txtStatus.Equals("Public"))
                                {
                                    bool checkMainExisted = _aboutManagement.CheckMainStatusExisted();
                                    if (checkMainExisted)
                                    {
                                        mess = "Add Failed! Main Content Existed";
                                        return AboutManagement(mess);
                                    }
                                    else
                                    {
                                        status = true;
                                    }
                                }
                                else
                                {
                                    status = false;
                                }

                                int result = _aboutManagement.AddAbout(id, uID, Title, Detail, Description, status);
                                if (result > 0)
                                {
                                    mess = "Add Successfully";
                                }
                                else
                                {
                                    mess = "Add failed";
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            mess = "Add failed! Please choose status for about";
                        }
                    }
                    else
                    {
                        mess = "Add failed! Detail can not be empty";
                    }
                }
                else
                {
                    mess = "Add failed! Title can not be Empty";
                }
            }
            catch (Exception e)
            {
                mess = e.Message;
            }
            return AboutManagement(mess);
        }
        [HttpGet]
        public ActionResult EditAbout(string aboutID, string mess)
        {
            var about = _aboutManagement.getAboutByID(aboutID);
            ViewBag.About = about;
            ViewBag.Mess = mess;
            return View("EditAbout");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditAbout(FormCollection collection)
        {
            string mess = string.Empty;
            string aboutID = collection["id"];
            bool status = false;
            try
            {
                string Title = collection["Title"];
                if (!String.IsNullOrEmpty(Title.Trim()))
                {
                    string Detail = collection["detail"];
                    if (!String.IsNullOrEmpty(Detail.Trim()))
                    {
                        string Desc = collection["des"];
                        if (!String.IsNullOrEmpty(Desc))
                        {
                            string txtStatus = collection["Status"];
                            if (txtStatus.Equals("Public"))
                            {
                                bool checkMainExisted = _aboutManagement.CheckMainStatusExisted();
                                if (checkMainExisted)
                                {
                                    mess = "Save Changes Failed! Main Content Existed";
                                    return EditAbout(aboutID, mess);
                                }
                                else
                                {
                                    status = true;
                                }
                            }

                            else
                            {
                                status = false;
                            }
                            int result = _aboutManagement.UpdateAbout(aboutID, Title, Detail, Desc, status);
                            if (result > 0)
                            {
                                mess = "Save Changes Successfully";
                                return AboutManagement(mess);
                            }
                            else
                            {
                                mess = "Save Changes Failed";
                            }
                        }
                        else
                        {
                            mess = "Save changes Failed! Description Can Not Be Empty";
                        }
                    }
                    else
                    {
                        mess = "Save Changes Failed! Detail Can Not Be Empty";
                    }
                }
                else
                {
                    mess = "Save Changes Failed! Title Can Not Be Empty";
                }
            }
            catch (Exception e)
            {
                mess = e.Message;
            }
            return EditAbout(aboutID, mess);
        }
        public ActionResult DeleteAbout(string aboutID)
        {
            string mess = string.Empty;
            try
            {
                int result = _aboutManagement.deleteAboutByID(aboutID);
                if (result > 0)
                {
                    mess = "Delete Succesfully";
                }
                else
                {
                    mess = "Delete Failed";
                }
            }
            catch (Exception e)
            {
                mess = e.Message;
            }
            return AboutManagement(mess);
        }
        #endregion
        #region Upload Cloudinary
        public string CloudinaryUpload(string img_path, string Name)
        {
            Account account = new Account(
          "dathm",
          "815564641783264",
          "NdzOfyAMMdTGE3GlfjU5hg8Kc3o");
            Cloudinary cloudinary = new Cloudinary(account);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(@"" + img_path),
                PublicId = Name
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            var uploadPath = uploadResult.Url.AbsoluteUri;
            return uploadPath;
        }
        #endregion
    }
}