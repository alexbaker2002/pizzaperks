using Microsoft.EntityFrameworkCore;
using pizzaperks.Data;
using pizzaperks.Models;
using pizzaperks.Services.Interfaces;

namespace pizzaperks.Services
{
	public class DataService(ApplicationDbContext context) : IDataService
	{
		private readonly ApplicationDbContext _context = context;

		public async Task<Ingredient?> GetIngredientByIdAsync(int id)
		{
			return await _context.Ingredients.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<List<Ingredient>> GetIngredientsAsync()
		{
			try
			{
				return await _context.Ingredients.ToListAsync();
			}
			catch (Exception)
			{

				throw;
			}


		}

		public async Task<Product?> GetProductAsync(int? id)
		{
			try
			{
				return await _context.Products.Include(p => p.Ingredients).FirstOrDefaultAsync(p => p.Id == id);
			}
			catch (Exception)
			{

				throw;
			}

		}

		public async Task<List<Product>> GetProductsAsync()
		{
			return await _context.Products.ToListAsync();
		}
	}
}
