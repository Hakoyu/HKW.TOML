#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion

using System.Diagnostics;
using System.Globalization;

namespace HKW.HKWTOML;

/// <summary>
/// Toml日期时间偏移量
/// </summary>
[DebuggerDisplay("{Value}, SecondsPrecision = {SecondsPrecision}")]
public class TomlDateTimeOffset : TomlDateTime
{
    /// <inheritdoc/>
    public override bool IsTomlDateTimeOffset { get; } = true;

    /// <summary>
    /// 值
    /// </summary>
    public DateTimeOffset Value { get; set; }

    /// <inheritdoc/>
    public override string ToString() => Value.ToString(CultureInfo.CurrentCulture);

    /// <inheritdoc/>
    public override string ToString(IFormatProvider formatProvider) =>
        Value.ToString(formatProvider);

    /// <inheritdoc/>
    public override string ToString(string? format, IFormatProvider? formatProvider) =>
        Value.ToString(format, formatProvider);

    /// <inheritdoc/>
    protected override string ToInlineTomlInternal() =>
        Value.ToString(TomlSyntax.RFC3339Formats[SecondsPrecision]);

    /// <inheritdoc/>
    public TomlDateTimeOffset(DateTimeOffset value)
    {
        Value = value;
    }
}
