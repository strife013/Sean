using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Sean.Security
{
    /// <summary>
    /// 简单AES加密
    /// </summary>
    public class SimpleAES
    {
        private static readonly byte[] SaltBytes = { 11, 29, 37, 43, 50, 66, 7, 80 };

        private readonly byte[] _keyBytes, _ivBytes;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="secretKey">密钥</param>
        /// <param name="iterations">密钥迭代次数</param>
        public SimpleAES(string secretKey, int iterations = 1000)
        {
            if (string.IsNullOrWhiteSpace(secretKey))
                throw new ArgumentNullException("password");

            if (iterations < 0)
            {
                iterations = 10;
            }

            using (var kdf = new Rfc2898DeriveBytes(secretKey, SaltBytes, iterations))
            {
                var bytes = kdf.GetBytes(32);
                _keyBytes = bytes.Skip(3).Take(16).ToArray();
                _ivBytes = bytes.Skip(16).ToArray();
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public byte[] Encrypt(string input)
        {
            var data = Encoding.UTF8.GetBytes(input);
            var packed = EncryptBytes(_keyBytes, data);
            return AddMac(_ivBytes, packed);
        }

        /// <summary>
        /// 加密并返回BASE64结果
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string EncryptToBase64String(string input)
        {
            var bytes = Encrypt(input);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="secret"></param>
        /// <returns></returns>
        public string Decrypt(byte[] secret)
        {
            var packed = RemoveMac(_ivBytes, secret);
            var plaintext = DecryptBytes(_keyBytes, packed);
            return Encoding.UTF8.GetString(plaintext);
        }

        #region 静态帮助方法

        private static byte[] EncryptBytes(byte[] key, byte[] data)
        {
            using (var cipher = new RijndaelManaged { Key = key })
            {
                using (var encryptor = cipher.CreateEncryptor())
                {
                    var ciphertext = encryptor.TransformFinalBlock(data, 0, data.Length);

                    // IV is prepended to ciphertext
                    return cipher.IV.Concat(ciphertext).ToArray();
                }
            }
        }

        private static byte[] DecryptBytes(byte[] key, byte[] packed)
        {
            using (var cipher = new RijndaelManaged { Key = key })
            {
                var ivSize = cipher.BlockSize / 8;

                cipher.IV = packed.Take(ivSize).ToArray();

                using (var encryptor = cipher.CreateDecryptor())
                {
                    return encryptor.TransformFinalBlock(packed, ivSize, packed.Length - ivSize);
                }
            }
        }

        private static byte[] AddMac(byte[] key, byte[] data)
        {
            using (var hmac = new HMACSHA256(key))
            {
                var macBytes = hmac.ComputeHash(data);

                // HMAC is appended to data
                return data.Concat(macBytes).ToArray();
            }
        }

        private static bool BadMac(byte[] found, byte[] computed)
        {
            var mismatch = 0;

            // Aim for consistent timing regardless of inputs
            for (var i = 0; i < found.Length; i++)
            {
                mismatch += found[i] == computed[i] ? 0 : 1;
            }

            return mismatch != 0;
        }

        private static byte[] RemoveMac(byte[] key, byte[] data)
        {
            using (var hmac = new HMACSHA256(key))
            {
                var macSize = hmac.HashSize / 8;

                var packed = data.Take(data.Length - macSize).ToArray();

                var foundMac = data.Skip(packed.Length).ToArray();

                var computedMac = hmac.ComputeHash(packed);

                if (BadMac(foundMac, computedMac))
                {
                    throw new Exception("Bad MAC");
                }

                return packed;
            }
        }

        #endregion
    }
}
