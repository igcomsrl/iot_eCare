//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System.Collections.Generic;
using Meti.Application.Dtos.Registry;

namespace Meti.Application.Dtos.Process
{
    public class ProcessEssentialDataDto
    {
        public IList<RegistryDetailDto> DoctorList { get; set; }
        public IList<RegistryDetailDto> PatientList { get; set; }
    }
}
