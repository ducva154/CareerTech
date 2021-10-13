using CareerTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerTech.Services
{
    public interface IAboutManagement<AboutService>
    {
        int AddAbout(Guid guid, string userID, string Title, string Detail, string Description, bool mainStatus);
        bool CheckMainStatusExisted();
        List<About> getAllAbouts();
        About getAboutByID(string aboutID);
        int UpdateAbout(string aboutId, string Title, string Detail, string Description, bool mainStatus);
        About getMainAbout();
        int deleteAboutByID(string aboutID);
        About getAbout();
    }
}
