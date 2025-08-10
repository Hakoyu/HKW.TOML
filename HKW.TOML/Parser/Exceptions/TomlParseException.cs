#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion

namespace HKW.HKWTOML;

/// <summary>
/// Toml解析错误
/// </summary>
/// <param name="parsed">解析过的表格</param>
/// <param name="exceptions">语法错误</param>
public class TomlParseException(TomlTable parsed, List<TomlSyntaxException> exceptions)
    : Exception("Toml parse error")
{
    /// <summary>
    /// 解析过的表格
    /// </summary>
    public TomlTable ParsedTable { get; } = parsed;

    /// <summary>
    /// 语法错误
    /// </summary>
    public List<TomlSyntaxException> SyntaxErrors { get; } = exceptions;
}
