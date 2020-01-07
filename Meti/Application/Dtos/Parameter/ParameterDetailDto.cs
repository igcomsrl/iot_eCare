//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using MateSharp.Framework.Dtos;
using Meti.Application.Dtos.Alarm;
using Meti.Application.Dtos.Device;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Meti.Application.Dtos.Parameter
{
    public class ParameterDetailDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public Guid? Process { get; set; }
        public DeviceEditDto Device { get; set; }
        public IList<AlarmEditDto> Alarms { get; set; }
        public int? Frequency { get; set; }
        public ItemDto<FrequencyType?> FrequencyType { get; set; }
        public bool? IsEnabled { get; set; }
        public string PositionMisure { get; set; }
    }
}