using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerTech.Services.Implement
{
    public class SubscriptionManagementService : ISubscriptionManagementService<SubscriptionManagementService>
    {
        private readonly ApplicationDbContext _applicationDbContext = null;

        public SubscriptionManagementService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public int AddNewSubscription(Guid SubId, string Name, float Price, string Type, int Period, string Desc)
        {
            Subscription subscription = new Subscription();
            subscription.ID = SubId.ToString();
            subscription.Name = Name;
            subscription.Price = Price;
            subscription.Type = Type;
            subscription.Period = Period;
            subscription.DetailDesc = Desc;
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

        public int UpdateSubscriptionByID(string subscriptionID, string Name, float Price, string Type, int Period, string Desc)
        {
            var subscription = GetSubscriptionByID(subscriptionID);
            subscription.Name = Name;
            subscription.Price = Price;
            subscription.Type = Type;
            subscription.Period = Period;
            subscription.DetailDesc = Desc;
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
            var subscription = GetSubscriptionByID(subscriptionID);
            _applicationDbContext.Subscriptions.Remove(subscription);
            int result = _applicationDbContext.SaveChanges();
            return result;
        }
    }
}