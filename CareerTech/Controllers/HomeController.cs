using CareerTech.Services;
using CareerTech.Services.Implement;
using CareerTech.Utils;
using log4net;
using Microsoft.AspNet.Identity;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Payment = PayPal.Api.Payment;

namespace CareerTech.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISubscriptionManagementService<SubscriptionManagementService> _subscriptionManagementService;
        private readonly ISolutionManagementService<SolutionManagementService> _solutionManagementService;
        private readonly IContentService<ContentService> _contentService;
        private readonly IAboutManagement<AboutService> _aboutManagement;
        private readonly IOrderManagementService<OrderManagementService> _orderManagementService;
        private readonly IPartnerManagementService<PartnerManagementService> _partnerManagementService;
        private readonly ILog log = LogManager.GetLogger(typeof(HomeController));

        public HomeController(ISubscriptionManagementService<SubscriptionManagementService> subscriptionManagementService,
            ISolutionManagementService<SolutionManagementService> solutionManagementService,
            IContentService<ContentService> contentService, IAboutManagement<AboutService> aboutManagement,
             IOrderManagementService<OrderManagementService> orderManagementService,
            IPartnerManagementService<PartnerManagementService> partnerManagementService)
        {
            _subscriptionManagementService = subscriptionManagementService;
            _solutionManagementService = solutionManagementService;
            _contentService = contentService;
            _aboutManagement = aboutManagement;
            _orderManagementService = orderManagementService;
            _partnerManagementService = partnerManagementService;
        }
        public ActionResult Index()
        {
            if (User.Identity.GetUserName() != null)
            {
                var username = User.Identity.GetUserName();

                log.Info(username + " is in HomePage");
            }
            else
            {
                log.Info("guest is in HomePage");
            }
            var subscription = _subscriptionManagementService.GetSubscriptions();
            ViewBag.Subs = subscription;
            var solution = _solutionManagementService.GetSolutions();
            ViewBag.Sol = solution;
            var intro = _contentService.GetPublicIntroduction();
            if (intro != null)
            {
                ViewBag.Intro = intro;
            }
            else
            {
                ViewBag.Intro = _contentService.GetIntroduction();
            }
            return View();
        }
        public ActionResult About()
        {
            if (User.Identity.GetUserName() != null)
            {
                var username = User.Identity.GetUserName();
                log.Info(username + " is in About Page");
            }
            else
            {
                log.Info("guest is in About Page");
            }
            var about = _aboutManagement.getMainAbout();
            if (about != null)
            {
                ViewBag.About = about;
            }
            else
            {
                ViewBag.About = _aboutManagement.getAbout();
            }
            var partners = _partnerManagementService.getAllPartners();
            ViewBag.partners = partners;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Authorize(Roles = "Partner")]
        public ActionResult SuccessView()
        {
            var username = User.Identity.GetUserName();
            log4net.GlobalContext.Properties["username"] = username;
            log.Info(username + " is in Success Payment View");
            return View();
        }
        [Authorize(Roles = "Partner")]
        public ActionResult FailView()
        {
            var username = User.Identity.GetUserName();
            log4net.GlobalContext.Properties["username"] = username;
            log.Info(username + " is in Fail Payment View");
            return View();
        }
        #region Paypal
        [Authorize(Roles = "Partner")]
        public ActionResult PaymentWithPaypal(string id)
        {
            //getting the apiContext  
            APIContext apiContext = PayPalService.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Home/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    log.Info("Create Payment");
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, id);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailView");
                    }
                    log.Info("Execute Payment");
                }
                string userId = User.Identity.GetUserId();
                if (Session["order"] != null)
                {
                    Models.Order order = (Models.Order)Session["order"];
                    string orderId = order.ID;
                    DateTime orderDate = order.OrderDate;
                    var subs = _subscriptionManagementService.GetSubscriptionByID(order.SubscriptionID);
                    int period = (int)subs.Period;
                    DateTime dueDate = DateTime.Parse(orderDate.AddMonths(period).ToString("MM/dd/yyyy HH:mm:ss"));
                    var UpdateOrder = _orderManagementService.UpdateOrder(orderId, "Paid");
                    if (UpdateOrder > 0)
                    {
                        //add to TIme
                        string timeId = Guid.NewGuid().ToString();
                        string paymentId = Guid.NewGuid().ToString();
                        bool partnerTimeExisted = _partnerManagementService.PartnerTimeExisted(userId);
                        if (partnerTimeExisted)
                        {
                            int compare = _partnerManagementService.CompareTime(userId);
                            if (compare < 0)
                            {
                                var time = _partnerManagementService.GetPartnerServiceTime(userId);
                                DateTime endDate = time.EndDate.AddMonths(period);
                                _partnerManagementService.UpdateServiceTime(userId, endDate);
                            }
                            else
                            {
                                _partnerManagementService.addServiceTime(timeId, userId, orderDate, dueDate);
                            }
                        }
                        else
                        {
                            _partnerManagementService.addServiceTime(timeId, userId, orderDate, dueDate);
                        }
                        //add  to Payment
                        _orderManagementService.AddPaymentDB(orderId, paymentId);
                    }
                    Session.Remove("order");
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message + " at Payment With PayPal");
                return View("FailView");
            }

            //on successful payment, show success page to user.  
            return View("SuccessView");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl, string id)
        {
            var sub = _subscriptionManagementService.GetSubscriptionByID(id);
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = sub.ID,
                currency = "USD",
                price = sub.Price.ToString(),
                quantity = "1",
                sku = "sku"
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "1",
                shipping = "0",
                subtotal = sub.Price.ToString()
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDouble(details.tax) + Convert.ToDouble(details.shipping) + Convert.ToDouble(details.subtotal)).ToString()
                , // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = Convert.ToString(new Random().Next(1000000000)),
                amount = amount,
                item_list = itemList
            });
            this.payment = new PayPal.Api.Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            string guid = Guid.NewGuid().ToString();
            string userID = User.Identity.GetUserId();
            DateTime orderDate = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
            string status = "Pending";
            double price = Double.Parse(amount.total);
            var result = _orderManagementService.AddOrder(guid, id, userID, orderDate, price, status);
            if (result > 0)
            {
                var order = _orderManagementService.GetOrderByID(guid);
                HttpContext.Session.Add("order", order);
            }

            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }
        #endregion
    }
}