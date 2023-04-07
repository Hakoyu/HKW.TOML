using System.Text;

namespace HKW.TOML;

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
        string[] missingDataProperties,
        string[] missingDataTomlNodes
    )
        : base(message)
    {
        MissingDataProperties = missingDataProperties;
        MissingDataTomlNodes = missingDataTomlNodes;
    }
    /// <inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder();
        if (MissingDataProperties.Any())
        {
            sb.AppendLine($"{nameof(MissingDataProperties)} Size: {MissingDataProperties.Length}");
            sb.AppendLine("    " + string.Join(Environment.NewLine + "    ", MissingDataProperties));
        }
        if (MissingDataTomlNodes.Any())
        {
            sb.AppendLine($"{nameof(MissingDataTomlNodes)} Size: {MissingDataTomlNodes.Length}");
            sb.AppendLine("    " + string.Join(Environment.NewLine + "    ", MissingDataTomlNodes));
        }
        return Message
            + Environment.NewLine
            + sb.ToString()
            + Environment.NewLine
            + "---------StackTraceInfo---------"
            + Environment.NewLine
            + StackTrace?.ToString();
    }
}
