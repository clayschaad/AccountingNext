using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
using Schaad.Accounting.Interfaces;
using Schaad.Accounting.Models;

namespace Schaad.Accounting.UI.Components.Pages.Dialogs;

public partial class BookingTextDialog : ComponentBase
{
    private EditContext editContext = null!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = null!;

    [Parameter]
    public BookingText Content { get; set; } = null!;
    
    [Inject]
    private IBookingTextRepository bookingTextRepository { get; set; } = null!;
    
    protected override void OnInitialized()
    {
        editContext = new EditContext(Content);
    }

    private async Task SaveAsync()
    {
        if (editContext.Validate())
        {
            bookingTextRepository.SaveBookingText(Content);
            await Dialog.CloseAsync(Content);
        }
    }

    private async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }
}