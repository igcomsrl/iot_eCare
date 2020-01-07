//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using Meti.Application.Dtos.Alarm;
using System;
using System.Collections.Generic;

namespace Meti.Application.Dtos.Parameter
{
    public class ParameterIndexDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public Guid? Process { get; set; }
        public Guid? Device { get; set; }
        public IList<AlarmDetailDto> Alarms { get; set; }
        public bool? IsEnabled { get; set; }
        public string PositionMisure { get; set; }
    }
}