using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerTech.Services
{
    public interface IContentService<T> where T : class
    {
        int addContent(Guid id, string userID, string Title, string Detail, string url_img, bool mainStatus);
        Introduction GetPublicIntroduction();
        int deleteIntroductionByID(string id);
        int updateContent(string id, string title, string detail, string url_img, bool mainStatus);
        List<Introduction> GetAllIntroductions();
        Introduction GetIntroductionByID(string id);
        Introduction GetIntroduction();
        bool CheckMainExisted();
    }
}
