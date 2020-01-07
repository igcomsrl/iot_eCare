//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Meti.Domain.Models
{
    public class Device : EntityBase<Guid?>
    {
        [Required, StringLength(255)]
        public virtual string Macaddress { get; set; }

        [StringLength(255)]
        public virtual string Name { get; set; }

        [Required]
        public virtual bool? IsEnabled { get; set; }

        public virtual IList<AlarmMetric> AlarmMetrics { get; set; }

        public Device()
        {
            IsEnabled = true;
            AlarmMetrics = new List<AlarmMetric>();
        }
    }
}