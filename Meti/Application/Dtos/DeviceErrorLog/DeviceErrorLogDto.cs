//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System;

namespace Meti.Application.Dtos.Device
{
    public class DeviceErrorLogDto
    {
        public string Error { get; set; }
        
        public string DeviceId { get; set; }
        
        public Guid? ProcessInstanceId { get; set; }
        public string InsertDate { get; set; }
    }
}