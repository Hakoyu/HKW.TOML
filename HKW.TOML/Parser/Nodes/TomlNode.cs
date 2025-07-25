#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion

using System.Collections;

namespace HKW.HKWTOML;

/// <summary>
/// Toml节点
/// </summary>
public abstract class TomlNode : IEnumerable
{
    /// <summary>
    /// 有值
    /// </summary>
    public virtual bool HasValue { get; } = false;

    #region TypeCheck

    /// <summary>
    /// 是Toml数组
    /// </summary>
    public virtual bool IsTomlArray { get; } = false;

    /// <summary>
    /// 是Toml表格
    /// </summary>
    public virtual bool IsTomlTable { get; } = false;

    /// <summary>
    /// 是Toml字符串
    /// </summary>
    public virtual bool IsTomlString { get; } = false;

    /// <summary>
    /// 是Toml整型
    /// </summary>
    public virtual bool IsTomlInteger { get; } = false;

    /// <summary>
    /// 是Toml浮点型
    /// </summary>
    public virtual bool IsTomlFloat { get; } = false;

    /// <summary>
    /// 是Toml日期时间
    /// </summary>
    public bool IsTomlDateTime => IsTomlDateTimeLocal || IsTomlDateTimeOffset;

    /// <summary>
    /// 是Toml地区日期时间
    /// </summary>
    public virtual bool IsTomlDateTimeLocal { get; } = false;

    /// <summary>
    /// 是Toml偏移日期时间
    /// </summary>
    public virtual bool IsTomlDateTimeOffset { get; } = false;

    /// <summary>
    /// 是Toml布尔类型
    /// </summary>
    public virtual bool IsTomlBoolean { get; } = false;

    #endregion

    /// <summary>
    /// 注释
    /// </summary>
    public virtual string Comment { get; set; } = string.Empty;

    /// <summary>
    /// 层级
    /// </summary>
    public virtual int CollapseLevel { get; set; }

    #region NativeType

    /// <summary>
    /// 转换为Int32
    /// </summary>
    public virtual int AsInt32 => (int)this;

    /// <summary>
    /// 转换为Int64
    /// </summary>
    public virtual long AsInt64 => (long)this;

    /// <summary>
    /// 转换为浮点
    /// </summary>
    public virtual float AsFloat => (float)Convert.ChangeType(this, TypeCode.Int64);

    /// <summary>
    /// 转换为双精度浮点
    /// </summary>
    public virtual double AsDouble => (double)this;

    /// <summary>
    /// 转换为字符串
    /// </summary>
    public virtual string AsString => AsTomlString;

    /// <summary>
    /// 转换为布尔
    /// </summary>
    public virtual bool AsBoolean => AsTomlBoolean;

    /// <summary>
    /// 转换为日期时间
    /// </summary>
    public virtual DateTime AsDateTime => AsTomlDateTimeLocal;

    /// <summary>
    /// 转换为日期时间偏移量
    /// </summary>
    public virtual DateTimeOffset AsDateTimeOffset => AsTomlDateTimeOffset;

    /// <summary>
    /// 转换为列表
    /// </summary>
    public virtual IList<TomlNode> AsList => AsTomlArray.RawArray;

    /// <summary>
    /// 转换为字典
    /// </summary>
    public virtual IDictionary<string, TomlNode> AsDictionary => AsTomlTable.RawTable;

    #endregion

    #region TomlType

    /// <summary>
    /// 转换为Toml表格
    /// </summary>
    public virtual TomlTable AsTomlTable => (this as TomlTable)!;

    /// <summary>
    /// 转换为Toml字符串
    /// </summary>
    public virtual TomlString AsTomlString => (this as TomlString)!;

    /// <summary>
    /// 转换为Toml整型
    /// </summary>
    public virtual TomlInteger AsTomlInteger => (this as TomlInteger)!;

    /// <summary>
    /// 转换为Toml浮点型
    /// </summary>
    public virtual TomlFloat AsTomlFloat => (this as TomlFloat)!;

    /// <summary>
    /// 转换为Toml布尔型
    /// </summary>
    public virtual TomlBoolean AsTomlBoolean => (this as TomlBoolean)!;

    /// <summary>
    /// 转换为Toml日期时间
    /// </summary>
    public virtual TomlDateTime AsTomlDateTime => (this as TomlDateTime)!;

