using CareerTech.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static CareerTech.Utils.LogConstants;

namespace CareerTech.Services.Implement
{
    public class AboutService : IAboutManagement<AboutService>
    {
        private readonly ApplicationDbContext _applicationDbContext = null;
        ILog log = LogManager.GetLogger(typeof(AboutService));
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
            log.Info($"{LOG_ADD_ABOUT}: id: {about.ID},title: {about.Title},detail: {about.Detail},desc: {about.Desc}");
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
                    log.Info(LOG_MAIN_EXISTED);
                    status = true;
                    break;
                }
                else
                {
                    log.Info(LOG_SET_MAIN);
                    status = false;
                }
            }
            return status;
        }

        public List<About> getAllAbouts()
        {
            var query = from about in _applicationDbContext.Abouts
                        select about;
            log.Info(LOG_GET_LIST_ABOUT);
            var listAbout = query.ToList();
            return listAbout;
        }

        public About getAboutByID(string aboutID)
        {
            var query = from ab in _applicationDbContext.Abouts
                        where ab.ID == aboutID
                        select ab;
            var about = query.FirstOrDefault();
            if (about != null)
            {
                log.Info($"{ LOG_GET_ABOUT_BYID} id: {about.ID},title: {about.Title},detail: {about.Detail},desc: {about.Desc}");
            }
            else
            {
                log.Error(LOG_NULL_VALUE);

            }
            return about;
        }

        public int UpdateAbout(string aboutId, string Title, string Detail, string Description, bool mainStatus)
        {
            var about = getAboutByID(aboutId);
            about.Title = Title;
            about.Detail = Detail;
            about.Desc = Description;
            about.Main = mainStatus;
            log.Info($"{LOG_EDIT_ABOUT}: id: {about.ID},title: {about.Title},detail: {about.Detail},desc: {about.Desc}");
            int result = _applicationDbContext.SaveChanges();
            return result;
        }

        public About getMainAbout()
        {
            var query = from a in _applicationDbContext.Abouts
                        where a.Main == true
                        select a;
            var about = query.FirstOrDefault();
            if (about != null)
            {
                log.Info($"{LOG_MAIN_ABOUT}: id: {about.ID},title: {about.Title},detail: {about.Detail},desc: {about.Desc}, status: {about.Main}");

            }
            else
            {
                log.Error(LOG_NULL_VALUE);

            }
            return about;
        }

        public int deleteAboutByID(string aboutID)
        {
            var about = getAboutByID(aboutID);
            log.Info($"{LOG_DELETE_ABOUT}id: {about.ID},title: {about.Title},detail: {about.Detail},desc: {about.Desc}, status: {about.Main}");
            _applicationDbContext.Abouts.Remove(about);
            int result = _applicationDbContext.SaveChanges();
            return result;
        }

        public About getAbout()
        {
            var query = from a in _applicationDbContext.Abouts
                        select a;
            var about = query.FirstOrDefault();
            if (about != null)
            {
                log.Info($"{LOG_GET_ABOUT}id: {about.ID},title: {about.Title},detail: {about.Detail},desc: {about.Desc}, status: {about.Main}");

            }
            else
            {
                log.Error(LOG_NULL_VALUE);

            }
            return about;
        }
    }
}