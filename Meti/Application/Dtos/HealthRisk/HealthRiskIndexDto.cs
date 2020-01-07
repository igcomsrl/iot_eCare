//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System;
using MateSharp.Framework.Dtos;
using Meti.Domain.ValueObjects;

namespace Meti.Application.Dtos.Process
{
    public class HealthRiskIndexDto
    {
        public Guid? Id { get; set; }
        public ItemDto<HealthRiskType?> Type { get; set; }
        public ItemDto<HealthRiskLevel?> Level { get; set; }
        public string Rating { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsLast { get; set; }
    }
}