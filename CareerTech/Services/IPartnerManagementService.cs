using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CareerTech.Services
{
    public interface IPartnerManagementService<T> where T : class
    {
        List<PartnerManagementViewModel> getAllPartners();
        int CountNoOfPartners();
        List<ApplicationUser> getPartners();
        int addServiceTime(string id, string userId, DateTime startDate, DateTime endDate);
        Time GetPartnerServiceTime(string userID);
        int CompareTime(string userId);
        CompanyProfile getPartnerByID(string comID);
        int ApprovePartner(string comID);
        Task RejectPartner(string comID);
        int UpdateServiceTime(string userID, DateTime dueDate);
        bool PartnerTimeExisted(string userID);
        List<PartnerManagementViewModel> getPartnersWithService();
    }
}
