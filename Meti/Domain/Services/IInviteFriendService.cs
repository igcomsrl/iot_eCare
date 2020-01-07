//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.User;
using Meti.Domain.Models;
using System;
using System.Collections.Generic;

namespace Meti.Domain.Services
{
    public interface IInviteFriendService : IServiceNHibernateBase
    {
        OperationResult<object> CreateInviteFriend(InviteFriendDto dto);

        OperationResult<object> UpdateInviteFriend(InviteFriendDto dto);

        OperationResult<object> DeleteInviteFriend(Guid? id);

        IList<InviteFriend> Fetch(Guid? processInstanceId, PaginationModel pagination, OrderByModel orderBy);

        InviteFriend GetByProcessInstance(Guid? processInstanceId);

        int Count(Guid? processInstanceId);
    }
}