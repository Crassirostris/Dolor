using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Statistics;

namespace Dolor.Core
{
    public class Series
    {
        public Dictionary<string, Tuple<Parameter, Parameter>> Parameters { get; private set; }

        public Dictionary<string, double[]> Pearson { get; private set; }

        public Dictionary<string, double[]> Spearman { get; private set; }

        public Series(Dictionary<string, Tuple<double[][], double[][]>> parametersValues)
        {
            Parameters = new Dictionary<string, Tuple<Parameter, Parameter>>();
            Pearson = new Dictionary<string, double[]>();
            Spearman = new Dictionary<string, double[]>();

            foreach (var kvp in parametersValues)
            {
                Parameters[kvp.Key] = new Tuple<Parameter, Parameter>(new Parameter(kvp.Value.Item1), new Parameter(kvp.Value.Item2));
                var values1 = Parameters[kvp.Key].Item1.Values;
                var values2 = Parameters[kvp.Key].Item2.Values;
                Pearson[kvp.Key] = values1.Zip(values2, (row1, row2) => Correlation.Pearson(row1.Values, row2.Values)).ToArray();
                Spearman[kvp.Key] = values1.Zip(values2, (row1, row2) => Correlation.Spearman(row1.Values, row2.Values)).ToArray();
            }
        }
    }
}