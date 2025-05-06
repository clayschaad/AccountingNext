using Microsoft.AspNetCore.Components;
using Schaad.Accounting.Interfaces;

namespace Schaad.Accounting.UI.Components;

public partial class MyHeader : ComponentBase
{
    [Inject]
    private ISettingsService settingsService { get; set; } = null!;
    
    [Inject]
    NavigationManager navigationManager { get; set; } = null!;
    
    private List<int> years = new ();
    private int selectedYear;
    
    private List<string> mandators = new ();
    private string selectedMandator = null!;
    
    protected override Task OnInitializedAsync()  
    {  
        for (var year = DateTime.Now.Year; year >= 2015; year--)
        {
            years.Add(year);
        }
        
        selectedYear = settingsService.GetYear();

        mandators = settingsService.GetMandators(selectedYear).ToList();
        selectedMandator = settingsService.GetMandator();
        
        return base.OnInitializedAsync();  
    }
    
    public void YearChanged(string syear)
    {
        var year = int.Parse(syear);
        if (selectedYear != year)
        {
            settingsService.SetYear(year);
            mandators = settingsService.GetMandators(year).ToList();
            navigationManager.NavigateTo(navigationManager.Uri, forceLoad: true);
        }
    }
    
    public void MandatorChanged(string mandator)
    {
        if (selectedMandator != mandator)
        {
            settingsService.SetMandator(mandator);
            navigationManager.NavigateTo(navigationManager.Uri, forceLoad: true);
        }
    }
}