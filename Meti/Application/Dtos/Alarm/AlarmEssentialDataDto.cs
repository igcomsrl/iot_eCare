//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using MateSharp.Framework.Dtos;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meti.Application.Dtos.Alarm
{
    public class AlarmEssentialDataDto
    {
        public IList<ItemDto<AlarmColor?>> AlarmColorList { get; set; }
    }
}
