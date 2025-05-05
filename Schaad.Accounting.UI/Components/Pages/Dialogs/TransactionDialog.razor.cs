using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
using Schaad.Accounting.Interfaces;
using Schaad.Accounting.Models;

namespace Schaad.Accounting.UI.Components.Pages.Dialogs;

public partial class TransactionDialog : ComponentBase
{
    private EditContext editContext = null!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = null!;

    [Parameter]
    public Transaction Content { get; set; } = null!;
    
    [Inject]
    private ITransactionRepository transactionRepository { get; set; } = null!;
    
    [Inject]
    private IViewService viewService { get; set; } = null!;

    private IReadOnlyList<Account> accounts = [];
    private DateTime? SelectedValue;

    protected override void OnInitialized()
    {
        SelectedValue = Content.ValueDate;
        editContext = new EditContext(Content);
        accounts = viewService.GetAccountViewList();
    }

    private async Task SaveAsync()
    {
        if (editContext.Validate())
        {
            Content.ValueDate = SelectedValue!.Value;
            Content.BookingDate = SelectedValue!.Value;
            transactionRepository.SaveTransaction(Content);
            await Dialog.CloseAsync(Content);
        }
    }

    private async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }
}