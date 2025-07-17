using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using OutletStatusPortal.Models;


namespace OutletStatusPortal.Controllers
{
    public class AccountController : BaseController
    {
        

       public AccountController(Outletdbcontext context)
        : base(context)
    {
    }
        [Authorize(Roles = "Admin")]
        public IActionResult Register() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Register(string stafid, string password, string role)
        {
            if (_context.Users.Any(u => u.StafId == stafid))
            {
                ModelState.AddModelError("", "User already exists");
                return View();
            }

            var user = new Users { StafId = stafid, PassWord = password, Role = role };
            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }
        [AllowAnonymous]
        public IActionResult Login() => View();

      
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string stafid, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.StafId == stafid && u.PassWord == password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View();
            }
            else if (user.Role == "User")
            {

                var claims1 = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.StafId),
            new Claim(ClaimTypes.Role, user.Role)
        };

                var identity1 = new ClaimsIdentity(claims1, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal1 = new ClaimsPrincipal(identity1);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal1);
                return RedirectToAction("Register", "Account");
            }
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.StafId),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            TempData["success"] = "Log in  successfully!";
            //TempData["error"] = "Failed to update profile!";
            //TempData["info"] = "This is some info.";
            //TempData["warning"] = "This is a warning!";

            return RedirectToAction("ViewStocks", "Stock");

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserList()
        {

            return View(_context.Users.ToList());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var countadmin = _context.Users.Where(x => x.Role == "Admin").ToList().Count();
            ViewData["adminount"] = countadmin;
            return View();
        }
    }

}

