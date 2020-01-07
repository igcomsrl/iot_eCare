//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using AutoMapper;
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
    public class InviteFriendRepository : RepositoryNHibernateBase<InviteFriend>, IInviteFriendRepository
    {
        public InviteFriendRepository(ISession session) : base(session)
        {
        }

        public InviteFriendRepository() : base()
        {
        }


        public InviteFriend GetByProcessInstance(Guid? processInstanceId)
        {
            ProcessInstance processInstanceAlias = null;

            var queryOver = Session.QueryOver<InviteFriend>();

            if (processInstanceId.HasValue)
            {
                queryOver.JoinAlias(e => e.ProcessInstance, () => processInstanceAlias).Where(() => processInstanceAlias.Id == processInstanceId);
            }

            return queryOver.SingleOrDefault();
        }


        public IList<InviteFriend> Fetch(Guid? processInstanceId, PaginationModel pagination, OrderByModel orderBy)
        {
            var queryOver = BuildQueryOver(processInstanceId);

            queryOver = orderBy != null && orderBy.IsValid()
                ? queryOver.OrderByString(orderBy)
                : queryOver.OrderBy(e => e.Surname).Asc;

            return queryOver.Skip(pagination?.StartRowIndex ?? 0).Take(pagination?.MaxRowIndex ?? _maxRowIndexDefault).List();
        }

        public int Count(Guid? processInstanceId)
        {
            var queryOver = BuildQueryOver(processInstanceId);

            return queryOver.RowCount();
        }

        private IQueryOver<InviteFriend, InviteFriend> BuildQueryOver(Guid? processInstanceId)
        {
            ProcessInstance processInstanceAlias = null;

            var queryOver = Session.QueryOver<InviteFriend>();
            
            if (processInstanceId.HasValue)
            {
                queryOver.JoinAlias(e => e.ProcessInstance, () => processInstanceAlias).Where(()=> processInstanceAlias.Id == processInstanceId);
            }

            return queryOver;
        }

    }
}

