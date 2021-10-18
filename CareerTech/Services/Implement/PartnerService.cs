using CareerTech.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CareerTech.Services.Implement
{
    public class PartnerService : IPartnerService<PartnerService>
    {
        ILog log = LogManager.GetLogger(typeof(PartnerService));
        private readonly ApplicationDbContext _dbContext = null;

        public PartnerService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddRecruitment(Recruitment obj)
        {
            _dbContext.Recruitments.Add(obj);
            _dbContext.SaveChanges();
        }

        public bool CheckEmailExist(string email,string userId)
        {
           List<ApplicationUser> listuser = _dbContext.Users.Where(u=> !u.Id.Equals(userId)).ToList();
           ApplicationUser user = listuser.Where(u => u.Email.Equals(email)).FirstOrDefault();
           if (user!=null)
           {
                return false;
           }
           return true;
        }

        public void CreateProfileCompany(CompanyProfile company)
        {
            _dbContext.CompanyProfiles.Add(company);
            _dbContext.SaveChanges();
        }

        public List<Job> GetAllJobCategory()
        {
           return _dbContext.Jobs.ToList();
        }

        public CompanyProfile GetCompanyProfileByPartnerId(string userId)
        {
            return _dbContext.CompanyProfiles.Where(c => c.UserID.Equals(userId)).FirstOrDefault();
        }

        public ApplicationUser GetPartnerByID(string userID)
        {
            return _dbContext.Users.Where(u => u.Id.Equals(userID)).FirstOrDefault();
        }

        public void UpdateCompany(CompanyProfile company)
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
        }

        public void UpdatePartnerProfile(ApplicationUser partner)
        {
            var partnerUpdate = _dbContext.Users.Where(u => u.Id.Equals(partner.Id)).FirstOrDefault();
            partnerUpdate.FullName = partner.FullName;
            partnerUpdate.Email = partner.Email;
            partnerUpdate.PhoneNumber = partner.PhoneNumber;
            _dbContext.SaveChanges();
        }

        public void DeleteRecruitmentByID(string recruitmentId)
        {
            Recruitment recruitment = _dbContext.Recruitments.Where(r => r.ID.Equals(recruitmentId)).FirstOrDefault();
            _dbContext.Recruitments.Remove(recruitment);
            _dbContext.SaveChanges();
        }

        public CompanyProfile GetCompanyProfileById(string companyId)
        {
            return _dbContext.CompanyProfiles.Where(c => c.ID.Equals(companyId)).FirstOrDefault();
        }

        public Recruitment GetRecruitmentById(string recruitmentId)
        {
            return _dbContext.Recruitments.Where(r => r.ID.Equals(recruitmentId)).FirstOrDefault();      
        }

        public void UpdateRecruitment(Recruitment recruitment)
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
        }

        public CompanyProfile GetCompanyProfileByRecruitmentId(string recruitmentId)
        {
            var recruitment = _dbContext.Recruitments.Where(r=>r.ID.Equals(recruitmentId)).FirstOrDefault();
            return _dbContext.CompanyProfiles.Where(c => c.ID.Equals(recruitment.CompanyProfileID)).FirstOrDefault();
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
            return model.ToList();
        }

        public List<Recruitment> GetListRecruitmentByCompanyID(string companyID)
        {
            return _dbContext.Recruitments.Where(r => r.CompanyProfileID.Equals(companyID)).ToList();
        }


        public void DeleteCandidateById(string id)
        {
            var candidate = _dbContext.Candidates.Where(c => c.ID.Equals(id)).FirstOrDefault();
            _dbContext.Candidates.Remove(candidate);
            _dbContext.SaveChanges();
        }

        public void UpdateCandidateByID(string candidateID)
        {
            var candidateUpdate = _dbContext.Candidates.Where(c => c.ID.Equals(candidateID)).FirstOrDefault();
            candidateUpdate.Status = "Approved";
            _dbContext.SaveChanges();
        }

        public int GetServiceTime(string userID)
        {
            var model = _dbContext.Times.Where(t => t.UserID.Equals(userID)).FirstOrDefault();
            var endDate = model.EndDate;
            var remainingTime = DateTime.Now.Date;
            var serviceTIme = endDate.Subtract(remainingTime).Days;
            return serviceTIme;
        }
    }
}