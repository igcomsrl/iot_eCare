//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Domain.Services;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.Parameter;
using Meti.Domain.Models;
using Meti.Domain.Repository;
using Meti.Domain.Services;
using Meti.Domain.ValueObjects;
using Meti.Infrastructure.Repository;
using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Meti.Application.Services
{
    public class ParameterService : ServiceNHibernateBase, IServiceNHibernateBase, IParameterService
    {
        #region Private fields

        private readonly IParameterRepository _parameterRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IProcessRepository _processRepository;
        private readonly IAlarmService _alarmService;

        #endregion private fields

        #region Costructors

        public ParameterService(ISession session)
            : base(session)
        {
            _parameterRepository = new ParameterRepository(session);
            _processRepository = new ProcessRepository(session);
            _deviceRepository = new DeviceRepository(session);
            _alarmService = new AlarmService(session);
        }

        public ParameterService(IParameterRepository parameterRepository, IProcessRepository processRepository, IDeviceRepository deviceRepository, IAlarmService alarmService)
        {
            _parameterRepository = parameterRepository;
            _deviceRepository = deviceRepository;
            _processRepository = processRepository;
            _alarmService = alarmService;
        }

        #endregion Costructors

        #region Services

        public OperationResult<Guid?> CreateParameter(ParameterEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            Parameter entity = new Parameter();
            entity.Name = dto.Name;
            //entity.Device = dto.Device != null && dto.Device.Id.HasValue ? _deviceRepository.Load(dto.Device.Id): null;
            entity.Process = dto.Process.HasValue ? _processRepository.Load(dto.Process) : null;
            entity.Frequency = dto.Frequency;
            entity.FrequencyType = dto.FrequencyType?.Id;
            entity.IsEnabled = !dto.IsEnabled.HasValue ? false : dto.IsEnabled;
            //entity.PositionMisure = dto.PositionMisure;

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _parameterRepository.Save(entity);
            }

            if (dto.Alarms != null && dto.Alarms.Count > 0)
            {
                entity.Alarms.Clear();
                foreach (var item in dto.Alarms)
                {
                    item.Parameter = entity.Id;
                    var oResult = _alarmService.CreateAlarm(item);
                    if (oResult.HasErrors())
                    {
                        return new OperationResult<Guid?>
                        {
                            ValidationResults = oResult.ValidationResults
                        };
                    }
                    var alarm = _alarmService.Load<Alarm, Guid?>(oResult.ReturnedValue);
                    entity.Alarms.Add(alarm);
                }
            }

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _parameterRepository.Save(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }

        public OperationResult<Guid?> UpdateParameter(ParameterEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (!dto.Id.HasValue) throw new ArgumentNullException(nameof(dto.Id));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            Parameter entity = _parameterRepository.Load(dto.Id);
            entity.Name = dto.Name;
            entity.Process = dto.Process.HasValue ? _processRepository.Load(dto.Process) : null;
            entity.Frequency = dto.Frequency;
            entity.FrequencyType = dto.FrequencyType?.Id;
            entity.IsEnabled = !dto.IsEnabled.HasValue ? false : dto.IsEnabled;

            if (dto.Alarms != null && dto.Alarms.Count > 0)
            {
                entity.Alarms.Clear();
                foreach (var item in dto.Alarms)
                {
                    item.Parameter = entity.Id;
                    var oResult = item.Id.HasValue ? _alarmService.UpdateAlarm(item): _alarmService.CreateAlarm(item);

                    if (oResult.HasErrors())
                    {
                        return new OperationResult<Guid?>
                        {
                            ValidationResults = oResult.ValidationResults
                        };
                    }
                    var alarm = _alarmService.Load<Alarm, Guid?>(oResult.ReturnedValue);
                    entity.Alarms.Add(alarm);
                }
            }

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _parameterRepository.Save(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }


        public OperationResult<Guid?> DeleteParameter(Guid? id)
        {
            //Validazione argomenti            
            if (!id.HasValue) throw new ArgumentNullException(nameof(id));

            //Dichiaro la listsa di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            Parameter entity = _parameterRepository.Load(id);

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _parameterRepository.Delete(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = null,
                ValidationResults = vResults
            };
        }

        public IList<Parameter> Fetch(string name, Guid? processId, Guid? deviceId, int? frequency, FrequencyType? frequencyType, PaginationModel pagination, OrderByModel orderBy)
        {
            var entities = _parameterRepository.Fetch(name, processId, deviceId, frequency, frequencyType, pagination, orderBy);
            return entities;
        }

        public IList<ParameterSensorDto> GetByDevice(string uuid)
        {
            var entity = _parameterRepository.GetByDevice(uuid);
            return entity;
        }

        public int Count(string name, Guid? processId, Guid? deviceId, int? frequency, FrequencyType? frequencyType)
        {
            int count = _parameterRepository.Count(name, processId, deviceId, frequency, frequencyType);
            return count;
        }
        #endregion Services

        #region Dispose

        protected override void Dispose(bool isDisposing)
        {
            //Se sto facendo la dispose
            if (isDisposing)
            {
                //Rilascio le risorse locali
                _parameterRepository.Dispose();
                _processRepository.Dispose();
                _deviceRepository.Dispose();
                _alarmService.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }



        #endregion Dispose
    }
}