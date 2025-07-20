using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
       .Include(b => b.StockItem) // include the StockItem data
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
            TempData["success"] = "Insert successfully!";
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
            TempData["success"] = "Insert successfully!";
            return RedirectToAction(nameof(ViewStocks));
        }

        //-------After outlet Setup----------------//
        public IActionResult AfteroutletSetupView()
        {
            var list = _context.AfterOutletSetups
                .Include(a => a.beforeOutletSetUp)
                .ToList();

            return View(list);
        }


        /// create AfteroutletSetup Action 
        /// 
        public IActionResult AfterOutletSetupCreate()
        {
            var outlets = _context.BeforeOutletSetUps
            .Select(o => new SelectListItem
        {
            Value = o.Sl.ToString(),
            Text = $"{o.OutletCode} - {o.OutletName}"
        }).ToList();

            ViewBag.Outlets = outlets;
            //dropdown for AssignPerson
              var userlist = _context.Users
             .Where(u => u.Role == "User")
             .Select(u => new SelectListItem
             {
                 Value = u.Name,
                 Text = u.Name
             }).ToList();

                    ViewBag.UserLists = userlist;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AfterOutletSetupCreate(AfterOutletSetup model)
        {
            if (ModelState.IsValid)
            {
                _context.AfterOutletSetups.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("AfteroutletSetupView");

                //foreach (var modelState in ModelState)
                //{
                //    var key = modelState.Key;
                //    var errors = modelState.Value.Errors;

                //    foreach (var error in errors)
                //    {
                //        Console.WriteLine($"Error in '{key}': {error.ErrorMessage}");
                //    }
                //}
            }


            // Rebuild dropdown if model is invalid
            //dropdown for OutletName
            ViewBag.Outlets = _context.BeforeOutletSetUps
                .Select(o => new SelectListItem
                {
                    Value = o.Sl.ToString(),
                    Text = $"{o.OutletCode} - {o.OutletName}"
                }).ToList();
            //dropdown for AssignPerson
            var userlist = _context.Users.Select(U => new SelectListItem
            {
                Value = U.Name.ToString(),
                Text = $"{U.Name}"
            }).ToList(); ;
            ViewBag.userlists = userlist;

            return View(model);
        }

        public IActionResult DeviceSetupStatusCreate()
        {
            var viewModel = new DeviceSetupStatusFormViewModel
            {
                BeforeOutletSetUpList = _context.BeforeOutletSetUps
                    .Select(b => new SelectListItem
                    {
                        Value = b.Sl.ToString(),
                        Text = b.OutletName // Replace with appropriate property
                    }).ToList()
            };

            // Add an initial row
            viewModel.DeviceStatuses.Add(new DeviceSetupStatus());

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult DeviceSetupStatusCreate(DeviceSetupStatusFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var device in model.DeviceStatuses)
                {
                    device.BeforeOutletSetUpSl = model.SelectedBeforeOutletSetUpSl;
                    device.UpdateDate = DateTime.Now;
                    _context.DeviceSetupStatuses.Add(device);
                }

                _context.SaveChanges();
                return RedirectToAction("Index"); // or wherever
            }

            // Refill dropdown on postback
            model.BeforeOutletSetUpList = _context.BeforeOutletSetUps
                    .Select(b => new SelectListItem
                    {
                        Value = b.Sl.ToString(),
                        Text = b.OutletName
                    }).ToList();

            return View(model);
        }

    }

}
