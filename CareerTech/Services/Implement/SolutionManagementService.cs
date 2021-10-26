using CareerTech.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static CareerTech.Utils.LogConstants;
namespace CareerTech.Services.Implement
{
    public class SolutionManagementService : ISolutionManagementService<SolutionManagementService>
    {
        private readonly ApplicationDbContext _applicationDbContext = null;
        private readonly ILog log = LogManager.GetLogger(typeof(SolutionManagementService));
        public SolutionManagementService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public int AddSolution(Guid id, string uID, string Name, string Des, string img_url)
        {
            Solution solution = new Solution();
            solution.ID = id.ToString();
            solution.Title = Name;
            solution.Detail = Des;
            solution.UserID = uID;
            solution.Url_image = img_url;
            log.Info($"{LOG_ADD_SOLUTION}: id:{solution.ID},title:{solution.Title},img:{solution.Url_image}");
            _applicationDbContext.Solutions.Add(solution);
            int result = _applicationDbContext.SaveChanges();
            return result;
        }

        public List<Solution> GetSolutions()
        {
            var query = from sol in _applicationDbContext.Solutions
                        select sol;
            var result = query.ToList();
            log.Info(LOG_GET_LIST_SUBSCRIPTION);
            return result;
        }

        public Solution GetSolutionByID(string solID)
        {
            var query = from sol in _applicationDbContext.Solutions
                        where sol.ID == solID
                        select sol;
            var solution = query.FirstOrDefault();
            if (solution != null)
            {
                log.Info($"{LOG_GET_SOLUTION_BYID}: {solution.ID},title:{solution.Title},img:{solution.Url_image}");

            }
            else
            {
                log.Error(LOG_NULL_VALUE);
            }
            return solution;
        }

        public int UpdateSolutionByID(string solID, string Title, string Des, string img_url)
        {

            var solution = GetSolutionByID(solID);
            solution.Title = Title;
            solution.Detail = Des;
            solution.Url_image = img_url;
            log.Info($"{LOG_EDIT_SOLUTION}: id:{solution.ID},title:{solution.Title},img:{solution.Url_image}");
            int row = _applicationDbContext.SaveChanges();
            return row;
        }

        public int DeleteSolutionByID(string solID)
        {
            var solution = GetSolutionByID(solID);
            log.Info($"{LOG_DELETE_SOLUTION}: id:{solution.ID},title:{solution.Title},img:{solution.Url_image}");
            _applicationDbContext.Solutions.Remove(solution);
            int row = _applicationDbContext.SaveChanges();
            return row;
        }
    }
}