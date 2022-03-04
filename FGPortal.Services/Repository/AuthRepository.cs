using FGPortal.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace FGPortal.Services
{
    public class AuthRepository : IAuthRepository
    {
        public IConfiguration _configuration;
        public AuthRepository(IConfiguration config)
        {
            _configuration = config;
        }
        public AuthLoginTokenVM GetAuthData(AuthLoginTokenVM model)
        {
            var expirationTime = DateTime.UtcNow.AddSeconds(Convert.ToInt32(_configuration["Jwt:JWTLifespan"]));

            //create claims details based on the user information
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", model.UserId.ToString()),
                        new Claim("UserName", model.UserName),
                        new Claim("Email", model.Email)
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new[]
            //    {
            //        new Claim("UserId", model.UserId.ToString()),
            //        new Claim("UserName", model.UserName.ToString()),
            //        new Claim("Email", model.Email.ToString())
            //    }),
            //    Expires = expirationTime,
            //    SigningCredentials = new SigningCredentials(
            //        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            //        SecurityAlgorithms.HmacSha256Signature
            //    )
            //};
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            model.Token = new JwtSecurityTokenHandler().WriteToken(token);
            model.TokenExpirationTime = ((DateTimeOffset)expirationTime).ToUnixTimeSeconds();

            return model;
        }

        public string HashPassword(string password)
        {
            return BC.HashPassword(password);
        }

        public bool VerifyPassword(string actualPassword, string hashedPassword)
        {
            try
            {
                return BC.Verify(actualPassword, hashedPassword);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
