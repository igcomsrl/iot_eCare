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

namespace Meti.Infrastructure.Repository
{
    public class ProcessRepository : RepositoryNHibernateBase<Process>, IProcessRepository
    {
        public ProcessRepository(ISession session) : base(session)
        {
        }

        public ProcessRepository() : base()
        {
        }

        public IList<Process> Fetch(string name, ProcessType? processType, PaginationModel pagination, OrderByModel orderBy)
        {
            var queryOver = BuildQueryOver(name, processType);

            queryOver = orderBy != null && orderBy.IsValid()
                ? queryOver.OrderByString(orderBy)
                : queryOver.OrderBy(e => e.Name).Asc;

            return queryOver.Skip(pagination?.StartRowIndex ?? 0).Take(pagination?.MaxRowIndex ?? _maxRowIndexDefault).List();
        }

        public int Count(string name, ProcessType? processType)
        {
            var queryOver = BuildQueryOver(name, processType);

            return queryOver.RowCount();
        }

        private IQueryOver<Process, Process> BuildQueryOver(string name, ProcessType? processType)
        {
            var queryOver = Session.QueryOver<Process>();

            if (!string.IsNullOrWhiteSpace(name))
                queryOver = queryOver.Where(e => e.Name.IsInsensitiveLike(name, MatchMode.Start));

            if (processType.HasValue)
                queryOver = queryOver.Where(e => e.ProcessType == processType);

            return queryOver;
        }
    }
}