using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FireSaverApi.Helpers
{
    [AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {

        public string[] Roles { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (MyHttpContext)context.HttpContext.Items["User"];
            if (user == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else
            {
                if (Roles != null)
                {
                    if (user.RolesList.Intersect(this.Roles).Count() == 0)
                    {
                        context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };
                    }
                }
            }
        }
    }
}