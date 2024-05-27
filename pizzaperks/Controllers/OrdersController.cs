using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pizzaperks.Data;
using pizzaperks.Models;
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

			var order = await _context.Orders
				.Include(o => o.CustomerAccount)
				.Include(o => o.OrderedItems)
				.FirstOrDefaultAsync(m => m.OrderNumber == orderNumber);


			if (order == null)
			{
				return NotFound();
			}
			PZUser? user = await _userManager.GetUserAsync(User);

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

		// POST: Orders/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,OrderNumber,OrderTotal,CustomerName,Status,OrderDateTime,Alterations,PZUserId")] Order order)
		{
			if (ModelState.IsValid)
			{
				_context.Add(order);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["PZUserId"] = new SelectList(_context.Users, "Id", "Id", order.PZUserId);
			return View(order);
		}

		// GET: Orders/Edit/5
		[Authorize(Roles = "Manager")]
		[Route("/Orders/Edit/{orderNumber}")]
		public async Task<IActionResult> Edit(string orderNumber)
		{
			if (orderNumber == null)
			{
				return NotFound();
			}

			var order = await _context.Orders.FirstOrDefaultAsync(c => c.OrderNumber == orderNumber);
			if (order == null)
			{
				return NotFound();
			}

			return View(order);
		}

		// POST: Orders/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,OrderNumber,OrderTotal,CustomerName,Status,OrderDateTime,Alterations,PZUserId")] Order order)
		{
			if (id != order.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(order);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!OrderExists(order.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["PZUserId"] = new SelectList(_context.Users, "Id", "Id", order.PZUserId);
			return View(order);
		}

		// GET: Orders/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var order = await _context.Orders
				.Include(o => o.CustomerAccount)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (order == null)
			{
				return NotFound();
			}

			return View(order);
		}

		// POST: Orders/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var order = await _context.Orders.FindAsync(id);
			if (order != null)
			{
				_context.Orders.Remove(order);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool OrderExists(int id)
		{
			return _context.Orders.Any(e => e.Id == id);
		}
	}
}
