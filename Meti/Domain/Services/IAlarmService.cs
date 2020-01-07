//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.Alarm;
using Meti.Domain.Models;
using System;
using System.Collections.Generic;

namespace Meti.Domain.Services
{
    public interface IAlarmService : IServiceNHibernateBase
    {
        OperationResult<Guid?> CreateAlarm(AlarmEditDto dto);

        OperationResult<Guid?> UpdateAlarm(AlarmEditDto dto);

        OperationResult<Guid?> DeleteAlarm(Guid? id);
    }
}
