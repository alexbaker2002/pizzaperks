namespace pizzaperks.Models.ViewModels
{
	public class EditCartProductViewModel
	{
		public string? OrderNumber { get; set; }
		public CartProduct? CartProduct { get; set; }
		public ICollection<Ingredient>? DefaultIngredients { get; set; }

		public OrderModification? Mods { get; set; }
	}
}
