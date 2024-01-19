using HKW.FastMember;
using HKW.HKWTOML.Deserializer;
using HKW.HKWTOML.Interfaces;
using HKW.HKWTOML.Serializer;
using System.Reflection;

namespace HKW.HKWTOML.Attributes;

/// <summary>
/// Toml值转换
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class TomlConverterAttribute : Attribute
{
    /// <summary>
    /// 转换器
    /// </summary>
    public ITomlConverter Converter { get; }

    /// <inheritdoc/>
    /// <param name="tomlConverter">Toml值转换类</param>
    public TomlConverterAttribute(Type tomlConverter)
    {
        Converter = (ITomlConverter)TypeAccessor.Create(tomlConverter).CreateNew();
    }
}
