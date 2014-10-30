using System;
using System.Linq;
using RDotNet;

namespace Dolor.Core
{
    public static class DolorCorrelation
    {
        private static readonly REngine rEngine;

        static DolorCorrelation()
        {
            rEngine = REngine.CreateInstance(Guid.NewGuid().ToString());
            rEngine.Initialize();
        }

        public static double Ttest(double[] left, double[] right)
        {
            var values1 = rEngine.CreateNumericVector(left);
            rEngine.SetSymbol("v1", values1);
            var values2 = rEngine.CreateNumericVector(right);
            rEngine.SetSymbol("v2", values2);
            var testResult = rEngine.Evaluate("t.test(v1, v2)").AsList();
            return testResult["p.value"].AsNumeric().First();
        }
    }
}