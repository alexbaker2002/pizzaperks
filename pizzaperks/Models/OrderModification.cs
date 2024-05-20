namespace pizzaperks.Models
{
    public class OrderModification
    {
        public int Id { get; set; }
        public string? OrderNumber { get; set; }
        public int LineItem { get; set; }
        public int IngredientId { get; set; }
        public double CostOfModification { get; set; }
        public bool LeaveIngredientOffProduct { get; set; } = false;
        public bool AddDoubleIngredient { get; set; }
        public string? ReasonForModification { get; set; }

        public string? ModifyingUserId { get; set; }

        public virtual PZUser? ModifyingUser { get; set; }
        public virtual Ingredient? Ingredient { get; set; }






    }
}
