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
public class TomlDeserializerTest
{
    [TestMethod]
    public void Deserialize()
    {
        var example1 = TomlDeserializer.Deserialize<ExampleObject>(
            Example.TomlExampleDataWithoutTime
        );
        var example2 = JsonConvert.DeserializeObject<ExampleObject>(
            Example.JsonExampleDataWithoutTime
        );
        var compareLogic = new CompareLogic();
        compareLogic.Config.MembersToIgnore.AddRange(
            [nameof(ITomlObjectComment.ObjectComment), nameof(ITomlObjectComment.PropertyComments)]
        );
        var result = compareLogic.Compare(example1, example2);
        Assert.IsTrue(result.AreEqual);
    }

    [TestMethod]
    public void DeserializeAsync()
    {
        var example1 = TomlDeserializer
            .DeserializeAsync<ExampleObject>(Example.TomlExampleDataWithoutTime)
            .GetAwaiter()
            .GetResult();
        var example2 = JsonConvert.DeserializeObject<ExampleObject>(
            Example.JsonExampleDataWithoutTime
        );
        var compareLogic = new CompareLogic();
        compareLogic.Config.MembersToIgnore.AddRange(
            [nameof(ITomlObjectComment.ObjectComment), nameof(ITomlObjectComment.PropertyComments)]
        );
        var result = compareLogic.Compare(example1, example2);
        Assert.IsTrue(result.AreEqual);
    }
}
