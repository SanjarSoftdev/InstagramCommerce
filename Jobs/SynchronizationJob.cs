using InstagramCommerce.Services;
using Quartz;

namespace InstagramCommerce.Jobs
{
    public class SynchronizationJob : IJob
    {
        private readonly SynchronizationService _syncService;

        public SynchronizationJob(SynchronizationService syncService)
        {
            _syncService = syncService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _syncService.SynchronizePostsAsync();
        }
    }
}
