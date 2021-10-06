using CareerTech.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerTech.Services
{
    public class AccountService
    {
        
        public static IdentityRole GetRoleByEmail(string email)
        {
            var user = new ApplicationDbContext().Users.Where(u => u.Email == email).FirstOrDefault();
            IdentityUserRole userrole = user.Roles.FirstOrDefault();
            return new ApplicationDbContext().Roles.Where(r => r.Id == userrole.RoleId).FirstOrDefault();
        }

    }
}