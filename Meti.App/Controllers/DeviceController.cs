//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using Meti.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Meti.Domain.ValueObjects;
using System.Net;
using System.Net.Http;
using MateSharp.Framework.Extensions;
using MateSharp.Framework.Dtos;
using Meti.Domain.Models;
using Meti.Domain.Services;
using Meti.Infrastructure.Configurations;
using MateSharp.Framework.Filters;
using MateSharp.Framework.Models;
using AutoMapper;
using Meti.Application.Dtos.Device;
using MateSharp.Framework.Helpers.NHibernate;
using Meti.App.Filters;
using Meti.Application.Dtos.Alarm;
using Meti.Application.Dtos.ProcessInstance;

namespace Meti.App.Controllers
{
    [Authorize]
    [CatchLogException]
    public class DeviceController : ApiController
    {
        #region Private Fields

        private readonly IDeviceService _deviceService;
        private readonly IProcessInstanceService _processInstanceService;

        #endregion Private Fields

        #region Costructors


        public DeviceController(IDeviceService DeviceService, IProcessInstanceService processInstanceService)
        {
            _deviceService = DeviceService;
            _processInstanceService = processInstanceService;
        }

        #endregion Costructors

        #region Api
        
        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Create(DeviceEditDto dto)
        {
            //Recupero l'entity
            var oResult = _deviceService.CreateDevice(dto);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la creazione di un dispositivo. Nome: {0}, Macaddress: {1}",
                   dto.Name, dto.Macaddress, oResult.GetValidationErrorsInline(" - ")));

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, oResult));
            }

            //Ritorno i risultati
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
        }

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Update(DeviceEditDto dto)
        {
            //Recupero l'entity
            var oResult = _deviceService.UpdateDevice(dto);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la modifica di un dispositivo. Nome: {0}, Macaddress: {1}",
                    dto.Name, dto.Macaddress, oResult.GetValidationErrorsInline(" - ")));
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
            Device entity = _deviceService.Get<Device, Guid?>(id);

            //Compongo il dto
            DeviceDetailDto dto = Mapper.Map<DeviceDetailDto>(entity);
            
            //Ritorno i risultati
            return Ok(dto);
        }


        /// <summary>
        /// Fetches the essential data.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult FetchEssentialData()
        {
            var processInstances = _processInstanceService.Fetch(string.Empty, null, null, null, ProcessInstanceState.Open, null, null);

            DeviceEssentialDataDto dto = new DeviceEssentialDataDto
            {
                ProcessInstanceList = processInstances.Select(e => Mapper.Map<ProcessInstanceIndexDto>(e)).ToList()
            };

            return Ok(dto);
        }

        /// <summary>
        /// Deletes the specified dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Delete(DeviceDetailDto dto)
        {
            //Recupero l'entity
            var oResult = _deviceService.DeleteDevice(dto?.Id);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la cancellazione di un Device. Id: {0} - Errore: {1}",
                   dto?.Id, oResult.GetValidationErrorsInline(" - ")));
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
        /// <param name="DeviceType"></param>
        /// <param name="pagination"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult Fetch(string name = null, string macAddress = null, Guid? processInstanceId= null, [FromUri] PaginationModel pagination = null, [FromUri] OrderByModel orderBy = null)
        {
            //Recupero le entità
            var entities = _deviceService.Fetch(name, macAddress, processInstanceId, pagination, orderBy);

            //Conto i risultati
            int count = _deviceService.Count(name, macAddress, processInstanceId);

            //Eseugo la mappatura a Dtos
            //var dtos = entities.Any() ? entities.Select(e => Mapper.Map<DeviceIndexDto>(e)).ToList() : new List<DeviceIndexDto>();

            //Compongo i risultati di ritorno
            var result = new FetchDto(entities, count);
            
            //Ritorno i risultati
            return Ok(result);
        }                

        #endregion Api

        #region Dispose Pattern
        
        protected override void Dispose(bool isDisposing)
        {
            //Se sto facendo la dispose
            if (isDisposing)
            {
                _deviceService.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose Pattern
    }
}