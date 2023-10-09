#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion

using System.Diagnostics;
using System.Globalization;

namespace HKW.HKWTOML;

/// <summary>
/// Toml整型
/// </summary>
[DebuggerDisplay("{Value}, IntegerBase = {IntegerBase}")]
public class TomlInteger : TomlNode
{
    /// <summary>
    /// 进位制
    /// </summary>
    public enum Base
    {
        /// <summary>
        /// 二进制
        /// </summary>
        Binary = 2,

        /// <summary>
        /// 八进制
        /// </summary>
        Octal = 8,

        /// <summary>
        /// 十进制
        /// </summary>
        Decimal = 10,

        /// <summary>
        /// 十六进制
        /// </summary>
        Hexadecimal = 16
    }

    /// <inheritdoc/>
    public override bool IsTomlInteger { get; } = true;

    /// <inheritdoc/>
    public override bool HasValue { get; } = true;

    /// <summary>
    /// 整型进位制
    /// </summary>
    public Base IntegerBase { get; set; } = Base.Decimal;

    /// <summary>
    /// 值
    /// </summary>
    public long Value { get; set; }

    /// <summary>
    /// 是64位整型
    /// </summary>
    public bool IsInteger64 => int.TryParse(Value.ToString(), out _) is false;

    /// <inheritdoc/>
    public override string ToString() => Value.ToString();

    /// <inheritdoc/>
    public override string ToInlineToml() =>
        IntegerBase != Base.Decimal
            ? $"0{TomlSyntax.BaseIdentifiers[(int)IntegerBase]}{Convert.ToString(Value, (int)IntegerBase)}"
            : Value.ToString(CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    public TomlInteger(long value)
    {
        Value = value;
    }
}
