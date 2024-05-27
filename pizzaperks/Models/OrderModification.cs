namespace pizzaperks.Models
{
    public class OrderModification
    {
        public int Id { get; set; }
        public string? OrderNumber { get; set; }

        public double CostOfModification { get; set; }

        public string? ReasonForModification { get; set; }

        public string? ModifyingUserId { get; set; }
        public virtual PZUser? ModifyingUser { get; set; }
        public virtual Order? Order { get; set; }






    }
}
