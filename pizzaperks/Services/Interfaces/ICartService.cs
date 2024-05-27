using pizzaperks.Models;


namespace pizzaperks.Services.Interfaces
{
	public interface ICartService
	{
		Task<CartProduct?> AddToCartAsync(CartProduct product, PZUser user);
		Task<Cart> CreateNewCartAsync(PZUser user);
		Task<CartProduct?> GetCartProductAsync(int? itemId);
		Task<CartProduct?> UpdateCartProductAsync(CartProduct product);
		Task DeleteFromCartProductAsync(CartProduct product);
		Task RemoveIngredientFromCartProductAsync(OrderedIngredient ingredient);
		Task AddIngredienttoCartProductAsync(OrderedIngredient ingredient, CartProduct product);
		Task<double> CalculateCartTotalAsync(Cart cart);
		Task ClearCartAsync(Cart cart);
		Task<Cart> GetCartWithItemsAsync(PZUser user);
	}
}
