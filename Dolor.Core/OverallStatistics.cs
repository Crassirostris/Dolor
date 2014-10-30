using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Statistics;

namespace Dolor.Core
{
    public class OverallStatistics
    {
        public Dictionary<string, Tuple<Parameter, Parameter>> Parameters { get; private set; }

        public Dictionary<string, double[]> PValues { get; private set; }

        private OverallStatistics()
        {
            Parameters = new Dictionary<string, Tuple<Parameter, Parameter>>();
            PValues = new Dictionary<string, double[]>();
        }

        public static OverallStatistics CreateNew(Dictionary<string, Tuple<double[][], double[][]>> parametersValues)
        {
            var overallStatistics = new OverallStatistics();

            foreach (var kvp in parametersValues)
            {
                var parameterFromSeries1 = new Parameter(kvp.Value.Item1);
                var parameterFromSeries2 = new Parameter(kvp.Value.Item2);

                overallStatistics.Parameters[kvp.Key] = new Tuple<Parameter, Parameter>(parameterFromSeries1, parameterFromSeries2);
                overallStatistics.PValues[kvp.Key] = CalculateSeriesRelation(parameterFromSeries1.Values, parameterFromSeries2.Values, DolorCorrelation.Ttest);
            }

            return overallStatistics;
        }

        private static double[] CalculateSeriesRelation(DataRowStatistics[] values1, DataRowStatistics[] values2, Func<double[], double[], double> relationFunc)
        {
            return values1.Zip(values2, (row1, row2) => relationFunc(row1.Values, row2.Values)).ToArray();
        }
    }
}