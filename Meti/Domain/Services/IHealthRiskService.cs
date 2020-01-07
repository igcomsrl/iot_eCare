//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.Process;
using Meti.Domain.Models;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Meti.Domain.Services
{
    public interface IHealthRiskService : IServiceNHibernateBase
    {
        OperationResult<Guid?> CreateHealthRisk(HealthRiskEditDto dto);

        OperationResult<Guid?> UpdateHealthRisk(HealthRiskEditDto dto);

        OperationResult<Guid?> DeleteHealthRisk(Guid? id);

        IList<HealthRisk> Fetch(HealthRiskType? type, HealthRiskLevel? level, string rating, DateTime? startDate, DateTime? endDate, bool? isLast, Guid? registryId, PaginationModel pagination, OrderByModel orderBy);        

        int Count(HealthRiskType? type, HealthRiskLevel? level, string rating, DateTime? startDate, DateTime? endDate, bool? isLast, Guid? registryId);

        IList<HealthRiskDetailDto> FetchByProcessInstanceIds(List<Guid?> processInstanceIds);
    }
}
