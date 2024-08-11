namespace InstagramCommerce.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContact { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }
}
