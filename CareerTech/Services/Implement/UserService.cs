using CareerTech.Models;
using CareerTech.Utils;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CareerTech.Services
{
    public class UserService : IUserService<UserService>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILog log = LogManager.GetLogger(typeof(UserService));
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int AddEducation(Education education)
        {
            education.ID = Guid.NewGuid().ToString();
            _context.Educations.Add(education);
            log.Info($"{LogConstants.LOG_ADD_EDUCATION}: id: {education.ID}, time: {education.Time}, detail: {education.Detail}");
            return _context.SaveChanges();
        }

        public int AddExperience(Experience experience)
        {
            experience.ID = Guid.NewGuid().ToString();
            _context.Experiences.Add(experience);
            log.Info($"{LogConstants.LOG_ADD_EXPERIENCE}: id: {experience.ID}, time: {experience.Time}, detail: {experience.Detail}");
            return _context.SaveChanges();
        }

        public int AddProduct(Product product)
        {
            product.ID = Guid.NewGuid().ToString();
            _context.Products.Add(product);
            log.Info($"{LogConstants.LOG_ADD_PRODUCT}: id: {product.ID}, portfolioId: {product.PortfolioID}, " +
                $"image: {product.Url_Image}, name: {product.Name}, skill: {product.Skill}, domain: {product.Domain}, " +
                $"teamSize: {product.TeamSize}, projectTech: {product.ProjectTech}, workProcess: {product.WorkProces}, " +
                $"company: {product.Company}, projectRole: {product.ProjectRole}");
            return _context.SaveChanges();
        }

        public int AddSkill(Skill skill)
        {
            skill.ID = Guid.NewGuid().ToString();
            _context.Skills.Add(skill);
            log.Info($"{LogConstants.LOG_ADD_SKILL}: id: {skill.ID}, skillName: {skill.SkillName}, skillLevel: {skill.SkillLevel}");
            return _context.SaveChanges();
        }

        public int CreateProfile(Profile profile)
        {
            profile.ID = Guid.NewGuid().ToString();
            _context.Profiles.Add(profile);
            log.Info($"{LogConstants.LOG_CREATE_PROFILE_PORTFOLIO}: id: {profile.ID}, portfolioID: {profile.PortfolioID}, name: {profile.Name}, " +
                $"position: {profile.Position}, desc: {profile.Desc}, address: {profile.Address}, age: {profile.Age}, gender: {profile.Gender}, " +
                $"phone: {profile.Phone}, email: {profile.Email}, avatar: {profile.Url_avatar}, facebook: {profile.Facebook_url}, " +
                $"instagram: {profile.Instagram_url}, youtube: {profile.Youtube_url}, twitter: {profile.Twitter_url}");
            return _context.SaveChanges();
        }

        public int DeleteEducation(string educationID)
        {
            var deleteEducation = GetEducationByID(educationID);
            _context.Educations.Remove(deleteEducation);
            log.Info($"{LogConstants.LOG_DELETE_EDUCATION}: id: {deleteEducation.ID}, time: {deleteEducation.Time}, detail: {deleteEducation.Detail}");
            return _context.SaveChanges();
        }

        public int DeleteExperience(string experienceID)
        {
            var deleteExperience = GetExperienceByID(experienceID);
            _context.Experiences.Remove(deleteExperience);
            log.Info($"{LogConstants.LOG_DELETE_EXPERIENCE}: id: {deleteExperience.ID}, time: {deleteExperience.Time}, detail: {deleteExperience.Detail}");
            return _context.SaveChanges();
        }

        public int DeletePortfolio(string portfolioID)
        {
            var deletePortfolio = _context.Portfolios.Where(p => p.ID == portfolioID).SingleOrDefault();
            _context.Portfolios.Remove(deletePortfolio);
            log.Info($"{LogConstants.LOG_DELETE_PORTFOLIO}: id: {deletePortfolio.ID}, name: {deletePortfolio.Name}");
            return _context.SaveChanges();
        }

        public int DeleteProduct(string productID)
        {
            var deleteProduct = _context.Products.Where(p => p.ID == productID).SingleOrDefault();
            _context.Products.Remove(deleteProduct);
            log.Info($"{LogConstants.LOG_DELETE_PRODUCT}: id: {deleteProduct.ID}, portfolioId: {deleteProduct.PortfolioID}, " +
                $"image: {deleteProduct.Url_Image}, name: {deleteProduct.Name}, skill: {deleteProduct.Skill}, domain: {deleteProduct.Domain}, " +
                $"teamSize: {deleteProduct.TeamSize}, projectTech: {deleteProduct.ProjectTech}, workProcess: {deleteProduct.WorkProces}, " +
                $"company: {deleteProduct.Company}, projectRole: {deleteProduct.ProjectRole}");
            return _context.SaveChanges();
        }

        public int DeleteSkill(string skillID)
        {
            var deleteSkill = GetSkillByID(skillID);
            _context.Skills.Remove(deleteSkill);
            log.Info($"{LogConstants.LOG_DELETE_SKILL}: id: {deleteSkill.ID}, skillName: {deleteSkill.SkillName}, skillLevel: {deleteSkill.SkillLevel}");
            return _context.SaveChanges();
        }

        public int EditEducation(string educationID, Education education)
        {
            Education oldEducation = GetEducationByID(educationID);
            oldEducation.Time = education.Time;
            oldEducation.Detail = education.Detail;
            log.Info($"{LogConstants.LOG_EDIT_EDUCATION}: id: {oldEducation.ID}, time: {oldEducation.Time}, detail: {oldEducation.Detail}");
            return _context.SaveChanges();
        }

        public int EditExperience(string experienceID, Experience experience)
        {
            Experience oldExperience = GetExperienceByID(experienceID);
            oldExperience.Time = experience.Time;
            oldExperience.Detail = experience.Detail;
            log.Info($"{LogConstants.LOG_EDIT_EXPERIENCE}: id: {oldExperience.ID}, time: {oldExperience.Time}, detail: {oldExperience.Detail}");
            return _context.SaveChanges();
        }

        public void ChangePortfolioStatus(string portfolioID)
        {
            Portfolio portfolio = GetPortfolioByID(portfolioID);
            portfolio.PublicStatus = !portfolio.PublicStatus;
            log.Info($"{LogConstants.LOG_CHANGE_PUBLIC_STATUS}: id: {portfolio.ID}, name: {portfolio.Name}, publicStatus: {portfolio.PublicStatus}");
            _context.SaveChanges();
            log.Info(LogConstants.LOG_SUCCESS);
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
            log.Info($"{LogConstants.LOG_EDIT_PRODUCT}: id: {oldProduct.ID}, portfolioId: {oldProduct.PortfolioID}, " +
                $"image: {oldProduct.Url_Image}, name: {oldProduct.Name}, skill: {oldProduct.Skill}, domain: {oldProduct.Domain}, " +
                $"teamSize: {oldProduct.TeamSize}, projectTech: {oldProduct.ProjectTech}, workProcess: {oldProduct.WorkProces}, " +
                $"company: {oldProduct.Company}, projectRole: {oldProduct.ProjectRole}");
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
            log.Info($"{LogConstants.LOG_EDIT_PROFILE_PORTFOLIO}: id: {oldProfile.ID}, portfolioID: {oldProfile.PortfolioID}, name: {oldProfile.Name}, " +
                $"position: {oldProfile.Position}, desc: {oldProfile.Desc}, address: {oldProfile.Address}, age: {oldProfile.Age}, gender: {oldProfile.Gender}, " +
                $"phone: {oldProfile.Phone}, email: {oldProfile.Email}, avatar: {oldProfile.Url_avatar}, facebook: {oldProfile.Facebook_url}, " +
                $"instagram: {oldProfile.Instagram_url}, youtube: {oldProfile.Youtube_url}, twitter: {oldProfile.Twitter_url}");
            return _context.SaveChanges();
        }

        public int EditSkill(string skillID, Skill skill)
        {
            Skill oldSkill = GetSkillByID(skillID);
            oldSkill.SkillName = skill.SkillName;
            oldSkill.SkillLevel = skill.SkillLevel;
            log.Info($"{LogConstants.LOG_EDIT_SKILL}: id: {oldSkill.ID}, name: {oldSkill.SkillName}, level: {oldSkill.SkillLevel}");
            return _context.SaveChanges();
        }

        public Education GetEducationByID(string educationID)
        {
            var education = _context.Educations.Where(e => e.ID == educationID).SingleOrDefault();
            if (education != null)
            {
                log.Info($"{LogConstants.LOG_GET_EDUCATION}: id: {education.ID}, time: {education.Time}, detail: {education.Detail}");
            }
            else
            {
                log.Info($"{LogConstants.LOG_GET_EDUCATION}: null");
            }
            return education;
        }

        public ICollection<Education> GetEducationByPortfolioID(string portfolioID)
        {
            ICollection<Education> educations = GetPortfolioByID(portfolioID).Educations.ToList();
            log.Info($"{LogConstants.LOG_GET_LIST_EDUCATION} by portfolioId: {portfolioID}");
            return educations;
        }

        public Experience GetExperienceByID(string experienceID)
        {
            var experience = _context.Experiences.Where(e => e.ID == experienceID).SingleOrDefault();
            if (experience != null)
            {
                log.Info($"{LogConstants.LOG_GET_EXPERIENCE}: id: {experience.ID}, time: {experience.Time}, detail: {experience.Detail}");
            }
            else
            {
                log.Info($"{LogConstants.LOG_GET_EXPERIENCE}: null");
            }
            return experience;
        }

        public ICollection<Experience> GetExperienceByPortfolioID(string portfolioID)
        {
            ICollection<Experience> experiences = GetPortfolioByID(portfolioID).Experiences.ToList();
            log.Info($"{LogConstants.LOG_GET_LIST_EXPERIENCE} by portfolioId: {portfolioID}");
            return experiences;
        }

        public Portfolio GetPortfolioByID(string portfolioID)
        {
            var portfolio = _context.Portfolios.Where(p => p.ID == portfolioID).SingleOrDefault();
            if (portfolio != null)
            {
                log.Info($"{LogConstants.LOG_GET_PORTFOLIO}: id: {portfolio.ID}, name: {portfolio.Name}, publicStatus: {portfolio.PublicStatus}, mainStatus: {portfolio.MainStatus}");
            }
            else
            {
                log.Info($"{LogConstants.LOG_GET_PORTFOLIO}: null");
            }
            return portfolio;
        }

        public ICollection<Portfolio> GetPortfolioByNameAndUser(string portfolioName, string userID)
        {
            ICollection<Portfolio> portfolios = _context.Portfolios.Where(p => (p.UserID == userID && p.Name == portfolioName)).ToList();
            log.Info($"{LogConstants.LOG_GET_LIST_PORTFOLIO} by portfolioName: {portfolioName} and userId: {userID}");
            return portfolios;
        }

        public ICollection<Portfolio> GetPortfolioByUser(string userID)
        {
            ICollection<Portfolio> portfolios = _context.Portfolios.Where(p => p.UserID == userID).ToList();
            log.Info($"{LogConstants.LOG_GET_LIST_PORTFOLIO} by userId: {userID}");
            return portfolios;
        }

        public Product GetProductByID(string productID)
        {
            var product = _context.Products.Where(p => p.ID == productID).SingleOrDefault();
            if (product != null)
            {
                log.Info($"{LogConstants.LOG_GET_PRODUCT}: id: {product.ID}, portfolioId: {product.PortfolioID}, " +
                $"image: {product.Url_Image}, name: {product.Name}, skill: {product.Skill}, domain: {product.Domain}, " +
                $"teamSize: {product.TeamSize}, projectTech: {product.ProjectTech}, workProcess: {product.WorkProces}, " +
                $"company: {product.Company}, projectRole: {product.ProjectRole}");
            }
            else
            {
                log.Info($"{LogConstants.LOG_GET_PRODUCT}: null");
            }
            return product;
        }

        public Product GetProductByNameAndPortfolioID(string productName, string portfolioID)
        {
            Product product = _context.Products.Where(p => (p.Name == productName && p.PortfolioID == portfolioID)).SingleOrDefault();
            if (product != null)
            {
                log.Info($"{LogConstants.LOG_GET_PRODUCT}: id: {product.ID}, portfolioId: {product.PortfolioID}, " +
                $"image: {product.Url_Image}, name: {product.Name}, skill: {product.Skill}, domain: {product.Domain}, " +
                $"teamSize: {product.TeamSize}, projectTech: {product.ProjectTech}, workProcess: {product.WorkProces}, " +
                $"company: {product.Company}, projectRole: {product.ProjectRole}");
            }
            else
            {
                log.Info($"{LogConstants.LOG_GET_PRODUCT}: null");
            }
            return product;
        }

        public ICollection<Product> GetProductByPortfolioID(string portfolioID)
        {
            ICollection<Product> products = GetPortfolioByID(portfolioID).Products.ToList();
            log.Info($"{LogConstants.LOG_GET_LIST_PRODUCT} by portfolioId: {portfolioID}");
            return products;
        }

        public Profile GetProfileByPortfolioID(string portfolioID)
        {
            Profile profile = GetPortfolioByID(portfolioID).Profiles.FirstOrDefault();
            if (profile != null)
            {
                log.Info($"{LogConstants.LOG_GET_PROFILE_PORTFOLIO}: id: {profile.ID}, portfolioID: {profile.PortfolioID}, name: {profile.Name}, " +
                $"position: {profile.Position}, desc: {profile.Desc}, address: {profile.Address}, age: {profile.Age}, gender: {profile.Gender}, " +
                $"phone: {profile.Phone}, email: {profile.Email}, avatar: {profile.Url_avatar}, facebook: {profile.Facebook_url}, " +
                $"instagram: {profile.Instagram_url}, youtube: {profile.Youtube_url}, twitter: {profile.Twitter_url}");
            }
            else
            {
                log.Info($"{LogConstants.LOG_GET_PROFILE_PORTFOLIO}: null");
            }
            return profile;
        }

        public Skill GetSkillByID(string skillID)
        {
            Skill skill = _context.Skills.Where(s => s.ID == skillID).FirstOrDefault();
            if (skill != null)
            {
                log.Info($"{LogConstants.LOG_GET_SKILL}: id: {skill.ID}, name: {skill.SkillName}, level: {skill.SkillLevel}");
            }
            else
            {
                log.Info($"{LogConstants.LOG_GET_SKILL}: null");
            }
            return skill;
        }

        public Skill GetSkillByNameAndPortfolioID(string skillName, string portfolioID)
        {
            Skill skill = _context.Skills.Where(s => (s.SkillName == skillName && s.PortfolioID == portfolioID)).SingleOrDefault();
            if (skill != null)
            {
                log.Info($"{LogConstants.LOG_GET_SKILL}: id: {skill.ID}, name: {skill.SkillName}, level: {skill.SkillLevel}");
            }
            else
            {
                log.Info($"{LogConstants.LOG_GET_SKILL}: null");
            }
            return skill;
        }

        public ICollection<Skill> GetSkillByPortfolioID(string portfolioID)
        {
            ICollection<Skill> skills = GetPortfolioByID(portfolioID).Skills.ToList();
            log.Info($"{LogConstants.LOG_GET_LIST_SKILL} by portfolioId: {portfolioID}");
            return skills;
        }

        public int InsertPortfolio(Portfolio portfolio)
        {
            _context.Portfolios.Add(portfolio);
            log.Info($"{LogConstants.LOG_CREATE_PORTFOLIO}: id: {portfolio.ID}, name: {portfolio.Name}, publicStatus: {portfolio.PublicStatus}, mainStatus: {portfolio.MainStatus}");
            return _context.SaveChanges();
        }

        public int EditUserProfile(string userID, ApplicationUser user)
        {
            ApplicationUser oldUser = _context.Users.Where(u => u.Id == userID).SingleOrDefault();
            oldUser.FullName = user.FullName;
            oldUser.PhoneNumber = user.PhoneNumber;
            log.Info($"{LogConstants.LOG_EDIT_USER_PROFILE}: userId: {oldUser.Id}, email: {oldUser.Email}, fullName: {oldUser.FullName}, phone: {oldUser.PhoneNumber}");
            return _context.SaveChanges();
        }

        public ICollection<Portfolio> GetPublicPortfolioByUser(string userID)
        {
            ICollection<Portfolio> portfolios = GetPortfolioByUser(userID).Where(p => p.PublicStatus == true).ToList();
            log.Info($"{LogConstants.LOG_GET_LIST_PORTFOLIO} by userId: {userID} and publicStatus: {true}");
            return portfolios;
        }

        public Portfolio GetMainPortfolioByUser(string userID)
        {
            Portfolio portfolio = GetPublicPortfolioByUser(userID).Where(p => p.MainStatus == true).SingleOrDefault();
            if (portfolio != null)
            {
                log.Info($"{LogConstants.LOG_GET_PORTFOLIO}: id: {portfolio.ID}, name: {portfolio.Name}, publicStatus: {portfolio.PublicStatus}, mainStatus: {portfolio.MainStatus}");
            }
            else
            {
                log.Info($"{LogConstants.LOG_GET_PORTFOLIO}: null");
            }
            return portfolio;
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
            log.Info($"{LogConstants.LOG_SET_MAIN_PORTFOLIO}: id: {portfolioID}");
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
            log.Info(LogConstants.LOG_GET_LIST_SEARCH_RECRUITMENT);
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
            log.Info($"{LogConstants.LOG_GET_LIST_SEARCH_RECRUITMENT} by address: {address} and jobId: {jobID}");
            return list;
        }

        public int ApplyRecruitment(Candidate candidate)
        {
            _context.Candidates.Add(candidate);
            log.Info($"{LogConstants.LOG_ADD_CANDIDATE}: id:{candidate.ID}, userId: {candidate.UserID}, recruitmentId: {candidate.RecruitmentID}, " +
                $"dateApply: {candidate.DateApply}, status: {candidate.Status}");
            return _context.SaveChanges();
        }

        public Candidate GetCandidateByUserIDAndRecruitmentID(string userID, string recruitmentID)
        {
            Candidate candidate = _context.Candidates.Where(c => (c.UserID == userID && c.RecruitmentID == recruitmentID)).SingleOrDefault();
            if (candidate != null)
            {
                log.Info($"{LogConstants.LOG_GET_CANDIDATE}: id:{candidate.ID}, userId: {candidate.UserID}, recruitmentId: {candidate.RecruitmentID}, " +
                $"dateApply: {candidate.DateApply}, status: {candidate.Status}");
            }
            else
            {
                log.Info($"{LogConstants.LOG_GET_CANDIDATE}: null");
            }
            return candidate;
        }

        public List<DashboardRecruitmentViewModel> GetDashboardRecruitmentByUserIDAndStatus(string userID)
        {
            List<Candidate> candidates = _context.Candidates.Where(c => c.UserID == userID).ToList();
            List<DashboardRecruitmentViewModel> dashboardRecruitments = new List<DashboardRecruitmentViewModel>();
            foreach (Candidate candidate in candidates)
            {
                Recruitment recruitment = _context.Recruitments.Where(r => r.ID == candidate.RecruitmentID).SingleOrDefault();
                if (GetCompanyStatusByCompanyProfileID(recruitment.CompanyProfileID).Equals(CommonConstants.APPROVED_STATUS))
                {
                    DashboardRecruitmentViewModel dashboardRecruitment = new DashboardRecruitmentViewModel();
                    dashboardRecruitment.RecruitmentID = candidate.RecruitmentID;

                    dashboardRecruitment.Title = recruitment.Title;
                    dashboardRecruitment.CompanyProfileID = recruitment.CompanyProfileID;
                    CompanyProfile companyProfile = _context.CompanyProfiles.Where(c => c.ID == recruitment.CompanyProfileID).SingleOrDefault();
                    dashboardRecruitment.CompanyName = companyProfile.CompanyName;
                    dashboardRecruitment.Status = candidate.Status;
                    dashboardRecruitments.Add(dashboardRecruitment);
                }
            }
            log.Info(LogConstants.LOG_GET_LIST_DASHBOARD_RECRUITMENT);
            return dashboardRecruitments;
        }

        public int CountCandidateByUserIDAndStatus(string userID, string status)
        {
            log.Info($"{LogConstants.LOG_COUNT_CANDIDATE} by userId: {userID} and status: {status}");
            int count = 0;
            List<Candidate> candidates = _context.Candidates.Where(c => (c.UserID == userID && c.Status == status)).ToList();
            foreach (Candidate candidate in candidates)
            {
                Recruitment recruitment = _context.Recruitments.Where(r => r.ID == candidate.RecruitmentID).SingleOrDefault();
                if (GetCompanyStatusByCompanyProfileID(recruitment.CompanyProfileID).Equals(CommonConstants.APPROVED_STATUS))
                {
                    count++;
                }
            }
            log.Info(count);
            return count;
        }

        public string GetCompanyStatusByCompanyProfileID(string companyProfileID)
        {
            log.Info($"{LogConstants.LOG_GET_COMPANY_STATUS} by companyProfileId: {companyProfileID}");
            CompanyProfile company = _context.CompanyProfiles.Where(c => c.ID.Equals(companyProfileID)).FirstOrDefault();
            log.Info(company.Status);
            return company.Status;
        }
    }
}