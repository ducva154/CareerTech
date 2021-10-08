using CareerTech.Models;
using CareerTech.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CareerTech.Startup))]
namespace CareerTech
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
           
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // In Startup iam creating first Admin Role and creating a default Admin User
            if (!roleManager.RoleExists(RoleNameConstant.ADMIN))
            {
                // first we create Admin rool
                var role = new IdentityRole();
                role.Name = RoleNameConstant.ADMIN;
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website

                var user = new ApplicationUser();
                user.UserName = "Admin@gmail.com";
                user.Email = "Admin@gmail.com";
                string userPWD = "Abc@123";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin
                if (chkUser.Succeeded)
                {
                    UserManager.AddToRole(user.Id, RoleNameConstant.ADMIN);
                }
            }

            if (!roleManager.RoleExists(RoleNameConstant.USER))
            {
                var role = new IdentityRole();
                role.Name = RoleNameConstant.USER;
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists(RoleNameConstant.PARTNER))
            {
                var role = new IdentityRole();
                role.Name = RoleNameConstant.PARTNER;
                roleManager.Create(role);
            }
        }
    }
}
