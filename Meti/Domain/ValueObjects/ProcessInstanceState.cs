//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System.ComponentModel;

namespace Meti.Domain.ValueObjects
{
    public enum ProcessInstanceState
    {
        [Description("Aperto")]
        Open = 1,

        [Description("Chiuso")]
        Close = 2,        
    }
}