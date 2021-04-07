using System;
using System.IO;
using System.Security.Cryptography;

namespace EnvironmentApi.Controllers.AuthenticateService
{
    public static class SecurityAes
    {
        static SecurityAes()
        {
            AesHandler = Aes.Create();
            AesHandler.Key = Convert.FromBase64String("lB2BxrJdI4UUjK3KEZyQ0obuSgavB1SYJuAFq9oVw0Y=");
            AesHandler.IV = Convert.FromBase64String("6lra6ceX26Fazwj1R4PCOg==");
        }

        private static Aes AesHandler { get; }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Encrypt(string source)
        {
            using (var mem = new MemoryStream())
            using (var stream = new CryptoStream(mem, AesHandler.CreateEncryptor(AesHandler.Key, AesHandler.IV),
                CryptoStreamMode.Write))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(source);
                }

                return Convert.ToBase64String(mem.ToArray());
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Decrypt(string source)
        {
            var data = Convert.FromBase64String(source);
            using (var mem = new MemoryStream(data))
            using (var crypto = new CryptoStream(mem, AesHandler.CreateDecryptor(AesHandler.Key, AesHandler.IV),
                CryptoStreamMode.Read))
            using (var reader = new StreamReader(crypto))
            {
                return reader.ReadToEnd();
            }
        }
    }
}