using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DemoApp1.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext) {
            if (!httpContext.Request.Cookies.TryGetValue("site1url", out var tok)) {
                httpContext.Response.Redirect("https://mcs425-jterhark.azurewebsites.net/Account/Login?msg=cookienotfound");
                return Task.CompletedTask;
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("WOl3brI9HCoHGPsbhrAoe5XhAv7wCKqOPNrQoI9DLkLuYEEt276C3IGojXR5qnsX"));

            var options = new TokenValidationParameters {
                ClockSkew = TimeSpan.FromMinutes(2),
                IssuerSigningKey = key,
                //RequireSignedTokens = true,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidAudience = "azurewebsites.net",
                ValidateIssuer = true,
                ValidIssuer = "mcs425-jterhark.azurewebsites.net"
            };

            try {
                var result = new JwtSecurityTokenHandler().ValidateToken(tok, options, out var tmp);
            }
            catch (SecurityTokenValidationException s) {
                httpContext.Response.Redirect(
                    "https://mcs425-jterhark.azurewebsites.net/Account/Login?msg=" + s.Message.ToString());
            }
            catch (ArgumentException) {
                httpContext.Response.Redirect(
                    "https://mcs425-jterhark.azurewebsites.net/Account/Login?msg=malformedtoken");
            }

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class JwtMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtMiddleware>();
        }
    }
}
