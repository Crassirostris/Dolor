namespace Dolor.Main
{
	internal static class ResultRendererProvider
	{
		public static IResultRenderer Get()
		{
			return new ResultRenderer(DefaultResultsPath);
		}

		private const string DefaultResultsPath = "results.xlsx";
	}
}