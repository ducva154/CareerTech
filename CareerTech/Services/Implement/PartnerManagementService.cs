using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerTech.Services.Implement
{
    public class PartnerManagementService : IPartnerManagementService<PartnerManagementService>
    {
        private readonly ApplicationDbContext _applicationDbContext = null;

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

            var rs = query.ToList();
            return rs;
        }

        public int CountNoOfPartners()
        {
            var query = from u in _applicationDbContext.Users
                        from ur in u.Roles
                        join r in _applicationDbContext.Roles on ur.RoleId equals r.Id
                        where r.Name == "Partner"
                        select u;
            return query.Count();
        }

        public List<ApplicationUser> getPartners()
        {
            var query = from u in _applicationDbContext.Users
                        from ur in u.Roles
                        join r in _applicationDbContext.Roles on ur.RoleId equals r.Id
                        where r.Name == "Partner"
                        select u;
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
            int result = _applicationDbContext.SaveChanges();
            return result;
        }

        public Time GetPartnerServiceTime(string userID)
        {
            var query = from t in _applicationDbContext.Times
                        where t.UserID == userID
                        select t;
            var partnerTime = query.FirstOrDefault();
            return partnerTime;
        }

        public int CompareTime(string userId)
        {
            var serviceTime = GetPartnerServiceTime(userId);
            int result = DateTime.Compare(DateTime.Now, serviceTime.EndDate);
            //result > 0 => datetime.now > endDate
            //result = 0 => datetime.now = endDate
            //result < 0 => datetime.now < endDate
            return result;
        }

        public CompanyProfile getPartnerByID(string comID)
        {
            var query = from p in _applicationDbContext.CompanyProfiles
                        where p.ID == comID
                        select p;
            var partner = query.FirstOrDefault();
            return partner;
        }

        public int ApprovePartner(string comID)
        {
            var com = getPartnerByID(comID);
            com.Status = "Approved";
            int result = _applicationDbContext.SaveChanges();
            return result;
        }

        public int RejectPartner(string comID)
        {
            var com = getPartnerByID(comID);
            com.Status = "Pending";
            int result = _applicationDbContext.SaveChanges();
            return result;
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
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}