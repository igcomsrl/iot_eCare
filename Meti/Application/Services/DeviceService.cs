//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Domain.Services;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.Device;
using Meti.Application.Dtos.ProcessInstance;
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
    public class DeviceService : ServiceNHibernateBase, IServiceNHibernateBase, IDeviceService
    {
        #region Private fields

        private readonly IDeviceRepository _deviceRepository;
        private readonly IProcessInstanceRepository _processInstanceRepository;

        #endregion Private fields

        #region Costructors

        public DeviceService(ISession session)
            : base(session)
        {
            _deviceRepository = new DeviceRepository(session);
            _processInstanceRepository = new ProcessInstanceRepository(session);
        }

        public DeviceService(IDeviceRepository deviceRepository, IProcessInstanceRepository processInstanceRepository)
        {
            _deviceRepository = deviceRepository;
            _processInstanceRepository = processInstanceRepository;
        }

        #endregion Costructors

        #region Services

        public OperationResult<Guid?> CreateDevice(DeviceEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            Device entity = new Device();
            entity.Name = dto.Name;
            entity.Macaddress = dto.Macaddress;
            entity.IsEnabled = !dto.IsEnabled.HasValue ? false : dto.IsEnabled;

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _deviceRepository.Save(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }

        public OperationResult<Guid?> UpdateDevice(DeviceEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (!dto.Id.HasValue) throw new ArgumentNullException(nameof(dto.Id));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();
            //Definisco l'entità
            Device entity = _deviceRepository.Load(dto.Id);
            entity.Name = dto.Name;
            entity.Macaddress = dto.Macaddress;
            entity.IsEnabled = !dto.IsEnabled.HasValue ? entity.IsEnabled : dto.IsEnabled;

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _deviceRepository.Save(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }

        private IList<ValidationResult> ValidateDelete(Device entity)
        {
            var vResults = ValidateEntity(entity);

            if (vResults.Any())
            {
                return vResults;
            }

            //Verifico se ci sono Processi associati.
            var devices = new List<Guid?>();
            devices.Add(entity.Id);

            var processInstances = _processInstanceRepository.FetchByDevice(devices)
                .GroupBy(p => p.Process.Name)
                .Select(g => g.First())
                .ToList();
            if (processInstances.Any())
            {
                var validationErrorMessage = string.Format("Il dispositivo {0} è associato al processo/i {1}. Quindi non potrà essere cancellato",
                    entity.Name + " " + entity.Macaddress,
                    processInstances.Select(e => e.Process.Name).Aggregate((i, j) => i + " / " + j));
                vResults.Add(new ValidationResult(validationErrorMessage));
            }

            return vResults;
        }

        public OperationResult<Guid?> DeleteDevice(Guid? id)
        {
            //Validazione argomenti
            if (!id.HasValue) throw new ArgumentNullException(nameof(id));

            //Dichiaro la listsa di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            Device entity = _deviceRepository.Load(id);

            //Eseguo la validazione logica
            vResults = ValidateDelete(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _deviceRepository.Delete(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = null,
                ValidationResults = vResults
            };
        }

        public IList<DeviceIndexDto> Fetch(string name, string macAddress, Guid? processInstanceId, PaginationModel pagination, OrderByModel orderBy)
        {
            var entities = _deviceRepository.Fetch(name, macAddress, processInstanceId, pagination, orderBy);
            var processInstances = _processInstanceRepository.FetchByDevice(entities.Select(e=>e.Id).ToList());


            IList<DeviceIndexDto> dtos = new List<DeviceIndexDto>();
            foreach (var entity in entities)
            {
                DeviceIndexDto dto = new DeviceIndexDto();
                dto = AutoMapper.Mapper.Map<DeviceIndexDto>(entity);

                foreach (var processInstance in processInstances)
                    foreach (var parameters in processInstance.Process.Parameters)
                        foreach (var alarm in parameters.Alarms)
                            foreach (var alarmMetric in alarm.AlarmMetrics)
                            {
                                var device = alarmMetric.Device;

                                if (device != null && device.Id == entity.Id)
                                {
                                    dto.PatientName = string.Format("{0} {1}", processInstance.Patient.Firstname, processInstance.Patient.Surname);
                                    dto.ProcessInstanceName = processInstance.Process.Name;

                                }
                            }
                dtos.Add(dto);
            }

            return dtos;
        }

        public int Count(string name, string macAddress, Guid? processInstanceId)
        {
            int count = _deviceRepository.Count(name, macAddress, processInstanceId);
            return count;
        }

        public IList<Device> FetchWithoutProcessInstanceId(Guid? processInstanceId, PaginationModel pagination, OrderByModel orderBy)
        {
            var entities = _deviceRepository.FetchWithoutProcessInstanceId(processInstanceId, pagination, orderBy);
            return entities;
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
                _processInstanceRepository.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose
    }
}