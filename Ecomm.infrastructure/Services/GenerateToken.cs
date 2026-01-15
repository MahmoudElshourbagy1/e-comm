using Ecom.Core.Entites;
using Ecom.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecomm.infrastructure.Services
{
    public class GenerateToken : IGenerateToken
    {
        private readonly IConfiguration configuration;
        public GenerateToken(IConfiguration configuration )
        {
            this.configuration = configuration;
        }
        public string GetAndCreateToken(AppUser user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                 new Claim(ClaimTypes.Email,user.Email)
            };
            var Security = configuration["Token:Secret"];
            var key = Encoding.ASCII.GetBytes(Security);
            SigningCredentials credentials = new SigningCredentials(new SymmetricSecurityKey( key),SecurityAlgorithms.HmacSha256);
            SecurityTokenDescriptor tokenDes = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires=DateTime.Now.AddDays(1),
                Issuer= configuration["Token:Issure"],
                SigningCredentials= credentials,
                NotBefore=DateTime.Now,
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDes);
            return handler.WriteToken(token);
        }
    }
}
