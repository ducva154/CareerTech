using CareerTech.Models;
using System.Collections.Generic;
using System.Linq;

namespace CareerTech.Services.Implement
{
    public class PartnerService : IPartnerService<PartnerService>
    {
       private readonly ApplicationDbContext dbContext = null;

        public PartnerService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddRecruitment(Recruitment obj)
        {
            dbContext.Recruitments.Add(obj);
            dbContext.SaveChanges();
        }

        public bool CheckEmailExist(string email,string userId)
        {
           List<ApplicationUser> listuser = dbContext.Users.Where(u=> !u.Id.Equals(userId)).ToList();
           ApplicationUser user = listuser.Where(u => u.Email.Equals(email)).FirstOrDefault();
           if (user!=null)
           {
                return false;
           }
           return true;
        }

        public void CreateProfileCompany(CompanyProfile company)
        {
            dbContext.CompanyProfiles.Add(company);
            dbContext.SaveChanges();
        }

        public List<Job> GetAllJobCategory()
        {
            return dbContext.Jobs.ToList();
        }


        public CompanyProfile GetCompanyProfileByPartnerId(string userId)
        {
            return dbContext.CompanyProfiles.Where(c => c.UserID.Equals(userId)).FirstOrDefault();
        }

        public ApplicationUser GetPartnerByID(string userID)
        {
            return dbContext.Users.Where(u => u.Id.Equals(userID)).FirstOrDefault();
        }

        public void UpdateCompany(CompanyProfile company)
        {
            var companyUpdate = dbContext.CompanyProfiles.Where(c => c.ID.Equals(company.ID)).FirstOrDefault();
            companyUpdate.CompanyName = company.CompanyName;
            companyUpdate.Address = company.Address;
            companyUpdate.Desc = company.Desc;
            companyUpdate.Url_Avatar = company.Url_Avatar;
            companyUpdate.Url_Background = company.Url_Background;
            companyUpdate.Phone = company.Phone;
            companyUpdate.Email = company.Email;
            dbContext.SaveChanges();
        }

        public void UpdatePartnerProfile(ApplicationUser partner)
        {
            var partnerUpdate = dbContext.Users.Where(u => u.Id.Equals(partner.Id)).FirstOrDefault();
            partnerUpdate.FullName = partner.FullName;
            partnerUpdate.Email = partner.Email;
            partnerUpdate.PhoneNumber = partner.PhoneNumber;
            dbContext.SaveChanges();
        }

        public void DeleteRecruitmentByID(string recruitmentId)
        {
            Recruitment recruitment = dbContext.Recruitments.Where(r => r.ID.Equals(recruitmentId)).FirstOrDefault();
            dbContext.Recruitments.Remove(recruitment);
            dbContext.SaveChanges();
        }

        public CompanyProfile GetCompanyProfileById(string companyId)
        {
            return dbContext.CompanyProfiles.Where(c => c.ID.Equals(companyId)).FirstOrDefault();
        }

        public Recruitment GetRecruitmentById(string recruitmentId)
        {
            return dbContext.Recruitments.Where(r => r.ID.Equals(recruitmentId)).FirstOrDefault();
        }

        public void UpdateRecruitment(Recruitment recruitment)
        {
            Recruitment recruitmentUpdate = dbContext.Recruitments.Where(r => r.ID.Equals(recruitment.ID)).FirstOrDefault();
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
            dbContext.SaveChanges();
        }

        public CompanyProfile GetCompanyProfileByRecruitmentId(string recruitmentId)
        {
            var recruitment = dbContext.Recruitments.Where(r=>r.ID.Equals(recruitmentId)).FirstOrDefault();
            return dbContext.CompanyProfiles.Where(c => c.ID.Equals(recruitment.CompanyProfileID)).FirstOrDefault();
        }

        public List<CandidateViewModel> GetListCandidateByCompanyID(string companyID)
        {
            var model = from c in dbContext.Candidates
                        join p in dbContext.Portfolios on c.UserID equals p.UserID
                        join r in dbContext.Recruitments on c.RecruitmentID equals r.ID
                        join u in dbContext.Users on c.UserID equals u.Id
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
            return dbContext.Recruitments.Where(r => r.CompanyProfileID.Equals(companyID)).ToList();
        }
        public List<Recruitment> GetListRecruitmentabcmpanyID(string companyID)
        {
            return dbContext.Recruitments.Where(r => r.Address.ToLower().Contains(companyID.ToLower())).ToList();
        }

        public void DeleteCandidateById(string id)
        {
            var candidate = dbContext.Candidates.Where(c => c.ID.Equals(id)).FirstOrDefault();
            dbContext.Candidates.Remove(candidate);
            dbContext.SaveChanges();
        }

        public void UpdateCandidateByID(string candidateID)
        {
            var candidateUpdate = dbContext.Candidates.Where(c => c.ID.Equals(candidateID)).FirstOrDefault();
            candidateUpdate.Status = "Approved";
            dbContext.SaveChanges();
        }






    }
}