using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FireSaverApi.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FireSaverApi.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate next;
        private readonly AppSettings appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            this.next = next;
            this.appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IAuthUserService authService, IIoTService ioTService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault();

            if (token != null)
            {
                var token_str = token.Split(" ").Last();


                await attachUserToContext(context, authService, ioTService, token_str);
            }
            else
            {
                var request = context.Request;
                if (request.Path.StartsWithSegments("/socket", StringComparison.OrdinalIgnoreCase) &&
                request.Query.TryGetValue("access_token", out var accessToken))
                {
                    await attachUserToContext(context, authService, ioTService, accessToken);
                }
            }

            await next(context);
        }

        private async Task attachUserToContext(HttpContext context, IAuthUserService authService, IIoTService ioTService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.Secret);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var requestorId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                var userType = jwtToken.Claims.First(x => x.Type == "type").Value;

                // attach user to context on successful jwt validation
                if (userType == "iot")
                {
                    context.Items["User"] = await ioTService.GetIotContext(requestorId);
                }
                else
                {
                    context.Items["User"] = await authService.GetUserContext(requestorId);
                }
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}