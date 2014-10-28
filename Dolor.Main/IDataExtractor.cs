using System.Collections.Generic;

namespace Dolor.Main
{
    internal interface IDataExtractor
    {
        Dictionary<string, double[][]> Extract(string filename);
    }
}