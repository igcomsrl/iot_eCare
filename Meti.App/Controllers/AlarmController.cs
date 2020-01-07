//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using AutoMapper;
using MateSharp.Framework.Dtos;
using MateSharp.Framework.Extensions;
using MateSharp.Framework.Filters;
using MateSharp.Framework.Helpers.NHibernate;
using MateSharp.Framework.Models;
using Meti.App.Filters;
using Meti.Application.Dtos.Alarm;
using Meti.Domain.Models;
using Meti.Domain.Services;
using Meti.Domain.ValueObjects;
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
    public class AlarmController : ApiController
    {
        #region Private Fields

        private readonly IAlarmService _AlarmService;

        #endregion Private Fields

        #region Costructors

        public AlarmController(IAlarmService AlarmService)
        {
            _AlarmService = AlarmService;
        }

        #endregion Costructors

        #region Api


        /// <summary>
        /// Fetches the essential data.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [NHibernateTransaction]
        public IHttpActionResult FetchEssentialData()
        {
            AlarmEssentialDataDto dto = new AlarmEssentialDataDto
            {
                AlarmColorList = Enum.GetValues(typeof(AlarmColor))
                              .Cast<AlarmColor>()
                              .Select(item => new ItemDto<AlarmColor?>
                              {
                                  Description = item.GetDescription(),
                                  Text = item.ToString(),
                                  Id = item
                              }).ToList(),
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
                _AlarmService.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose Pattern
    }
}