    /// <summary>
    /// 转换为Toml地区日期时间
    /// </summary>
    public virtual TomlDateTimeLocal AsTomlDateTimeLocal => (this as TomlDateTimeLocal)!;

    /// <summary>
    /// 转换为Toml日期时间偏移量
    /// </summary>
    public virtual TomlDateTimeOffset AsTomlDateTimeOffset => (this as TomlDateTimeOffset)!;

    /// <summary>
    /// 转化为Toml数组
    /// </summary>
    public virtual TomlArray AsTomlArray => (this as TomlArray)!;

    #endregion

    /// <summary>
    /// 子数量
    /// </summary>
    public virtual int ChildrenCount => 0;

#pragma warning disable S3237,S108 // "value" contextual keyword should be used
    /// <summary>
    /// 使用键获取值(用于Toml表格)
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>值</returns>
    public virtual TomlNode this[string key]
    {
        get => null!;
        set { }
    }

    /// <summary>
    /// 使用索引获取值(用于Toml数组)
    /// </summary>
    /// <param name="index">索引</param>
    /// <returns>值</returns>
    public virtual TomlNode this[int index]
    {
        get => null!;
        set { }
    }
#pragma warning restore S3237 // "value" contextual keyword should be used

    /// <summary>
    /// 子成员
    /// </summary>
    public virtual IEnumerable<TomlNode> Children
    {
        get { yield break; }
    }

    /// <summary>
    /// 所有键
    /// </summary>
    public virtual IEnumerable<string> Keys
    {
        get { yield break; }
    }

    /// <inheritdoc/>
    public IEnumerator GetEnumerator() => Children.GetEnumerator();

    /// <summary>
    /// 尝试获取值
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="node">值</param>
    /// <returns>成功为 <see langword="true"/> 失败为 <see langword="false"/></returns>
    public virtual bool TryGetNode(string key, out TomlNode node)
    {
        node = null!;
        return false;
    }

    /// <summary>
    /// 拥有键
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>成功为 <see langword="true"/> 失败为 <see langword="false"/></returns>
    public virtual bool HasKey(string key) => false;

    /// <summary>
    /// 拥有索引
    /// </summary>
    /// <param name="index">索引</param>
    /// <returns>成功为 <see langword="true"/> 失败为 <see langword="false"/></returns>
    public virtual bool HasItemAt(int index) => false;

    /// <summary>
    /// 添加键值对(用于Toml表格)
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="node">值</param>
    public virtual void Add(string key, TomlNode node) { }

    /// <summary>
    /// 添加值(用于Toml数组)
    /// </summary>
    /// <param name="node">值</param>
    public virtual void Add(TomlNode node) { }

    /// <summary>
    /// 删除值(用于Toml数组)
    /// </summary>
    /// <param name="node">值</param>
    public virtual void Delete(TomlNode node) { }

    /// <summary>
    /// 使用键删除值(用于Toml表格)
    /// </summary>
    /// <param name="key"></param>
    public virtual void Delete(string key) { }

    /// <summary>
    /// 使用索引删除值(用于Toml数组)
    /// </summary>
    /// <param name="index"></param>
    public virtual void Delete(int index) { }

    /// <summary>
    /// 添加多个值(用于Toml数组)
    /// </summary>
    /// <param name="nodes">多个值</param>
    public virtual void AddRange(IEnumerable<TomlNode> nodes)
    {
        foreach (var tomlNode in nodes)
            Add(tomlNode);
    }

    /// <summary>
    /// 写入至
    /// </summary>
    /// <param name="tw">文本写入流</param>
    /// <param name="name">名称</param>
    public virtual void WriteTo(TextWriter tw, string name = null!) => tw.WriteLine(ToInlineToml());

    /// <summary>
    /// 转换为行内Toml格式字符串
    /// </summary>
    /// <returns>Toml格式字符串</returns>
    public virtual string ToInlineToml() => ToString()!;

    #region Native type to TOML cast

    /// <summary>
    /// 隐式转换 string -> TomlString
    /// </summary>
    /// <param name="value">字符串</param>
    public static implicit operator TomlNode(string value) => new TomlString(value);

    /// <summary>
    /// 隐式转换 bool -> TomlBoolean
    /// </summary>
    /// <param name="value">布尔类型</param>

