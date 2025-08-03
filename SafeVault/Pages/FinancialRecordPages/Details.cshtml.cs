using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SafeVault.Models;

namespace SafeVault.Pages.FinancialRecordPages;

public class DetailsModel : PageModel
{
    private readonly FinRecDbContext _context;
    public DetailsModel(FinRecDbContext context)
    {
        _context = context;
    }

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
        else
        {
            FinancialRecord = financialrecord;
        }

        return Page();
    }
}
