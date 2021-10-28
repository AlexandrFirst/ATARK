using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FireSaverApi.Helpers
{
   [AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {

        public string Role { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (HttpUserContext)context.HttpContext.Items["User"];
            if (user == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else
            {
                if(!String.IsNullOrEmpty(Role))
                {
                    if(!user.RolesList.Contains(this.Role)){
                        context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };
                    }
                }
            }
        }
    }
}