//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System.ComponentModel;

namespace Meti.Domain.ValueObjects
{
    public enum BloodGroup
    {
        [Description("Gruppo O Rh-")]
        ORhMeno,

        [Description("Gruppo O Rh+")]
        ORhPiu,

        [Description("Gruppo A Rh-")]
        ORaMeno,

        [Description("Gruppo A Rh+")]
        ORaPiu,

        [Description("Gruppo B Rh-")]
        ORbMeno,

        [Description("Gruppo B Rh+")]
        ORbPiu,

        [Description("Gruppo AB Rh-")]
        ORabMeno,

        [Description("Gruppo AB Rh+")]
        ORabPiu,
    }
}