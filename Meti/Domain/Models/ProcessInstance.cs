//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Entity.Base;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Meti.Domain.Models
{
    public class ProcessInstance : EntityBase<Guid?>
    {
        [StringLength(255)]
        public virtual string Name { get; set; }

        [Required]
        public virtual IList<Registry> Doctors { get; set; }

        [Required]
        public virtual Registry Patient { get; set; }

        public virtual IList<Registry> ReferencePersons { get; set; }

        public virtual ProcessInstanceState? ProcessInstanceState { get; set; }        
        public virtual DateTime? ReOpenDate { get; set; }

        [StringLength(255)]
        public virtual string ReOpenFrom { get; set; }

        public virtual DateTime? CloseDate { get; set; }

        [StringLength(255)]
        public virtual string CloseFrom { get; set; }

        [Required]
        public virtual Process Process { get; set; }

        public ProcessInstance()
        {
            Doctors = new List<Registry>();
            ReferencePersons = new List<Registry>();
        }
    }
}