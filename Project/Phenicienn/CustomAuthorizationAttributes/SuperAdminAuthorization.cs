using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Phenicienn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Phenicienn.CustomAuthorizationAttributes
{
    public class SuperAdminAuthorization : AuthorizeAttribute, IAuthorizationFilter

    {

        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var role = context.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if (role != "SuperAdmin")
            {
                if(role == "Admin")
                    context.Result = new RedirectResult("/Admin");
                else
                    context.Result = new RedirectResult("/");
            }
        }
    }
}
