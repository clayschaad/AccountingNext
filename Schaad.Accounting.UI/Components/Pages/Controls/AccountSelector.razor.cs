using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Schaad.Accounting.Models;

namespace Schaad.Accounting.UI.Components.Pages.Controls; 

public partial class AccountSelector : ComponentBase
{
    [Parameter] public required string? AccountId { get; set; }
    
    [Parameter] public required IReadOnlyList<Account> Accounts { get; set; }

    [Parameter] public EventCallback<string> AccountIdChanged { get; set; }
    
    private Account? SelectedAccount { get; set; }

    protected override void OnInitialized()
    {
        SelectedAccount = Accounts.FirstOrDefault(account => account.Id == AccountId);
    }
    
    private async Task OnSearchAsync(OptionsSearchEventArgs<Account> e)
    {
        e.Items = Accounts.Where(i => i.Name.Contains(e.Text, StringComparison.OrdinalIgnoreCase))
            .OrderBy(i => i.Name);
        await Task.CompletedTask;
    }
    
    public async Task OnSelectedChanged(IEnumerable<Account>? s)
    {
        SelectedAccount = s?.FirstOrDefault();
        AccountId = SelectedAccount?.Id;
        await AccountIdChanged.InvokeAsync(AccountId);
    }
}