using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication2.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private const string Key = "AuthToken";
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // 簡易認証（実際はDBでチェック）
            if (request.Username == "test" && request.Password == "password")
            {
                var token = GenerateJwtToken(request.Username);
                // HttpOnly Cookie に保存
                Response.Cookies.Append(Key, token, new CookieOptions
                {
                    HttpOnly = true, // JavaScriptからアクセス不可
                    Secure = true,   // HTTPS通信時のみ送信
                    SameSite = SameSiteMode.Strict, // CSRF対策（同一サイトのみ送信）
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });
                //return Ok(new { token });

                // トークンはJSONで返さない
                return Ok(new { message = "Login successful" });
            }
            return Unauthorized();
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Cookieを削除（有効期限を過去に設定）
            Response.Cookies.Append(Key, "",
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(-1)
                });

            return Ok(new { message = "ログアウトしました" });
        }

        private string GenerateJwtToken(string username)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username)
        };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public record LoginRequest(string Username, string Password);

}
