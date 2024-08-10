using InstagramCommerce.Shared.Data;
using InstagramCommerce.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace InstagramCommerce.Shared.Services
{
    public class OrderService
    {
        private readonly InstagramCommerceContext _context;

        public OrderService(InstagramCommerceContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _context.Orders.Include(o => o.Product).ToListAsync();
        }

        public async Task<Order> PlaceOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
            }
        }
    }
}
