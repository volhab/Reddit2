using Microsoft.Extensions.Options;
using RedditListener.Entities;
using RedditListener.Interfaces;
using System.Net.Http.Headers;
using System.Text;

namespace RedditListener.Services
{
    public class RedditService: IRedditService
    {
        private readonly RedditSettings _settings;
        private readonly HttpClient _httpClient;

        public RedditService(IOptions<RedditSettings> settings, HttpClient httpClient)
        {
            _settings = settings.Value;
            _httpClient = httpClient;
        }

        public async Task<string> GetToken()
        {
            SetHeaders();

            var authenticationString = $"{_settings.AppId}:{_settings.AppSecret}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

            var response = await _httpClient.PostAsync("https://www.reddit.com/api/v1/access_token?grant_type=client_credentials&duration=permanent", null);

            AuthResponse? authResponse = null;
            if (response.IsSuccessStatusCode)
            {
                authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
            }
            else
            {
                await LogError(response);
            }
            return authResponse?.access_token ?? "";
        }

        public async Task<PostResponse?> ReadPosts(string token, string after)
        {
            SetHeaders();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = $"https://oauth.reddit.com/r/{_settings.Subreddit}/new";
            if (!string.IsNullOrEmpty(after))
            {
                url += "?after=" + after;
            }
            var response = await _httpClient.GetAsync(url);

            PostResponse? postResponse = new();
            if (response.IsSuccessStatusCode)
            {
                postResponse = await response.Content.ReadFromJsonAsync<PostResponse>();
            }
            else
            {
                await LogError(response);
            }
            postResponse.responseCode = response.StatusCode;

            foreach (var header in response.Headers)
            {
                switch (header.Key)
                {
                    case "x-ratelimit-used":
                        postResponse.ratelimitUsed = Convert.ToInt32(header.Value.First());
                        break;
                    case "x-ratelimit-remaining":
                        postResponse.ratelimitRemaining = Convert.ToDouble(header.Value.First());
                        break;
                    case "x-ratelimit-reset":
                        postResponse.ratelimitReset = Convert.ToInt32(header.Value.First());
                        break;
                    default:
                        break;

                }
            }
            return postResponse;
        }

        private void SetHeaders()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36");
        }

        private static async Task LogError(HttpResponseMessage response)
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {response.StatusCode}");
            Console.WriteLine($"{error}");
        }
    }
}
