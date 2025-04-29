using Microsoft.AspNetCore.Components;
using Schaad.Accounting.Interfaces;
using Schaad.Accounting.Models;

namespace Schaad.Accounting.UI.Components.Pages;

public partial class BankTransactions : ComponentBase
{
    [Inject]
    private IBankTransactionRepository bankTransactionRepository { get; set; } = null!;
    private IQueryable<BankTransaction>? TransactionList;
    
    protected override Task OnInitializedAsync()  
    {  
        TransactionList = bankTransactionRepository.GetBankTransactionList().AsQueryable();  
        return base.OnInitializedAsync();  
    }
}