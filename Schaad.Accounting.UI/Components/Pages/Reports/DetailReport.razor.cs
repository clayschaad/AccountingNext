using Microsoft.AspNetCore.Components;
using Schaad.Accounting.Datasets;
using Schaad.Accounting.Interfaces;

namespace Schaad.Accounting.UI.Components.Pages.Reports;

public partial class DetailReport : ComponentBase
{
    [Inject]
    private IViewService viewService { get; set; } = null!;
    
    [Inject]
    private ISettingsService settingsService { get; set; } = null!;
    
    private IReadOnlyList<AccountDataset> accounts = null!;
    private IReadOnlyList<TransactionDataset> transactions = null!;
    private decimal profit;
    private decimal loss;
    private string header = null!;
    private string footer = null!;
    
    protected override Task OnInitializedAsync()
    {
        accounts = viewService.GetAccountViewList();
        transactions = viewService.GetTransactionViewList();
        
        profit = Math.Abs(accounts.Where(m => m.Class == 3).Sum(m => m.Balance));
        loss = Math.Abs(accounts.Where(m => m.Class == 4).Sum(m => m.Balance));
        
        (header, footer) = Report.GetViewDataTitleAndFooter("Detailaufstellung", settingsService);
        
        return base.OnInitializedAsync();  
    }
}