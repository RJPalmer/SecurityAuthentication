using Microsoft.AspNetCore.Mvc;
using SafeVault.Pages;

namespace SafeVault.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }

        // GET: /User/Login
        [HttpGet]
        public IActionResult Login()
        {
            return RedirectToPage("/Pages/Login.cshtml");
        }

        // POST: /User/Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserEmail == email);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.UserPassword))
            {
                // Authentication successful
                // Redirect to the Index Razor Page
                return Redirect("/Index");
            }
            // Authentication failed
            ModelState.AddModelError("", "Invalid email or password.");
            return View();
        }
    }
}
