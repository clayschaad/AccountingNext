using Microsoft.AspNetCore.Components;
using Schaad.Accounting.Models;

namespace Schaad.Accounting.UI.Components.Pages.Controls;

public partial class AccountSelector : ComponentBase
{
    [Parameter] public required string? AccountId { get; set; }
    
    [Parameter] public required IReadOnlyList<Account> Accounts { get; set; }

    [Parameter] public EventCallback<string> AccountIdChanged { get; set; }

    private async Task OnInputChanged(ChangeEventArgs e)
    {
        AccountId = e.Value?.ToString();
        await AccountIdChanged.InvokeAsync(AccountId);
    }
}