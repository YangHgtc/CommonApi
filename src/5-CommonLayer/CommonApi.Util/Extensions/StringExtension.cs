namespace CommonApi.Util.Extensions;

public static class StringExtension
{
    /// <summary>
    /// 忽略大小写比较，推荐把确定不为空的字符串放在首位。不要使用ToUpper或ToLower方法比较
    /// </summary>
    /// <param name="str1"></param>
    /// <param name="str2"></param>
    /// <returns></returns>
    public static bool EqualsIgnoreCase(this string str1, string str2)
    {
        return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
    }
}
