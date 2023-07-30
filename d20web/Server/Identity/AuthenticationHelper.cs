using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace d20Web.Identity
{
    class AuthenticationHelper
    {
        public static string GenerateJwtToken(ApplicationUser user, JwtConfiguration configuration, ApplicationRole[] availableRoles)
        {
            if (string.IsNullOrEmpty(user?.UserName))
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(configuration.Key))
                throw new ArgumentNullException(nameof(configuration));

            Dictionary<string, string?> roleMap = availableRoles.ToDictionary(p => p.Id.ToString(), p => p.Name);

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            foreach (string role in user.Roles)
            {
                if (roleMap.TryGetValue(role, out string? roleName) && roleName != null)
                    claims.Add(new Claim(ClaimTypes.Role, roleName));
                else
                    claims.Add(new Claim(ClaimTypes.Role, role));
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Key));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime expires = DateTime.Now.AddDays(Convert.ToDouble(configuration.ExpireDays));

            JwtSecurityToken token = new JwtSecurityToken(
                configuration.Issuer,
                configuration.Issuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
