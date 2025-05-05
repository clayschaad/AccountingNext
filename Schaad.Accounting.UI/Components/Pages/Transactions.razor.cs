using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Schaad.Accounting.Datasets;
using Schaad.Accounting.Interfaces;
using Schaad.Accounting.Models;
using Schaad.Accounting.UI.Components.Pages.Dialogs;

namespace Schaad.Accounting.UI.Components.Pages;

public partial class Transactions : ComponentBase
{
    [Inject]
    private IViewService viewService { get; set; } = null!;
    
    [Inject]
    private ITransactionRepository transactionRepository { get; set; } = null!;
    
    [Inject]
    private IDialogService dialogService { get; set; } = null!;

    private IReadOnlyList<AccountDataset> accounts = null!;
    
    private IQueryable<TransactionDataset>? transactionList;
    private string selectedAccountId = null!;
    private AccountDataset selectedAccount = null!;
    
    protected override Task OnInitializedAsync()  
    {  
        accounts = viewService.GetAccountViewList();
        ShowTransactions(accounts.First().Id);
        return base.OnInitializedAsync();
    }

    private void ShowTransactions(string accountId)
    {
        selectedAccountId = accountId;
        selectedAccount = accounts.Single(a => a.Id == accountId);
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
    
    private async Task AddAsync()
    {
        var data = new Transaction
        {
            ValueDate = DateTime.Now,
            BookingDate = DateTime.Now,
            TargetAccountId = selectedAccountId
        };
        var dialog = await dialogService.ShowDialogAsync<TransactionDialog>(data, new DialogParameters()
        {
            Height = "500px",
            Title = "Transaktion",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (!result.Cancelled && result.Data != null)
        {
            transactionList = viewService.GetTransactionViewList(selectedAccountId).AsQueryable();
        }
    }
    
    private async Task EditAsync(string id)
    {
        var data = transactionRepository.GetTransaction(id);
        var dialog = await dialogService.ShowDialogAsync<TransactionDialog>(data, new DialogParameters()
        {
            Height = "500px",
            Title = "Transaktion",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (!result.Cancelled && result.Data != null)
        {
            transactionList = viewService.GetTransactionViewList(selectedAccountId).AsQueryable();
        }
    }
    
    private async Task DeleteAsync(string id)
    {
        var transaction = transactionRepository.GetTransaction(id);
        var dialog = await dialogService.ShowConfirmationAsync($"Transaction '{transaction.Text}' mit Betrag {transaction.Value} wirklich löschen?", "Ja", "Nein", "Transaktion löschen");
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            transactionRepository.DeleteTransaction(id);
            transactionList = viewService.GetTransactionViewList(selectedAccountId).AsQueryable();
        }
    }
}