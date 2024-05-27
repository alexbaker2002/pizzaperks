namespace pizzaperks.Models
{
    public class OrderedIngredient : Ingredient
    {
        public bool Remove { get; set; }
        public bool Double { get; set; }
        public virtual CartProduct? Product { get; set; }
    }
}
