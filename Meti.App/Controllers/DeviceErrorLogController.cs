//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System;
using AutoMapper;
using MateSharp.Framework.Helpers.NHibernate;
using Meti.Application.Dtos.Device;
using Meti.Domain.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Meti.Domain.Services;
using Meti.Infrastructure.Configurations;
using MateSharp.Framework.Filters;
using MateSharp.Framework.Models;
using System.Linq;
using System.Collections.Generic;
using MateSharp.Framework.Dtos;
using Meti.App.Filters;

namespace Meti.App.Controllers
{
    [Authorize]
    [CatchLogException]
    public class DeviceErrorLogController : ApiController
    {
        private readonly IDeviceErrorLogService _deviceErrorLogService;

        #region Costructors

        public DeviceErrorLogController(IDeviceErrorLogService deviceErrorLogService)
        {
            _deviceErrorLogService = deviceErrorLogService;
        }

        #endregion Costructors

        #region Api

        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult Fetch([FromUri] PaginationModel pagination = null, [FromUri] OrderByModel orderBy = null)
        {
            //Recupero le entità
            var entities = _deviceErrorLogService.Fetch<DeviceErrorLog>(null, pagination, orderBy);

            //Conto i risultati
            int count = _deviceErrorLogService.Count<DeviceErrorLog>(null);

            //Eseugo la mappatura a Dtos
            var dtos = entities.Any() ? entities.Select(e => Mapper.Map<DeviceErrorLogDto>(e)).ToList() : new List<DeviceErrorLogDto>();

            //Compongo i risultati di ritorno
            var result = new FetchDto(dtos, count);

            //Ritorno i risultati
            return Ok(result);
        }

        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [NHibernateTransaction]
        public IHttpActionResult Log(DeviceErrorLogDto dto)
        {            
            try
            {
                var results = _deviceErrorLogService.CreateDeviceErrorLog(dto);

                if (results.HasErrors())
                {
                    Log4NetConfig.ApplicationLog.Error(string.Format("Errore durante la chiamata api/DeviceErrorLog/Log: {0}", results.GetValidationErrorsInline("-")));
                    NHibernateHelper.SessionFactory.GetCurrentSession().Transaction.Rollback();
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, results.GetValidationErrorsInline("-")));
                }
            }
            catch (Exception ex)
            {
                Log4NetConfig.ApplicationLog.Error(string.Format("Errore durante la chiamata api/DeviceErrorLog/Log: {0}", ex.Message.ToString()));
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest));
            }



            //Ritorno i risultati
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
        }

        //[HttpGet]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        ////public IHttpActionResult Log(DeviceErrorLogDto dto)
        //public IHttpActionResult Log(string error = null, Guid? deviceId = null, Guid? processInstanceId = null)
        //{
        //    //Recupero l'entity
        //    using (var session = NHibernateHelper.SessionFactory.OpenSession())
        //    {
        //        using (var transaction = session.BeginTransaction())
        //        {
        //            DeviceErrorLogDto dto = new DeviceErrorLogDto
        //            {
        //                Error = error,
        //                DeviceId = deviceId,
        //                ProcessInstanceId = processInstanceId
        //            };
        //            DeviceErrorLog entity = Mapper.Map<DeviceErrorLog>(dto);
        //            session.SaveOrUpdate(entity);
        //            transaction.Commit();
        //        }
        //    }

        //    //Ritorno i risultati
        //    return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
        //}

        #endregion Api

        #region Dispose Pattern

        protected override void Dispose(bool isDisposing)
        {
            //Se sto facendo la dispose
            if (isDisposing)
            {
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose Pattern
    }
}