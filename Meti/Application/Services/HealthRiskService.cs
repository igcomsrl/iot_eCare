//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Domain.Services;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.Process;
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
    public class HealthRiskService : ServiceNHibernateBase, IServiceNHibernateBase, IHealthRiskService
    {
        #region Private fields

        private readonly IRegistryRepository _registryRepository;
        private readonly IHealthRiskRepository _healthRiskRepository;

        #endregion Private fields

        #region Costructors

        public HealthRiskService(ISession session)
            : base(session)
        {
            _registryRepository = new RegistryRepository(session);
            _healthRiskRepository = new HealthRiskRepository(session);
        }

        public HealthRiskService(IRegistryRepository registryRepository, IHealthRiskRepository healthRiskRepository)
        {
            _registryRepository = registryRepository;
            _healthRiskRepository = healthRiskRepository;
        }

        #endregion Costructors

        #region Services
        

        public OperationResult<Guid?> CreateHealthRisk(HealthRiskEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            HealthRisk entity = new HealthRisk();
            entity.Registry = dto.Registry != null && dto.Registry.Id.HasValue ? _registryRepository.Load(dto.Registry.Id): null;
            entity.Level = dto.Level?.Id;
            entity.Rating = dto.Rating;
            entity.StartDate = dto.StartDate.HasValue ? (DateTime?)dto.StartDate.Value.AddDays(1): null;
            entity.EndDate = dto.EndDate.HasValue ? (DateTime?)dto.EndDate.Value.AddDays(1) : null;
            entity.Type = dto.Type?.Id;
            

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _healthRiskRepository.Save(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }

        public OperationResult<Guid?> UpdateHealthRisk(HealthRiskEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (!dto.Id.HasValue) throw new ArgumentNullException(nameof(dto.Id));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();
            //Definisco l'entità
            HealthRisk entity = _healthRiskRepository.Load(dto.Id);
            entity.Registry = dto.Registry != null && dto.Registry.Id.HasValue ? _registryRepository.Load(dto.Registry.Id) : null;
            entity.Level = dto.Level?.Id;
            entity.Rating = dto.Rating;
            entity.Type = dto.Type?.Id;
            entity.StartDate= dto.StartDate;
            entity.EndDate = dto.EndDate;

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _healthRiskRepository.Save(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }

        private IList<ValidationResult> ValidateDelete(HealthRisk entity)
        {
            var vResults = ValidateEntity(entity);
            return vResults;
        }

        public OperationResult<Guid?> DeleteHealthRisk(Guid? id)
        {
            //Validazione argomenti
            if (!id.HasValue) throw new ArgumentNullException(nameof(id));

            //Dichiaro la listsa di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            HealthRisk entity = _healthRiskRepository.Load(id);

            //Eseguo la validazione logica
            vResults = ValidateDelete(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _healthRiskRepository.Delete(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = null,
                ValidationResults = vResults
            };
        }
        
        public IList<HealthRiskDetailDto> FetchByProcessInstanceIds(List<Guid?> processInstanceIds)
        {
            var entities = _healthRiskRepository.FetchByProcessInstanceIds(processInstanceIds);
            
            return entities;
        }

        public IList<HealthRisk> Fetch(HealthRiskType? type, HealthRiskLevel? level, string rating, DateTime? startDate, DateTime? endDate, bool? isLast, Guid? registryId, PaginationModel pagination, OrderByModel orderBy)
        {
            var entities = _healthRiskRepository.Fetch(type, level, rating, startDate, endDate, isLast, registryId, pagination, orderBy);          
            return entities;
        }

        public int Count(HealthRiskType? type, HealthRiskLevel? level, string rating, DateTime? startDate, DateTime? endDate, bool? isLast, Guid? registryId)
        {
            int count = _healthRiskRepository.Count(type, level, rating, startDate, endDate, isLast, registryId);
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
                _healthRiskRepository.Dispose();
                _registryRepository.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose
    }
}