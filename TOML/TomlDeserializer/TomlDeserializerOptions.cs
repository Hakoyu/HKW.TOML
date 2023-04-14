namespace HKW.TOML.TomlDeserializer;

/// <summary>
/// Toml反序列化设置
/// </summary>
public class TomlDeserializerOptions
{
    /// <summary>
    /// 删除键的单词分隔符 如 "_"
    /// <para>默认为 <see langword="true"/></para>
    /// </summary>
    public bool RemoveKeyWordSeparator { get; set; } = true;

    /// <summary>
    /// 单词分隔符
    /// <para>默认为 "<see langword="_"/>"</para>
    /// </summary>
    public string KeyWordSeparator { get; set; } = "_";

    /// <summary>
    /// 检查一致性
    /// <para>默认为 <see langword="false"/></para>
    /// </summary>
    public bool CheckConsistency { get; set; } = false;
}