//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Models;
using MateSharp.Framework.Repository;
using Meti.Domain.Models;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Meti.Domain.Repository
{
    public interface IProcessInstanceRepository : IRepositoryNHibernateBase<ProcessInstance>
    {
        IList<ProcessInstance> Fetch(string name, Guid? doctorId, Guid? patientId, Guid? processId, ProcessInstanceState? processInstanceState, PaginationModel pagination, OrderByModel orderBy);
        int Count(string name, Guid? doctorId, Guid? patientId, Guid? processId, ProcessInstanceState? processInstanceState);
        IList<ProcessInstance> GetByRegistry(Guid? registryId);    
        IList<ProcessInstance> GetByRegistryByEmail(string email);
        IList<ProcessInstance> FetchByDevice(IList<Guid?> devices);
    }
}
