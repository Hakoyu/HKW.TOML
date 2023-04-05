using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKW.TOML;

/// <summary>
/// Toml忽略值
/// <para>在序列化和反序列化时忽略的值</para>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class TomlIgnore : Attribute { }

/// <summary>
/// Toml键名称
/// <para>指定Toml键的名称</para>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class TomlKeyName : Attribute
{
    /// <summary>
    /// 键名
    /// </summary>
    public string Value { get; }
    /// <inheritdoc/>
    public TomlKeyName(string name) => Value = name;
}

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

