using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pizzaperks.Data;
using pizzaperks.Models;
using pizzaperks.Models.Enums;
using pizzaperks.Services.Interfaces;

namespace pizzaperks.Services
{
	public class OrdersService(ApplicationDbContext context, UserManager<PZUser> userManager, ICartService cartSerivce) : IOrdersService
	{
		private readonly ApplicationDbContext _context = context;
		private readonly UserManager<PZUser> _userManager = userManager;
		private readonly ICartService _cartSerivce = cartSerivce;

		public async Task AddModificationAsync(OrderModification modification, PZUser user)
		{

			Order order = await GetCustomerOrderAsync(modification.OrderNumber!, user);
			order.Alterations = true;
			modification.Order = order;
			_context.OrderModifications.Add(modification);
			_context.Orders.Update(order);

			order.OrderTotal = CalculateOrderTotal(order);

			_context.Orders.Update(order);

			await _context.SaveChangesAsync();


		}

		public double CalculateOrderTotal(Order order)
		{
			double total = order.OrderedItems.Sum(item => item.Cost);
			if (order.Alterations)
			{
				foreach (OrderModification mod in order.OrderModifications)
				{
					total += mod.CostOfModification;
				}
			}

			return Math.Round(total + (total * 0.0775), 2);
		}

		public async Task<Order> CancelOrderAsync(Order order)
		{
			try
			{
				Order? dborder = await _context.Orders.FirstOrDefaultAsync(o => o.OrderNumber == order.OrderNumber);
				if (dborder == null) { throw new NullReferenceException(order.OrderNumber); }
				dborder.Status = nameof(OrderStatusEnum.Canceled);
				_context.Orders.Update(dborder);
				await _context.SaveChangesAsync();

				return order;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message.ToString());
				throw;
			}
		}

		public Task<Order> ChangeOrderAsync(Order order, PZUser pZUser, OrderModification modification)
		{
			//Add Change information to OrderModifications


			throw new NotImplementedException();
		}

		public async Task<Order> ChangeOrderStatusAsync(Order order, OrderStatus status)
		{
			try
			{
				Order? dborder = await _context.Orders.FirstOrDefaultAsync(o => o.OrderNumber == order.OrderNumber);
				if (dborder == null) { throw new NullReferenceException(order.OrderNumber); }
				dborder.Status = status.ToString();
				_context.Orders.Update(dborder);
				await _context.SaveChangesAsync();
				return order;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message.ToString());

				throw;
			}
		}

		public async Task<Order> CreateOrderAsync(PZUser user)
		{


			Cart? cart = await _cartSerivce.GetCartWithItemsAsync(user);
			if (cart is null)
			{
				return null!;
			}
			if (!cart.Products.Any())
			{
				return null!;
			}


			try
			{
				Order order = new Order();
				foreach (CartProduct cartProduct in cart.Products)
				{
					CartProduct cp = new CartProduct
					{
						Cost = cartProduct.Cost,
						Description = cartProduct.Description,
						Modifications = cartProduct.Modifications,
						Name = cartProduct.Name
					};
					cartProduct.Ingredients = _cartSerivce.GetCartProductAsync(cartProduct.Id).Result!.Ingredients;
					foreach (Ingredient ingredient in cartProduct.Ingredients)
					{
						cp.Ingredients.Add(new OrderedIngredient
						{
							Name = ingredient.Name,
							Cost = ingredient.Cost,
							Description = ingredient.Description,
							Double = false,
							Remove = false

						});
					}

					order.OrderedItems.Add(cp);
				}

				order.OrderDateTime = DateTime.UtcNow;
				order.CustomerName = user!.FullName;
				order.CustomerAccount = user;
				order.OrderTotal = Math.Round(CalculateOrderTotal(order), 2);
				order.Alterations = false;
				order.Status = nameof(OrderStatusEnum.Preparing);
				order.PZUserId = user!.Id;



				// save record and get ID
				await _context.AddAsync(order);
				await _context.SaveChangesAsync();
				int recordId = order.Id;
				if (recordId < 99)
				{
					string filler = "";
					if (recordId < 10)
					{
						filler = "00";
					}
					else if (recordId > 9)
					{
						filler = "0";
					}

					order.OrderNumber = $"ORD{filler}{order.Id}";
				}
				else
				{
					order.OrderNumber = $"ORD{recordId}";
				}
				_context.Orders.Update(order);
				await _context.SaveChangesAsync();

				await _cartSerivce.ClearCartAsync(cart);

				return order;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message.ToString());

				throw;
			}
		}

		public Task<List<Order>> GetActiveOrdersAsync()
		{
			throw new NotImplementedException();
		}

		public async Task<List<Order>> GetAllOrdersAsync()
		{
			try
			{
				List<Order> orders = await _context.Orders
					.Include(o => o.OrderedItems)
					.ThenInclude(o => o.Ingredients)
					.ToListAsync();

				orders = orders.OrderByDescending(order => order.OrderDateTime).ToList();
				return orders;
			}
			catch (Exception ex)
			{
				Console.WriteLine("*************  ERROR  *************");
				Console.WriteLine("Error Getting Orders.");
				Console.WriteLine(ex.Message);
				Console.WriteLine("***********************************");
				return null!;
			}


		}

		public async Task<Order> GetCustomerOrderAsync(string orderNumber, PZUser user)
		{
			Order? order = await _context.Orders
				.Include(o => o.OrderedItems)
					.ThenInclude(o => o.Ingredients).
					Include(o => o.OrderModifications).
					FirstOrDefaultAsync(o => o.OrderNumber == orderNumber.ToUpper());
			if (order is null)
			{
				return null!;
			}

			if (await _userManager.IsInRoleAsync(user, "Customer"))
			{
				if (order.PZUserId == user.Id || await _userManager.IsInRoleAsync(user, "Manager"))
				{
					return order;
				}
				else
				{
					return null!;
				}
			}
			return order;



		}

		public Task<List<Order>> GetCustomerOrdersAsync(int userId)
		{
			throw new NotImplementedException();
		}

		public async Task<Order> UpdateOrderAsync(Order order)
		{
			try
			{
				_context.Orders.Update(order);
				await _context.SaveChangesAsync();
				return order;
			}
			catch (Exception)
			{

				throw;
			}
		}


	}
}
