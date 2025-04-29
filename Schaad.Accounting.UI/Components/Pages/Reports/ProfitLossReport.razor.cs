using Microsoft.AspNetCore.Components;
using Schaad.Accounting.Datasets;
using Schaad.Accounting.Interfaces;

namespace Schaad.Accounting.UI.Components.Pages.Reports;

public partial class ProfitLossReport : ComponentBase
{
    [Inject]
    private IViewService viewService { get; set; } = null!;
    
    [Inject]
    private ISettingsService settingsService { get; set; } = null!;
    
    private IReadOnlyList<AccountDataset> accounts = null!;
    private decimal profit;
    private decimal loss;
    private decimal win;
    private string header = null!;
    private string footer = null!;
    
    protected override Task OnInitializedAsync()
    {
        accounts = viewService.GetAccountViewList();
        profit = Math.Abs(accounts.Where(m => m.Class == 3).Sum(m => m.Balance));
        loss = Math.Abs(accounts.Where(m => m.Class == 4).Sum(m => m.Balance));
        win = profit-loss;

        (header, footer) = Report.GetViewDataTitleAndFooter("Erfolgsrechnung", settingsService);
        
        return base.OnInitializedAsync();  
    }
}