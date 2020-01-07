//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Domain.Services;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.Device;
using Meti.Domain.Models;
using Meti.Domain.Repository;
using Meti.Domain.Services;
using Meti.Infrastructure.Repository;
using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Meti.Application.Services
{
    public class DeviceErrorLogService : ServiceNHibernateBase, IServiceNHibernateBase, IDeviceErrorLogService
    {
        #region Private fields

        private readonly IDeviceErrorLogRepository _deviceErrorLogRepository;

        #endregion Private fields

        #region Costructors

        public DeviceErrorLogService(ISession session)
            : base(session)
        {
            _deviceErrorLogRepository = new DeviceErrorLogRepository(session);
        }

        public DeviceErrorLogService(IDeviceErrorLogRepository DeviceErrorLogRepository)
        {
            _deviceErrorLogRepository = DeviceErrorLogRepository;
        }

        #endregion Costructors

        #region Services

        public OperationResult<Guid?> CreateDeviceErrorLog(DeviceErrorLogDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            DeviceErrorLog entity = new DeviceErrorLog();
            entity.Error = dto.Error;
            entity.DeviceId = dto.DeviceId;
            entity.ProcessInstanceId = dto.ProcessInstanceId;

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _deviceErrorLogRepository.Save(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }

     

        #endregion Services

        #region Dispose

        protected override void Dispose(bool isDisposing)
        {
            //Se sto facendo la dispose
            if (isDisposing)
            {
                //Rilascio le risorse locali
                _deviceErrorLogRepository.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose
    }
}