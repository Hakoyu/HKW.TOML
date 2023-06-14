using System.Collections.Generic;

namespace HKW.HKWTOML.Deserializer;

/// <summary>
/// Toml反序列化设置
/// </summary>
public class DeserializerOptions
{
    /// <summary>
    /// 删除键的单词分隔符 如 "_"
    /// <para>默认为 <see langword="true"/></para>
    /// </summary>
    public bool RemoveKeyWordSeparator { get; set; } = true;

    /// <summary>
    /// 单词分隔符
    /// <para>默认为 { '<see langword="_"/>' }</para>
    /// </summary>
    public HashSet<char> KeyWordSeparators { get; set; } = new() { '_' };

    /// <summary>
    /// 属性名称忽略大小写
    /// <para>默认为 <see langword="true"/></para>
    /// </summary>
    public bool PropertyNameCaseInsensitive { get; set; } = true;
}
