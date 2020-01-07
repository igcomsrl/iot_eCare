//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Repository;
using Meti.Domain.Models;

namespace Meti.Domain.Repository
{
    public interface IAlarmMetricRepository : IRepositoryNHibernateBase<AlarmMetric>
    {
    }
}