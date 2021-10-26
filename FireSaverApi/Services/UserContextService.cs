using FireSaverApi.Contracts;
using FireSaverApi.Helpers;
using FireSaverApi.Helpers.ExceptionHandler.CustomExceptions;
using Microsoft.AspNetCore.Http;

namespace FireSaverApi.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor contextAccessor;

        public UserContextService(IHttpContextAccessor accessor)
        {
            this.contextAccessor = accessor;
        }

        private HttpContext Context
        {
            get
            {
                return contextAccessor.HttpContext;
            }
        }

        public HttpUserContext GetUserContext()
        {
            var userContext = (HttpUserContext)Context.Items["User"];
            if (userContext == null)
            {
                throw new UserContextNotFoundException();
            }

            return userContext;
        }
    }
}