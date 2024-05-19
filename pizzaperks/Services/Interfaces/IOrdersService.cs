using pizzaperks.Models;

namespace pizzaperks.Services.Interfaces
{
    public interface IOrdersService
    {
        Task<List<Order>> GetCustomerOrdersAsync(int userId);
        Task<List<Order>> GetActiveOrdersAsync();
        Task<List<Order>> GetAllOrdersAsync();

        Task<Order> CreateOrderAsync(Order order);

        Task<Order> UpdateOrderAsync(Order order);
        Task<Order> CancelOrderAsync(Order order);

        double CalculateOrderTotal(Order order);
    }
}
