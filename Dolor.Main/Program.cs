using System;
using System.Collections.Generic;
using System.IO;
using Dolor.Core;

namespace Dolor.Main
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2 || !File.Exists(args[0]) || !File.Exists(args[1]))
            {
                PrintUsage();
                Environment.Exit(1);
            }
            IDataExtractor dataExtractor = DataExtractorProvider.Get();
            var firstSeriesData = ExtractSeriesData(args[0], dataExtractor);
            var secondSeriesData = ExtractSeriesData(args[1], dataExtractor);
            var mergesSeries = Merge(firstSeriesData, secondSeriesData);
            var overallStatistics = new OverallStatistics(mergesSeries);
            IResultRenderer renderer = ResultRendererProvider.Get();
            renderer.Render(overallStatistics);
        }

        private static Dictionary<string, Tuple<double[][], double[][]>> Merge(Dictionary<string, double[][]> first, Dictionary<string, double[][]> second)
        {
            var result = new Dictionary<string, Tuple<double[][], double[][]>>();
            foreach (var key in first.Keys)
                result[key] = new Tuple<double[][], double[][]>(first[key], second[key]);
            return result;
        }

        private static Dictionary<string, double[][]> ExtractSeriesData(string filename, IDataExtractor dataExtractor)
        {
            var firstSeriesData = new Dictionary<string, double[][]>();
            foreach (var line in File.ReadAllLines(filename))
                foreach (var kvp in dataExtractor.Extract(line))
                    firstSeriesData[kvp.Key] = kvp.Value;
            return firstSeriesData;
        }

        private static void PrintUsage()
        {
            Console.WriteLine(string.Join(Environment.NewLine, new []
            {
                "Usage:",
                string.Format("{0} <first_series_files_list> <second_series_files_list>", AppDomain.CurrentDomain.FriendlyName)
            }));
        }
    }
}
