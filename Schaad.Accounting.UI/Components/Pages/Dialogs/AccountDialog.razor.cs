using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
using Schaad.Accounting.Interfaces;
using Schaad.Accounting.Models;

namespace Schaad.Accounting.UI.Components.Pages.Dialogs;

public partial class AccountDialog : ComponentBase
{
    private EditContext editContext = null!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = null!;

    [Parameter]
    public Account Content { get; set; } = null!;
    
    [Inject]
    private IAccountRepository accountRepository { get; set; } = null!;
    
    protected override void OnInitialized()
    {
        editContext = new EditContext(Content);
    }

    private async Task SaveAsync()
    {
        if (editContext.Validate())
        {
            accountRepository.SaveAccount(Content);
            await Dialog.CloseAsync(Content);
        }
    }

    private async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }
}