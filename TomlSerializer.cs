using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HKW.Libs.TOML;

public class TomlSerializer
{
    private TomlSerializer() { }
    public static void SerializerToFile(object source, string tomlFile)
    {
        CreateTomlTable(source).SaveTo(tomlFile);
    }
    private static TomlTable CreateTomlTable(object source)
    {
        var table = new TomlTable();
        var properties = source.GetType().GetProperties();
        foreach (var propertyInfo in properties)
        {
            if (Attribute.IsDefined(propertyInfo, typeof(TomlIgnore)))
                continue;
            if (propertyInfo.Name == nameof(ITomlClass.TableComment) || propertyInfo.Name == nameof(ITomlClass.ValueComments))
                continue;
            if (propertyInfo.GetValue(source) is not object value)
                continue;
            var node = CreateTomlNode(value);
            table.Add(propertyInfo.Name, node);
        }
        SetComments(source as ITomlClass, table);
        return table;
    }
    private static TomlArray CreateTomlArray(IList list)
    {
        var array = new TomlArray();
        foreach (var item in list)
            array.Add(CreateTomlNode(item));
        return array;
    }

    private static TomlNode CreateTomlNode(object source)
    {
        return source switch
        {
            bool => new TomlBoolean { Value = (bool)source },
            string => new TomlString { Value = source != null ? source.ToString()! : string.Empty },

            float
                => new TomlFloat
                {
                    Value = (double)Convert.ChangeType((float)source, TypeCode.Double)
                },
            double => new TomlFloat { Value = (double)Convert.ChangeType(source, TypeCode.Double) },

            sbyte => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },
            byte => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },
            short => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },
            ushort => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },
            int => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },
            uint => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },
            long => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },
            ulong => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },

            DateTime => new TomlDateTimeLocal { Value = (DateTime)source },
            DateTimeOffset => new TomlDateTimeOffset { Value = (DateTimeOffset)source },

            TomlNode => (TomlNode)source,
            IList v => CreateTomlArray(v),
            var _ => CreateTomlTable(source),
        };
    }

    private static void SetComments(ITomlClass? iTomlClass, TomlTable table)
    {
        if (iTomlClass is null)
            return;
        if (string.IsNullOrWhiteSpace(iTomlClass.TableComment) is false)
            table.Comment = iTomlClass.TableComment;
        if (iTomlClass.ValueComments is null)
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
