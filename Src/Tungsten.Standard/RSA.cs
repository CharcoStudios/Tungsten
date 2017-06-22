using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using W.Logging;

namespace W.Encryption
{
    /// <summary>
    /// The public and private RSA keys
    /// </summary>
    public class PublicPrivateKeyPair
    {
        /// <summary>
        /// The private RSA key.  Should never be shared.
        /// </summary>
        public RSAParameters PrivateKey { get; set; }
        /// <summary>
        /// the public RSA key. Should be shared.
        /// </summary>
        public RSAParameters PublicKey { get; set; }
    }
    /// <summary>
    /// Provides RSA encryption functionality
    /// </summary>
    /// <remarks>
    /// Adapted from an online sample http://digitalsquid.co.uk/2009/01/rsa-crypto/
    /// </remarks>
    public class RSA : IDisposable
    {
        private System.Security.Cryptography.RSA _rsa = System.Security.Cryptography.RSA.Create();
        private int _keySize;

        /// <summary>
        /// The private key used to decrypt data(do not share)
        /// </summary>
        public RSAParameters PrivateKey { get; set; }
        /// <summary>
        /// The public key used to encrypt data (should be shared)
        /// </summary>
        public RSAParameters PublicKey { get; set; }

        /// <summary>
        /// Gets the key sizes that are supported by the asymmetric algorithm
        /// </summary>
        /// <returns>An enumeration of the supported key sizes</returns>
        public IEnumerable<KeySizes> LegalKeySizes
        {
            get
            {
                foreach (var value in _rsa.LegalKeySizes)
                    yield return value;
            }
        }

        /// <summary>
        /// constructs a new RSA object
        /// </summary>
        public RSA(int keySize = -1)
        {
            _keySize = keySize != -1 ? keySize : _rsa.KeySize;
            //foreach(var s in _rsa.LegalKeySizes)
            //    Log.i("Min:{0}, Max:{1}, Skip:{2}", s.MinSize, s.MaxSize, s.SkipSize);
            _rsa.KeySize = _keySize;
            PrivateKey = _rsa.ExportParameters(true);
            PublicKey = _rsa.ExportParameters(false);
        }

