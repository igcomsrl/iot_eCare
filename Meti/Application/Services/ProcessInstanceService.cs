//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Domain.Services;
using MateSharp.Framework.Helpers;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.ProcessInstance;
using Meti.Application.Dtos.Registry;
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
    public class ProcessInstanceService : ServiceNHibernateBase, IProcessInstanceService
    {
        #region Private fields

        private readonly IProcessInstanceRepository _processInstanceRepository;
        private readonly IRegistryRepository _registryRepository;
        private readonly IProcessService _processService;
        private readonly IAlarmMetricRepository _alarmMetricRepository;

        #endregion private fields

        #region Costructors

        public ProcessInstanceService(ISession session)
            : base(session)
        {
            _processInstanceRepository = new ProcessInstanceRepository(session);
            _processService = new ProcessService(session);
            _registryRepository = new RegistryRepository(session);
            _alarmMetricRepository = new AlarmMetricRepository(session);
        }

        public ProcessInstanceService(IProcessInstanceRepository ProcessInstanceRepository, IProcessService parameterService, IRegistryRepository registryRepository, IAlarmMetricRepository alarmMetricRepository)
        {
            _processInstanceRepository = ProcessInstanceRepository;
            _processService = parameterService;
            _registryRepository = registryRepository;
            _alarmMetricRepository = alarmMetricRepository;
        }

        #endregion Costructors

        #region Services

        private void NormalizeParametersIds(ProcessInstanceEditDto dto)
        {
            foreach (var parameter in dto.Process.Parameters)
            {
                parameter.Id = null;
                parameter.Process = null;
            }
        }
        private void NormalizeAlarmsIds(ProcessInstanceEditDto dto)
        {
            foreach (var parameter in dto.Process.Parameters)
            {
                foreach (var alarm in parameter.Alarms)
                {
                    alarm.Id = null;
                    alarm.Parameter = null;
                }
            }
            
        }
        private void NormalizeProcessMacrosIds(ProcessInstanceEditDto dto)
        {
            foreach (var processMacro in dto.Process.ProcessMacros)
            {
                processMacro.Id = null;
                processMacro.Process = null;
            }
        }
        

        public OperationResult<Guid?> CreateProcessInstance(ProcessInstanceEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            NormalizeParametersIds(dto);
            NormalizeProcessMacrosIds(dto);
            NormalizeAlarmsIds(dto);

            dto.Process.ProcessType = ProcessType.Instance;
            var oResult = _processService.CreateProcess(dto.Process);
            if (oResult.HasErrors())
            {
                return oResult;
            }

            //Definisco l'entità
            ProcessInstance entity = new ProcessInstance();
            entity.Name = dto.Name;
            entity.Patient = dto.Patient.HasValue ? _registryRepository.Load(dto.Patient) : null;
            entity.Process = _processService.Load<Process, Guid?>(oResult.ReturnedValue);
            entity.ProcessInstanceState = ProcessInstanceState.Open;

            entity.Doctors.Clear();
            if (dto.Doctors != null && dto.Doctors.Count() > 0) 
            {
                foreach (var doctor in dto.Doctors)
                {
                    Registry doctorEntity = _registryRepository.Load(doctor.Id);
                    entity.Doctors.Add(doctorEntity);
                }
            }

            entity.ReferencePersons.Clear();
            if (dto.ReferencePersons != null && dto.ReferencePersons.Count() > 0)
            {
                foreach (var referencePerson in dto.ReferencePersons)
                {
                    Registry referencePersonEntity = _registryRepository.Load(referencePerson.Id);
                    entity.ReferencePersons.Add(referencePersonEntity);
                }
            }

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _processInstanceRepository.Save(entity);
            
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }

     

        public OperationResult<Guid?> UpdateProcessInstance(ProcessInstanceEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (!dto.Id.HasValue) throw new ArgumentNullException(nameof(dto.Id));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            var oResult = _processService.UpdateProcess(dto.Process);
            if (oResult.HasErrors())
            {
                return oResult;
            }

            //Definisco l'entità
            ProcessInstance entity = _processInstanceRepository.Load(dto.Id);
            entity.Name = dto.Name;
            entity.Patient = dto.Patient.HasValue ? _registryRepository.Load(dto.Patient) : null;
            entity.Process = _processService.Load<Process, Guid?>(oResult.ReturnedValue);            

            entity.Doctors.Clear();
            if (dto.Doctors != null && dto.Doctors.Count() > 0)
            {
                foreach (var doctor in dto.Doctors)
                {
                    Registry doctorEntity = _registryRepository.Load(doctor.Id);
                    entity.Doctors.Add(doctorEntity);
                }
            }

            entity.ReferencePersons.Clear();
            if (dto.ReferencePersons != null && dto.ReferencePersons.Count() > 0)
            {
                foreach (var referencePerson in dto.ReferencePersons)
                {
                    Registry referencePersonEntity = _registryRepository.Load(referencePerson.Id);
                    entity.ReferencePersons.Add(referencePersonEntity);
                }
            }

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _processInstanceRepository.Save(entity);

            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }


        public OperationResult<Guid?> DeleteProcessInstance(Guid? id)
        {
            //Validazione argomenti            
            if (!id.HasValue) throw new ArgumentNullException(nameof(id));

            //Dichiaro la listsa di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            ProcessInstance entity = _processInstanceRepository.Load(id);

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _processInstanceRepository.Delete(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = null,
                ValidationResults = vResults
            };
        }

        public IList<ProcessInstance> Fetch(string name, Guid? doctorId, Guid? processId, Guid? patientId, ProcessInstanceState? processInstanceState, PaginationModel pagination, OrderByModel orderBy)
        {
            var entities = _processInstanceRepository.Fetch(name, doctorId, processId, patientId, processInstanceState, pagination, orderBy);
            return entities;
        }

        public IList<ProcessInstance> GetByRegistry(Guid? registryId)
        {
            var entities = _processInstanceRepository.GetByRegistry(registryId);
            return entities;
        }


        public IList<ProcessInstance> GetByRegistryByEmail(string email)
        {
            var entities = _processInstanceRepository.GetByRegistryByEmail(email);
            return entities;
        }

        public int Count(string name, Guid? doctorId, Guid? processId, Guid? patientId, ProcessInstanceState? processInstanceState)
        {
            int count = _processInstanceRepository.Count(name, doctorId, processId, patientId, processInstanceState);
            return count;
        }

        public OperationResult<Guid?> UpdatePositionLast(Guid? processInstanceId, string latitude, string longitude)
        {
            //Validazione argomenti            
            if (!processInstanceId.HasValue) throw new ArgumentNullException(nameof(processInstanceId));

            //Dichiaro la listsa di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            ProcessInstance entity = _processInstanceRepository.Load(processInstanceId);
            entity.Patient.LatitudeLast = latitude;
            entity.Patient.LongitudeLast = longitude;

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _processInstanceRepository.Save(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = null,
                ValidationResults = vResults
            };
        }


        public OperationResult<Guid?> CloseProcessInstance(Guid? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id));

            IList<ValidationResult> vResults = new List<ValidationResult>();

            ProcessInstance entity = _processInstanceRepository.Load(id);

            entity.ProcessInstanceState = ProcessInstanceState.Close;
            entity.CloseDate = DateTime.Now;
            entity.CloseFrom = IdentityHelper.GetUsername();

            foreach (var parameter in entity.Process.Parameters)
            {
                foreach (var alarm in parameter.Alarms)
                {
                    foreach (var alarmMetric in alarm.AlarmMetrics)
                    {
                        alarmMetric.Device = null;
                        var alarmMetricValidation = ValidateEntity(alarmMetric);
                        if (!alarmMetricValidation.Any())
                        {
                            _alarmMetricRepository.Save(alarmMetric);
                        }
                    }
                }
            }

            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _processInstanceRepository.Save(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = null,
                ValidationResults = vResults
            };
        }

        public OperationResult<Guid?> OpenProcessInstance(Guid? id)
        {
            if (!id.HasValue) throw new ArgumentNullException(nameof(id));

            IList<ValidationResult> vResults = new List<ValidationResult>();

            ProcessInstance entity = _processInstanceRepository.Load(id);

            entity.ProcessInstanceState = ProcessInstanceState.Open;
            entity.ReOpenDate = DateTime.Now;
            entity.ReOpenFrom = IdentityHelper.GetUsername();

            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _processInstanceRepository.Save(entity);
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
                _processInstanceRepository.Dispose();
                _processService.Dispose();
                _registryRepository.Dispose();
                _alarmMetricRepository.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }
        
        #endregion Dispose
    }
}