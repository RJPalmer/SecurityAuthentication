using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SafeVault.Models;

namespace SafeVault.Pages.FinancialRecordPages;

public class CreateModel : PageModel
{
    private readonly FinRecDbContext _context;

    public CreateModel(FinRecDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public FinancialRecord FinancialRecord { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.FinancialRecord.Add(FinancialRecord);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
