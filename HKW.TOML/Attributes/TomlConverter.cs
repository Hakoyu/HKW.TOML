using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HKW.HKWTOML.Deserializer;
using HKW.HKWTOML.Interfaces;
using HKW.HKWTOML.Serializer;

namespace HKW.HKWTOML.Attributes;

/// <summary>
/// Toml值转换
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class TOMLConverterAttribute : Attribute
{
    private readonly object _iTomlConverter;
    private readonly MethodInfo _readMethod;
    private readonly MethodInfo _writeMethod;

    /// <inheritdoc/>
    /// <param name="tomlConverter">Toml值转换类</param>
    /// <exception cref="Exceptions">Unimplemented interface <see cref="ITOMLConverter{T}"/></exception>
    public TOMLConverterAttribute(Type tomlConverter)
    {
        if (
            tomlConverter.GetInterfaces().Any(i => i.Name == typeof(ITOMLConverter<>).Name) is false
        )
            throw new Exception($"Unimplemented interface \"{typeof(ITOMLConverter<>).Name}\"");
        _iTomlConverter = tomlConverter.Assembly.CreateInstance(tomlConverter.FullName!)!;
        _readMethod = tomlConverter.GetMethod(nameof(ITOMLConverter<object>.Read))!;
        _writeMethod = tomlConverter.GetMethod(nameof(ITOMLConverter<object>.Write))!;
    }

    /// <summary>
    /// 从Toml节点读取值 用于 <see cref="TOMLDeserializer"/>
    /// </summary>
    /// <param name="node">Toml节点</param>
    /// <returns>转换后的值</returns>
    public object Read(TomlNode node)
    {
        return _readMethod.Invoke(_iTomlConverter, new[] { node })!;
    }

    /// <summary>
    /// 从值转换成Toml节点 用于 <see cref="TOMLSerializer"/>
    /// </summary>
    /// <param name="value">值</param>
    /// <returns>转换后的Toml节点</returns>
    public TomlNode Write(object value)
    {
        return (TomlNode)_writeMethod.Invoke(_iTomlConverter, new[] { value })!;
    }
}
