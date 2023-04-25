using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HKWToml.Utils;

/// <summary>
/// 方法和参数
/// </summary>
internal class MethodAndParameters
{
    /// <summary>
    /// 方法信息
    /// </summary>
    public MethodInfo Method { get; set; }
    /// <summary>
    /// 参数
    /// </summary>
    public object[]? Parameters { get; set; }

    /// <inheritdoc/>
    /// <param name="method">方法信息</param>
    /// <param name="parameters">参数</param>
    public MethodAndParameters(MethodInfo method, params object[]? parameters)
    {
        Method = method;
        Parameters = parameters;
    }
}
