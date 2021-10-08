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

            container.RegisterType<AccountController>(new InjectionConstructor(typeof(IAccountService<AccountService>)));
            container.RegisterType<ManageController>(new InjectionConstructor());

            container.RegisterType<IAccountService<AccountService>, AccountService>();
            container.RegisterType<IPartnerService<PartnerService>, PartnerService>();
            container.RegisterType<ApplicationDbContext, ApplicationDbContext>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}