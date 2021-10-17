using CareerTech.Models;
using CareerTech.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerTech.Services.Implement
{
    public class APIService : IAPIService<APIService>
    {
        ApplicationDbContext _dbContext = null;

        public APIService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

  
        public List<CandidateFilterViewModel> GetListCandidate()
        {
            var model = from p in _dbContext.Portfolios
                        join u in _dbContext.Users on p.UserID equals u.Id
                        join pro in _dbContext.Profiles on p.ID equals pro.PortfolioID
                        where p.MainStatus == true
                        select new CandidateFilterViewModel
                        {
                            FullName = u.FullName,
                            Career = pro.Position,
                            Address = pro.Address,
                            Age = pro.Age,
                            Gender = pro.Gender,
                            Email = pro.Email,
                            Phone = pro.Phone,
                            PortfolioID = p.ID
                        };
            return model.ToList();
        }

    }
}