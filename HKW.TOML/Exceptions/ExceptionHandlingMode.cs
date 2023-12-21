namespace HKW.HKWTOML.Exceptions;

/// <summary>
/// 异常处理方式
/// </summary>
public enum ExceptionHandlingMode
{
    /// <summary>
    /// 抛出
    /// </summary>
    Throw,

    /// <summary>
    /// 忽视
    /// </summary>
    Ignore,

    /// <summary>
    /// 记录
    /// </summary>
    Record
}
