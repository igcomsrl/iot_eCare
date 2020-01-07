//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System.Collections.Generic;
using MateSharp.Framework.Dtos;
using Meti.Application.Dtos.Process;
using Meti.Application.Dtos.Registry;
using Meti.Domain.ValueObjects;

namespace Meti.Application.Dtos.ProcessInstance
{
    public class ProcessInstanceEssentialDataDto
    {
        public IList<RegistryDetailDto> DoctorList { get; set; }
        public IList<RegistryDetailDto> PatientList { get; set; }
        public IList<ProcessEditDto> ProcessList { get; set; }
        public IList<RegistryDetailDto> ReferencePersonList { get; set; }
        public IList<ItemDto<ProcessInstanceState>> ProcessInstanceStateList{ get; set; }
    }
}
