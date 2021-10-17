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
        Order GetOrder();
        int AddOrder(string orderID, string subID, string userID, DateTime orderDate, double price, string status);
        List<Order> getListPendingOrderOfUser(string userID);
        int UpdateOrder(string orderID, string status);
        Order getOrderByID(string orderID);
        int AddPaymentDB(string orderID, string paymentID);
        List<orderDetailViewModel> getOrderDetail(string partnerID);
        List<PaymentViewModel> getAllOrderDetail();
    }
}
