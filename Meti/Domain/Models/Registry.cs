//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Entity.Base;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Meti.Domain.Models
{
    public class Registry : EntityBase<Guid?>
    {
        [Required, StringLength(255)]
        public virtual string Firstname { get; set; }
        [Required, StringLength(255)]
        public virtual string Surname { get; set; }
        [Required]
        public virtual SexType? Sex { get; set; }
        [Required]
        public virtual RegistryType? RegistryType { get; set; }
        [StringLength(255)]
        public virtual string MobilePhone { get; set; }
        [StringLength(255)]
        public virtual string Phone { get; set; }
        [Required, StringLength(255)]
        public virtual string Email { get; set; }
        public virtual DateTime? BirthDate { get; set; }
        [StringLength(255)]
        public virtual string BirthPlace { get; set; }
        [StringLength(255)]
        public virtual string MunicipalityPlace { get; set; }
        [StringLength(255)]
        public virtual string MunicipalityPlaceAddress { get; set; }
        [StringLength(255)]
        public virtual string MunicipalityPlaceCap { get; set; }
        [StringLength(255)]
        public virtual string DomicilePlace { get; set; }
        [StringLength(255)]
        public virtual string DomicilePlaceAddress { get; set; }
        [StringLength(255)]
        public virtual string DomicilePlaceCap { get; set; }
        [StringLength(255)]
        public virtual string RegionalMedicalCode { get; set; }
        [StringLength(255)]
        public virtual string Latitude { get; set; }
        [StringLength(255)]
        public virtual string Longitude { get; set; }
        [StringLength(2000)]
        public virtual string Allergy { get; set; }
        [StringLength(2000)]
        public virtual string Intollerance { get; set; }
        public virtual BloodGroup? BloodGroup { get; set; }
        [StringLength(2000)]
        public virtual string Diagnosis { get; set; }
        [StringLength(2000)]
        public virtual string PreviousIllnesses { get; set; }
        [StringLength(2000)]
        public virtual string NextMedicalHistory { get; set; }
        [StringLength(2000)]
        public virtual string RemoteAnamnesis { get; set; }
        [StringLength(2000)]
        public virtual string Diet { get; set; }
        [StringLength(2000)]
        public virtual string PathologiesInProgress { get; set; }
        [StringLength(2000)]
        public virtual string Note { get; set; }
        [StringLength(255)]
        public virtual string Weight { get; set; }
        [StringLength(255)]
        public virtual string Height { get; set; }
        [StringLength(255)]
        public virtual string LatitudeLast { get; set; }
        [StringLength(255)]
        public virtual string LongitudeLast { get; set; }
        public virtual IList<File> Files { get; set; }
        public virtual IList<HealthRisk> HealthRisks { get; set; }
        
        public virtual LifeStyle? LifeStyle { get; set; }
        public Registry()
        {
            Files = new List<File>();
            HealthRisks = new List<HealthRisk>();
        }
    }
}
