using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Sean.Security
{
    public class StrongAES
    {
        //Preconfigured Encryption Parameters
        private const int BlockBitSize = 128;
        private const int KeyBitSize = 256;

        //Preconfigured Password Key Derivation Parameters
        private const int SaltBitSize = 64;
        private const int MinPasswordLength = 12;

        private string _password;
        private int _iterations;
        private byte[] _nonSecretPayload;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretKey">密钥</param>
        /// <param name="nonSecretPayload">无加密负载</param>
        /// <param name="iterations">密钥迭代次数</param>
        public StrongAES(string secretKey, byte[] nonSecretPayload = null, int iterations = 100)
        {
            if (string.IsNullOrWhiteSpace(secretKey) || secretKey.Length < MinPasswordLength)
                throw new ArgumentException(string.Format("Must have a password of at least {0} characters!", MinPasswordLength), "password");
            if (nonSecretPayload == null)
            {
                nonSecretPayload = new byte[] { 77, 199, 35, 46 };
            }
            if (iterations < 0)
            {
                iterations = 10;
            }
            _password = secretKey;
            _iterations = iterations;
            _nonSecretPayload = nonSecretPayload;
        }

        #region ----加密----

        /// <summary>
        /// Simple Encryption (AES) then Authentication (HMAC) of a UTF8 message
        /// using Keys derived from a Password (PBKDF2).
        /// </summary>
        /// <param name="input">The secret message.</param>
        /// <returns>
        /// Encrypted Message
        /// </returns>
        /// <exception cref="System.ArgumentException">password</exception>
        /// <remarks>
        /// Significantly less secure than using random binary keys.
        /// Adds additional non secret payload for key generation parameters.
        /// </remarks>
        public string Encrypt(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException("input");

            var messageBytes = Encoding.UTF8.GetBytes(input);

            var payload = new byte[SaltBitSize / 8 * 2 + _nonSecretPayload.Length];

            Array.Copy(_nonSecretPayload, payload, _nonSecretPayload.Length);

            var payloadIndex = _nonSecretPayload.Length;

            byte[] cryptKey;
            byte[] authKey;
            //Use Random Salt to prevent pre-generated weak password attacks.
            using (var generator = new Rfc2898DeriveBytes(_password, SaltBitSize / 8, _iterations))
            {
                var salt = generator.Salt;

                //Generate Keys
                cryptKey = generator.GetBytes(KeyBitSize / 8);

                //Create Non Secret Payload
                Array.Copy(salt, 0, payload, payloadIndex, salt.Length);
                payloadIndex += salt.Length;
            }

            //Deriving separate key, might be less efficient than using HKDF, 
            //but now compatible with RNEncryptor which had a very similar wireformat and requires less code than HKDF.
            using (var generator = new Rfc2898DeriveBytes(_password, SaltBitSize / 8, _iterations))
            {
                var salt = generator.Salt;

                //Generate Keys
                authKey = generator.GetBytes(KeyBitSize / 8);

                //Create Rest of Non Secret Payload
                Array.Copy(salt, 0, payload, payloadIndex, salt.Length);
            }

            var cipherBytes = Encrypt(messageBytes, cryptKey, authKey, payload);

            return Convert.ToBase64String(cipherBytes);
        }

        private static byte[] Encrypt(byte[] inputBytes, byte[] cryptKey, byte[] authKey, byte[] nonSecretPayload = null)
        {
            if (inputBytes == null || inputBytes.Length < 1)
                throw new ArgumentNullException("inputBytes");

            if (cryptKey == null || cryptKey.Length != KeyBitSize / 8)
                throw new ArgumentException(string.Format("Key needs to be {0} bit!", KeyBitSize), "cryptKey");

            if (authKey == null || authKey.Length != KeyBitSize / 8)
                throw new ArgumentException(string.Format("Key needs to be {0} bit!", KeyBitSize), "authKey");

            //non-secret payload optional
            nonSecretPayload = nonSecretPayload ?? new byte[0];

            byte[] cipherText;
            byte[] iv;

            using (var aes = new AesManaged
            {
                KeySize = KeyBitSize,
                BlockSize = BlockBitSize,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            })
            {

                //Use random IV
                aes.GenerateIV();
                iv = aes.IV;

                using (var encrypter = aes.CreateEncryptor(cryptKey, iv))
                using (var cipherStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(cipherStream, encrypter, CryptoStreamMode.Write))
                    using (var binaryWriter = new BinaryWriter(cryptoStream))
                    {
                        //Encrypt Data
                        binaryWriter.Write(inputBytes);
                    }

                    cipherText = cipherStream.ToArray();
                }

            }

            //Assemble encrypted message and add authentication
            using (var hmac = new HMACSHA256(authKey))
            using (var encryptedStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(encryptedStream))
                {
                    //Prepend non-secret payload if any
                    binaryWriter.Write(nonSecretPayload);
                    //Prepend IV
                    binaryWriter.Write(iv);
                    //Write Ciphertext
                    binaryWriter.Write(cipherText);
                    binaryWriter.Flush();

                    //Authenticate all data
                    var tag = hmac.ComputeHash(encryptedStream.ToArray());
                    //Postpend tag
                    binaryWriter.Write(tag);
                }
                return encryptedStream.ToArray();
            }

        }

        #endregion

        #region ----解密----

        /// <summary>
        /// Simple Authentication (HMAC) and then Descryption (AES) of a UTF8 Message
        /// using keys derived from a password (PBKDF2). 
        /// </summary>
        /// <param name="secret">The encrypted message.</param>
        /// <returns>
        /// Decrypted Message
        /// </returns>
        /// <exception cref="System.ArgumentException">Encrypted Message Required!;encryptedMessage</exception>
        /// <remarks>
        /// Significantly less secure than using random binary keys.
        /// </remarks>
        public string Decrypt(string secret)
        {
            if (string.IsNullOrWhiteSpace(secret))
                throw new ArgumentException("Encrypted Message Required!", "secret");

            var cipherBytes = Convert.FromBase64String(secret);
            var plainBytes = Decrypt(cipherBytes);
            return plainBytes == null ? null : Encoding.UTF8.GetString(plainBytes);
        }

        /// <summary>
        /// Simple Authentication (HMAC) and then Descryption (AES) of a UTF8 Message
        /// using keys derived from a password (PBKDF2). 
        /// </summary>
        /// <param name="secret">The encrypted message.</param>
        /// <returns>
        /// Decrypted Message
        /// </returns>
        /// <exception cref="System.ArgumentException">Must have a password of minimum length;password</exception>
        /// <remarks>
        /// Significantly less secure than using random binary keys.
        /// </remarks>
        public byte[] Decrypt(byte[] secret)
        {
            if (secret == null || secret.Length == 0)
                throw new ArgumentException("Encrypted Message Required!", "secret");

            var cryptSalt = new byte[SaltBitSize / 8];
            var authSalt = new byte[SaltBitSize / 8];

            //Grab Salt from Non-Secret Payload
            Array.Copy(secret, _nonSecretPayload.Length, cryptSalt, 0, cryptSalt.Length);
            Array.Copy(secret, _nonSecretPayload.Length + cryptSalt.Length, authSalt, 0, authSalt.Length);

            byte[] cryptKey;
            byte[] authKey;

            //Generate crypt key
            using (var generator = new Rfc2898DeriveBytes(_password, cryptSalt, _iterations))
            {
                cryptKey = generator.GetBytes(KeyBitSize / 8);
            }
            //Generate auth key
            using (var generator = new Rfc2898DeriveBytes(_password, authSalt, _iterations))
            {
                authKey = generator.GetBytes(KeyBitSize / 8);
            }

            return Decrypt(secret, cryptKey, authKey, cryptSalt.Length + authSalt.Length + _nonSecretPayload.Length);
        }

        private static byte[] Decrypt(byte[] secret, byte[] cryptKey, byte[] authKey, int nonSecretPayloadLength = 0)
        {
            //Basic Usage Error Checks
            if (cryptKey == null || cryptKey.Length != KeyBitSize / 8)
                throw new ArgumentException(string.Format("CryptKey needs to be {0} bit!", KeyBitSize), "cryptKey");

            if (authKey == null || authKey.Length != KeyBitSize / 8)
                throw new ArgumentException(string.Format("AuthKey needs to be {0} bit!", KeyBitSize), "authKey");

            if (secret == null || secret.Length == 0)
                throw new ArgumentException("Encrypted Message Required!", "secret");

            using (var hmac = new HMACSHA256(authKey))
            {
                var sentTag = new byte[hmac.HashSize / 8];
                //Calculate Tag
                var calcTag = hmac.ComputeHash(secret, 0, secret.Length - sentTag.Length);
                var ivLength = BlockBitSize / 8;

                //if message length is to small just return null
                if (secret.Length < sentTag.Length + nonSecretPayloadLength + ivLength)
                    return null;

                //Grab Sent Tag
                Array.Copy(secret, secret.Length - sentTag.Length, sentTag, 0, sentTag.Length);

                //Compare Tag with constant time comparison
                var compare = 0;
                for (var i = 0; i < sentTag.Length; i++)
                    compare |= sentTag[i] ^ calcTag[i];

                //if message doesn't authenticate return null
                if (compare != 0)
                    return null;

                using (var aes = new AesManaged
                {
                    KeySize = KeyBitSize,
                    BlockSize = BlockBitSize,
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                })
                {

                    //Grab IV from message
                    var iv = new byte[ivLength];
                    Array.Copy(secret, nonSecretPayloadLength, iv, 0, iv.Length);

                    using (var decrypter = aes.CreateDecryptor(cryptKey, iv))
                    using (var plainTextStream = new MemoryStream())
                    {
                        using (var decrypterStream = new CryptoStream(plainTextStream, decrypter, CryptoStreamMode.Write))
                        using (var binaryWriter = new BinaryWriter(decrypterStream))
                        {
                            //Decrypt Cipher Text from Message
                            binaryWriter.Write(
                                secret,
                                nonSecretPayloadLength + iv.Length,
                                secret.Length - nonSecretPayloadLength - iv.Length - sentTag.Length
                            );
                        }
                        //Return Plain Text
                        return plainTextStream.ToArray();
                    }
                }
            }
        }

        #endregion
    }
}
