//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System;
using MateSharp.Framework.Models;
using MateSharp.Framework.Repository;
using Meti.Domain.Models;
using System.Collections.Generic;

namespace Meti.Domain.Repository
{
    public interface IDeviceRepository : IRepositoryNHibernateBase<Device>
    {
        IList<Device> Fetch(string name, string macAddress, Guid? processInstanceId, PaginationModel pagination, OrderByModel orderBy);
        IList<Device> FetchWithoutProcessInstanceId(Guid? processInstanceId, PaginationModel pagination, OrderByModel orderBy);
        int Count(string name, string macAddress, Guid? processInstanceId);
    }
}
