//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using Meti.Application.Dtos.Alarm;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Meti.Application.Dtos.Parameter
{
    public class ParameterSensorDetailDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        //public string PositionMisure { get; set; }
        //public int? Frequency { get; set; }
        //public FrequencyType? FrequencyType { get; set; }
        public IList<AlarmSensorDto> Alarms { get; set; }
        //public bool? IsEnabled { get; set; }
        public Guid? ProcessInstanceId { get; set; }
    }
}