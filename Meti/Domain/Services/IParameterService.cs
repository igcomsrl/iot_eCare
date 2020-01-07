//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.Parameter;
using Meti.Domain.Models;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Meti.Domain.Services
{
    public interface IParameterService : IServiceNHibernateBase
    {
        OperationResult<Guid?> CreateParameter(ParameterEditDto dto);

        OperationResult<Guid?> UpdateParameter(ParameterEditDto dto);

        OperationResult<Guid?> DeleteParameter(Guid? id);

        IList<Parameter> Fetch(string name, Guid? processId, Guid? deviceId, int? frequency, FrequencyType? frequencyType, PaginationModel pagination, OrderByModel orderBy);

        int Count(string name, Guid? processId, Guid? deviceId, int? frequency, FrequencyType? frequencyType);        
        //IList<ParameterSensorDetailDto> GetByDeviceDetail(string uuid);
        IList<ParameterSensorDto> GetByDevice(string uuid);
    }
}
