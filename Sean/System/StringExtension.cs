using System;
using System.Collections.Generic;
using Sean.Utilities;
using System.Linq;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// left cut
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Left(this string input, int length)
        {
            if (input == null) return string.Empty;
            return input.Length <= length ? input : input.Substring(0, length);
        }

        /// <summary>
        /// left cut
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <param name="reminder"></param>
        /// <returns></returns>
        public static string LeftWithReminder(this string input, int length, string reminder = "...")
        {
            if (input == null) return string.Empty;
            if (input.Length <= length) return input;
            return input.Substring(0, length) + reminder;
        }

        /// <summary>
        /// Converts a hexadecimal string into its binary representation.
        /// </summary>
        /// <param name="hexString">The hex string</param>
        /// <returns>The byte array corresponding to the contents of the hex string,
        /// or null if the input string is not a valid hex string.</returns>
        public static byte[] HexToBinary(this string hexString)
        {
            if (hexString == null || hexString.Length % 2 != 0)
            {
                // input string length is not evenly divisible by 2
                return null;
            }

            var binary = new byte[hexString.Length / 2];

            for (var i = 0; i < binary.Length; i++)
            {
                var highNibble = NumberUtil.HexToInt(hexString[2 * i]);
                var lowNibble = NumberUtil.HexToInt(hexString[2 * i + 1]);

                if (highNibble == -1 || lowNibble == -1)
                {
                    return null; // bad hex data
                }
                binary[i] = (byte)((highNibble << 4) | lowNibble);
            }

            return binary;
        }

        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 按照逗号分割字符串，忽略字符串前后和逗号前后的空格
        /// </summary>
        public static string[] SplitByComma(this string str, bool removeEmpty = true)
        {
            var len = str.Length;
            if (len == 0)
            {
                return new string[0];
            }
            string tmp;
            var start = 0;
            var list = new List<string>(16);
            for (var i = 0; i < len; i++)
            {
                if (str[i] == ',')
                {
                    tmp = string.Empty;
                    if (start < i)
                    {
                        tmp = str.Substring(start, i - start).Trim();
                    }
                    if (tmp.Length > 0 || !removeEmpty)
                    {
                        list.Add(tmp);
                    }
                    start = i + 1;
                }
            }

            if (start < len)
            {
                tmp = str.Substring(start, len - start).Trim();
            }
            else
            {
                tmp = string.Empty;
            }

            if (tmp.Length > 0 || !removeEmpty)
            {
                list.Add(tmp);
            }

            return list.ToArray();
        }

        /// <summary>
        /// 如果str为null或空或空白字符串，返回defaultValue，否则调用Convert.ToDecimal返回
        /// </summary>
        public static decimal ToDecimal(this string str, decimal defaultValue)
        {
            if (string.IsNullOrWhiteSpace(str))
                return defaultValue;

            return Convert.ToDecimal(str.Trim());
        }

        /// <summary>
        /// 删除字符串两端空白，如果字符串是null，返回string.empty。
        /// </summary>
        public static string TrimToEmpty(this string str)
        {
            return str?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// 删除字符串两端空白，空字符串转为null返回。
        /// </summary>
        public static string TrimToNull(this string str)
        {
            if (str == null) return null;
            str = str.Trim();
            return str.Length > 0 ? str : null;
        }

        public static string Repeat(this string str, int count, string separator = "")
        {
            if (string.IsNullOrEmpty(str)) return null;
            if (count < 1) return string.Empty;
            return string.Join(separator, Enumerable.Repeat(str, count));
        }
    }
}
