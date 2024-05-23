using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pizzaperks.Models;
using pizzaperks.Models.Enums;
using pizzaperks.Services.Interfaces;

namespace pizzaperks.Data
{
    public class DataUtility()
    {



        public static async Task ManageDataAsync(WebApplication app)
        {



            using var svcScope = app.Services.CreateScope();
            var svcProvider = svcScope.ServiceProvider;


            //Service: An instance of DatabaseManager
            var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();
            //Service: An instance of RoleManager
            var roleManagerSvc = svcProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //Service: An instance of the UserManager
            var userManagerSvc = svcProvider.GetRequiredService<UserManager<PZUser>>();
            //Migration: This is the programmatic equivalent to Update-Database

            IOrdersService _ordersService = svcProvider.GetRequiredService<IOrdersService>();



            await dbContextSvc.Database.MigrateAsync();

            await SeedRolesAsync(roleManagerSvc);

            List<PZUser> seededUserAccounts = await SeedDemoUsersAsync(userManagerSvc);

            //TODO: Seed Cart
            await SeedUserCartsAsync(dbContextSvc);

            //seed ingredients 
            List<Ingredient> ingredients = await SeedIngredientsAsync(dbContextSvc);


            //seed Prodcuts
            List<Product> products = await SeedProductsAsync(dbContextSvc, ingredients);
            // seed orders
            await SeedOrdersAsync(dbContextSvc, products, ingredients, _ordersService, seededUserAccounts);
        }



        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.Manager.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Customer.ToString()));
        }

        private static async Task<List<PZUser>> SeedDemoUsersAsync(UserManager<PZUser> userManager)
        {
            List<PZUser> Users = new List<PZUser>();


            //Seed Demo Manager User
            var defaultUser = new PZUser
            {
                UserName = "dman@pizzaperks.com",
                Email = "dman@pizzaperks.com",
                FirstName = "Dustin",
                LastName = "Manager",
                EmailConfirmed = true,
                CartId = -1 // Manager Account

            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {

                    var testUser = await userManager.CreateAsync(defaultUser, "Fw%@P!ZZ@8");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Manager.ToString());
                }
                Users.Add(defaultUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Demo Manager User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }



            //Seed Demo Customer User
            var defaultCustomer = new PZUser
            {
                UserName = "christy@notmymail.com",
                Email = "christy@notmymail.com",
                FirstName = "Christy",
                LastName = "Customer",
                EmailConfirmed = true,
                CartId = 1

            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultCustomer.Email);
                if (user == null)
                {
                    var testUser = await userManager.CreateAsync(defaultCustomer, "Fw%@P!ZZ@8");
                    await userManager.AddToRoleAsync(defaultCustomer, Roles.Customer.ToString());
                }
                Users.Add(defaultCustomer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Demo Customer User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

            //Seed Demo Customer User 1
            var defaultCustomer1 = new PZUser
            {
                UserName = "cari@notmymail.com",
                Email = "cari@notmymail.com",
                FirstName = "Cari",
                LastName = "Customer",
                EmailConfirmed = true,
                CartId = 2

            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultCustomer1.Email);
                if (user == null)
                {
                    var testUser = await userManager.CreateAsync(defaultCustomer1, "Fw%@P!ZZ@8");
                    await userManager.AddToRoleAsync(defaultCustomer1, Roles.Customer.ToString());
                }
                Users.Add(defaultCustomer1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Demo Customer User 1.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

            //Seed Demo Customer User 2
            var defaultCustomer2 = new PZUser
            {
                UserName = "chris@notmymail.com",
                Email = "chris@notmymail.com",
                FirstName = "Chris",
                LastName = "Customer",
                EmailConfirmed = true,
                CartId = 3

            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultCustomer2.Email);
                if (user == null)
                {
                    var testUser = await userManager.CreateAsync(defaultCustomer2, "Fw%@P!ZZ@8");
                    await userManager.AddToRoleAsync(defaultCustomer2, Roles.Customer.ToString());
                }
                Users.Add(defaultCustomer2);
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Demo Customer User 1.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }


            return Users;
        }

        private static async Task SeedUserCartsAsync(ApplicationDbContext context)
        {
            //seed 1 cart as cart Id Num 1
            Cart cart = new Cart();
            try
            {
                //Have we seeded these already?
                var dbCart = context.Carts.FirstOrDefaultAsync(c => c.Id == 1);
                // Select ones that are not already in the database
                await context.Carts.AddAsync(cart);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Cart.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

        }

        private static async Task<List<Ingredient>> SeedIngredientsAsync(ApplicationDbContext context)
        {
            List<Ingredient> ingredients = new List<Ingredient>() {

            new Ingredient { Name = "Tomato Sauce", Description = "Classic pizza sauce", Cost = 1.50 },
            new Ingredient { Name = "Pesto", Description = "Basil pesto sauce", Cost = 2.00 },
            new Ingredient { Name = "Alfredo Sauce", Description = "Creamy white sauce", Cost = 2.50 },
            new Ingredient { Name = "Barbecue Sauce", Description = "Smoky BBQ sauce", Cost = 2.00 },
            new Ingredient { Name = "Garlic Cream Sauce", Description = "Rich garlic cream sauce", Cost = 2.75 },
            new Ingredient { Name = "Olive Oil", Description = "Extra virgin olive oil", Cost = 1.00 },
            new Ingredient { Name = "Buffalo Sauce", Description = "Spicy buffalo sauce", Cost = 2.00 },
            new Ingredient { Name = "Ranch Dressing", Description = "Creamy ranch dressing", Cost = 1.75 },
            new Ingredient { Name = "Mozzarella", Description = "Shredded mozzarella cheese", Cost = 3.00 },
            new Ingredient { Name = "Parmesan", Description = "Grated parmesan cheese", Cost = 3.50 },
            new Ingredient { Name = "Cheddar", Description = "Shredded cheddar cheese", Cost = 2.50 },
            new Ingredient { Name = "Feta", Description = "Crumbled feta cheese", Cost = 2.75 },
            new Ingredient { Name = "Goat Cheese", Description = "Crumbled goat cheese", Cost = 3.00 },
            new Ingredient { Name = "Blue Cheese", Description = "Crumbled blue cheese", Cost = 3.25 },
            new Ingredient { Name = "Provolone", Description = "Sliced provolone cheese", Cost = 2.75 },
            new Ingredient { Name = "Ricotta", Description = "Ricotta cheese", Cost = 2.50 },
            new Ingredient { Name = "Gorgonzola", Description = "Crumbled gorgonzola cheese", Cost = 3.25 },
            new Ingredient { Name = "Gouda", Description = "Shredded gouda cheese", Cost = 3.00 },
            new Ingredient { Name = "Pepperoni", Description = "Sliced pepperoni", Cost = 2.50 },
            new Ingredient { Name = "Sausage", Description = "Ground sausage", Cost = 2.75 },
            new Ingredient { Name = "Bacon", Description = "Crispy bacon pieces", Cost = 3.00 },
            new Ingredient { Name = "Ham", Description = "Diced ham", Cost = 2.50 },
            new Ingredient { Name = "Grilled Chicken", Description = "Grilled chicken strips", Cost = 3.00 },
            new Ingredient { Name = "Ground Beef", Description = "Seasoned ground beef", Cost = 2.75 },
            new Ingredient { Name = "Prosciutto", Description = "Sliced prosciutto", Cost = 4.00 },
            new Ingredient { Name = "Salami", Description = "Sliced salami", Cost = 2.75 },
            new Ingredient { Name = "Anchovies", Description = "Anchovy fillets", Cost = 3.50 },
            new Ingredient { Name = "Meatballs", Description = "Sliced meatballs", Cost = 3.00 },
            new Ingredient { Name = "Bell Peppers", Description = "Sliced bell peppers", Cost = 1.50 },
            new Ingredient { Name = "Onions", Description = "Sliced onions", Cost = 1.25 },
            new Ingredient { Name = "Mushrooms", Description = "Sliced mushrooms", Cost = 2.00 },
            new Ingredient { Name = "Spinach", Description = "Fresh spinach", Cost = 1.50 },
            new Ingredient { Name = "Tomatoes", Description = "Sliced tomatoes", Cost = 1.25 },
            new Ingredient { Name = "Olives", Description = "Sliced olives", Cost = 1.50 },
            new Ingredient { Name = "Jalapeños", Description = "Sliced jalapeños", Cost = 1.25 },
            new Ingredient { Name = "Artichoke Hearts", Description = "Quartered artichoke hearts", Cost = 2.00 },
            new Ingredient { Name = "Zucchini", Description = "Sliced zucchini", Cost = 1.50 },
            new Ingredient { Name = "Eggplant", Description = "Sliced eggplant", Cost = 1.75 },
            new Ingredient { Name = "Arugula", Description = "Fresh arugula", Cost = 1.75 },
            new Ingredient { Name = "Sun-dried Tomatoes", Description = "Sun-dried tomatoes", Cost = 2.50 },
            new Ingredient { Name = "Pineapple", Description = "Pineapple chunks", Cost = 1.50 },
			//new Ingredient { Name = "Apple Slices", Description = "Fresh apple slices", Cost = 1.75 },
			//new Ingredient { Name = "Pear Slices", Description = "Fresh pear slices", Cost = 1.75 },
			new Ingredient { Name = "Figs", Description = "Sliced figs", Cost = 2.75 },
            new Ingredient { Name = "Basil", Description = "Fresh basil leaves", Cost = 1.25 },
            new Ingredient { Name = "Oregano", Description = "Dried oregano", Cost = 0.75 },
            new Ingredient { Name = "Rosemary", Description = "Fresh rosemary", Cost = 1.25 },
            new Ingredient { Name = "Thyme", Description = "Fresh thyme", Cost = 1.25 },
            new Ingredient { Name = "Red Pepper Flakes", Description = "Crushed red pepper flakes", Cost = 0.50 },
            new Ingredient { Name = "Garlic", Description = "Fresh garlic", Cost = 0.75 },
            new Ingredient { Name = "Parsley", Description = "Fresh parsley", Cost = 1.00 },
			//new Ingredient { Name = "Shrimp", Description = "Cooked shrimp", Cost = 4.00 },
			//new Ingredient { Name = "Clams", Description = "Cooked clams", Cost = 3.75 },
			//new Ingredient { Name = "Smoked Salmon", Description = "Sliced smoked salmon", Cost = 5.00 },
			//new Ingredient { Name = "Tuna", Description = "Canned tuna", Cost = 2.25 },
			//new Ingredient { Name = "Pine Nuts", Description = "Toasted pine nuts", Cost = 3.50 },
			//new Ingredient { Name = "Walnuts", Description = "Chopped walnuts", Cost = 2.75 },
			//new Ingredient { Name = "Sesame Seeds", Description = "Toasted sesame seeds", Cost = 1.25 },
			//new Ingredient { Name = "Sunflower Seeds", Description = "Toasted sunflower seeds", Cost = 1.50 },
			new Ingredient { Name = "Capers", Description = "Pickled capers", Cost = 2.00 },
            new Ingredient { Name = "Balsamic Glaze", Description = "Sweet balsamic glaze", Cost = 2.25 },
			//new Ingredient { Name = "Truffle Oil", Description = "Truffle-infused oil", Cost = 4.00 },
			//new Ingredient { Name = "Pickles", Description = "Sliced pickles", Cost = 1.50 },
			//new Ingredient { Name = "Avocado", Description = "Sliced avocado", Cost = 2.75 },
			//new Ingredient { Name = "Corn", Description = "Sweet corn kernels", Cost = 1.25 },
			//new Ingredient { Name = "Capicola", Description = "Sliced capicola", Cost = 3.25 }

			};


            try
            {
                //Have we seeded these already?
                var dbIngredients = context.Ingredients.Select(c => c.Name).ToList();
                // Select ones that are not already in the database
                await context.Ingredients.AddRangeAsync(ingredients.Where(c => !dbIngredients.Contains(c.Name)));
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Ingredients.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

            return ingredients;
        }

        private static async Task<List<Product>> SeedProductsAsync(ApplicationDbContext context, List<Ingredient> ingredients)
        {
            List<Product> products = new List<Product>() {
                new Product
            {

                Name = "Pepperoni Pizza",
                Description = "Classic pepperoni pizza with tomato sauce and mozzarella cheese",
                Cost = 8.99,
                Ingredients = new List<Ingredient>
                {
                    ingredients.Find(i => i.Name == "Tomato Sauce") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Mozzarella") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Pepperoni") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!")
                }
            },
            new Product
            {

                Name = "Veggie Pizza",
                Description = "Healthy veggie pizza with tomato sauce, mozzarella cheese, bell peppers, onions, and olives",
                Cost = 9.99,
                Ingredients = new List<Ingredient>
                {
                    ingredients.Find(i => i.Name == "Tomato Sauce") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Mozzarella") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Bell Peppers") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Onions") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Olives") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!")
                }
            },
            new Product
            {

                Name = "Pesto Chicken Pizza",
                Description = "Delicious pesto pizza with mozzarella cheese and grilled chicken",
                Cost = 10.99,
                Ingredients = new List<Ingredient>
                {
                    ingredients.Find(i => i.Name == "Pesto") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Mozzarella") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Grilled Chicken")?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!")
                }
            },
            new Product
            {

                Name = "Italian Pizza",
                Description = "Traditional Italian pizza with tomato sauce, mozzarella, basil, and olive oil",
                Cost = 11.99,
                Ingredients = new List<Ingredient>
                {
                    ingredients.Find(i => i.Name == "Tomato Sauce") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Mozzarella") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Basil") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Olive Oil") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!")
                }
            },
            new Product
            {

                Name = "Greek Pizza",
                Description = "Mediterranean style pizza with feta cheese, olives, spinach, and artichoke hearts",
                Cost = 12.99,
                Ingredients = new List<Ingredient>
                {
                    ingredients.Find(i => i.Name == "Feta") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Olives") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Spinach") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Artichoke Hearts") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!")
                }
            },
            new Product
            {

                Name = "Caucasian Pizza",
                Description = "Unique pizza with mozzarella, cheddar, and bacon",
                Cost = 13.99,
                Ingredients = new List<Ingredient>
                {
                    ingredients.Find(i => i.Name == "Mozzarella") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Cheddar") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Bacon") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!")
                }
            },
            new Product
            {

                Name = "American Pizza",
                Description = "Classic American pizza with tomato sauce, mozzarella, pepperoni, and ground beef",
                Cost = 10.99,
                Ingredients = new List<Ingredient>
                {
                    ingredients.Find(i => i.Name == "Tomato Sauce") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Mozzarella") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Pepperoni") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Ground Beef") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!")
                }
            },
            new Product
            {

                Name = "Tomato Pie",
                Description = "Traditional tomato pie with tomato sauce, mozzarella, and ricotta",
                Cost = 9.99,
                Ingredients = new List<Ingredient>
                {
                    ingredients.Find(i => i.Name == "Tomato Sauce") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Mozzarella") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!"),
                    ingredients.Find(i => i.Name == "Ricotta") ?? throw new Exception("UNFOUND INGREDIENT WHEN SEEDING !!!")
                }
            }

            };



            try
            {
                //Have we seeded these already?
                var dbProducts = context.Products.Select(c => c.Name).ToList();
                // Select ones that are not already in the database
                await context.Products.AddRangeAsync(products.Where(c => !dbProducts.Contains(c.Name)));
                await context.SaveChangesAsync();
                return products;
            }
            catch (Exception ex)
            {

                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Pizzas.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }


        }
        private static async Task SeedOrdersAsync(ApplicationDbContext context, List<Product> products, List<Ingredient> ingredients, IOrdersService _ordersService, List<PZUser> _seededUsers)
        {
            //TODO: REDO MODELS FOR ORDERS---

            List<Order> orders = new List<Order>()
            {
                new Order
                {

                    OrderNumber = "ORD006",
                    CustomerName = "Christy Customer",
                    CustomerAccount = _seededUsers.Find(user => user.UserName == "christy@notmymail.com"),

                    Status = nameof(Models.Enums.OrderStatusEnum.Preparing),
                    OrderDateTime = DateTime.UtcNow.AddMinutes(-1),
                    OrderedItems = new List<Product>
                    {
                        products.Find(p => p.Name == "Pepperoni Pizza")  ?? throw new Exception("UNFOUND PIZZA WHEN SEEDING !!!"),
                        products.Find(p => p.Name == "Veggie Pizza")  ?? throw new Exception("UNFOUND PIZZA WHEN SEEDING !!!"),
                        products.Find(p => p.Name == "Tomato Pie")  ?? throw new Exception("UNFOUND PIZZA WHEN SEEDING !!!")
                    }
                },
                new Order
                {

                    OrderNumber = "ORD005",
                    CustomerName = "Cari Customer",
                    CustomerAccount = _seededUsers.Find(user => user.UserName == "cari@notmymail.com"),
                    Status = nameof(Models.Enums.OrderStatusEnum.Cooking),
                    OrderDateTime = DateTime.UtcNow.AddMinutes(-3),
                    OrderedItems = new List<Product>
                    {
                        products.Find(p => p.Name == "Pesto Chicken Pizza")  ?? throw new Exception("UNFOUND PIZZA WHEN SEEDING !!!"),
						// add anchovies to below later in method
						products.Find(p => p.Name == "Italian Pizza")  ?? throw new Exception("UNFOUND PIZZA WHEN SEEDING !!!")
                    }


                },
                new Order
                {

                    OrderNumber = "ORD004",
                    CustomerName = "Chris Customer",
                    CustomerAccount = _seededUsers.Find(user => user.UserName == "chris@notmymail.com"),
                    Status = nameof(Models.Enums.OrderStatusEnum.Ready),
                    OrderDateTime = DateTime.UtcNow.AddMinutes(-20),
                     OrderedItems = new List<Product>
                    {
                        products.Find(p => p.Name == "American Pizza")  ?? throw new Exception("UNFOUND PIZZA WHEN SEEDING !!!"),
                        products.Find(p => p.Name == "American Pizza")  ?? throw new Exception("UNFOUND PIZZA WHEN SEEDING !!!"),
                        products.Find(p => p.Name == "Italian Pizza")  ?? throw new Exception("UNFOUND PIZZA WHEN SEEDING !!!"),
                        products.Find(p => p.Name == "Tomato Pie") ?? throw new Exception("UNFOUND PIZZA WHEN SEEDING !!!")
                    }
                },
                new Order
                {

                    OrderNumber = "ORD003",
                    CustomerName = "Christy Customer",
                    CustomerAccount = _seededUsers.Find(user => user.UserName == "christy@notmymail.com"),
                    Status = nameof(Models.Enums.OrderStatusEnum.Complete),
                    OrderDateTime = DateTime.UtcNow.AddDays(-7),
                    OrderedItems = new List<Product>
                    {
                        products.Find(p => p.Name == "American Pizza")  ?? throw new Exception("UNFOUND PIZZA WHEN SEEDING !!!"),
                        products.Find(p => p.Name == "Italian Pizza")  ?? throw new Exception("UNFOUND PIZZA WHEN SEEDING !!!"),
                        products.Find(p => p.Name == "Tomato Pie")  ?? throw new Exception("UNFOUND PIZZA WHEN SEEDING !!!")
                    }
                },
                new Order
                {

                    OrderNumber = "ORD002",
                    CustomerName = "Christy Customer",
                    CustomerAccount = _seededUsers.Find(user => user.UserName == "christy@notmymail.com"),
                    Status = nameof(Models.Enums.OrderStatusEnum.Complete),
                    OrderDateTime = DateTime.UtcNow.AddDays(-10),
                    OrderedItems = new List<Product>
                    {
                        products.Find(p => p.Name == "Pepperoni Pizza")  ?? throw new Exception("UNFOUND PIZZA WHEN SEEDING !!!"),
                        products.Find(p => p.Name == "Greek Pizza")  ?? throw new Exception("UNFOUND PIZZA WHEN SEEDING !!!")
                    }
                },
                new Order
                {
                    OrderNumber = "ORD001",
                    CustomerName = "Cari Customer",
                    CustomerAccount = _seededUsers.Find(user => user.UserName == "cari@notmymail.com"),
                    Status = nameof(Models.Enums.OrderStatusEnum.Complete),
                    OrderDateTime = DateTime.UtcNow.AddDays(-15),
                    OrderedItems = new List<Product>
                    {
                        products.Find(p => p.Name == "Caucasian Pizza")  ?? throw new Exception("UNFOUND PIZZA WHEN SEEDING !!!"),
                        products.Find(p => p.Name == "Tomato Pie")  ?? throw new Exception("UNFOUND PIZZA WHEN SEEDING !!!")
                    }
                }
            };

            //Add rest of Orders Info

            //Strip Order Id's

            // Add Ingredient to Order
            orders.FirstOrDefault(c => c.OrderNumber == "ORD005")!
            .OrderedItems.FirstOrDefault(c => c.Name == "Italian Pizza")!
            .Ingredients.Add(ingredients.FirstOrDefault(c => c.Name == "Anchovies")!);

            //Add cost for Ingredient to order
            orders.FirstOrDefault(c => c.OrderNumber == "ORD005")!
                .OrderedItems.FirstOrDefault(c => c.Name == "Italian Pizza")!
                .Cost += ingredients.FirstOrDefault(c => c.Name == "Anchovies")!.Cost;

            orders.FirstOrDefault(c => c.OrderNumber == "ORD002")!
            .OrderedItems.FirstOrDefault(c => c.Name == "Pepperoni Pizza")!
            .Ingredients.Add(ingredients.FirstOrDefault(c => c.Name == "Pepperoni")!);

            orders.FirstOrDefault(c => c.OrderNumber == "ORD002")!
                .OrderedItems.FirstOrDefault(c => c.Name == "Pepperoni Pizza")!
                .Cost += ingredients.FirstOrDefault(c => c.Name == "Pepperoni")!.Cost;


            foreach (Order order in orders)
            {
                order.OrderTotal = _ordersService.CalculateOrderTotal(order);
            }


            try
            {
                //Have we seeded these already?
                var dbOrders = context.Orders.Select(c => c.OrderNumber).ToList();
                // Select ones that are not already in the database
                await context.Orders.AddRangeAsync(orders.Where(c => !dbOrders.Contains(c.OrderNumber)));
                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Orders.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }



        }
    }
}
