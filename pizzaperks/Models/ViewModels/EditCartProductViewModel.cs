namespace pizzaperks.Models.ViewModels
{
	public class EditCartProductViewModel
	{
		public CartProduct? CartProduct { get; set; }
		public ICollection<Ingredient>? DefaultIngredients { get; set; }


	}
}
