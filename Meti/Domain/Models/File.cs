//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Entity.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Meti.Domain.Models
{
    public class File : EntityBase<Guid?>
    {
        [Required, StringLength(500)]
        public virtual string FilepathPhysical { get; set; }
        [Required, StringLength(500)]
        public virtual string FilepathVirtual { get; set; }
        [Required, StringLength(500)]
        public virtual string Name { get; set; }
        [StringLength(500)]
        public virtual string Type { get; set; }
        [StringLength(500)]
        public virtual string Size { get; set; }
    }
}
