//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System.Collections.Generic;
using MateSharp.Framework.Dtos;
using Meti.Domain.ValueObjects;

namespace Meti.Application.Dtos.Registry
{
    public class RegistryEssentialDataDto
    {
        public IList<ItemDto<SexType?>> SexList { get; set; }
        public IList<ItemDto<RegistryType?>> RegistryTypeList { get; set; }
        public IList<ItemDto<BloodGroup?>> BloodGroupList { get; set; }
        public IList<ItemDto<LifeStyle?>> LifeStyleList { get; set; }
    }
}
