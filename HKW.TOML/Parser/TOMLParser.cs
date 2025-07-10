#region TOML Official Site

// https://toml.io
// 原始项目
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
    /// 文本读取器
    /// </summary>
    private readonly TextReader _reader;

    /// <summary>
    /// 空字符
    /// </summary>
    private const char NULL_CHAR = '\0';

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
    /// <param name="reader">文本读取器</param>
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
        _syntaxErrors = [];
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
                        ConsumeChar();
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
                        ConsumeChar();
                    continue;
                }
            }
            if (_currentState is ParseState.SkipToNextLine)
            {
                if (ParseSkipToNextLine(c) is bool skipPased)
                {
                    if (skipPased)
                        ConsumeChar();
                    continue;
                }
            }
            ConsumeChar();
        }

        if (_currentState != ParseState.None && _currentState != ParseState.SkipToNextLine)
            AddError("意外的文件结尾！");

        if (_syntaxErrors.Count > 0)
            throw new TomlParseException(rootTable, _syntaxErrors);

        return rootTable;
    }

    /// <summary>
    /// 解析空状态
    /// </summary>
    /// <param name="c">字符</param>
    /// <param name="rootTable">根表格</param>
    /// <param name="isFirstComment">是首个注释</param>
    /// <param name="latestComment">最新注释</param>
    /// <returns>消耗字符并跳过为 <see langword="true"/> 只跳过为 <see langword="false"/> 不操作为 <see langword="null"/></returns>
    private bool? ParseNone(
        char c,
        TomlTable rootTable,
        ref bool isFirstComment,
        ref StringBuilder latestComment
    )
    {
        // 跳过空白字符
        if (TomlSyntax.IsWhiteSpace(c))
            return true;

        if (TomlSyntax.IsNewLine(c))
        {
            // 检查是否有注释，并且到目前为止没有声明任何项目
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

        // 注释开始；忽略直到换行符
        if (c is TomlSyntax.COMMENT_SYMBOL)
        {
            latestComment ??= new StringBuilder();
            latestComment.AppendLine(ParseComment());
            AdvanceLine(1);
            return false;
        }

        // 遇到非注释值。注释必须属于它（忽略可能的换行符）！
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
            AddError($"意外的字符 \"{c}\"");
            return false;
        }
    }

    /// <summary>
    /// 解析键值对
    /// </summary>
    /// <param name="currentTable">当前表格</param>
    /// <param name="keyParts">键部分列表</param>
    /// <param name="latestComment">最新注释</param>
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
                AddError("解析键值对失败！");
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
    /// <param name="rootTable">根表格</param>
    /// <param name="currentTable">当前表格</param>
    /// <param name="keyParts">键部分列表</param>
    /// <param name="isArrayTable">是数组表格</param>
    /// <param name="latestComment">最新注释</param>
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
            // 我们有数组表格
            if (c is TomlSyntax.TABLE_START_SYMBOL)
            {
                // 消耗字符
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
                AddError("表格名称为空。");
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
                // 消耗结束括号，以便我们可以查看下一个字符
                ConsumeChar();
                var nextChar = _reader.Peek();
                if (nextChar < 0 || (char)nextChar != TomlSyntax.TABLE_END_SYMBOL)
                {
                    AddError($"数组表格 {".".Join(keyParts)} 只有一个结束括号。");
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
                    AddError("创建表格数组时出错！");
                // 将节点重置为根节点，以便尝试继续解析
                currentTable = rootTable;
                return false;
            }

            _currentState = ParseState.SkipToNextLine;
            return true;
        }

        if (keyParts.Count is not 0)
        {
            AddError($"意外的字符 \"{c}\"");
            keyParts.Clear();
            isArrayTable = false;
            latestComment = null!;
        }
        return null;
    }

    /// <summary>
    /// 解析跳过到下一行
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

        AddError($"在行尾遇到意外字符 \"{c}\"。");
        return null;
    }

    private bool AddError(string message, bool skipLine = true)
    {
        _syntaxErrors.Add(new TomlSyntaxException(message, _currentState, _line, _column));
        // 跳过整行，希望这只是一个单一的错误值（而且不是多行的）
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

    #region 键值对解析

    /// <summary>
    /// 读取单个键值对。
    /// 假设光标位于属于该对的第一个字符（包括可能的空白字符）。
    /// 消耗属于键和值的所有字符（忽略末尾可能的尾随空白字符）。
    ///
    /// 示例：
    /// foo = "bar"  ==> foo = "bar"
    /// ^                           ^
    /// </summary>
    private TomlNode? ReadKeyValuePair(List<string> keyParts)
    {
        int cur;
        while ((cur = _reader.Peek()) >= 0)
        {
            var c = (char)cur;

            if (TomlSyntax.IsQuoted(c) || TomlSyntax.IsBareKey(c))
            {
                if (keyParts.Count != 0)
                {
                    AddError("在键定义中遇到额外字符！");
                    return null;
                }

                if (ReadKeyName(ref keyParts, TomlSyntax.KEY_VALUE_SEPARATOR) is false)
                    return null;

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

            AddError($"在键名中遇到意外字符 \"{c}\"。");
            return null;
        }

        return null;
    }

    /// <summary>
    /// 读取单个值。
    /// 假设光标位于属于值的第一个字符（包括可能的起始空白字符）。
    /// 消耗属于值的所有字符（忽略末尾可能的尾随空白字符）。
    ///
    /// 示例：
    /// "Test"  ==> "Test"
    /// ^                 ^
    /// </summary>
    private TomlNode? ReadValue(bool skipNewlines = false)
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
                AddError("未找到值！");
                return null;
            }

            if (TomlSyntax.IsNewLine(c))
            {
                if (skipNewlines)
                {
                    _reader.Read();
                    AdvanceLine(1);
                    continue;
                }

                AddError("期望值时遇到换行符！");
                return null;
            }

            if (TomlSyntax.IsQuoted(c))
            {
                var isMultiline = IsTripleQuote(c, out var excess);

                // 三引号解析时发生错误
                if (_currentState is ParseState.None)
                    return null;

                var value = isMultiline
                    ? ReadQuotedValueMultiLine(c)
                    : ReadQuotedValueSingleLine(c, excess);

                if (value is null)
                    return null;

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

    /// <summary>
    /// 读取单个键名。
    /// 假设光标位于属于键的第一个字符（如果 `skipWhitespace = true`，则可能有尾随空白字符）。
    /// 消耗所有字符，直到遇到 `until` 字符为止（但不消耗该字符本身）。
    ///
    /// 示例 1：
    /// foo.bar  ==>  foo.bar           (`skipWhitespace = false`, `until = ' '`)
    /// ^                    ^
    ///
    /// 示例 2：
    /// [ foo . bar ] ==>  [ foo . bar ]     (`skipWhitespace = true`, `until = ']'`)
    /// ^                             ^
    /// </summary>
    private bool ReadKeyName(ref List<string> parts, char until)
    {
        var buffer = new StringBuilder();
        var quoted = false;
        var prevWasSpace = false;
        int cur;
        while ((cur = _reader.Peek()) >= 0)
        {
            var c = (char)cur;

            // 到达最终字符
            if (c == until)
                break;

            if (TomlSyntax.IsWhiteSpace(c))
            {
                prevWasSpace = true;
                ConsumeChar();
                continue;
            }

            if (buffer.Length == 0)
                prevWasSpace = false;

            if (c is TomlSyntax.SUBKEY_SEPARATOR)
            {
                if (buffer.Length == 0 && quoted is false)
                    return AddError($"在 {".".Join(parts)}... 中发现额外的子键分隔符");

                parts.Add(buffer.ToString());
                buffer.Length = 0;
                quoted = false;
                prevWasSpace = false;
                ConsumeChar();
                continue;
            }

            if (prevWasSpace)
                return AddError("键名中的空格无效");

            if (TomlSyntax.IsQuoted(c))
            {
                if (quoted)
                    return AddError("期望子键分隔符，但得到了额外数据！");

                if (buffer.Length != 0)
                    return AddError("在子键名中间遇到引号！");

                // 消耗引号字符并读取键名
                _column++;
                buffer.Append(ReadQuotedValueSingleLine((char)_reader.Read()));
                quoted = true;
                continue;
            }

            if (TomlSyntax.IsBareKey(c))
            {
                buffer.Append(c);
                ConsumeChar();
                continue;
            }

            // 如果我们看到无效符号，让下一个解析器处理它
            break;
        }

        if (buffer.Length == 0 && quoted is false)
            return AddError($"在 {".".Join(parts)}... 中发现额外的子键分隔符");

        parts.Add(buffer.ToString());

        return true;
    }

    #endregion

    #region 非字符串值解析

    /// <summary>
    /// 读取整个原始值，直到遇到第一个非值字符。
    /// 假设光标起始位置在第一个值字符处，并消耗所有可能与值相关的字符。
    ///
    /// 示例：
    /// 1_0_0_0  ==>  1_0_0_0
    /// ^                    ^
    /// </summary>
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

        // 用手动空格计数替换 trim？
        return result.ToString().Trim();
    }

    /// <summary>
    /// 读取并解析非字符串、非复合的 TOML 值。
    /// 假设光标位于与值相关的第一个字符（可能有空格）。
    /// 消耗与值相关的所有字符。
    ///
    /// 示例：
    /// 1_0_0_0 # 这是一个注释
    /// &lt;newline&gt;
    ///     ==>  1_0_0_0 # 这是一个注释
    ///     ^                                                  ^
    /// &lt;/newline&gt;
    /// </summary>
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

        // 通过移除空格分隔符进行规范化
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

        AddError($"值 \"{value}\" 不是有效的 TOML 值！");
        return null!;
    }

    /// <summary>
    /// 读取数组值。
    /// 假设光标位于数组定义的开始。读取所有字符直到数组结束括号。
    ///
    /// 示例：
    /// [1, 2, 3]  ==>  [1, 2, 3]
    /// ^                        ^
    /// </summary>
    private TomlArray? ReadArray()
    {
        // 消耗数组开始字符
        ConsumeChar();
        var result = new TomlArray();
        TomlNode? currentValue = null;
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
                    AddError("遇到多个值分隔符");
                    return null;
                }

                result.Add(currentValue);
                currentValue = null;
                expectValue = true;
                ConsumeChar();
                continue;
            }

            if (expectValue is false)
            {
                AddError("值之间缺少分隔符");
                return null;
            }
            currentValue = ReadValue(true);
            if (currentValue == null)
            {
                if (_currentState != ParseState.None)
                    AddError("无法确定和解析值！");
                return null;
            }
            expectValue = false;
        }

        if (currentValue is not null)
            result.Add(currentValue);
        return result;
    }

    /// <summary>
    /// 读取内联表格。
    /// 假设光标位于表格定义的开始。读取所有字符直到表格结束括号。
    ///
    /// 示例：
    /// { Test = "foo", value = 1 }  ==>  { Test = "foo", value = 1 }
    /// ^                                                            ^
    /// </summary>
    private TomlTable? ReadInlineTable()
    {
        ConsumeChar();
        var result = new TomlTable { IsInline = true };
        TomlNode? currentValue = null;
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
                AddError("内联表格定义不完整！");
                return null;
            }

            if (TomlSyntax.IsNewLine(c))
            {
                AddError("内联表格只能在单行上");
                return null;
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
                    AddError("在内联表格中遇到多个值分隔符！");
                    return null;
                }

                if (InsertNode(currentValue, result, keyParts) is false)
                    return null;
                keyParts.Clear();
                currentValue = null;
                separator = true;
                ConsumeChar();
                continue;
            }

            separator = false;
            currentValue = ReadKeyValuePair(keyParts);
        }

        if (separator)
        {
            AddError("内联表格中不允许尾随逗号。");
            return null;
        }

        if (currentValue is not null && !InsertNode(currentValue, result, keyParts))
            return null;

        return result;
    }

    #endregion

    #region 字符串解析

    /// <summary>
    /// 检查字符串值是否为多行字符串（即三引号字符串）。
    /// 假设光标位于第一个引号字符。消耗确定字符串是否为多行所需的最少字符数。
    ///
    /// 如果结果为 false，则通过 `excess` 变量返回消耗的字符。
    ///
    /// 示例 1：
    /// """Test"""  ==>  """Test"""
    /// ^                   ^
    ///
    /// 示例 2：
    /// "Test"  ==>  "Test"         （不返回第一个引号）
    /// ^             ^
    ///
    /// 示例 3：
    /// ""  ==>  ""        （通过 `excess` 变量返回额外的 `"`）
    /// ^          ^
    /// </summary>
    private bool IsTripleQuote(char quote, out char excess)
    {
        // 复制粘贴，但更快...

        int cur;
        // 消耗第一个引号
        ConsumeChar();
        if ((cur = _reader.Peek()) < 0)
        {
            excess = NULL_CHAR;
            return AddError("意外的文件结尾！");
        }

        if ((char)cur != quote)
        {
            excess = NULL_CHAR;
            return false;
        }

        // 消耗第二个引号
        excess = (char)ConsumeChar();
        if ((cur = _reader.Peek()) < 0 || (char)cur != quote)
            return false;

        // 消耗最后一个引号
        ConsumeChar();
        excess = NULL_CHAR;
        return true;
    }

    /// <summary>
    /// 处理引号内单个字符的便利方法。
    /// </summary>
    private bool ProcessQuotedValueCharacter(
        char quote,
        bool isNonLiteral,
        char c,
        StringBuilder sb,
        ref bool escaped
    )
    {
        if (TomlSyntax.MustBeEscaped(c))
            return AddError($"字符 U+{(int)c:X8} 必须在字符串中进行转义！");

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
            return AddError("在单行字符串中遇到换行符！");

        sb.Append(c);
        return false;
    }

    /// <summary>
    /// 读取单行字符串。
    /// 假设光标位于属于字符串的第一个字符。
    /// 消耗属于字符串的所有字符（包括结束引号）。
    ///
    /// 示例：
    /// "Test"  ==>  "Test"
    /// ^                 ^
    /// </summary>
    private string ReadQuotedValueSingleLine(char quote, char initialData = NULL_CHAR)
    {
        var isNonLiteral = quote is TomlSyntax.BASI_STRING_SYMBOL;
        var sb = new StringBuilder();
        var escaped = false;

        if (initialData != NULL_CHAR)
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
            // 消耗字符
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
            AddError("未闭合的字符串。");
            return null!;
        }

        if (isNonLiteral is false)
            return sb.ToString();
        if (sb.ToString().TryUnescape(out var unescaped, out var unescapedEx))
            return unescaped;
        AddError(unescapedEx.Message);
        return null!;
    }

    /// <summary>
    /// 读取多行字符串。
    /// 假设光标位于属于字符串的第一个字符。
    /// 消耗属于字符串的所有字符和三个结束引号。
    ///
    /// 示例：
    /// """Test"""  ==>  """Test"""
    /// ^                       ^
    /// </summary>
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
                AddError($"字符 U+{(int)c:X8} 必须进行转义！");
                return null!;
            }
            // 修剪第一个换行符
            if (first && TomlSyntax.IsNewLine(c))
            {
                if (TomlSyntax.IsLineBreak(c))
                    first = false;
                else
                    AdvanceLine();
                continue;
            }

            first = false;
            //TODO: 重用 ProcessQuotedValueCharacter
            // 如果当前字符稍后会被转义，则跳过它
            if (escaped)
            {
                sb.Append(c);
                escaped = false;
                continue;
            }

            // 如果我们当前正在跳过空格，跳过
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
                    AddError("修剪标记后的非空白字符。");
                    return null!;
                }

                skipWhitespaceLineSkipped = false;
                skipWhitespace = false;
            }

            // 如果我们遇到转义序列...
            if (isBasic && c is TomlSyntax.ESCAPE_SYMBOL)
            {
                var next = _reader.Peek();
                var nc = (char)next;
                if (next >= 0)
                {
                    // ...而下一个字符是空格，我们必须跳过所有空白字符
                    if (TomlSyntax.IsEmptySpace(nc))
                    {
                        skipWhitespace = true;
                        continue;
                    }

                    // ...我们有 \" 或 \，跳过字符
                    if (nc == quote || nc is TomlSyntax.ESCAPE_SYMBOL)
                        escaped = true;
                }
            }

            // 计算连续的引号
            if (c == quote)
                quotesEncountered++;
            else
                quotesEncountered = 0;

            // 如果有三个引号，将它们算作结束引号
            if (quotesEncountered == 3)
                break;

            sb.Append(c);
        }

        // TOML 实际上允许有五个结束引号，如
        // """"" => "" 属于字符串 + """ 是实际的结尾
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

        // 移除最后两个引号（第三个默认不包括）
        sb.Length -= 2;
        if (isBasic is false)
            return sb.ToString();
        if (sb.ToString().TryUnescape(out var res, out var ex))
            return res;
        AddError(ex.Message);
        return null!;
    }

    #endregion

    #region 节点创建

    private bool InsertNode(TomlNode node, TomlNode root, List<string> path)
    {
        var latestNode = root;
        if (path.Count > 1)
            for (var index = 0; index < path.Count - 1; index++)
            {
                var subkey = path[index];
                if (latestNode.TryGetNode(subkey, out var currentNode))
                {
                    if (currentNode.HasValue)
                        return AddError($"键 {".".Join(path)} 已经有分配给它的值！");
                }
                else
                {
                    currentNode = new TomlTable();
                    latestNode[subkey] = currentNode;
                }

                latestNode = currentNode;
                if (latestNode is TomlTable { IsInline: true })
                    return AddError($"无法分配 {".".Join(path)}，因为它将编辑不可变表格。");
            }

        if (latestNode.HasKey(path[^1]))
            return AddError($"键 {".".Join(path)} 已经定义！");
        latestNode[path[^1]] = node;
        node.CollapseLevel = path.Count - 1;
        return true;
    }

    private TomlTable CreateTable(TomlNode root, List<string> path, bool arrayTable)
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
                        AddError($"数组 {".".Join(path)} 无法重新定义为数组表格！");
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
                    AddError($"无法创建表格 {".".Join(path)}，因为它将编辑不可变表格。");
                    return null!;
                }

                if (node.HasValue)
                {
                    if (node is not TomlArray { IsTableArray: true } array)
                    {
                        AddError($"键 {".".Join(path)} 有分配给它的值！");
                        return null!;
                    }

                    latestNode = array[array.ChildrenCount - 1];
                    continue;
                }

                if (index == path.Count - 1)
                {
                    if (arrayTable && !node.IsTomlArray)
                    {
                        AddError($"表格 {".".Join(path)} 无法重新定义为数组表格！");
                        return null!;
                    }

                    if (node is TomlTable { isImplicit: false })
                    {
                        AddError($"表格 {".".Join(path)} 被多次定义！");
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

    #region 其他解析

    private string ParseComment()
    {
        ConsumeChar();
        var commentLine = _reader.ReadLine()?.Trim() ?? string.Empty;
        if (commentLine.Any(ch => TomlSyntax.MustBeEscaped(ch)))
            AddError("注释不能包含除制表符以外的控制字符。", false);
        return commentLine;
    }

    #endregion
}
