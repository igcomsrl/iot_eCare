//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using MateSharp.Framework.Dtos;
using Meti.Domain.ValueObjects;
using System;
using Meti.Application.Dtos.Parameter;
using Meti.Application.Dtos.ProcessInstance;
using Meti.Application.Dtos.Alarm;

namespace Meti.Application.Dtos.AlarmFired
{
    public class AlarmFiredSwiftDto
    {
        public Guid? Id { get; set; }
        public ItemDto<AlarmColor?> AlarmColor { get; set; }
        public  string Result { get; set; }
        public string ParameterName { get; set; }
        public string PatientName { get; set; }
        //public ParameterDetailDto Parameter { get; set; }
        //public ProcessInstanceDetailDto ProcessInstance { get; set; }
        //public AlarmDetailDto Alarm { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? InsertDate { get; set; }
        public string ResolvingNotes { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool? IsFall { get; set; }
        public bool? IsFallNoResponse { get; set; }
        public string Feedback { get; set; }
        public DateTime? FeedbackDate { get; set; }
        public string FeedbackBy { get; set; }
        public string PatientFeedback { get; set; }
        public string InsertUser { get; set; }
    }
}
