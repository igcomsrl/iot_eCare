using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meti.Application.Dtos
{
    public class ProcessMonitorDataDto
    {
        public string Name { get; set; }
        IList<ParameterMonitorDataDto> Parameters { get; set; } 
    }
    public class ParameterMonitorDataDto
    {
        public string Name { get; set; }
        public DeviceMonitorDataDto Device { get; set; }
        public int? Frequency { get; set; }        
        public FrequencyType? FrequencyType { get; set; }
        IList<AlarmMonitorDataDto> Alarms { get; set; }
    }
    public enum FrequencyType
    {
        Second = 1,        
        Minute = 2,        
        Hour = 3,        
        Day = 4,
        Week = 5,
        Month = 6
    }

    public class DeviceMonitorDataDto
    {
        public string Macaddress { get; set; }
        public string Name { get; set; }
    }

    public class AlarmMonitorDataDto
    {
        public string Name { get; set; }
        public int ThresholdMin { get; set; }
        public int ThresholdMax { get; set; }
    }
}
