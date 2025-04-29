using System;
using System.Collections.Generic;

namespace Schaad.Accounting.Datasets.Charts
{
    public record DataSerie(string Id, string Name, IReadOnlyList<DateOnly> X, IReadOnlyList<decimal> Y);
}