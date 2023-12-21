#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion

using System.Globalization;
using System.Text;

namespace HKW.HKWTOML;

/// <summary>
/// TOML解析器
/// </summary>
public class TomlParser : IDisposable
{
    /// <summary>
    /// 解析状态
    /// </summary>
    public enum ParseState
    {
        /// <summary>
        /// 空
        /// </summary>
        None,

        /// <summary>
        /// 键值对
        /// </summary>
        KeyValuePair,

        /// <summary>
        /// 跳过下一行
        /// </summary>
        SkipToNextLine,

        /// <summary>
        /// 表格
        /// </summary>
        Table
    }

    /// <summary>
    /// 强制ASCII编码
    /// </summary>
    public bool ForceASCII { get; set; }

    /// <summary>
    /// 文本写入器
    /// </summary>
    private readonly TextReader _reader;

    /// <summary>
    /// 当前状态
    /// </summary>
    private ParseState _currentState;

    /// <summary>
    /// 行
    /// </summary>
    private int _line;

    /// <summary>
    /// 列
    /// </summary>
    private int _column;

    private List<TomlSyntaxException> _syntaxErrors = null!;

    /// <summary>
    /// 从文本读取器解析
    /// </summary>
    /// <param name="reader">文明读取器</param>
    public TomlParser(TextReader reader)
    {
        _reader = reader;
        _line = _column = 0;
    }

