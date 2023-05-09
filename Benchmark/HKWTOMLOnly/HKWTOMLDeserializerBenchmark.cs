using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using HKW.TOML;
using HKW.TOML.Deserializer;

namespace Benchmark.HKWTOMLOnly;

[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public class HKWTOMLDeserializerBenchmark
{
    [Benchmark]
    public object? HKWTOMLDeserializer()
    {
        var table = TOML.Parse(TomlExample.ExampleData);
        return TomlDeserializer.Deserialize<ClassExample>(table);
    }
}
#if FALSE
1.1.7
|              Method |     Mean |   Error |  StdDev |   Gen0 | Allocated |
|-------------------- |---------:|--------:|--------:|-------:|----------:|
| HKWTOMLDeserializer | 389.6 us | 7.51 us | 9.50 us | 6.3477 |  52.94 KB |
#endif
