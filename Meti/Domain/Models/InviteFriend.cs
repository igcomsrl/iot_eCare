//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.DataAnnotations;
using MateSharp.Framework.Entity.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Meti.Domain.Models
{
    [UniqueGuid("Email")]
    public class InviteFriend : EntityBase<Guid?>
    {
        [Required, StringLength(500)]
        public virtual string Email { get; set; }
        [Required, StringLength(500)]
        public virtual string Surname { get; set; }
        [Required, StringLength(500)]
        public virtual string Firstname { get; set; }
        [Required]
        public virtual bool? ShowPeso { get; set; }
        [Required]
        public virtual bool? ShowGlicemia { get; set; }
        [Required]
        public virtual bool? ShowFrequenza { get; set; }
        [Required]
        public virtual bool? ShowPressione { get; set; }
        [Required]
        public virtual bool? ShowTemperatura { get; set; }
        [Required]
        public virtual bool? ShowOssigeno { get; set; }
        [Required]
        public virtual bool? ShowCamera { get; set; }
        public virtual ProcessInstance ProcessInstance { get; set; }

        public InviteFriend()
        {
            ShowPeso = true;
            ShowGlicemia = true;
            ShowFrequenza = true;
            ShowPressione = true;
            ShowTemperatura = true;
            ShowCamera = true;
            ShowOssigeno = true;
        }
    }
}
