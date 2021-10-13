using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerTech.Services
{
    public class UserService : IUserService<UserService>
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddEducation(Education education)
        {
            education.ID = Guid.NewGuid().ToString();
            _context.Educations.Add(education);
            _context.SaveChanges();
        }

        public void AddExperience(Experience experience)
        {
            experience.ID = Guid.NewGuid().ToString();
            _context.Experiences.Add(experience);
            _context.SaveChanges();
        }

        public void AddProduct(Product product)
        {
            product.ID = Guid.NewGuid().ToString();
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void AddSkill(Skill skill)
        {
            skill.ID = Guid.NewGuid().ToString();
            _context.Skills.Add(skill);
            _context.SaveChanges();
        }

        public void CreateProfile(Profile profile)
        {
            profile.ID = Guid.NewGuid().ToString();
            _context.Profiles.Add(profile);
            _context.SaveChanges();
        }

        public void DeleteEducation(string educationID)
        {
            var deleteEducation = GetEducationByID(educationID);
            if (deleteEducation != null)
            {
                _context.Educations.Remove(deleteEducation);
                _context.SaveChanges();
            }
        }

        public void DeleteExperience(string experienceID)
        {
            var deleteExperience = GetExperienceByID(experienceID);
            if (deleteExperience != null)
            {
                _context.Experiences.Remove(deleteExperience);
                _context.SaveChanges();
            }
        }

        public void DeletePortfolio(string portfolioID)
        {
            var deletePortfolio = _context.Portfolios.Where(p => p.ID == portfolioID).SingleOrDefault();
            if (deletePortfolio != null)
            {
                _context.Portfolios.Remove(deletePortfolio);
                _context.SaveChanges();
            }
        }

        public void DeleteProduct(string productID)
        {
            var deleteProduct = _context.Products.Where(p => p.ID == productID).SingleOrDefault();
            if (deleteProduct != null)
            {
                _context.Products.Remove(deleteProduct);
                _context.SaveChanges();
            }
        }

        public void DeleteSkill(string skillID)
        {
            var deleteSkill = GetSkillByID(skillID);
            if (deleteSkill != null)
            {
                _context.Skills.Remove(deleteSkill);
                _context.SaveChanges();
            }
        }

        public void EditEducation(string educationID, Education education)
        {
            Education oldEducation = GetEducationByID(educationID);
            oldEducation.Time = education.Time;
            oldEducation.Detail = education.Detail;
            _context.SaveChanges();
        }

        public void EditExperience(string experienceID, Experience experience)
        {
            Experience oldExperience = GetExperienceByID(experienceID);
            oldExperience.Time = experience.Time;
            oldExperience.Detail = experience.Detail;
            _context.SaveChanges();
        }

        public void ChangePortfolioStatus(string portfolioID)
        {
            Portfolio portfolio = GetPortfolioByID(portfolioID);
            portfolio.PublicStatus = !portfolio.PublicStatus;
            _context.SaveChanges();
        }

        public void EditProduct(string productID, Product product)
        {
            Product oldProduct = GetProductByID(productID);
            oldProduct.Name = product.Name;
            oldProduct.Url_Image = product.Url_Image;
            oldProduct.Company = product.Company;
            oldProduct.Domain = product.Domain;
            oldProduct.Skill = product.Skill;
            oldProduct.TeamSize = product.TeamSize;
            oldProduct.ProjectTech = product.ProjectTech;
            oldProduct.WorkProces = product.WorkProces;
            oldProduct.ProjectRole = product.ProjectRole;
            _context.SaveChanges();
        }

        public void EditProfile(Profile profile)
        {
            var portfolio = GetPortfolioByID(profile.PortfolioID);
            var oldProfile = _context.Profiles.Where(p => p.PortfolioID == portfolio.ID).FirstOrDefault();
            oldProfile.Name = profile.Name;
            oldProfile.Url_avatar = profile.Url_avatar;
            oldProfile.Gender = profile.Gender;
            oldProfile.Age = profile.Age;
            oldProfile.Address = profile.Address;
            oldProfile.Position = profile.Position;
            oldProfile.Desc = profile.Desc;
            oldProfile.Phone = profile.Phone;
            oldProfile.Email = profile.Email;
            profile.Facebook_url = profile.Facebook_url;
            profile.Twitter_url = profile.Twitter_url;
            profile.Youtube_url = profile.Youtube_url;
            profile.Instagram_url = profile.Instagram_url;
            _context.SaveChanges();
        }

        public void EditSkill(string skillID, Skill skill)
        {
            Skill oldSkill = GetSkillByID(skillID);
            oldSkill.SkillName = skill.SkillName;
            oldSkill.SkillLevel = skill.SkillLevel;
            _context.SaveChanges();
        }

        public Education GetEducationByID(string educationID)
        {
            return _context.Educations.Where(e => e.ID == educationID).SingleOrDefault();
        }

        public ICollection<Education> GetEducationByPortfolioID(string portfolioID)
        {
            return GetPortfolioByID(portfolioID).Educations.ToList();
        }

        public Experience GetExperienceByID(string experienceID)
        {
            return _context.Experiences.Where(e => e.ID == experienceID).SingleOrDefault();
        }

        public ICollection<Experience> GetExperienceByPortfolioID(string portfolioID)
        {
            return GetPortfolioByID(portfolioID).Experiences.ToList();
        }

        public Portfolio GetPortfolioByID(string portfolioID)
        {
            return _context.Portfolios.Where(p => p.ID == portfolioID).SingleOrDefault();
        }

        public ICollection<Portfolio> GetPortfolioByNameAndUser(string portfolioName, string userID)
        {
            return _context.Portfolios.Where(p => (p.UserID == userID && p.Name == portfolioName)).ToList();
        }

        public ICollection<Portfolio> GetPortfolioByUser(string userID)
        {
            return _context.Portfolios.Where(p => p.UserID == userID).ToList();
        }

        public Product GetProductByID(string productID)
        {
            return _context.Products.Where(p => p.ID == productID).SingleOrDefault();
        }

        public Product GetProductByNameAndPortfolioID(string productName, string portfolioID)
        {
            return _context.Products.Where(p => (p.Name == productName && p.PortfolioID == portfolioID)).SingleOrDefault();
        }

        public ICollection<Product> GetProductByPortfolioID(string portfolioID)
        {
            return GetPortfolioByID(portfolioID).Products.ToList();
        }

        public Profile GetProfileByPortfolioID(string portfolioID)
        {
            return GetPortfolioByID(portfolioID).Profiles.FirstOrDefault();
        }

        public Skill GetSkillByID(string skillID)
        {
            return _context.Skills.Where(s => s.ID == skillID).FirstOrDefault();
        }

        public Skill GetSkillByNameAndPortfolioID(string skillName, string portfolioID)
        {
            return _context.Skills.Where(s => (s.SkillName.Equals(skillName) && s.PortfolioID == portfolioID)).SingleOrDefault();
        }

        public ICollection<Skill> GetSkillByPortfolioID(string portfolioID)
        {
            return GetPortfolioByID(portfolioID).Skills.ToList();
        }

        public void InsertPortfolio(Portfolio portfolio)
        {
            _context.Portfolios.Add(portfolio);
            _context.SaveChanges();
        }
    }
}