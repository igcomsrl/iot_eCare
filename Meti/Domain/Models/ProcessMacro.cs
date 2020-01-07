//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Entity.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Meti.Domain.Models
{
    public class ProcessMacro : EntityBase<Guid?>
    {
        [Required]
        public virtual Process Process { get; set; }
        [Required, StringLength(255)]
        public virtual string Name { get; set; }
        [Required, StringLength(255)]
        public virtual string Value { get; set; }
    }
}