﻿using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerTech.Services.Implement
{
    public class SolutionManagementService : ISolutionManagementService<SolutionManagementService>
    {
        ApplicationDbContext _applicationDbContext { get; set; }
public SolutionManagementService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public int AddSolution(Guid id,string uID, string Name, string Des,string img_url)
        {
            Solution solution = new Solution();
            solution.ID = id.ToString();
            solution.Title = Name;
            solution.Detail = Des;
            solution.UserID = uID;
            solution.Url_image = img_url;
            _applicationDbContext.Solutions.Add(solution);
            int result = _applicationDbContext.SaveChanges();
            return result;
        }

        public List<Solution> GetSolutions()
        {
            var query = from sol in _applicationDbContext.Solutions
                        select sol;
            var result = query.ToList();
            
            return result;
        }

        public Solution GetSolutionByID(string solID)
        {
            var query = from sol in _applicationDbContext.Solutions
                        where sol.ID == solID
                        select sol;
            var solution = query.FirstOrDefault();
            return solution;
        }

        public int UpdateSolutionByID(string solID, string Title, string Des, string img_url)
        {

            var solution = GetSolutionByID(solID);
            solution.Title = Title;
            solution.Detail = Des;
            solution.Url_image = img_url;
            int row = _applicationDbContext.SaveChanges();
            return row;
        }

        public int DeleteSolutionByID(string solID)
        {
            var solution = GetSolutionByID(solID);
            _applicationDbContext.Solutions.Remove(solution);
            int row = _applicationDbContext.SaveChanges();
            return row;
        }
    }
}