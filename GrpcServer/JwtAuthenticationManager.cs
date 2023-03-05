using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GrpcServer;

public static class JwtAuthenticationManager
{
  private static readonly int JWT_TOKEN_VALIDITY = 30;

  private static readonly string JWT_TOKEN_KEY = "0ff455a2708394633e4bb2f88002e3cd80cbd76f";

  private static byte[] TokenKey => Encoding.ASCII.GetBytes(JWT_TOKEN_KEY);

  private static DateTime TokenExpiryDateTime => DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY);

  public static SymmetricSecurityKey SymmetricSecurityKey => new(TokenKey);

  public static string Issuer => "nhanvuong.vn";

  public static string Audience => "nhanvuong.vn";

  private static SigningCredentials SigningCredentials => new(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

  public static AuthenticationResponse Authenticate(AuthenticationRequest authenticationRequest)
  {
    JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

    var claims = new List<Claim>
    {
      new(ClaimTypes.Name, authenticationRequest.UserName)
    };

    JwtSecurityToken jwtSecurityToken = new(
      claims: claims,
      issuer: Issuer,
      audience: Audience,
      expires: TokenExpiryDateTime,
      signingCredentials: SigningCredentials
    );

    var token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

    return new()
    {
      AccessToken = token,
      ExpiresIn = (int)TokenExpiryDateTime.Subtract(DateTime.Now).TotalSeconds
    };
  }
}
