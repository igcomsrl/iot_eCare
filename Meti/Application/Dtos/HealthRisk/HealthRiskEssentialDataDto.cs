//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using MateSharp.Framework.Dtos;
using Meti.Domain.ValueObjects;
using System.Collections.Generic;

namespace Meti.Application.Dtos.Process
{
    public class HealthRiskEssentialDataDto
    {
        public IList<ItemDto<HealthRiskType?>> HealthRiskTypeList { get; set; }
        public IList<ItemDto<HealthRiskLevel?>> HealthRiskLevelList { get; set; }
    }
}