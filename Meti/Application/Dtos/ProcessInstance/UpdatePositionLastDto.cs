//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meti.Application.Dtos.Process;
using Meti.Application.Dtos.Registry;

namespace Meti.Application.Dtos.ProcessInstance
{
    public class UpdatePositionLastDto
    {
        public Guid? ProcessInstanceId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string StressLabel { get; set; }
    }
}
