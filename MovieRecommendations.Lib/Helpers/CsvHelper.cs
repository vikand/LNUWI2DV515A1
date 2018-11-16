using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MovieRecommendations.Lib.Helpers
{
    public class CsvHelper : ICsvHelper
    {
        public IEnumerable<string[]> ReadAllLines(string path, char fieldSeparator = ';', bool skipHeaderLine = false)
        {
            return this.ReadAllLines(path, fieldSeparator, 0, int.MaxValue, skipHeaderLine, null);
        }

        public IEnumerable<string[]> ReadAllLines(string path, int minColumns, int maxColumns, char fieldSeparator = ';', bool skipHeaderLine = true)
        {
            return this.ReadAllLines(path, fieldSeparator, minColumns, maxColumns, skipHeaderLine, null);
        }

        public IEnumerable<string[]> ReadAllLines(string path, string[] columns, char fieldSeparator = ';', bool skipHeaderLine = true)
        {
            return this.ReadAllLines(path, fieldSeparator, columns.Length, columns.Length, skipHeaderLine, columns);
        }

        public IEnumerable<string[]> ReadAllLines(string path, string columns, char fieldSeparator = ';', bool skipHeaderLine = true)
        {
            return this.ReadAllLines(path, columns.Split(fieldSeparator), fieldSeparator, skipHeaderLine);
        }

        private IEnumerable<string[]> ReadAllLines(
            string path,
            char fieldDelimiter,
            int minColumns,
            int maxColumns,
            bool skipInitialHeaderLine,
            string[] expectedHeaderLine)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Argument cannot be null or empty", nameof(path));
            }

            if (minColumns < 0)
            {
                throw new ArgumentException("Argument cannot be negative", nameof(minColumns));
            }

            if (minColumns < 0)
            {
                throw new ArgumentException("Argument cannot be negative", nameof(maxColumns));
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File not found: '{path}'");
            }

            var lines = new List<string[]>();

            using (var reader = new StreamReader(path))
            {
                string line = reader.ReadLine();
                string[] values;

                if (skipInitialHeaderLine || expectedHeaderLine != null)
                {
                    if (line == null)
                    {
                        throw new InvalidDataException($"Expected header line missing in file '{path}.");
                    }

                    values = SplitLineIntoValues(line, fieldDelimiter);

                    if (expectedHeaderLine != null)
                    {
                        if (values.Length != expectedHeaderLine.Length)
                        {
                            throw new InvalidDataException($"Unxpected number of headers in in file '{path}.");
                        }

                        for (var index = 0; index < values.Length; index++)
                        {
                            if (!values[index].Equals(expectedHeaderLine[index], StringComparison.InvariantCultureIgnoreCase))
                            {
                                throw new InvalidDataException($"Unxpected header in in file '{path}.");
                            }
                        }
                    }

                    if (!skipInitialHeaderLine)
                    {
                        lines.Add(values);
                    }

                    line = reader.ReadLine();
                }

                while (line != null)
                {
                    values = SplitLineIntoValues(line, fieldDelimiter);

                    if (values.Length < minColumns)
                    {
                        throw new InvalidDataException($"Wrong number of columns in file '{path}': Expected={minColumns}, Actual={values.Length}");
                    }

                    if (values.Length > minColumns)
                    {
                        throw new InvalidDataException($"Wrong number of columns in file '{path}': Expected={maxColumns}, Actual={values.Length}");
                    }

                    lines.Add(values);

                    line = reader.ReadLine();
                }
            }

            return lines;
        }

        private string[] SplitLineIntoValues(string line, char delimiter)
        {
            var values = line.Split(delimiter);

            for (var index = 0; index < values.Length; index++)
            {
                var value = values[index];
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (value.StartsWith("\"") && value.EndsWith("\""))
                    {
                        values[index] = value.Substring(1, value.Length - 2);
                    }
                }
            }

            return values;
        }
    }
}

