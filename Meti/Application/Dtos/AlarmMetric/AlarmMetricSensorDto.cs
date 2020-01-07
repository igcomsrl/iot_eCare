//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System;

namespace Meti.Application.Dtos.AlarmMetric
{
    public class AlarmMetricSensorDto
    {
        //public Guid? Id { get; set; }
        public string Metric { get; set; }
        public decimal ThresholdMin { get; set; }
        public decimal ThresholdMax { get; set; }
        public string Mac { get; set; }
        //public Guid? Alarm { get; set; }
    }
}