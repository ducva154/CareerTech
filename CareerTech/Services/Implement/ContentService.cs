using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerTech.Services.Implement
{
    public class ContentService : IContentService<ContentService>
    {
        ApplicationDbContext _applicationDbContext { get; set; }

        public ContentService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public int addContent(Guid id, string userID, string Title, string Detail,string img_url)
        {
            Introduction intro = new Introduction();
            intro.ID = id.ToString();
            intro.UserID = userID;
            intro.Title = Title;
            intro.Detail = Detail;
            intro.Url_Image = img_url;
            _applicationDbContext.Introductions.Add(intro);
            int result = _applicationDbContext.SaveChanges();
            return result;

        }

        public int deleteIntroductionByID(string id)
        {
            var introduction = GetIntroductionByID(id);
            _applicationDbContext.Introductions.Remove(introduction);
           return _applicationDbContext.SaveChanges();
        }

        public Introduction GetIntroduction()
        {
            var query = from intro in _applicationDbContext.Introductions
                        select intro;
            var introduction = query.FirstOrDefault();
            return introduction;
        }

        public int updateContent(string id, string title, string detail,string url_img)
        {
            var query = from intro in _applicationDbContext.Introductions
                        where intro.ID == id
                        select intro;
            var introduction = query.FirstOrDefault();
            introduction.Title = title;
            introduction.Detail = detail;
            introduction.Url_Image = url_img;
            return _applicationDbContext.SaveChanges();

        }

        public List<Introduction> GetAllIntroductions()
        {
            var query = from intro in _applicationDbContext.Introductions
                        select intro;
            var listIntro = query.ToList();
            return listIntro;
        }

        public Introduction GetIntroductionByID(string id)
        {
            var query = from intro in _applicationDbContext.Introductions
                        where intro.ID == id
                        select intro;
            var introduction = query.FirstOrDefault();
            return introduction;
        }
    }
}