        /// <summary>
        /// Destructs the RSA object and calls Dispose 
        /// </summary>
        ~RSA()
        {
            Dispose();
        }

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="text">The string to encrypt</param>
        /// <returns>A string containing the encrypted value</returns>
        public string Encrypt(string text)
        {
            return Encrypt(text.AsBytes(), PublicKey);
        }
        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="text">The string to encrypt</param>
        /// <param name="publicKey">The public key used to encrypt the string</param>
        /// <returns>A string containing the encrypted value</returns>
        public string Encrypt(string text, RSAParameters publicKey)
        {
            return Encrypt(text.AsBytes(), publicKey);
        }
        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="byteData">The data to encrypt</param>
        /// <param name="publicKey">The public key used to encrypt the data</param>
        /// <returns>A string containing the encrypted data</returns>
        public string Encrypt(byte[] byteData, RSAParameters publicKey)
        {
            //Log.v("Encryption Hash={0}", MD5.GetMd5Hash(publicKey.AsXml<RSAParameters>()));

            _rsa.ImportParameters(publicKey);
            //byte[] byteData = Encoding.UTF32.GetBytes(text);
            const int maxLength = 214;
            int dataLength = byteData.Length;
            int iterations = dataLength / maxLength;

            var sb = new StringBuilder();
            for (int i = 0; i <= iterations; i++)
            {
                var tempBytes = new byte[(dataLength - maxLength * i > maxLength) ? maxLength : dataLength - maxLength * i];
                Buffer.BlockCopy(byteData, maxLength * i, tempBytes, 0, tempBytes.Length);

                var encbyteData = _rsa.Encrypt(tempBytes, RSAEncryptionPadding.Pkcs1);
                sb.Append(Convert.ToBase64String(encbyteData));
            }
            var result = sb.ToString();
            return result;
        }
        /// <summary>
        /// Encrypts data asynchronously
        /// </summary>
        /// <param name="data">The data to encrypt</param>
        /// <param name="key">The public key used to encrypt the data</param>
        /// <returns>A Task for the asynchronous operation</returns>
        public async Task<string> EncryptAsync(byte[] data, RSAParameters key)
        {
            var result = new StringBuilder();
            _rsa.ImportParameters(key);

            const int maxLength = 214;
            int dataLength = data.Length;
            int iterations = dataLength / maxLength;

            var tasks = new List<Task>();
            var semaphore = new SemaphoreSlim(8);

            for (var i = 0; i <= iterations; i++)
            {
                await semaphore.WaitAsync();

                var tempBytes = new byte[(dataLength - maxLength * i > maxLength) ? maxLength : dataLength - maxLength * i];
                Buffer.BlockCopy(data, maxLength * i, tempBytes, 0, tempBytes.Length);
                tasks.Add(EncryptBlock(semaphore, tempBytes, (s, iteration) =>
                {
                    result.Append(s);
                    //Log.i("Iteration:{0}", iteration);
                }, _rsa, i));
            }
            await Task.WhenAll(tasks);
            return result.ToString();
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private static async Task EncryptBlock(SemaphoreSlim semaphore, byte[] data, Action<string, int> result, System.Security.Cryptography.RSA rsa, int iteration)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                var encbyteData = rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);
                var r = Convert.ToBase64String(encbyteData);
                result?.Invoke(r, iteration);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debugger.Break();
                Log.e(e);
            }
            finally
            {
                semaphore.Release();
            }
        }

        /// <summary>
        /// Decrypts a string (previously encrypted with the Encrypt method)
        /// </summary>
        /// <param name="base64String">The encrypted string</param>
        /// <returns>A string containing the decrypted value</returns>
        public string Decrypt(string base64String)
        {
            return Decrypt(base64String, PrivateKey);
        }
        /// <summary>
        /// Decrypts a string (previously encrypted with the Encrypt method)
        /// </summary>
        /// <param name="base64String">The encrypted string</param>
        /// <param name="privateKey">The private key used to decrypt the string</param>
        /// <returns>A string containing the decrypted value</returns>
        public string Decrypt(string base64String, RSAParameters privateKey)
        {
            string result = null;
            try
            {
                _rsa.ImportParameters(privateKey);
                int base64BlockSize = (256 % 3 != 0) ? ((256 / 3) * 4) + 4 : (256 / 3) * 4;
                int iterations = base64String.Length / base64BlockSize;
                int l = 0;
                var fullbytes = new byte[0];
                for (int i = 0; i < iterations; i++)
                {
                    byte[] encBytes = Convert.FromBase64String(base64String.Substring(base64BlockSize * i, base64BlockSize));
                    byte[] bytes = _rsa.Decrypt(encBytes, RSAEncryptionPadding.Pkcs1);
                    Array.Resize(ref fullbytes, fullbytes.Length + bytes.Length);
                    foreach (byte t in bytes)
                    {
                        fullbytes[l] = t;
                        l++;
                    }
                }
                result = fullbytes.AsString();// Encoding.UTF32.GetString(fullbytes);
            }
            catch (Exception e)
            {
                Log.e(e);
            }
            return result;
        }

        /// <summary>
        /// Decrypts a byte array (previously encrypted with the Encrypt method)
        /// </summary>
        /// <param name="cipher">The encrypted data</param>
        /// <returns>A byte array containing the decrypted value</returns>
        public byte[] Decrypt(byte[] cipher)
        {
            return Decrypt(cipher, PrivateKey);
        }
        /// <summary>
        /// Decrypts a byte array (previously encrypted with the Encrypt method)
        /// </summary>
        /// <param name="cipher">The encrypted byte array</param>
        /// <param name="privateKey">The private key used to decrypt the data</param>
        /// <returns>A byte array containing the decrypted value</returns>
        public byte[] Decrypt(byte[] cipher, RSAParameters privateKey)
        {
            byte[] result = null;
            try
            {
                _rsa.ImportParameters(privateKey);
                int base64BlockSize = (256 % 3 != 0) ? ((256 / 3) * 4) + 4 : (256 / 3) * 4;
                int iterations = cipher.Length / base64BlockSize;
                int l = 0;
                var fullbytes = new byte[0];
                var encBytes = new byte[base64BlockSize];// {} cipher.Substring(base64BlockSize * i, base64BlockSize));
                for (int i = 0; i < iterations; i++)
                {
                    Array.Copy(cipher, base64BlockSize * i, encBytes, 0, base64BlockSize);
                    byte[] bytes = _rsa.Decrypt(encBytes, RSAEncryptionPadding.Pkcs1);
                    Array.Resize(ref fullbytes, fullbytes.Length + bytes.Length);
                    foreach (byte t in bytes)
                    {
                        fullbytes[l] = t;
                        l++;
                    }
                }
                result = fullbytes;// Encoding.UTF32.GetString(fullbytes);
            }
            catch (Exception e)
            {
                Log.e(e);
            }
            return result;
        }
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _rsa?.Dispose();
            _rsa = null;
        }
    }

    /// <summary>
    /// Used to generate MD5 hashes and verify input strings against them
    /// </summary>
    public class MD5
    {
        /// <summary>
        /// Generates an MD5 hash of the input string
        /// </summary>
        /// <param name="input">An MD5 hash of this input will be created</param>
        /// <returns>An MD5 hash of the inputted value</returns>
        public static string GetMd5Hash(string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                return MD5.GetMd5Hash(input, md5);
            }
        }
        /// <summary>
        /// Generates an MD5 hash of the input string
        /// </summary>
        /// <param name="input">An MD5 hash of this input will be created</param>
        /// <param name="md5">The previously allocated MD5 object to use</param>
        /// <returns>An MD5 hash of the inputted value</returns>
        public static string GetMd5Hash(string input, System.Security.Cryptography.MD5 md5)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Verifies a hash against a string
        /// </summary>
        /// <param name="input">The string to verify</param>
        /// <param name="hash">The MD5 hash used in the verification</param>
        /// <returns>True if the input string is verified, otherwise False</returns>
        public static bool VerifyMd5Hash(string input, string hash)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                // Hash the input.
                string hashOfInput = GetMd5Hash(input, md5);

                // Create a StringComparer an compare the hashes.
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;

                if (0 == comparer.Compare(hashOfInput, hash))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}