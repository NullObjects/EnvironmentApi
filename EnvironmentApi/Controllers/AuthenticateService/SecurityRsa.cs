using System;
using System.Security.Cryptography;
using System.Text;
using XC.RSAUtil;

namespace EnvironmentApi.Controllers
{
    public class SecurityRsa
    {
        public static RSAParameters PublicKey { get; private set; }
        public static string PublicKeyString { get; private set; }
        public static RSAParameters PrivateKey { get; private set; }
        public static string PrivateKeyString { get; private set; }

        static SecurityRsa()
        {
            InitRsa();
        }

        /// <summary>
        /// 生成秘钥
        /// </summary>
        private static void InitRsa()
        {
            var handle = RSA.Create();
            PublicKey = handle.ExportParameters(false);
            PrivateKey = handle.ExportParameters(true);

            PublicKeyString = RsaKeyConvert.PublicKeyXmlToPem(
                handle.ToXmlString(false));
            PrivateKeyString = RsaKeyConvert.PrivateKeyXmlToPkcs1(
                handle.ToXmlString(true));
        }

        /// <summary>
        /// 使用现有秘钥
        /// </summary>
        private static void InitRsaByKey()
        {
            PrivateKeyString = @"-----BEGIN RSA PRIVATE KEY-----
MIICXQIBAAKBgQCnozW0Xw45g8vRHup9M9uFGIVe9kA535AOpYJl2bHvei5ECc7x
Z/ltMVkB/US1O2tY22VO9OwnnSeNERAriaPEuwkGOgN5mkH+Tci6h0s8jjt0zXUU
zNQH101gZTEVPdg8wxNzSIMDWIpyLSZN9j8tctxl/TybKLv87FCIxX5BHwIDAQAB
AoGAAtOUpzrnEX8wY5FC4OOL8v2L2iMKyC8Fzclqnm8COgh0WT9VFurq4LVxS8BK
dUfQMopYdfFZPf2/WfiMrj8+3J5Pb0JgMbZUxQq2/2hkkDCgzZ7OlzmB4zUQs6FQ
weNU2yeu4aUFKUceZztoxicJZogDPFmaE+SM565oBs0Oq/ECQQDbSXicW+uqLG5R
Qn//WFc1kSIcEjuyiCe+yrcHEqDlb4FVvS3zRnBk6deKhlx+XKhliFTywpNHTeQ0
rDzDVLApAkEAw7QRts1MBUle/axtLV7z5cGqH9iH/ngw9gMUZaynwaYDWlTOCyVW
a3oQmdHazt3BMQ9oMCP3DfVXMn8uVa/wBwJBAJJfzpSAfySOl+zB+PKXI09sW1pl
iVe4rt8aLWYZNzEvZyO2Lc/vuuKBfRDIc0Ed1IJBqIKLAwhbo/LM7ZptE1ECQQCX
fY10zwh40yn3/gBpbaYpBTgW/LuQ7SMhXJLufa9CRKs1wo3YrOdvvQaau4rANYJ1
4rJJCZ4VrQP5r9+DLvUDAkBwoQZ3OOjaqKa15v6byMbEXORNR9Eg1x+sFAvmjB3p
igGGM7Pky0xc+F6qzvKfLLwog1nkzJ4D1p6nHli3o9bl
-----END RSA PRIVATE KEY-----";

            PublicKeyString = @"-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnozW0Xw45g8vRHup9M9uFGIVe
9kA535AOpYJl2bHvei5ECc7xZ/ltMVkB/US1O2tY22VO9OwnnSeNERAriaPEuwkG
OgN5mkH+Tci6h0s8jjt0zXUUzNQH101gZTEVPdg8wxNzSIMDWIpyLSZN9j8tctxl
/TybKLv87FCIxX5BHwIDAQAB
-----END PUBLIC KEY-----";

            ImportKeyPair(RsaKeyConvert.PublicKeyPemToXml(PublicKeyString),
                RsaKeyConvert.PrivateKeyPkcs1ToXml(PrivateKeyString));
        }

        /// <summary>
        /// 导入秘钥
        /// </summary>
        /// <param name="publicKeyXmlString"></param>
        /// <param name="privateKeyXmlString"></param>
        public static void ImportKeyPair(string publicKeyXmlString, string privateKeyXmlString)
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
                        Encoding.Default.GetBytes(dataToEncrypt), false);
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
                        Convert.FromBase64String(dataToDecrypt), false);
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