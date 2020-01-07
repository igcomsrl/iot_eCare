//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using Meti.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace Meti.App.Filters
{
    public class CatchLogExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionContext)
        {
            if (actionContext.Exception == null) return;

            Exception exception = actionContext.Exception;

            Log4NetConfig.ApplicationLog.Error(exception.Message, exception);
            Log4NetConfig.ApplicationLog.Error(exception.StackTrace);
        }
    }
}