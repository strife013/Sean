using System.Security.Cryptography;
using System.Text;
using Sean.Utilities;

namespace Sean.Security
{
    /// <summary>
    /// hash util
    /// </summary>
    public static class HashHelper
    {
        /// <summary>
        /// md5 hash
        /// </summary>
        public static string MD5(string input, bool lowerCase = true)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            using (var hashAlgorithm = new MD5Cng())
            {
                var hexString = BinaryUtil.ToHex(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input)));
                return lowerCase ? hexString.ToLowerInvariant() : hexString;
            }
        }

        /// <summary>
        /// md5 hash
        /// </summary>
        public static string MD5(string input, int iterations, bool lowerCase = true)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            if (iterations < 1)
            {
                iterations = 1;
            }

            var bytes1 = Encoding.UTF8.GetBytes(input);
            var bytes2 = new byte[] { 9, 81 };

            using (var hashAlgorithm = new MD5Cng())
            {
                for (var i = 0; i < iterations; i++)
                {
                    var bytes = new byte[bytes1.Length + bytes2.Length];
                    bytes1.CopyTo(bytes, 0);
                    bytes2.CopyTo(bytes, bytes1.Length);
                    bytes2 = hashAlgorithm.ComputeHash(bytes);
                }
                var hexString = BinaryUtil.ToHex(bytes2);
                return lowerCase ? hexString.ToLowerInvariant() : hexString;
            }
        }

        /// <summary>
        /// md5 hash
        /// </summary>
        public static string MD5(byte[] buffer, bool lowerCase = true)
        {
            if (buffer == null || buffer.Length == 0)
                return string.Empty;

            using (var hashAlgorithm = new MD5Cng())
            {
                var hexString = BinaryUtil.ToHex(hashAlgorithm.ComputeHash(buffer));
                return lowerCase ? hexString.ToLowerInvariant() : hexString;
            }
        }

        /// <summary>
        /// sha1 hash
        /// </summary>
        /// <param name="input"></param>
        /// <param name="lowerCase"></param>
        /// <returns></returns>
        public static string SHA1(string input, bool lowerCase = true)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            using (var hashAlgorithm = new SHA1Cng())
            {
                var hexString = BinaryUtil.ToHex(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input)));
                return lowerCase ? hexString.ToLowerInvariant() : hexString;
            }
        }
    }
}
