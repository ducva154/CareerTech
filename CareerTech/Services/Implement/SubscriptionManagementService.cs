using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerTech.Services.Implement
{
    public class SubscriptionManagementService : ISubscriptionManagementService<SubscriptionManagementService>
    {
        ApplicationDbContext _applicationDbContext { get; set; }

        public SubscriptionManagementService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public int addNewSubscription(Guid id,string Name,float Price,string Type)
        {
            Subscription subscription = new Subscription();
            subscription.ID = id.ToString();
            subscription.Name = Name;
            subscription.Price = Price;
            subscription.Type = Type;
            
            _applicationDbContext.Subscriptions.Add(subscription);
            int row = _applicationDbContext.SaveChanges();
            return row;
        }

        public List<Subscription> GetSubscriptions()
        {
            var query = from s in _applicationDbContext.Subscriptions
                        select s;
            var result = query.ToList();
            return result;
        }

        public int UpdateSubscriptionByID(string subscriptionID, string Name, float Price, string Type)
        {
            var query = from sub in _applicationDbContext.Subscriptions
                        where sub.ID == subscriptionID
                        select sub;
            var subscription = query.FirstOrDefault();
            subscription.Name = Name;
            subscription.Price = Price;
            subscription.Type = Type;
            int row = _applicationDbContext.SaveChanges();
            return row;
        }

        public Subscription GetSubscriptionByID(string subscriptionID)
        {
            var query = from sub in _applicationDbContext.Subscriptions
                        where sub.ID == subscriptionID
                        select sub;
            var subscription = query.FirstOrDefault();
            return subscription;
        }

        public int DeleteSubscriptionByID(string subscriptionID)
        {
            var query = from sub in _applicationDbContext.Subscriptions
                        where sub.ID == subscriptionID
                        select sub;
            var subscription = query.FirstOrDefault();
            _applicationDbContext.Subscriptions.Remove(subscription);
            int result = _applicationDbContext.SaveChanges();
            return result;
        }
    }
}