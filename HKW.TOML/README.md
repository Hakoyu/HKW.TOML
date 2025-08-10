# HKW.TOML

[GitHub](https://github.com/Hakoyu/HKW.TOML)  
是使用 C# 编写的高性能 Toml 解析器
包含有 TOML 的读取和编写, 序列化反序列化, 生成 C# 实体类等功能  
其中 读取器和编写器 修改至[Tommy](https://github.com/dezhidki/Tommy)  
可以添加单独的 TomlParse.cs 文件以只使用读取器和编写器功能  
或者从[NuGet](https://www.nuget.org/packages/HKW.TOML)包中获取完整功能

## 特征

- 完全实现 TOML 1.0.0 规范
- 支持实现使用 `ITomlObjectComment` 读取并修改注释
- 支持.NET 8+ !

## 如何使用

[测试文件](https://github.com/Hakoyu/HKW.TOML/blob/master/HKW.TOMLBenchmark/Example.toml)
[测试类](https://github.com/Hakoyu/HKW.TOML/blob/master/HKW.TOMLBenchmark/ExampleObject.cs)

### [解析 Toml 文件](https://github.com/Hakoyu/HKW.TOML/blob/master/HKW.TOMLTests/TomlParseTests.cs)

```csharp
TomlTable table = TOML.Parse(TomlExample.ExampleData);
//TomlTable table = TOML.ParseFromFile(TomlExample.ExampleFile);

string title = table["title"].AsString;
string titleComment = table["title"].Comment;
List<int> numbers = table["integers"].AsList.Select(node => node.AsInt32).ToList();
```

### 创建 TomlTable

```csharp
TomlTable table =
    new()
    {
        new TomlString("TOML example \\U0001F60A")
        {
            Comment = "Simple key/value with a string."
        },
        new TomlArray() { 42, 0x42, 042, 0b0110 }
    };
```

### [从 TOML 文件 生成 C# 的类](https://github.com/Hakoyu/HKW.TOML/blob/master/HKW.TOMLTests/TomlObjectGeneratorTests.cs)

生成相关设置请查看[TomlObjectGeneratorOptions](https://github.com/Hakoyu/HKW.TOML/blob/master/HKW.TOML/ObjectGenerator/TomlObjectGenerator.cs)

```csharp
TomlTable table = TOML.Parse(TomlExample.ExampleData);
string classString = TomlObjectGenerator.Generate(table, "ClassExample");
Console.WriteLine(classString);
```

### [TOML 反序列化](https://github.com/Hakoyu/HKW.TOML/blob/master/HKW.TOMLTests/TomlDeserializerTests.cs)

反序列化相关设置请查看[TomlDeserializerOptions](https://github.com/Hakoyu/HKW.TOML/blob/master/HKW.TOML/Deserializer/TomlDeserializerOptions.cs)

```csharp
TomlTable table = TOML.Parse(TomlExample.ExampleData);
ClassExample example = TomlDeserializer.Deserialize<ClassExample>(table);
```

### [TOML 序列化](https://github.com/Hakoyu/HKW.TOML/blob/master/HKW.TOMLTests/TomlSerializerTests.cs)

序列化相关设置请查看[TomlSerializerOptions](https://github.com/Hakoyu/HKW.TOML/blob/master/HKW.TOML/Serializer/TomlSerializerOptions.cs)

```csharp
TomlTable table = TOML.Parse(TomlExample.ExampleData);
ClassExample example = TomlDeserializer.Deserialize<ClassExample>(table);
TomlTable serializeTable = TomlSerializer.Serialize(example);
```
