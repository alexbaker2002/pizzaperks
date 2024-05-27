using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pizzaperks.Models;

namespace pizzaperks.Data
{
    public class ApplicationDbContext : IdentityDbContext<PZUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }

        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductIngredient> ProductIngredients { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderModification> OrderModifications { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }

        public virtual DbSet<CartProduct> CartProducts { get; set; }
        public virtual DbSet<OrderedIngredient> OrderedIngredients { get; set; }









    }
}
