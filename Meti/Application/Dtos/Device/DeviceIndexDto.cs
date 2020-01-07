//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using Meti.Application.Dtos.ProcessInstance;
using System;

namespace Meti.Application.Dtos.Device
{
    public class DeviceIndexDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Macaddress { get; set; }
        public bool? IsEnabled { get; set; }
        //public ProcessInstanceDetailDto ProcessInstance { get; set; }
        public string ProcessInstanceName{ get; set; }
        public string PatientName { get; set; }

    }
}