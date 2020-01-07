//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using Meti.Application.Dtos;
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
using MateSharp.Framework.Helpers.NHibernate;
using Meti.Application.Dtos.Process;
using Meti.Application.Dtos.ProcessInstance;
using Meti.Application.Dtos.Registry;
using Meti.Domain.ValueObjects;
using MateSharp.Framework.Extensions;
using Meti.App.Filters;

namespace Meti.App.Controllers
{
    [Authorize]
    [CatchLogException]
    public class ProcessInstanceController : ApiController
    {
        #region Private Fields

        private readonly IProcessInstanceService _processInstanceService;
        private readonly IProcessService _processService;

        #endregion Private Fields

        #region Costructors


        public ProcessInstanceController(IProcessInstanceService ProcessInstanceService, IProcessService processService)
        {
            _processInstanceService = ProcessInstanceService;
            _processService = processService;
        }

        #endregion Costructors

        #region Api
        
        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Create(ProcessInstanceEditDto dto)
        {
            //Recupero l'entity
            var oResult = _processInstanceService.CreateProcessInstance(dto);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la creazione di un ProcessInstanceo. Nome: {0}",
                   dto.Name, oResult.GetValidationErrorsInline(" - ")));
                NHibernateHelper.SessionFactory.GetCurrentSession().Transaction.Rollback();
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, oResult));
            }

            //Ritorno i risultati
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, new { id = oResult.ReturnedValue.Value}));
        }

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Update(ProcessInstanceEditDto dto)
        {
            //Recupero l'entity
            var oResult = _processInstanceService.UpdateProcessInstance(dto);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la modifica di un ProcessInstanceo. Nome: {0}",
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
            ProcessInstance entity = _processInstanceService.Get<ProcessInstance, Guid?>(id);

            //Compongo il dto
            ProcessInstanceDetailDto dto = Mapper.Map<ProcessInstanceDetailDto>(entity);
            
            //Ritorno i risultati
            return Ok(dto);
        }

        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult GetByRegistry(Guid? registryId)
        {
            //Recupero l'entity
            IList<ProcessInstance> entities = _processInstanceService.GetByRegistry(registryId);

            //Compongo il dto
            IList<ProcessInstanceDetailDto> dtos = entities.Select(e => Mapper.Map<ProcessInstanceDetailDto>(e)).ToList();

            //Ritorno i risultati
            return Ok(dtos);
        }

        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult GetByRegistryEmail(string email)
        {
            //Recupero l'entity
            IList<ProcessInstance> entities = _processInstanceService.GetByRegistryByEmail(email);

            //Compongo il dto
            IList<ProcessInstanceDetailDto> dtos = entities.Select(e => Mapper.Map<ProcessInstanceDetailDto>(e)).ToList();

            //Ritorno i risultati
            return Ok(dtos);
        }


        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult FetchProcessInstanceGeo()
        {
            //Recupero l'entity
            IList<ProcessInstance> entities = _processInstanceService.Fetch<ProcessInstance>(null, null, null);

            //Compongo il dto
            IList<GeolocationSwiftDto> dtos = entities.Select(e=> Mapper.Map<GeolocationSwiftDto>(e)).ToList();

            //Ritorno i risultati
            return Ok(dtos);
        }

        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult GetProcessInstanceGeo(Guid? processInstanceId)
        {
            //Recupero l'entity
            ProcessInstance entity = _processInstanceService.Get<ProcessInstance, Guid?>(processInstanceId);

            //Compongo il dto
            GeolocationDto dto = Mapper.Map<GeolocationDto>(entity);

            //Ritorno i risultati
            return Ok(dto);
        }

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult UpdatePositionLast(UpdatePositionLastDto dto)
        {
            //Recupero l'entity
            var oResult = _processInstanceService.UpdatePositionLast(dto.ProcessInstanceId, dto.Latitude, dto.Longitude);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante l'aggiornamento della posizione. ProcessInstanceId: {0} - Errori: {1}",
                    dto.ProcessInstanceId, oResult.GetValidationErrorsInline(" - ")));
                NHibernateHelper.SessionFactory.GetCurrentSession().Transaction.Rollback();
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, oResult));
            }

            //Ritorno i risultati
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
        }

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Open(ProcessInstanceEditDto dto)
        {
            //Recupero l'entity
            var oResult = _processInstanceService.OpenProcessInstance(dto.Id);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante l'apertura del processo. ProcessInstanceId: {0} - Errori: {1}",
                   dto.Id, oResult.GetValidationErrorsInline(" - ")));
                NHibernateHelper.SessionFactory.GetCurrentSession().Transaction.Rollback();
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, oResult));
            }

            //Ritorno i risultati
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
        }

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Close(ProcessInstanceEditDto dto)
        {
            //Recupero l'entity
            var oResult = _processInstanceService.CloseProcessInstance(dto.Id);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la chiusura del processo. ProcessInstanceId: {0} - Errori: {1}",
                   dto.Id, oResult.GetValidationErrorsInline(" - ")));
                NHibernateHelper.SessionFactory.GetCurrentSession().Transaction.Rollback();
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, oResult));
            }

            //Ritorno i risultati
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
        }

        /// <summary>
        /// Deletes the specified dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Delete(ProcessInstanceDetailDto dto)
        {
            //Recupero l'entity
            var oResult = _processInstanceService.DeleteProcessInstance(dto?.Id);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la cancellazione di una ProcessInstance. Id: {0} - Errore: {1}",
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
        /// <param name="ProcessInstanceType"></param>
        /// <param name="pagination"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult Fetch(string name = null, Guid? doctorId = null, Guid? processId = null, Guid? patientId = null, ProcessInstanceState? processInstanceState= null,  [FromUri] PaginationModel pagination = null, [FromUri] OrderByModel orderBy = null)
        {
            //Recupero le entità
            var entities = _processInstanceService.Fetch(name, doctorId, processId, patientId, processInstanceState, pagination, orderBy);

            //Conto i risultati
            int count = _processInstanceService.Count(name, doctorId, processId, patientId, processInstanceState);

            //Eseugo la mappatura a Dtos
            var dtos = entities.Any() ? entities.Select(e => Mapper.Map<ProcessInstanceIndexDto>(e)).ToList() : new List<ProcessInstanceIndexDto>();

            //Compongo i risultati di ritorno
            var result = new FetchDto(dtos, count);

            //Ritorno i risultati
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));            
        }        

        /// <summary>
        /// Fetches the essential data.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult FetchEssentialData()
        {
            ProcessInstanceEssentialDataDto dto = new ProcessInstanceEssentialDataDto();

            var registryList = _processInstanceService.Fetch<Registry>(null, null, null);
            var processList = _processService.Fetch<Process>(e=>e.ProcessType == ProcessType.Template, null, null);

            dto.DoctorList = registryList.Where(e => e.RegistryType == RegistryType.Medico).Select(r => Mapper.Map<Registry, RegistryDetailDto>(r)).ToList();
            dto.PatientList = registryList.Where(e => e.RegistryType == RegistryType.Paziente).Select(r => Mapper.Map<Registry, RegistryDetailDto>(r)).ToList();
            dto.ReferencePersonList = registryList.Where(e => e.RegistryType == RegistryType.ReferencePerson).Select(r => Mapper.Map<Registry, RegistryDetailDto>(r)).ToList();
            dto.ProcessList = processList.Select(r => Mapper.Map<Process, ProcessEditDto>(r)).ToList();
            dto.ProcessInstanceStateList = Enum.GetValues(typeof(ProcessInstanceState)).Cast<ProcessInstanceState>()
                .Select(item => new ItemDto<ProcessInstanceState>
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
                _processInstanceService.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose Pattern
    }
}