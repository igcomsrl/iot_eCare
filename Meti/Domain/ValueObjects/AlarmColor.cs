//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System.ComponentModel;

namespace Meti.Domain.ValueObjects
{
    public enum AlarmColor
    {
        [Description("Verde")]
        Green = 1,

        [Description("Giallo")]
        Yellow = 2,

        [Description("Rosso")]
        Red = 3
    }
}