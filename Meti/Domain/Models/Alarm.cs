//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Entity.Base;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Meti.Domain.Models
{
    public class Alarm : EntityBase<Guid?>
    {
        [Required, StringLength(255)]
        public virtual string Name { get; set; }
        public virtual Parameter Parameter { get; set; }
        
        public virtual AlarmColor? AlarmColor { get; set; }

        /// <summary>
        /// Indicare le email separate da ;. paolo.rossi@libero.it;b.tedesco@gmail.com
        /// </summary>
        [StringLength(1000)]
        public virtual string Emails { get; set; }

        /// <summary>
        /// Indicare i numeri da contattare separati da ; 333333333;33333300303
        /// </summary>
        public virtual string SmsNumbers { get; set; }
        
        public virtual bool? ContactOperator { get; set; }

        [Required]
        public virtual bool? IsEnabled { get; set; }

        public virtual string HelpMessage { get; set; }

        public virtual IList<AlarmMetric> AlarmMetrics { get; set; }

        public Alarm()
        {
            IsEnabled = true;
            AlarmMetrics = new List<AlarmMetric>();
        }
    }
}
