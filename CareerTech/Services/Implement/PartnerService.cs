using CareerTech.Models;
using CareerTech.Utils;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CareerTech.Services.Implement
{
    public class PartnerService : IPartnerService<PartnerService>
    {
       private readonly ILog log = LogManager.GetLogger(typeof(PartnerService));
        private readonly ApplicationDbContext _dbContext = null;

        public PartnerService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddRecruitment(Recruitment obj)
        {
            try
            {
                log.Info(obj.Title);
                _dbContext.Recruitments.Add(obj);
                _dbContext.SaveChanges();
                log.Info(LogConstants.ADD_RECRUITMENT_SUCCESS);
            }
            catch (Exception)
            {
                throw new Exception();
            }

        }

        public void CreateProfileCompany(CompanyProfile company)
        {
            try
            {
                log.Info(company.CompanyName);
                _dbContext.CompanyProfiles.Add(company);
                _dbContext.SaveChanges();
                log.Info(LogConstants.CREATE_COMPANY_SUCCESS);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public List<Job> GetAllJobCategory()
        {
           log.Info("Get list job category");
           return _dbContext.Jobs.ToList();
        }

        public CompanyProfile GetCompanyProfileByPartnerId(string userId)
        {
            CompanyProfile model = _dbContext.CompanyProfiles.Where(c => c.UserID.Equals(userId)).FirstOrDefault();
            log.Info("Get company profile");
            return model;
        }

        public ApplicationUser GetPartnerByID(string userID)
        {
            ApplicationUser partner = _dbContext.Users.Where(u => u.Id.Equals(userID)).FirstOrDefault();
            log.Info(partner.FullName);
            return partner;
        }

        public void UpdateCompany(CompanyProfile company)
        {
            try
            {
                var companyUpdate = _dbContext.CompanyProfiles.Where(c => c.ID.Equals(company.ID)).FirstOrDefault();
                companyUpdate.CompanyName = company.CompanyName;
                companyUpdate.Address = company.Address;
                companyUpdate.Desc = company.Desc;
                companyUpdate.Url_Avatar = company.Url_Avatar;
                companyUpdate.Url_Background = company.Url_Background;
                companyUpdate.Phone = company.Phone;
                companyUpdate.Email = company.Email;
                _dbContext.SaveChanges();
                log.Info(LogConstants.UPDATE_COMPANY_SUCCESS);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public void UpdatePartnerProfile(ApplicationUser partner)
        {
            try
            {
                var partnerUpdate = _dbContext.Users.Where(u => u.Id.Equals(partner.Id)).FirstOrDefault();
                partnerUpdate.FullName = partner.FullName;
                partnerUpdate.Email = partner.Email;
                partnerUpdate.PhoneNumber = partner.PhoneNumber;
                _dbContext.SaveChanges();
                log.Info(LogConstants.UPDATE_PARTNER_PROFILE_SUCCESS);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public void DeleteRecruitmentByID(string recruitmentId)
        {
            try
            {
                Recruitment recruitment = _dbContext.Recruitments.Where(r => r.ID.Equals(recruitmentId)).FirstOrDefault();
                _dbContext.Recruitments.Remove(recruitment);
                _dbContext.SaveChanges();
                log.Info(LogConstants.DELETE_RECRUITMENT_SUCCESS);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public CompanyProfile GetCompanyProfileById(string companyId)
        {
            var company  = _dbContext.CompanyProfiles.Where(c => c.ID.Equals(companyId)).FirstOrDefault();
            log.Info(company.CompanyName);
            return company;
        }

        public Recruitment GetRecruitmentById(string recruitmentId)
        {
            var recruitment = _dbContext.Recruitments.Where(r => r.ID.Equals(recruitmentId)).FirstOrDefault();
            log.Info(recruitment.Title);
            return recruitment;
        }

        public void UpdateRecruitment(Recruitment recruitment)
        {
            try
            {
                Recruitment recruitmentUpdate = _dbContext.Recruitments.Where(r => r.ID.Equals(recruitment.ID)).FirstOrDefault();
                recruitment.JobID = recruitment.JobID;
                recruitment.Title = recruitment.Title;
                recruitment.Address = recruitment.Address;
                recruitment.Salary = recruitment.Salary;
                recruitment.Workingform = recruitment.Workingform;
                recruitment.Amount = recruitment.Amount;
                recruitment.Position = recruitment.Position;
                recruitment.Experience = recruitment.Experience;
                recruitment.Gender = recruitment.Gender;
                recruitment.EndDate = recruitment.EndDate;
                recruitment.DetailDesc = recruitment.DetailDesc.ToString();
                _dbContext.SaveChanges();
                log.Info(LogConstants.UPDATE_RECRUITMENT_SUCCESS);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public CompanyProfile GetCompanyProfileByRecruitmentId(string recruitmentId)
        {
            var recruitment = _dbContext.Recruitments.Where(r=>r.ID.Equals(recruitmentId)).FirstOrDefault();
            var company = _dbContext.CompanyProfiles.Where(c => c.ID.Equals(recruitment.CompanyProfileID)).FirstOrDefault();
            log.Info(company.CompanyName);
            return company;
        }

        public List<CandidateViewModel> GetListCandidateByCompanyID(string companyID)
        {
            var model = from c in _dbContext.Candidates
                        join p in _dbContext.Portfolios on c.UserID equals p.UserID
                        join r in _dbContext.Recruitments on c.RecruitmentID equals r.ID
                        join u in _dbContext.Users on c.UserID equals u.Id
                        where r.CompanyProfileID == companyID && p.MainStatus==true && p.PublicStatus == true
                        select new CandidateViewModel
                        {
                            CandidateID = c.ID,
                            FullName = u.FullName,
                            UserID = u.Id,
                            Email = u.Email,
                            Phone = u.PhoneNumber,
                            DateApply = c.DateApply,
                            PortfolioID = p.ID,
                            RecruitmentID = r.ID,
                            Status = c.Status
                        };
            log.Info("Get list candidateviewmodel");
            return model.ToList();
        }

        public List<Recruitment> GetListRecruitmentByCompanyID(string companyID)
        {
            log.Info("Get list recruitment");
            return _dbContext.Recruitments.Where(r => r.CompanyProfileID.Equals(companyID)).ToList();
        }


        public void DeleteCandidateById(string id)
        {
            try
            {
                var candidate = _dbContext.Candidates.Where(c => c.ID.Equals(id)).FirstOrDefault();
                _dbContext.Candidates.Remove(candidate);
                _dbContext.SaveChanges();
                log.Info(LogConstants.DELETE_CANDIDATE_SUCCESS);
            }
            catch (Exception)
            {

                throw new Exception();
            }
        }

        public void UpdateCandidateByID(string candidateID)
        {
            try
            {
                var candidateUpdate = _dbContext.Candidates.Where(c => c.ID.Equals(candidateID)).FirstOrDefault();
                candidateUpdate.Status = CommonConstants.APPROVED_STATUS;
                _dbContext.SaveChanges();
                log.Info(LogConstants.UPDATE_CANDIDATE_SUCCESS);
            }
            catch (Exception)
            {

                throw new Exception();
            }
        }

        public int GetServiceTime(string userID)
        {
            try
            {
                var model = _dbContext.Times.Where(t => t.UserID.Equals(userID)).FirstOrDefault();
                var endDate = model.EndDate;
                var remainingTime = DateTime.Now.Date;
                var serviceTime = endDate.Subtract(remainingTime).Days;
                log.Info("servicetime:" + serviceTime);
                return serviceTime;
            }
            catch (Exception)
            {

                return -1;
            }
        }
    }
}