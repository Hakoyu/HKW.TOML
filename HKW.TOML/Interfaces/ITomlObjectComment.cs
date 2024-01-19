namespace HKW.HKWTOML.Interfaces;

/// <summary>
/// Toml类接口
/// </summary>
public interface ITomlObjectComment
{
    /// <summary>
    /// 类注释
    /// </summary>
    public string ObjectComment { get; set; }

    /// <summary>
    /// 值注释
    /// <para>(PropertyName, Comment)</para>
    /// </summary>
    public Dictionary<string, string> PropertyComments { get; set; }
}
