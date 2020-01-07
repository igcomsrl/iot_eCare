//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
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
using MateSharp.Framework.Helpers.NHibernate;
using System.Threading.Tasks;
using MateSharp.Framework.Helpers;
using System.IO;
using Meti.Application.Dtos.File;
using Meti.Application.Dtos.Process;
using Meti.Application.Dtos.ProcessInstance;

namespace Meti.App.Controllers
{
    [Authorize]
    public class HealthRiskController : ApiController
    {
        #region Private Fields

        private readonly IHealthRiskService _healthRiskService;

        #endregion Private Fields

        #region Costructors

        public HealthRiskController(IHealthRiskService healthRiskService)
        {
            _healthRiskService = healthRiskService;

        }

        #endregion Costructors

        #region Api
        
        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Create(HealthRiskEditDto dto)
        {
            //Recupero l'entity
            var oResult = _healthRiskService.CreateHealthRisk(dto);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la creazione di un health risk. Type: {0}, Level: {1}",
                   dto.Type, dto.Level, oResult.GetValidationErrorsInline(" - ")));
                NHibernateHelper.SessionFactory.GetCurrentSession().Transaction.Rollback();
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, oResult));
            }

            //Ritorno i risultati
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, new {id = oResult.ReturnedValue.Value}));
        }

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Update(HealthRiskEditDto dto)
        {
            //Recupero l'entity
            var oResult = _healthRiskService.UpdateHealthRisk(dto);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la creazione di un health risk. Type: {0}, Level: {1}",
                   dto.Type, dto.Level, oResult.GetValidationErrorsInline(" - ")));
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
            HealthRisk entity = _healthRiskService.Get<HealthRisk, Guid?>(id);

            //Compongo il dto
            HealthRiskDetailDto dto = Mapper.Map<HealthRiskDetailDto>(entity);
            
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
        public IHttpActionResult Delete(HealthRiskDetailDto dto)
        {
            //Recupero l'entity
            var oResult = _healthRiskService.DeleteHealthRisk(dto?.Id);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la cancellazione di una HealthRisk. Id: {0} - Errore: {1}",
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
        /// <param name="HealthRiskType"></param>
        /// <param name="pagination"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult Fetch(HealthRiskType? type = null, HealthRiskLevel? level = null, string rating = null, DateTime? startDate = null, DateTime? endDate = null, bool? isLast = null, Guid? registryId = null, [FromUri] PaginationModel pagination = null, [FromUri] OrderByModel orderBy = null)
        {
            if (!registryId.HasValue)
                return Ok(new FetchDto(null, 0));

            //Recupero le entità
            var entities = _healthRiskService.Fetch(type, level, rating, startDate, endDate, isLast, registryId, pagination, orderBy);

            //Conto i risultati
            int count = _healthRiskService.Count(type, level, rating, startDate, endDate, isLast, registryId);

            //Eseugo la mappatura a Dtos
            var dtos = entities.Any() ? entities.Select(e => Mapper.Map<HealthRiskIndexDto>(e)).ToList() : new List<HealthRiskIndexDto>();

            //Compongo i risultati di ritorno
            var result = new FetchDto(dtos, count);
            
            //Ritorno i risultati
            return Ok(result);
        }
        
        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult FetchByProcessInstanceIds(HealthRiskProcessInstanceDto dto)
        {
            var processInstanceIds = dto?.ProcessInstanceIds;

            //Recupero le entità
            var entities = _healthRiskService.FetchByProcessInstanceIds(processInstanceIds);

            //Ritorno i risultati
            return Ok(entities);
        }

        /// <summary>
        /// Fetches the essential data.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult FetchEssentialData()
        {
            HealthRiskEssentialDataDto dto = new HealthRiskEssentialDataDto
            {
                HealthRiskTypeList = Enum.GetValues(typeof(HealthRiskType))
                    .Cast<HealthRiskType>()
                    .Select(item => new ItemDto<HealthRiskType?>
                    {
                        Description = item.GetDescription(),
                        Text = item.ToString(),
                        Id = item
                    }).ToList(),

                HealthRiskLevelList = Enum.GetValues(typeof(HealthRiskLevel))
                    .Cast<HealthRiskLevel>()
                    .Select(item => new ItemDto<HealthRiskLevel?>
                    {
                        Description = item.GetDescription(),
                        Text = item.ToString(),
                        Id = item
                    }).ToList()
            };
            
            return Ok(dto);
        }

        #endregion Api

        #region Dispose Pattern
        
        protected override void Dispose(bool isDisposing)
        {
            //Se sto facendo la dispose
            if (isDisposing)
            {
                _healthRiskService.Dispose();                
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose Pattern
    }
}