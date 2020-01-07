//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Entity.Base;
using Meti.Domain.ValueObjects;
using System;
using System.ComponentModel.DataAnnotations;

namespace Meti.Domain.Models
{
    public class AlarmMetric : EntityBase<Guid?>
    {
        [Required, StringLength(255)]
        public virtual string Metric { get; set; }

        public virtual decimal? ThresholdMin { get; set; }

        public virtual decimal? ThresholdMax { get; set; }

        public virtual Device Device { get; set; }

        public virtual Alarm Alarm { get; set; }

        public AlarmMetric()
        {
        }
    }
}
