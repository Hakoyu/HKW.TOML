using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKW.TOML;

/// <summary>
/// Toml类接口
/// </summary>
public interface ITomlClassComment
{
    /// <summary>
    /// 类注释
    /// </summary>
    public string ClassComment { get; set; }
    /// <summary>
    /// 值注释
    /// </summary>
    public Dictionary<string, string> ValueComments { get; set; }
}
