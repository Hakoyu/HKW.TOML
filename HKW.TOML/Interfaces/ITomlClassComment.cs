namespace HKW.HKWTOML.Interfaces;

/// <summary>
/// Toml类接口
/// </summary>
public interface ITOMLClassComment
{
    /// <summary>
    /// 类注释
    /// </summary>
    public string ClassComment { get; set; }

    /// <summary>
    /// 值注释
    /// </summary>
    public Dictionary<string, string> ValueComments { get; set; }
}
