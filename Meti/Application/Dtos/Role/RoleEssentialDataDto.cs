//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using MateSharp.RoleClaim.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meti.Application.Dtos.Role
{
    public class RoleEssentialDataDto
    {
        public IList<UserDto> Users { get; set; }
        public IList<ClaimDto> Claims { get; set; }
    }
}
