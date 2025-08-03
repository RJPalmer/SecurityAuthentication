using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SafeVault.Models;

namespace SafeVault.Pages.FinancialRecordPages;

public class IndexModel : PageModel
{
    private readonly FinRecDbContext _context;

    public IndexModel(FinRecDbContext context)
    {
        _context = context;
    }

    public IList<FinancialRecord> FinancialRecord { get; set; } = default!;

    public async Task OnGetAsync()
    {
        FinancialRecord = await _context.FinancialRecord.ToListAsync();
    }
}
