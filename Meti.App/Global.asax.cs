//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using MateSharp.Framework.Helpers.NHibernate;
using System;

namespace Meti.App
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            NHibernateHelper.BindNHibernateUnitOfWork();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            NHibernateHelper.UnbindNHibernateUnitOfWork();
        }
    }
}