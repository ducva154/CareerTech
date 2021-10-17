using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerTech.Services.Implement
{
    public class OrderManagementService : IOrderManagementService<OrderManagementService>
    {
        private readonly ApplicationDbContext _applicationDbContext = null;

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
            _applicationDbContext.Orders.Add(order);
            int result = _applicationDbContext.SaveChanges();
            return result;
        }

        public int AddPaymentDB(string orderID, string paymentID)
        {
            Payment payments = new Payment();
            payments.OrderID = orderID;
            payments.ID = paymentID;
            _applicationDbContext.Payments.Add(payments);
            int result = _applicationDbContext.SaveChanges();
            return result;
        }

        public List<Order> getListPendingOrderOfUser(string userID)
        {
            var query = from o in _applicationDbContext.Orders
                        where o.UserID == userID && o.Status == "Pending"
                        select o;
            var listOrder = query.ToList();
            return listOrder;
        }

        public Order GetOrder()
        {
            throw new NotImplementedException();
        }

        public Order getOrderByID(string orderID)
        {
            var query = from o in _applicationDbContext.Orders
                        where o.ID == orderID
                        select o;
            var order = query.FirstOrDefault();
            return order;
        }

        public int UpdateOrder(string orderID, string status)
        {
            var order = getOrderByID(orderID);
            order.Status = status;
            int result = _applicationDbContext.SaveChanges();
            return result;
        }
        public List<orderDetailViewModel> getOrderDetail(string partnerID)
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
            return listOrder;

        }
        public List<PaymentViewModel> getAllOrderDetail()
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
            return listOrder;

        }
    }
}