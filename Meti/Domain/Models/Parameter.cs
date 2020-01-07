//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Entity.Base;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Meti.Domain.Models
{
    public class Parameter : EntityBase<Guid?>
    {
        [Required, StringLength(255)]
        public virtual string Name { get; set; }
        
        public virtual Process Process { get; set; }
        
        [Required]
        public virtual int? Frequency { get; set; }

        [Required]
        public virtual FrequencyType? FrequencyType { get; set; }

        [Required]
        public virtual bool? IsEnabled { get; set; }

        public virtual IList<Alarm> Alarms { get; set; }
        
        public Parameter()
        {
            IsEnabled = true;
            Alarms = new List<Alarm>();
        }

    }
}
