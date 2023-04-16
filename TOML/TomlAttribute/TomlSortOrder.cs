namespace HKW.TOML.Attribute;

/// <summary>
/// Toml参数顺序
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class TomlPropertyOrderAttribute : System.Attribute
{
    /// <summary>
    /// 顺序
    /// </summary>
    public int Value { get; }

    /// <inheritdoc/>
    public TomlPropertyOrderAttribute(int order) => Value = order;
}