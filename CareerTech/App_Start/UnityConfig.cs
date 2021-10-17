using CareerTech.Controllers;
using CareerTech.Models;
using CareerTech.Services;
using CareerTech.Services.Implement;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace CareerTech
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            //container.RegisterType<ServiceController>(new InjectionConstructor(typeof(IAPIService<APIService>)));
            container.RegisterType<AccountController>(new InjectionConstructor(typeof(IAccountService<AccountService>)));
            container.RegisterType<ManageController>(new InjectionConstructor());
            container.RegisterType<IUserManagmentService<UserManagementService>, UserManagementService>();
            container.RegisterType<IPartnerManagementService<PartnerManagementService>, PartnerManagementService>();
            container.RegisterType<IAccountService<AccountService>, AccountService>();
            container.RegisterType<IPartnerService<PartnerService>, PartnerService>();
            container.RegisterType<IUserService<UserService>, UserService>();
            container.RegisterType<ApplicationDbContext, ApplicationDbContext>();
            container.RegisterType<ISubscriptionManagementService<SubscriptionManagementService>, SubscriptionManagementService>();
            container.RegisterType<ISolutionManagementService<SolutionManagementService>,SolutionManagementService > ();
            container.RegisterType<IContentService<ContentService>, ContentService>();
            container.RegisterType<IAboutManagement<AboutService>, AboutService>();
            container.RegisterType<IOrderManagementService<OrderManagementService>, OrderManagementService>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}