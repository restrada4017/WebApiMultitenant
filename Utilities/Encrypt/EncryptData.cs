using GPT.Application.Contracts.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GPT.Utilities.Encrypt
{
    public class EncryptData : IEncrypt
    {

        public string EncryptMD51(string text, string key)
        {


            byte[] keyArray;

            byte[] Arreglo_a_Cifrar = UTF8Encoding.UTF8.GetBytes(text);

            //Se utilizan las clases de encriptación MD5

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

            hashmd5.Clear();

            //Algoritmo TripleDES
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();

            byte[] ArrayResult = cTransform.TransformFinalBlock(Arreglo_a_Cifrar, 0, Arreglo_a_Cifrar.Length);

            tdes.Clear();

            //se regresa el resultado en forma de una cadena
            text = Convert.ToBase64String(ArrayResult, 0, ArrayResult.Length);

            return text;
        }

        public string DecryptMDS1(string text, string key)
        {

            byte[] keyArray;
            byte[] Array_a_Decrypt = Convert.FromBase64String(text);

            //algoritmo MD5
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();

            byte[] resultArray = cTransform.TransformFinalBlock(Array_a_Decrypt, 0, Array_a_Decrypt.Length);

            tdes.Clear();
            text = UTF8Encoding.UTF8.GetString(resultArray);


            return text;
        }


    }
}
