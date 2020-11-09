using System.Collections.Generic;
using System.Security.Claims;
using System;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using ApiInterpares.Settings;
using System.Threading.Tasks;

namespace ApiInterpares.Services
{
    public class TokenService
    {
        private readonly UserManager<IdentityUser> _signInManager;
        private readonly JwtSecurityTokenSettings _jwt;

        public TokenService(UserManager<IdentityUser> signInManager, JwtSecurityTokenSettings jwt)
        {
            _signInManager = signInManager;
            this._jwt = jwt;
        }
        public async Task<JwtSecurityToken> GenerateToken(IdentityUser user)
        {
            var userClaims = await _signInManager.GetClaimsAsync(user).ConfigureAwait(false);
            var roles = await _signInManager.GetRolesAsync(user).ConfigureAwait(false);

            var roleClaims = new List<Claim>();
            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }
            string ipAndress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", ipAndress)               
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
