using CareerTech.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerTech.Services
{
    public interface IUserManagmentService<T> where T : class
    {
        List<ApplicationUser> getAllUsers();
        int deleteUser(string uID);
        int countNumerOfUser();

    }
}
