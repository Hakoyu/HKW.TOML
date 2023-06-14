using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HKW.HKWTOML.AsClasses;

public partial class TomlAsClasses
{
    /// <summary>
    /// toml类值
    /// </summary>
    [DebuggerDisplay("{TypeName}, {Name}")]
    private class TomlClassValue
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// 特性
        /// </summary>
        public HashSet<string> Attributes { get; set; } = new();

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="typeName">类型名称</param>
        public TomlClassValue(string name, string typeName)
        {
            Name = name;
            TypeName = typeName;
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="node">类值(推断类型名称)</param>
        public TomlClassValue(string name, TomlNode node)
        {
            Name = name;
            TypeName = s_options.GetConvertName(node, TomlType.GetTypeCode(node));
        }

        /// <summary>
        /// 转化为格式化字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            var valueData = string.Format(
                s_options.PropertyFormat,
                s_options.Indent,
                TypeName,
                Name
            );
            return GetComment(Comment) + GetAttribute(Attributes) + valueData;
        }

        /// <summary>
        /// 获取注释
        /// </summary>
        /// <param name="comment">注释</param>
        /// <returns>格式化的注释</returns>
        private static string GetComment(string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
                return comment;
            var comments = comment.Split(
                new[] { '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries
            );
            if (comments.Length is 1)
                return string.Format(s_options.CommentFormat, s_options.Indent, comments[0]);
            var multiLineComment =
                comments[0]
                + "\n"
                + string.Join(
                    "\n",
                    comments[1..].Select(
                        s => string.Format(s_options.CommentParaFormat, s_options.Indent, s)
                    )
                );
            return string.Format(s_options.CommentFormat, s_options.Indent, multiLineComment);
        }

        /// <summary>
        /// 获取特性数据
        /// </summary>
        /// <param name="attributes">特性</param>
        /// <returns>格式化的特性数据</returns>
        private static string GetAttribute(IEnumerable<string> attributes)
        {
            var sb = new StringBuilder();
            foreach (var attribute in attributes)
                sb.AppendLine(
                    string.Format(s_options.AttributeFormat, s_options.Indent, attribute)
                );
            return sb.ToString();
        }
    }
}
