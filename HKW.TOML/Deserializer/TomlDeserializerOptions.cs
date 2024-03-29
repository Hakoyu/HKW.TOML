﻿using System.ComponentModel;
using System.Reflection;
using HKW.HKWTOML.Exceptions;

namespace HKW.HKWTOML.Deserializer;

/// <summary>
/// Toml反序列化设置
/// </summary>
public class TOMLDeserializerOptions
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
    /// 键比较
    /// </summary>
    [DefaultValue(StringComparison.CurrentCulture)]
    public StringComparison KeyComparison { get; set; } = StringComparison.CurrentCulture;

    /// <summary>
    /// 枚举转换时忽略大小写
    /// </summary>
    [DefaultValue(false)]
    public bool EnumIgnoreCase { get; set; } = false;

    /// <summary>
    /// 缺失的必要属性值
    /// </summary>
    public HashSet<string> MissingPequiredProperties { get; set; } = new();

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
