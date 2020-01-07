//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Extensions.NHibernate;
using MateSharp.Framework.Models;
using MateSharp.Framework.Repository.NHibernate;
using Meti.Domain.Models;
using Meti.Domain.Repository;
using Meti.Domain.ValueObjects;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Meti.Infrastructure.Repository
{
    public class ProcessInstanceRepository : RepositoryNHibernateBase<ProcessInstance>, IProcessInstanceRepository
    {
        public ProcessInstanceRepository(ISession session) : base(session)
        {
        }

        public ProcessInstanceRepository() : base()
        {
        }

        public IList<ProcessInstance> Fetch(string name, Guid? doctorId, Guid? processId, Guid? patientId, ProcessInstanceState? processInstanceState, PaginationModel pagination, OrderByModel orderBy)
        {
            var queryOver = BuildQueryOver(name, doctorId, patientId, processId, processInstanceState);

            queryOver = orderBy != null && orderBy.IsValid()
                ? queryOver.OrderByString(orderBy)
                : queryOver.OrderBy(e => e.InsertDate).Desc.ThenBy(e => e.Name).Asc;

            return queryOver.Skip(pagination?.StartRowIndex ?? 0).Take(pagination?.MaxRowIndex ?? _maxRowIndexDefault).List();
        }

        public IList<ProcessInstance> FetchByDevice(IList<Guid?> devices)
        {
            Process processAlias = null;
            Parameter parameterAlias = null;
            Alarm alarmAlias = null;
            AlarmMetric alarmMetricAlias = null;
            Device deviceAlias = null;

            var queryOver = Session.QueryOver<ProcessInstance>()
                .JoinAlias(e => e.Process, () => processAlias)
                .JoinAlias(() => processAlias.Parameters, () => parameterAlias)
                .JoinAlias(() => parameterAlias.Alarms, () => alarmAlias)
                .JoinAlias(() => alarmAlias.AlarmMetrics, () => alarmMetricAlias)
                .JoinAlias(() => alarmMetricAlias.Device, () => deviceAlias)
                .WhereRestrictionOn(() => deviceAlias.Id).IsIn(devices.ToArray());

            return queryOver.List();
            
        }

        public int Count(string name, Guid? doctorId, Guid? patientId, Guid? processId, ProcessInstanceState? processInstanceState)
        {
            var queryOver = BuildQueryOver(name, doctorId, patientId, processId, processInstanceState);

            return queryOver.RowCount();
        }

        private IQueryOver<ProcessInstance, ProcessInstance> BuildQueryOver(string name, Guid? doctorId, Guid? patientId, Guid? processId, ProcessInstanceState? processInstanceState)
        {
            Registry doctorAlias = null;
            Registry patientAlias = null;
            Process processAlias = null;

            var queryOver = Session.QueryOver<ProcessInstance>();

            if (!string.IsNullOrWhiteSpace(name))
                queryOver = queryOver.Where(e => e.Name.IsInsensitiveLike(name, MatchMode.Start));

            if (processInstanceState.HasValue)
                queryOver = queryOver.Where(e => e.ProcessInstanceState == processInstanceState);

            if (doctorId.HasValue)
            {
                queryOver = queryOver.JoinAlias(e => e.Doctors, ()=> doctorAlias)
                    .Where(()=> doctorAlias.Id == doctorId);
            }
                

            if (patientId.HasValue)
                queryOver = queryOver.JoinAlias(e => e.Patient, ()=> patientAlias).Where(e => e.Patient.Id == patientId);

            if (processId.HasValue)
                queryOver = queryOver.JoinAlias(e => e.Process, ()=> processAlias).Where(e => e.Process.Id == processId);

            return queryOver;
        }

        public IList<ProcessInstance> GetByRegistry(Guid? registryId)
        {
            Registry registryAlias = null;

            var queryOver = Session.QueryOver<ProcessInstance>()
                .JoinAlias(e => e.Patient, () => registryAlias)
                .Where(() => registryAlias.Id == registryId);

            return queryOver.List();
        }

        public IList<ProcessInstance> GetByRegistryByEmail(string email)
        {
            Registry registryAlias = null;

            var queryOver = Session.QueryOver<ProcessInstance>()
                .JoinAlias(e => e.Patient, () => registryAlias)
                .Where(() => registryAlias.Email == email);

            return queryOver.List();
        }
    }
}