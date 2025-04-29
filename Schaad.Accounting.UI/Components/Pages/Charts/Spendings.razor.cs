using Microsoft.AspNetCore.Components;
using Plotly.Blazor;
using Plotly.Blazor.Traces;
using Schaad.Accounting.Interfaces;

namespace Schaad.Accounting.UI.Components.Pages.Charts;

public partial class Spendings : ComponentBase
{
    private PlotlyChart chart = null!;
    private Config config = null!;
    private Plotly.Blazor.Layout layout = null!;
    private IList<ITrace> data = null!;
    
    [Inject]
    private IViewService viewService { get; set; } = null!;
    
    protected override Task OnInitializedAsync()
    {
        config = new Config
        {
            Responsive = true
        };

        layout = new Plotly.Blazor.Layout
        {
        };
        
        var accounts = viewService.GetAccountViewList().Where(a => a.Class == ClassIds.Expenses);
        var values = new List<object>();
        var labels = new List<object>();
        foreach (var grp in accounts.GroupBy(a => a.SubClass).Select(a => new {Key = a.Key, List = a.ToList()}))
        {
            if (grp.List.Sum(g => g.Balance) != 0)
            {
                values.Add(grp.List.Sum(g => g.Balance));
                labels.Add(grp.List.First().SubClassName);
            }
        }

        data = new List<ITrace>
        {
            new Pie
            {
                Values =values,
                Labels = labels
            }
        };

        return base.OnInitializedAsync();  
    }
}