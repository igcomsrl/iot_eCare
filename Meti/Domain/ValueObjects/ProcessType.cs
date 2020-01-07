//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meti.Domain.ValueObjects
{
    public enum ProcessType
    {
        [Description("Template")]
        Template = 1,
        [Description("Instance")]
        Instance = 2
    }
}
