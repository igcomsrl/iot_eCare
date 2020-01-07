//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using AutoMapper;
using MateSharp.Framework.Dtos;
using MateSharp.Framework.Filters;
using MateSharp.Framework.Helpers;
using MateSharp.Framework.Helpers.NHibernate;
using MateSharp.Framework.Models;
using MateSharp.RoleClaim.Domain.Dtos;
using MateSharp.RoleClaim.Domain.Entities;
using MateSharp.RoleClaim.Domain.Services.Interfaces;
using Meti.App.Filters;
using Meti.Application.Dtos.User;
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
    public class UserController : ApiController
    {
        #region Private Fields

        private readonly IAuthorizeService _authorizeService;
        private readonly IAccountService _accountService;

        #endregion Private Fields

        #region Costructors

        public UserController(IAuthorizeService authorizeService, IAccountService accountService)
        {
            _authorizeService = authorizeService;
            _accountService = accountService;
        }

        #endregion Costructors

        #region Api

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Create(UserDto dto)
        {
            //Recupero l'entity
            var oResult = _accountService.Register(dto, null);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la creazione di un utente. Cognome: {0}, Nome: {1}",
                   dto.Surname, dto.Firstname, ValidationHelper.GetErrorsInline(oResult.ValidationResults, " - ")));
                NHibernateHelper.SessionFactory.GetCurrentSession().Transaction.Rollback();
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, oResult.ValidationResults));
            }

            //Ritorno i risultati
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
        }

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Update(UserDto dto)
        {
            //Recupero l'entity
            var oResult = _accountService.UpdateAccount(dto, null);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la modifica di un utente. Cognome: {0}, Nome: {1}",
                   dto.Surname, dto.Firstname, ValidationHelper.GetErrorsInline(oResult.ValidationResults, " - ")));
                NHibernateHelper.SessionFactory.GetCurrentSession().Transaction.Rollback();
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, oResult));
            }

            //Ritorno i risultati
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
        }
        

        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult Get(Guid? id)
        {
            //Recupero l'entity
            User entity = _authorizeService.Get<User, Guid?>(id);

            //Compongo il dto
            UserUpdateDto dto = Mapper.Map<UserUpdateDto>(entity);

            //Ritorno i risultati
            return Ok(dto);
        }

        [HttpPost]
        [NHibernateTransaction]
        [AllowAnonymous]
        public IHttpActionResult ResetPassword(UserDto dto)
        {
            //Recupero l'entity
            var oResult = _accountService.ResetPassword(dto?.Username);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante il reset della password dell'utente. Username: {0}",
                   dto?.Username, ValidationHelper.GetErrorsInline(oResult.ValidationResults, " - ")));
                NHibernateHelper.SessionFactory.GetCurrentSession().Transaction.Rollback();
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, oResult));
            }

            //Ritorno i risultati
            return Ok(oResult);
        }

        /// <summary>
        /// Deletes the specified dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Delete(UserUpdateDto dto)
        {
            //Recupero l'entity
            var vResults = _authorizeService.DeleteUser(dto?.Id);

            //Se ci sono stati errori, li notifico
            if (vResults.Any())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la cancellazione di un utente. Cognome: {0}, Nome: {1}",
                   dto.Surname, dto.Firstname, ValidationHelper.GetErrorsInline(vResults, " - ")));
                NHibernateHelper.SessionFactory.GetCurrentSession().Transaction.Rollback();
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, vResults));
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
        /// <param name="UserType"></param>
        /// <param name="pagination"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult Fetch(string username= null, string firstname = null, string surname = null, string email = null, [FromUri] PaginationModel pagination = null, [FromUri] OrderByModel orderBy = null)
        {
            //Recupero le entità
            //Compongo il dto in entrata
            UserDto dto = new UserDto
            {
                Username = username,
                Firstname = firstname,
                Surname = surname,
                Email = email,
            };

            //Recupero le entità
            IList<User> entities = _authorizeService.FetchUsers(dto, pagination?.StartRowIndex, pagination?.MaxRowIndex, orderBy?.OrderByProperty, orderBy?.OrderByType);
            //Conto i risultati
            int count = _authorizeService.CountUsers(dto, null, null);

            //Eseugo la mappatura a Dtos
            var dtos = entities.Any() ? entities.Select(e => Mapper.Map<UserUpdateDto>(e)).ToList() : new List<UserUpdateDto>();

            //Compongo i risultati di ritorno
            var result = new FetchDto(dtos, count);

            //Ritorno i risultati
            return Ok(result);
        }

        /// <summary>
        /// Fetches the essential data.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult FetchEssentialData()
        {
            return Ok();
        }

        #endregion Api

        #region Dispose Pattern

        protected override void Dispose(bool isDisposing)
        {
            //Se sto facendo la dispose
            if (isDisposing)
            {
                _authorizeService.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose Pattern
    }
}