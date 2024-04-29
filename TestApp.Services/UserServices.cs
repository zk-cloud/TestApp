using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Dao;
using TestApp.Entity;

namespace TestApp.Services
{
    public class UserServices
    {
        
        public UserDao userDao { get; set; }

        public bool Login(User user)
        {
            return userDao.Login(user);
        }
    }
}
