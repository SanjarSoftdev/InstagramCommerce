using InstagramCommerce.Models;
using Newtonsoft.Json;

namespace InstagramCommerce.Services
{
    public class InstagramService
    {
        private readonly HttpClient _httpClient;

        public InstagramService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<InstagramPost>> GetPostsAsync(string accessToken)
        {
            // Call Instagram API to fetch posts
            var response = await _httpClient.GetAsync($"https://graph.instagram.com/me/media?fields=id,caption&access_token={accessToken}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            // Deserialize and process the content
            var posts = JsonConvert.DeserializeObject<List<InstagramPost>>(content);
            return posts;
        }
    }

    public class InstagramResponse
    {
        [JsonProperty("data")]
        public List<InstagramPost> Data { get; set; }
    }
}
