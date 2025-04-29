using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Schaad.Accounting.Datasets;
using Schaad.Accounting.Interfaces;
using Schaad.Accounting.Models;
using Schaad.Accounting.UI.Components.Pages.Dialogs;

namespace Schaad.Accounting.UI.Components.Pages;

public partial class BookingRules : ComponentBase
{
    [Inject]
    private IViewService viewService { get; set; } = null!;
    
    [Inject]
    private IBookingRuleRepository bookingRuleRepository { get; set; } = null!;
    
    [Inject]
    private IDialogService dialogService { get; set; } = null!;
    
    private IQueryable<BookingRuleDataset>? bookingRulesQueryable;
    
    protected override Task OnInitializedAsync()  
    {  
        bookingRulesQueryable = viewService.GetBookingRuleViewList().AsQueryable();  
        return base.OnInitializedAsync();  
    }
    
    private async Task AddAsync()
    {
        var data = new BookingRule();
        var dialog = await dialogService.ShowDialogAsync<BookingRuleDialog>(data, new DialogParameters()
        {
            Height = "500px",
            Title = "Buchungsregel",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (!result.Cancelled && result.Data != null)
        {
            bookingRulesQueryable = viewService.GetBookingRuleViewList().AsQueryable();
        }
    }
    
    private async Task EditAsync(string id)
    {
        var data = bookingRuleRepository.GetBookingRule(id);
        var dialog = await dialogService.ShowDialogAsync<BookingRuleDialog>(data, new DialogParameters()
        {
            Height = "500px",
            Title = "Buchungsregel",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (!result.Cancelled && result.Data != null)
        {
            bookingRulesQueryable = viewService.GetBookingRuleViewList().AsQueryable();
        }
    }
    
    private async Task DeleteAsync(string id)
    {
        var bookingRule = bookingRuleRepository.GetBookingRule(id);
        var dialog = await dialogService.ShowConfirmationAsync($"Regel '{bookingRule.LookupText}' wirklich löschen?", "Ja", "Nein", "Buchungsregel löschen");
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            bookingRuleRepository.DeleteBookingRule(id);
            bookingRulesQueryable = viewService.GetBookingRuleViewList().AsQueryable();
        }
    }
}