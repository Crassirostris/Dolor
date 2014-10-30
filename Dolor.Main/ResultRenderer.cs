using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dolor.Core;
using OfficeOpenXml;

namespace Dolor.Main
{
	internal class ResultRenderer : IResultRenderer
	{
		public ResultRenderer(string resultsPath, string templatePath)
		{
			this.resultsPath = resultsPath;
			this.templatePath = templatePath;
		}

		public void Render(OverallStatistics overallStatistics)
		{
			var fileInfo = new FileInfo(resultsPath);
			if (fileInfo.Exists)
				File.Delete(fileInfo.FullName);
			using (var package = new ExcelPackage(fileInfo, new FileInfo(templatePath)))
			{
				var sheet = package.Workbook.Worksheets.Count > 0 ?
					package.Workbook.Worksheets.First() :
					package.Workbook.Worksheets.Add(Статистика" /*- хуистика*/);
				int row = 6;
				foreach (var parameter in overallStatistics.Parameters)
				{
					sheet.Cells[row, 1].Value = parameter.Key;
					row += RenderGroup(sheet.Cells.Offset(row - 1, 2), parameter.Value.Item1,
						Enumerable.Empty<double>(), Enumerable.Empty<double>()).Rows;
					row += RenderGroup(sheet.Cells.Offset(row - 1, 2), parameter.Value.Item2,
						overallStatistics.Pearson[parameter.Key], overallStatistics.Spearman[parameter.Key]).Rows;
				}
				package.Save();
			}
		}

		private ExcelRangeBase RenderGroup(ExcelRangeBase range, Parameter parameter, IEnumerable<double> pearson, IEnumerable<double> spearman)
		{
			return range.LoadFromArrays(TransposeData(new[]
			{
				parameter.Values.Select((e, i) => (object)("t=" + i)).ToArray(),
				parameter.Values.Select(e => (object)e.Mean).ToArray(),
				parameter.Values.Select(e => (object)e.Variance).ToArray(),
				new object[] {null}.Concat(parameter.Differences.Select(e => (object)e.Mean)).ToArray(),
				new object[] {null}.Concat(parameter.Differences.Select(e => (object)e.Variance)).ToArray(),
				new object[] {null}.Concat(parameter.RelativeDifferences.Select(e => (object)e.Mean)).ToArray(),
				new object[] {null}.Concat(parameter.RelativeDifferences.Select(e => (object)e.Variance)).ToArray(),
				new object[] {}, 
				pearson.Select(e => (object)e).ToArray(),
				spearman.Select(e => (object)e).ToArray()
			}));
		}

		private IEnumerable<object[]> TransposeData(object[][] data)
		{
			var columnLength = data.Max(e => e.Length);
			return Enumerable.Range(0, columnLength).Select(i => data.Select(row => i >= row.Length ? null : row[i]).ToArray());
		}

		private readonly string resultsPath;
		private readonly string templatePath;
	}
}