//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System;

namespace Meti.Application.Dtos.User
{
    public class InviteFriendDto
    {
        public Guid? Id { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public bool? ShowPeso { get; set; }
        public bool? ShowGlicemia { get; set; }
        public bool? ShowFrequenza { get; set; }
        public bool? ShowPressione { get; set; }
        public bool? ShowTemperatura { get; set; }
        public bool? ShowCamera { get; set; }
        public bool? ShowOssigeno { get; set; }
        public Guid? ProcessInstanceId { get; set; }
    }
}