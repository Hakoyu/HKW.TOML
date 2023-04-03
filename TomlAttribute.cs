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
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class TomlIgnore : Attribute { }

/// <summary>
/// Toml键名称
/// <para>指定Toml键的名称</para>
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class TomlKeyName : Attribute
{
    /// <summary>
    /// 键名
    /// </summary>
    public string Name { get; }

    public TomlKeyName(string name) => Name = name;
}

/// <summary>
/// Toml参数顺序
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class TomlParameterOrder : Attribute
{
    /// <summary>
    /// 顺序
    /// </summary>
    public int Order { get; }

    public TomlParameterOrder(int order) => Order  = order;
}

