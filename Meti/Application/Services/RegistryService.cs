//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Domain.Services;
using MateSharp.Framework.Models;
using Meti.Application.Dtos;
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
using Meti.Application.Dtos.Registry;
using MateSharp.RoleClaim.Domain.Dtos;
using MateSharp.RoleClaim.Domain.Contracts.Repository;
using MateSharp.RoleClaim.Repository.Relational.NHibernate;
using Meti.Infrastructure.Configurations;
using AutoMapper;

namespace Meti.Application.Services
{
    public class RegistryService : ServiceNHibernateBase, IRegistryService
    {
        #region Private fields

        private readonly IRegistryRepository _registryRepository;
        private readonly IAccountService _accountService;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProcessInstanceRepository _processInstanceRepository;
        private readonly IHealthRiskService _healthRiskService;
        private readonly IHealthRiskRepository _healthRiskRepository;
        #endregion private fields

        #region Costructors

        public RegistryService(ISession session)
            : base(session)
        {
            _registryRepository = new RegistryRepository(session);
            _accountService = new AccountService(session);
            _roleRepository = new RoleRepository(session);
            _userRepository = new UserRepository(session);
            _processInstanceRepository = new ProcessInstanceRepository(session);
            _healthRiskService = new HealthRiskService(session);
            _healthRiskRepository = new HealthRiskRepository(session);
        }

        public RegistryService(IRegistryRepository registryRepository, IAccountService accountService, IRoleRepository roleRepository, IUserRepository userRepository, IProcessInstanceRepository processInstanceRepository, IHealthRiskService healthRiskService, IHealthRiskRepository healthRiskRepository)
        {
            _registryRepository = registryRepository;
            _accountService = accountService;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _processInstanceRepository = processInstanceRepository;
            _healthRiskService = healthRiskService;
            _healthRiskRepository = healthRiskRepository;
        }

        #endregion Costructors

        #region Services

        public OperationResult<Guid?> CreateRegistry(RegistryEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            Registry entity = new Registry();
            entity.Firstname = dto.Firstname;
            entity.Surname = dto.Surname;
            entity.RegistryType = dto.RegistryType;
            entity.Sex = dto.Sex;
            entity.BirthDate = dto.BirthDate;
            entity.BirthPlace = dto.BirthPlace;
            entity.DomicilePlace = dto.DomicilePlace;
            entity.DomicilePlaceAddress = dto.DomicilePlaceAddress;
            entity.DomicilePlaceCap = dto.DomicilePlaceCap;
            entity.MunicipalityPlace = dto.MunicipalityPlace;
            entity.MunicipalityPlaceAddress = dto.MunicipalityPlaceAddress;
            entity.MunicipalityPlaceCap = dto.MunicipalityPlaceCap;
            entity.Email = dto.Email;
            entity.MobilePhone = dto.MobilePhone;
            entity.Phone = dto.Phone;
            entity.RegionalMedicalCode = dto.RegionalMedicalCode;
            entity.Latitude = dto.Latitude;
            entity.Longitude = dto.Longitude;
            entity.Allergy = dto.Allergy;
            entity.Intollerance = dto.Intollerance;
            entity.BloodGroup = dto.BloodGroup;
            entity.Diagnosis = dto.Diagnosis;
            entity.PreviousIllnesses = dto.PreviousIllnesses;
            entity.NextMedicalHistory = dto.NextMedicalHistory;
            entity.RemoteAnamnesis = dto.RemoteAnamnesis;
            entity.Diet = dto.Diet;
            entity.PathologiesInProgress = dto.PathologiesInProgress;
            entity.Note = dto.Note;
            entity.Weight = dto.Weight;
            entity.Height = dto.Height;
            entity.LifeStyle = dto.LifeStyle;

            if (string.IsNullOrWhiteSpace(entity.MunicipalityPlaceAddress))
            {
                entity.Latitude = null;
                entity.Longitude = null;
            }

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);
            
            if (!vResults.Any())
            {
                //Salvataggio su db
                _registryRepository.Save(entity);

                if (dto.HealthRisks != null)
                {
                    foreach (var item in dto.HealthRisks)
                    {
                        item.Registry = new RegistryDetailDto { Id = entity.Id };
                        var healthRiskValidation = _healthRiskService.CreateHealthRisk(item);
                        if (healthRiskValidation.HasErrors())
                        {
                            return healthRiskValidation;
                        }
                    }
                }
            }

            //Creo anche l'utenza di sistema
            if (entity.RegistryType == RegistryType.Paziente)
            {
                UserDto userDto = new UserDto();
                userDto.Email = entity.Email;
                userDto.Firstname = entity.Firstname;
                userDto.Surname = entity.Surname;
                userDto.Username = entity.Email;
                var roles = _roleRepository.Fetch(e => e.Name == "Cliente", null);

                var oResult = _accountService.Register(userDto, roles);

                if (oResult.HasErrors())
                {
                    Log4NetConfig.ApplicationLog.Error(string.Format("Errore durante la creazione dell'utenza di sistema: {0} {1}", userDto.Firstname, userDto.Surname));
                    return new OperationResult<Guid?>
                    {
                        ReturnedValue = (Guid?)oResult.ReturnedValue,
                        ValidationResults = oResult.ValidationResults                        
                    };
                }
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }

