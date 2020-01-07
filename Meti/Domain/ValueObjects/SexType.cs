//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Meti.Domain.ValueObjects
{
    public enum SexType
    {
        [Description("Maschio")]
        Maschio = 1,
        [Description("Femmina")]
        Femmina = 2
    }
}
