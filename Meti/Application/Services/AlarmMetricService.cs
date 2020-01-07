//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Domain.Services;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.Alarm;
using Meti.Application.Dtos.AlarmMetric;
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
    public class AlarmMetricService : ServiceNHibernateBase, IServiceNHibernateBase, IAlarmMetricService
    {
        #region Private fields

        private readonly IAlarmMetricRepository _alarmMetricRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IAlarmRepository _alarmRepository;

        #endregion Private fields

        #region Costructors

        public AlarmMetricService(ISession session)
            : base(session)
        {
            _alarmMetricRepository = new AlarmMetricRepository(session);
            _deviceRepository = new DeviceRepository(session);
            _alarmRepository = new AlarmRepository(session);
        }

        public AlarmMetricService(IAlarmMetricRepository AlarmMetricRepository, IDeviceRepository DeviceRepository, IAlarmRepository alarmRepository)
        {
            _alarmMetricRepository = AlarmMetricRepository;
            _deviceRepository = DeviceRepository;
            _alarmRepository = alarmRepository;
        }

        #endregion Costructors

        #region Services

        public OperationResult<Guid?> CreateAlarmMetric(AlarmMetricEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            AlarmMetric entity = new AlarmMetric();
            entity.Metric = dto.Metric;
            entity.Device = dto.Device != null && dto.Device.Id.HasValue ? _deviceRepository.Load(dto.Device.Id) : null;
            entity.Alarm = dto.Alarm.HasValue ? _alarmRepository.Load(dto.Alarm) : null;
            entity.ThresholdMax = dto.ThresholdMax;
            entity.ThresholdMin = dto.ThresholdMin;

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _alarmMetricRepository.Save(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }

        public OperationResult<Guid?> UpdateAlarmMetric(AlarmMetricEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (!dto.Id.HasValue) throw new ArgumentNullException(nameof(dto.Id));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            AlarmMetric entity = _alarmMetricRepository.Load(dto.Id);
            entity.Metric = dto.Metric;
            entity.Device = dto.Device != null && dto.Device.Id.HasValue ? _deviceRepository.Load(dto.Device.Id) : null;
            entity.Alarm = dto.Alarm.HasValue ? _alarmRepository.Load(dto.Alarm) : null;
            entity.ThresholdMax = dto.ThresholdMax;
            entity.ThresholdMin = dto.ThresholdMin;

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _alarmMetricRepository.Save(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }

        public OperationResult<Guid?> DeleteAlarmMetric(Guid? id)
        {
            //Validazione argomenti
            if (!id.HasValue) throw new ArgumentNullException(nameof(id));

            //Dichiaro la listsa di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            AlarmMetric entity = _alarmMetricRepository.Load(id);

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _alarmMetricRepository.Delete(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = null,
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
                _deviceRepository.Dispose();
                _alarmMetricRepository.Dispose();
                _alarmRepository.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose
    }
}