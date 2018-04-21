#region  usings 

using System;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace Vedaantees.Framework.Providers.Security
{
    /// <summary>
    /// SHA256CryptoServiceProvider adapter
    /// </summary>
    public class Sha256CryptoServiceProviderAdapter : ICryptographicService
    {
        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="stringToHash">The string to hash.</param>
        /// <returns></returns>
        public string ComputeHash(string stringToHash)
        {
            var hashAlg = new SHA256CryptoServiceProvider();
            var bytValue = Encoding.UTF8.GetBytes(stringToHash);
            var bytHash = hashAlg.ComputeHash(bytValue);
            return Convert.ToBase64String(bytHash);
        }
    }
}