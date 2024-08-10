using InstagramCommerce.Shared.Data;
using InstagramCommerce.Shared.Models;

namespace InstagramCommerce.Shared.Services
{
    public class SynchronizationService
    {
        private readonly InstagramService _instagramService;
        private readonly InstagramCommerceContext _context;

        public SynchronizationService(InstagramService instagramService, InstagramCommerceContext context)
        {
            _instagramService = instagramService;
            _context = context;
        }

        public async Task SynchronizePostsAsync()
        {
            var accessToken = "YourAccessToken"; // Replace with your actual access token

            var posts = await _instagramService.GetPostsAsync(accessToken);
            foreach (var post in posts)
            {
                var product = new Product
                {
                    Name = post.Caption,
                    Description = post.Caption,
                    ImageUrl = post.MediaUrl,
                    // Other properties can be filled based on post details or defaults
                };
                _context.Products.Add(product);
            }
            await _context.SaveChangesAsync();

            var log = new SynchronizationLog
            {
                SyncTime = DateTime.Now,
                Status = "Success"
            };
            _context.SynchronizationLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
