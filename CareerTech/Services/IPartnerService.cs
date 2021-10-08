using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerTech.Services
{
    public interface IPartnerService<T> where T : class
    {
        ApplicationUser GetPartnerByID(string userID);
    }
}
