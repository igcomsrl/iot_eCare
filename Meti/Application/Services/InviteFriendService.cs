//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Domain.Services;
using MateSharp.Framework.Models;
using MateSharp.RoleClaim.Domain.Dtos;
using Meti.Application.Dtos.User;
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
    public class InviteFriendService : ServiceNHibernateBase, IServiceNHibernateBase, IInviteFriendService
    {
        #region Private fields

        private readonly IInviteFriendRepository _inviteFriendRepository;
        private readonly IProcessInstanceRepository _processInstanceRepository;
        private readonly IAccountService _accountService;

        #endregion Private fields

        #region Costructors

        public InviteFriendService(ISession session)
            : base(session)
        {
            _inviteFriendRepository = new InviteFriendRepository(session);
            _processInstanceRepository = new ProcessInstanceRepository(session);
            _accountService = new AccountService(session);
        }

        public InviteFriendService(IInviteFriendRepository inviteFriendRepository, IProcessInstanceRepository processInstanceRepository, IAccountService accountService)
        {
            _inviteFriendRepository = inviteFriendRepository;
            _processInstanceRepository = processInstanceRepository;
            _accountService = accountService;
        }

        #endregion Costructors

        #region Services

        public OperationResult<object> CreateInviteFriend(InviteFriendDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            InviteFriend entity = new InviteFriend();
            entity.Firstname = dto.Firstname;
            entity.Surname = dto.Surname;
            entity.Email = dto.Email;
            entity.ProcessInstance = dto.ProcessInstanceId.HasValue ? _processInstanceRepository.Load(dto.ProcessInstanceId) : null;
            entity.ShowFrequenza = dto.ShowFrequenza;
            entity.ShowGlicemia= dto.ShowGlicemia;
            entity.ShowPeso = dto.ShowPeso;
            entity.ShowPressione = dto.ShowPressione;
            entity.ShowTemperatura = dto.ShowTemperatura;
            entity.ShowOssigeno = dto.ShowOssigeno;
            entity.ShowCamera = dto.ShowCamera;

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Creo l'utente di sistema per l'accesso
                UserDto user = new UserDto();
                user.Email = dto.Email;
                user.Firstname = dto.Firstname;
                user.Surname = dto.Surname;
                user.Username = dto.Email;

                var userResult = _accountService.Register(user, null);
                if (userResult.HasErrors())
                    return userResult;

                //Salvataggio su db
                _inviteFriendRepository.Save(entity);
            }

            //Ritorno i risultati
            return new OperationResult<object>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }

        public OperationResult<object> UpdateInviteFriend(InviteFriendDto dto)
        {
            //Validazione argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            InviteFriend entity = _inviteFriendRepository.Load(dto.Id);
            entity.Firstname = dto.Firstname;
            entity.Surname = dto.Surname;
            entity.Email = dto.Email;
            entity.ShowFrequenza = dto.ShowFrequenza;
            entity.ShowGlicemia = dto.ShowGlicemia;
            entity.ShowPeso = dto.ShowPeso;
            entity.ShowPressione = dto.ShowPressione;
            entity.ShowTemperatura = dto.ShowTemperatura;
            entity.ShowOssigeno = dto.ShowOssigeno;
            entity.ShowCamera = dto.ShowCamera;

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _inviteFriendRepository.Save(entity);
            }

            //Ritorno i risultati
            return new OperationResult<object>
            {
                ReturnedValue = entity.Id,
                ValidationResults = vResults
            };
        }

        public OperationResult<object> DeleteInviteFriend(Guid? id)
        {
            //Validazione argomenti
            if (!id.HasValue) throw new ArgumentNullException(nameof(id));

            //Dichiaro la listsa di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Definisco l'entità
            InviteFriend entity = _inviteFriendRepository.Load(id);

            //Eseguo la validazione logica
            vResults = ValidateEntity(entity);

            if (!vResults.Any())
            {
                //Salvataggio su db
                _inviteFriendRepository.Delete(entity);
            }

            //Ritorno i risultati
            return new OperationResult<object>
            {
                ReturnedValue = id,
                ValidationResults = vResults
            };
        }

        public IList<InviteFriend> Fetch(Guid? processInstanceId, PaginationModel pagination, OrderByModel orderBy)
        {
            var entities = _inviteFriendRepository.Fetch(processInstanceId, pagination, orderBy);
            return entities;
        }

        public InviteFriend GetByProcessInstance(Guid? processInstanceId)
        {
            var entity = _inviteFriendRepository.GetByProcessInstance(processInstanceId);
            return entity;
        }

        public int Count(Guid? processInstanceId)
        {
            int count = _inviteFriendRepository.Count(processInstanceId);
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
                _inviteFriendRepository.Dispose();
                _processInstanceRepository.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose
    }
}