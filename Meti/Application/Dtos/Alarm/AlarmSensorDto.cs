//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using MateSharp.Framework.Dtos;
using Meti.Application.Dtos.AlarmMetric;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Meti.Application.Dtos.Alarm
{
    public class AlarmSensorDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Emails { get; set; }
        public string SmsNumbers { get; set; }
        public bool? ContactOperator { get; set; }
        public bool? IsEnabled { get; set; }
        public string ResolvingNotes { get; set; }
        public IList<AlarmMetricSensorDto> AlarmMetrics { get; set; }
        public ItemDto<AlarmColor?> AlarmColor { get; set; }
    }
}