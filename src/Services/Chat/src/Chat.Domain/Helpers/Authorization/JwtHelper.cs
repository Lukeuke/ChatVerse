using System.IdentityModel.Tokens.Jwt;

namespace Chat.Domain.Helpers.Authorization;

public static class JwtHelper
{
    public static Guid ParseTokenIntoUserIdHeader(string authorization)
    {
        var token = authorization.Split(" ")[1];
        
        var jsonToken = new JwtSecurityToken(token);
        
        var (key, value) = jsonToken.Payload.First(x => x.Key == "id");

        var userId = Guid.Parse(value.ToString()!);

        return userId;
    }
    
    public static Guid ParseTokenIntoUserId(string token)
    {
        var jsonToken = new JwtSecurityToken(token);
        
        var (key, value) = jsonToken.Payload.First(x => x.Key == "id");

        var userId = Guid.Parse(value.ToString()!);

        return userId;
    }
}