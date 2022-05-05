using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAPI.Models
{
    public class Citisec
    {
        
            private TripleDESCryptoServiceProvider TripleDes = new TripleDESCryptoServiceProvider();

        public Citisec()
        {
            string key = "FintechESB";
            this.TripleDes.Key = this.TruncateHash(key, this.TripleDes.KeySize / 8);
            this.TripleDes.IV = this.TruncateHash("", this.TripleDes.BlockSize / 8);
        }

        public string EncryptSHA256Managed(string Message)
        {
            SHA256 sHA = new SHA256Managed();
            byte[] bytes = Encoding.Default.GetBytes(Message);
            byte[] array = sHA.ComputeHash(bytes);
            string text = string.Empty;
            for (int i = 0; i < array.Length; i++)
            {
                text += array[i].ToString("x");
            }
            return text;
        }

        public static string EncryptString(string Message)
        {
            string result;
            try
            {
                string s = "FintechESB";
                UTF8Encoding uTF8Encoding = new UTF8Encoding();
                MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
                byte[] key = mD5CryptoServiceProvider.ComputeHash(uTF8Encoding.GetBytes(s));
                TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
                tripleDESCryptoServiceProvider.Key = key;
                tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
                tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
                byte[] bytes = uTF8Encoding.GetBytes(Message);
                byte[] inArray;
                try
                {
                    ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateEncryptor();
                    inArray = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
                }
                finally
                {
                    tripleDESCryptoServiceProvider.Clear();
                    mD5CryptoServiceProvider.Clear();
                }
                result = Convert.ToBase64String(inArray);
            }
            catch
            {
                result = "";
            }
            return result;
        }

        public static string DecryptString(string Message)
        {
            string result;
            try
            {
                string s = "FintechESB";
                UTF8Encoding uTF8Encoding = new UTF8Encoding();
                MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
                byte[] key = mD5CryptoServiceProvider.ComputeHash(uTF8Encoding.GetBytes(s));
                TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
                tripleDESCryptoServiceProvider.Key = key;
                tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
                tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
                byte[] array = Convert.FromBase64String(Message);
                byte[] bytes;
                try
                {
                    ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor();
                    bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
                }
                finally
                {
                    tripleDESCryptoServiceProvider.Clear();
                    mD5CryptoServiceProvider.Clear();
                }
                result = uTF8Encoding.GetString(bytes);
            }
            catch
            {
                result = "";
            }
            return result;
        }
        private byte[] TruncateHash(string key, int length)
        {
            SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
            byte[] bytes = Encoding.Unicode.GetBytes(key);
            byte[] result = sHA1CryptoServiceProvider.ComputeHash(bytes);
            Array.Resize<byte>(ref result, length);
            return result;
        }

        public string EncryptData(string Plaintext)
        {
            string result;
            try
            {
                byte[] bytes = Encoding.Unicode.GetBytes(Plaintext);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, this.TripleDes.CreateEncryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(bytes, 0, bytes.Length);
                cryptoStream.FlushFinalBlock();
                result = Convert.ToBase64String(memoryStream.ToArray());
            }
            catch
            {
                result = "";
            }
            return result;
        }

        public string DecryptData(string encryptedtext)
        {
            byte[] array = Convert.FromBase64String(encryptedtext);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, this.TripleDes.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(array, 0, array.Length);
            cryptoStream.FlushFinalBlock();
            return Encoding.Unicode.GetString(memoryStream.ToArray());
        }

        public static string MD5Hash(string Message)
        {
            MD5 mD = MD5.Create();
            byte[] array = mD.ComputeHash(Encoding.Default.GetBytes(Message));
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        private static bool verifyMd5Hash(string input, string hash)
        {
            string x = Citisec.MD5Hash(input);
            StringComparer ordinalIgnoreCase = StringComparer.OrdinalIgnoreCase;
            return 0 == ordinalIgnoreCase.Compare(x, hash);
        }
    }
}
