//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Extensions.NHibernate;
using MateSharp.Framework.Models;
using MateSharp.Framework.Repository.NHibernate;
using Meti.Application.Dtos.Process;
using Meti.Domain.Models;
using Meti.Domain.Repository;
using Meti.Domain.ValueObjects;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Meti.Infrastructure.Repository
{
    public class HealthRiskRepository : RepositoryNHibernateBase<HealthRisk>, IHealthRiskRepository
    {
        public HealthRiskRepository(ISession session) : base(session)
        {
        }

        public HealthRiskRepository() : base()
        {
        }

        public IList<HealthRisk> Fetch(HealthRiskType? type, HealthRiskLevel? level, string rating, DateTime? startDate, DateTime? endDate, bool? isLast, Guid? registryId, PaginationModel pagination, OrderByModel orderBy)
        {
            var queryOver = BuildQueryOver(type, level, rating, startDate, endDate, isLast, registryId);

            queryOver = orderBy != null && orderBy.IsValid()
                ? queryOver.OrderByString(orderBy)
                : queryOver.OrderBy(e => e.Type).Asc;

            return queryOver.Skip(pagination?.StartRowIndex ?? 0).Take(pagination?.MaxRowIndex ?? _maxRowIndexDefault).List();
        }

        public int Count(HealthRiskType? type, HealthRiskLevel? level, string rating, DateTime? startDate, DateTime? endDate, bool? isLast, Guid? registryId)
        {
            var queryOver = BuildQueryOver(type, level, rating, startDate, endDate, isLast, registryId);

            return queryOver.RowCount();
        }

        private IQueryOver<HealthRisk, HealthRisk> BuildQueryOver(HealthRiskType? type, HealthRiskLevel? level, string rating, DateTime? startDate, DateTime? endDate, bool? isLast, Guid? registryId)
        {            
            Registry registryAlias = null;

            var queryOver = Session.QueryOver<HealthRisk>();

            if (type.HasValue)
                queryOver = queryOver.Where(e => e.Type == type);

            if (level.HasValue)
                queryOver = queryOver.Where(e => e.Level == level);

            if (!string.IsNullOrWhiteSpace(rating))
                queryOver = queryOver.Where(e => e.Rating == rating);

            if (startDate.HasValue)
            {
                startDate = startDate.Value.Date + new TimeSpan(0, 0, 0);
                queryOver = queryOver.Where(e => e.StartDate.Value.Date >= startDate);
            }

            if (endDate.HasValue)
            {
                endDate = endDate.Value.Date + new TimeSpan(0, 0, 0);
                queryOver = queryOver.Where(e => e.EndDate.Value.Date <= endDate);
            }

            if (registryId.HasValue)
            {
                queryOver.JoinAlias(e => e.Registry, () => registryAlias).Where(()=>registryAlias.Id == registryId);                
            }

            return queryOver;
        }

        public IList<HealthRiskDetailDto> FetchByProcessInstanceIds(List<Guid?> processInstanceIds)
        {
            var queryOverProcessInstance = Session.QueryOver<ProcessInstance>()
                .Fetch(e => e.Patient).Eager;

            if (processInstanceIds != null && processInstanceIds.Count > 0)
            {
                queryOverProcessInstance = queryOverProcessInstance
                    .WhereRestrictionOn(e => e.Id).IsIn(processInstanceIds);
            }

            var processInstanceList = queryOverProcessInstance.List();

            var patientList = processInstanceList.Select(e => e.Patient).ToList();

            var queryOverHealthRisk = Session.QueryOver<HealthRisk>()
               .WhereRestrictionOn(e => e.Registry.Id)
               .IsIn(patientList.Select(p => p.Id).ToArray()).List();

            var results = new List<HealthRiskDetailDto>();

            foreach (var healthRisk in queryOverHealthRisk)
            {
                var dto = AutoMapper.Mapper.Map<HealthRisk, HealthRiskDetailDto>(healthRisk);
                Guid? processInstanceId = processInstanceList.SingleOrDefault(e => e.Patient.Id == healthRisk.Registry.Id).Id;
                dto.ProcessInstanceId = processInstanceId;
                dto.SexType = healthRisk.Registry.Sex;
                dto.LifeStyle = healthRisk.Registry.LifeStyle;
                dto.Registry = null;
                results.Add(dto);
                
            }

            return results;
        }
    }
}