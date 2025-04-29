using Microsoft.AspNetCore.Components;
using Schaad.Accounting.Datasets.Reports;
using Schaad.Accounting.Interfaces;

namespace Schaad.Accounting.UI.Components.Pages.Reports;

public partial class BalanceReport : ComponentBase
{
    [Inject]
    private IViewService viewService { get; set; } = null!;
    
    [Inject]
    private ISettingsService settingsService { get; set; } = null!;
    
    private BalanceDataset balance = null!;
    private decimal win;
    private string header = null!;
    private string footer = null!;
    
    protected override Task OnInitializedAsync()
    {
        balance = viewService.GetBalanceView();
        win = balance.TotalActivaCHF - balance.TotalPassivaCHF;
        (header, footer) = Report.GetViewDataTitleAndFooter("Bilanz", settingsService);
        
        return base.OnInitializedAsync();  
    }
}