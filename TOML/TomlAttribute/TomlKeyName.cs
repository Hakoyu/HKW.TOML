namespace HKW.TOML.TomlAttribute;

/// <summary>
/// Toml键名称
/// <para>指定Toml键的名称</para>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class TomlPropertyName : Attribute
{
    /// <summary>
    /// 键名
    /// </summary>
    public string Value { get; }

    /// <inheritdoc/>
    public TomlPropertyName(string name) => Value = name;
}
