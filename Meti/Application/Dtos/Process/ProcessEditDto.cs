//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System;
using System.Collections.Generic;
using Meti.Application.Dtos.Parameter;
using Meti.Application.Dtos.ProcessMacro;
using Meti.Domain.ValueObjects;

namespace Meti.Application.Dtos.Process
{
    public class ProcessEditDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public IList<ProcessMacroEditDto> ProcessMacros { get; set; }
        public IList<ParameterEditDto> Parameters { get; set; }
        public bool? IsEnabled { get; set; }
        public ProcessType? ProcessType { get; set; }
    }
}