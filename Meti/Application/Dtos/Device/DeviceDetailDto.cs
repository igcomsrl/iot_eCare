//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System;

namespace Meti.Application.Dtos.Device
{
    public class DeviceDetailDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Macaddress { get; set; }
        public Guid? DeviceController { get; set; }
        public bool? IsEnabled { get; set; }
    }
}