//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using MateSharp.Framework.Dtos;
using Meti.Application.Dtos.Registry;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Meti.Application.Dtos.ProcessInstance
{
    public class ProcessInstanceIndexDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public IList<RegistryEditDto> Doctors { get; set; }
        public RegistryEditDto Patient{ get; set; }
        public IList<RegistryEditDto> ReferencePersons { get; set; }
        public string ProcessName { get; set; }
        public DateTime? InsertDate { get; set; }
        public ItemDto<ProcessInstanceState?> ProcessInstanceState { get; set; }
    }
}