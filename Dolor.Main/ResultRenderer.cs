using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dolor.Core;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.Style;

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
					package.Workbook.Worksheets.Add("Статистика" /*- хуистика*/);
				int currentRow = 6;
				foreach (var kvp in overallStatistics.Parameters)
				{
				    int experimentInitialRow = currentRow;
					RenderSeries("Э", sheet, kvp.Value.Item1, ref currentRow);
				    int controlInitialRow = currentRow;
					RenderSeries("К", sheet, kvp.Value.Item2, ref currentRow);
				    RenderSeriesRelation(controlInitialRow, overallStatistics.PValues[kvp.Key]);
				    SetCaptionStyle(kvp.Key, sheet.Cells[experimentInitialRow, 1, currentRow - 1, 1]);
				}
				package.Save();
			}
		}

	    private void RenderSeries(string groupName, ExcelWorksheet sheet, Parameter parameter, ref int currentRow)
	    {
	        sheet.Cells[currentRow, 2].LoadFromArrays(TransposeData(new[]
	        {
	            new object[] { groupName },
	            parameter.Values.Select((e, i) => (object) ("t=" + i)).ToArray(),
	            parameter.Values.Select(e => (object) e.Mean).ToArray(),
	            parameter.Values.Select(e => (object) e.StandardDeviation).ToArray(),
	            parameter.Values.Select(e => (object) (e.StandardDeviation / e.Mean * 100)).ToArray(),
	            new object[] { null }.Concat(parameter.Differences.Select(e => (object) e.Mean)).ToArray(),
	            new object[] { null }.Concat(parameter.RelativeDifferences.Select(e => (object) e.Mean)).ToArray(),
	            new object[] { },
	            parameter.Values.Select(stat => stat.PValue).Cast<object>().ToArray()
	        }));
	        var dataLengthRows = parameter.Values.Length + 1;
            SetCaptionStyle(groupName, sheet.Cells[currentRow, 2, currentRow + dataLengthRows - 1, 2]);
	        currentRow += dataLengthRows;
	    }

	    private void SetCaptionStyle(string value, ExcelRange excelRange)
	    {
	        excelRange.Style.Font.Bold = true;
	        excelRange.Style.WrapText = true;
            excelRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
	        excelRange.Merge = true;
	        excelRange.Value = value;
	    }

	    private void RenderSeriesRelation(int controlInitialRow, double[] pValues)
	    {
	        
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