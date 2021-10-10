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
    public class AdminController : Controller
    {
        private readonly IUserManagmentService<UserManagementService> _UserManagmentService;
        private readonly IPartnerManagementService<PartnerManagementService> _PartnerManagementService;
        private readonly ISubscriptionManagementService<SubscriptionManagementService> _subscriptionManagementService;
        private readonly ISolutionManagementService<SolutionManagementService> _solutionManagementService;
        private readonly IContentService<ContentService> _contentManagement;
        public AdminController(IUserManagmentService<UserManagementService> UserManagmentService,
            IPartnerManagementService<PartnerManagementService> PartnerManagementService,
            ISubscriptionManagementService<SubscriptionManagementService> subscriptionManagementService,
            ISolutionManagementService<SolutionManagementService> solutionManagementService,
            IContentService<ContentService> contentManagement)
        {
            _UserManagmentService = UserManagmentService;
            _PartnerManagementService = PartnerManagementService;
            _subscriptionManagementService = subscriptionManagementService;
            _solutionManagementService = solutionManagementService;
            _contentManagement = contentManagement;
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
        [Authorize(Roles = "Admin")]
        public ActionResult UserManagement(string mess)
        {
            var users = _UserManagmentService.getAllUsers();
            ViewBag.User = users;
            ViewBag.Mess = mess;
            return View("UserManagement");
        }
        [Authorize(Roles = "Admin")]
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
            }catch(Exception e)
            {
                mess = e.Message;
            }
            return UserManagement(mess);
        }
        [Authorize(Roles = "Admin")]
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
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        #endregion
        #region PaymentManagement
        [Authorize(Roles = "Admin")]
        public ActionResult PaymentManagement()
        {
            return View();
        }
        #endregion
        #region PartnerManagement
        [Authorize(Roles = "Admin")]
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
                if (Name.Trim().Length > 0)
                {
                    string Type = Request.Form["type"].ToString();
                    if (Type.Trim().Length > 0)
                    {
                        float Price = float.Parse(Request.Form["price"].ToString());
                        if (Price.ToString().Trim().Length > 0)
                        {
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
                        }else
                    {
                            mess = "Price can not be empty";
                    }
                    }else
                    {
                        mess = "Type can not be empty";
                    }
                }
                else
                {
                    mess = "Name can not be empty";
                }
            }catch(Exception e)
            {
                mess = e.Message;
            }
            return SubscriptionManagement(mess);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditSubscription(string subID)
        {
            var subscription = _subscriptionManagementService.GetSubscriptionByID(subID);
            ViewBag.SubInfo = subscription;
            return View();

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditSubscription(string ID, string Name, float Price, string Type)
        {
            string mess = string.Empty;
            try
            {
                ID = Request.Form["id"].ToString();
                Name = Request.Form["SubName"].ToString();
                Type = Request.Form["type"].ToString();
                Price = float.Parse(Request.Form["price"].ToString());
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
            }catch(Exception e)
            {
                mess = e.Message;
            }
            return SubscriptionManagement(mess);
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult ContentManagement(string mess)
        {
            var intro = _contentManagement.GetAllIntroductions();
            ViewBag.Intro = intro;
            ViewBag.Mess = mess;
            return View("ContentManagement");
        }
        
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles="Admin")]
        public ActionResult AddContent(HttpPostedFileBase file, FormCollection collection)
        {
            string mess = string.Empty;
            try
            {
                Guid id = Guid.NewGuid();
                string uID = User.Identity.GetUserId();
                string Title = collection["Title"].ToString();
                if (Title.Trim().Length > 0)
                {
                    string Detail = collection["Des"].ToString();
                    if (Detail.Trim().Length > 0) {
                        string url = "";

                        if (file.ContentLength > 0)
                        {
                            string _FileName = Path.GetFileName(file.FileName);
                            string _path = Path.Combine(Server.MapPath("~/Content/Home/img"), _FileName);
                            file.SaveAs(_path);
                            url = CloudinaryUpload(_path, _FileName);
                        }
                        int result = _contentManagement.addContent(id, uID, Title, Detail, url);
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
                        mess = "Detail can not be empty";
                    }
                }
                else
                {
                    mess = "Title can not be Empty";
                }
            }
            catch(Exception e)
            {
                mess = e.Message;
            }
            return ContentManagement(mess);
           
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditContent(string contentID)
        {
            var intro = _contentManagement.GetIntroductionByID(contentID);
            ViewBag.Content = intro;
            return View(); 
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditContent(string contentID,HttpPostedFile file, FormCollection collection)
        {

            return View();
        }
        #endregion
        #region SolutionManangement
        [Authorize(Roles = "Admin")]
        public ActionResult SolutionManagement(string mess)
        {
            var sol = _solutionManagementService.GetSolutions();
            ViewBag.Mess = mess;
            ViewBag.Sol = sol;
          
            return View("SolutionManagement");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AddSolution(HttpPostedFileBase file)
        {
            string mess = string.Empty;
            try
            {
                Guid id = Guid.NewGuid();
                string Name = Request.Form["SolName"].ToString();
                string Des = Request.Form["des"].ToString();
               
                string url = "";
                
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Content/Home/img/services"), _FileName);
                    file.SaveAs(_path);
                    url = CloudinaryUpload(_path,_FileName);
                }
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
            catch(Exception e)
            {
                mess = e.Message;
            }
            return SolutionManagement(mess);
        }
       
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditSolution(string solID)
        {
            var solution = _solutionManagementService.GetSolutionByID(solID);
            ViewBag.SolInfo = solution;
            
            return View();

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditSolution(string solID, string Title, string Detail, string url_img, HttpPostedFileBase file)
        {
            string mess = string.Empty;
            try
            {
                solID = Request.Form["id"].ToString();
                Title = Request.Form["SolName"].ToString();
                Detail = Request.Form["des"].ToString();
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Content/Home/img/services"), _FileName);
                    file.SaveAs(_path);
                    url_img = CloudinaryUpload(_path,_FileName);
                }
                int result = _solutionManagementService.UpdateSolutionByID(solID, Title, Detail, url_img);
                if (result > 0)
                {
                    mess = "Save changes Successfully";
                }
                else
                {
                    mess = "Save changes failed";
                }
            }
            catch(Exception e)
            {
                mess = e.Message;
            }
            return SolutionManagement(mess);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult deleteSolution(string solID)
        {
            string mess = string.Empty;
            try
            {
                int result = _solutionManagementService.DeleteSolutionByID(solID);
                if(result > 0)
                {
                    mess = "Delete Successfully";
                }
                else
                {
                    mess = "Delete failed";
                }
            }catch(Exception e)
            {
                mess = e.Message;
            }
            return SolutionManagement(mess);
        }
        #endregion
        #region About Management
        [Authorize(Roles = "Admin")]
        public ActionResult AboutManagement()
        {
            return View();
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
            var uploadPath = uploadResult.Uri.AbsoluteUri;
            return uploadPath;
        }
        #endregion
    }
}