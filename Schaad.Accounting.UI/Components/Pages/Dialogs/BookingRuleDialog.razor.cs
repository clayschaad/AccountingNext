using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
using Schaad.Accounting.Interfaces;
using Schaad.Accounting.Models;

namespace Schaad.Accounting.UI.Components.Pages.Dialogs;

public partial class BookingRuleDialog : ComponentBase
{
    private EditContext editContext = null!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = null!;

    [Parameter]
    public BookingRule Content { get; set; } = null!;
    
    [Inject]
    private IBookingRuleRepository bookingRuleRepository { get; set; } = null!;
    
    [Inject]
    private IViewService viewService { get; set; } = null!;

    private IReadOnlyList<Account> accounts = [];

    protected override void OnInitialized()
    {
        editContext = new EditContext(Content);
        accounts = viewService.GetAccountViewList();
    }

    private async Task SaveAsync()
    {
        if (editContext.Validate())
        {
            bookingRuleRepository.SaveBookingRule(Content);
            await Dialog.CloseAsync(Content);
        }
    }

    private async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }
}