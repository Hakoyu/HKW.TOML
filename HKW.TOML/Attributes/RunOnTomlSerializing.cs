using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HKW.HKWTOML.Attributes;

/// <summary>
/// 运行于序列化前
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RunOnTomlSerializingAttribute : Attribute
{
    /// <summary>
    /// 方法
    /// </summary>
    public MethodInfo? Method { get; }

    /// <summary>
    /// 参数
    /// </summary>
    public object[]? Parameters { get; }

    /// <inheritdoc/>
    public RunOnTomlSerializingAttribute() { }

    /// <inheritdoc/>
    /// <param name="parameters">参数</param>
    public RunOnTomlSerializingAttribute(params object[] parameters)
    {
        Parameters = parameters;
    }

    /// <summary>
    /// 运行于序列化前的方法
    /// </summary>
    /// <param name="type">目标类</param>
    /// <param name="staticMethodName">静态方法名称</param>
    /// <param name="parameters">参数</param>
    public RunOnTomlSerializingAttribute(
        Type type,
        string staticMethodName,
        params object[] parameters
    )
    {
        if (type.GetMethod(staticMethodName, BindingFlags.Static) is not MethodInfo method)
            throw new Exception($"Not found static method {staticMethodName} in {type.FullName}");
        Method = method;
        Parameters = parameters;
    }
}
