using System;
using System.Collections.Generic;
using System.Text;

namespace Sean.Security
{
    /// <summary>
    /// 数据保护类
    /// </summary>
    public static class DataProtect
    {
        const string magic = "1\t";
        private static string _keyVersion;
        private static readonly Dictionary<string, SimpleAES> _keyDic;

        static DataProtect()
        {
            _keyVersion = string.Empty;
            _keyDic = new Dictionary<string, SimpleAES>();
        }

        /// <summary>
        /// 添加密钥
        /// </summary>
        /// <param name="keyVersion">密钥版本</param>
        /// <param name="secretKey"></param>
        public static void AddSecretKey(string keyVersion, string secretKey)
        {
            if (string.IsNullOrWhiteSpace(keyVersion))
            {
                throw new ArgumentNullException("keyVersion", "keyVersion不能为空");
            }
            _keyDic.Add(keyVersion, new SimpleAES(secretKey, 100));
        }

        public static void SetCurrentKeyVersion(string keyVersion)
        {
            _keyVersion = keyVersion;
        }

        /// <summary>
        /// 数据加密
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static string Encrypt(string data)
        {
            SimpleAES simpleAes;

            if (!_keyDic.TryGetValue(_keyVersion, out simpleAes))
            {
                throw new Exception("没有找到密钥,密钥版本:" + _keyVersion);
            }

            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }

            var bytes = Encoding.UTF8.GetBytes(magic + _keyVersion);
            var header = Base64UrlEncode(bytes);

            bytes = simpleAes.Encrypt(data);
            data = Base64UrlEncode(bytes);

            return header + "." + data;
        }

        /// <summary>
        /// 对加密数据解密
        /// </summary>
        /// <param name="secretData"></param>
        /// <returns></returns>
        public static string Decrypt(string secretData)
        {
            if (string.IsNullOrEmpty(secretData)) return null;

            var index = secretData.IndexOf('.');

            if (index < 0)
            {
                throw new Exception("格式错误:缺少.");
            }

            var header = secretData.Substring(0, index);
            var headerBytes = Base64UrlDecode(header);
            header = Encoding.UTF8.GetString(headerBytes);

            var index2 = header.IndexOf('\t');

            if (index2 < 0)
            {
                throw new Exception("格式错误:头部数据格式");
            }

            var keyVersion = header.Substring(index2 + 1);

            SimpleAES simpleAes;

            if (!_keyDic.TryGetValue(keyVersion, out simpleAes))
            {
                throw new Exception("没有找到密钥,密钥版本:" + keyVersion);
            }

            var payload = secretData.Substring(index + 1);

            var bytes = Base64UrlDecode(payload);

            payload = simpleAes.Decrypt(bytes);

            return payload;
        }

        /// <summary>
        /// 尝试对加密数据解密
        /// </summary>
        /// <param name="secretData"></param>
        /// <param name="decrypted"></param>
        /// <returns></returns>
        public static bool TryDecrypt(string secretData, out string decrypted)
        {
            try
            {
                decrypted = Decrypt(secretData);
                return true;
            }
            catch
            {
                decrypted = null;
                return false;
            }
        }

        /// <remarks>From JWT spec</remarks>
        public static string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0]; // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }

        /// <remarks>From JWT spec</remarks>
        public static byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break;  // One pad char
                default: throw new FormatException("Illegal base64url string!");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }
    }
}
