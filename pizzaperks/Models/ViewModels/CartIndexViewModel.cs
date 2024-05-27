namespace pizzaperks.Models.ViewModels
{
	public class CartIndexViewModel
	{
		public ICollection<CartProduct>? CartProducts { get; set; }
		public ICollection<Product>? DefaultProducts { get; set; }
		public ICollection<Ingredient>? DefaultIngredients { get; set; }

		public double OrderTotal { get; set; } = 0.00;

		public int cartId { get; set; }

	}
}
