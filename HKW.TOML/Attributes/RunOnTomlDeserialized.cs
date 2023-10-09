using System.Reflection;

namespace HKW.HKWTOML.Attributes;

/// <summary>
/// 运行于反序列化后
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RunOnTOMLDeserializedAttribute : Attribute
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
    public RunOnTOMLDeserializedAttribute() { }

    /// <inheritdoc/>
    /// <param name="parameters">参数</param>
    public RunOnTOMLDeserializedAttribute(params object[] parameters)
    {
        Parameters = parameters;
    }

    /// <summary>
    /// 运行于反序列化后的方法
    /// </summary>
    /// <param name="type">目标类</param>
    /// <param name="staticMethodName">静态方法名称</param>
    /// <param name="parameters">参数</param>
    public RunOnTOMLDeserializedAttribute(
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
