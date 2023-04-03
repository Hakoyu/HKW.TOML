using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using HKW.TOML;

namespace HKWToml
{
    [SimpleJob(RuntimeMoniker.Net60)]
    [MemoryDiagnoser]
    internal class Benchmark
    {
        private static string s_tomlFile = "C:\\Users\\HKW\\Desktop\\Dotnet\\test.toml";
        private static string s_outTomlFile = "C:\\Users\\HKW\\Desktop\\Dotnet\\test1.toml";

        [Benchmark]
        public void Test()
        {
            var test = TomlDeserializer.DeserializeFromFile<Test>(s_tomlFile);
            TomlSerializer.SerializeToFile(test, s_outTomlFile);
        }

        [Benchmark]
        public async Task TestAsync()
        {
            var test = await TomlDeserializer.DeserializeFromFileAsync<Test>(s_tomlFile);
            await TomlSerializer.SerializeToFileAsync(test, s_outTomlFile);
        }
    }
}
