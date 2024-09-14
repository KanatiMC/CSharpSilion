using System;
using System.Security.Cryptography;
using System.Text;

namespace CSharpSilon.Utils
{
    public class Decrypt
    {
        public static byte[] ivBytes = new byte[16];
        public static byte[] keyBytes = new byte[16];

        public static void GenerateIVBytes()
        {
            new Random().NextBytes(ivBytes);
        }

        public static void GenerateKeyBytes()
        {
            int sum = 0;
            foreach (char curChar in "csln")
            {
                sum += curChar;
            }
            new Random(sum).NextBytes(keyBytes);
        }

        public static string Encode(string data)
        {
            GenerateIVBytes();
            GenerateKeyBytes();
            ICryptoTransform cryptoTransform = Aes.Create().CreateEncryptor(keyBytes, ivBytes);
            byte[] input = Encoding.BigEndianUnicode.GetBytes(data);
            byte[] output = cryptoTransform.TransformFinalBlock(input, 0, input.Length);

            // Combine IV and encrypted data
            byte[] result = new byte[ivBytes.Length + output.Length];
            Buffer.BlockCopy(ivBytes, 0, result, 0, ivBytes.Length);
            Buffer.BlockCopy(output, 0, result, ivBytes.Length, output.Length);

            // Return the base64 string of the combined IV and encrypted data
            return Convert.ToBase64String(result);
        }

        public static string Decode(string text)
        {
            byte[] encryptedDataWithIv = Convert.FromBase64String(text);

            // Extract IV
            Buffer.BlockCopy(encryptedDataWithIv, 0, ivBytes, 0, ivBytes.Length);

            // Extract encrypted data
            byte[] encryptedData = new byte[encryptedDataWithIv.Length - ivBytes.Length];
            Buffer.BlockCopy(encryptedDataWithIv, ivBytes.Length, encryptedData, 0, encryptedData.Length);

            GenerateKeyBytes();
            ICryptoTransform cryptoTransform = Aes.Create().CreateDecryptor(keyBytes, ivBytes);
            byte[] output = cryptoTransform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

            return Encoding.BigEndianUnicode.GetString(output);
        }
    }
}