//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using MateSharp.Framework.Dtos;
using Meti.Domain.Models;
using Meti.Domain.Services;
using Meti.Infrastructure.Configurations;
using MateSharp.Framework.Filters;
using MateSharp.Framework.Models;
using AutoMapper;
using Meti.Application.Dtos.Alarm;
using Meti.Application.Dtos.Parameter;
using Meti.Domain.ValueObjects;
using Meti.Application.Dtos.Device;
using MateSharp.Framework.Extensions;
using MateSharp.Framework.Helpers.NHibernate;
using Meti.App.Filters;

namespace Meti.App.Controllers
{
    [Authorize]
    [CatchLogException]
    public class ParameterController : ApiController
    {
        #region Private Fields

        private readonly IParameterService _parameterService;
        private readonly IAlarmService _alarmService;
        private readonly IDeviceService _deviceService;

        #endregion Private Fields

        #region Costructors


        public ParameterController(IParameterService parameterService, IDeviceService deviceService, IAlarmService alarmService)
        {
            _parameterService = parameterService;
            _deviceService = deviceService;
            _alarmService = alarmService;
        }

        #endregion Costructors

        #region Api
        
        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Create(ParameterEditDto dto)
        {
            //Recupero l'entity
            var oResult = _parameterService.CreateParameter(dto);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la creazione di un Parametro. Nome: {0}",
                   dto.Name, oResult.GetValidationErrorsInline(" - ")));
                NHibernateHelper.SessionFactory.GetCurrentSession().Transaction.Rollback();
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, oResult));
            }

            //Ritorno i risultati
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, new { id = oResult.ReturnedValue.Value}));
        }

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Update(ParameterEditDto dto)
        {
            //Recupero l'entity
            var oResult = _parameterService.UpdateParameter(dto);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la modifica di un Parametro. Nome: {0}",
                    dto.Name, oResult.GetValidationErrorsInline(" - ")));
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
            Parameter entity = _parameterService.Get<Parameter, Guid?>(id);

            //Compongo il dto
            ParameterDetailDto dto = Mapper.Map<ParameterDetailDto>(entity);
            
            //Ritorno i risultati
            return Ok(dto);
        }

        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult GetByDevice(string uuid)
        {
            //Recupero i dto
            IList<ParameterSensorDto> dtos = _parameterService.GetByDevice(uuid);
            
            //Ritorno i risultati
            return Ok(dtos);
        }

        //[HttpGet]
        //[NHibernateTransaction]
        //public IHttpActionResult GetByDeviceDetail(string uuid)
        //{
        //    //Recupero i dto
        //    IList<ParameterSensorDetailDto> dtos = _parameterService.GetByDeviceDetail(uuid);

        //    //Ritorno i risultati
        //    return Ok(dtos);
        //}

        /// <summary>
        /// Deletes the specified dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Delete(ParameterDetailDto dto)
        {
            //Recupero l'entity
            var oResult = _parameterService.DeleteParameter(dto?.Id);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la cancellazione di una Parameter. Id: {0} - Errore: {1}",
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
        /// <param name="ParameterType"></param>
        /// <param name="pagination"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult Fetch(string name = null, Guid? processId = null, Guid? deviceId = null, int? frequency = null, FrequencyType? frequencyType = null, [FromUri] PaginationModel pagination = null, [FromUri] OrderByModel orderBy = null)
        {
            //Recupero le entità
            var entities = _parameterService.Fetch(name, processId, deviceId, frequency, frequencyType, pagination, orderBy);

            //Conto i risultati
            int count = _parameterService.Count(name, processId, deviceId, frequency, frequencyType);

            //Eseugo la mappatura a Dtos
            var dtos = entities.Any() ? entities.Select(e => Mapper.Map<ParameterIndexDto>(e)).ToList() : new List<ParameterIndexDto>();

            //Compongo i risultati di ritorno
            var result = new FetchDto(dtos, count);

            //Ritorno i risultati
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processInstanceId">Escludi tutti i process instance tranne questo</param>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult FetchEssentialData(Guid? processInstanceId = null)
        {
            ParameterEssentialDataDto dto = new ParameterEssentialDataDto();

            dto.AlarmList = _alarmService.Fetch<Alarm>(null, null, null).Select(e=> Mapper.Map<Alarm, AlarmDetailDto>(e)).ToList();
            dto.DeviceList = _deviceService.FetchWithoutProcessInstanceId(processInstanceId, null, null).Select(e => Mapper.Map<Device, DeviceDetailDto>(e)).ToList();

            dto.FrequencyTypeList = Enum.GetValues(typeof(FrequencyType))
                .Cast<FrequencyType>()
                .Select(item => new ItemDto<FrequencyType?>
                {
                    Description = item.GetDescription(),
                    Text = item.ToString(),
                    Id = item
                }).ToList();

            return Ok(dto);
        }

        #endregion Api

        #region Dispose Pattern
        
        protected override void Dispose(bool isDisposing)
        {
            //Se sto facendo la dispose
            if (isDisposing)
            {
                _parameterService.Dispose();
                _deviceService.Dispose();
                _alarmService.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose Pattern
    }
}