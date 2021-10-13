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
        void InsertPortfolio(Portfolio portfolio);

        ICollection<Portfolio> GetPortfolioByUser(string userID);

        ICollection<Portfolio> GetPortfolioByNameAndUser(string portfolioName, string userID);

        Portfolio GetPortfolioByID(string portfolioID);

        void ChangePortfolioStatus(string portfolioID);

        void DeletePortfolio(string portfolioID);

        Profile GetProfileByPortfolioID(string portfolioID);

        void CreateProfile(Profile profile);

        void EditProfile(Profile profile);

        ICollection<Skill> GetSkillByPortfolioID(string portfolioID);

        Skill GetSkillByID(string skillID);

        Skill GetSkillByNameAndPortfolioID(string skillName, string portfolioID);

        void AddSkill(Skill skill);

        void DeleteSkill(string skillID);

        void EditSkill(string skillID, Skill skill);

        ICollection<Education> GetEducationByPortfolioID(string portfolioID);

        Education GetEducationByID(string educationID);

        void AddEducation(Education education);

        void DeleteEducation(string educationID);

        void EditEducation(string educationID, Education education);

        ICollection<Experience> GetExperienceByPortfolioID(string portfolioID);

        Experience GetExperienceByID(string experienceID);

        void AddExperience(Experience experience);

        void DeleteExperience(string experienceID);

        void EditExperience(string experienceID, Experience experience);

        ICollection<Product> GetProductByPortfolioID(string portfolioID);

        Product GetProductByID(string productID);

        Product GetProductByNameAndPortfolioID(string productName, string portfolioID);

        void AddProduct(Product product);

        void DeleteProduct(string productID);

        void EditProduct(string productID, Product product);

    }
}
