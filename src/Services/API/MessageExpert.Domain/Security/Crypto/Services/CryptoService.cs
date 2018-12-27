using MessageExpert.Domain.Security.Crypto.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MessageExpert.Domain.Security.Crypto.Services
{

    public class CryptoService : ICryptoService
    {
        public HashString Encrypt(string text)
        {
            HashAlgorithm algorithm = null;
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            algorithm = MD5.Create();
            StringBuilder builder = new StringBuilder();
            var computed = algorithm.ComputeHash(textBytes);
            for (int i = 0; i < computed.Length; i++)
            {
                builder.Append(computed[i].ToString("x2"));
            }

            algorithm.Dispose();
            return new HashString(builder.ToString());
        }
    }
}
