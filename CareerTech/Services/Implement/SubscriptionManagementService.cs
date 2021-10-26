using CareerTech.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static CareerTech.Utils.LogConstants;
namespace CareerTech.Services.Implement
{
    public class SubscriptionManagementService : ISubscriptionManagementService<SubscriptionManagementService>
    {
        private readonly ApplicationDbContext _applicationDbContext = null;
        private readonly ILog log = LogManager.GetLogger(typeof(SubscriptionManagementService));

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
            log.Info($"{LOG_ADD_SUBSCRIPTION}: id:{subscription.ID},name:{subscription.Name}, price:{subscription.Price},type:{subscription.Type},period:{subscription.Period}");
            _applicationDbContext.Subscriptions.Add(subscription);
            int row = _applicationDbContext.SaveChanges();
            return row;
        }

        public List<Subscription> GetSubscriptions()
        {
            var query = from s in _applicationDbContext.Subscriptions
                        select s;
            var result = query.ToList();
            log.Info(LOG_GET_LIST_SUBSCRIPTION);
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
            log.Info($"{LOG_EDIT_SUBSCRIPTION}: id:{subscription.ID},name:{subscription.Name}, price:{subscription.Price},type:{subscription.Type},period:{subscription.Period}");
            int row = _applicationDbContext.SaveChanges();
            return row;
        }

        public Subscription GetSubscriptionByID(string subscriptionID)
        {
            var query = from sub in _applicationDbContext.Subscriptions
                        where sub.ID == subscriptionID
                        select sub;
            var subscription = query.FirstOrDefault();
            if (subscription != null)
            {
                log.Info($"{LOG_GET_SUBSCIPTION_BYID}: id:{subscription.ID},name:{subscription.Name}, price:{subscription.Price},type:{subscription.Type},period:{subscription.Period}");

            }
            else
            {
                log.Error(LOG_NULL_VALUE);
            }
            return subscription;
        }

        public int DeleteSubscriptionByID(string subscriptionID)
        {
            var subscription = GetSubscriptionByID(subscriptionID);
            log.Info($"{LOG_DELETE_SUBSCRIPTION}: id:{subscription.ID},name:{subscription.Name}, price:{subscription.Price},type:{subscription.Type},period:{subscription.Period}");
            _applicationDbContext.Subscriptions.Remove(subscription);
            int result = _applicationDbContext.SaveChanges();
            return result;
        }
    }
}