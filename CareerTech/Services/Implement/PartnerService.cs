using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerTech.Services.Implement
{
    public class PartnerService : IPartnerService<PartnerService>
    {
        ApplicationDbContext dbContext = null;

        public PartnerService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ApplicationUser GetPartnerByID(String userID)
        {
            return dbContext.Users.Where(u => u.Id.Equals(userID)).FirstOrDefault();
        }
    }
}