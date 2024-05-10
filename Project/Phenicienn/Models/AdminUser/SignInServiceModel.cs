using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phenicienn.Models.AdminUser
{
    public class SignInServiceModel
    {
        public AdminUser AdminUser { set; get; }
        public string user { set; get; }
        public string role { set; get; }
        public string setup { set; get; }
        public string password { set; get; }
        public string passwordHash { set; get; }

    }
}
