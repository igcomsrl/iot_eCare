//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Meti.Domain.ValueObjects
{
    public enum FrequencyType
    {
        [Description("Secondo/i")]
        Second = 1,

        [Description("Minuto/i")]
        Minute = 2,

        [Description("Ora/e")]
        Hour = 3,

        [Description("Giorno/i")]
        Day = 4,

        [Description("Settimana/e")]
        Week = 5,

        [Description("Mese/i")]
        Month = 6
    }
}
