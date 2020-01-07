//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Entity.Base;
using Meti.Domain.ValueObjects;
using System;
using System.ComponentModel.DataAnnotations;

namespace Meti.Domain.Models
{
    public class AlarmFired : EntityBase<Guid?>
    {
        public virtual string Patient { get; set; }

        [Required]
        public virtual AlarmColor? AlarmColor { get; set; }

        /// <summary>
        /// Parametro rilevato
        /// </summary>
        [Required]
        public virtual string Result { get; set; }

        //[Required]
        public virtual Parameter Parameter { get; set; }

        [Required]
        public virtual ProcessInstance ProcessInstance { get; set; }

        //[Required]
        public virtual Alarm Alarm { get; set; }
        /// <summary>
        /// Attivo fino a quando l'operatore non spegne l'alert
        /// </summary>
        [Required]
        public virtual bool? IsActive { get; set; }

        [StringLength(2000)]
        public virtual string ResolvingNotes { get; set; }

        [StringLength(255)]
        public virtual string Latitude { get; set; }
        [StringLength(255)]
        public virtual string Longitude { get; set; }
        [Required]
        public virtual bool? IsFall { get; set; }
        [Required]
        public virtual bool? IsFallNoResponse { get; set; }

        [StringLength(2000)]
        public virtual string Feedback { get; set; }
        public virtual DateTime? FeedbackDate { get; set; }
    
        [StringLength(255)]
        public virtual string FeedbackBy { get; set; }

        [StringLength(255)]
        public virtual string PatientFeedback { get; set; }
        
    }
}
