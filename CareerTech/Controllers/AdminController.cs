using CareerTech.Services;
using CareerTech.Services.Implement;
using CareerTech.Utils;
using log4net;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static CareerTech.Utils.CloudDiaryService;
using static CareerTech.Utils.LogConstants;

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
        private readonly IOrderManagementService<OrderManagementService> _orderManagementService;
        private readonly ILog log = LogManager.GetLogger(typeof(AdminController));

        public AdminController(IUserManagmentService<UserManagementService> UserManagmentService,
            IPartnerManagementService<PartnerManagementService> PartnerManagementService,
            ISubscriptionManagementService<SubscriptionManagementService> subscriptionManagementService,
            ISolutionManagementService<SolutionManagementService> solutionManagementService,
            IContentService<ContentService> contentManagement, IAboutManagement<AboutService> aboutManagement,
            IOrderManagementService<OrderManagementService> orderManagementService)
        {
            _UserManagmentService = UserManagmentService;
            _PartnerManagementService = PartnerManagementService;
            _subscriptionManagementService = subscriptionManagementService;
            _solutionManagementService = solutionManagementService;
            _contentManagement = contentManagement;
            _aboutManagement = aboutManagement;
            _orderManagementService = orderManagementService;
        }
        string mess = string.Empty;
        // GET: Admin

        public ActionResult Index()
        {
            try
            {
                int NoOfUser = _UserManagmentService.countNumerOfUser();
                ViewBag.NOUser = NoOfUser;
                int NoOfPartner = _PartnerManagementService.CountNoOfPartners();
                ViewBag.NOPartner = NoOfPartner;
                if (ViewBag.NOUser == null || ViewBag.NOPartner == null)
                {
                    log.Error("No Data");
                    return View("error");
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                ViewBag.Mess = e.Message;
                return View("error");
            }
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
        public ActionResult DeleteUser(string userID)
        {
            try
            {
                var del = _UserManagmentService.deleteUser(userID);
                if (del > 0)
                {
                    mess = MessageConstant.DELETE_SUCCESS;
                }
                else
                {
                    mess = MessageConstant.DELETE_FAIL;
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                mess = e.Message;
                return View("error");
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
                log.Info(DOWNLOAD_USER_INFO);
                Response.End();
            }
            catch (Exception e)
            {
                log.Info(e.Message + " while exporting");

            }

        }

        #endregion
        #region PaymentManagement
        public ActionResult PaymentManagement()
        {
            var paymentDetail = _orderManagementService.GetAllOrderDetail();
            ViewBag.Payment = paymentDetail;
            return View();
        }
        [HttpGet]
        public ActionResult OrderDetail(string id)
        {
            var orderDetail = _orderManagementService.GetOrderDetail(id);
            ViewBag.Order = orderDetail;
            return View();
        }
        #endregion
        #region PartnerManagement
        public ActionResult PartnerManagement(string mess)
        {
            var partnersWithCompany = _PartnerManagementService.getAllPartners();
            var partners = _PartnerManagementService.getPartners();
            ViewBag.Partners = partnersWithCompany;
            ViewBag.Partners2 = partners;
            ViewBag.Mess = mess;
            return View("PartnerManagement");
        }
        public ActionResult ApprovePartner(string id)
        {
            try
            {
                int result = _PartnerManagementService.ApprovePartner(id);
                log.Info("Approved Partner " + id);
                if (result > 0)
                {
                    mess = MessageConstant.APPROVE_SUCCESS;
                }
                else
                {
                    mess = MessageConstant.APPROVE_FAIL;
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message + " at ApprovePartner");
                mess = e.Message;
            }
            return PartnerManagement(mess);
        }
        public async Task<ActionResult> RejectPartner(string id)
        {
            try
            {
                await _PartnerManagementService.RejectPartner(id);
            }
            catch (Exception e)
            {
                log.Error(e.Message + " at RejectPartner");
                mess = e.Message;
            }
            return PartnerManagement(mess);
        }
        #endregion
        #region SubscriptionManagement
        public ActionResult SubscriptionManagement(string mess)
        {
            var subs = _subscriptionManagementService.GetSubscriptions();
            ViewBag.Sub = subs;
            ViewBag.Mess = mess;
            return View("SubscriptionManagement");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult AddnewSubscription(FormCollection collection)
        {
            try
            {
                Guid id = Guid.NewGuid();
                string Name = collection["SubName"];
                if (!string.IsNullOrEmpty(Name.Trim()))
                {
                    string Type = collection["type"];
                    if (!string.IsNullOrEmpty(Type.Trim()))
                    {
                        string txtPrice = collection["price"];
                        if (!string.IsNullOrEmpty(txtPrice.Trim()))
                        {
                            float Price = float.Parse(txtPrice);
                            string txtPeriod = collection["period"];
                            if (!string.IsNullOrEmpty(txtPeriod))
                            {
                                int Period = Int32.Parse(txtPeriod);
                                string detailDescription = collection["Des"];
                                if (!string.IsNullOrEmpty(detailDescription))
                                {
                                    int result = _subscriptionManagementService.AddNewSubscription(id, Name, Price, Type, Period, detailDescription);
                                    if (result > 0)
                                    {

                                        mess = MessageConstant.ADD_SUCCESS;
                                        log.Error(mess);
                                    }
                                    else
                                    {

                                        mess = MessageConstant.ADD_FAILED;
                                        log.Error(mess);

                                    }
                                }
                                else
                                {

                                    mess = MessageConstant.UPDATE_FAILED_DATA_EMPTY;
                                    log.Error(mess);

                                }
                            }
                            else
                            {

                                mess = MessageConstant.UPDATE_FAILED_DATA_EMPTY;
                                log.Error(mess);

                            }
                        }
                        else
                        {

                            mess = MessageConstant.UPDATE_FAILED_DATA_EMPTY;
                            log.Error(mess);

                        }
                    }
                    else
                    {

                        mess = MessageConstant.UPDATE_FAILED_DATA_EMPTY;
                        log.Error(mess);

                    }
                }
                else
                {

                    mess = MessageConstant.UPDATE_FAILED_DATA_EMPTY;
                    log.Error(mess);

                }
            }
            catch (Exception e)
            {
                log.Error(e.Message + " at Add Subscription");
                mess = e.Message;
            }
            return SubscriptionManagement(mess);
        }
        [HttpGet]
        public ActionResult EditSubscription(string subID, string mess)
        {
            try
            {
                var subscription = _subscriptionManagementService.GetSubscriptionByID(subID);
                ViewBag.SubInfo = subscription;
                if (ViewBag.SubInfo == null)
                {
                    ViewBag.Mess = "No data";
                    log.Error(ViewBag.Mess);
                    return View("error");
                }
                ViewBag.Mess = mess;
            }
            catch (Exception e)
            {
                log.Error(e.Message + " at EditSubscription");
                ViewBag.Mess = e.Message;
                return View("error");
            }

            return View();

        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditSubscription(FormCollection collection)
        {
            string ID;
            try
            {
                ID = collection["id"].ToString();
                string Name = collection["SubName"];
                if (!string.IsNullOrEmpty(Name))
                {
                    string Type = collection["type"];
                    if (!string.IsNullOrEmpty(Type))
                    {
                        string txtPrice = Request.Form["price"].ToString();
                        if (!string.IsNullOrEmpty(txtPrice))
                        {
                            float Price = float.Parse(txtPrice);
                            string txtPeriod = collection["period"];
                            if (!string.IsNullOrEmpty(txtPeriod))
                            {
                                int Period = Int32.Parse(txtPeriod);
                                string Desc = collection["Des"];
                                if (!string.IsNullOrEmpty(Desc))
                                {
                                    int edit = _subscriptionManagementService.UpdateSubscriptionByID(ID, Name, Price, Type, Period, Desc);
                                    if (edit > 0)
                                    {
                                        mess = MessageConstant.UPDATE_SUCCESS;
                                        return SubscriptionManagement(mess);
                                    }
                                    else
                                    {
                                        mess = MessageConstant.UPDATE_FAIL;
                                        log.Error(mess);
                                    }
                                }
                                else
                                {
                                    mess = MessageConstant.UPDATE_FAILED_DATA_EMPTY;
                                    log.Error(mess);

                                }
                            }
                            else
                            {
                                mess = MessageConstant.UPDATE_FAILED_DATA_EMPTY;
                                log.Error(mess);

                            }
                        }
                        else
                        {
                            mess = MessageConstant.UPDATE_FAILED_DATA_EMPTY;
                            log.Error(mess);

                        }
                    }
                    else
                    {
                        mess = MessageConstant.UPDATE_FAILED_DATA_EMPTY;
                        log.Error(mess);

                    }
                }
                else
                {
                    mess = MessageConstant.UPDATE_FAILED_DATA_EMPTY;
                    log.Error(mess);

                }
            }
            catch (Exception e)
            {
                log.Error(e.Message + " at EditSubscription");
                mess = e.Message;
                return View("error");
            }
            return EditSubscription(ID, mess);
        }
        public ActionResult DeleteSub(string subID)
        {
            try
            {
                var del = _subscriptionManagementService.DeleteSubscriptionByID(subID);
                if (del > 0)
                {
                    mess = MessageConstant.DELETE_SUCCESS;
                }
                else
                {
                    mess = MessageConstant.DELETE_FAIL;
                    log.Error(mess);

                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return View("error");
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
        public async Task<ActionResult> AddContent(HttpPostedFileBase file, FormCollection collection)
        {
            try
            {
                Guid id = Guid.NewGuid();
                string uID = User.Identity.GetUserId();
                string Title = collection["Title"].ToString();
                bool status;
                if (!string.IsNullOrEmpty(Title.Trim()))
                {
                    string Detail = collection["Des"].ToString();
                    if (!string.IsNullOrEmpty(Detail.Trim()))
                    {
                        string url = "";
                        string txtStatus = collection["Status"];
                        if (!string.IsNullOrEmpty(txtStatus))
                        {

                            if (txtStatus.Equals("Public"))
                            {
                                bool checkMainExisted = _contentManagement.CheckMainExisted();
                                if (checkMainExisted)
                                {
                                    mess = MessageConstant.ADD_FAILED + MessageConstant.MAIN_EXISTED;
                                    log.Info(mess);
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
                                url = await CloudinaryUpload(_path, 200, 300);
                                int result = _contentManagement.addContent(id, uID, Title, Detail, url, status);
                                if (result > 0)
                                {
                                    mess = MessageConstant.ADD_SUCCESS;
                                }
                                else
                                {
                                    mess = MessageConstant.ADD_FAILED;
                                    log.Error(mess);
                                }
                            }
                        }
                        else
                        {
                            mess = MessageConstant.ADD_FAILED_DATA_EMPTY;
                            log.Error(mess);
                        }
                    }
                    else
                    {
                        mess = MessageConstant.ADD_FAILED_DATA_EMPTY;
                        log.Error(mess);
                    }
                }
                else
                {
                    mess = MessageConstant.ADD_FAILED_DATA_EMPTY;
                    log.Error(mess);
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message + "at AddContent");
                mess = e.Message;
                ViewBag.Mess = "No data";
            }
            return ContentManagement(mess);

        }
        [HttpGet]
        public ActionResult EditContent(string contentID, string mess)
        {
            try
            {
                var intro = _contentManagement.GetIntroductionByID(contentID);
                ViewBag.Content = intro;
                if (ViewBag.Content == null)
                {
                    ViewBag.Mess = "No data";
                    log.Error(ViewBag.Mess);
                    return View("error");
                }
                ViewBag.Mess = mess;
            }
            catch (Exception e)
            {
                log.Error(e.Message + " at EditContent");
                ViewBag.Mess = e.Message;
                return View("error");
            }
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> EditContent(HttpPostedFileBase file, FormCollection collection)
        {
            string contentID;
            try
            {
                contentID = collection["id"];
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
                                mess = MessageConstant.UPDATE_FAIL + MessageConstant.MAIN_EXISTED;
                                log.Error(mess);
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
                            url = await CloudinaryUpload(_path, 200, 200);
                        }

                        int result = _contentManagement.updateContent(contentID, Title, Detail, url, status);
                        if (result > 0)
                        {
                            mess = MessageConstant.UPDATE_SUCCESS;
                            return ContentManagement(mess);
                        }
                        else
                        {
                            mess = MessageConstant.UPDATE_FAIL;
                            log.Error(mess);
                        }
                    }
                    else
                    {
                        mess = MessageConstant.UPDATE_FAILED_DATA_EMPTY;
                        log.Error(mess);

                    }
                }
                else
                {
                    mess = MessageConstant.UPDATE_FAILED_DATA_EMPTY;
                    log.Error(mess);
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message + " at EditContent");
                mess = e.Message;
                return View("error");

            }
            return EditContent(contentID, mess);
        }
        public ActionResult DeleteContent(string contentID)
        {
            try
            {
                int result = _contentManagement.deleteIntroductionByID(contentID);
                if (result > 0)
                {
                    mess = MessageConstant.DELETE_SUCCESS;
                }
                else
                {
                    mess = MessageConstant.DELETE_FAIL;
                }
            }
            catch (Exception e)
            {
                mess = e.Message;
                log.Error(mess + " at DeleteContent");
                return View("error");
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
        public async Task<ActionResult> AddSolution(HttpPostedFileBase file)
        {
            try
            {
                Guid id = Guid.NewGuid();
                string Name = Request.Form["SolName"].ToString();
                if (!string.IsNullOrEmpty(Name.Trim()))
                {
                    string Des = Request.Form["des"].ToString();
                    if (!string.IsNullOrEmpty(Des.Trim()))
                    {
                        string url = "";
                        if (file == null)
                        {
                            mess = MessageConstant.ADD_FAILED_DATA_EMPTY;
                        }
                        else
                        {
                            string _FileName = Path.GetFileName(file.FileName);
                            string _path = Path.Combine(Server.MapPath("~/Content/Home/img/services"), _FileName);
                            file.SaveAs(_path);
                            url = await CloudinaryUpload(_path, 200, 200);
                            string uID = User.Identity.GetUserId();
                            int result = _solutionManagementService.AddSolution(id, uID, Name, Des, url);
                            if (result > 0)
                            {
                                mess = MessageConstant.ADD_SUCCESS;
                            }
                            else
                            {
                                mess = MessageConstant.ADD_FAILED;
                                log.Error(mess);

                            }
                        }
                    }
                    else
                    {
                        mess = MessageConstant.ADD_FAILED_DATA_EMPTY;
                        log.Error(mess);

                    }
                }
                else
                {
                    mess = MessageConstant.ADD_FAILED_DATA_EMPTY;
                    log.Error(mess);

                }
            }
            catch (Exception e)
            {
                log.Error(e.Message + " at AddSolution");
                mess = e.Message;
                return View("error");
            }
            return SolutionManagement(mess);
        }

        [HttpGet]
        public ActionResult EditSolution(string solID, string mess)
        {
            try
            {
                var solution = _solutionManagementService.GetSolutionByID(solID);
                ViewBag.SolInfo = solution;
                if (ViewBag.SolInfo == null)
                {
                    ViewBag.Mess = "No data";
                    log.Error(ViewBag.Mess);

                    return View("error");
                }
                ViewBag.Mess = mess;
            }
            catch (Exception e)
            {
                log.Error(e.Message + "at Edit Solution");
                ViewBag.Mess = e.Message;
                return View("error");
            }

            return View();

        }
        [HttpPost]
        public async Task<ActionResult> EditSolution(HttpPostedFileBase file)
        {
            string solID;
            try
            {
                solID = Request.Form["id"].ToString();
                string url_img = string.Empty;
                string Title = Request.Form["SolName"].ToString();
                if (!string.IsNullOrEmpty(Title.Trim()))
                {
                    string Detail = Request.Form["des"].ToString();
                    if (!string.IsNullOrEmpty(Detail.Trim()))
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
                            url_img = await CloudinaryUpload(_path, 200, 200);
                        }

                        int result = _solutionManagementService.UpdateSolutionByID(solID, Title, Detail, url_img);
                        if (result > 0)
                        {

                            mess = MessageConstant.UPDATE_SUCCESS;
                            return SolutionManagement(mess);
                        }
                        else
                        {
                            mess = MessageConstant.UPDATE_FAIL;
                            log.Error(mess);
                        }
                    }
                    else
                    {
                        mess = MessageConstant.UPDATE_FAILED_DATA_EMPTY;
                        log.Error(mess);

                    }
                }
                else
                {
                    mess = MessageConstant.UPDATE_FAILED_DATA_EMPTY;
                    log.Error(mess);

                }
            }
            catch (Exception e)
            {
                log.Error(e.Message + " at Edit Solution");
                mess = e.Message;
                return View("error");
            }
            return EditSolution(solID, mess);
        }
        public ActionResult DeleteSolution(string solID)
        {
            try
            {
                int result = _solutionManagementService.DeleteSolutionByID(solID);
                if (result > 0)
                {
                    mess = MessageConstant.DELETE_SUCCESS;
                }
                else
                {
                    mess = MessageConstant.DELETE_FAIL;
                    log.Error(mess);
                }
            }
            catch (Exception e)
            {
                mess = e.Message;
                log.Error(e.Message + "at DeleteSolution ");
                return View("error");
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
            try
            {
                Guid id = Guid.NewGuid();
                string uID = User.Identity.GetUserId();
                string Title = collection["Title"].ToString();
                bool status;
                if (!string.IsNullOrEmpty(Title.Trim()))
                {
                    string Detail = collection["detail"].ToString();
                    if (!string.IsNullOrEmpty(Detail.Trim()))
                    {
                        string Description = collection["Des"];
                        if (!string.IsNullOrEmpty(Description.Trim()))
                        {
                            string txtStatus = collection["Status"];
                            if (!string.IsNullOrEmpty(txtStatus))
                            {

                                if (txtStatus.Equals("Public"))
                                {
                                    bool checkMainExisted = _aboutManagement.CheckMainStatusExisted();
                                    if (checkMainExisted)
                                    {
                                        mess = MessageConstant.ADD_FAILED + MessageConstant.MAIN_EXISTED;
                                        log.Error(mess);
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

                                    mess = MessageConstant.ADD_SUCCESS;
                                }
                                else
                                {
                                    mess = MessageConstant.ADD_FAILED;
                                    log.Error(mess);

                                }
                            }
                            else
                            {
                                mess = MessageConstant.ADD_FAILED_DATA_EMPTY;
                                log.Error(mess);

                            }
                        }
                        else
                        {
                            mess = MessageConstant.ADD_FAILED_DATA_EMPTY;
                            log.Error(mess);

                        }
                    }
                    else
                    {
                        mess = MessageConstant.ADD_FAILED_DATA_EMPTY;
                        log.Error(mess);

                    }
                }
                else
                {
                    mess = MessageConstant.ADD_FAILED_DATA_EMPTY;
                    log.Error(mess);

                }
            }
            catch (Exception e)
            {
                log.Error(e.Message + "at Add About");
                mess = e.Message;
                return View("error");
            }
            return AboutManagement(mess);
        }
        [HttpGet]
        public ActionResult EditAbout(string aboutID, string mess)
        {
            try
            {
                var about = _aboutManagement.getAboutByID(aboutID);
                ViewBag.About = about;
                if (ViewBag.About == null)
                {
                    ViewBag.Mess = "No data";
                    return View("error");
                }
                ViewBag.Mess = mess;
            }
            catch (Exception e)
            {
                log.Error(e.Message + "at Edit About");
                ViewBag.Mess = e.Message;
                return View("error");
            }

            return View("EditAbout");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditAbout(FormCollection collection)
        {
            string aboutID = collection["id"];
            try
            {
                string Title = collection["Title"];
                bool status = false;
                if (!string.IsNullOrEmpty(Title.Trim()))
                {
                    string Detail = collection["detail"];
                    if (!string.IsNullOrEmpty(Detail.Trim()))
                    {
                        string Desc = collection["des"];
                        if (!string.IsNullOrEmpty(Desc))
                        {
                            string txtStatus = collection["Status"];
                            if (txtStatus.Equals("Public"))
                            {
                                bool checkMainExisted = _aboutManagement.CheckMainStatusExisted();
                                if (checkMainExisted)
                                {
                                    mess = MessageConstant.UPDATE_FAIL + MessageConstant.MAIN_EXISTED;
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

                                mess = MessageConstant.UPDATE_SUCCESS;
                                return AboutManagement(mess);
                            }
                            else
                            {
                                mess = MessageConstant.UPDATE_FAIL;
                                log.Error(mess);

                            }
                        }
                        else
                        {
                            mess = MessageConstant.UPDATE_FAILED_DATA_EMPTY;
                            log.Error(mess);

                        }
                    }
                    else
                    {
                        mess = MessageConstant.UPDATE_FAILED_DATA_EMPTY;
                        log.Error(mess);

                    }
                }
                else
                {
                    mess = MessageConstant.UPDATE_FAILED_DATA_EMPTY;
                    log.Error(mess);

                }
            }
            catch (Exception e)
            {
                mess = e.Message;
                log.Error(e.Message + "at Update about");
                return View("error");
            }
            return EditAbout(aboutID, mess);
        }
        public ActionResult DeleteAbout(string aboutID)
        {
            try
            {
                int result = _aboutManagement.deleteAboutByID(aboutID);
                if (result > 0)
                {
                    mess = MessageConstant.DELETE_SUCCESS;
                }
                else
                {
                    mess = MessageConstant.DELETE_FAIL;
                    log.Error(mess);

                }
            }
            catch (Exception e)
            {
                log.Error(e.Message + " at Delete About");
                mess = e.Message;
                return View("error");
            }
            return AboutManagement(mess);
        }
        #endregion
    }
}