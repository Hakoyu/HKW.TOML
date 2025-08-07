#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion

using System.Diagnostics;

namespace HKW.HKWTOML;

#region TOML Nodes

/// <summary>
/// Toml字符串
/// </summary>
[DebuggerDisplay("{Value}")]
public class TomlString : TomlNode
{
    /// <inheritdoc/>
    public override bool HasValue { get; } = true;

    /// <inheritdoc/>
    public override bool IsTomlString { get; } = true;

    /// <summary>
    /// 是多行
    /// </summary>
    public bool IsMultiline { get; set; }

    /// <summary>
    /// 是多行首行
    /// </summary>
    public bool MultilineTrimFirstLine { get; set; }

    /// <summary>
    /// 首选文字
    /// </summary>
    public bool PreferLiteral { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; }

    /// <inheritdoc/>
    public override string ToString() => Value;

    /// <inheritdoc/>
    public override string ToInlineToml()
    {
        // Automatically convert literal to non-literal if there are too many literal string symbols
        if (
            Value.Contains(new string(TomlSyntax.LITERAL_STRING_SYMBOL, IsMultiline ? 3 : 1))
            && PreferLiteral
        )
            PreferLiteral = false;
        var quotes = new string(
            PreferLiteral ? TomlSyntax.LITERAL_STRING_SYMBOL : TomlSyntax.BASI_STRING_SYMBOL,
            IsMultiline ? 3 : 1
        );
        var result = PreferLiteral ? Value : Value.Escape(IsMultiline is false);
        if (IsMultiline)
        {
            // 不转换, 保留原始换行符
            //var temp = result.Replace("\r\n", "\n");
            //var lineCount = temp.Count(c => c is '\n');
            //result = temp.Replace("\n", Environment.NewLine);

            // 自适应多行模式
            var lineCount = result.Count(c => c is '\n' || c is '\r');
            if (MultilineTrimFirstLine || lineCount > 0)
                result = $"{Environment.NewLine}{result}";
        }
        return $"{quotes}{result}{quotes}";
    }

    /// <inheritdoc/>
    public TomlString(string value)
    {
        Value = value;
    }
}

#endregion
