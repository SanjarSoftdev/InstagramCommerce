namespace InstagramCommerce.Shared.Models
{
    public class SynchronizationLog
    {
        public int Id { get; set; }
        public DateTime SyncTime { get; set; }
        public string Status { get; set; }
    }
}
