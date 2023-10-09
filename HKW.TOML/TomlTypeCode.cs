#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion


namespace HKW.HKWTOML;

/// <summary>
/// Toml类型代码
/// </summary>
[Flags]
public enum TomlTypeCode
{
    /// <summary>
    /// 空
    /// </summary>
    None = 0,

    /// <summary>
    /// 整型
    /// </summary>
    Integer = 1,

    /// <summary>
    /// 浮点型
    /// </summary>
    Float = 2,

    /// <summary>
    /// 字符串
    /// </summary>
    String = 4,

    /// <summary>
    /// 布尔类型
    /// </summary>
    Boolean = 8,

    /// <summary>
    /// 日期时间
    /// </summary>
    DateTime = 16,

    /// <summary>
    /// 地区日期时间
    /// </summary>
    DateTimeLocal = 32,

    /// <summary>
    /// 日期时间偏移量
    /// </summary>
    DateTimeOffset = 64,

    /// <summary>
    /// 数组
    /// </summary>
    Array = 128,

    /// <summary>
    /// 表格
    /// </summary>
    Table = 256,
}
