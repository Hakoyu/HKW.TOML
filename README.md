# HKWToml

[GitHub](https://github.com/Hakoyu/HKWToml)  
HKWToml 是使用 C# 编写的  
包含有 TOML 的读取和编写,序列化反序列化, TOML 转换为 C# 类功能  
其中 读取器和编写器 修改至[Tommy](https://github.com/dezhidki/Tommy)  
可以添加单独的 TomlParse.cs 文件以只使用读取器和编写器功能  
或者从[NuGet](https://www.nuget.org/packages/HKWToml)包中获取完整功能  

## 特征

- 完全实现 TOML1.0.0 规范
- 对解析和保存注释的基本支持
- 支持.NET 3.5+，Mono，.NET Core！

## 如何使用

### TOML 文件

```toml
# This is a TOML document

# title
# tttttttttttttttt
title = "TOML Example"
# int1
int1 = 123456789
# long1
long1 = 1234567890000
# noop
noop = [{ a = 1, b = 2 }, { b = 2, c = 3 }]

[owner]
# name
name = "Tom Preston-Werner"
date = 2022-07-23
dob = 1979-05-27T07:32:00-08:00

[database]
enabled = true
points = [[1, 2, 3], [4, 5.5, 6], [7, 8, 99999999999999]]
ports = [8000, 8001, 8002]
data = [["delta", "phi"], [3.14]]
temp_targets = { cpu = 79.5, apu = 72.0 }

[servers]

# aaa
[servers.alpha]
ip = "10.0.0.1"
role = "frontend"

# bbb
[servers.beta]
ip = "10.0.0.2"
role = "backend"
```

### 解析 Toml 文件

```csharp
using HKW.TOML
string file = "test.toml";
// 从文件中读取
TomlTable table = TOML.Parse(file);
// 从流中读取
// using(StreamReader reader = File.OpenText("configuration.toml"))
// {
//     TomlTable table = TOML.Parse(reader);
// }

// 获取数据
Console.WriteLine(table["title"]);
Console.WriteLine(table["owner"]["name"]);
// 获取注释
Console.WriteLine(table.Comment);
// 遍历所有数据
foreach(var keyValue in table)
    Console.WriteLine($"Name = {keyValue.Key}, Value = {keyValue.Value}");
```

### 创建 TomlTable

```csharp
using HKW.TOML
string file = "test.toml";
TomlTable toml = new TomlTable
{
    ["title"] = "TOML Example",
    // You can also insert comments before a node with a special property
    ["value-with-comment"] = new TomlString
    {
        Value = "Some value",
        Comment = "This is just some value with a comment"
    },
    // You don't need to specify a type for tables or arrays -- Tommy will figure that out for you
    ["owner"] =
    {
        ["name"] = "Tom Preston-Werner",
        ["dob"] = DateTime.Now
    },
    ["array-table"] = new TomlArray
    {
        // This is marks the array as a TOML array table
        IsTableArray = true,
        [0] =
        {
            ["value"] = 10
        },
        [1] =
        {
            ["value"] = 20
        }
    },
    ["inline-table"] = new TomlTable
    {
        IsInline = true,
        ["foo"] = "bar",
        ["bar"] = "baz",
        // Implicit cast from TomlNode[] to TomlArray
        ["array"] = new TomlNode[] { 1, 2, 3 }
    }
};
toml.SaveTo(file);
```

### 从 TOML 文件 生成 C# 的类

生成相关设置请查看[TomlAsClassesOptions](https://github.com/Hakoyu/HKWToml/blob/master/TOML/TomlAsClassesOptions.cs)

```csharp
using HKW.TOML
string file = "test.toml";
string classString = TomlAsClasses.ConstructFromFile(file, "Test");
Console.WriteLine(classString);
```

### TOML 反序列化

反序列化相关设置请查看[TomlDeserializerOptions](https://github.com/Hakoyu/HKWToml/blob/master/TOML/TomlDeserializerOptions.cs)

```csharp
using HKW.TOML
string file = "test.toml";
Test test = TomlDeserializer.DeserializeFromFile<Test>(file);
```

### TOML 序列化

序列化相关设置请查看[TomlSerializerOptions](https://github.com/Hakoyu/HKWToml/blob/master/TOML/TomlSerializerOptions.cs)

```csharp
using HKW.TOML
string newFile = "newTest.toml";
TomlSerializer.SerializeToFile(test, newFile);
```
