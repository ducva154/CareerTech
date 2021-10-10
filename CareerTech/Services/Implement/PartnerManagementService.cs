using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerTech.Services.Implement
{
    public class PartnerManagementService : IPartnerManagementService<PartnerManagementService>
    {
        private ApplicationDbContext _applicationDbContext { get; set; }

        public PartnerManagementService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public List<CompanyProfile> getAllPartners()
        {
            var query = from u in _applicationDbContext.Users
                        from ur in u.Roles
                        join r in _applicationDbContext.Roles on ur.RoleId equals r.Id
                        join com in _applicationDbContext.CompanyProfiles on u.Id equals com.UserID
                        where r.Name == "Partner"
                        select com;

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
    }
}