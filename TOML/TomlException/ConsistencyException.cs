using System.Text;

namespace HKW.TOML.TomlException;

/// <summary>
/// 一致性异常
/// </summary>
public class ConsistencyException : Exception
{
    /// <summary>
    /// 缺失数据的属性
    /// </summary>
    public string[] MissingDataProperties { get; private set; } = null!;

    /// <summary>
    /// 缺失数据的Toml节点
    /// </summary>
    public string[] MissingDataTomlNodes { get; private set; } = null!;

    /// <summary>
    /// 一致性异常
    /// </summary>
    /// <param name="message">信息</param>
    /// <param name="missingDataProperties">缺失的属性</param>
    /// <param name="missingDataTomlNodes">缺失的Toml节点</param>
    public ConsistencyException(
        string message,
        IEnumerable<string> missingDataProperties,
        IEnumerable<string> missingDataTomlNodes
    )
        : base(message)
    {
        MissingDataProperties = missingDataProperties.ToArray();
        MissingDataTomlNodes = missingDataTomlNodes.ToArray();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder();
        if (MissingDataProperties.Any())
        {
            sb.AppendLine($"{nameof(MissingDataProperties)} Size: {MissingDataProperties.Length}");
            sb.AppendLine("    " + string.Join("\n    ", MissingDataProperties));
        }
        if (MissingDataTomlNodes.Any())
        {
            sb.AppendLine($"{nameof(MissingDataTomlNodes)} Size: {MissingDataTomlNodes.Length}");
            sb.AppendLine("    " + string.Join("\n    ", MissingDataTomlNodes));
        }
        return Message
            + "\n"
            + sb.ToString()
            + "\n---------StackTraceInfo---------\n"
            + StackTrace?.ToString();
    }
}
