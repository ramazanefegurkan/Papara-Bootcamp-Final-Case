using CommerceHub.Base.Auth.Token;
using CommerceHub.Data.Domain;
using CommerceHub.Schema;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.Auth.Token
{
    public class TokenService : ITokenService
    {
        private readonly JwtConfig jwtConfig;

        public TokenService(JwtConfig jwtConfig)
        {
            this.jwtConfig = jwtConfig;
        }
        public string GetToken(User user)
        {
            Claim[] claims = GetClaims(user);
            var secret = Encoding.ASCII.GetBytes(jwtConfig.Secret);

            JwtSecurityToken jwtToken = new JwtSecurityToken(
                jwtConfig.Issuer,
                jwtConfig.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(jwtConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret),
                    SecurityAlgorithms.HmacSha256Signature)
            );

            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return token;
        }

        private Claim[] GetClaims(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("Email", user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim("UserName", user.UserName)
            };

            return claims.ToArray();
        }
    }
}
