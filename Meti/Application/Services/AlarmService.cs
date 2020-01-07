//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Domain.Services;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.Alarm;
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
    public class AlarmService : ServiceNHibernateBase, IServiceNHibernateBase, IAlarmService
    {
        #region Private fields

        private readonly IAlarmRepository _alarmRepository;
        private readonly IParameterRepository _parameterRepository;
        private readonly IAlarmMetricService _alarmMetricService;

        #endregion Private fields

        #region Costructors

        public AlarmService(ISession session)
            : base(session)
        {
            _alarmRepository = new AlarmRepository(session);
            _parameterRepository = new ParameterRepository(session);
            _alarmMetricService = new AlarmMetricService(session);
        }

        public AlarmService(IAlarmRepository alarmRepository, IParameterRepository parameterRepository, IAlarmMetricService alarmMetricService)
        {
            _alarmRepository = alarmRepository;
            _parameterRepository = parameterRepository;
            _alarmMetricService = alarmMetricService;
        }

        #endregion Costructors

        #region Services

        public OperationResult<Guid?> CreateAlarm(AlarmEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            Alarm entity = new Alarm();
            entity.Name = dto.Name;
            entity.ContactOperator = dto.ContactOperator;
            entity.Emails = dto.Emails;
            entity.SmsNumbers= dto.SmsNumbers;
            entity.IsEnabled= !dto.IsEnabled.HasValue ? false: dto.IsEnabled;
            entity.Parameter = dto.Parameter.HasValue ? _parameterRepository.Load(dto.Parameter) : null;
            entity.HelpMessage = dto.HelpMessage;
            entity.AlarmColor = dto.AlarmColor?.Id;

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _alarmRepository.Save(entity);
            }

            if (dto.AlarmMetrics != null && dto.AlarmMetrics.Count > 0)
            {
                entity.AlarmMetrics.Clear();
                foreach (var item in dto.AlarmMetrics)
                {
                    item.Alarm = entity.Id;
                    var oResult = _alarmMetricService.CreateAlarmMetric(item);
                    if (oResult.HasErrors())
                    {
                        return new OperationResult<Guid?>
                        {
                            ValidationResults = oResult.ValidationResults
                        };
                    }
                    var alarmMetric = _alarmMetricService.Load<AlarmMetric, Guid?>(oResult.ReturnedValue);
                    entity.AlarmMetrics.Add(alarmMetric);
                }
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }

        public OperationResult<Guid?> UpdateAlarm(AlarmEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (!dto.Id.HasValue) throw new ArgumentNullException(nameof(dto.Id));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            Alarm entity = _alarmRepository.Load(dto.Id);
            entity.Name = dto.Name;
            entity.ContactOperator = dto.ContactOperator;
            entity.Emails = dto.Emails;
            entity.SmsNumbers = dto.SmsNumbers;
            entity.IsEnabled = !dto.IsEnabled.HasValue ? false : dto.IsEnabled;
            entity.Parameter = dto.Parameter.HasValue ? _parameterRepository.Load(dto.Parameter) : null;
            entity.HelpMessage = dto.HelpMessage;
            entity.AlarmColor = dto.AlarmColor?.Id;

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _alarmRepository.Save(entity);
            }

            if (dto.AlarmMetrics != null && dto.AlarmMetrics.Count > 0)
            {
                entity.AlarmMetrics.Clear();
                foreach (var item in dto.AlarmMetrics)
                {
                    item.Alarm = entity.Id;
                    var oResult = item.Id.HasValue ? _alarmMetricService.UpdateAlarmMetric(item) : _alarmMetricService.CreateAlarmMetric(item);

                    if (oResult.HasErrors())
                    {
                        return new OperationResult<Guid?>
                        {
                            ValidationResults = oResult.ValidationResults
                        };
                    }
                    var alarmMetric = _alarmMetricService.Load<AlarmMetric, Guid?>(oResult.ReturnedValue);
                    entity.AlarmMetrics.Add(alarmMetric);
                }
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }

        public OperationResult<Guid?> DeleteAlarm(Guid? id)
        {
            //Validazione argomenti
            if (!id.HasValue) throw new ArgumentNullException(nameof(id));

            //Dichiaro la listsa di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            Alarm entity = _alarmRepository.Load(id);

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _alarmRepository.Delete(entity);
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
                _alarmRepository.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose
    }
}