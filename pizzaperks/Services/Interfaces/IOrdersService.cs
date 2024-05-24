using pizzaperks.Models;

namespace pizzaperks.Services.Interfaces
{
    public interface IOrdersService
    {
        Task<List<Order>> GetCustomerOrdersAsync(int userId);
        Task<List<Order>> GetActiveOrdersAsync();
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> GetCustomerOrderAsync(string orderNumber, PZUser user);

        Task<Order> CreateOrderAsync(Order order);

        Task<Order> UpdateOrderAsync(Order order);
        Task<Order> CancelOrderAsync(Order order);

        Task<Order> ChangeOrderStatusAsync(Order order, OrderStatus status);

        Task<Order> ChangeOrderAsync(Order order, PZUser pZUser, OrderModification modification);

        double CalculateOrderTotal(Order order);
    }
}
