//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System.ComponentModel;

namespace Meti.Domain.ValueObjects
{
    public enum HealthRiskLevel
    {
        [Description("1 (Assente)")]
        Blank = 1,
        [Description("2 (Lieve)")]
        Soft = 2,
        [Description("3 (Moderato)")]
        Moderate = 3,
        [Description("4 (Grave)")]
        Serious = 4,
        [Description("5 (Molto grave)")]
        VerySerious = 5
    }
}