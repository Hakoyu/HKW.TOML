#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion


namespace HKW.HKWTOML;

/// <summary>
/// Toml语法错误
/// </summary>
public class TomlSyntaxException : Exception
{
    /// <summary>
    /// 解析状态
    /// </summary>
    public TOMLParser.ParseState ParseState { get; }

    /// <summary>
    /// 行
    /// </summary>
    public int Line { get; }

    /// <summary>
    /// 列
    /// </summary>
    public int Column { get; }

    /// <summary>
    /// Toml语法错误
    /// </summary>
    /// <param name="message">信息</param>
    /// <param name="state">解析状态</param>
    /// <param name="line">行</param>
    /// <param name="column">列</param>
    public TomlSyntaxException(string message, TOMLParser.ParseState state, int line, int column)
        : base(message)
    {
        ParseState = state;
        Line = line;
        Column = column;
    }
}
