using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Sean.Security
{
    public class RSACrypter
    {
        private bool hasKey;
        private BigInteger exponent;
        private BigInteger modulus;

        public RSACrypter SetKey(string key)
        {
            byte[] b1, b2;
            ResolveKey(key, out b1, out b2);
            exponent = new BigInteger(b1);
            modulus = new BigInteger(b2);
            hasKey = true;
            return this;
        }

        public RSACrypter SetKeyFile(string keyFile)
        {
            SetKey(System.IO.File.ReadAllText(keyFile));
            return this;
        }

        #region 加密/解密数据

        private byte[] Transform(byte[] data)
        {
            if (!hasKey)
                throw new Exception("please set key");
            if (data == null || data.Length == 0)
                throw new ArgumentNullException("data");
            var encData = new BigInteger(data);
            var bnData = BigInteger.ModPow(encData, exponent, modulus);
            return bnData.ToByteArray();
        }

        public string Encrypt(byte[] data)
        {
            var bytes = Transform(data);
            return Convert.ToBase64String(bytes);
        }

        public string Encrypt(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            bytes = Transform(bytes);
            return Convert.ToBase64String(bytes);
        }

        public string Decrypt(byte[] data)
        {
            var bytes = Transform(data);
            return Encoding.UTF8.GetString(bytes);
        }

        public string Decrypt(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            bytes = Transform(bytes);
            return Encoding.UTF8.GetString(bytes);
        }

        #endregion

        #region 静态帮助方法

        /// <summary>
        /// RSA加密的密匙结构  公钥和私匙
        /// </summary>
        public struct RSAKey
        {
            public string PublicKey { get; set; }
            public string PrivateKey { get; set; }
        }

        /// <summary>
        /// 得到RSA的解谜的密匙对
        /// </summary>
        /// <returns></returns>
        public static RSAKey GenerateKey()
        {
            //声明一个指定大小的RSA容器
            using (var rsa = new RSACryptoServiceProvider())
            {
                //取得RSA容易里的各种参数
                var p = rsa.ExportParameters(true);
                return new RSAKey()
                {
                    PublicKey = ComponentKey(p.Exponent, p.Modulus),
                    PrivateKey = ComponentKey(p.D, p.Modulus)
                };
            }
        }

        /// <summary>
        /// 组合成密匙字符串
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        private static string ComponentKey(byte[] b1, byte[] b2)
        {
            var list = new List<byte>();
            //在前端加上第一个数组的长度值 这样今后可以根据这个值分别取出来两个数组
            list.Add((byte)b1.Length);
            list.AddRange(b1);
            list.AddRange(b2);
            var b = list.ToArray<byte>();
            return Convert.ToBase64String(b);
        }

        /// <summary>
        /// 解析密匙
        /// </summary>
        /// <param name="key">密匙</param>
        /// <param name="b1">RSA的相应参数1</param>
        /// <param name="b2">RSA的相应参数2</param>
        protected static void ResolveKey(string key, out byte[] b1, out byte[] b2)
        {
            //从base64字符串 解析成原来的字节数组
            var b = Convert.FromBase64String(key);
            //初始化参数的数组长度
            b1 = new byte[b[0]];
            b2 = new byte[b.Length - b[0] - 1];
            //将相应位置是值放进相应的数组
            for (int n = 1, i = 0, j = 0; n < b.Length; n++)
            {
                if (n <= b[0])
                {
                    b1[i++] = b[n];
                }
                else
                {
                    b2[j++] = b[n];
                }
            }
        }

        #endregion
    }
}
