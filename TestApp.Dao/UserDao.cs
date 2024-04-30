using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Entity;
using XFramework.XInject.Attributes;

namespace TestApp.Dao
{
    [Bean("userDao")]
    public class UserDao
    {
        public bool Login(User user)
        {
            using (var db = new Entities())
            {
                var res = db.User.Where(a => a.UserName == user.UserName && a.PassWord == user.PassWord).FirstOrDefault();
                if (res != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
