namespace Sean.Utilities
{
    /// <summary>
    /// binary util
    /// </summary>
    public static class BinaryUtil
    {
        /// <summary>
        /// Converts a byte array into its hexadecimal representation.
        /// </summary>
        /// <param name="data">The binary byte array.</param>
        /// <returns>The hexadecimal (uppercase) equivalent of the byte array.</returns>
        public static string ToHex(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            var hex = new char[checked(data.Length * 2)];

            for (var i = 0; i < data.Length; i++)
            {
                var thisByte = data[i];
                hex[2 * i] = NibbleToHex((byte)(thisByte >> 4)); // high nibble
                hex[2 * i + 1] = NibbleToHex((byte)(thisByte & 0xf)); // low nibble
            }

            return new string(hex);
        }

        // converts a nibble (4 bits) to its uppercase hexadecimal character representation [0-9, A-F]
        private static char NibbleToHex(byte nibble)
        {
            return (char)((nibble < 10) ? (nibble + '0') : (nibble - 10 + 'A'));
        }
    }
}
