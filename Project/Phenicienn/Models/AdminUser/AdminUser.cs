using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Phenicienn.Models.AdminUser
{
    public class AdminUser : IdentityUser
    {
        [ForeignKey("FK_ADMINUSER_RESTURANT")]
        public int Restaurant { set; get; }
        [DefaultValue(false)]
        public bool active { get; set; }
        [DefaultValue(false)]
        public bool setup { get; set; }
    }
}
