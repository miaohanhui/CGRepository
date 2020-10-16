using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGBlockMarket.Permission
{
    public class PermissionRequirement:IAuthorizationRequirement
    {
        //public List<UserPermission> UserPermissions { private set; get; }

        //public string DeninedAction { set; get; }


        //public PermissionRequirement(List<UserPermission> userPermissons, string deniedAction)
        //{
        //    UserPermissions = userPermissons;
        //    DeninedAction = deniedAction;
        //}

        public List<UserPermission> UserPermissions { private set; get; }

        public string DeniedAction { set; get; }

        public string ClaimType { set; get; }

        public string LoginPath { set; get; }

        public string Issuer { set; get; }

        public string Audience { set; get; }

        public TimeSpan Expiration { set; get; }

        public SigningCredentials SigningCredentials { set; get; }//凭据

        public PermissionRequirement(string deniedAction, string claimType, string issuer, SigningCredentials signingCredentials, string audience, TimeSpan expiration)
        {
            DeniedAction = deniedAction;
            ClaimType = claimType;
            Issuer = issuer;
            Audience = audience;
            Expiration = expiration;
            SigningCredentials = signingCredentials;
        }
    }
}
