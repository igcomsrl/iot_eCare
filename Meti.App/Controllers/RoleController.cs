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
using Meti.Application.Dtos.Role;
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
    public class RoleController : ApiController
    {
        #region Private Fields

        private readonly IAuthorizeService _authorizeService;

        #endregion Private Fields

        #region Costructors

        public RoleController(IAuthorizeService authorizeService)
        {
            _authorizeService = authorizeService;
        }

        #endregion Costructors

        #region Api

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Create(RoleCreateDto dto)
        {
            //Recupero l'entity
            var vResults = _authorizeService.CreateRole(dto);

            //Se ci sono stati errori, li notifico
            if (vResults.Any())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la creazione di un ruolo. Nome: {0}, Descrizione: {1}",
                   dto.Name, dto.Description, ValidationHelper.GetErrorsInline(vResults, " - ")));
                NHibernateHelper.SessionFactory.GetCurrentSession().Transaction.Rollback();
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, vResults));
            }

            //Ritorno i risultati
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
        }

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Update(RoleUpdateDto dto)
        {
            //Recupero l'entity
            var vResults = _authorizeService.UpdateRole(dto);

            //Se ci sono stati errori, li notifico
            if (vResults.Any())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la modifica di un ruolo. Nome: {0}, Descrizione: {1}",
                   dto.Name, dto.Description, ValidationHelper.GetErrorsInline(vResults, " - ")));
                NHibernateHelper.SessionFactory.GetCurrentSession().Transaction.Rollback();
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, vResults));
            }

            //Ritorno i risultati
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
        }

        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult Get(int? id)
        {
            //Recupero l'entity
            Role entity = _authorizeService.Get<Role, int?>(id);

            //Compongo il dto
            RoleUpdateDto dto = new RoleUpdateDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                ClaimsId = entity.Claims.Select(e => e.Id).ToList(),
                UsersId = entity.Users.Select(e => e.Id).ToList()
            };

            //Ritorno i risultati
            return Ok(dto);
        }

        /// <summary>
        /// Deletes the specified dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Delete(RoleUpdateDto dto)
        {
            //Recupero l'entity
            var vResults = _authorizeService.DeleteRole(dto?.Id);

            //Se ci sono stati errori, li notifico
            if (vResults.Any())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la cancellazione di un utente. Name: {0}, Description: {1}",
                   dto.Name, dto.Description, ValidationHelper.GetErrorsInline(vResults, " - ")));
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
        /// <param name="RoleType"></param>
        /// <param name="pagination"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult Fetch(string name = null, string description = null, [FromUri] PaginationModel pagination = null, [FromUri] OrderByModel orderBy = null)
        {
            //Recupero le entità
            //Compongo il dto in entrata
            RoleDto dto = new RoleDto
            {
                Name = name,
                Description = description
            };

            //Recupero le entità
            IList<Role> entities = _authorizeService.FetchRoles(dto, pagination?.StartRowIndex, pagination?.MaxRowIndex, orderBy?.OrderByProperty, orderBy?.OrderByType);

            //Conto i risultati
            int count = _authorizeService.CountRoles(dto, null, null);

            //Eseugo la mappatura a Dtos
            var dtos = entities.Any() ? entities.Select(e => Mapper.Map<RoleUpdateDto>(e)).ToList() : new List<RoleUpdateDto>();

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
            RoleEssentialDataDto dto = new RoleEssentialDataDto();

            dto.Claims = _authorizeService.Fetch<Claim>(null, null, null).Select(e => Mapper.Map<ClaimDto>(e)).ToList();
            dto.Users =  _authorizeService.Fetch<User>(null, null, null).Select(e => Mapper.Map<UserDto>(e)).ToList();

            foreach (var user in dto.Users)
            {
                user.Password = null;                
            }

            return Ok(dto);
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