//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System.Collections.Generic;
using MateSharp.Framework.Dtos;
using Meti.Application.Dtos.Alarm;
using Meti.Application.Dtos.Device;
using Meti.Domain.ValueObjects;

namespace Meti.Application.Dtos.Parameter
{
    public class ParameterEssentialDataDto
    {
        public IList<ItemDto<FrequencyType?>> FrequencyTypeList { get; set; }
        public IList<DeviceDetailDto> DeviceList { get; set; }
        public IList<AlarmDetailDto> AlarmList { get; set; }
    }
}
