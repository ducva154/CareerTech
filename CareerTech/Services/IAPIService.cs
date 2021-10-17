
using CareerTech.Models;
using System.Collections.Generic;

namespace CareerTech.Services
{
    public interface IAPIService<T> where T : class
    {
        List<CandidateFilterViewModel> GetListCandidate();

    }
}
