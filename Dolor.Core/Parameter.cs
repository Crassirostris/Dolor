using System;
using System.CodeDom;
using System.Linq;

namespace Dolor.Core
{
    public class Parameter
    {
        public StatisticsInformation[] Values { get; private set; }

        public StatisticsInformation[] Differences { get; private set; }

        public StatisticsInformation[] RelativeDifferences { get; private set; }

        public Parameter(double[][] values)
        {
            Values = values.Select(row => new StatisticsInformation(row)).ToArray();
            Differences = new StatisticsInformation[values.Length];
            RelativeDifferences = new StatisticsInformation[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                int minuend = Math.Max(i, (i + 1) % values.Length);
                int subtrahend = Math.Min(i, (i + 1) % values.Length);
                Differences[i] = new StatisticsInformation(values[minuend].Zip(values[subtrahend], (left, right) => left - right));
                RelativeDifferences[i] = new StatisticsInformation(values[minuend].Zip(values[subtrahend], (left, right) => (left - right) / right));
            }
        }
    }
}
