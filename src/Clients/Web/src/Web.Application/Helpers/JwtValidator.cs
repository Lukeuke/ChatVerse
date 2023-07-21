using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace Web.Application.Helpers;

public static class JwtValidator
{
    public static bool ValidateJwt(this string jwt)
    {
        var claims = ParseClaimsFromJwt(jwt);

        var exp = claims.FirstOrDefault(x => x.Type == "exp");
       
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(jwt);
        var tokenS = jsonToken as JwtSecurityToken;

        if (long.Parse(exp!.Value) < DateTimeOffset.Now.ToUnixTimeSeconds())
        {
            Console.WriteLine("token has expired");
            return false;
        }
        

        if (tokenS.Header.Alg is "" or "None")
        {
            Console.WriteLine("invalid signature alg");
            return false;
        }

        return true;
    }
    
    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}