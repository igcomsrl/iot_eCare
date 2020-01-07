//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.Device;
using System;

namespace Meti.Domain.Services
{
    public interface IDeviceErrorLogService : IServiceNHibernateBase
    {
        OperationResult<Guid?> CreateDeviceErrorLog(DeviceErrorLogDto dto);        
    }
}
