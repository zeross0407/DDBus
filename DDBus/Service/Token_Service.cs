using DDBus.Entity;
using DDBus.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
namespace DDBus.Service
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;


        public TokenService(IConfiguration configuration, IOptions<DatabaseSettings> DatabaseSettings)
        {
            _configuration = configuration;
        }

        private string GetCollectionName<T>()
        {
            var attribute = typeof(T).GetCustomAttribute<CollectionNameAttribute>();
            return attribute?.Name ?? typeof(T).Name;
        }

        // Tạo một refresh token ngẫu nhiên
        public async Task<string> GenerateToken(string user_id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityTokenHandler jwt_handler = new JwtSecurityTokenHandler();
            var token = new JwtSecurityToken
                (
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddHours(10),
                    signingCredentials: credentials,
                    claims: new[]
                    {
                    new Claim(JwtRegisteredClaimNames.Sub, user_id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    }
                );
            return jwt_handler.WriteToken(token);
        }


    }

}
