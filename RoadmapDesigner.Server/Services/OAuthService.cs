using System.Text.Json;

namespace RoadmapDesigner.Server.Services
{
    public class OAuthService
    {
        private readonly HttpClient _httpClient;

        public OAuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> RefreshAccessTokenAsync(string refreshToken, string clientId, string clientSecret)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth.tpu.ru/access_token")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken }
            })
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            return json.RootElement.GetProperty("access_token").GetString();
        }
    }

}
