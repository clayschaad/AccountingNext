using Microsoft.AspNetCore.Components;
using Schaad.Accounting.Interfaces;

namespace Schaad.Accounting.UI.Components;

public partial class MyHeader : ComponentBase
{
    [Inject]
    private ISettingsService settingsService { get; set; } = null!;
    
    private List<int> years = new ();
    private int selectedYear;
    
    protected override Task OnInitializedAsync()  
    {  
        for (var year = DateTime.Now.Year; year >= 2015; year--)
        {
            years.Add(year);
        }

        selectedYear = settingsService.GetYear();
        
        return base.OnInitializedAsync();  
    }
    
    public void YearChanged(string obj)
    {
        settingsService.SetYear(int.Parse(obj));
    }

}