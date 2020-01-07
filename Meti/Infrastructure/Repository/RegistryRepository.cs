//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Extensions.NHibernate;
using MateSharp.Framework.Models;
using MateSharp.Framework.Repository.NHibernate;
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
    public class RegistryRepository : RepositoryNHibernateBase<Registry>, IRegistryRepository
    {
        public RegistryRepository(ISession session) : base(session)
        {
        }

        public RegistryRepository() : base()
        {
        }

        public IList<Registry> Fetch(string firstname, string surname, SexType? sex, RegistryType? registryType, PaginationModel pagination, OrderByModel orderBy)
        {
            var queryOver = BuildQueryOver(firstname, surname, sex, registryType);

            queryOver = orderBy != null && orderBy.IsValid()
                ? queryOver.OrderByString(orderBy)
                : queryOver.OrderBy(e => e.Surname).Asc;

            return queryOver.Skip(pagination?.StartRowIndex ?? 0).Take(pagination?.MaxRowIndex ?? _maxRowIndexDefault).List();
        }

        public int Count(string firstname, string surname, SexType? sex, RegistryType? registryType)
        {
            var queryOver = BuildQueryOver(firstname, surname, sex, registryType);

            return queryOver.RowCount();
        }

        private IQueryOver<Registry, Registry> BuildQueryOver(string firstname, string surname, SexType? sex, RegistryType? registryType)
        {
            var queryOver = Session.QueryOver<Registry>();

            if (!string.IsNullOrWhiteSpace(firstname))
                queryOver = queryOver.Where(e => e.Firstname.IsInsensitiveLike(firstname, MatchMode.Start));

            if (!string.IsNullOrWhiteSpace(surname))
                queryOver = queryOver.Where(e => e.Surname.IsInsensitiveLike(surname, MatchMode.Start));

            if (sex.HasValue)
                queryOver = queryOver.Where(e => e.Sex == sex);

            if (registryType.HasValue)
                queryOver = queryOver.Where(e => e.RegistryType == registryType);

            return queryOver;
        }

        public IList<ProcessInstance> FetchByProcessInstanceIds(List<Guid?> processInstanceIds)
        {
            var queryOverProcessInstance = Session.QueryOver<ProcessInstance>()
                .WhereRestrictionOn(e => e.Id).IsIn(processInstanceIds)
                .Fetch(e => e.Patient).Eager;

            return queryOverProcessInstance.List();
        }
    }
}