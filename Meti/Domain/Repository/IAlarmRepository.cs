//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System;
using MateSharp.Framework.Models;
using MateSharp.Framework.Repository;
using Meti.Domain.Models;
using System.Collections.Generic;

namespace Meti.Domain.Repository
{
    public interface IAlarmRepository : IRepositoryNHibernateBase<Alarm>
    {
    }
}
