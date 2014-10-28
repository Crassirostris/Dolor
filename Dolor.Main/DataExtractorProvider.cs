using System;

namespace Dolor.Main
{
    internal static class DataExtractorProvider
    {
        public static IDataExtractor Get()
        {
	        return new DataExtractor();
        }
    }
}