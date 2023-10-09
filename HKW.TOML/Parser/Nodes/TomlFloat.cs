#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion

using System.Diagnostics;
using System.Globalization;

namespace HKW.HKWTOML;

/// <summary>
/// Toml浮点型
/// </summary>
[DebuggerDisplay("Value")]
public class TomlFloat : TomlNode, IFormattable
{
    /// <inheritdoc/>
    public override bool IsTomlFloat { get; } = true;

    /// <inheritdoc/>
    public override bool HasValue { get; } = true;

    /// <summary>
    /// 值
    /// </summary>
    public double Value { get; set; }

    /// <inheritdoc/>
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider) =>
        Value.ToString(format, formatProvider);

    /// <inheritdoc/>
    public string ToString(IFormatProvider formatProvider) => Value.ToString(formatProvider);

    /// <inheritdoc/>
    public override string ToInlineToml() =>
        Value switch
        {
            var v when double.IsNaN(v) => TomlSyntax.NAN_VALUE,
            var v when double.IsPositiveInfinity(v) => TomlSyntax.INF_VALUE,
            var v when double.IsNegativeInfinity(v) => TomlSyntax.NEG_INF_VALUE,
            var v => v.ToString("G", CultureInfo.InvariantCulture).ToLowerInvariant()
        };

    /// <inheritdoc/>
    public TomlFloat(double value)
    {
        Value = value;
    }
}
