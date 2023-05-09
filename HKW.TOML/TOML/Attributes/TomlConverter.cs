using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HKW.TOML.Deserializer;
using HKW.TOML.Interfaces;
using HKW.TOML.Serializer;

namespace HKW.TOML.Attributes;

/// <summary>
/// Toml值转换
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class TomlConverterAttribute : Attribute
{
    private readonly object _iTomlConverter;
    private readonly MethodInfo _readMethod;
    private readonly MethodInfo _writeMethod;

    /// <inheritdoc/>
    /// <param name="tomlConverter">Toml值转换类</param>
    /// <exception cref="Exceptions">Unimplemented interface <see cref="ITomlConverter{Object}"/></exception>
    public TomlConverterAttribute(Type tomlConverter)
    {
        if (
            tomlConverter.GetInterfaces().Any(i => i.Name == typeof(ITomlConverter<>).Name) is false
        )
            throw new Exception($"Unimplemented interface \"{typeof(ITomlConverter<>).Name}\"");
        _iTomlConverter = tomlConverter.Assembly.CreateInstance(tomlConverter.FullName!)!;
        _readMethod = tomlConverter.GetMethod(nameof(ITomlConverter<object>.Read))!;
        _writeMethod = tomlConverter.GetMethod(nameof(ITomlConverter<object>.Write))!;
    }

    /// <summary>
    /// 从Toml节点读取值 用于 <see cref="TomlDeserializer"/>
    /// </summary>
    /// <param name="node">Toml节点</param>
    /// <returns>转换后的值</returns>
    public object Read(TomlNode node)
    {
        return _readMethod.Invoke(_iTomlConverter, new[] { node })!;
    }

    /// <summary>
    /// 从值转换成Toml节点 用于 <see cref="TomlSerializer"/>
    /// </summary>
    /// <param name="value">值</param>
    /// <returns>转换后的Toml节点</returns>
    public TomlNode Write(object value)
    {
        return (TomlNode)_writeMethod.Invoke(_iTomlConverter, new[] { value })!;
    }
}
