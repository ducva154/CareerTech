using CareerTech.Models;
using System.Collections.Generic;
using System.Linq;

namespace CareerTech.Services.Implement
{
    public class PartnerService : IPartnerService<PartnerService>
    {
       private readonly ApplicationDbContext dbContext = null;

        public PartnerService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool CheckEmailExist(string email,string userId)
        {
           List<ApplicationUser> listuser = dbContext.Users.Where(u=> !u.Id.Equals(userId)).ToList();
           ApplicationUser user = listuser.Where(u => u.Email.Equals(email)).FirstOrDefault();
           if (user!=null)
           {
                return false;
           }
           return true;
        }

        public void CreateProfileCompany(CompanyProfile company)
        {
            dbContext.CompanyProfiles.Add(company);
            dbContext.SaveChanges();
        }

        public CompanyProfile GetCompanyProfileByPartnerId(string userId)
        {
            return dbContext.CompanyProfiles.Where(c => c.UserID.Equals(userId)).FirstOrDefault();
        }

        public ApplicationUser GetPartnerByID(string userID)
        {
            return dbContext.Users.Where(u => u.Id.Equals(userID)).FirstOrDefault();
        }

        public void UpdateCompany(CompanyProfile company)
        {
            var companyUpdate = dbContext.CompanyProfiles.Where(c => c.ID == company.ID).FirstOrDefault();
            companyUpdate.CompanyName = company.CompanyName;
            companyUpdate.Address = company.Address;
            companyUpdate.Desc = company.Desc;
            companyUpdate.Url_Avatar = company.Url_Avatar;
            companyUpdate.Url_Background = company.Url_Background;
            companyUpdate.Phone = company.Phone;
            companyUpdate.Email = company.Email;
            dbContext.SaveChanges();
        }

        public void UpdatePartnerProfile(ApplicationUser partner)
        {
            var partnerUpdate = dbContext.Users.Where(u => u.Id == partner.Id).FirstOrDefault();
            partnerUpdate.FullName = partner.FullName;
            partnerUpdate.Email = partner.Email;
            partnerUpdate.PhoneNumber = partner.PhoneNumber;
            dbContext.SaveChanges();
        }

     



    }
}