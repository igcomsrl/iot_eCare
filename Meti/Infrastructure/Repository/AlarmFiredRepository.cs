//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Repository.NHibernate;
using Meti.Domain.Models;
using Meti.Domain.Repository;
using NHibernate;

namespace Meti.Infrastructure.Repository
{
    public class AlarmFiredRepository : RepositoryNHibernateBase<AlarmFired>, IAlarmFiredRepository
    {
        public AlarmFiredRepository(ISession session) : base(session)
        {
        }

        public AlarmFiredRepository() : base()
        {
        }
    }
}