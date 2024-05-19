using Microsoft.EntityFrameworkCore;
using pizzaperks.Data;
using pizzaperks.Models;
using pizzaperks.Services.Interfaces;

namespace pizzaperks.Services
{
    public class OrdersService(ApplicationDbContext context) : IOrdersService
    {
        private readonly ApplicationDbContext _context = context;

        public double CalculateOrderTotal(Order order)
        {

            double total = order.OrderedItems.Sum(item => item.Cost);
            return total + (total * 0.775);
        }

        public Task<Order> CancelOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public Task<Order> CreateOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetActiveOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            List<Order> orders = await _context.Orders.ToListAsync();
            orders = orders.OrderByDescending(order => order.OrderDateTime).ToList();
            return orders;
        }

        public Task<List<Order>> GetCustomerOrdersAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<Order> UpdateOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
