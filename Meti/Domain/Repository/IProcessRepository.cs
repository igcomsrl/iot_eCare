//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Models;
using MateSharp.Framework.Repository;
using Meti.Domain.Models;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Meti.Domain.Repository
{
    public interface IProcessRepository : IRepositoryNHibernateBase<Process>
    {
        IList<Process> Fetch(string name, ProcessType? processType, PaginationModel pagination, OrderByModel orderBy);
        int Count(string name, ProcessType? processType);
    }
}
