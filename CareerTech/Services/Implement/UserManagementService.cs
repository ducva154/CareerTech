using CareerTech.Models;
using log4net;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using static CareerTech.Utils.LogConstants;
namespace CareerTech.Services.Implement
{
    public class UserManagementService : IUserManagmentService<UserManagementService>
    {
        private readonly ApplicationDbContext _applicationDbContext = null;
        private readonly ILog log = LogManager.GetLogger(typeof(UserManagementService));


        public UserManagementService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public List<ApplicationUser> getAllUsers()
        {
            var query = from u in _applicationDbContext.Users
                        from ur in u.Roles
                        join r in _applicationDbContext.Roles on ur.RoleId equals r.Id
                        where r.Name == "User"
                        select u;
            log.Info(LOG_GET_USER);
            var result = query.ToList();
            return result;
        }

        public int deleteUser(string uID)
        {
            var user = (from u in _applicationDbContext.Users
                        where u.Id == uID
                        select u).FirstOrDefault();
            log.Info($"{LOG_DELETE_USER}: id:{user.Id},name:{user.FullName}");
            _applicationDbContext.Users.Remove(user);
            int result = _applicationDbContext.SaveChanges();
            return result;
        }

        public int countNumerOfUser()
        {
            int count = getAllUsers().Count();
            log.Info(LOG_NUMBER_OF_USER);
            return count;
        }
    }
}