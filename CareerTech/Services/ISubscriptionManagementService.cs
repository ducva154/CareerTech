using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerTech.Services
{
    public interface ISubscriptionManagementService<T> where T:class
    {
        int addNewSubscription(Guid id, string Name, float Price, string Type);
        List<Subscription> GetSubscriptions();
        int UpdateSubscriptionByID(string subscriptionID, string Name, float Price, string Type);
        Subscription GetSubscriptionByID(string subscriptionID);
        int DeleteSubscriptionByID(string subscriptionID);
    }
        
}
