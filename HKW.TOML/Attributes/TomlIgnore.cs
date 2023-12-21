namespace HKW.HKWTOML.Attributes;

/// <summary>
/// Toml忽略值
/// <para>在序列化和反序列化时忽略的值</para>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class TOMLIgnoreAttribute : Attribute { }
