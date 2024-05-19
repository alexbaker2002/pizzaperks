using Microsoft.EntityFrameworkCore;
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

		public async Task<Cart> GetCartWithItemsAsync(int? cartId)
		{
			//TODO: Left off Here!!
			if (cartId is null)
			{
				return new Cart();
			}


			Cart? cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == cartId);
			if (cart is null)
			{
				return new Cart();
			}


			return cart;

		}

		public double CalculateOrderTotal(Order order)
		{

			double total = order.OrderedItems.Sum(item => item.Cost);
			return total + (total * 0.775);
		}
	}
}
