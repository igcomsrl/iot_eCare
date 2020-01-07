//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using AutoMapper;
using MateSharp.Framework.Extensions.NHibernate;
using MateSharp.Framework.Models;
using MateSharp.Framework.Repository.NHibernate;
using Meti.Application.Dtos.Parameter;
using Meti.Domain.Models;
using Meti.Domain.Repository;
using Meti.Domain.ValueObjects;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Meti.Infrastructure.Repository
{
    public class ParameterRepository : RepositoryNHibernateBase<Parameter>, IParameterRepository
    {
        public ParameterRepository(ISession session) : base(session)
        {
        }

        public ParameterRepository() : base()
        {
        }

        public IList<Parameter> Fetch(string name, Guid? processId, Guid? deviceId, int? frequency, FrequencyType? frequencyType, PaginationModel pagination, OrderByModel orderBy)
        {
            var queryOver = BuildQueryOver(name, processId, deviceId, frequency, frequencyType);

            queryOver = orderBy != null && orderBy.IsValid()
                ? queryOver.OrderByString(orderBy)
                : queryOver.OrderBy(e => e.Name).Asc;

            return queryOver.Skip(pagination?.StartRowIndex ?? 0).Take(pagination?.MaxRowIndex ?? _maxRowIndexDefault).List();
        }

        public int Count(string name, Guid? processId, Guid? deviceId, int? frequency, FrequencyType? frequencyType)
        {
            var queryOver = BuildQueryOver(name, processId, deviceId, frequency, frequencyType);

            return queryOver.RowCount();
        }

        private IQueryOver<Parameter, Parameter> BuildQueryOver(string name, Guid? processId, Guid? deviceId, int? frequency, FrequencyType? frequencyType)
        {
            Process processAlias = null;
            Device deviceAlias = null;

            var queryOver = Session.QueryOver<Parameter>();

            if (!string.IsNullOrWhiteSpace(name))
                queryOver = queryOver.Where(e => e.Name.IsInsensitiveLike(name, MatchMode.Start));

            if (processId.HasValue)
                queryOver = queryOver.JoinAlias(e=>e.Process, ()=> processAlias).Where(e => e.Process.Id == processId);
            
            if (frequency.HasValue)
                queryOver = queryOver.Where(e => e.Frequency == frequency);

            if (frequencyType.HasValue)
                queryOver = queryOver.Where(e => e.FrequencyType == frequencyType);

            return queryOver;
        }

        public IList<ParameterSensorDto> GetByDevice(string uuid)
        {
            Device deviceAlias = null;
            Process processAlias = null;
            Parameter parameterAlias = null;
            Alarm alarmAlias = null;


            var queryOverDevice = Session.QueryOver<Device>()
               .Where(e => e.Macaddress == uuid)
               .Where(e => e.IsEnabled == true)
               .SingleOrDefault();

            var queryOverAlarmMetric = Session.QueryOver<AlarmMetric>()
                .JoinAlias(e => e.Device, () => deviceAlias)
                .Where(() => deviceAlias.Id == queryOverDevice.Id)
                .List();

            if (queryOverAlarmMetric == null)
                return new List<ParameterSensorDto>();

            var alarms = queryOverAlarmMetric.Select(x => x.Alarm).ToList();
            Alarm alarm = alarms.FirstOrDefault();

            ProcessInstance queryOverProcessInstance = Session.QueryOver<ProcessInstance>()
                   .JoinAlias(e => e.Process, () => processAlias)
                   .JoinAlias(() => processAlias.Parameters, () => parameterAlias)
                   .JoinAlias(() => parameterAlias.Alarms, () => alarmAlias)
                   .Where(() => processAlias.IsEnabled == true)
                   .Where(() => processAlias.ProcessType == ProcessType.Instance)
                   .Where(() => parameterAlias.IsEnabled == true)
                   .Where(() => alarmAlias.Id == alarm.Id)
                   .SingleOrDefault();

            IList<ParameterSensorDto> results = new List<ParameterSensorDto>();
            
            //Eseguo la mappatura dei dto in uscita
            foreach (var parameter in queryOverProcessInstance.Process.Parameters)
            {
                foreach (var alarmParameter in parameter.Alarms)
                {
                    if (alarms.Contains(alarmParameter))
                    {
                        ParameterSensorDto dto = Mapper.Map<ParameterSensorDto>(parameter);

                        if (!results.Select(e=>e.Id).Contains(dto.Id))
                        {
                            //Recupero il processo Id associato ai parametri
                            dto.ProcessInstanceId = queryOverProcessInstance?.Id;
                            dto.Firstname = queryOverProcessInstance?.Patient.Firstname;
                            dto.Firstname = queryOverProcessInstance?.Patient.Surname;
                            results.Add(dto);
                        }
                    }
                }

            }

            return results;
        }

    }
}

