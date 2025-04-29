using Microsoft.AspNetCore.Components;
using Schaad.Accounting.Datasets;
using Schaad.Accounting.Interfaces;

namespace Schaad.Accounting.UI.Components.Pages;

public partial class Transactions : ComponentBase
{
    [Inject]
    private IViewService viewService { get; set; } = null!;

    private IReadOnlyList<AccountDataset> accounts = null!;
    
    private IQueryable<TransactionDataset>? transactionList;
    private string? selectedAccountId;
    
    protected override Task OnInitializedAsync()  
    {  
        accounts = viewService.GetAccountViewList();  
        return base.OnInitializedAsync();  
    }

    private void ShowTransactions(string accountId)
    {
        selectedAccountId = accountId;
        transactionList = viewService.GetTransactionViewList(accountId).AsQueryable();
    }
    
    private string GetAccountName(TransactionDataset transaction)
    {
        if (selectedAccountId == transaction.OriginAccountId)
        {
            return transaction.TargetAccount.Name;
        }

        return transaction.OriginAccount.Name;
    }
}