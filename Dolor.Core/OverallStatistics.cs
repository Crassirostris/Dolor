using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Statistics;

namespace Dolor.Core
{
    public class OverallStatistics
    {
        public Dictionary<string, Tuple<Parameter, Parameter>> Parameters { get; private set; }

        public Dictionary<string, double[]> Pearson { get; private set; }

        public Dictionary<string, double[]> Spearman { get; private set; }

        public Dictionary<string, double[]> Ttest { get; private set; }

        public OverallStatistics(Dictionary<string, Tuple<double[][], double[][]>> parametersValues)
        {
            Parameters = new Dictionary<string, Tuple<Parameter, Parameter>>();
            Pearson = new Dictionary<string, double[]>();
            Spearman = new Dictionary<string, double[]>();
        }

        public void Evaluate()
        {
            foreach (var kvp in Parameters)
            {
                var values1 = kvp.Value.Item1.Values;
                var values2 = kvp.Value.Item2.Values;

                Pearson[kvp.Key] = CalculateSeriesRelation(values1, values2, Correlation.Pearson);
                Spearman[kvp.Key] = CalculateSeriesRelation(values1, values2, Correlation.Spearman);
                Ttest[kvp.Key] = CalculateSeriesRelation(values1, values2, DolorCorrelation.Ttest);
            }
        }

        private static double[] CalculateSeriesRelation(DataRowStatistics[] values1, DataRowStatistics[] values2, Func<double[], double[], double> relationFunc)
        {
            return values1.Zip(values2, (row1, row2) => relationFunc(row1.Values, row2.Values)).ToArray();
        }
    }
}