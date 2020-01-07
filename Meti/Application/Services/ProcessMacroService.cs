//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Domain.Services;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.ProcessMacro;
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
    public class ProcessMacroService : ServiceNHibernateBase, IServiceNHibernateBase, IProcessMacroService
    {
        #region private fields

        private readonly IProcessMacroRepository _processMacroRepository;
        private readonly IProcessRepository _processRepository;

        #endregion private fields

        #region Costructors

        public ProcessMacroService(ISession session)
            : base(session)
        {
            _processMacroRepository = new ProcessMacroRepository(session);
            _processRepository = new ProcessRepository(session);
        }

        public ProcessMacroService(IProcessMacroRepository processMacroRepository, IProcessRepository processRepository)
        {
            _processMacroRepository = processMacroRepository;            
            _processRepository = processRepository;
        }

        #endregion Costructors

        #region Services

        public OperationResult<Guid?> CreateProcessMacro(ProcessMacroEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            ProcessMacro entity = new ProcessMacro();
            entity.Name = dto.Name;
            entity.Value = dto.Value;
            entity.Process = dto.Process.HasValue ? _processRepository.Load(dto.Process) : null;    

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _processMacroRepository.Save(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }

        public OperationResult<Guid?> UpdateProcessMacro(ProcessMacroEditDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (!dto.Id.HasValue) throw new ArgumentNullException(nameof(dto.Id));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            ProcessMacro entity = _processMacroRepository.Load(dto.Id);
            entity.Name = dto.Name;
            entity.Value = dto.Value;
            entity.Process = dto.Process.HasValue ? _processRepository.Load(dto.Process) : null;
            
            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _processMacroRepository.Save(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }


        public OperationResult<Guid?> DeleteProcessMacro(Guid? id)
        {
            //Validazione argomenti            
            if (!id.HasValue) throw new ArgumentNullException(nameof(id));

            //Dichiaro la listsa di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            ProcessMacro entity = _processMacroRepository.Load(id);

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _processMacroRepository.Delete(entity);
            }

            //Ritorno i risultati
            return new OperationResult<Guid?>
            {
                ReturnedValue = null,
                ValidationResults = vResults
            };
        }

        public IList<ProcessMacro> Fetch(string name, string value, Guid? processId, PaginationModel pagination, OrderByModel orderBy)
        {
            var entities = _processMacroRepository.Fetch(name, value, processId, pagination, orderBy);
            return entities;
        }

        public int Count(string name, string value, Guid? processId)
        {
            int count = _processMacroRepository.Count(name, value, processId);
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
                _processMacroRepository.Dispose();
                _processRepository.Dispose();      
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }


        #endregion Dispose
    }
}