    /// <summary>
    /// 弃置
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _reader?.Dispose();
    }

    /// <summary>
    /// 解析
    /// </summary>
    /// <returns>解析完成的Toml表格</returns>
    /// <exception cref="TomlParseException">解析错误</exception>
    public TomlTable Parse()
    {
        _syntaxErrors = new List<TomlSyntaxException>();
        _line = _column = 1;
        var rootTable = new TomlTable();
        var currentTable = rootTable;
        _currentState = ParseState.None;
        var keyParts = new List<string>();
        var isArrayTable = false;
        StringBuilder latestComment = null!;
        var isFirstComment = true;

        int currentChar;
        while ((currentChar = _reader.Peek()) >= 0)
        {
            var c = (char)currentChar;

            if (_currentState is ParseState.None)
            {
                if (
                    ParseNone(c, rootTable, ref isFirstComment, ref latestComment)
                    is bool noneParsed
                )
                {
                    if (noneParsed)
                        ConsumeCharacter();
                    continue;
                }
            }
            if (_currentState is ParseState.KeyValuePair)
            {
                if (ParseKeyValuePair(currentTable, keyParts, ref latestComment))
                    continue;
            }
            if (_currentState is ParseState.Table)
            {
                if (
                    ParseTable(
                        c,
                        rootTable,
                        ref currentTable,
                        keyParts,
                        ref isArrayTable,
                        ref latestComment
                    )
                    is bool tableParsed
                )
                {
                    if (tableParsed)
                        ConsumeCharacter();
                    continue;
                }
            }
            if (_currentState is ParseState.SkipToNextLine)
            {
                if (ParseSkipToNextLine(c) is bool skipPased)
                {
                    if (skipPased)
                        ConsumeCharacter();
                    continue;
                }
            }
            ConsumeCharacter();
        }

        if (_currentState != ParseState.None && _currentState != ParseState.SkipToNextLine)
            AddError("Unexpected end of file!");

        if (_syntaxErrors.Count > 0)
            throw new TomlParseException(rootTable, _syntaxErrors);

        return rootTable;
    }

    private void ConsumeCharacter()
    {
        _reader.Read();
        _column++;
    }

    /// <summary>
    /// 解析空字符
    /// </summary>
    /// <param name="c">字符</param>
    /// <param name="rootTable">原始表格</param>
    /// <param name="isFirstComment">是首个注释</param>
    /// <param name="latestComment">末尾注释</param>
    /// <returns>消耗字符并跳过为 <see langword="true"/> 只跳过为 <see langword="false"/> 不操作为 <see langword="null"/></returns>
    private bool? ParseNone(
        char c,
        TomlTable rootTable,
        ref bool isFirstComment,
        ref StringBuilder latestComment
    )
    {
        // Skip white space
        if (TomlSyntax.IsWhiteSpace(c))
            return true;

        if (TomlSyntax.IsNewLine(c))
        {
            // Check if there are any comments and so far no items being declared
            if (latestComment is not null && isFirstComment)
            {
                rootTable.Comment = latestComment.ToString().TrimEnd();
                latestComment = null!;
                isFirstComment = false;
            }

            if (TomlSyntax.IsLineBreak(c))
                AdvanceLine();

            return true;
        }

        // Start of a comment; ignore until newline
        if (c is TomlSyntax.COMMENT_SYMBOL)
        {
            latestComment ??= new StringBuilder();
            latestComment.AppendLine(ParseComment());
            AdvanceLine(1);
            return false;
        }

        // Encountered a non-comment value. The comment must belong to it (ignore possible newlines)!
        isFirstComment = false;

        if (c is TomlSyntax.TABLE_START_SYMBOL)
        {
            _currentState = ParseState.Table;
            return true;
        }

        if (TomlSyntax.IsBareKey(c) || TomlSyntax.IsQuoted(c))
        {
            _currentState = ParseState.KeyValuePair;
            return null;
        }
        else
        {
            AddError($"Unexpected character \"{c}\"");
            return false;
        }
    }

    /// <summary>
    /// 解析键值对
    /// </summary>
    /// <param name="currentTable">当前表格</param>
    /// <param name="keyParts">键列表</param>
    /// <param name="latestComment">最后注释</param>
    /// <returns>跳过为 <see langword="true"/> 不跳过为 <see langword="false"/></returns>
    private bool ParseKeyValuePair(
        TomlTable currentTable,
        List<string> keyParts,
        ref StringBuilder latestComment
    )
    {
        var keyValuePair = ReadKeyValuePair(keyParts);

        if (keyValuePair == null)
        {
            latestComment = null!;
            keyParts.Clear();

            if (_currentState != ParseState.None)
                AddError("Failed to parse key-value pair!");
            return true;
        }

        keyValuePair.Comment = latestComment?.ToString()?.TrimEnd()!;
        var inserted = InsertNode(keyValuePair, currentTable, keyParts);
        latestComment = null!;
        keyParts.Clear();
        if (inserted)
            _currentState = ParseState.SkipToNextLine;
        return true;
    }

    /// <summary>
    /// 解析表格
    /// </summary>
    /// <param name="c">字符</param>
    /// <param name="rootTable">原始表格</param>
    /// <param name="currentTable">当前表格</param>
    /// <param name="keyParts">键列表</param>
    /// <param name="isArrayTable">是数组表格</param>
    /// <param name="latestComment">末尾注释</param>
    /// <returns>消耗字符并跳过为 <see langword="true"/> 只跳过为 <see langword="false"/> 不操作为 <see langword="null"/></returns>
    private bool? ParseTable(
        char c,
        TomlTable rootTable,
        ref TomlTable currentTable,
        List<string> keyParts,
        ref bool isArrayTable,
        ref StringBuilder latestComment
    )
    {
        if (keyParts.Count is 0)
        {
            // We have array table
            if (c is TomlSyntax.TABLE_START_SYMBOL)
            {
                // Consume the character
                ConsumeChar();
                isArrayTable = true;
            }

            if (ReadKeyName(ref keyParts, TomlSyntax.TABLE_END_SYMBOL) is false)
            {
                keyParts.Clear();
                return false;
            }

            if (keyParts.Count is 0)
            {
                AddError("Table name is emtpy.");
                isArrayTable = false;
                latestComment = null!;
                keyParts.Clear();
            }

            return false;
        }

        if (c is TomlSyntax.TABLE_END_SYMBOL)
        {
            if (isArrayTable)
            {
                // Consume the ending bracket so we can peek the next character
                ConsumeChar();
                var nextChar = _reader.Peek();
                if (nextChar < 0 || (char)nextChar != TomlSyntax.TABLE_END_SYMBOL)
                {
                    AddError($"Array table {".".Join(keyParts)} has only one closing bracket.");
                    keyParts.Clear();
                    isArrayTable = false;
                    latestComment = null!;
                    return false;
                }
            }

            currentTable = CreateTable(rootTable, keyParts, isArrayTable);
            if (currentTable is not null)
            {
                currentTable.IsInline = false;
                currentTable.Comment = latestComment?.ToString()?.TrimEnd()!;
            }

            keyParts.Clear();
            isArrayTable = false;
            latestComment = null!;

            if (currentTable == null)
            {
                if (_currentState != ParseState.None)
                    AddError("Error creating table array!");
                // Reset a node to root in order to try and continue parsing
                currentTable = rootTable;
                return false;
            }

            _currentState = ParseState.SkipToNextLine;
            return true;
        }

        if (keyParts.Count is not 0)
        {
            AddError($"Unexpected character \"{c}\"");
            keyParts.Clear();
            isArrayTable = false;
            latestComment = null!;
        }
        return null;
    }

    /// <summary>
    /// 跳过下一行
    /// </summary>
    /// <param name="c">字符</param>
    /// <returns>消耗字符并跳过为 <see langword="true"/> 只跳过为 <see langword="false"/> 不操作为 <see langword="null"/></returns>
    private bool? ParseSkipToNextLine(char c)
    {
        if (TomlSyntax.IsWhiteSpace(c) || c is TomlSyntax.NEWLINE_CARRIAGE_RETURN_CHARACTER)
            return true;

        if (c is TomlSyntax.COMMENT_SYMBOL or TomlSyntax.NEWLINE_CHARACTER)
        {
            _currentState = ParseState.None;
            AdvanceLine();

            if (c is TomlSyntax.COMMENT_SYMBOL)
            {
                _column++;
                ParseComment();
                return false;
            }

            return true;
        }

        AddError($"Unexpected character \"{c}\" at the end of the line.");
        return null;
    }

    private bool AddError(string message, bool skipLine = true)
    {
        _syntaxErrors.Add(new TomlSyntaxException(message, _currentState, _line, _column));
        // Skip the whole _line in hope that it was only a single faulty value (and non-multiline one at that)
        if (skipLine)
        {
            _reader.ReadLine();
            AdvanceLine(1);
        }
        _currentState = ParseState.None;
        return false;
    }

    private void AdvanceLine(int startCol = 0)
    {
        _line++;
        _column = startCol;
    }

    private int ConsumeChar()
    {
        _column++;
        return _reader.Read();
    }

    #region Key-Value pair parsing

    /**
     * Reads a single key-value pair.
     * Assumes the cursor is at the first character that belong to the pair (including possible whitespace).
     * Consumes all characters that belong to the key and the value (ignoring possible trailing whitespace at the end).
     *
     * CreateClassExample:
     * foo = "bar"  ==> foo = "bar"
     * ^                           ^
     */

    private TomlNode ReadKeyValuePair(List<string> keyParts)
    {
        int cur;
        while ((cur = _reader.Peek()) >= 0)
        {
            var c = (char)cur;

            if (TomlSyntax.IsQuoted(c) || TomlSyntax.IsBareKey(c))
            {
                if (keyParts.Count != 0)
                {
                    AddError("Encountered extra characters in key definition!");
                    return null!;
                }

                if (ReadKeyName(ref keyParts, TomlSyntax.KEY_VALUE_SEPARATOR) is false)
                    return null!;

                continue;
            }

            if (TomlSyntax.IsWhiteSpace(c))
            {
                ConsumeChar();
                continue;
            }

            if (c is TomlSyntax.KEY_VALUE_SEPARATOR)
            {
                ConsumeChar();
                return ReadValue();
            }

            AddError($"Unexpected character \"{c}\" in key name.");
            return null!;
        }

        return null!;
    }

    /**
     * Reads a single value.
     * Assumes the cursor is at the first character that belongs to the value (including possible starting whitespace).
     * Consumes all characters belonging to the value (ignoring possible trailing whitespace at the end).
     *
     * CreateClassExample:
     * "Test"  ==> "Test"
     * ^                 ^
     */

    private TomlNode ReadValue(bool skipNewlines = false)
    {
        int cur;
        while ((cur = _reader.Peek()) >= 0)
        {
            var c = (char)cur;

            if (TomlSyntax.IsWhiteSpace(c))
            {
                ConsumeChar();
                continue;
            }

            if (c is TomlSyntax.COMMENT_SYMBOL)
            {
                AddError("No value found!");
                return null!;
            }

            if (TomlSyntax.IsNewLine(c))
            {
                if (skipNewlines)
                {
                    _reader.Read();
                    AdvanceLine(1);
                    continue;
                }

                AddError("Encountered a newline when expecting a value!");
                return null!;
            }

            if (TomlSyntax.IsQuoted(c))
            {
                var isMultiline = IsTripleQuote(c, out var excess);

                // Error occurred in triple quote parsing
                if (_currentState is ParseState.None)
                    return null!;

                var value = isMultiline
                    ? ReadQuotedValueMultiLine(c)
                    : ReadQuotedValueSingleLine(c, excess);

                if (value is null)
                    return null!;

                return new TomlString(value)
                {
                    IsMultiline = isMultiline,
                    PreferLiteral = c is TomlSyntax.LITERAL_STRING_SYMBOL
                };
            }

            return c switch
            {
                TomlSyntax.INLINE_TABLE_START_SYMBOL => ReadInlineTable(),
                TomlSyntax.ARRAY_START_SYMBOL => ReadArray(),
                var _ => ReadTomlValue()
            };
        }

        return null!;
    }

    /**
     * Reads a single key tomlFile.
     * Assumes the cursor is at the first character belonging to the key (with possible trailing whitespace if `skipWhitespace = true`).
     * Consumes all the characters until the `until` character is met (but does not consume the character itself).
     *
     * CreateClassExample 1:
     * foo.bar  ==>  foo.bar           (`skipWhitespace = false`, `until = ' '`)
     * ^                    ^
     *
     * CreateClassExample 2:
     * [ foo . bar ] ==>  [ foo . bar ]     (`skipWhitespace = true`, `until = ']'`)
     * ^                             ^
     */

    private bool ReadKeyName(ref List<string> parts, char until)
    {
        var buffer = new StringBuilder();
        var quoted = false;
        var prevWasSpace = false;
        int cur;
        while ((cur = _reader.Peek()) >= 0)
        {
            var c = (char)cur;

            // Reached the final character
            if (c == until)
                break;

            if (TomlSyntax.IsWhiteSpace(c))
            {
                prevWasSpace = true;
                ConsumeCharacter();
                continue;
            }

            if (buffer.Length == 0)
                prevWasSpace = false;

            if (c is TomlSyntax.SUBKEY_SEPARATOR)
            {
                if (buffer.Length == 0 && quoted is false)
                    return AddError($"Found an extra subkey separator in {".".Join(parts)}...");

                parts.Add(buffer.ToString());
                buffer.Length = 0;
                quoted = false;
                prevWasSpace = false;
                ConsumeCharacter();
                continue;
            }

            if (prevWasSpace)
                return AddError("Invalid spacing in key name");

            if (TomlSyntax.IsQuoted(c))
            {
                if (quoted)

                    return AddError("Expected a subkey separator but got extra data instead!");

                if (buffer.Length != 0)
                    return AddError("Encountered a quote in the middle of subkey name!");

                // Consume the quote character and read the key tomlFile
                _column++;
                buffer.Append(ReadQuotedValueSingleLine((char)_reader.Read()));
                quoted = true;
                continue;
            }

            if (TomlSyntax.IsBareKey(c))
            {
                buffer.Append(c);
                ConsumeCharacter();
                continue;
            }

            // If we see an invalid symbol, let the next parser handle it
            break;
        }

        if (buffer.Length == 0 && quoted is false)
            return AddError($"Found an extra subkey separator in {".".Join(parts)}...");

        parts.Add(buffer.ToString());

        return true;
    }

    #endregion

    #region Non-string value parsing

    /**
     * Reads the whole raw value until the first non-value character is encountered.
     * Assumes the cursor start position at the first value character and consumes all characters that may be related to the value.
     * CreateClassExample:
     *
     * 1_0_0_0  ==>  1_0_0_0
     * ^                    ^
     */

    private string ReadRawValue()
    {
        var result = new StringBuilder();
        int cur;
        while ((cur = _reader.Peek()) >= 0)
        {
            var c = (char)cur;
            if (
                c is TomlSyntax.COMMENT_SYMBOL
                || TomlSyntax.IsNewLine(c)
                || TomlSyntax.IsValueSeparator(c)
            )
                break;
            result.Append(c);
            ConsumeChar();
        }

        // Replace trim with manual space counting?
        return result.ToString().Trim();
    }

    /**
     * Reads and parses a non-string, non-composite TOML value.
     * Assumes the cursor at the first character that is related to the value (with possible spaces).
     * Consumes all the characters that are related to the value.
     *
     * CreateClassExample
     * 1_0_0_0 # This is a comment
     * <newline>
     *     ==>  1_0_0_0 # This is a comment
     *     ^                                                  ^
     * </newline>
     **/

    private TomlNode ReadTomlValue()
    {
        var value = ReadRawValue();
        TomlNode node = value switch
        {
            var v when TomlSyntax.IsBoolean(v) => bool.Parse(v),
            var v when TomlSyntax.IsNaN(v) => double.NaN,
            var v when TomlSyntax.IsPosInf(v) => double.PositiveInfinity,
            var v when TomlSyntax.IsNegInf(v) => double.NegativeInfinity,
            var v when TomlSyntax.IsInteger(v)
                => long.Parse(
                    value.RemoveAll(TomlSyntax.INT_NUMBE_SEPARATOR),
                    CultureInfo.InvariantCulture
                ),
            var v when TomlSyntax.IsFloat(v)
                => double.Parse(
                    value.RemoveAll(TomlSyntax.INT_NUMBE_SEPARATOR),
                    CultureInfo.InvariantCulture
                ),
            var v when TomlSyntax.IsIntegerWithBase(v, out var numberBase)
                => new TomlInteger(
                    Convert.ToInt64(
                        value[2..].RemoveAll(TomlSyntax.INT_NUMBE_SEPARATOR),
                        numberBase
                    )
                )
                {
                    IntegerBase = (TomlInteger.Base)numberBase
                },
            var _ => null!
        };
        if (node is not null)
            return node;

        // Normalize by removing space separator
        value = value.Replace(TomlSyntax.RFC3339EmptySeparator, TomlSyntax.ISO861Separator);
        if (
            StringUtils.TryParseDateTime<DateTime>(
                value,
                TomlSyntax.RFC3339LocalDateTimeFormats,
                DateTimeStyles.AssumeLocal,
                DateTime.TryParseExact,
                out var dateTimeResult,
                out var precision
            )
        )
            return new TomlDateTimeLocal(dateTimeResult) { SecondsPrecision = precision };

        if (
            DateTime.TryParseExact(
                value,
                TomlSyntax.LocalDateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeLocal,
                out dateTimeResult
            )
        )
            return new TomlDateTimeLocal(dateTimeResult)
            {
                Style = TomlDateTimeLocal.DateTimeStyle.Date
            };

        if (
            StringUtils.TryParseDateTime(
                value,
                TomlSyntax.RFC3339LocalTimeFormats,
                DateTimeStyles.AssumeLocal,
                DateTime.TryParseExact,
                out dateTimeResult,
                out precision
            )
        )
            return new TomlDateTimeLocal(dateTimeResult)
            {
                Style = TomlDateTimeLocal.DateTimeStyle.Time,
                SecondsPrecision = precision
            };

        if (
            StringUtils.TryParseDateTime<DateTimeOffset>(
                value,
                TomlSyntax.RFC3339Formats,
                DateTimeStyles.None,
                DateTimeOffset.TryParseExact,
                out var dateTimeOffsetResult,
                out precision
            )
        )
            return new TomlDateTimeOffset(dateTimeOffsetResult) { SecondsPrecision = precision };

        AddError($"Value \"{value}\" is not a valid TOML value!");
        return null!;
    }

    /**
     * Reads an array value.
     * Assumes the cursor is at the start of the array definition. Reads all character until the array closing bracket.
     *
     * CreateClassExample:
     * [1, 2, 3]  ==>  [1, 2, 3]
     * ^                        ^
     */

    private TomlArray ReadArray()
    {
        // Consume the start of array character
        ConsumeChar();
        var result = new TomlArray();
        TomlNode currentValue = null!;
        var expectValue = true;

        int cur;
        while ((cur = _reader.Peek()) >= 0)
        {
            var c = (char)cur;

            if (c is TomlSyntax.ARRAY_END_SYMBOL)
            {
                ConsumeChar();
                break;
            }

            if (c is TomlSyntax.COMMENT_SYMBOL)
            {
                _reader.ReadLine();
                AdvanceLine(1);
                continue;
            }

            if (TomlSyntax.IsWhiteSpace(c) || TomlSyntax.IsNewLine(c))
            {
                if (TomlSyntax.IsLineBreak(c))
                    AdvanceLine();
                ConsumeChar();
                continue;
            }

            if (c is TomlSyntax.ITEM_SEPARATOR)
            {
                if (currentValue == null)
                {
                    AddError("Encountered multiple value separators");
                    return null!;
                }

                result.Add(currentValue);
                currentValue = null!;
                expectValue = true;
                ConsumeChar();
                continue;
            }

            if (expectValue is false)
            {
                AddError("Missing separator between values");
                return null!;
            }
            currentValue = ReadValue(true);
            if (currentValue == null)
            {
                if (_currentState != ParseState.None)
                    AddError("Failed to determine and parse a value!");
                return null!;
            }
            expectValue = false;
        }

        if (currentValue is not null)
            result.Add(currentValue);
        return result;
    }

    /**
     * Reads an inline table.
     * Assumes the cursor is at the start of the table definition. Reads all character until the table closing bracket.
     *
     * CreateClassExample:
     * { Test = "foo", value = 1 }  ==>  { Test = "foo", value = 1 }
     * ^                                                            ^
     */

    private TomlNode ReadInlineTable()
    {
        ConsumeChar();
        var result = new TomlTable { IsInline = true };
        TomlNode currentValue = null!;
        var separator = false;
        var keyParts = new List<string>();
        int cur;
        while ((cur = _reader.Peek()) >= 0)
        {
            var c = (char)cur;

            if (c is TomlSyntax.INLINE_TABLE_END_SYMBOL)
            {
                ConsumeChar();
                break;
            }

            if (c is TomlSyntax.COMMENT_SYMBOL)
            {
                AddError("Incomplete inline table definition!");
                return null!;
            }

            if (TomlSyntax.IsNewLine(c))
            {
                AddError("Inline tables are only allowed to be on single line");
                return null!;
            }

            if (TomlSyntax.IsWhiteSpace(c))
            {
                ConsumeChar();
                continue;
            }

            if (c is TomlSyntax.ITEM_SEPARATOR)
            {
                if (currentValue == null)
                {
                    AddError("Encountered multiple value separators in inline table!");
                    return null!;
                }

                if (InsertNode(currentValue, result, keyParts) is false)
                    return null!;
                keyParts.Clear();
                currentValue = null!;
                separator = true;
                ConsumeChar();
                continue;
            }

            separator = false;
            currentValue = ReadKeyValuePair(keyParts);
        }

        if (separator)
        {
            AddError("Trailing commas are not allowed in inline tables.");
            return null!;
        }

        if (currentValue is not null && !InsertNode(currentValue, result, keyParts))
            return null!;

        return result;
    }

    #endregion

    #region String parsing

    /**
     * Checks if the string value a multiline string (i.ex. a triple quoted string).
     * Assumes the cursor is at the first quote character. Consumes the least amount of characters needed to determine if the string is multiline.
     *
     * If the result is false, returns the consumed character through the `excess` variable.
     *
     * CreateClassExample 1:
     * """Test"""  ==>  """Test"""
     * ^                   ^
     *
     * CreateClassExample 2:
     * "Test"  ==>  "Test"         (doesn't return the first quote)
     * ^             ^
     *
     * CreateClassExample 3:
     * ""  ==>  ""        (returns the extra `"` through the `excess` variable)
     * ^          ^
     */

    private bool IsTripleQuote(char quote, out char excess)
    {
        // Copypasta, but it's faster...

        int cur;
        // Consume the first quote
        ConsumeChar();
        if ((cur = _reader.Peek()) < 0)
        {
            excess = '\0';
            return AddError("Unexpected end of file!");
        }

        if ((char)cur != quote)
        {
            excess = '\0';
            return false;
        }

        // Consume the second quote
        excess = (char)ConsumeChar();
        if ((cur = _reader.Peek()) < 0 || (char)cur != quote)
            return false;

        // Consume the final quote
        ConsumeChar();
        excess = '\0';
        return true;
    }

    /**
     * A convenience method to process a single character within a quote.
     */

    private bool ProcessQuotedValueCharacter(
        char quote,
        bool isNonLiteral,
        char c,
        StringBuilder sb,
        ref bool escaped
    )
    {
        if (TomlSyntax.MustBeEscaped(c))
            return AddError($"The character U+{(int)c:X8} must be escaped in a string!");

        if (escaped)
        {
            sb.Append(c);
            escaped = false;
            return false;
        }

        if (c == quote)
            return true;
        if (isNonLiteral && c is TomlSyntax.ESCAPE_SYMBOL)
            escaped = true;
        if (c is TomlSyntax.NEWLINE_CHARACTER)
            return AddError("Encountered newline in single line string!");

        sb.Append(c);
        return false;
    }

    /**
     * Reads a single-_line string.
     * Assumes the cursor is at the first character that belongs to the string.
     * Consumes all characters that belong to the string (including the closing quote).
     *
     * CreateClassExample:
     * "Test"  ==>  "Test"
     * ^                 ^
     */

    private string ReadQuotedValueSingleLine(char quote, char initialData = '\0')
    {
        var isNonLiteral = quote is TomlSyntax.BASI_STRING_SYMBOL;
        var sb = new StringBuilder();
        var escaped = false;

        if (initialData != '\0')
        {
            var shouldReturn = ProcessQuotedValueCharacter(
                quote,
                isNonLiteral,
                initialData,
                sb,
                ref escaped
            );
            if (_currentState is ParseState.None)
                return null!;
            if (shouldReturn)
                if (isNonLiteral)
                {
                    if (sb.ToString().TryUnescape(out var res, out var ex))
                        return res;
                    AddError(ex.Message);
                    return null!;
                }
                else
                    return sb.ToString();
        }

        int cur;
        var readDone = false;
        while ((cur = _reader.Read()) >= 0)
        {
            // Consume the character
            _column++;
            var c = (char)cur;
            readDone = ProcessQuotedValueCharacter(quote, isNonLiteral, c, sb, ref escaped);
            if (readDone)
            {
                if (_currentState is ParseState.None)
                    return null!;
                break;
            }
        }

        if (readDone is false)
        {
            AddError("Unclosed string.");
            return null!;
        }

        if (isNonLiteral is false)
            return sb.ToString();
        if (sb.ToString().TryUnescape(out var unescaped, out var unescapedEx))
            return unescaped;
        AddError(unescapedEx.Message);
        return null!;
    }

    /**
     * Reads a multiline string.
     * Assumes the cursor is at the first character that belongs to the string.
     * Consumes all characters that belong to the string and the three closing quotes.
     *
     * CreateClassExample:
     * """Test"""  ==>  """Test"""
     * ^                       ^
     */

    private string ReadQuotedValueMultiLine(char quote)
    {
        var isBasic = quote is TomlSyntax.BASI_STRING_SYMBOL;
        var sb = new StringBuilder();
        var escaped = false;
        var skipWhitespace = false;
        var skipWhitespaceLineSkipped = false;
        var quotesEncountered = 0;
        var first = true;
        int cur;
        while ((cur = ConsumeChar()) >= 0)
        {
            var c = (char)cur;
            if (TomlSyntax.MustBeEscaped(c, true))
            {
                AddError($"The character U+{(int)c:X8} must be escaped!");
                return null!;
            }
            // Trim the first newline
            if (first && TomlSyntax.IsNewLine(c))
            {
                if (TomlSyntax.IsLineBreak(c))
                    first = false;
                else
                    AdvanceLine();
                continue;
            }

            first = false;
            //?TODO: Reuse ProcessQuotedValueCharacter
            // Skip the current character if it is going to be escaped later
            if (escaped)
            {
                sb.Append(c);
                escaped = false;
                continue;
            }

            // If we are currently skipping empty spaces, skip
            if (skipWhitespace)
            {
                if (TomlSyntax.IsEmptySpace(c))
                {
                    if (TomlSyntax.IsLineBreak(c))
                    {
                        skipWhitespaceLineSkipped = true;
                        AdvanceLine();
                    }
                    continue;
                }

                if (skipWhitespaceLineSkipped is false)
                {
                    AddError("Non-whitespace character after trim marker.");
                    return null!;
                }

                skipWhitespaceLineSkipped = false;
                skipWhitespace = false;
            }

            // If we encounter an escape sequence...
            if (isBasic && c is TomlSyntax.ESCAPE_SYMBOL)
            {
                var next = _reader.Peek();
                var nc = (char)next;
                if (next >= 0)
                {
                    // ...and the next char is empty space, we must skip all whitespaces
                    if (TomlSyntax.IsEmptySpace(nc))
                    {
                        skipWhitespace = true;
                        continue;
                    }

                    // ...and we have \" or \, skip the character
                    if (nc == quote || nc is TomlSyntax.ESCAPE_SYMBOL)
                        escaped = true;
                }
            }

            // Count the consecutive quotes
            if (c == quote)
                quotesEncountered++;
            else
                quotesEncountered = 0;

            // If the are three quotes, count them as closing quotes
            if (quotesEncountered == 3)
                break;

            sb.Append(c);
        }

        // TOML actually allows to have five ending quotes like
        // """"" => "" belong to the string + """ is the actual ending
        quotesEncountered = 0;
        while ((cur = _reader.Peek()) >= 0)
        {
            var c = (char)cur;
            if (c == quote && ++quotesEncountered < 3)
            {
                sb.Append(c);
                ConsumeChar();
            }
            else
                break;
        }

        // Remove last two quotes (third one wasn't included by default)
        sb.Length -= 2;
        if (isBasic is false)
            return sb.ToString();
        if (sb.ToString().TryUnescape(out var res, out var ex))
            return res;
        AddError(ex.Message);
        return null!;
    }

    #endregion

    #region Node creation

    private bool InsertNode(TomlNode node, TomlNode root, IList<string> path)
    {
        var latestNode = root;
        if (path.Count > 1)
            for (var index = 0; index < path.Count - 1; index++)
            {
                var subkey = path[index];
                if (latestNode.TryGetNode(subkey, out var currentNode))
                {
                    if (currentNode.HasValue)
                        return AddError(
                            $"The key {".".Join(path)} already has a value assigned to it!"
                        );
                }
                else
                {
                    currentNode = new TomlTable();
                    latestNode[subkey] = currentNode;
                }

                latestNode = currentNode;
                if (latestNode is TomlTable { IsInline: true })
                    return AddError(
                        $"Cannot assign {".".Join(path)} because it will edit an immutable table."
                    );
            }

        if (latestNode.HasKey(path[path.Count - 1]))
            return AddError($"The key {".".Join(path)} is already defined!");
        latestNode[path[path.Count - 1]] = node;
        node.CollapseLevel = path.Count - 1;
        return true;
    }

    private TomlTable CreateTable(TomlNode root, IList<string> path, bool arrayTable)
    {
        if (path.Count is 0)
            return null!;
        var latestNode = root;
        for (var index = 0; index < path.Count; index++)
        {
            var subkey = path[index];

            if (latestNode.TryGetNode(subkey, out var node))
            {
                if (node.IsTomlArray && arrayTable)
                {
                    var arr = (TomlArray)node;

                    if (arr.IsTableArray is false)
                    {
                        AddError(
                            $"The array {".".Join(path)} cannot be redefined as an array table!"
                        );
                        return null!;
                    }

                    if (index == path.Count - 1)
                    {
                        latestNode = new TomlTable();
                        arr.Add(latestNode);
                        break;
                    }

                    latestNode = arr[arr.ChildrenCount - 1];
                    continue;
                }

                if (node is TomlTable { IsInline: true })
                {
                    AddError(
                        $"Cannot create table {".".Join(path)} because it will edit an immutable table."
                    );
                    return null!;
                }

                if (node.HasValue)
                {
                    if (node is not TomlArray { IsTableArray: true } array)
                    {
                        AddError($"The key {".".Join(path)} has a value assigned to it!");
                        return null!;
                    }

                    latestNode = array[array.ChildrenCount - 1];
                    continue;
                }

                if (index == path.Count - 1)
                {
                    if (arrayTable && !node.IsTomlArray)
                    {
                        AddError(
                            $"The table {".".Join(path)} cannot be redefined as an array table!"
                        );
                        return null!;
                    }

                    if (node is TomlTable { isImplicit: false })
                    {
                        AddError($"The table {".".Join(path)} is defined multiple times!");
                        return null!;
                    }
                }
            }
            else
            {
                if (index == path.Count - 1 && arrayTable)
                {
                    var table = new TomlTable();
                    var arr = new TomlArray { IsTableArray = true };
                    arr.Add(table);
                    latestNode[subkey] = arr;
                    latestNode = table;
                    break;
                }

                node = new TomlTable { isImplicit = true };
                latestNode[subkey] = node;
            }

            latestNode = node;
        }

        var result = (TomlTable)latestNode;
        result.isImplicit = false;
        return result;
    }

    #endregion

    #region Misc parsing

    private string ParseComment()
    {
        ConsumeChar();
        var commentLine = _reader.ReadLine()?.Trim() ?? string.Empty;
        if (commentLine.Any(ch => TomlSyntax.MustBeEscaped(ch)))
            AddError("Comment must not contain control characters other than tab.", false);
        return commentLine;
    }

    #endregion
}
