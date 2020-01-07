//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.Device;
using Meti.Domain.Models;
using System;
using System.Collections.Generic;

namespace Meti.Domain.Services
{
    public interface IDeviceService : IServiceNHibernateBase
    {
        OperationResult<Guid?> CreateDevice(DeviceEditDto dto);

        OperationResult<Guid?> UpdateDevice(DeviceEditDto dto);

        OperationResult<Guid?> DeleteDevice(Guid? id);

        IList<DeviceIndexDto> Fetch(string name, string macAddress, Guid? processInstanceId, PaginationModel pagination, OrderByModel orderBy);

        IList<Device> FetchWithoutProcessInstanceId(Guid? processInstanceId, PaginationModel pagination, OrderByModel orderBy);

        int Count(string name, string macAddress, Guid? processInstanceId);
    }
}
