//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.ProcessInstance;
using Meti.Domain.Models;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Meti.Domain.Services
{
    public interface IProcessInstanceService : IServiceNHibernateBase
    {
        OperationResult<Guid?> CreateProcessInstance(ProcessInstanceEditDto dto);

        OperationResult<Guid?> UpdateProcessInstance(ProcessInstanceEditDto dto);

        OperationResult<Guid?> DeleteProcessInstance(Guid? id);

        OperationResult<Guid?> CloseProcessInstance(Guid? id);

        OperationResult<Guid?> OpenProcessInstance(Guid? id);

        IList<ProcessInstance> Fetch(string name, Guid? doctorId, Guid? processId, Guid? patientId, ProcessInstanceState? processInstanceState, PaginationModel pagination, OrderByModel orderBy);

        int Count(string name, Guid? doctorId, Guid? processId, Guid? patientId, ProcessInstanceState? processInstanceState);

        IList<ProcessInstance> GetByRegistry(Guid? registryId);

        IList<ProcessInstance> GetByRegistryByEmail(string email);

        OperationResult<Guid?> UpdatePositionLast(Guid? processInstanceId, string latitude, string longitude);
    }
}