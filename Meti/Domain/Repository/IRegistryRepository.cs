//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Models;
using MateSharp.Framework.Repository;
using Meti.Domain.Models;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Meti.Domain.Repository
{
    public interface IRegistryRepository : IRepositoryNHibernateBase<Registry>
    {
        IList<Registry> Fetch(string firstname, string surname, SexType? sex, RegistryType? registryType, PaginationModel pagination, OrderByModel orderBy);
        int Count(string firstname, string surname, SexType? sex, RegistryType? registryType);
        IList<ProcessInstance> FetchByProcessInstanceIds(List<Guid?> processInstanceIds);
    }
}
