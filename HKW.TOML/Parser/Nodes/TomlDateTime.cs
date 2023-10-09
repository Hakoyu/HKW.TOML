#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion


namespace HKW.HKWTOML;

/// <summary>
/// Toml日期时间
/// </summary>
public class TomlDateTime : TomlNode, IFormattable
{
    /// <summary>
    /// 秒级精度
    /// </summary>
    public int SecondsPrecision { get; set; }

    /// <inheritdoc/>
    public override bool HasValue { get; } = true;

    /// <inheritdoc/>
    public virtual string ToString(string? format, IFormatProvider? formatProvider) => string.Empty;

    /// <inheritdoc/>
    public virtual string ToString(IFormatProvider formatProvider) => string.Empty;

    /// <summary>
    /// 转换为行内内部Toml格式字符串
    /// </summary>
    /// <returns>Toml格式字符串</returns>
    protected virtual string ToInlineTomlInternal() => string.Empty;

    /// <inheritdoc/>
    public override string ToInlineToml() =>
        ToInlineTomlInternal()
            .Replace(TomlSyntax.RFC3339EmptySeparator, TomlSyntax.ISO861Separator)
            .Replace(TomlSyntax.ISO861ZeroZone, TomlSyntax.RFC3339ZeroZone);
}
