namespace Dolor.Main
{
	internal static class ResultRendererProvider
	{
		public static IResultRenderer Get()
		{
			return new ResultRenderer(DefaultResultsPath, DefaultTemplatePath);
		}

		private const string DefaultResultsPath = "results.xlsx";
		private const string DefaultTemplatePath = "results_template.xlsx";
	}
}