using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.HKWTOML.Benchmark;
using HKW.HKWTOML.Deserializer;
using HKW.HKWTOML.Interfaces;
using HKW.HKWTOML.Serializer;
using KellermanSoftware.CompareNetObjects;

namespace HKW.HKWTOML.Tests;

[TestClass]
public class TomlSerializerTests
{
    [TestMethod]
    public void Serialize()
    {
        var example = TomlDeserializer.Deserialize<ExampleObject>(Example.TomlExampleData);
        var serializeTable = TomlSerializer.Serialize(example);
        var serializeTableString = serializeTable!.ToTomlString();
        var newExample = TomlDeserializer.Deserialize<ExampleObject>(serializeTableString);
        var compareLogic = new CompareLogic();
        compareLogic.Config.TypesToIgnore.AddRange(
            [typeof(TomlNode), typeof(EmptyInlineTableClass)]
        );
        compareLogic.Config.MembersToIgnore.AddRange(
            [
                nameof(ITomlObjectComment.ObjectComment),
                nameof(ITomlObjectComment.PropertyComments),
                nameof(ExampleObject.EmptyArray),
                nameof(ExampleObject.NestedArray),
                nameof(ExampleObject.MixedArray),
                nameof(ExampleObject.ArrayOfArrays),
                nameof(ExampleObject.PreciseDatetime)
            ]
        );
        var result = compareLogic.Compare(newExample, example);
        Assert.IsTrue(result.AreEqual);
        //Console.WriteLine(serializeTable.ToTomlString());
    }

    [TestMethod]
    public void SerializeAsync()
    {
        var example = TomlDeserializer.Deserialize<ExampleObject>(Example.TomlExampleData);
        var serializeTable = TomlSerializer.SerializeAsync(example).GetAwaiter().GetResult();
        var serializeTableString = serializeTable!.ToTomlString();
        var newExample = TomlDeserializer.Deserialize<ExampleObject>(serializeTableString);
        var compareLogic = new CompareLogic();
        compareLogic.Config.TypesToIgnore.AddRange(
            [typeof(TomlNode), typeof(EmptyInlineTableClass)]
        );
        compareLogic.Config.MembersToIgnore.AddRange(
            [
                nameof(ITomlObjectComment.ObjectComment),
                nameof(ITomlObjectComment.PropertyComments),
                nameof(ExampleObject.EmptyArray),
                nameof(ExampleObject.NestedArray),
                nameof(ExampleObject.MixedArray),
                nameof(ExampleObject.ArrayOfArrays),
                nameof(ExampleObject.PreciseDatetime)
            ]
        );
        var result = compareLogic.Compare(newExample, example);
        Assert.IsTrue(result.AreEqual);
        //Console.WriteLine(serializeTable.ToTomlString());
    }
}
