using pizzaperks.Models;


namespace pizzaperks.Services.Interfaces
{
	public interface ICartService
	{
		Task<Cart> CreateNewCartAsync(Cart cart);
		Task<Cart> GetCartWithItemsAsync(int? CartId);
		double CalculateOrderTotal(Order order);

	}
}
