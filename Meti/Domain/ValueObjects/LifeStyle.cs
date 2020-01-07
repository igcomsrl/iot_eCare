//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System.ComponentModel;

namespace Meti.Domain.ValueObjects
{
    public enum LifeStyle
    {
        [Description("Sedentario")]
        Sedentary = 1,
        [Description("Parzialmente attivo")]
        PartiallyActive = 2,
        [Description("Attivo")]
        Active = 3,
        [Description("Sportivo")]
        Sports = 4,
        [Description("Atletico")]
        Athletic = 5
    }
}