using System.Collections.Generic;

namespace HKW.HKWTOML.Deserializer;

/// <summary>
/// Toml反序列化设置
/// </summary>
public class TOMLDeserializerOptions
{
    ///// <summary>
    ///// 删除键的单词分隔符 如 "_"
    ///// <para>默认为 <see langword="true"/></para>
    ///// </summary>
    //public bool RemoveKeyWordSeparator { get; set; } = true;

    ///// <summary>
    ///// 单词分隔符
    ///// <para>默认为 '<see langword="_"/>'</para>
    ///// </summary>
    //public char KeyWordSeparator { get; set; } = '_';

    /// <summary>
    /// 属性名称忽略大小写
    /// <para>默认为 <see langword="true"/></para>
    /// </summary>
    public bool PropertyNameCaseInsensitive { get; set; } = true;

    /// <summary>
    /// 枚举转换时忽略大小写
    /// <para>默认为 <see langword="true"/></para>
    /// <para>在 <see cref="IntegerToEnum"/> 为 <see langword="true"/> 时 此值无效</para>
    /// </summary>
    public bool EnumIgnoreCase { get; set; } = true;

    /// <summary>
    /// 从 <see cref="TomlInteger"/> 而不是 <see cref="TomlString"/> 中获取枚举
    /// <para>默认为 <see langword="false"/></para>
    /// </summary>
    public bool IntegerToEnum { get; set; } = false;
}
