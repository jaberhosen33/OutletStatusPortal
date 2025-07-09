using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutletStatusPortal.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace OutletStatusPortal.Controllers
{
    public class NewOutletInfoController : BaseController
    {
        public NewOutletInfoController(Outletdbcontext context)
 : base(context)
        {
        }

        // GET: NewOutletInfo
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.NewOutletInfos.ToListAsync());
        }

        // GET: NewOutletInfo/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        // POST: NewOutletInfo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewOutletInfo newOutletInfo)
        {
            if (ModelState.IsValid)
            {
               
                _context.Add(newOutletInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            LoadDropdowns();
            return View(newOutletInfo);
        }

        // GET: NewOutletInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var outlet = await _context.NewOutletInfos.FindAsync(id);
            if (outlet == null) return NotFound();

            LoadDropdowns();
            return View(outlet);
        }

        // POST: NewOutletInfo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NewOutletInfo newOutletInfo)
        {
            if (id != newOutletInfo.Sl) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newOutletInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewOutletInfoExists(newOutletInfo.Sl))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            LoadDropdowns();
            return View(newOutletInfo);
        }

        // GET: NewOutletInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var outlet = await _context.NewOutletInfos.FirstOrDefaultAsync(m => m.Sl == id);
            if (outlet == null) return NotFound();

            return View(outlet);
        }

        // GET: NewOutletInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var outlet = await _context.NewOutletInfos
                .FirstOrDefaultAsync(m => m.Sl == id);
            if (outlet == null) return NotFound();

            return View(outlet);
        }

        // POST: NewOutletInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var outlet = await _context.NewOutletInfos.FindAsync(id);
            if (outlet != null)
            {
                _context.NewOutletInfos.Remove(outlet);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns()
        {
            ViewBag.OfficeITSetup = new List<string> { "Yes", "No", "Pending" };
            ViewBag.CouriarStatus = new List<string> { "Delivered", "In Transit", "Not Sent" };
            ViewBag.OutletITSetup = new List<string> { "Completed", "Not Started", "In Progress" };
            ViewBag.LinkStatus = new List<string> { "Active", "Inactive" };
            ViewBag.SAPID = new List<string> { "`No", "Yes" };
            ViewBag.MailID = new List<string> { "`No", "Yes" };
            ViewBag.POSID = new List<string> { "`No", "Yes" };
            ViewBag.EPSLive = new List<string> { "`No", "Yes" };
        }

        private bool NewOutletInfoExists(int id)
        {
            return _context.NewOutletInfos.Any(e => e.Sl == id);
        }
    }
}

