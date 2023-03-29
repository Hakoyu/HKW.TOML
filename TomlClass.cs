using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace HKW.Libs.TOML;

[DebuggerDisplay("{Name},Count = {Count}")]
public class TomlClass : IDictionary<string, TomlClassValue>
{
    public string Name { get; private set; }
    public string ParentName { get; private set; }

    private readonly Dictionary<string, TomlClassValue> _baseDictionary = new();

    public TomlClass(string name, string parentName = "")
    {
        Name = name;
        ParentName = parentName;
    }

    public override string ToString()
    {
        var sb = new StringBuilder($"public class {Name}\n{{\n");
        foreach (var item in _baseDictionary.Values)
            sb.AppendLine(item.ToString());
        sb.AppendLine("}");
        return sb.ToString();
    }

    public TomlClassValue this[string key]
    {
        get => _baseDictionary[key];
        set => _baseDictionary[key] = value;
    }

    public ICollection<string> Keys => _baseDictionary.Keys;

    public ICollection<TomlClassValue> Values => _baseDictionary.Values;

    public int Count => _baseDictionary.Count;

    public bool IsReadOnly =>
        ((ICollection<KeyValuePair<string, TomlClassValue>>)_baseDictionary).IsReadOnly;

    public void Add(string key, TomlClassValue value) => _baseDictionary.Add(key, value);

    public void Add(KeyValuePair<string, TomlClassValue> item) =>
        ((ICollection<KeyValuePair<string, TomlClassValue>>)_baseDictionary).Add(item);

    public void Clear() => _baseDictionary.Clear();

    public bool Contains(KeyValuePair<string, TomlClassValue> item) => _baseDictionary.Contains(item);

    public bool ContainsKey(string key) => _baseDictionary.ContainsKey(key);

    public void CopyTo(KeyValuePair<string, TomlClassValue>[] array, int arrayIndex) =>
        ((ICollection<KeyValuePair<string, TomlClassValue>>)_baseDictionary).CopyTo(array, arrayIndex);

    public IEnumerator<KeyValuePair<string, TomlClassValue>> GetEnumerator() =>
        _baseDictionary.GetEnumerator();

    public bool Remove(string key) => _baseDictionary.Remove(key);

    public bool Remove(KeyValuePair<string, TomlClassValue> item) =>
        ((ICollection<KeyValuePair<string, TomlClassValue>>)_baseDictionary).Remove(item);

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out TomlClassValue value) =>
        _baseDictionary.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_baseDictionary).GetEnumerator();
}
