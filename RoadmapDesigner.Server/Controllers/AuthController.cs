using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;  // Для доступа к конфигурации
using RoadmapDesigner.Server.Services;

namespace RoadmapDesigner.Server.Controllers
{
    public class AuthController : Controller
    {
        private readonly GitHubOAuthService _githubOAuthService;
        private readonly IConfiguration _config; // добавляем зависимость

        // Внедряем IConfiguration через конструктор
        public AuthController(GitHubOAuthService githubOAuthService, IConfiguration config)
        {
            _githubOAuthService = githubOAuthService;
            _config = config; // сохраняем ссылку на конфигурацию
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            // Используем _config для получения настроек
            var githubAuthUrl = $"https://github.com/login/oauth/authorize?client_id={_config["GitHubOAuth:ClientId"]}&redirect_uri={_config["GitHubOAuth:RedirectUri"]}";
            return Redirect(githubAuthUrl);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("Code not provided");

            var accessToken = await _githubOAuthService.GetAccessTokenAsync(code);
            var user = await _githubOAuthService.GetUserAsync(accessToken);

            // Верните данные пользователя или создайте JWT
            return Ok(user);
        }
    }
}

