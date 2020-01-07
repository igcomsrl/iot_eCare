//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System;
using Meti.Application.Dtos.Registry;
using Meti.Domain.ValueObjects;

namespace Meti.Application.Dtos.Process
{
    public class HealthRiskDetailDto
    {
        public Guid? Id { get; set; }
        public HealthRiskType? Type { get; set; }
        public HealthRiskLevel? Level { get; set; }
        public string Rating { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsLast { get; set; }
        public RegistryDetailDto Registry {get; set;}
        public Guid? ProcessInstanceId { get; set;}
        public SexType? SexType { get; set; }
        public LifeStyle? LifeStyle { get; set; }
    }
}