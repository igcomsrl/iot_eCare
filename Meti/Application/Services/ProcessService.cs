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
    public class ProcessService : ServiceNHibernateBase, IServiceNHibernateBase, IProcessService
    {
        #region private fields

        private readonly IProcessRepository _processRepository;
        private readonly IParameterService _parameterService;
        private readonly IProcessMacroService _processMacroService;

        #endregion private fields

        #region Costructors

        public ProcessService(ISession session)
            : base(session)
        {
            _processRepository = new ProcessRepository(session);
            _parameterService = new ParameterService(session);
            _processMacroService = new ProcessMacroService(session);
        }

        public ProcessService(IProcessRepository processRepository, IParameterService parameterService, IProcessMacroService processMacroService)
        {
            _processRepository = processRepository;
            _parameterService = parameterService;
            _processMacroService = processMacroService;
        }

        #endregion Costructors

        #region Services

        public OperationResult<Guid?> CreateProcess(ProcessEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            Process entity = new Process();
            entity.Name = dto.Name;
            entity.ProcessType = dto.ProcessType;
            entity.IsEnabled = !dto.IsEnabled.HasValue ? false : dto.IsEnabled;

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _processRepository.Save(entity);
            }

            if (dto.Parameters != null && dto.Parameters.Count > 0)
            {
                entity.Parameters.Clear();
                foreach (var item in dto.Parameters)
                {
                    item.Process = entity.Id;
                    var oResult = _parameterService.CreateParameter(item);
                    if (oResult.HasErrors())
                    {
                        return new OperationResult<Guid?>
                        {
                            ValidationResults = oResult.ValidationResults
                        };
                    }
                    var parameter = _parameterService.Load<Parameter, Guid?>(oResult.ReturnedValue);
                    entity.Parameters.Add(parameter);
                }
            }

            if (dto.ProcessMacros != null && dto.ProcessMacros.Count > 0)
            {
                entity.ProcessMacros.Clear();
                foreach (var item in dto.ProcessMacros)
                {
                    item.Process = entity.Id;
                    var oResult = _processMacroService.CreateProcessMacro(item);
                    if (oResult.HasErrors())
                    {
                        return new OperationResult<Guid?>
                        {
                            ValidationResults = oResult.ValidationResults
                        };
                    }
                    var processMacro = _processMacroService.Load<ProcessMacro, Guid?>(oResult.ReturnedValue);
                    entity.ProcessMacros.Add(processMacro);
                }
            }

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _processRepository.Save(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }

        public OperationResult<Guid?> UpdateProcess(ProcessEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (!dto.Id.HasValue) throw new ArgumentNullException(nameof(dto.Id));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            Process entity = _processRepository.Load(dto.Id);
            entity.Name = dto.Name;
            entity.IsEnabled = !dto.IsEnabled.HasValue ? false : dto.IsEnabled;


            if (dto.Parameters != null && dto.Parameters.Count > 0)
            {
                entity.Parameters.Clear();
                foreach (var item in dto.Parameters)
                {
                    item.Process = entity.Id;
                    var oResult = item.Id.HasValue ? _parameterService.UpdateParameter(item): _parameterService.CreateParameter(item);                    

                    if (oResult.HasErrors())
                    {
                        return new OperationResult<Guid?>
                        {
                            ValidationResults = oResult.ValidationResults
                        };
                    }

                    var parameter = _parameterService.Load<Parameter, Guid?>(oResult.ReturnedValue);
                    entity.Parameters.Add(parameter);
                }
            }

            if (dto.ProcessMacros != null && dto.ProcessMacros.Count > 0)
            {
                entity.ProcessMacros.Clear();
                foreach (var item in dto.ProcessMacros)
                {
                    item.Process = entity.Id;
                    var oResult = item.Id.HasValue ? _processMacroService.UpdateProcessMacro(item) : _processMacroService.CreateProcessMacro(item);

                    if (oResult.HasErrors())
                    {
                        return new OperationResult<Guid?>
                        {
                            ValidationResults = oResult.ValidationResults
                        };
                    }

                    var processMacro = _processMacroService.Load<ProcessMacro, Guid?>(oResult.ReturnedValue);
                    entity.ProcessMacros.Add(processMacro);
                }
            }

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _processRepository.Save(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }


        public OperationResult<Guid?> DeleteProcess(Guid? id)
        {
            //Validazione argomenti            
            if (!id.HasValue) throw new ArgumentNullException(nameof(id));

            //Dichiaro la listsa di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            Process entity = _processRepository.Load(id);

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _processRepository.Delete(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = null,
                ValidationResults = vResults
            };
        }

        public IList<Process> Fetch(string name, ProcessType? processType, PaginationModel pagination, OrderByModel orderBy)
        {
            var entities = _processRepository.Fetch(name, processType, pagination, orderBy);
            return entities;
        }

        public int Count(string name, ProcessType? processType)
        {
            int count = _processRepository.Count(name, processType);
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
                _processRepository.Dispose();
                _parameterService.Dispose();
                _processMacroService.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose
    }
}