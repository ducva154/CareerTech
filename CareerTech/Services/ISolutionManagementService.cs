using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerTech.Services
{
    public interface ISolutionManagementService<T> where T:class
    {
        int AddSolution(Guid id, string uID, string Name, string Des, string img_url);
        List<Solution> GetSolutions();
        Solution GetSolutionByID(string solID);
        int UpdateSolutionByID(string solID, string Title,string Des,string img_url);
        int DeleteSolutionByID(string solID);
    }
}
