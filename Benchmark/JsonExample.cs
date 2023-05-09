using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark;

internal class JsonExample
{
    public static string ExampleData { get; set; } =
        @"
{
  ""title"": ""TOML example 😊"",
  ""desc"": ""An example TOML document. "",
  ""integers"": [
    42,
    66,
    34,
    6
  ],
  ""floats"": [
    1.42,
    0.01
  ],
  ""duration"": [
    ""4m49s"",
    ""8m03s"",
    ""1231h15m55s""
  ],
  ""distros"": [
    {
      ""name"": ""Arch Linux"",
      ""packages"": ""pacman""
    },
    {
      ""name"": ""Void Linux"",
      ""packages"": ""xbps""
    },
    {
      ""name"": ""Debian"",
      ""packages"": ""apt""
    }
  ],
  ""servers"": {
    ""alpha"": {
      ""ip"": ""10.0.0.1"",
      ""hostname"": ""server1"",
      ""enabled"": false
    },
    ""beta"": {
      ""ip"": ""10.0.0.2"",
      ""hostname"": ""server2"",
      ""enabled"": true
    }
  },
  ""characters"": {
    ""star-trek"": [
      {
        ""name"": ""James Kirk"",
        ""rank"": ""Captain""
      },
      {
        ""name"": ""Spock"",
        ""rank"": ""Science officer""
      }
    ]
  },
  ""undecoded"": {
    ""key"": ""This table intentionally left undecoded""
  }
}
";
}
