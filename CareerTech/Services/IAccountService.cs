using CareerTech.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerTech.Services
{
    public interface IAccountService<T> where T : class
    {
        IdentityRole GetRoleByEmail(string email);

        ApplicationUser GetUserByEmail(string email);
    }
}
