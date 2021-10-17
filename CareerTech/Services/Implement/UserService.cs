using CareerTech.Models;
using CareerTech.Utils;
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

        public int AddEducation(Education education)
        {
            education.ID = Guid.NewGuid().ToString();
            _context.Educations.Add(education);
            return _context.SaveChanges();
        }

        public int AddExperience(Experience experience)
        {
            experience.ID = Guid.NewGuid().ToString();
            _context.Experiences.Add(experience);
            return _context.SaveChanges();
        }

        public int AddProduct(Product product)
        {
            product.ID = Guid.NewGuid().ToString();
            _context.Products.Add(product);
            return _context.SaveChanges();
        }

        public int AddSkill(Skill skill)
        {
            skill.ID = Guid.NewGuid().ToString();
            _context.Skills.Add(skill);
            return _context.SaveChanges();
        }

        public int CreateProfile(Profile profile)
        {
            profile.ID = Guid.NewGuid().ToString();
            _context.Profiles.Add(profile);
            return _context.SaveChanges();
        }

        public int DeleteEducation(string educationID)
        {
            var deleteEducation = GetEducationByID(educationID);
            _context.Educations.Remove(deleteEducation);
            return _context.SaveChanges();
        }

        public int DeleteExperience(string experienceID)
        {
            var deleteExperience = GetExperienceByID(experienceID);
            _context.Experiences.Remove(deleteExperience);
            return _context.SaveChanges();
        }

        public int DeletePortfolio(string portfolioID)
        {
            var deletePortfolio = _context.Portfolios.Where(p => p.ID == portfolioID).SingleOrDefault();
            _context.Portfolios.Remove(deletePortfolio);
            return _context.SaveChanges();
        }

        public int DeleteProduct(string productID)
        {
            var deleteProduct = _context.Products.Where(p => p.ID == productID).SingleOrDefault();
            _context.Products.Remove(deleteProduct);
            return _context.SaveChanges();
        }

        public int DeleteSkill(string skillID)
        {
            var deleteSkill = GetSkillByID(skillID);
            _context.Skills.Remove(deleteSkill);
            return _context.SaveChanges();
        }

        public int EditEducation(string educationID, Education education)
        {
            Education oldEducation = GetEducationByID(educationID);
            oldEducation.Time = education.Time;
            oldEducation.Detail = education.Detail;
            return _context.SaveChanges();
        }

        public int EditExperience(string experienceID, Experience experience)
        {
            Experience oldExperience = GetExperienceByID(experienceID);
            oldExperience.Time = experience.Time;
            oldExperience.Detail = experience.Detail;
            return _context.SaveChanges();
        }

        public void ChangePortfolioStatus(string portfolioID)
        {
            Portfolio portfolio = GetPortfolioByID(portfolioID);
            portfolio.PublicStatus = !portfolio.PublicStatus;
            _context.SaveChanges();
        }

        public int EditProduct(string productID, Product product)
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
            return _context.SaveChanges();
        }

        public int EditProfile(Profile profile)
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
            return _context.SaveChanges();
        }

        public int EditSkill(string skillID, Skill skill)
        {
            Skill oldSkill = GetSkillByID(skillID);
            oldSkill.SkillName = skill.SkillName;
            oldSkill.SkillLevel = skill.SkillLevel;
            return _context.SaveChanges();
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
            return _context.Skills.Where(s => (s.SkillName == skillName && s.PortfolioID == portfolioID)).SingleOrDefault();
        }

        public ICollection<Skill> GetSkillByPortfolioID(string portfolioID)
        {
            return GetPortfolioByID(portfolioID).Skills.ToList();
        }

        public int InsertPortfolio(Portfolio portfolio)
        {
            _context.Portfolios.Add(portfolio);
            return _context.SaveChanges();
        }

        public int EditUserProfile(string userID, ApplicationUser user)
        {
            ApplicationUser oldUser = _context.Users.Where(u => u.Id == userID).SingleOrDefault();
            oldUser.FullName = user.FullName;
            oldUser.PhoneNumber = user.PhoneNumber;
            return _context.SaveChanges();
        }

        public ICollection<Portfolio> GetPublicPortfolioByUser(string userID)
        {
            return GetPortfolioByUser(userID).Where(p => p.PublicStatus == true).ToList();
        }

        public Portfolio GetMainPortfolioByUser(string userID)
        {
            return GetPublicPortfolioByUser(userID).Where(p => p.MainStatus == true).SingleOrDefault();
        }

        public int EditMainStatus(string portfolioID, string userID)
        {
            ICollection<Portfolio> listPortfolio = GetPortfolioByUser(userID);
            foreach (Portfolio portfolio in listPortfolio)
            {
                if (portfolio.ID == portfolioID)
                {
                    portfolio.MainStatus = true;
                }
                else
                {
                    portfolio.MainStatus = false;
                }
            }
            return _context.SaveChanges();
        }

        public List<SearchRecruitmentViewModel> SearchAllRecruiment()
        {
            List<Recruitment> recruitments = _context.Recruitments.ToList();
            List<SearchRecruitmentViewModel> searchRecruitments = new List<SearchRecruitmentViewModel>();
            foreach (Recruitment recruitment in recruitments)
            {
                if (GetCompanyStatusByCompanyProfileID(recruitment.CompanyProfileID).Equals(CommonConstants.APPROVED_STATUS))
                {
                    SearchRecruitmentViewModel searchRecruitment = new SearchRecruitmentViewModel();
                    searchRecruitment.RecruitmentID = recruitment.ID;
                    searchRecruitment.CompanyProfileID = recruitment.CompanyProfileID;
                    CompanyProfile company = _context.CompanyProfiles.Where(c => c.ID.Equals(recruitment.CompanyProfileID)).FirstOrDefault();
                    searchRecruitment.Url_Avatar = company.Url_Avatar;
                    searchRecruitment.Title = recruitment.Title;
                    searchRecruitment.CompanyName = company.CompanyName;
                    searchRecruitment.Address = recruitment.Address;
                    searchRecruitment.Salary = recruitment.Salary;
                    searchRecruitment.JobID = recruitment.JobID;
                    searchRecruitment.JobName = _context.Jobs.Where(j => j.ID == recruitment.JobID).SingleOrDefault().JobName;
                    searchRecruitments.Add(searchRecruitment);
                }
            }
            return searchRecruitments;
        }

        public List<SearchRecruitmentViewModel> SearchRecruimentByFilter(string address, string jobID)
        {
            List<SearchRecruitmentViewModel> list = SearchAllRecruiment();
            if (!string.IsNullOrEmpty(address))
            {
                list = list.Where(i => i.Address.Equals(address)).ToList();
            }
            if (!string.IsNullOrEmpty(jobID))
            {
                list = list.Where(i => i.JobID.Equals(jobID)).ToList();
            }
            return list;
        }

        public int ApplyRecruitment(Candidate candidate)
        {
            _context.Candidates.Add(candidate);
            return _context.SaveChanges();
        }

        public Candidate GetCandidateByUserIDAndRecruitmentID(string userID, string recruitmentID)
        {
            return _context.Candidates.Where(c => (c.UserID == userID && c.RecruitmentID == recruitmentID)).SingleOrDefault();
        }

        public List<DashboardRecruitmentViewModel> GetDashboardRecruitmentByUserIDAndStatus(string userID)
        {
            List<Candidate> candidates = _context.Candidates.Where(c => c.UserID == userID).ToList();
            List<DashboardRecruitmentViewModel> dashboardRecruitments = new List<DashboardRecruitmentViewModel>();
            foreach (Candidate candidate in candidates)
            {
                DashboardRecruitmentViewModel dashboardRecruitment = new DashboardRecruitmentViewModel();
                dashboardRecruitment.RecruitmentID = candidate.RecruitmentID;
                Recruitment recruitment = _context.Recruitments.Where(r => r.ID == candidate.RecruitmentID).SingleOrDefault();
                dashboardRecruitment.Title = recruitment.Title;
                dashboardRecruitment.CompanyProfileID = recruitment.CompanyProfileID;
                CompanyProfile companyProfile = _context.CompanyProfiles.Where(c => c.ID == recruitment.CompanyProfileID).SingleOrDefault();
                dashboardRecruitment.CompanyName = companyProfile.CompanyName;
                dashboardRecruitment.Status = candidate.Status;
                dashboardRecruitments.Add(dashboardRecruitment);
            }
            return dashboardRecruitments;
        }

        public int CountCandidateByUserIDAndStatus(string userID, string status)
        {
            return _context.Candidates.Where(c => (c.UserID == userID && c.Status == status)).Count();
        }

        public string GetCompanyStatusByCompanyProfileID(string companyProfileID)
        {
            CompanyProfile company = _context.CompanyProfiles.Where(c => c.ID.Equals(companyProfileID)).FirstOrDefault();
            return company.Status;
        }
    }
}