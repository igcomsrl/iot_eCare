//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.Process;
using Meti.Domain.Models;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Meti.Domain.Services
{
    public interface IProcessService : IServiceNHibernateBase
    {
        OperationResult<Guid?> CreateProcess(ProcessEditDto dto);

        OperationResult<Guid?> UpdateProcess(ProcessEditDto dto);

        OperationResult<Guid?> DeleteProcess(Guid? id);

        IList<Process> Fetch(string name, ProcessType? processType, PaginationModel pagination, OrderByModel orderBy);

        int Count(string name, ProcessType? processType);
    }
}
