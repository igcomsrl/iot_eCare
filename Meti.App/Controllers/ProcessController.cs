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
using Meti.Application.Dtos.Process;
using Meti.Application.Dtos.Registry;
using Meti.Domain.ValueObjects;
using MateSharp.Framework.Helpers.NHibernate;
using Meti.App.Filters;

namespace Meti.App.Controllers
{
    [Authorize]
    [CatchLogException]
    public class ProcessController : ApiController
    {
        #region Private Fields

        private readonly IProcessService _processService;

        #endregion Private Fields

        #region Costructors


        public ProcessController(IProcessService ProcessService)
        {
            _processService = ProcessService;
            
        }

        #endregion Costructors

        #region Api
        
        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Create(ProcessEditDto dto)
        {
            //Recupero l'entity
            var oResult = _processService.CreateProcess(dto);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la creazione di un processo. Nome: {0}",
                   dto.Name, oResult.GetValidationErrorsInline(" - ")));
                NHibernateHelper.SessionFactory.GetCurrentSession().Transaction.Rollback();
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, oResult));
            }

            //Ritorno i risultati
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, new { id = oResult.ReturnedValue.Value}));
        }

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Update(ProcessEditDto dto)
        {
            //Recupero l'entity
            var oResult = _processService.UpdateProcess(dto);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la modifica di un processo. Nome: {0}",
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
            Process entity = _processService.Get<Process, Guid?>(id);

            //Compongo il dto
            ProcessDetailDto dto = Mapper.Map<ProcessDetailDto>(entity);
            
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
        public IHttpActionResult Delete(ProcessDetailDto dto)
        {
            //Recupero l'entity
            var oResult = _processService.DeleteProcess(dto?.Id);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la cancellazione di una Process. Id: {0} - Errore: {1}",
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
        /// <param name="ProcessType"></param>
        /// <param name="pagination"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult Fetch(string name = null, ProcessType? processType = null, [FromUri] PaginationModel pagination = null, [FromUri] OrderByModel orderBy = null)
        {
            //Recupero le entità
            var entities = _processService.Fetch(name, processType, pagination, orderBy);

            //Conto i risultati
            int count = _processService.Count(name, processType);

            //Eseugo la mappatura a Dtos
            var dtos = entities.Any() ? entities.Select(e => Mapper.Map<ProcessIndexDto>(e)).ToList() : new List<ProcessIndexDto>();

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
            ProcessEssentialDataDto dto = new ProcessEssentialDataDto();

            var registryList = _processService.Fetch<Registry>(null, null, null);

            dto.DoctorList = registryList.Where(e => e.RegistryType == RegistryType.Medico).Select(r => Mapper.Map<Registry, RegistryDetailDto>(r)).ToList();
            dto.PatientList = registryList.Where(e => e.RegistryType == RegistryType.Paziente).Select(r => Mapper.Map<Registry, RegistryDetailDto>(r)).ToList();

            return Ok(dto);
        }

        #endregion Api

        #region Dispose Pattern
        
        protected override void Dispose(bool isDisposing)
        {
            //Se sto facendo la dispose
            if (isDisposing)
            {
                _processService.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose Pattern
    }
}