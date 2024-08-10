using InstagramCommerce.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace InstagramCommerce.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SynchronizationController : ControllerBase
    {
        private readonly SynchronizationService _syncService;

        public SynchronizationController(SynchronizationService syncService)
        {
            _syncService = syncService;
        }

        [HttpPost]
        public async Task<IActionResult> Synchronize()
        {
            await _syncService.SynchronizePostsAsync();
            return Ok();
        }
    }
}
