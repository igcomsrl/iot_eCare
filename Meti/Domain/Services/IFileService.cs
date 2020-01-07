//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.File;
using Meti.Domain.Models;
using System;

namespace Meti.Domain.Services
{
    public interface IFileService : IServiceNHibernateBase
    {
        OperationResult<File> CreateFile(FileDto dto, Guid? registryId);

        OperationResult<File> DeleteFile(FileDto dto, Guid? registryId);
    }
}
