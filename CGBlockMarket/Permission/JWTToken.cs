using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CGBlockMarket.Permission
{
    public class JWTToken
    {
        public static dynamic BuildJwtToken(Claim[] claims, PermissionRequirement permissionRequirement)
        {
            var jwt = new JwtSecurityToken(
                    issuer: permissionRequirement.Issuer,
                    audience: permissionRequirement.Audience,
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.Add(permissionRequirement.Expiration),
                    signingCredentials: permissionRequirement.SigningCredentials
                );
            var encodedJwtToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JwtToken
            {
                Status = true,
                access_Token = encodedJwtToken,
                expires = permissionRequirement.Expiration.TotalSeconds,
                token_type = "Bearer"
            };           
        }

        public class JwtToken
        {
            public bool Status { set; get; }
            public string access_Token { set; get; }
            public double expires { set; get; }
            public string token_type { set; get; }
        }
    }
}
