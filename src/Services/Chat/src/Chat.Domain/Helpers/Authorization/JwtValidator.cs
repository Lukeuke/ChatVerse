using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Domain.Helpers.Authorization;

public static class JwtValidator
{
    public static bool Validate(this string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;

            if (tokenS is null) return false;
            
            if (tokenS.ValidFrom > DateTime.Now)
            {
                return false;
            }
            
            if (tokenS.ValidTo < DateTime.Now)
            {
                return false;
            }

            var alg = (string)tokenS.Header.First(x => x.Key == "alg").Value;
            
            if (alg is "" or "None")
            {
                return false;
            }

            SecurityToken validatedToken;
            IPrincipal principal = handler.ValidateToken(token, GetValidationParameters(), out validatedToken);
            
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    private static TokenValidationParameters GetValidationParameters()
    {
        const string key = "ADSJKJ219DJ912D29kjdj301d1238j2jd8oj3d32jid3i2dj328032jdj32d80jadhn2819dh21dhu12";
        
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)) //TODO: Get from service settings
        };
    }
}