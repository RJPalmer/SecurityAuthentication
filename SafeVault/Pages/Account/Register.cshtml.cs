using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace SafeVault.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly AppDbContext _context;

        public RegisterModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [BindProperty]
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string UserEmail { get; set; }

        [BindProperty]
        [Required]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        public string SuccessMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check if email already exists
            if (_context.Users.Any(u => u.UserEmail == UserEmail))
            {
                ModelState.AddModelError("UserEmail", "Email is already registered.");
                return Page();
            }

            // Hash the password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(UserPassword);

            var user = new User
            {
                UserName = UserName,
                UserEmail = UserEmail,
                UserPassword = hashedPassword
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            SuccessMessage = "Registration successful! You can now log in.";
            ModelState.Clear();
            return Page();
        }
    }
}
