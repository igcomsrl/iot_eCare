//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Extensions.NHibernate;
using MateSharp.Framework.Models;
using MateSharp.Framework.Repository.NHibernate;
using Meti.Domain.Models;
using Meti.Domain.Repository;
using NHibernate;

namespace Meti.Infrastructure.Repository
{
    public class AlarmMetricRepository : RepositoryNHibernateBase<AlarmMetric>, IAlarmMetricRepository
    {
        public AlarmMetricRepository(ISession session) : base(session)
        {
        }

        public AlarmMetricRepository() : base()
        {
        }
        
    }
}