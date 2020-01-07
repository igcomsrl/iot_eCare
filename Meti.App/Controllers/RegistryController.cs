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
using Meti.Application.Dtos.Registry;
using MateSharp.Framework.Helpers.NHibernate;
using System.Threading.Tasks;
using MateSharp.Framework.Helpers;
using System.IO;
using Meti.Application.Dtos.File;
using Meti.App.Filters;
using Newtonsoft.Json.Linq;
using Meti.Application.Dtos.ProcessInstance;

namespace Meti.App.Controllers
{
    [Authorize]
    [CatchLogException]
    public class RegistryController : ApiController
    {
        #region Private Fields

        private readonly IRegistryService _registryService;
        private readonly IFileService _fileService;

        #endregion Private Fields

        #region Costructors

        public RegistryController(IRegistryService registryService, IFileService FileService)
        {
            _registryService = registryService;
            _fileService = FileService;

        }

        #endregion Costructors

        #region Api
        [HttpPost]
        [NHibernateTransaction]
        public async Task<IHttpActionResult> CreateFile()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            //Recupero il provider multipart
            //var provider = MultipartHelper.GetFromDataStreamProviderAsync(Path.GetTempPath());
            var provider = MultipartHelper.GetFromDataStreamProviderAsync("~/App_data/temp");

            await Task.Run(async () => await Request.Content.ReadAsMultipartAsync(provider));

            FileDto dto = new FileDto();
            Guid? registryId = null;
            if (provider.FormData.Get("registryId") != null)
                registryId = new Guid(provider.FormData.Get("registryId"));
            dto.Name = provider.FormData.Get("name");
            dto.Size = provider.FormData.Get("size");
            dto.Type = provider.FormData.Get("type");
            dto.FilepathBodypart = provider.FileData != null && provider.FileData.Count > 0
                ? provider.FileData[0].LocalFileName
                : string.Empty;

            var oResult = _fileService.CreateFile(dto, registryId);

            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante l'upload di un File. Id: {0} - Errore: {1}",
                    dto?.Id, oResult.GetValidationErrorsInline(" - ")));
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, oResult));
            }

            dto = Mapper.Map<FileDto>(oResult.ReturnedValue);

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, dto));
        }

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult DeleteFile(FileDto dto)
        {
            var oResult = _fileService.DeleteFile(dto, dto.RegistryId);

            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la cancellazione di un File. Id: {0} - Errore: {1}",
                    dto?.Id, oResult.GetValidationErrorsInline(" - ")));
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, oResult));
            }

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
        }

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Create(RegistryEditDto dto)
        {
            //Recupero l'entity
            var oResult = _registryService.CreateRegistry(dto);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la creazione di un'anagrafica. Cognome: {0}, Nome: {1}",
                   dto.Surname, dto.Firstname, oResult.GetValidationErrorsInline(" - ")));
                NHibernateHelper.SessionFactory.GetCurrentSession().Transaction.Rollback();
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, oResult));
            }

            //Ritorno i risultati
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, new { id = (Guid?)oResult.ReturnedValue}));
        }

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult Update(RegistryEditDto dto)
        {
            //Recupero l'entity
            var oResult = _registryService.UpdateRegistry(dto);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la modifica di un'anagrafica. Cognome: {0}, Nome: {1}",
                    dto.Surname, dto.Firstname, oResult.GetValidationErrorsInline(" - ")));
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
            Registry entity = _registryService.Get<Registry, Guid?>(id);

            //Compongo il dto
            RegistryDetailDto dto = Mapper.Map<RegistryDetailDto>(entity);
            
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
        public IHttpActionResult Delete(RegistryDetailDto dto)
        {
            //Recupero l'entity
            var oResult = _registryService.DeleteRegistry(dto?.Id);

            //Se ci sono stati errori, li notifico
            if (oResult.HasErrors())
            {
                Log4NetConfig.ApplicationLog.Warn(string.Format("Errore durante la cancellazione di una Registry. Id: {0} - Errore: {1}",
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
        /// <param name="registryType"></param>
        /// <param name="pagination"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult Fetch(string firstname = null, string surname = null, SexType? sex = null, RegistryType? registryType = null, [FromUri] PaginationModel pagination = null, [FromUri] OrderByModel orderBy = null)
        {
            //Recupero le entità
            var entities = _registryService.Fetch(firstname, surname, sex, registryType, pagination, orderBy);

            //Conto i risultati
            int count = _registryService.Count(firstname, surname, sex, registryType);

            //Eseugo la mappatura a Dtos
            var dtos = entities.Any() ? entities.Select(e => Mapper.Map<RegistryIndexDto>(e)).ToList() : new List<RegistryIndexDto>();

            //Compongo i risultati di ritorno
            var result = new FetchDto(dtos, count);
            
            //Ritorno i risultati
            return Ok(result);
        }

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult FetchByProcessInstanceIds(RegistryProcessInstanceDto dto)
        {
            var processInstanceIds = dto.ProcessInstanceIds;

            //Recupero le entità
            var entities = _registryService.FetchByProcessInstanceIds(processInstanceIds);

            //Ritorno i risultati
            return Ok(entities);
        }

        [HttpPost]
        [NHibernateTransaction]
        public IHttpActionResult GetByProcessInstanceIds(RegistryProcessInstanceDto dto)
        {
            var processInstanceIds = dto.ProcessInstanceIds;

            //Recupero le entità
            var entities = _registryService.FetchByProcessInstanceIds(processInstanceIds);

            IList<RegistryDetailDto> dtos = new List<RegistryDetailDto>();
            dtos.Add(entities.FirstOrDefault().Value);

            //Ritorno i risultati
            return Ok(dtos);
        }

        /// <summary>
        /// Fetches the essential data.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult FetchEssentialData()
        {
            RegistryEssentialDataDto dto = new RegistryEssentialDataDto
            {
                SexList = Enum.GetValues(typeof(SexType))
                    .Cast<SexType>()
                    .Select(item => new ItemDto<SexType?>
                    {
                        Description = item.GetDescription(),
                        Text = item.ToString(),
                        Id = item
                    }).ToList(),

                RegistryTypeList = Enum.GetValues(typeof(RegistryType))
                    .Cast<RegistryType>()
                    .Select(item => new ItemDto<RegistryType?>
                    {
                        Description = item.GetDescription(),
                        Text = item.ToString(),
                        Id = item
                    }).ToList(),

                BloodGroupList = Enum.GetValues(typeof(BloodGroup))
                    .Cast<BloodGroup>()
                    .Select(item => new ItemDto<BloodGroup?>
                    {
                        Description = item.GetDescription(),
                        Text = item.ToString(),
                        Id = item
                    }).ToList(),
                LifeStyleList = Enum.GetValues(typeof(LifeStyle))
                    .Cast<LifeStyle>()
                    .Select(item => new ItemDto<LifeStyle?>
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
                _registryService.Dispose();
                _fileService.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose Pattern
    }
}