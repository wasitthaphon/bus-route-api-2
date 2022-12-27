using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BusRouteApi.Helpers;
using BusRouteApi.RequestModels;
using BusRouteApi.ServiceLayer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BusRouteApi.JwtMiddleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, UserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                await attachUserToContext(context, userService, token);
            }

            await _next(context);
        }


        private async Task attachUserToContext(HttpContext context, UserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "Id").Value);

                UserBody userBody;
                Exception e;

                (userBody, e) = await userService.GetUser(userId);

                context.Items["User"] = userBody;
            }
            catch
            {
            }
        }
    }
}