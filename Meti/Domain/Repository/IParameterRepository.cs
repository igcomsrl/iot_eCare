//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System;
using MateSharp.Framework.Models;
using MateSharp.Framework.Repository;
using Meti.Domain.Models;
using Meti.Domain.ValueObjects;
using System.Collections.Generic;
using Meti.Application.Dtos.Parameter;

namespace Meti.Domain.Repository
{
    public interface IParameterRepository : IRepositoryNHibernateBase<Parameter>
    {
        IList<Parameter> Fetch(string name, Guid? processId, Guid? deviceId, int? frequency, FrequencyType? frequencyType, PaginationModel pagination, OrderByModel orderBy);
        int Count(string name, Guid? processId, Guid? deviceId, int? frequency, FrequencyType? frequencyType);        
        IList<ParameterSensorDto> GetByDevice(string uuid);
    }
}
