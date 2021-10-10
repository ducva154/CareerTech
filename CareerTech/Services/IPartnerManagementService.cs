using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerTech.Services
{
    public interface IPartnerManagementService<T> where T : class
    {
        List<CompanyProfile> getAllPartners();
        int CountNoOfPartners();
    }
}
