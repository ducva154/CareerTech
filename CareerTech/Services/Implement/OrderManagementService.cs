using CareerTech.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static CareerTech.Utils.LogConstants;

namespace CareerTech.Services.Implement
{
    public class OrderManagementService : IOrderManagementService<OrderManagementService>
    {
        private readonly ApplicationDbContext _applicationDbContext = null;
        private readonly ILog log = LogManager.GetLogger(typeof(OrderManagementService));
        public OrderManagementService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public int AddOrder(string orderID, string subID, string userID, DateTime orderDate, double price, string status)
        {
            Order order = new Order();
            order.ID = orderID;
            order.SubscriptionID = subID;
            order.UserID = userID;
            order.OrderDate = orderDate;
            order.TotalPrice = price;
            order.Status = status;
            log.Info($"{LOG_ADD_ORDER}: id: {order.ID},subID:{order.SubscriptionID},orderDate:{order.OrderDate},Price:{order.TotalPrice}");
            _applicationDbContext.Orders.Add(order);
            int result = _applicationDbContext.SaveChanges();
            return result;
        }

        public int AddPaymentDB(string orderID, string paymentID)
        {
            Payment payments = new Payment();
            payments.OrderID = orderID;
            payments.ID = paymentID;
            log.Info($"{LOG_ADD_PAYMENT}: id: {payments.ID},orderid: {payments.OrderID}");
            _applicationDbContext.Payments.Add(payments);
            int result = _applicationDbContext.SaveChanges();
            return result;
        }

        public List<Order> GetListPendingOrderOfUser(string userID)
        {
            var query = from o in _applicationDbContext.Orders
                        where o.UserID == userID && o.Status == "Pending"
                        select o;
            var listOrder = query.ToList();
            log.Info(LOG_GET_LIST_PENDING_ORDER);
            return listOrder;
        }


        public Order GetOrderByID(string orderID)
        {
            var query = from o in _applicationDbContext.Orders
                        where o.ID == orderID
                        select o;
            var order = query.FirstOrDefault();
            if (order != null)
            {
                log.Info($"{LOG_GET_ORDER_BYID}: id: {order.ID},subID:{order.SubscriptionID},orderDate:{order.OrderDate},Price:{order.TotalPrice}");

            }
            else
            {
                log.Error(LOG_NULL_VALUE);

            }
            return order;
        }

        public int UpdateOrder(string orderID, string status)
        {
            var order = GetOrderByID(orderID);
            order.Status = status;
            log.Info($"{LOG_EDIT_ORDER}: id: {order.ID},subID:{order.SubscriptionID},orderDate:{order.OrderDate},Price:{order.TotalPrice}");
            int result = _applicationDbContext.SaveChanges();
            return result;
        }
        public List<orderDetailViewModel> GetOrderDetail(string partnerID)
        {
            var query = from u in _applicationDbContext.Users
                        join order in _applicationDbContext.Orders on u.Id equals order.UserID
                        where u.Id == partnerID
                        select new orderDetailViewModel
                        {
                            UserID = u.Id,
                            UserName = u.FullName,
                            Email = u.Email,
                            Phone = u.PhoneNumber,
                            SubscriptionName = order.Subscription.Name,
                            OrderDate = order.OrderDate,
                            Status = order.Status,
                            TotalPrice = order.TotalPrice
                        };
            var listOrder = query.ToList();
            log.Info(LOG_GET_ORDER_DETAIL_BYUSERID);
            return listOrder;

        }
        public List<PaymentViewModel> GetAllOrderDetail()
        {
            var query = from u in _applicationDbContext.Users
                        join order in _applicationDbContext.Orders on u.Id equals order.UserID
                        select new PaymentViewModel
                        {
                            UserID = u.Id,
                            UserName = u.FullName,
                            Email = u.Email,
                            Phone = u.PhoneNumber,
                            SubscriptionName = order.Subscription.Name,
                            OrderDate = order.OrderDate,
                            TotalPrice = order.TotalPrice
                        };
            var listOrder = query.ToList();
            log.Info(LOG_GET_LIST_ORDER_DETAIL);
            return listOrder;

        }
    }
}