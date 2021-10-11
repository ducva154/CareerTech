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

        Boolean CheckEmailExist(string email,string userId);

        void UpdatePartnerProfile(ApplicationUser partner);

        CompanyProfile GetCompanyProfileByPartnerId(string userId);

        void CreateProfileCompany(CompanyProfile company);

        void UpdateCompany(CompanyProfile company);
    }
}
