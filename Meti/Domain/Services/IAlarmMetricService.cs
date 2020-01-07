//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.AlarmMetric;
using System;

namespace Meti.Domain.Services
{
    public interface IAlarmMetricService : IServiceNHibernateBase
    {
        OperationResult<Guid?> CreateAlarmMetric(AlarmMetricEditDto dto);

        OperationResult<Guid?> UpdateAlarmMetric(AlarmMetricEditDto dto);

        OperationResult<Guid?> DeleteAlarmMetric(Guid? id);
    }
}