using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECap.Core.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string User_Login { get; set; }
        public string  User_AdminRole { get; set; }
        public string  User_ClientGroup { get; set; }
        public bool UserLogin_Flag { get; set; }
    }
}
