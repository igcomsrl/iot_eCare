//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System;

namespace Meti.Application.Dtos.ProcessMacro
{
    public class ProcessMacroEditDto
    {
        public Guid? Id { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public Guid? Process { get; set; }
    }
}