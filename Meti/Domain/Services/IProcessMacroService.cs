//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.ProcessMacro;
using Meti.Domain.Models;
using System;
using System.Collections.Generic;

namespace Meti.Domain.Services
{
    public interface IProcessMacroService : IServiceNHibernateBase
    {
        OperationResult<Guid?> CreateProcessMacro(ProcessMacroEditDto dto);

        OperationResult<Guid?> UpdateProcessMacro(ProcessMacroEditDto dto);

        OperationResult<Guid?> DeleteProcessMacro(Guid? id);

        IList<ProcessMacro> Fetch(string name, string value, Guid? processId, PaginationModel pagination, OrderByModel orderBy);

        int Count(string name, string value, Guid? processId);
    }
}
