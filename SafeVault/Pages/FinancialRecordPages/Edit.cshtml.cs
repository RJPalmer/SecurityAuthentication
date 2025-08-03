using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SafeVault.Models;

namespace SafeVault.Pages.FinancialRecordPages;

public class EditModel : PageModel
{
    private readonly FinRecDbContext _context;

    public EditModel(FinRecDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public FinancialRecord FinancialRecord { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? financialrecordid)
    {
        if (financialrecordid is null)
        {
            return NotFound();
        }

        var financialrecord = await _context.FinancialRecord.FirstOrDefaultAsync(m => m.FinancialRecordID == financialrecordid);
        if (financialrecord is null)
        {
            return NotFound();
        }
        FinancialRecord = financialrecord;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(FinancialRecord).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FinancialRecordExists(FinancialRecord.FinancialRecordID))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool FinancialRecordExists(int financialrecordid)
    {
        return _context.FinancialRecord.Any(e => e.FinancialRecordID == financialrecordid);
    }
}
