//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System;
using MateSharp.Framework.Models;
using MateSharp.Framework.Repository;
using Meti.Domain.Models;
using Meti.Domain.ValueObjects;
using System.Collections.Generic;

namespace Meti.Domain.Repository
{
    public interface IInviteFriendRepository : IRepositoryNHibernateBase<InviteFriend>
    {
        InviteFriend GetByProcessInstance(Guid? processInstanceId);

        IList<InviteFriend> Fetch(Guid? processInstanceId, PaginationModel pagination, OrderByModel orderBy);

        int Count(Guid? processInstanceId);
    }
}
