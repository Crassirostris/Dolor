using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Statistics;

namespace Dolor.Core
{
    public class DataRowStatistics
    {
        public double[] Values { get; private set; }

        public double Mean { get; private set; }

        public double Variance { get; private set; }

        public double StandardDeviation { get; set; }

        public DataRowStatistics(IEnumerable<double> values)
        {
            Values = values.ToArray();
            Mean = Values.Mean();
            Variance = Values.Variance();
            StandardDeviation = Values.StandardDeviation();
        }
    }
}