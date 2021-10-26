using CareerTech.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static CareerTech.Utils.LogConstants;
namespace CareerTech.Services.Implement
{
    public class ContentService : IContentService<ContentService>
    {
        private readonly ApplicationDbContext _applicationDbContext = null;
        ILog log = LogManager.GetLogger(typeof(ContentService));
        public ContentService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public int addContent(Guid id, string userID, string Title, string Detail, string img_url, bool mainStatus)
        {
            Introduction intro = new Introduction();
            intro.ID = id.ToString();
            intro.UserID = userID;
            intro.Title = Title;
            intro.Detail = Detail;
            intro.Url_Image = img_url;
            intro.Main = mainStatus;
            log.Info($"{LOG_ADD_CONTENT}: id: {intro.ID}, title: {intro.Title}, detail:{intro.Detail},img: {intro.Url_Image}, MainStatus: {intro.Main}");
            _applicationDbContext.Introductions.Add(intro);
            int result = _applicationDbContext.SaveChanges();
            return result;

        }

        public int deleteIntroductionByID(string id)
        {
            var intro = GetIntroductionByID(id);
            log.Info($"{LOG_DELETE_CONTENT}: id: {intro.ID}, title: {intro.Title}, detail:{intro.Detail},img: {intro.Url_Image}, MainStatus: {intro.Main}");
            _applicationDbContext.Introductions.Remove(intro);
            return _applicationDbContext.SaveChanges();
        }

        public Introduction GetPublicIntroduction()
        {
            var query = from i in _applicationDbContext.Introductions
                        where i.Main == true
                        select i;
            var intro = query.FirstOrDefault();
            if (intro != null)
            {
                log.Info($"{LOG_MAIN_CONTENT}: id: {intro.ID}, title: {intro.Title}, detail:{intro.Detail},img: {intro.Url_Image}, MainStatus: {intro.Main}");

            }
            else
            {
                log.Error(LOG_NULL_VALUE);

            }
            return intro;
        }

        public int updateContent(string id, string title, string detail, string url_img, bool mainStatus)
        {
            var query = from i in _applicationDbContext.Introductions
                        where i.ID == id
                        select i;
            var intro = query.FirstOrDefault();
            intro.Title = title;
            intro.Detail = detail;
            intro.Url_Image = url_img;
            intro.Main = mainStatus;
            log.Info($"{LOG_EDIT_CONTENT}: id: {intro.ID}, title: {intro.Title}, detail:{intro.Detail},img: {intro.Url_Image}, MainStatus: {intro.Main}");
            return _applicationDbContext.SaveChanges();

        }

        public List<Introduction> GetAllIntroductions()
        {
            var query = from intro in _applicationDbContext.Introductions
                        select intro;
            var listIntro = query.ToList();
            log.Info(LOG_GET_LIST_CONTENT);
            return listIntro;
        }

        public Introduction GetIntroductionByID(string id)
        {
            var query = from i in _applicationDbContext.Introductions
                        where i.ID == id
                        select i;
            var intro = query.FirstOrDefault();
            if (intro != null)
            {
                log.Info($"{LOG_MAIN_CONTENT}: id: {intro.ID}, title: {intro.Title}, detail:{intro.Detail},img: {intro.Url_Image}, MainStatus: {intro.Main}");

            }
            else
            {
                log.Error(LOG_NULL_VALUE);

            }
            return intro;
        }
        public Introduction GetIntroduction()
        {
            var query = from i in _applicationDbContext.Introductions
                        select i;
            var intro = query.FirstOrDefault();
            if (intro != null)
            {
                log.Info($"{LOG_MAIN_CONTENT}: id: {intro.ID}, title: {intro.Title}, detail:{intro.Detail},img: {intro.Url_Image}, MainStatus: {intro.Main}");

            }
            else
            {
                log.Error(LOG_NULL_VALUE);

            }
            return intro;
        }

        public bool CheckMainExisted()
        {
            var contents = GetAllIntroductions();
            bool status = false;
            foreach (var c in contents)
            {
                if (c.Main == true)
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
    }
}