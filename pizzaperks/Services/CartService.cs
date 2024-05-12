using pizzaperks.Data;
using pizzaperks.Models;
using pizzaperks.Services.Interfaces;


namespace pizzaperks.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartService> _logger;
        public CartService(ApplicationDbContext context, ILogger<CartService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Cart> CreateNewCartAsync(Cart cart)
        {
            if (cart is not null)
                try
                {
                    _context.Add(cart);
                    await _context.SaveChangesAsync();
                    return cart;
                }
                catch (Exception ex)
                {
                    _logger.LogError(80, ex.Message);
                    return null!;
                }
            return null!;
        }
    }
}
