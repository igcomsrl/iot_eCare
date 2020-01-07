//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using Meti.Domain.ValueObjects;
using System;

namespace Meti.Application.Dtos.AlarmFired
{
    public class AlarmFiredEditDto
    {
        public Guid? Id { get; set; }
        public AlarmColor? AlarmColor { get; set; }
        public  string Result { get; set; }
        public Guid? ParameterId { get; set; }
        public Guid? AlarmId { get; set; }
        public Guid? ProcessInstanceId { get; set; }
        public bool? IsActive { get; set; }
        public string ResolvingNotes { get; set; }
        public DateTime? InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Latitude { get; set; }        
        public string Longitude { get; set; }        
        public bool? Fall { get; set; }
        public bool? FallNoResponse { get; set; }
        public string Feedback { get; set; }
        public DateTime? FeedbackDate { get; set; }
        public string FeedbackBy { get; set; }
        public string PatientFeedback { get; set; }
        public string InsertUser { get; set; }
    }
}
