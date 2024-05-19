using System.ComponentModel.DataAnnotations.Schema;

namespace pizzaperks.Models
{
	public class Product
	{


		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public double Cost { get; set; }

		[NotMapped]
		public int? OrderId { get; set; }

		// Ingredients

		public virtual ICollection<Ingredient> Ingredients { get; set; } = new HashSet<Ingredient>();

	}
}
