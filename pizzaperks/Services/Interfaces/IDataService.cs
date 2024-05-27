using pizzaperks.Models;

namespace pizzaperks.Services.Interfaces
{
	public interface IDataService
	{
		Task<List<Ingredient>> GetIngredientsAsync();
		Task<Product?> GetProductAsync(int? id);
		Task<List<Product>> GetProductsAsync();

		Task<Ingredient?> GetIngredientByIdAsync(int id);


	}
}
