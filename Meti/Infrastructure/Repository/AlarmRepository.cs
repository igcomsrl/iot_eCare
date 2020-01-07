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

namespace Meti.Infrastructure.Repository
{
    public class AlarmRepository : RepositoryNHibernateBase<Alarm>, IAlarmRepository
    {
        public AlarmRepository(ISession session) : base(session)
        {
        }

        public AlarmRepository() : base()
        {
        }
        
    }
}