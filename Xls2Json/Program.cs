using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;

namespace Xls2Json
{
	public class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length != 1 || !File.Exists(args[0]))
			{
				PrintUsage();
				return;
			}
			Console.WriteLine(ParseWorkbook(args[0]));
		}

		private static void PrintUsage()
		{
			Console.WriteLine("Usage: {0} <path-to-xlsx>", AppDomain.CurrentDomain.FriendlyName);
		}

		private static JArray ParseWorkbook(string path)
		{
			var values = new List<ValueInfo>();
			using (var package = new ExcelPackage(new FileInfo(path)))
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
			return new JArray(values.GroupBy(e => e.FactorName).Select(g => new JObject
			{
				{ "label", g.Key },
				{ "values", new JArray(g.GroupBy(e => e.Time).Select(r => new JArray(r.OrderBy(q => q.Id).Select(q => q.Value)))) }
			}));
		}

		private class ValueInfo
		{
			public string FactorName { get; set; }
			public int Time { get; set; }
			public int Id { get; set; }
			public double Value { get; set; }
		}
	}
}
