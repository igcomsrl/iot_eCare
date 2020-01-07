//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Entity.Base;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Meti.Domain.Models
{
    public class HealthRisk : EntityBase<Guid?>
    {
        [Required]
        public virtual HealthRiskType? Type { get; set; }

        [Required]
        public virtual HealthRiskLevel? Level { get; set; }

        [StringLength(255)]
        public virtual string Rating { get; set; }

        [Required]
        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        [Required]
        public virtual Registry Registry { get; set; }
    }
}