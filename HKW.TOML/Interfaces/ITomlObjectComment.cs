namespace HKW.HKWTOML.Interfaces;

/// <summary>
/// Toml类接口
/// </summary>
public interface ITomlObjectComment
{
    /// <summary>
    /// 类注释
    /// </summary>
    public TomlComment ObjectComment { get; set; }

    /// <summary>
    /// 子项注释
    /// <para>(PropertyName, TomlComment)</para>
    /// </summary>
    public Dictionary<string, TomlComment> PropertyComments { get; set; }
}
