//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System;

namespace Meti.Application.Dtos.Process
{
    public class ProcessIndexDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public DateTime? InsertDate { get; set; }
        public bool? IsEnabled { get; set; }
    }
}