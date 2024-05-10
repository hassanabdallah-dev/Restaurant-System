using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Phenicienn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Phenicienn.CustomAuthorizationAttributes
{
    public class AdminSetupReverseAuthorize : AuthorizeAttribute, IAuthorizationFilter

    {        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var role = context.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if (role == "Admin")
            {
                var SetUp = context.HttpContext.User.FindFirst("setup").Value;
                var sessionSetUp = context.HttpContext.Session.GetString("setup");
                if (SetUp == "true" || sessionSetUp == "true")
                {
                    context.Result = new RedirectResult("/Admin");
                }
            }
            else if(role == "SuperAdmin")
            {
                context.Result = new RedirectResult("/Manage");
            }
            else
            {
                context.Result = new RedirectResult("/");
            }
        }
    }
}
