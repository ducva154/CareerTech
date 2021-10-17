using CareerTech.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace CareerTech.Services.Implement
{
    public class UserManagementService : IUserManagmentService<UserManagementService>
    {
        private readonly ApplicationDbContext _applicationDbContext = null;

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
            var result = query.ToList();
            return result;
        }

        public int deleteUser(string uID)
        {
            var user = (from u in _applicationDbContext.Users
                        where u.Id == uID
                        select u).FirstOrDefault();
            _applicationDbContext.Users.Remove(user);
            int result = _applicationDbContext.SaveChanges();
            return result;
        }

        public int countNumerOfUser()
        {

            return getAllUsers().Count();
        }
    }
}