using System;
using System.Linq;

namespace Dolor.Core
{
    public class Parameter
    {
        public DataRowStatistics[] Values { get; private set; }

        public DataRowStatistics[] Differences { get; private set; }

        public DataRowStatistics[] RelativeDifferences { get; private set; }

        public Parameter(double[][] values)
        {
            Values = values.Select(row => new DataRowStatistics(row)).ToArray();
            Differences = new DataRowStatistics[values.Length];
            RelativeDifferences = new DataRowStatistics[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                int minuend = Math.Max(i, (i + 1) % values.Length);
                int subtrahend = Math.Min(i, (i + 1) % values.Length);
                Differences[i] = new DataRowStatistics(values[minuend].Zip(values[subtrahend], (left, right) => left - right));
                RelativeDifferences[i] = new DataRowStatistics(values[minuend].Zip(values[subtrahend], (left, right) => (left - right) / right * 100));
            }
        }
    }
}
