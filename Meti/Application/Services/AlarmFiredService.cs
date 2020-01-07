//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Domain.Services;
using MateSharp.Framework.Helpers;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.AlarmFired;
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
    public class AlarmFiredService : ServiceNHibernateBase, IServiceNHibernateBase, IAlarmFiredService
    {
        #region Private fields

        private readonly IAlarmFiredRepository _alarmFiredRepository;
        private readonly IParameterRepository _parameterRepository;
        private readonly IProcessInstanceRepository _processInstanceRepository;
        private readonly IAlarmRepository _alarmRepository;

        #endregion Private fields

        #region Costructors

        public AlarmFiredService(ISession session)
            : base(session)
        {
            _alarmFiredRepository = new AlarmFiredRepository(session);
            _parameterRepository = new ParameterRepository(session);
            _processInstanceRepository = new ProcessInstanceRepository(session);
            _alarmRepository = new AlarmRepository(session);
        }

        public AlarmFiredService(IAlarmFiredRepository AlarmFiredRepository, IParameterRepository parameterRepository, IProcessInstanceRepository processInstanceRepository, IAlarmRepository alarmRepository)
        {
            _alarmFiredRepository = AlarmFiredRepository;
            _parameterRepository = parameterRepository;
            _processInstanceRepository = processInstanceRepository;
            _alarmRepository = alarmRepository;
        }

        #endregion Costructors

        #region Services

        public OperationResult<Guid?> CreateAlarmFired(AlarmFiredEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();
           
            Alarm alarm = dto.AlarmId.HasValue ? _alarmRepository.Load(dto.AlarmId):null;

            //Definisco l'entità
            AlarmFired entity = new AlarmFired();
            entity.IsActive = true;
            entity.Parameter = dto.ParameterId.HasValue ? _parameterRepository.Load(dto.ParameterId) : null;
            entity.ProcessInstance = dto.ProcessInstanceId.HasValue ? _processInstanceRepository.Load(dto.ProcessInstanceId) : null;
            entity.AlarmColor = dto.AlarmColor;
            entity.Result = dto.Result;
            entity.Alarm = dto.AlarmId.HasValue ? _alarmRepository.Load(dto.AlarmId) : null;
            entity.ResolvingNotes = alarm?.HelpMessage;
            entity.IsFall = !dto.Fall.HasValue ? false : dto.Fall;
            entity.IsFallNoResponse = !dto.FallNoResponse.HasValue? false: dto.FallNoResponse;
            entity.Latitude = dto.Latitude;
            entity.Longitude = dto.Longitude;
            entity.Patient = string.Format("{0} {1}", entity.ProcessInstance.Patient.Firstname, entity.ProcessInstance.Patient.Surname);
            entity.PatientFeedback = dto.PatientFeedback;
            entity.InsertUser = IdentityHelper.GetUsername(); 

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _alarmFiredRepository.Save(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }

        public OperationResult<Guid?> TurnOff(AlarmFiredEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (!dto.Id.HasValue) throw new ArgumentNullException(nameof(dto.Id));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            AlarmFired entity = _alarmFiredRepository.Load(dto.Id);
            entity.IsActive = false;
            entity.UpdateDate = DateTime.Now;
            entity.Feedback = dto.Feedback;
            if (!string.IsNullOrWhiteSpace(dto.Feedback))
            {
                entity.FeedbackDate = DateTime.Now;
                entity.FeedbackBy = IdentityHelper.GetUsername();
            }
            entity.InsertUser = IdentityHelper.GetUsername();

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _alarmFiredRepository.Save(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }

        public OperationResult<Guid?> DeleteAlarmFired(Guid? id)
        {
            //Validazione argomenti
            if (!id.HasValue) throw new ArgumentNullException(nameof(id));

            //Dichiaro la listsa di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            AlarmFired entity = _alarmFiredRepository.Load(id);

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _alarmFiredRepository.Delete(entity);
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
                _parameterRepository.Dispose();
                _alarmFiredRepository.Dispose();
                _alarmRepository.Dispose();
                _processInstanceRepository.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose
    }
}