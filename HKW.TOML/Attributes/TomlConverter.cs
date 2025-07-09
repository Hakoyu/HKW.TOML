using System.Reflection;
using HKW.FastMember;
using HKW.HKWTOML.Deserializer;
using HKW.HKWTOML.Interfaces;
using HKW.HKWTOML.Serializer;

namespace HKW.HKWTOML.Attributes;

/// <summary>
/// Toml值转换
/// </summary>
/// <param name="tomlConverter">Toml值转换类</param>
[AttributeUsage(AttributeTargets.Property)]
public class TomlConverterAttribute(Type tomlConverter) : Attribute
{
    /// <summary>
    /// 转换器
    /// </summary>
    public ITomlConverter Converter { get; } =
        (ITomlConverter)TypeAccessor.Create(tomlConverter).CreateNew();
}
