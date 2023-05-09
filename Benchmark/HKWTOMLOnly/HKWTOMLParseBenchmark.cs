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
public class HKWTOMLParseBenchmark
{
    [Benchmark]
    public object? HKWTOMLParse()
    {
        return TOML.Parse(TomlExample.ExampleData);
    }
}
#if FALSE
1.1.7
|       Method |     Mean |    Error |   StdDev |   Gen0 |   Gen1 | Allocated |
|------------- |---------:|---------:|---------:|-------:|-------:|----------:|
| HKWTOMLParse | 30.36 us | 0.569 us | 0.584 us | 3.4180 | 0.0610 |  28.25 KB |
#endif
