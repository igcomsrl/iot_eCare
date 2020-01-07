//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System;
using MateSharp.Framework.Models;
using MateSharp.Framework.Repository;
using Meti.Domain.Models;
using System.Collections.Generic;
using Meti.Domain.ValueObjects;
using Meti.Application.Dtos.Process;

namespace Meti.Domain.Repository
{
    public interface IHealthRiskRepository : IRepositoryNHibernateBase<HealthRisk>
    {
        IList<HealthRisk> Fetch(HealthRiskType? type, HealthRiskLevel? level, string rating, DateTime? startDate, DateTime? endDate, bool? isLast, Guid? registryId, PaginationModel pagination, OrderByModel orderBy);
        int Count(HealthRiskType? type, HealthRiskLevel? level, string rating, DateTime? startDate, DateTime? endDate, bool? isLast, Guid? registryId);
        IList<HealthRiskDetailDto> FetchByProcessInstanceIds(List<Guid?> processInstanceIds);
    }
}
