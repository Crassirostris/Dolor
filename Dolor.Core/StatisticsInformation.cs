﻿using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Statistics;

namespace Dolor.Core
{
    public class StatisticsInformation
    {
        public double[] Values { get; private set; }

        public double Mean { get; private set; }

        public double Variance { get; private set; }

        public StatisticsInformation(IEnumerable<double> values)
        {
            Values = values.ToArray();
            Mean = Values.Mean();
            Variance = Values.Variance();
        }
    }
}