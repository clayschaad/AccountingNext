using Microsoft.AspNetCore.Components;
using Plotly.Blazor;
using Plotly.Blazor.LayoutLib;
using Plotly.Blazor.Traces;
using Schaad.Accounting.Interfaces;

namespace Schaad.Accounting.UI.Components.Pages.Charts;

public partial class SpendingsOverTime : ComponentBase
{
    private PlotlyChart chart = null!;
    private Config config = null!;
    private Plotly.Blazor.Layout layout = null!;
    private IList<ITrace> data = new List<ITrace>();
    
    [Inject]
    private IChartService chartService { get; set; } = null!;
    
    protected override Task OnInitializedAsync()
    {
        config = new Config
        {
            Responsive = true
        };

        layout = new Plotly.Blazor.Layout
        {
            Title = new Title { Text = "Ausgaben im Verlauf" },
            BarMode = BarModeEnum.Stack,
            YAxis = new List<YAxis>
            {
                new()
                {
                    Title = new Plotly.Blazor.LayoutLib.YAxisLib.Title { Text = "CHF" }
                }
            },
            XAxis = new List<XAxis>
            {
                new()
                {
                    Type = Plotly.Blazor.LayoutLib.XAxisLib.TypeEnum.Date
                }
            }
        };
        

        var dataSeries = chartService.GetExpensesPerMonth();
        foreach (var dataSerie in dataSeries)
        {
            data.Add(new Bar
            {
                Name = dataSerie.Name,
                X = dataSerie.X.Select(d => (object)d).ToList(),
                Y = dataSerie.Y.Select(d => (object)d).ToList(),
            });
        };

        return base.OnInitializedAsync();  
    }
}