//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System;
using System.Collections.Generic;
using Meti.Application.Dtos.File;
using Meti.Application.Dtos.Process;
using Meti.Domain.ValueObjects;

namespace Meti.Application.Dtos.Registry
{
    public class RegistryEditDto
    {
        public Guid? Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public SexType? Sex { get; set; }
        public RegistryType? RegistryType { get; set; }
        public LifeStyle? LifeStyle { get; set; }        
        public string MobilePhone { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string MunicipalityPlace { get; set; }
        public string MunicipalityPlaceAddress { get; set; }
        public string MunicipalityPlaceCap { get; set; }
        public string DomicilePlace { get; set; }
        public string DomicilePlaceAddress { get; set; }
        public string DomicilePlaceCap { get; set; }
        public string RegionalMedicalCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string LatitudeLast { get; set; }
        public string LongitudeLast { get; set; }
        public string Allergy { get; set; }
        public string Intollerance { get; set; }
        public BloodGroup? BloodGroup { get; set; }
        public string Diagnosis { get; set; }
        public string PreviousIllnesses { get; set; }
        public string NextMedicalHistory { get; set; }
        public string RemoteAnamnesis { get; set; }
        public string Diet { get; set; }
        public string PathologiesInProgress { get; set; }
        public string Note { get; set; }        
        public string Weight { get; set; }        
        public string Height { get; set; }
        public IList<FileDto> Files { get; set; }

        public IList<HealthRiskEditDto> HealthRisks { get; set; }
    }
}