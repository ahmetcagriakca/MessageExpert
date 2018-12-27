using MessageExpert.Domain.Security.Crypto.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Domain.Security.Crypto.Services
{
    /// <summary>
    /// <see cref="CryptoService"/>
    /// </summary>
    public interface ICryptoService 
    {
        HashString Encrypt(string text);
    }
}
