#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion

using System.Diagnostics;
using System.Globalization;

namespace HKW.HKWTOML;

/// <summary>
/// Toml地区日期时间
/// </summary>
[DebuggerDisplay("{Value}, Style = {Style}, SecondsPrecision = {SecondsPrecision}")]
public class TomlDateTimeLocal : TomlDateTime
{
    /// <summary>
    /// 日期时间类型
    /// </summary>
    public enum DateTimeStyle
    {
        /// <summary>
        /// 日期
        /// </summary>
        Date,

        /// <summary>
        /// 时间
        /// </summary>
        Time,

        /// <summary>
        /// 日期时间
        /// </summary>
        DateTime
    }

    /// <inheritdoc/>
    public override bool IsTomlDateTimeLocal { get; } = true;

    /// <summary>
    /// 日期时间类型
    /// </summary>
    public DateTimeStyle Style { get; set; } = DateTimeStyle.DateTime;

    /// <summary>
    /// 值
    /// </summary>
    public DateTime Value { get; set; }

    /// <inheritdoc/>
    public override string ToString() => Value.ToString(CultureInfo.CurrentCulture);

    /// <inheritdoc/>
    public override string ToString(IFormatProvider formatProvider) =>
        Value.ToString(formatProvider);

    /// <inheritdoc/>
    public override string ToString(string? format, IFormatProvider? formatProvider) =>
        Value.ToString(format, formatProvider);

    /// <inheritdoc/>
    public override string ToInlineToml() =>
        Style switch
        {
            DateTimeStyle.Date => Value.ToString(TomlSyntax.LocalDateFormat),
            DateTimeStyle.Time
                => Value.ToString(TomlSyntax.RFC3339LocalTimeFormats[SecondsPrecision]),
            var _ => Value.ToString(TomlSyntax.RFC3339LocalDateTimeFormats[SecondsPrecision])
        };

    /// <inheritdoc/>
    public TomlDateTimeLocal(DateTime dateTime)
    {
        Value = dateTime;
    }
}
