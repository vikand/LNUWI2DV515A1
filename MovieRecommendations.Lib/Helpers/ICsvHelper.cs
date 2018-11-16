using System.Collections.Generic;

namespace MovieRecommendations.Lib.Helpers
{
    public interface ICsvHelper
    {
        IEnumerable<string[]> ReadAllLines(string path, char fieldSeparator = ';', bool skipHeaderLine = false);
        IEnumerable<string[]> ReadAllLines(string path, int minColumns, int maxColumns, char fieldSeparator = ';', bool skipHeaderLine = true);
        IEnumerable<string[]> ReadAllLines(string path, string[] columns, char fieldSeparator = ';', bool skipHeaderLine = true);
        IEnumerable<string[]> ReadAllLines(string path, string columns, char fieldSeparator = ';', bool skipHeaderLine = true);
    }
}