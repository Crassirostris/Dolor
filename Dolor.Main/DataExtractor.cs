using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace Dolor.Main
{
	internal class DataExtractor : IDataExtractor
	{
		public Dictionary<string, double[][]> Extract(string filename)
		{
			var values = new List<ValueInfo>();
			using (var package = new ExcelPackage(new FileInfo(filename)))
			{
				foreach (var sheet in package.Workbook.Worksheets)
				{
					var factorName = "";
					for (int i = 7; i <= sheet.Cells.Rows && sheet.Cells.Offset(i, 0).Any(); i++)
					{
						if (!sheet.Name.StartsWith("Пациент"))
							continue;
						var row = sheet.Cells[sheet.Cells[i, 1, i, 3].Address];
						var rowValues = row.Select(cell => cell.GetValue<string>()).ToList();
						if (!string.IsNullOrEmpty(rowValues[0]))
							factorName = rowValues[0];
						if (string.IsNullOrEmpty(rowValues[1]))
							continue;
						values.Add(new ValueInfo
						{
							FactorName = factorName,
							Time = int.Parse(rowValues[1].Substring(2)),
							Value = double.Parse(rowValues[2]),
							Id = sheet.Index
						});
					}
				}
			}
			return values.GroupBy(e => e.FactorName)
				.ToDictionary(g => g.Key, g => g.GroupBy(e => e.Time)
					.Select(r => r.OrderBy(q => q.Id).Select(q => q.Value).ToArray()).ToArray());
		}
	}
}