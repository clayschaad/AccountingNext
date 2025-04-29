using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Schaad.Accounting.Interfaces;
using Schaad.Accounting.Models;
using Schaad.Accounting.UI.Components.Pages.Dialogs;

namespace Schaad.Accounting.UI.Components.Pages;

public partial class BookingTexts : ComponentBase
{
    [Inject]
    private IBookingTextRepository bookingTextRepository { get; set; } = null!;
    
    [Inject]
    private IDialogService dialogService { get; set; } = null!;
    
    private IQueryable<BookingText>? bookingTextQueryable;
    
    protected override Task OnInitializedAsync()  
    {  
        bookingTextQueryable = bookingTextRepository.GetBookingTextList().AsQueryable();  
        return base.OnInitializedAsync();  
    }
    
    private async Task AddAsync()
    {
        var data = new BookingText();
        var dialog = await dialogService.ShowDialogAsync<BookingTextDialog>(data, new DialogParameters()
        {
            Height = "500px",
            Title = "Add Buchungstext",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (!result.Cancelled && result.Data != null)
        {
            bookingTextQueryable = bookingTextRepository.GetBookingTextList().AsQueryable();
        }
    }
    
    private async Task EditAsync(string id)
    {
        var data = bookingTextRepository.GetBookingText(id);
        var dialog = await dialogService.ShowDialogAsync<BookingTextDialog>(data, new DialogParameters()
        {
            Height = "500px",
            Title = "Edit Buchungstext",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (!result.Cancelled && result.Data != null)
        {
            bookingTextQueryable = bookingTextRepository.GetBookingTextList().AsQueryable();
        }
    }

    private async Task DeleteAsync(string id)
    {
        var bookingText = bookingTextRepository.GetBookingText(id);
        var dialog = await dialogService.ShowConfirmationAsync($"Buchungstext '{bookingText.Text}' wirklich löschen?", "Ja", "Nein", "Buchungstext löschen");
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            bookingTextRepository.DeleteBookingText(id);
            bookingTextQueryable = bookingTextRepository.GetBookingTextList().AsQueryable();
        }
    }
}