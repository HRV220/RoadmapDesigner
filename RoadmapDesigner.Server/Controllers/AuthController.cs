using Microsoft.AspNetCore.Mvc;
using RoadmapDesigner.Server.Services;

namespace RoadmapDesigner.Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly OAuthService _oAuthService;

        public AuthController(OAuthService oAuthService)
        {
            _oAuthService = oAuthService;
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromForm] string refreshToken)
        {
            try
            {
                var newAccessToken = await _oAuthService.RefreshAccessTokenAsync(
                    refreshToken,
                    "ваш_client_id",
                    "ваш_client_secret"
                );

                return Ok(new { access_token = newAccessToken });
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(new { error = "Failed to refresh token", details = ex.Message });
            }
        }
    }

}
