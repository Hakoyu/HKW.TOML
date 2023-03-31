using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKW.Libs.TOML;

public interface ITomlClass
{
    public string TableComment { get; set; }
    public Dictionary<string, string> ValueComments { get; set; }
}
