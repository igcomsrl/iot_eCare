//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using Meti.Application.Dtos.Device;
using System;

namespace Meti.Application.Dtos.AlarmMetric
{
    public class AlarmMetricEditDto
    {
        public Guid? Id { get; set; }
        public string Metric { get; set; }
        public decimal ThresholdMin { get; set; }
        public decimal ThresholdMax { get; set; }
        public DeviceEditDto Device { get; set; }
        public Guid? Alarm { get; set; }
    }
}