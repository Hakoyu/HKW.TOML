using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKW.TOML.Attributes;

/// <summary>
/// 必要属性
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class TomlRequiredAttribute : Attribute { }
