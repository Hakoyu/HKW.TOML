﻿using System.ComponentModel;
using System.Reflection;
using HKW.HKWTOML.Exceptions;

namespace HKW.HKWTOML.Serializer;

/// <summary>
/// Toml序列化设置
/// </summary>
public class TOMLSerializerOptions
{
    /// <summary>
    /// 允许非公有属性
    /// </summary>
    [DefaultValue(false)]
    public bool AllowNonPublicProperty { get; set; } = false;

    /// <summary>
    /// 允许静态属性
    /// </summary>
    [DefaultValue(false)]
    public bool AllowStaticProperty { get; set; } = false;

    /// <summary>
    /// 属性排序比较器
    /// </summary>
    public IComparer<PropertyInfo>? PropertiesOrderComparer { get; set; }

    /// <summary>
    /// 属性倒序排列
    /// </summary>
    [DefaultValue(false)]
    public bool PropertiesReverseOrder { get; set; } = false;

    /// <summary>
    /// 将枚举转换为 <see cref="TomlInteger"/> 而不是 <see cref="TomlString"/>
    /// </summary>
    [DefaultValue(false)]
    public bool EnumToInteger { get; set; } = false;

    /// <summary>
    /// 异常处理模式
    /// </summary>
    [DefaultValue(ExceptionHandlingMode.Throw)]
    public ExceptionHandlingMode ExceptionHandling { get; set; } = ExceptionHandlingMode.Throw;

    /// <summary>
    /// 所有异常
    /// <para>
    /// (PropertyFullName, Exception)
    /// </para>
    /// </summary>
    public Dictionary<string, Exception> Exceptions { get; set; } = new();
}
