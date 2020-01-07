//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meti.Infrastructure.Security
{
    public class PasswordGenerator
    {

        /// <summary>
        /// Generazione password temporanea.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GenerateRandom(int passwordLength)
        {
            int passwordLengthShadow = passwordLength;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < passwordLengthShadow--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
