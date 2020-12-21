using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLoginJdepaz.web.Auth
{
    public class AuthValidations
    {
        public AuthValidations()
        {
        }

        public bool AudienceValidation(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            var castedToken = securityToken as JwtSecurityToken;
            return audiences.FirstOrDefault() == castedToken.Audiences.FirstOrDefault();
        }


        public bool SignInKeyValidation(SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            var castedToken = securityToken as JwtSecurityToken;
            return validationParameters.IssuerSigningKey == securityKey && validationParameters.IssuerSigningKey == castedToken.SigningKey;
        }

        public string RoleClaimTypeRetreiverAssign(SecurityToken securityToken, string roleType)
        {
            var castedToken = securityToken as JwtSecurityToken;
            var roleClaim = castedToken.Claims.SingleOrDefault(s => s.Type == "role");
            return roleClaim.Value;
        }
    }
}
