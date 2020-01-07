//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Extensions.NHibernate;
using MateSharp.Framework.Models;
using MateSharp.Framework.Repository.NHibernate;
using Meti.Domain.Models;
using Meti.Domain.Repository;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Meti.Infrastructure.Repository
{
    public class DeviceRepository : RepositoryNHibernateBase<Device>, IDeviceRepository
    {
        public DeviceRepository(ISession session) : base(session)
        {
        }

        public DeviceRepository() : base()
        {
        }

        /// <summary>
        /// Recupero tutti i device che non sono già stati usati in un processo di monitoraggio
        /// </summary>
        /// <param name="processInstanceId">Includo il processInstance </param>
        /// <param name="pagination"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public IList<Device> FetchWithoutProcessInstanceId(Guid? processInstanceId, PaginationModel pagination, OrderByModel orderBy)
        {
            Process processAlias = null;
            Parameter parameterAlias = null;
            Alarm alarmAlias = null;

            var subQueryProcessInstance = QueryOver.Of<ProcessInstance>()
                .Select(e => e.Process.Id);

            if (processInstanceId.HasValue)
            {
                subQueryProcessInstance = subQueryProcessInstance.Where(e => e.Id != processInstanceId);
            }

            var subQueryParameter = QueryOver.Of<Parameter>()
                .JoinAlias(e => e.Process, () => processAlias)
                .WithSubquery
                .WhereProperty(() => processAlias.Id).In(subQueryProcessInstance)
                .Select(e => e.Id);

            var subQueryAlarm = QueryOver.Of<Alarm>()
                .JoinAlias(e => e.Parameter, () => parameterAlias)
                .WithSubquery
                .WhereProperty(() => parameterAlias.Id).In(subQueryParameter)
                .Select(e => e.Id);

            var subQueryAlarmMetric = QueryOver.Of<AlarmMetric>()
                .JoinAlias(e => e.Alarm, () => alarmAlias)
                .WithSubquery
                .WhereProperty(() => alarmAlias.Id).In(subQueryAlarm)
                .Select(e => e.Device.Id);

            var devicesBusy = Session.QueryOver<Device>()
                .WithSubquery
                .WhereProperty(e => e.Id).In(subQueryAlarmMetric).List();

            var queryOver = Session.QueryOver<Device>()
                .WhereRestrictionOn(e => e.Id).Not.IsIn(devicesBusy.Select(e => e.Id).ToList());

            return queryOver.Skip(pagination?.StartRowIndex ?? 0).Take(pagination?.MaxRowIndex ?? _maxRowIndexDefault).List();
        }

        public IList<Device> Fetch(string name, string macAddress, Guid? processInstanceId, PaginationModel pagination, OrderByModel orderBy)
        {
            var queryOver = BuildQueryOver(name, macAddress, processInstanceId);

            queryOver = orderBy != null && orderBy.IsValid()
                ? queryOver.OrderByString(orderBy)
                : queryOver.OrderBy(e => e.Name).Asc;

            return queryOver.Skip(pagination?.StartRowIndex ?? 0).Take(pagination?.MaxRowIndex ?? _maxRowIndexDefault).List();
        }

        public int Count(string name, string macAddress, Guid? processInstanceId)
        {
            var queryOver = BuildQueryOver(name, macAddress, processInstanceId);

            return queryOver.RowCount();
        }

        private IQueryOver<Device, Device> BuildQueryOver(string name, string macAddress, Guid? processInstanceId)
        {
            Process processAlias = null;
            Parameter parameterAlias = null;

            var queryOver = Session.QueryOver<Device>();

            if (!string.IsNullOrWhiteSpace(name))
                queryOver = queryOver.Where(e => e.Name.IsInsensitiveLike(name, MatchMode.Start));

            if (!string.IsNullOrWhiteSpace(macAddress))
                queryOver = queryOver.Where(e => e.Macaddress == macAddress);

            if (processInstanceId.HasValue)
            {
                IList<Device> devices = new List<Device>();

                var processInstanceQueryOver = Session.QueryOver<ProcessInstance>()
                    .Where(e => e.Id == processInstanceId)
                    .SingleOrDefault();

                var parameters = processInstanceQueryOver.Process.Parameters;

                foreach (var parameter in parameters)
                {
                    foreach (var alarm in parameter.Alarms)
                    {
                        foreach (var alarmMetric in alarm.AlarmMetrics)
                        {
                            if(alarmMetric.Device != null && !devices.Contains(alarmMetric.Device))
                                devices.Add(alarmMetric.Device);
                        }
                    }
                }

                queryOver = queryOver.WhereRestrictionOn(e => e.Id).IsIn(devices.Select(e => e.Id).ToArray());
            }

            return queryOver;
        }
    }
}