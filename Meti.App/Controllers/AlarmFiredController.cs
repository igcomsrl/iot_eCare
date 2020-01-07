//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using AutoMapper;
using MateSharp.Framework.Dtos;
using MateSharp.Framework.Filters;
using MateSharp.Framework.Helpers.NHibernate;
using MateSharp.Framework.Models;
using Meti.App.Filters;
using Meti.Application.Dtos.AlarmFired;
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
    public class AlarmFiredController : ApiController
    {
        #region Private Fields

        private readonly IAlarmFiredService _alarmFiredService;

        #endregion Private Fields

        #region Costructors

        public AlarmFiredController(IAlarmFiredService alarmFiredService)
        {
            _alarmFiredService = alarmFiredService;
        }

        #endregion Costructors

        #region Api

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Create(AlarmFiredEditDto dto)
        {
            //Recupero l'entity
            var oResult = _alarmFiredService.CreateAlarmFired(dto);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la creazione di un allarme. id: {0}, parameterId: {1}, Errors: {2}",
                   dto.Id, dto.ParameterId, oResult.GetValidationErrorsInline(" - ")));
                NHibernateHelper.SessionFactory.GetCurrentSession().Transaction.Rollback();
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, oResult));
            }

            //Ritorno i risultati
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
        }


        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult TurnOff(AlarmFiredEditDto dto)
        {
            //Recupero l'entity
            var oResult = _alarmFiredService.TurnOff(dto);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la modifica di un allarme. id: {0}, parameterName: {1}, Errors: {2}",
                    dto.Id, dto.ParameterId, oResult.GetValidationErrorsInline(" - ")));
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
        /// <param name="AlarmFiredType"></param>
        /// <param name="pagination"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult Fetch(bool? isActive, [FromUri] PaginationModel pagination = null, [FromUri] OrderByModel orderBy = null)
        {
            //Recupero le entità
            var entities = _alarmFiredService.Fetch<AlarmFired>(e => e.IsActive == isActive, pagination, orderBy);

            var count = _alarmFiredService.Count<AlarmFired>(e => e.IsActive == isActive);

            //Eseugo la mappatura a Dtos
            var dtos = entities.Any() ? entities.Select(e => Mapper.Map<AlarmFiredSwiftDto>(e)).ToList() : new List<AlarmFiredSwiftDto>();

            //Compongo i risultati di ritorno
            var result = new FetchDto(dtos, count);

            //Ritorno i risultati
            return Ok(result);
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
            var entity = _alarmFiredService.Get<AlarmFired, Guid?>(id);
            
            //Eseugo la mappatura a Dtos
            var dtos = Mapper.Map<AlarmFiredDetailDto>(entity);
            
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
                _alarmFiredService.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose Pattern
    }
}