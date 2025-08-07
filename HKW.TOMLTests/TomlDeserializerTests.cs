using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.HKWTOML.Benchmark;
using HKW.HKWTOML.Deserializer;
using HKW.HKWTOML.Interfaces;
using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;

namespace HKW.HKWTOML.Tests;

[TestClass]
public class TomlDeserializerTests
{
    [TestMethod]
    public void Deserialize()
    {
        var example1 = TomlDeserializer.Deserialize<ExampleObject>(
            Example.TomlExampleDataCompatibleJson
        );
        var example2 = JsonConvert.DeserializeObject<ExampleObject>(Example.JsonExampleData);
        var compareLogic = new CompareLogic();
        compareLogic.Config.TypesToIgnore.Add(typeof(TomlNode));
        compareLogic.Config.MembersToIgnore.AddRange(
            [
                nameof(ITomlObjectComment.ObjectComment),
                nameof(ITomlObjectComment.PropertyComments),
                nameof(ExampleObject.Empty_array),
                nameof(ExampleObject.Nested_array),
                nameof(ExampleObject.Mixed_array),
                nameof(ExampleObject.Array_of_arrays)
            ]
        );
        var result = compareLogic.Compare(example1, example2);
        Assert.IsTrue(result.AreEqual);
    }

    [TestMethod]
    public void DeserializeAsync()
    {
        var example1 = TomlDeserializer
            .DeserializeAsync<ExampleObject>(Example.TomlExampleDataCompatibleJson)
            .GetAwaiter()
            .GetResult();
        var example2 = JsonConvert.DeserializeObject<ExampleObject>(Example.JsonExampleData);
        var compareLogic = new CompareLogic();
        compareLogic.Config.TypesToIgnore.Add(typeof(TomlNode));
        compareLogic.Config.MembersToIgnore.AddRange(
            [
                nameof(ITomlObjectComment.ObjectComment),
                nameof(ITomlObjectComment.PropertyComments),
                nameof(ExampleObject.Empty_array),
                nameof(ExampleObject.Nested_array),
                nameof(ExampleObject.Mixed_array),
                nameof(ExampleObject.Array_of_arrays)
            ]
        );
        var result = compareLogic.Compare(example1, example2);
        Assert.IsTrue(result.AreEqual);
    }
}
