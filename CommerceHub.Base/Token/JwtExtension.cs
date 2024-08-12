using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace CommerceHub.Base.Auth.Token
{
    public static class JwtManager
    {
        public static Session GetSession(HttpContext context)
        {
            Session session = new Session();
            var identity = context.User.Identity as ClaimsIdentity;
            var claims = identity.Claims;
            session.UserName = GetClaimValue(claims, "UserName");
            session.UserId = Convert.ToInt32(GetClaimValue(claims, "UserId"));
            session.Role = GetClaimValue(claims, "Role");
            session.Email = GetClaimValue(claims, "Email");
            session.FullName = GetClaimValue(claims, ClaimTypes.Name);
            return session;
        }

        private static string GetClaimValue(IEnumerable<Claim> claims, string name)
        {
            var claim = claims.FirstOrDefault(c => c.Type == name);
            return claim?.Value;
        }
    }
}
