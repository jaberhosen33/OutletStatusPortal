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
        [Authorize(Roles = "Admin,User")]

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
        [Authorize(Roles = "Admin")]
        public IActionResult AfteroutletSetupView()
        {
            var list = _context.AfterOutletSetups
                .Include(a => a.beforeOutletSetUp)
                .ToList();

            return View(list);
        }


        /// create AfteroutletSetup Action 
        /// 
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        public IActionResult AfterOutletSetupEdit(int id)
        {
            var afterOutletSetup = _context.AfterOutletSetups.Find(id);
            if (afterOutletSetup == null)
            {
                return NotFound();
            }

            // Populate dropdowns again
            ViewBag.Outlets = _context.BeforeOutletSetUps
                .Select(o => new SelectListItem
                {
                    Value = o.Sl.ToString(),
                    Text = $"{o.OutletCode} - {o.OutletName}"
                }).ToList();

            ViewBag.UserLists = _context.Users
                .Where(u => u.Role == "User")
                .Select(u => new SelectListItem
                {
                    Value = u.Name,
                    Text = u.Name
                }).ToList();

            return View(afterOutletSetup);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AfterOutletSetupEdit(int id, AfterOutletSetup model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("AfteroutletSetupView");
            }

            // Repopulate dropdowns if validation fails
            ViewBag.Outlets = _context.BeforeOutletSetUps
                .Select(o => new SelectListItem
                {
                    Value = o.Sl.ToString(),
                    Text = $"{o.OutletCode} - {o.OutletName}"
                }).ToList();

            ViewBag.UserLists = _context.Users
                .Where(u => u.Role == "User")
                .Select(u => new SelectListItem
                {
                    Value = u.Name,
                    Text = u.Name
                }).ToList();

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            var afterOutletSetup = await _context.AfterOutletSetups.FindAsync(id);
            if (afterOutletSetup == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            _context.AfterOutletSetups.Remove(afterOutletSetup);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Deleted successfully." });
        }


        private List<SelectListItem> GetBeforeOutletSetUpList()
        {
            return _context.BeforeOutletSetUps
                .Select(b => new SelectListItem
                {
                    Value = b.Sl.ToString(),
                    Text = b.OutletName
                }).ToList();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DeviceSetupStatusCreate()
        {
            var viewModel = new DeviceSetupStatusFormViewModel
            {
                BeforeOutletSetUpList = GetBeforeOutletSetUpList()
            };
            viewModel.DeviceStatuses.Add(new DeviceSetupStatus());
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult DeviceSetupStatusCreate(DeviceSetupStatusFormViewModel model)
        {

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState)
                {
                    var key = modelState.Key;
                    var errors = modelState.Value.Errors;

                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Error in '{key}': {error.ErrorMessage}");
                    }
                }
            }
          else
            {
                foreach (var device in model.DeviceStatuses)
                {
                    device.BeforeOutletSetUpSl = model.SelectedBeforeOutletSetUpSl;
                    device.WorkBy = ViewBag.CurrentUserName;
                    device.UpdateDate = DateTime.Now;
                    _context.DeviceSetupStatuses.Add(device);
                }

                _context.SaveChanges();
                return RedirectToAction("DeviceSetupStatusView");
            }

            // Refill dropdown
            model.BeforeOutletSetUpList = GetBeforeOutletSetUpList();
            return View(model);
        }


        public IActionResult DeviceSetupStatusView()
        {
            var groupedData = _context.DeviceSetupStatuses
         .Include(d => d.beforeOutletSetUp)
         .ToList()
         .GroupBy(d => d.BeforeOutletSetUpSl)
         .Select(g => new DeviceSetupStatusGroupViewModel
         {
             Outlet = g.First().beforeOutletSetUp,
             Devices = g.ToList()
         })
         .ToList();

            return View(groupedData);
        }

        [HttpPost]
        public IActionResult DeleteDevice(int id)
        {
            var device = _context.DeviceSetupStatuses.Find(id);
            if (device == null)
            {
                return Json(new { success = false, message = "Device not found." });
            }

            _context.DeviceSetupStatuses.Remove(device);
            _context.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult DeleteAllDevices(int outletSl)
        {
            var devices = _context.DeviceSetupStatuses.Where(d => d.BeforeOutletSetUpSl == outletSl).ToList();

            if (!devices.Any())
            {
                return Json(new { success = false, message = "No devices found for this outlet." });
            }

            _context.DeviceSetupStatuses.RemoveRange(devices);
            _context.SaveChanges();

            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult EditSingleDevice(int id)
        {
            var device = _context.DeviceSetupStatuses
                .Include(d => d.beforeOutletSetUp)
                .FirstOrDefault(d => d.Id == id);

            if (device == null)
                return NotFound();

            return View(device);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSingleDevice(DeviceSetupStatus model)
        {
            if (ModelState.IsValid)
            {
                var existing = _context.DeviceSetupStatuses.FirstOrDefault(d => d.Id == model.Id);
                if (existing == null)
                    return NotFound();

                existing.WorkStatus = model.WorkStatus;
                existing.WorkBy = model.WorkBy;
                existing.UpdateDate = DateTime.Now;

                _context.SaveChanges();
                TempData["success"] = "Device updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult EditAllDevices(int outletSl)
        {
            var devices = _context.DeviceSetupStatuses
                .Include(d => d.beforeOutletSetUp)
                .Where(d => d.BeforeOutletSetUpSl == outletSl)
                .ToList();

            if (!devices.Any())
                return NotFound();

            return View(devices);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAllDevices(List<DeviceSetupStatus> devices)
        {
            foreach (var updatedDevice in devices)
            {
                var existingDevice = _context.DeviceSetupStatuses.FirstOrDefault(d => d.Id == updatedDevice.Id);
                if (existingDevice != null)
                {
                    existingDevice.WorkStatus = updatedDevice.WorkStatus;
                    existingDevice.WorkBy = updatedDevice.WorkBy;
                    existingDevice.UpdateDate = DateTime.Now;
                }
            }

            _context.SaveChanges();
            TempData["success"] = "All Devices updated successfully!";
            return RedirectToAction(nameof(Index));
        }

    }

}
