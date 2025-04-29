using Microsoft.AspNetCore.Components;
using Schaad.Accounting.Interfaces;
using Schaad.Accounting.Models;

namespace Schaad.Accounting.UI.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject]
    private IViewService viewService { get; set; } = null!;

    private List<BankTransaction> bankTransactions = null!;
    
    protected override Task OnInitializedAsync()  
    {  
        bankTransactions = viewService.GetOpenBankTransactionList();  
        return base.OnInitializedAsync();  
    }  
}
