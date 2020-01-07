//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Models;
using Meti.Application.Dtos;
using Meti.Domain.Models;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using Meti.Application.Dtos.Registry;

namespace Meti.Domain.Services
{
    public interface IRegistryService : IServiceNHibernateBase
    {
        OperationResult<Guid?> CreateRegistry(RegistryEditDto dto);
        OperationResult<Guid?> UpdateRegistry(RegistryEditDto dto);
        OperationResult<Guid?> DeleteRegistry(Guid? id);
        IList<Registry> Fetch(string firstname, string surname, SexType? sex, RegistryType? registryType, PaginationModel pagination, OrderByModel orderBy);
        int Count(string firstname, string surname, SexType? sex, RegistryType? registryType);
        IDictionary<Guid?, RegistryDetailDto> FetchByProcessInstanceIds(List<Guid?> processInstanceIds);
    }
}
