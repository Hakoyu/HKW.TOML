#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion


namespace HKW.TOML;

/// <summary>
/// Toml类
/// </summary>
public class TomlType
{
    /// <summary>
    /// 获取Toml类型代码
    /// </summary>
    /// <param name="node">Toml节点</param>
    /// <returns>Toml类型代码</returns>
    public static TomlTypeCode GetTypeCode(TomlNode node)
    {
        return node switch
        {
            TomlBoolean => TomlTypeCode.Boolean,
            TomlString => TomlTypeCode.String,
            TomlInteger => TomlTypeCode.Integer,
            TomlFloat => TomlTypeCode.Float,
            TomlDateTimeOffset => TomlTypeCode.DateTimeOffset,
            TomlDateTimeLocal => TomlTypeCode.DateTime,
            TomlDateTime => TomlTypeCode.DateTime,
            TomlArray => TomlTypeCode.Array,
            TomlTable => TomlTypeCode.Table,
            _ => TomlTypeCode.None,
        };
    }
}
