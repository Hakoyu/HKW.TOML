namespace HKW.HKWTOML.Attributes;

/// <summary>
/// Toml属性名称
/// <para>指定Toml键的名称</para>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class TOMLPropertyNameAttribute : Attribute
{
    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; }

    /// <inheritdoc/>
    public TOMLPropertyNameAttribute(string name) => Value = name;
}
