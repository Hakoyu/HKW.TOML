using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HKW.Libs.TOML;

/// <summary>
/// Toml序列化
/// </summary>
public class TomlSerializer
{
    private TomlSerializer() { }

    /// <summary>
    /// 序列化至Toml文件
    /// </summary>
    /// <param name="source">源</param>
    /// <param name="tomlFile">Toml文件</param>
    public static void SerializerToFile(object source, string tomlFile)
    {
        CreateTomlTable(source).SaveTo(tomlFile);
    }

    /// <summary>
    /// 异步序列化至Toml文件
    /// </summary>
    /// <param name="source">源</param>
    /// <param name="tomlFile">Toml文件</param>
    public static async Task SerializerToFileAsync(object source, string tomlFile)
    {
        await Task.Run(() =>
        {
            CreateTomlTable(source).SaveTo(tomlFile);
        });
    }

    /// <summary>
    /// 序列化至Toml表格数据
    /// </summary>
    /// <param name="source">源</param>
    /// <returns>Toml表格数据</returns>
    public static TomlTable Serializer(object source)
    {
        return CreateTomlTable(source);
    }

    /// <summary>
    /// 异步序列化至Toml表格数据
    /// </summary>
    /// <param name="source">源</param>
    /// <returns>Toml表格数据</returns>
    public static async Task<TomlTable> SerializerAsync(object source)
    {
        return await Task.Run(() =>
        {
            return CreateTomlTable(source);
        });
    }

    /// <summary>
    /// 创建Toml表格
    /// </summary>
    /// <param name="source">源</param>
    /// <returns>Toml表格</returns>
    private static TomlTable CreateTomlTable(object source)
    {
        var table = new TomlTable();
        // 获取所有属性
        var properties = source.GetType().GetProperties();
        var isITomlClass = source is ITomlClass;

        foreach (var propertyInfo in properties)
        {
            // 检测是否有隐藏特性
            if (Attribute.IsDefined(propertyInfo, typeof(TomlIgnore)))
                continue;
            // 跳过ITomlClass生成的接口
            if (
                isITomlClass
                && (
                    propertyInfo.Name == nameof(ITomlClass.TableComment)
                    || propertyInfo.Name == nameof(ITomlClass.ValueComments)
                )
            )
                continue;
            // 获取属性的值
            if (propertyInfo.GetValue(source) is not object value)
                continue;
            // 创建Toml节点
            var node = CreateTomlNode(value);
            table.Add(propertyInfo.Name, node);
        }
        // 设置注释
        SetComments(source as ITomlClass, table);
        return table;
    }

    /// <summary>
    /// 创建Toml数组
    /// </summary>
    /// <param name="list">列表</param>
    /// <returns>Toml数组</returns>
    private static TomlArray CreateTomlArray(IList list)
    {
        var array = new TomlArray();
        foreach (var item in list)
            array.Add(CreateTomlNode(item));
        return array;
    }

    /// <summary>
    /// 创建Toml节点
    /// </summary>
    /// <param name="source">源</param>
    /// <returns>Toml节点</returns>
    private static TomlNode CreateTomlNode(object source)
    {
        return source switch
        {
            bool => new TomlBoolean { Value = (bool)source },
            string => new TomlString { Value = source != null ? source.ToString()! : string.Empty },

            // 浮点型
            float
                => new TomlFloat
                {
                    Value = (double)Convert.ChangeType((float)source, TypeCode.Double)
                },
            double => new TomlFloat { Value = (double)Convert.ChangeType(source, TypeCode.Double) },

            // 整形
            sbyte => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },
            byte => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },
            short => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },
            ushort => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },
            int => new TomlInteger { Value = (int)source },
            uint => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },
            long => new TomlInteger { Value = (long)source },
            ulong => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },

            DateTime => new TomlDateTimeLocal { Value = (DateTime)source },
            DateTimeOffset => new TomlDateTimeOffset { Value = (DateTimeOffset)source },

            TomlNode => (TomlNode)source,
            IList v => CreateTomlArray(v),
            var _ => CreateTomlTable(source),
        };
    }

    /// <summary>
    /// 设置注释
    /// </summary>
    /// <param name="iTomlClass">TomlClass接口</param>
    /// <param name="table">Toml表格</param>
    private static void SetComments(ITomlClass? iTomlClass, TomlTable table)
    {
        if (iTomlClass is null)
            return;
        if (string.IsNullOrWhiteSpace(iTomlClass.TableComment) is false)
            table.Comment = iTomlClass.TableComment;
        // 检查值注释
        if (iTomlClass.ValueComments?.Any() is null or false)
            return;
        foreach (var commentKV in iTomlClass.ValueComments)
        {
            var name = commentKV.Key;
            var comment = commentKV.Value;
            if (string.IsNullOrWhiteSpace(comment) is false)
                table[name].Comment = comment;
        }
    }
}
