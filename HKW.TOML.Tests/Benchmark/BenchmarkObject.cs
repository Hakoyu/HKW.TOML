using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKW.HKWTOML.Tests.Benchmark;

public class BenchmarkObject
{
    public Dictionary<string, object> CachedLoadedAndDatedArcs { get; set; }

    public List<MenusListAnonymousClass> MenusList { get; set; }
}

public class MenusListAnonymousClass
{
    public string FileName { get; set; }

    public string PathInMenu { get; set; }

    public int Hash { get; set; }

    public string Name { get; set; }

    public int Version { get; set; }

    public string Icon { get; set; }

    public string Description { get; set; }

    public int Category { get; set; }

    public int ColorSetMPN { get; set; }

    public int MultiColorID { get; set; }

    public bool DelMenu { get; set; }

    public bool ManMenu { get; set; }

    public double Priority { get; set; }

    public bool LegacyMenu { get; set; }

    public string SourceArc { get; set; }

    public string ColorSetMenu { get; set; }
}
