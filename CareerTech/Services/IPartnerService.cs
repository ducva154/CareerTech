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

        CompanyProfile GetCompanyProfileById(string companyId);

        CompanyProfile GetCompanyProfileByRecruitmentId(string recruitmentId);

        void CreateProfileCompany(CompanyProfile company);

        void UpdateCompany(CompanyProfile company);

        List<Recruitment> GetListRecruitmentByCompanyID(string companyID);

        void AddRecruitment(Recruitment obj);

        List<Job> GetAllJobCategory();

        Recruitment GetRecruitmentById(string recruitmentId);
        void DeleteRecruitmentByID(string recruitmentId);
        void UpdateRecruitment(Recruitment recruitment);
        List<CandidateViewModel> GetListCandidateByCompanyID(string companyID);
        void DeleteCandidateById(string id);

        void UpdateCandidateByID(string candidateID);
    }
}
