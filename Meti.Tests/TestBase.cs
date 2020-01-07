//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Helpers.NHibernate;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meti.Tests
{
    public class TestBase
    {
        protected ISession _session;

        public TestBase()
        {
            NHibernateHelper.InitNHibernate();
            _session = NHibernateHelper.SessionFactory.OpenSession();
        }
    }
}
