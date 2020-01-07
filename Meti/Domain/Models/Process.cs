//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Entity.Base;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Meti.Domain.Models
{
    public class Process : EntityBase<Guid?>
    {
        [Required, StringLength(255)]
        public virtual string Name { get; set; }

        public virtual IList<ProcessMacro> ProcessMacros { get; set; }

        public virtual IList<Parameter> Parameters { get; set; }

        [Required]
        public virtual bool? IsEnabled { get; set; }

        [Required]
        public virtual ProcessType? ProcessType { get; set; }

        public Process()
        {
            ProcessMacros = new List<ProcessMacro>();
            Parameters = new List<Parameter>();
            IsEnabled = true;
        }
    }
}