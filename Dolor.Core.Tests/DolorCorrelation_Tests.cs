using System;
using System.Linq;
using NUnit.Framework;

namespace Dolor.Core.Tests
{
    [TestFixture]
    internal class DolorCorrelation_Tests
    {
        [Test]
        public void TtestTest()
        {
            var random = new Random();
            var row1 = Enumerable.Range(0, 100).Select(e => random.NextDouble()).ToArray();
            var row2 = Enumerable.Range(0, 100).Select(e => random.NextDouble()).ToArray();
            var result = DolorCorrelation.Ttest(row1, row2);
            Console.WriteLine(result);
        }
    }
}
