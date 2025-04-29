using Microsoft.AspNetCore.Components;
using Plotly.Blazor;
using Plotly.Blazor.Traces;
using Schaad.Accounting.Interfaces;

namespace Schaad.Accounting.UI.Components.Pages.Charts;

public partial class Assets : ComponentBase
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
        
        var accounts = viewService.GetAccountViewList().Where(a => a.Class == ClassIds.Activa);
        var values = new List<object>();
        var labels = new List<object>();
        var ids = new List<object>();
        foreach (var account in accounts)
        {
            if (account.BalanceCHF > 0)
            {
                values.Add(account.BalanceCHF);
                labels.Add(account.Name);
                ids.Add(account.Id);
            }
        }

        data = new List<ITrace>
        {
            new Pie
            {
                Values = values,
                Labels = labels,
                Ids = ids
            }
        };

        return base.OnInitializedAsync();  
    }
}