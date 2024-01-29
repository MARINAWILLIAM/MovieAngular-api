using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Security.Core;
using Security.Core.SecurityEntites;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Securityservices
{
    public class TokenServices : ItokenServices
    {
        private readonly IConfiguration _configuration;

        public TokenServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            //Private Claims [USER-DEFINED]
            var authClaims=new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,user.DisplayName),
                new Claim(ClaimTypes.Email,user.Email),

            };
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
           // var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            //create token
            var token = new JwtSecurityToken(
                 issuer: _configuration["Jwt:ValidationIssure"],
         audience: _configuration["Jwt:ValidationAudience"],
         expires: DateTime.Now.AddDays(double.Parse(_configuration["Jwt:DurationInDays"])),
         claims: authClaims,
         //payload
        signingCredentials: new SigningCredentials(authkey, SecurityAlgorithms.HmacSha256Signature)
        //key
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
            //                    obj              method   token string

        }
    }
}
