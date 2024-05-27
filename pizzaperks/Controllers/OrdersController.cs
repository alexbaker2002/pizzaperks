using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pizzaperks.Data;
using pizzaperks.Models;
using pizzaperks.Models.ViewModels;
using pizzaperks.Services.Interfaces;

namespace pizzaperks.Controllers
{
	public class OrdersController(ApplicationDbContext context, UserManager<PZUser> userManager,
		ICartService cartService,
		IDataService dataService, IOrdersService ordersService) : Controller
	{
		private readonly ApplicationDbContext _context = context ?? null!;
		private readonly UserManager<PZUser> _userManager = userManager ?? null!;
		private readonly ICartService _cartService = cartService ?? null!;
		private readonly IDataService _dataService = dataService ?? null!;
		private readonly IOrdersService _ordersService = ordersService ?? null!;



		// GET: Orders
		public async Task<IActionResult> Index()
		{
			var applicationDbContext = _context.Orders.Include(o => o.CustomerAccount);
			return View(await applicationDbContext.ToListAsync());
		}

		// GET: Orders/Details/5
		[Route("/Orders/Details/{orderNumber}")]

		public async Task<IActionResult> Details(string orderNumber)
		{
			if (orderNumber == null)
			{
				return NotFound();
			}
			PZUser? user = await _userManager.GetUserAsync(User);

			var order = await _ordersService.GetCustomerOrderAsync(orderNumber, user!);


			if (order == null)
			{
				return NotFound();
			}

			if (order.PZUserId != user!.Id)
			{
				RedirectToAction("Index", "Home");
			}

			return View(order);
		}

		// GET: Orders/Create
		public async Task<IActionResult> Create()
		{
			PZUser? user = await _userManager.GetUserAsync(User);
			if (user is null)
			{
				RedirectToAction("Index", "Home");
			}
			Order order = await _ordersService.CreateOrderAsync(user!);
			if (order is null)
			{
				return RedirectToAction("Index", "Home");
			}

			return View(order);
		}

		// GET: Orders/Edit/ORD001
		[Authorize(Roles = "Manager")]
		[Route("/Orders/Edit/{orderNumber}/{itemNumber}")]
		public async Task<IActionResult> Edit(string? orderNumber, int? itemNumber)
		{
			if (itemNumber == null)
			{
				return NotFound();
			}
			PZUser? user = await _userManager.GetUserAsync(User);
			CartProduct? item = await _cartService.GetCartProductAsync(itemNumber);

			if (item == null)
			{
				return NotFound();
			}
			EditCartProductViewModel model = new EditCartProductViewModel()
			{
				CartProduct = item,
				DefaultIngredients = _dataService.GetIngredientsAsync().Result.Distinct().ToList(),
				OrderNumber = orderNumber

			};


			return View(model);
		}


		[Authorize(Roles = "Manager")]
		[Route("/Orders/ManagerOverride/{orderNumber}")]
		public async Task<IActionResult> ManagerOverride(string? orderNumber)
		{
			OrderModification mod = new OrderModification();
			if (orderNumber == null) { return RedirectToAction("Dashboard", "Home"); }
			PZUser? manager = await _userManager.GetUserAsync(User);
			Order order = await _ordersService.GetCustomerOrderAsync(orderNumber, manager!);

			mod.OrderNumber = orderNumber;
			mod.ModifyingUser = manager;
			mod.Order = order;




			return View(mod);

		}


		[Route("/Orders/ManagerOverride/{orderNumber}")]
		[Authorize(Roles = "Manager")]
		[HttpPost]

		public async Task<IActionResult> ManagerOverride([Bind("OrderNumber", "CostOfModification", "ReasonForModification")] OrderModification modification)
		{
			PZUser? user = await _userManager.GetUserAsync(User);
			if (!ModelState.IsValid)
			{
				return View(modification);
			}
			modification.ModifyingUser = user;
			modification.ModifyingUserId = user!.Id;


			await _ordersService.AddModificationAsync(modification, user);


			return RedirectToAction("Dashboard", "Home");
		}

	}
}
