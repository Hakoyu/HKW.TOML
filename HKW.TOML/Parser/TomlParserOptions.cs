using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKW.HKWTOML;

/// <summary>
/// Toml解析设置
/// </summary>
public class TomlParserOptions
{
    /// <summary>
    /// 抛出异常
    /// </summary>
    [DefaultValue(true)]
    public bool ThrowException { get; set; } = true;

    /// <summary>
    /// 创建自定义 TomlTable
    /// <para><![CDATA[
    /// 例如创建忽略大小写的 TomlTable, 或是使用任何支持 IDictionary<string, TomlNode> 的自定义字典
    /// () => new TomlTable(new Dictionary<string, TomlNode>(StringComparer.OrdinalIgnoreCase));
    /// ]]></para>
    /// </summary>
    public Func<TomlTable>? CreateTomlTable { get; set; }

    /// <summary>
    /// 创建自定义 TomlArray
    /// <para><![CDATA[
    /// 创建自定义 TomlTable, 或是任何支持 IList<TomlNode> 的自定义列表
    /// () => new TomlArray(new ObservableCollection<TomlNode>());
    /// ]]></para>
    /// </summary>
    public Func<TomlArray>? CreateTomlArray { get; set; }
}
