//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.AlarmFired;
using Meti.Domain.Models;
using System;
using System.Collections.Generic;

namespace Meti.Domain.Services
{
    public interface IAlarmFiredService : IServiceNHibernateBase
    {
        OperationResult<Guid?> CreateAlarmFired(AlarmFiredEditDto dto);

        OperationResult<Guid?> TurnOff(AlarmFiredEditDto dto);

        OperationResult<Guid?> DeleteAlarmFired(Guid? id);
    }
}
