#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion

namespace HKW.HKWTOML;

/// <summary>
/// Toml解析错误
/// </summary>
public class TomlParseException : Exception
{
    /// <summary>
    /// 解析过的表格
    /// </summary>
    public TomlTable ParsedTable { get; }

    /// <summary>
    /// 语法错误
    /// </summary>
    public IEnumerable<TomlSyntaxException> SyntaxErrors { get; }

    /// <summary>
    /// Toml解析错误
    /// </summary>
    /// <param name="parsed">解析过的表格</param>
    /// <param name="exceptions">语法错误</param>
    public TomlParseException(TomlTable parsed, IEnumerable<TomlSyntaxException> exceptions)
        : base("HKWTOML file contains format errors")
    {
        ParsedTable = parsed;
        SyntaxErrors = exceptions;
    }
}
