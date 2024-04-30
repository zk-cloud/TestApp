using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Dao;
using TestApp.Entity;
using XFramework.XInject.Attributes;

namespace TestApp.Services
{
    [Bean("userServices")]
    public class UserServices
    {
        [Autowired]
        UserDao userDao { get; set; }

        public bool Login(User user)
        {
            return userDao.Login(user);
        }
    }
}
