namespace Dbarone.Net.Extensions;
using System.Text;
using System.Globalization;

/// <summary>
/// Specifies a string case.
/// </summary>
public enum CaseType
{
    /// <summary>
    /// No specific case defined.
    /// </summary>
    None,

    /// <summary>
    /// Lower case string.
    /// </summary>
    LowerCase,

    /// <summary>
    /// Upper case string.
    /// </summary>
    UpperCase,

    /// <summary>
    /// Pascal case string.
    /// </summary>
    PascalCase,

    /// <summary>
    /// Camel case string.
    /// </summary>
    CamelCase,

    /// <summary>
    /// Snake case string.
    /// </summary>
    SnakeCase
}

/// <summary>
/// The justification type.
/// </summary>
public enum Justification
{
    /// <summary>
    /// Left-aligned
    /// </summary>
    LEFT,

    /// <summary>
    /// Centre-aligned
    /// </summary>
    CENTRE,

    /// <summary>
    /// Right-aligned
    /// </summary>
    RIGHT
}

/// <summary>
/// A collection of string extension methods.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Allows a short (TimeLow) guid or full guid to be converted to Guid
    /// </summary>
    /// <param name="str">The input string value.</param>
    /// <returns></returns>
    public static Guid ToGuid(this string str)
    {
        if (str.Length == 8)
            return new Guid(string.Format("{0}-0000-0000-0000-000000000000", str));
        else
            return new Guid(str);
    }

    /// <summary>
    /// Justifies text.
    /// </summary>
    /// <param name="str">The input string to justify.</param>
    /// <param name="length">The length of text.</param>
    /// <param name="justification">The justification style.</param>
    /// <returns></returns>
    public static string Justify(this string str, int length, Justification justification)
    {
        if (str.Length > length)
            str = str.Substring(0, length);

        if (justification == Justification.LEFT)
            return str.PadRight(length);
        else if (justification == Justification.CENTRE)
            return (" " + str.PadRight(length / 2).PadLeft(length / 2)).PadRight(length);
        else
            return str.PadLeft(length);
    }

    /// <summary>
    /// Parses a string for arguments. Arguments can be separated by whitespace. Single or double quotes
    /// can be used to delimit fields that contain space characters.
    /// </summary>
    /// <param name="str">The input string to parse.</param>
    /// <returns></returns>
    public static string[] ParseArgs(this string str)
    {
        List<string> args = new List<string>();
        string currentArg = "";
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(str ?? "")))
        {
            using (var sr = new StreamReader(ms))
            {
                bool inWhiteSpace = false;
                char inQuoteChar = '\0';
                char nextChar;
                while (!sr.EndOfStream)
                {
                    nextChar = (char)sr.Read();
                    if (inQuoteChar == '\0' && (nextChar == '\'' || nextChar == '"'))
                    {
                        // Start of quoted field
                        inQuoteChar = nextChar;
                        currentArg = "";
                    }
                    else if (nextChar == inQuoteChar && nextChar != '\0')
                    {
                        // End of quoted field
                        // The end of quoted field MUST be followed by whitespace.
                        args.Add(currentArg);
                        inQuoteChar = '\0';
                    }
                    else if (!inWhiteSpace && inQuoteChar == '\0' && string.IsNullOrWhiteSpace(nextChar.ToString()))
                    {
                        // Start of whitespace, not in quoted field
                        args.Add(currentArg);
                        inWhiteSpace = true;
                    }
                    else if (inWhiteSpace && inQuoteChar == '\0' && !string.IsNullOrWhiteSpace(nextChar.ToString()))
                    {
                        // Start of new argument
                        currentArg = nextChar.ToString();
                        inWhiteSpace = false;
                    }
                    else
                    {
                        currentArg += nextChar.ToString();
                    }
                }
                if (!string.IsNullOrEmpty(currentArg))
                    args.Add(currentArg);
            }
        }
        return args.ToArray();
    }

    /// <summary>
    /// Wrapper for .NET IsNullOrWhiteSpace method.
    /// </summary>
    /// <param name="str">Input value to test.</param>
    /// <returns></returns>
    public static bool IsNullOrWhiteSpace(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// Wrapper for .NET IsNullOrEmpty method.
    /// </summary>
    /// <param name="str">input value to test.</param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// Removes characters from the right end of a string
    /// </summary>
    /// <param name="str">The input string.</param>
    /// <param name="length">The required length of the string.</param>
    /// <returns></returns>
    public static string RemoveRight(this string str, int length)
    {
        return str.Remove(str.Length - length);
    }

    /// <summary>
    /// Removes characters from the left end of a string
    /// </summary>
    /// <param name="str">The input string.</param>
    /// <param name="length">The required length of the string.</param>
    /// <returns></returns>
    public static string RemoveLeft(this string str, int length)
    {
        return str.Remove(0, length);
    }

    /// <summary>
    /// Converts a string value to a stream.
    /// </summary>
    /// <param name="str">The input string.</param>
    /// <returns></returns>
    public static Stream ToStream(this string str)
    {
        MemoryStream stream = new MemoryStream();
        StreamWriter writer = new StreamWriter(stream);
        writer.Write(str);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    private static IEnumerable<string> WordWrapInternal(string str, int length)
    {
        if (str == null)
            throw new ArgumentNullException("s");
        if (length <= 0)
            throw new ArgumentException("Part length has to be positive.", "partLength");

        var i = 0;
        while (i < str.Length)
        {
            // remove white space at start of line
            while (i < str.Length && char.IsWhiteSpace(str[i]))
                i++;

            var j = length;   // add extra character to check white space just after line.

            while (j >= 0)
            {
                if (i + j < str.Length && char.IsWhiteSpace(str[i + j]))
                    break;
                else if (i + j == str.Length)
                    break;
                j--;
            }
            if (j <= 0 || j > length)
                j = length;

            if (i + j >= str.Length)
                j = str.Length - i;

            var result = str.Substring(i, j);
            i += j;
            yield return result;
        }
    }

    /// <summary>
    /// Splits a string into chunks of [length] characters. Word breaks are avoided.
    /// </summary>
    /// <param name="str">The input string</param>
    /// <param name="length">The length of the line to wrap.</param>
    /// <param name="newLineChars">The new line string.</param>
    /// <returns>Returns wrapped text.</returns>
    public static string WordWrap(this string str, int length, string newLineChars)
    {
        var lines = str.Split(newLineChars);
        var inputArray = new List<string>();

        foreach (var line in lines)
        {
            inputArray.AddRange(WordWrapInternal(line, length));
        }
        return string.Join(newLineChars, inputArray);
    }

    public static string Table(this string table, int length, bool firstRowHeaders, string newLineChars, string columnSeparator)
    {
        List<List<string>> cells = new List<List<string>>();
        var lines = table.Split(newLineChars);
        foreach (var line in lines)
        {
            List<string> rowCells = line.Split(columnSeparator).ToList();
            cells.Add(rowCells);
        }
        return cells.Table(length, firstRowHeaders, newLineChars);
    }

    /// <summary>
    /// Takes a 2 dimensional array of cell values, and formats into a table.
    /// </summary>
    /// <param name="cells">2-dimensional array of cell values.</param>
    /// <param name="length">width of the table.</param>
    /// <param name="firstRowHeaders">Set to true if the first row are headers.</param>
    /// <returns>Sequence of string values representing a table.</returns>
    public static string Table(this IEnumerable<IEnumerable<string>> cells, int length, bool firstRowHeaders, string newLineChars)
    {
        // Algorithm:
        // 1. Make sure all rows have same number of cells (columns)
        // 2. Calculate number of cell separators required (columns - 1)
        // 2. Loop through all rows, getting max length for each column
        // 3. If maxlength <= length-cellseparators, then format each cell according its max length, join cells using space, and return array
        // 4. Otherwise:
        // 4.1 Take column with widest length, and reduce by and do wordwrap
        // 4.2 If maxlength <= length-cellseparators, then format each cell according its max length, join cells using space, and return array
        // 4.3 go to 4.1

        if (cells is null)
        {
            throw new Exception("Must provide array of cells.");
        }
        if (cells.Count() == 0)
        {
            return "";
        }
        if (length < 0)
        {
            throw new Exception("Length must be positive");
        }

        List<List<string>> cellArray = new List<List<string>>();
        foreach (var line in cells)
        {
            cellArray.Add(line.ToList());
        }

        int columns = 0;
        foreach (var row in cellArray)
        {
            var c = row.Count();
            if (columns == 0)
            {
                columns = c;
            }
            else
            {
                if (c != columns)
                {
                    throw new Exception("Cell array is ragged.");
                }
            }
        }
        List<int> columnWidths = new int[columns].ToList();
        if (length < (columns * 2) - 1)
        {
            throw new Exception("Length too small.");
        }
        var separators = columns - 1;

        // Get initial column widths
        foreach (var row in cellArray)
        {
            for (int i = 0; i < row.Count(); i++)
            {
                var str = row[i];
                var l = str.Length;
                if (columnWidths[i] == 0)
                {
                    columnWidths[i] = l;
                }
                else if (l > columnWidths[i])
                {
                    columnWidths[i] = l;
                }
            }
        }

        while (columnWidths.Sum() > length - separators)
        {
            var index = 0;
            var max = 0;
            for (int i = 0; i < columnWidths.Count(); i++)
            {
                if (columnWidths[i] > max)
                {
                    max = columnWidths[i];
                    index = i;
                }
            }
            // reduce size of widest column
            columnWidths[index] = columnWidths[index] - 1;

            for (var i = 0; i < cellArray.Count(); i++)
            {
                for (int j = 0; j < cellArray[i].Count; j++)
                {
                    cellArray[i][j] = cellArray[i][j].WordWrap(columnWidths[j], newLineChars);
                }
            }
        }

        // At this point, all columns should be narrow enough to fit in table length.
        // We need to scan each row, and pad all columns with newline characters, so
        // all rows have same number of wrapped lines.
        for (int i = 0; i < cellArray.Count(); i++)
        {
            int maxLines = 0;
            for (int j = 0; j < cellArray[i].Count(); j++)
            {
                var lines = cellArray[i][j].Lines(newLineChars);
                if (lines > maxLines)
                {
                    maxLines = lines;
                }
            }
            for (int j = 0; j < cellArray[i].Count(); j++)
            {
                var lines = cellArray[i][j].Lines(newLineChars);
                cellArray[i][j] = cellArray[i][j] + newLineChars.Repeat(maxLines - lines);
            }
        }

        // At this point, all rows should fit into width, and all have same number of lines.
        List<string> output = new List<string>();
        for (int i = 0; i < cellArray.Count(); i++)
        {
            if (i == 1 && firstRowHeaders)
            {
                // draw headers
                List<string> headerLines = new List<string>();
                for (int j = 0; j < cellArray[i].Count; j++)
                {
                    headerLines.Add("-".Repeat(columnWidths[j]));
                }
                output.Add(string.Join(" ", headerLines));
            }

            var lines = cellArray[i][0].Lines(newLineChars);
            for (int l = 0; l < lines; l++)
            {
                var strArray = new List<string>();
                for (int j = 0; j < cellArray[i].Count(); j++)
                {
                    strArray.Add(cellArray[i][j].Split(newLineChars)[l].Justify(columnWidths[j], Justification.LEFT));
                }
                output.Add(string.Join(" ", strArray));
            }
        }
        return string.Join(newLineChars, output);
    }

    public static string Repeat(this string str, int count)
    {
        return string.Join("", Enumerable.Repeat(str, count));
    }

    /// <summary>
    /// Returns the number of lines that a string contains.
    /// </summary>
    /// <param name="str">The string value.</param>
    /// <param name="newLineChars">The newline characters.</param>
    /// <returns>Returns the count of lines.</returns>
    public static int Lines(this string str, string newLineChars)
    {
        return str.Split(newLineChars).Count();
    }

    /// <summary>
    /// Converts a string to snake case.
    /// </summary>
    /// <param name="str">The input string value.</param>
    /// <param name="delimiter">Optional delimiter. Defaults to "_".</param>
    /// <returns></returns>
    public static string ToSnakeCase(this string str, string delimiter = "_")
    {
        return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? delimiter + x.ToString() : x.ToString())).ToLower();
    }

    /// <summary>
    /// Converts a string to a specific case. As well as converting to upper/lower case, this method can be used
    /// to convert text between Pascal, camel, and snake case.
    /// </summary>
    /// <param name="str">The input string.</param>
    /// <param name="case">The case to convert to.</param>
    /// <param name="culture">Option culture setting.</param>
    /// <param name="delimiter">Optional delimiter</param>
    /// <returns></returns>
    public static string ChangeCase(this string str, CaseType @case, CultureInfo? culture = null, string delimiter = "_")
    {
        switch (@case)
        {
            case CaseType.None:
                return str;
            case CaseType.LowerCase:
                return str.ToLower(culture).Replace(delimiter, "");
            case CaseType.UpperCase:
                return str.ToUpper(culture).Replace(delimiter, "");
            case CaseType.PascalCase:
                return (char.ToUpper(str[0]) + str.Substring(1)).Replace(delimiter, "");
            case CaseType.CamelCase:
                return (char.ToLower(str[0]) + str.Substring(1)).Replace(delimiter, "");
            case CaseType.SnakeCase:
                return str.ToSnakeCase(delimiter);
            default:
                return str;
        }
    }

    public static bool IsCase(this string str, CaseType @case, CultureInfo? culture = null, string delimiter = "_")
    {
        var changedCase = str.ChangeCase(@case, culture, delimiter);
        if (str.IsNullOrEmpty())
        {
            return false;
        }
        if (changedCase.Equals(str, StringComparison.Ordinal))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Implementation of Like function similar to SQL Like.
    /// Single wildcard character is '_' and multi-character wildcard is '%'.
    /// Also supports [] characters to define character domains.
    /// See: https://stackoverflow.com/questions/5417070/c-sharp-version-of-sql-like/8583383#8583383
    /// </summary>
    /// <param name="str">The input string.</param>
    /// <param name="pattern">The pattern to test for.</param>
    /// <returns>Returns true if the pattern matches the input string.</returns>
    public static bool Like(this string str, string pattern)
    {
        bool isMatch = true,
            isWildCardOn = false,
            isCharWildCardOn = false,
            isCharSetOn = false,
            isNotCharSetOn = false,
            endOfPattern = false;
        int lastWildCard = -1;
        int patternIndex = 0;
        List<char> set = new List<char>();
        char p = '\0';

        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];
            endOfPattern = (patternIndex >= pattern.Length);
            if (!endOfPattern)
            {
                p = pattern[patternIndex];

                if (!isWildCardOn && p == '%')
                {
                    lastWildCard = patternIndex;
                    isWildCardOn = true;
                    while (patternIndex < pattern.Length &&
                        pattern[patternIndex] == '%')
                    {
                        patternIndex++;
                    }
                    if (patternIndex >= pattern.Length) p = '\0';
                    else p = pattern[patternIndex];
                }
                else if (p == '_')
                {
                    isCharWildCardOn = true;
                    patternIndex++;
                }
                else if (p == '[')
                {
                    if (pattern[++patternIndex] == '^')
                    {
                        isNotCharSetOn = true;
                        patternIndex++;
                    }
                    else isCharSetOn = true;

                    set.Clear();
                    if (pattern[patternIndex + 1] == '-' && pattern[patternIndex + 3] == ']')
                    {
                        char start = char.ToUpper(pattern[patternIndex]);
                        patternIndex += 2;
                        char end = char.ToUpper(pattern[patternIndex]);
                        if (start <= end)
                        {
                            for (char ci = start; ci <= end; ci++)
                            {
                                set.Add(ci);
                            }
                        }
                        patternIndex++;
                    }

                    while (patternIndex < pattern.Length &&
                        pattern[patternIndex] != ']')
                    {
                        set.Add(pattern[patternIndex]);
                        patternIndex++;
                    }
                    patternIndex++;
                }
            }

            if (isWildCardOn)
            {
                if (char.ToUpper(c) == char.ToUpper(p))
                {
                    isWildCardOn = false;
                    patternIndex++;
                }
            }
            else if (isCharWildCardOn)
            {
                isCharWildCardOn = false;
            }
            else if (isCharSetOn || isNotCharSetOn)
            {
                bool charMatch = (set.Contains(char.ToUpper(c)));
                if ((isNotCharSetOn && charMatch) || (isCharSetOn && !charMatch))
                {
                    if (lastWildCard >= 0) patternIndex = lastWildCard;
                    else
                    {
                        isMatch = false;
                        break;
                    }
                }
                isNotCharSetOn = isCharSetOn = false;
            }
            else
            {
                if (char.ToUpper(c) == char.ToUpper(p))
                {
                    patternIndex++;
                }
                else
                {
                    if (lastWildCard >= 0) patternIndex = lastWildCard;
                    else
                    {
                        isMatch = false;
                        break;
                    }
                }
            }
        }
        endOfPattern = (patternIndex >= pattern.Length);

        if (isMatch && !endOfPattern)
        {
            bool isOnlyWildCards = true;
            for (int i = patternIndex; i < pattern.Length; i++)
            {
                if (pattern[i] != '%')
                {
                    isOnlyWildCards = false;
                    break;
                }
            }
            if (isOnlyWildCards) endOfPattern = true;
        }
        return isMatch && endOfPattern;
    }
}
