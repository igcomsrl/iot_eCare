//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using MateSharp.Framework.Dtos;
using Meti.Domain.ValueObjects;
using System;

namespace Meti.Application.Dtos.Alarm
{
    public class AlarmDetailDto
    {
        public string Name { get; set; }
        public int ThresholdMin { get; set; }
        public int ThresholdMax { get; set; }
        public Guid? Parameter { get; set; }
        public string Emails { get; set; }
        public string SmsNumbers { get; set; }
        public bool? ContactOperator { get; set; }
        public bool? IsEnabled { get; set; }
        public ItemDto<AlarmColor?> AlarmColor { get; set; }
    }
}