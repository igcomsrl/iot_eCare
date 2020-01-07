//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Meti.Infrastructure.Security
{
    public class EncryptPassword
    {
        /// <summary>
        /// Encrypts the with sha256.
        /// </summary>
        /// <param name="decryptedPassword">The decrypted password.</param>
        /// <returns>System.String.</returns>
        public static string EncryptWithSha256(string decryptedPassword)
        {
            HashAlgorithm hash = new SHA256Managed();
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(decryptedPassword);
            byte[] hashBytes = hash.ComputeHash(plainTextBytes);
            //in this string you got the encrypted password
            string hashValue = Convert.ToBase64String(hashBytes);
            return hashValue;
        }
    }
}
