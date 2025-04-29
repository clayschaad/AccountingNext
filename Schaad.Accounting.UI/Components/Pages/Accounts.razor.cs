using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Schaad.Accounting.Interfaces;
using Schaad.Accounting.Models;
using Schaad.Accounting.UI.Components.Pages.Dialogs;

namespace Schaad.Accounting.UI.Components.Pages;

public partial class Accounts : ComponentBase
{
     [Inject]
    private IAccountRepository accountRepository { get; set; } = null!;
    
    [Inject]
    private IDialogService dialogService { get; set; } = null!;
    
    private IQueryable<Account>? accountQueryable;
    
    protected override Task OnInitializedAsync()  
    {  
        accountQueryable = accountRepository.GetAccountList().AsQueryable();  
        return base.OnInitializedAsync();  
    }
    
    private async Task AddAsync()
    {
        var data = new Account();
        var dialog = await dialogService.ShowDialogAsync<AccountDialog>(data, new DialogParameters()
        {
            Height = "500px",
            Title = "Konto",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (!result.Cancelled && result.Data != null)
        {
            accountQueryable = accountRepository.GetAccountList().AsQueryable();  
        }
    }
    
    private async Task EditAsync(string id)
    {
        var data = accountRepository.GetAccount(id);
        var dialog = await dialogService.ShowDialogAsync<AccountDialog>(data, new DialogParameters()
        {
            Height = "500px",
            Title = "Konto",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (!result.Cancelled && result.Data != null)
        {
            accountQueryable = accountRepository.GetAccountList().AsQueryable();  
        }
    }
    
    private async Task DeleteAsync(string id)
    {
        var account = accountRepository.GetAccount(id);
        var dialog = await dialogService.ShowConfirmationAsync($"Konto '{account.Name}' wirklich löschen?", "Ja", "Nein", "Konto löschen");
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            accountRepository.DeleteAccount(id);
            accountQueryable = accountRepository.GetAccountList().AsQueryable();  
        }
    }
}