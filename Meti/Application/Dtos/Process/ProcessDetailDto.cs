//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System;
using System.Collections.Generic;
using Meti.Application.Dtos.Parameter;
using Meti.Application.Dtos.ProcessMacro;

namespace Meti.Application.Dtos.Process
{
    public class ProcessDetailDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public IList<ProcessMacroDetailDto> ProcessMacros { get; set; }
        public IList<ParameterDetailDto> Parameters { get; set; }
        public bool? IsEnabled { get; set; }
    }
}