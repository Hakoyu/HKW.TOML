namespace HKW.TOML.TomlAttribute;

/// <summary>
/// Toml属性名称
/// <para>指定Toml键的名称</para>
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class TomlClassNameAttribute : Attribute
{
    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; }

    /// <inheritdoc/>
    public TomlClassNameAttribute(string name) => Value = name;
}

