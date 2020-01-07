//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System;
using System.Collections.Generic;
using MateSharp.Framework.Extensions.NHibernate;
using MateSharp.Framework.Models;
using MateSharp.Framework.Repository.NHibernate;
using Meti.Domain.Models;
using Meti.Domain.Repository;
using NHibernate;
using NHibernate.Criterion;

namespace Meti.Infrastructure.Repository
{
    public class ProcessMacroRepository : RepositoryNHibernateBase<ProcessMacro>, IProcessMacroRepository
    {
        public ProcessMacroRepository(ISession session) : base(session)
        {
        }

        public ProcessMacroRepository() : base()
        {
        }

        public IList<ProcessMacro> Fetch(string name, string value, Guid? processId, PaginationModel pagination, OrderByModel orderBy)
        {
            var queryOver = BuildQueryOver(name, value, processId);

            queryOver = orderBy != null && orderBy.IsValid()
                ? queryOver.OrderByString(orderBy)
                : queryOver.OrderBy(e => e.Name).Asc;

            return queryOver.Skip(pagination?.StartRowIndex ?? 0).Take(pagination?.MaxRowIndex ?? _maxRowIndexDefault).List();
        }

        public int Count(string name, string value, Guid? processId)
        {
            var queryOver = BuildQueryOver(name, value, processId);

            return queryOver.RowCount();
        }

        private IQueryOver<ProcessMacro, ProcessMacro> BuildQueryOver(string name, string value, Guid? processId)
        {
            Process processAlias = null;

            var queryOver = Session.QueryOver<ProcessMacro>();

            if (!string.IsNullOrWhiteSpace(name))
                queryOver = queryOver.Where(e => e.Name == name);

            if (!string.IsNullOrWhiteSpace(value))
                queryOver = queryOver.Where(e => e.Value == value);

            if (processId.HasValue)
                queryOver = queryOver.JoinAlias(e => e.Process, () => processAlias).Where(e => e.Process.Id == processId);

            return queryOver;
        }
    }
}