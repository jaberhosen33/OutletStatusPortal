using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutletStatusPortal.Models;
using System.Linq;
using System.Threading.Tasks;
namespace OutletStatusPortal.Controllers
{
  




    public class StockController : BaseController
    {
        //private readonly Outletdbcontext _context;

        public StockController(Outletdbcontext context) : base(context)
        {
            
        }

        // View all stock items
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ViewStocks()
        {
            var stockItems = await _context.StockItems.ToListAsync();
            return View(stockItems);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BeforeSeupView()
        {
            var setups = await _context.BeforeOutletSetUps
                .Include(b => b.DeviceStatuses)
                .ToListAsync();

            return View(setups);
        }
        // GET: Assign stock form
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Assign()
        {
            ViewBag.StockItems = await _context.StockItems.ToListAsync();
            return View();
        }

        // POST: Assign stock to outlet
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(BeforeOutletSetUp model)
        {
            var stock = await _context.StockItems.FindAsync(model.StockItemId);
            if (stock == null) return NotFound();

            // Check stock availability
            if (stock.Pos < model.Pos || stock.Om < model.Om || stock.Server < model.Server ||
                stock.Router < model.Router || stock.Scanner < model.Scanner || stock.Icmo < model.Icmo)
            {
                ModelState.AddModelError("", "Not enough stock to assign");
                ViewBag.StockItems = await _context.StockItems.ToListAsync();
                return View(model);
            }

            // Reduce stock
            stock.Pos -= model.Pos;
            stock.Om -= model.Om;
            stock.Server -= model.Server;
            stock.Router -= model.Router;
            stock.Scanner -= model.Scanner;
            stock.Icmo -= model.Icmo;
            //stock.StockType = model.StockType;
            _context.BeforeOutletSetUps.Add(model);
            _context.StockTransactions.Add(new StockTransaction
            {
                StockItemId = stock.Id,
                OperationType = OperationType.Assign,
                OutletCode = model.OutletName,
                Pos = model.Pos,
                Om = model.Om,
                Server = model.Server,
                Router = model.Router,
                Scanner = model.Scanner,
                Icmo = model.Icmo,
                PerformedBy = ViewBag.CurrentUserName
            });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(BeforeSeupView));
        }

 //create Stock Item 
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new StockItemViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Key: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }

                return View(model);
            }

            var existing = await _context.StockItems.FirstOrDefaultAsync(s =>
                s.VendorName == model.VendorName &&
                s.StockType == model.StockType
                &&
                ((model.StockType == StockTypeEnum.DirectStockForOutlet && s.OutletName == model.OutletName) ||
                 (model.StockType == StockTypeEnum.StockForAll && s.OutletName == null))
            );

            if (existing != null)
            {
                existing.Pos += model.Pos;
                existing.Om += model.Om;
                existing.Server += model.Server;
                existing.Router += model.Router;
                existing.Scanner += model.Scanner;
                existing.Icmo += model.Icmo;

                _context.StockItems.Update(existing);

                _context.StockTransactions.Add(new StockTransaction
                {
                    StockItemId = existing.Id,
                    OperationType = OperationType.Assign,
                    
                    Pos = model.Pos,
                    Om = model.Om,
                    Server = model.Server,
                    Router = model.Router,
                    Scanner = model.Scanner,
                    Icmo = model.Icmo,
                    PerformedBy = ViewBag.CurrentUserName
                });

            }
            else
            {
               
                var stockItem = new StockItem
                {
                    VendorName = model.VendorName,
                    OutletName = model.OutletName,
                    StockType = model.StockType,
                    Pos = model.Pos,
                    Om = model.Om,
                    Server = model.Server,
                    Router = model.Router,
                    Scanner = model.Scanner,
                    Icmo = model.Icmo
                };

                _context.StockItems.Add(stockItem);
                await _context.SaveChangesAsync();
                _context.StockTransactions.Add(new StockTransaction
                {
                    StockItemId = stockItem.Id,
                    OperationType = OperationType.Assign,

                    Pos = model.Pos,
                    Om = model.Om,
                    Server = model.Server,
                    Router = model.Router,
                    Scanner = model.Scanner,
                    Icmo = model.Icmo,
                    PerformedBy = ViewBag.CurrentUserName
                });
                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewStocks));
        }

      
    }

}
