namespace HKW.TOML.TomlAttribute;

/// <summary>
/// Toml参数顺序
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class TomlSortOrder : Attribute
{
    /// <summary>
    /// 顺序
    /// </summary>
    public int Value { get; }

    /// <inheritdoc/>
    public TomlSortOrder(int order) => Value = order;
}