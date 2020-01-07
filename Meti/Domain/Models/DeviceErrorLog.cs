//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Meti.Domain.Models
{
    public class DeviceErrorLog : EntityBase<Guid?>
    {
        [Required, StringLength(255)]
        public virtual string Error { get; set; }

        [StringLength(255)]
        public virtual string DeviceId { get; set; }
        
        public virtual Guid? ProcessInstanceId { get; set; }        
    }
}