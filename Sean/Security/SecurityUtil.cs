using System;
using System.Linq;
using System.Security.Cryptography;

namespace Sean.Security
{
    /// <summary>
    /// 
    /// </summary>
    public static class SecurityUtil
    {
        private static readonly byte[] hmacBytes;

        private static readonly RandomNumberGenerator RandomNumber;

        static SecurityUtil()
        {
            RandomNumber = RandomNumberGenerator.Create();

            hmacBytes = new byte[] { 1, 25, 3, 42, 58, 6, 7, 8, 91, 10, 110, 129, 13, 145, 153, 16 };
        }

        /// <summary>
        /// Helper that generates a random key on each call.
        /// </summary>
        /// <returns></returns>
        public static byte[] NewKey(int byteSize)
        {
            var key = new byte[byteSize];
            RandomNumber.GetBytes(key);
            return key;
        }

        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var data = System.Text.Encoding.UTF8.GetBytes(password);

            using (var hmac = new HMACSHA256(hmacBytes))
            {
                var macBytes = hmac.ComputeHash(data);
                data = data.Concat(macBytes).ToArray();
            }

            return HashHelper.MD5(data).Substring(2, 20);
        }
    }
}
