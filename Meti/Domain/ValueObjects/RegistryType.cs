//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Meti.Domain.ValueObjects
{
    public enum RegistryType
    {
        [Description("Paziente")]
        Paziente = 1,
        [Description("Medico")]
        Medico = 2,
        [Description("Persona di riferimento")]
        ReferencePerson = 3
    }
}
