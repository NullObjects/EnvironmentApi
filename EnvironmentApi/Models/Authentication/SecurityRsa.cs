using System;
using System.Security.Cryptography;
using System.Text;
using XC.RSAUtil;

namespace EnvironmentApi.Models
{
    public class SecurityRsa
    {
        public static RSAParameters PublicKey { get; private set; }
        public static string PublicKeyString { get; private set; }
        public static RSAParameters PrivateKey { get; private set; }
        public static string PrivateKeyString { get; private set; }

        static SecurityRsa()
        {
            InitWindows();
        }

        private static void InitWindows()
        {
            var parameters = new CspParameters()
            {
                KeyContainerName = "ENVIRONMENTRSAHELPER" // 默认的RSA保存密钥的容器名称
            };
            var handle = new RSACryptoServiceProvider(parameters);
            PublicKey = handle.ExportParameters(false);
            PrivateKey = handle.ExportParameters(true);

            PublicKeyString = RsaKeyConvert.PublicKeyXmlToPem(
                handle.ToXmlString(false));
            PrivateKeyString = RsaKeyConvert.PrivateKeyXmlToPkcs1(
                handle.ToXmlString(true));
        }

        public static void ExportKeyPair(string publicKeyXmlString, string privateKeyXmlString)
        {
            var handle = new RSACryptoServiceProvider();
            handle.FromXmlString(privateKeyXmlString);
            PrivateKey = handle.ExportParameters(true);
            handle.FromXmlString(publicKeyXmlString);
            PublicKey = handle.ExportParameters(false);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="dataToEncrypt"></param>
        /// <returns></returns>
        public static string Encrypt(string dataToEncrypt)
        {
            try
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(PublicKey);
                    var encryptedData = rsa.Encrypt(
                        Encoding.Default.GetBytes(dataToEncrypt), true);
                    return Convert.ToBase64String(encryptedData);
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="dataToDecrypt"></param>
        /// <returns></returns>
        public static string Decrypt(string dataToDecrypt)
        {
            try
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(PrivateKey);
                    var decryptedData = rsa.Decrypt(
                        Convert.FromBase64String(dataToDecrypt), true);
                    return Encoding.Default.GetString(decryptedData);
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}