using System.IdentityModel.Tokens.Jwt;

namespace Chat.Application.Helpers.Authorization;

public static class JwtValidator
{
    public static bool Validate(this string token)
    {
        var jsonToken = new JwtSecurityToken(token);

        if (jsonToken.Header.Alg is "" or null)
        {
            return false;
        }
        
        if (jsonToken.Payload.ValidTo < DateTime.Now)
        {
            return false;
        }

        if (jsonToken.Payload.ValidFrom > DateTime.Now)
        {
            return false;
        }
        
        return true;
    }
}