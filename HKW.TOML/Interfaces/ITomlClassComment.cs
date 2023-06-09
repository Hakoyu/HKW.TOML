﻿using System.Collections.Generic;

namespace HKW.HKWTOML.Interfaces;

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
