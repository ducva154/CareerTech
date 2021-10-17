using CareerTech.Models;
using CareerTech.Services;
using CareerTech.Services.Implement;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CareerTech.Controllers
{
    public class AdminServiceController : ApiController
    {
        private readonly  IAPIService<APIService> _aPIService = null;

        public AdminServiceController(IAPIService<APIService> aPIService)
        {
            _aPIService = aPIService;
        }

        //public AdminServiceController()
        //{

        //}

        [Route("api/getcandidate")]
        [HttpPost]
        public IEnumerable<CandidateFilterViewModel> GetListCandidate([FromBody] FilterViewModel data)
        {
            List<CandidateFilterViewModel> list = _aPIService.GetListCandidate();
            if (!string.IsNullOrEmpty(data.Address))
            {
                list = list.Where(i => i.Address.Equals(data.Address)).ToList();
            }
            if (!string.IsNullOrEmpty(data.Career))
            {
                list = list.Where(i => i.Career.Equals(data.Career)).ToList();
            }
            if (data.Gender != null)
            {
                list = list.Where(i => i.Gender.Equals(data.Gender)).ToList();
            }
            return list;
        }


        // POST: api/AdminService
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/AdminService/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/AdminService/5
        public void Delete(int id)
        {
        }
    }
}