        public OperationResult<Guid?> UpdateRegistry(RegistryEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (!dto.Id.HasValue) throw new ArgumentNullException(nameof(dto.Id));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            Registry entity = _registryRepository.Load(dto.Id);
            
            entity.Firstname = dto.Firstname;
            entity.Surname = dto.Surname;
            entity.RegistryType = dto.RegistryType;
            entity.Sex = dto.Sex;
            entity.BirthDate = dto.BirthDate;
            entity.BirthPlace = dto.BirthPlace;
            entity.DomicilePlace = dto.DomicilePlace;
            entity.DomicilePlaceAddress = dto.DomicilePlaceAddress;
            entity.DomicilePlaceCap = dto.DomicilePlaceCap;
            entity.MunicipalityPlace = dto.MunicipalityPlace;
            entity.MunicipalityPlaceAddress = dto.MunicipalityPlaceAddress;
            entity.MunicipalityPlaceCap = dto.MunicipalityPlaceCap;
            entity.Email = dto.Email;
            entity.MobilePhone = dto.MobilePhone;
            entity.Phone = dto.Phone;
            entity.RegionalMedicalCode = dto.RegionalMedicalCode;
            entity.Latitude = dto.Latitude;
            entity.Longitude = dto.Longitude;
            entity.Allergy = dto.Allergy;
            entity.Intollerance = dto.Intollerance;
            entity.BloodGroup = dto.BloodGroup;
            entity.Diagnosis = dto.Diagnosis;
            entity.PreviousIllnesses = dto.PreviousIllnesses;
            entity.NextMedicalHistory = dto.NextMedicalHistory;
            entity.RemoteAnamnesis = dto.RemoteAnamnesis;
            entity.Diet = dto.Diet;
            entity.PathologiesInProgress = dto.PathologiesInProgress;
            entity.Note = dto.Note;
            entity.Weight= dto.Weight;
            entity.Height= dto.Height;
            entity.LifeStyle = dto.LifeStyle;

            if (string.IsNullOrWhiteSpace(entity.MunicipalityPlaceAddress))
            {
                entity.Latitude = null;
                entity.Longitude = null;
            }


            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);
            
            if (!vResults.Any())
            {
                //Salvataggio su db
                _registryRepository.Save(entity);

                if (dto.HealthRisks != null)
                {
                    if (entity.HealthRisks == null)
                        entity.HealthRisks = new List<HealthRisk>();

                    entity.HealthRisks.Clear();
                    foreach (var item in dto.HealthRisks)
                    {
                        item.Registry = new RegistryDetailDto { Id = entity.Id };

                        var healthRiskValidation = item.Id.HasValue ?
                            _healthRiskService.UpdateHealthRisk(item) :
                            _healthRiskService.CreateHealthRisk(item);

                        if (healthRiskValidation.HasErrors())
                        {
                            return healthRiskValidation;
                        }

                        HealthRisk healthRisk = _healthRiskRepository.Load(healthRiskValidation.ReturnedValue);
                        entity.HealthRisks.Add(healthRisk);                        
                    }
                }
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }

        private IList<ValidationResult> ValidateDelete(Registry entity)
        {
            var vResults = ValidateEntity(entity);

            if (vResults.Any())
            {
                return vResults;
            }

            //Verifico se ci sono Processi associati.
            var processInstances = _processInstanceRepository.GetByRegistry(entity.Id);
            if (processInstances.Any())
            {
                var validationErrorMessage = string.Format("Il paziente {0} è associato al processo/i {1}. Quindi non potrà essere cancellato", 
                    entity.Surname +" "+ entity.Firstname, 
                    processInstances.Select(e => e.Process.Name).Aggregate((i, j) => i + " / " + j));
                vResults.Add(new ValidationResult(validationErrorMessage));
            }

            return vResults;
        }

        public OperationResult<Guid?> DeleteRegistry(Guid? id)
        {
            //Validazione argomenti            
            if (!id.HasValue) throw new ArgumentNullException(nameof(id));

            //Dichiaro la listsa di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            Registry entity = _registryRepository.Load(id);

            //Eseguo la validazione logica
            vResults = ValidateDelete(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _registryRepository.Delete(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = null,
                ValidationResults = vResults
            };
        }

        public IDictionary<Guid?, RegistryDetailDto> FetchByProcessInstanceIds(List<Guid?> processInstanceIds)
        {
            var entities = _registryRepository.FetchByProcessInstanceIds(processInstanceIds);

            IDictionary<Guid?, RegistryDetailDto> results = new Dictionary<Guid?, RegistryDetailDto>();

            foreach (var entity in entities)
            {
                entity.Patient.Files.Clear();
                results.Add(entity.Id, Mapper.Map<Registry, RegistryDetailDto>(entity.Patient));
            }

            return results;
        }
        
        public IList<Registry> Fetch(string firstname, string surname, SexType? sex, RegistryType? registryType, PaginationModel pagination, OrderByModel orderBy)
        {
            var entities = _registryRepository.Fetch(firstname, surname, sex, registryType, pagination, orderBy);
            return entities;
        }

        public int Count(string firstname, string surname, SexType? sex, RegistryType? registryType)
        {
            int count = _registryRepository.Count(firstname, surname, sex, registryType);
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
                _registryRepository.Dispose();
                _processInstanceRepository.Dispose();
                _roleRepository.Dispose();
                _userRepository.Dispose();
                _healthRiskService.Dispose();
                _healthRiskRepository.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }


        #endregion Dispose
    }
}