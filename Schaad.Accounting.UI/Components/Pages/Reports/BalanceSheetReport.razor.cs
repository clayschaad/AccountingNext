using Microsoft.AspNetCore.Components;
using Schaad.Accounting.Datasets.Reports;
using Schaad.Accounting.Interfaces;

namespace Schaad.Accounting.UI.Components.Pages.Reports;

public partial class BalanceSheetReport : ComponentBase
{
    [Inject]
    private IViewService viewService { get; set; } = null!;
    
    [Inject]
    private ISettingsService settingsService { get; set; } = null!;
    
    private BalanceSheetDataset balanceSheet = null!;
    private decimal win;
    private string header = null!;
    private string footer = null!;
    
    protected override Task OnInitializedAsync()
    {
        balanceSheet = viewService.GetBalanceSheetView(settingsService.GetYear());
        win = balanceSheet.ProfitCHF - balanceSheet.LossCHF;
        (header, footer) = Report.GetViewDataTitleAndFooter("Jahresabschluss", settingsService);
        
        return base.OnInitializedAsync();  
    }
}