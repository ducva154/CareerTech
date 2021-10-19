using CareerTech.Models;
using System.Collections.Generic;

namespace CareerTech.Services
{
    public interface IPartnerService<T> where T : class
    {
        ApplicationUser GetPartnerByID(string userID);

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

        int GetServiceTime(string userID);

    }
}
