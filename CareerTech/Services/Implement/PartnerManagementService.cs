using CareerTech.Models;
using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static CareerTech.Utils.LogConstants;
namespace CareerTech.Services.Implement
{
    public class PartnerManagementService : IPartnerManagementService<PartnerManagementService>
    {
        private readonly ApplicationDbContext _applicationDbContext = null;
        private readonly ILog log = LogManager.GetLogger(typeof(PartnerManagementService));
        public PartnerManagementService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public List<PartnerManagementViewModel> getAllPartners()
        {
            var query = from u in _applicationDbContext.Users
                        from ur in u.Roles
                        join r in _applicationDbContext.Roles on ur.RoleId equals r.Id
                        join com in _applicationDbContext.CompanyProfiles on u.Id equals com.UserID
                        where r.Name == "Partner"
                        select new PartnerManagementViewModel
                        {
                            UserID = u.Id,
                            UserName = u.FullName,
                            CompanyName = com.CompanyName,
                            CompanyID = com.ID,
                            Phone = u.PhoneNumber,
                            Email = u.Email,
                            Status = com.Status,
                            Url_Img = com.Url_Avatar,
                            Address = com.Address
                        };
            log.Info(LOG_GET_PARTNERWITHCOMPANY);
            var result = query.ToList();
            return result;
        }

        public int CountNoOfPartners()
        {
            var query = from u in _applicationDbContext.Users
                        from ur in u.Roles
                        join r in _applicationDbContext.Roles on ur.RoleId equals r.Id
                        where r.Name == "Partner"
                        select u;
            log.Info(LOG_NUMBER_OF_USER);
            return query.Count();
        }

        public List<ApplicationUser> getPartners()
        {
            var query = from u in _applicationDbContext.Users
                        from ur in u.Roles
                        join r in _applicationDbContext.Roles on ur.RoleId equals r.Id
                        where r.Name == "Partner"
                        select u;
            log.Info(LOG_GET_PARTNER);
            return query.ToList();
        }
        public int addServiceTime(string id, string userId, DateTime startDate, DateTime endDate)
        {
            Time serviceTime = new Time();
            serviceTime.ID = id;
            serviceTime.UserID = userId;
            serviceTime.StartDate = startDate;
            serviceTime.EndDate = endDate;
            _applicationDbContext.Times.Add(serviceTime);
            log.Info($"{LOG_ADD_SERVICE_TIME}: id:{serviceTime.ID},userID:{serviceTime.UserID},startDate:{serviceTime.StartDate},endDate:{serviceTime.EndDate}");
            int result = _applicationDbContext.SaveChanges();
            return result;
        }

        public Time GetPartnerServiceTime(string userID)
        {
            var query = from t in _applicationDbContext.Times
                        where t.UserID == userID
                        select t;
            var partnerTime = query.FirstOrDefault();
            if (partnerTime != null)
            {
                log.Info($"{LOG_GET_SERVICE_TIME}: id:{partnerTime.ID},userID:{partnerTime.UserID},startDate:{partnerTime.StartDate},endDate:{partnerTime.EndDate}");

            }
            else
            {
                log.Error(LOG_NULL_VALUE);
            }
            return partnerTime;
        }

        public int CompareTime(string userId)
        {
            var serviceTime = GetPartnerServiceTime(userId);
            int result = DateTime.Compare(DateTime.Now, serviceTime.EndDate);
            //result > 0 => datetime.now > endDate
            //result = 0 => datetime.now = endDate
            //result < 0 => datetime.now < endDate
            log.Info(LOG_COMPARE_ENDDATE_NOW);
            return result;
        }

        public CompanyProfile getPartnerByID(string comID)
        {
            var query = from p in _applicationDbContext.CompanyProfiles
                        where p.ID == comID
                        select p;
            var partner = query.FirstOrDefault();
            if (partner != null)
            {
                log.Info($"{LOG_GET_PARTNERWITHCOMPANY_BYID}: id{partner.UserID},comID:{partner.ID},companyName{partner.CompanyName}");
            }
            else
            {
                log.Error(LOG_NULL_VALUE);
            }
            return partner;
        }

        public int ApprovePartner(string comID)
        {
            var com = getPartnerByID(comID);
            com.Status = "Approved";
            log.Info($"{APPROVE_PARTNER}: id{com.UserID},comID:{com.ID},companyName{com.CompanyName}");
            int result = _applicationDbContext.SaveChanges();
            return result;
        }

        public async Task RejectPartner(string comID)
        {
            var com = getPartnerByID(comID);
            com.Status = "Pending";
            log.Info($"{REJECT_PARTNER}: id{com.UserID},comID:{com.ID},companyName{com.CompanyName}");
            await _applicationDbContext.SaveChangesAsync();
        }

        public int UpdateServiceTime(string userID, DateTime dueDate)
        {
            var time = GetPartnerServiceTime(userID);
            time.EndDate = dueDate;
            int result = _applicationDbContext.SaveChanges();
            return result;
        }

        public bool PartnerTimeExisted(string userID)
        {
            var time = GetPartnerServiceTime(userID);
            if (time != null)
            {
                log.Info(PARTNER_TIME_EXISED);
                return true;
            }
            else
            {
                return false;
            }
        }


        public List<PartnerManagementViewModel> getPartnersWithService()
        {
            var query = from u in _applicationDbContext.Users
                        join com in _applicationDbContext.CompanyProfiles on u.Id equals com.UserID
                        join time in _applicationDbContext.Times on u.Id equals time.UserID
                        select new PartnerManagementViewModel
                        {
                            UserID = u.Id,
                            UserName = u.FullName,
                            CompanyName = com.CompanyName,
                            CompanyID = com.ID,
                            Phone = u.PhoneNumber,
                            Email = u.Email,
                            Status = com.Status,
                            Url_Img = com.Url_Avatar,
                            Address = com.Address,
                            startDate = time.StartDate,
                            endDate = time.EndDate
                        };
            log.Info(GET_ALL_PARTNER_WITH_SERVICE);
            return query.ToList();
        }
    }
}