using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Schaad.Accounting.Interfaces;
using Schaad.Accounting.Models;
using Schaad.Accounting.UI.Components.Pages.Dialogs;

namespace Schaad.Accounting.UI.Components.Pages;

public partial class Classes : ComponentBase
{
    [Inject]
    private ISubclassRepository subclassRepository { get; set; } = null!;
    
    [Inject]
    private IDialogService dialogService { get; set; } = null!;
    
    private IQueryable<SubClass>? subclassQueryable;
    
    protected override Task OnInitializedAsync()  
    {  
        subclassQueryable = subclassRepository.GetSubClassList().AsQueryable();  
        return base.OnInitializedAsync();  
    }
    
    private async Task AddAsync()
    {
        var data = new SubClass();
        var dialog = await dialogService.ShowDialogAsync<ClassDialog>(data, new DialogParameters()
        {
            Height = "500px",
            Title = "Klasse",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (!result.Cancelled && result.Data != null)
        {
            subclassQueryable = subclassRepository.GetSubClassList().AsQueryable();  
        }
    }
    
    private async Task EditAsync(string id)
    {
        var data = subclassRepository.GetSubClass(id);
        var dialog = await dialogService.ShowDialogAsync<ClassDialog>(data, new DialogParameters()
        {
            Height = "500px",
            Title = "Klasse",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (!result.Cancelled && result.Data != null)
        {
            subclassQueryable = subclassRepository.GetSubClassList().AsQueryable(); 
        }
    }
    
    private async Task DeleteAsync(string id)
    {
        var subclass = subclassRepository.GetSubClass(id);
        var dialog = await dialogService.ShowConfirmationAsync($"Klasse '{subclass.Name}' wirklich löschen?", "Ja", "Nein", "Klasse löschen");
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            subclassRepository.DeleteSubClass(id);
            subclassQueryable = subclassRepository.GetSubClassList().AsQueryable(); 
        }
    }
}