    public static implicit operator TomlNode(bool value) => new TomlBoolean(value);

    /// <summary>
    /// 隐式转换 int -> TomlInteger
    /// </summary>
    /// <param name="value">整型</param>

    public static implicit operator TomlNode(int value) => new TomlInteger(value);

    /// <summary>
    /// 隐式转换 long -> TomlInteger
    /// </summary>
    /// <param name="value">64位整型</param>

    public static implicit operator TomlNode(long value) => new TomlInteger(value);

    /// <summary>
    /// 隐式转换 float -> TomlFloat
    /// </summary>
    /// <param name="value">浮点型</param>

    public static implicit operator TomlNode(float value) => new TomlFloat(value);

    /// <summary>
    /// 隐式转换 double -> TomlFloat
    /// </summary>
    /// <param name="value">双精度浮点</param>

    public static implicit operator TomlNode(double value) => new TomlFloat(value);

    /// <summary>
    /// 隐式转换 DateTime -> TomlDateTimeLocal
    /// </summary>
    /// <param name="value">日期时间</param>
    public static implicit operator TomlNode(DateTime value) => new TomlDateTimeLocal(value);

    /// <summary>
    /// 隐式转换 DateTimeOffset -> TomlDateTimeOffset
    /// </summary>
    /// <param name="value">日期时间偏移量</param>
    public static implicit operator TomlNode(DateTimeOffset value) => new TomlDateTimeOffset(value);

    /// <summary>
    /// 隐式转换 TomlNode[] -> TomlArray
    /// </summary>
    /// <param name="nodes">TomlNode数组</param>
    public static implicit operator TomlNode(TomlNode[] nodes)
    {
        return new TomlArray(nodes);
    }

    /// <summary>
    /// 隐式转换 List&lt;TomlNode&gt; -> TomlArray
    /// </summary>
    /// <param name="nodes">TomlNode数组</param>
    public static implicit operator TomlNode(List<TomlNode> nodes)
    {
        return new TomlArray(nodes);
    }

    #endregion

    #region TOML to native type cast

    /// <summary>
    /// 隐式转换 TomlNode -> string
    /// </summary>
    /// <param name="value">Toml节点</param>
    public static implicit operator string(TomlNode value) => value.ToString()!;

    /// <summary>
    /// 隐式转换 TomlNode -> int
    /// </summary>
    /// <param name="value">Toml节点</param>
    public static implicit operator int(TomlNode value)
    {
        if (value.IsTomlInteger)
            return (int)value.AsTomlInteger.Value;
        else
            return (int)value.AsTomlFloat.Value;
    }

    /// <summary>
    /// 隐式转换 TomlNode -> long
    /// </summary>
    /// <param name="value">Toml节点</param>
    public static implicit operator long(TomlNode value)
    {
        if (value.IsTomlInteger)
            return value.AsTomlInteger.Value;
        else
            return (long)value.AsTomlFloat.Value;
    }

    /// <summary>
    /// 隐式转换 TomlNode -> float
    /// </summary>
    /// <param name="value">Toml节点</param>
    public static implicit operator float(TomlNode value)
    {
        if (value.IsTomlInteger)
            return value.AsTomlInteger.Value;
        else
            return (float)value.AsTomlFloat.Value;
    }

    /// <summary>
    /// 隐式转换 TomlNode -> double
    /// </summary>
    /// <param name="value">Toml节点</param>
    public static implicit operator double(TomlNode value)
    {
        if (value.IsTomlInteger)
            return value.AsTomlInteger.Value;
        else
            return value.AsTomlFloat.Value;
    }

    /// <summary>
    /// 隐式转换 TomlNode -> bool
    /// </summary>
    /// <param name="value">Toml节点</param>
    public static implicit operator bool(TomlNode value) => value.AsTomlBoolean.Value;

    /// <summary>
    /// 隐式转换 TomlNode -> DateTime
    /// </summary>
    /// <param name="value">Toml节点</param>
    public static implicit operator DateTime(TomlNode value) => value.AsTomlDateTimeLocal.Value;

    /// <summary>
    /// 隐式转换 TomlNode -> DateTimeOffset
    /// </summary>
    /// <param name="value">Toml节点</param>
    public static implicit operator DateTimeOffset(TomlNode value) =>
        value.AsTomlDateTimeOffset.Value;

    #endregion
}
