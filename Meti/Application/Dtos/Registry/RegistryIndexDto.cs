//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System;
using System.Collections.Generic;
using MateSharp.Framework.Dtos;
using Meti.Application.Dtos.Process;
using Meti.Domain.ValueObjects;

namespace Meti.Application.Dtos.Registry
{
    public class RegistryIndexDto
    {
        public Guid? Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public ItemDto<SexType?> Sex { get; set; }
        public ItemDto<RegistryType?> RegistryType { get; set; }
        public string MobilePhone { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string RegionalMedicalCode { get; set; }
        public DateTime? InsertDate { get; set; }
        public IList<HealthRiskEditDto> HealthRisks { get; set; }
    }
}