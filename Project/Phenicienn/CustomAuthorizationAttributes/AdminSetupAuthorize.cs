using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Phenicienn.Data;
using System.Security.Claims;

namespace Phenicienn.CustomAuthorizationAttributes
{
    public class AdminSetupAuthorize : AuthorizeAttribute, IAuthorizationFilter

    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var role = context.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if (role == "Admin")
            {
                var SetUp = context.HttpContext.User.FindFirst("setup").Value;
                var sessionSetUp = context.HttpContext.Session.GetString("setup");
                if ((SetUp == "false" || SetUp == null) && (sessionSetUp == "false" || sessionSetUp == null))
                {
                    var errorMessage = "You Should set up your account first";
                    context.HttpContext.Session.SetString("ErrorMessage", errorMessage);
                    context.Result = new RedirectToActionResult("Index", "Setup", new { area = "Admin" });
                }
            }
            else if (role == "SuperAdmin")
            {
                context.Result = new RedirectResult("/Manage");
            }
            else
                context.Result = new RedirectResult("/");
        }
    }
}
