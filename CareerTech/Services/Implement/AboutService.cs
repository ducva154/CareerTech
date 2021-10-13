using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerTech.Services.Implement
{
    public class AboutService : IAboutManagement<AboutService>
    {
        ApplicationDbContext _applicationDbContext { get; set; }

        public AboutService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public int AddAbout(Guid aboutId, string userID, string Title, string Detail, string Description, bool mainStatus)
        {
            About about = new About();
            about.ID = aboutId.ToString();
            about.UserID = userID;
            about.Title = Title;
            about.Detail = Detail;
            about.Desc = Description;
            about.Main = mainStatus;
            _applicationDbContext.Abouts.Add(about);
            int result = _applicationDbContext.SaveChanges();
            return result;
        }

        public bool CheckMainStatusExisted()
        {
            var listAbout = getAllAbouts();
            bool status = false;
            foreach (var about in listAbout)
            {
                if (about.Main == true)
                {
                    status = true;
                    break;
                }
                else
                {
                    status = false;
                }
            }
            return status;
        }

        public List<About> getAllAbouts()
        {
            var query = from about in _applicationDbContext.Abouts
                        select about;
            var listAbout = query.ToList();
            return listAbout;
        }

        public About getAboutByID(string aboutID)
        {
            var query = from ab in _applicationDbContext.Abouts
                        where ab.ID == aboutID
                        select ab;
            var about = query.FirstOrDefault();
            return about;
        }

        public int UpdateAbout(string aboutId, string Title, string Detail, string Description, bool mainStatus)
        {
            var about = getAboutByID(aboutId);
            about.Title = Title;
            about.Detail = Detail;
            about.Desc = Description;
            about.Main = mainStatus;
            int result = _applicationDbContext.SaveChanges();
            return result;
        }

        public About getMainAbout()
        {
            var query = from about in _applicationDbContext.Abouts
                        where about.Main == true
                        select about;
            var _about = query.FirstOrDefault();
            return _about;
        }

        public int deleteAboutByID(string aboutID)
        {
            var about = getAboutByID(aboutID);
            _applicationDbContext.Abouts.Remove(about);
            int result = _applicationDbContext.SaveChanges();
            return result;
        }

        public About getAbout()
        {
            var query = from about in _applicationDbContext.Abouts
                        select about;
            var _about = query.FirstOrDefault();
            return _about;
        }
    }
}