namespace CommonApi.Util.Extensions;

public static class StringExtension
{
    /// <summary>
    ///     忽略大小写比较
    /// </summary>
    /// <param name="str1"></param>
    /// <param name="str2"></param>
    /// <returns></returns>
    /// <remarks>
    ///     推荐把确定不为空的字符串放在首位。不要使用ToUpper或ToLower方法比较
    /// </remarks>
    public static bool EqualsIgnoreCase(this string str1, string str2)
    {
        return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///     从字符串中提取数字字符串
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string GetNumberFromString(this string str)
    {
        return new string(str.Where(char.IsDigit).ToArray());
    }

    /// <summary>
    ///     通过substring获取字符串的数字
    /// </summary>
    /// <param name="str1"></param>
    /// <param name="start">起始位置</param>
    /// <param name="length">分割的长度</param>
    /// <returns></returns>
    public static int GetNumberBySubString(this string str1, int start, int length)
    {
        return int.TryParse(str1.AsSpan(start, length), out var res) ? res : 0;
    }
}
