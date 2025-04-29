using System.Collections.Generic;
using Schaad.Accounting.Datasets.Charts;

namespace Schaad.Accounting.Interfaces
{
    public interface IChartService
    {
        IReadOnlyList<DataSerie> GetExpensesPerMonth();
    }
}