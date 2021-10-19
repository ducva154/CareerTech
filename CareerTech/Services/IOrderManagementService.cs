using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerTech.Services
{
    public interface IOrderManagementService<T> where T : class
    {
        int AddOrder(string orderID, string subID, string userID, DateTime orderDate, double price, string status);
        List<Order> GetListPendingOrderOfUser(string userID);
        int UpdateOrder(string orderID, string status);
        Order GetOrderByID(string orderID);
        int AddPaymentDB(string orderID, string paymentID);
        List<orderDetailViewModel> GetOrderDetail(string partnerID);
        List<PaymentViewModel> GetAllOrderDetail();
    }
}
