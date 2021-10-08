using CareerTech.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerTech.Services.Implement
{
    public class AccountService: IAccountService<AccountService>
    {
        ApplicationDbContext dbContext = null;

        public AccountService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public IdentityRole GetRoleByEmail(string email)
        {
            var user = dbContext.Users.Where(u => u.Email == email).FirstOrDefault();
            IdentityUserRole userrole = user.Roles.FirstOrDefault();
            return dbContext.Roles.Where(r => r.Id == userrole.RoleId).FirstOrDefault();
        }

        public ApplicationUser GetUserByEmail(string email)
        {
           return dbContext.Users.Where(u => u.Email == email).FirstOrDefault(); ;
        }
    }
}