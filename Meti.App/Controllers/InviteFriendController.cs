//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using AutoMapper;
using MateSharp.Framework.Dtos;
using MateSharp.Framework.Filters;
using MateSharp.Framework.Helpers.NHibernate;
using MateSharp.Framework.Models;
using Meti.App.Filters;
using Meti.Application.Dtos.User;
using Meti.Domain.Models;
using Meti.Domain.Services;
using Meti.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Meti.App.Controllers
{
    [Authorize]
    [CatchLogException]
    public class InviteFriendController : ApiController
    {
        #region Private Fields

        private readonly IInviteFriendService _inviteFriendService;

        #endregion Private Fields

        #region Costructors

        public InviteFriendController(IInviteFriendService InviteFriendService)
        {
            _inviteFriendService = InviteFriendService;
        }

        #endregion Costructors

        #region Api

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Create(InviteFriendDto dto)
        {
            //Recupero l'entity
            var oResult = _inviteFriendService.CreateInviteFriend(dto);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la creazione di un InviteFriend. id: {0}, nome: {1}, Cognome: {2}, Errors: {3}",
                   dto.Id, dto.Surname, dto.Firstname, oResult.GetValidationErrorsInline(" - ")));
                NHibernateHelper.SessionFactory.GetCurrentSession().Transaction.Rollback();
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, oResult));
            }

            //Ritorno i risultati
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
        }

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Update(InviteFriendDto dto)
        {
            //Recupero l'entity
            var oResult = _inviteFriendService.UpdateInviteFriend(dto);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la modifica di un InviteFriend. id: {0}, nome: {1}, Cognome: {2}, Errors: {3}",
                   dto.Id, dto.Surname, dto.Firstname, oResult.GetValidationErrorsInline(" - ")));
                NHibernateHelper.SessionFactory.GetCurrentSession().Transaction.Rollback();
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, oResult));
            }

            //Ritorno i risultati
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
        }

        /// <summary>
        /// Recupero le entità che rispettano i filtri
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="surname"></param>
        /// <param name="sexType"></param>
        /// <param name="InviteFriendType"></param>
        /// <param name="pagination"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult Fetch(Guid? processInstanceId, [FromUri] PaginationModel pagination = null, [FromUri] OrderByModel orderBy = null)
        {
            //Recupero le entità
            var entities = _inviteFriendService.Fetch(processInstanceId, pagination, orderBy);

            var count = _inviteFriendService.Count(processInstanceId);

            //Eseugo la mappatura a Dtos
            var dtos = entities.Any() ? entities.Select(e => Mapper.Map<InviteFriendDto>(e)).ToList() : new List<InviteFriendDto>();

            //Compongo i risultati di ritorno
            var result = new FetchDto(dtos, count);

            //Ritorno i risultati
            return Ok(result);
        }

        /// <summary>
        /// Recupero le entità che rispettano i filtri
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="surname"></param>
        /// <param name="sexType"></param>
        /// <param name="InviteFriendType"></param>
        /// <param name="pagination"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult IsInvited(string email)
        {
            //Recupero le entità
            var entity = _inviteFriendService.Fetch<InviteFriend>(e=>e.Email == email, null, null).SingleOrDefault();

            var dto = Mapper.Map<InviteFriendDto>(entity);

            //Ritorno i risultati
            return Ok(new { result = dto });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult Get(Guid? id)
        {
            //Recupero le entità
            var entity = _inviteFriendService.Get<InviteFriend, Guid?>(id);

            //Eseugo la mappatura a Dtos
            var dtos = Mapper.Map<InviteFriendDto>(entity);

            //Ritorno i risultati
            return Ok(dtos);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult GetByProcessInstance(Guid? processInstanceId)
        {
            //Recupero le entità
            var entity = _inviteFriendService.GetByProcessInstance(processInstanceId);

            //Eseugo la mappatura a Dtos
            var dtos = Mapper.Map<InviteFriendDto>(entity);

            //Ritorno i risultati
            return Ok(dtos);
        }

        #endregion Api

        #region Dispose Pattern

        protected override void Dispose(bool isDisposing)
        {
            //Se sto facendo la dispose
            if (isDisposing)
            {
                _inviteFriendService.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose Pattern
    }
}