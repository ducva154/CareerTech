using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerTech.Services
{
    public interface IUserService<T> where T : class
    {
        int InsertPortfolio(Portfolio portfolio);

        ICollection<Portfolio> GetPortfolioByUser(string userID);

        ICollection<Portfolio> GetPublicPortfolioByUser(string userID);

        Portfolio GetMainPortfolioByUser(string userID);

        ICollection<Portfolio> GetPortfolioByNameAndUser(string portfolioName, string userID);

        Portfolio GetPortfolioByID(string portfolioID);

        void ChangePortfolioStatus(string portfolioID);

        int DeletePortfolio(string portfolioID);

        Profile GetProfileByPortfolioID(string portfolioID);

        int CreateProfile(Profile profile);

        int EditProfile(Profile profile);

        ICollection<Skill> GetSkillByPortfolioID(string portfolioID);

        Skill GetSkillByID(string skillID);

        Skill GetSkillByNameAndPortfolioID(string skillName, string portfolioID);

        int AddSkill(Skill skill);

        int DeleteSkill(string skillID);

        int EditSkill(string skillID, Skill skill);

        ICollection<Education> GetEducationByPortfolioID(string portfolioID);

        Education GetEducationByID(string educationID);

        int AddEducation(Education education);

        int DeleteEducation(string educationID);

        int EditEducation(string educationID, Education education);

        ICollection<Experience> GetExperienceByPortfolioID(string portfolioID);

        Experience GetExperienceByID(string experienceID);

        int AddExperience(Experience experience);

        int DeleteExperience(string experienceID);

        int EditExperience(string experienceID, Experience experience);

        ICollection<Product> GetProductByPortfolioID(string portfolioID);

        Product GetProductByID(string productID);

        Product GetProductByNameAndPortfolioID(string productName, string portfolioID);

        int AddProduct(Product product);

        int DeleteProduct(string productID);

        int EditProduct(string productID, Product product);

        int EditUserProfile(string userID, ApplicationUser user);

        int EditMainStatus(string portfolioID, string userID);

        List<SearchRecruitmentViewModel> SearchAllRecruiment();

        List<SearchRecruitmentViewModel> SearchRecruimentByFilter(string address, string jobID);

        int ApplyRecruitment(Candidate candidate);

        Candidate GetCandidateByUserIDAndRecruitmentID(string userID, string recruitmentID);

        List<DashboardRecruitmentViewModel> GetDashboardRecruitmentByUserIDAndStatus(string userID);

        int CountCandidateByUserIDAndStatus(string userID, string status);

        string GetCompanyStatusByCompanyProfileID(string companyProfileID);
    }
}
