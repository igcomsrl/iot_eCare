//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System;

namespace Meti.Maintenance
{
    public static class ConsoleColorHelper
    {
        public static void WritelineWithColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}