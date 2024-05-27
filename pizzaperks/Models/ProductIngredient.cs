using System.ComponentModel.DataAnnotations.Schema;

namespace pizzaperks.Models
{
    public class ProductIngredient : Ingredient
    {


        [NotMapped]
        public virtual Product? Product { get; set; }
    }
}
