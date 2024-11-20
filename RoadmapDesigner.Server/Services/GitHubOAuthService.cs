using System.Net.Http.Headers;
using System.Text.Json;

namespace RoadmapDesigner.Server.Services
{
    public class GitHubOAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public GitHubOAuthService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<string> GetAccessTokenAsync(string code)
        {
            var response = await _httpClient.PostAsync(_config["GitHubOAuth:TokenEndpoint"],
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                { "client_id", _config["GitHubOAuth:ClientId"] },
                { "client_secret", _config["GitHubOAuth:ClientSecret"] },
                { "code", code },
                { "redirect_uri", _config["GitHubOAuth:RedirectUri"] }
                }));

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var queryParams = System.Web.HttpUtility.ParseQueryString(content);
            return queryParams["access_token"];
        }

        public async Task<GitHubUser> GetUserAsync(string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config["GitHubOAuth:UserEndpoint"]);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Add("User-Agent", "MyReactApp");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GitHubUser>(content);
        }
    }

    public class GitHubUser
    {
        public string Login { get; set; }
        public string AvatarUrl { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
