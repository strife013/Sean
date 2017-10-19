using System.Diagnostics;

namespace Sean.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class NumberUtil
    {
        /// <summary>
        /// 16进制字符转数字
        /// </summary>
        /// <param name="h">16进制字符,0-9,a-f,A-F</param>
        /// <returns>如果不是0-F,返回-1</returns>
        public static int HexToInt(char h)
        {
            return (h >= '0' && h <= '9') ? h - '0' :
            (h >= 'a' && h <= 'f') ? h - 'a' + 10 :
            (h >= 'A' && h <= 'F') ? h - 'A' + 10 : -1;
        }

        /// <summary>
        /// 转16进制字符
        /// </summary>
        /// <param name="n">n大于等于0并且小于16</param>
        /// <returns></returns>
        public static char IntToHex(int n)
        {
            Debug.Assert(n >= 0 && n < 0x10);

            return n <= 9 ? (char)(n + '0') : (char)(n - 10 + 'a');
        }
    }
}
