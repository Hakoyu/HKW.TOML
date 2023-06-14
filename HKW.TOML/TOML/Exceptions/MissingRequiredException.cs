using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKW.HKWTOML.Exceptions;

/// <summary>
/// 缺失必要属性异常
/// </summary>
public class MissingRequiredException : Exception
{
    /// <summary>
    /// 缺失的必要属性
    /// </summary>
    public string[] MissingProperties { get; private set; }

    /// <inheritdoc/>
    /// <param name="message">信息</param>
    /// <param name="missingProperties">缺失的属性</param>
    public MissingRequiredException(string message, IEnumerable<string> missingProperties)
        : base(message)
    {
        MissingProperties = missingProperties.ToArray();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder();
        if (MissingProperties.Any())
        {
            sb.AppendLine($"{nameof(MissingProperties)} Size: {MissingProperties.Length}");
            sb.AppendLine("    " + string.Join("\n    ", MissingProperties));
        }
        return Message
            + "\n"
            + sb.ToString()
            + "\n---------StackTraceInfo---------\n"
            + StackTrace?.ToString();
    }
}
