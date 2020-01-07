//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Models;
using MateSharp.Framework.Repository;
using Meti.Domain.Models;
using System;
using System.Collections.Generic;

namespace Meti.Domain.Repository
{
    public interface IProcessMacroRepository : IRepositoryNHibernateBase<ProcessMacro>
    {
        IList<ProcessMacro> Fetch(string name, string value, Guid? processId, PaginationModel pagination, OrderByModel orderBy);
        int Count(string name, string value, Guid? processId);
    }
}
