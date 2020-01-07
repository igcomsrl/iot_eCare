//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System;
using System.Configuration;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.WebApi;
using MateSharp.Framework.Extensions.NHibernate;
using Meti.App;
using Meti.App.App_Start;
using Meti.Infrastructure.Configurations;
using Meti.Infrastructure.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Meti.App
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        public HttpConfiguration config;

        public IAppBuilder app;

        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="appBuilder"></param>
        public void Configuration(IAppBuilder appBuilder)
        {
            config = new HttpConfiguration();
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            app = appBuilder;

            ConfigureOAuth();

            ConfigureNHibernate();

            ConfigureAutomapper();

            ConfigureAutofac();

            ConfigureWebApi();

            ConfigureMvc();

            ConfigureBundles();

            ConfigureLog();

            ConfigureCors();
        }

        #region Configurations 

        private void ConfigureOAuth()
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(Convert.ToInt32(ConfigurationManager.AppSettings["AccessTokenExpirationHours"])),                
                Provider = new MetiAuthTokenProvider(),
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private void ConfigureNHibernate()
        {
            app.UseNHibernate();
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
        }

        private void ConfigureAutomapper()
        {
            AutomapperConfig.InitMapper();
        }

        private void ConfigureAutofac()
        {
            AutofacContainerProvider.InitBuilder();
            AutofacContainerProvider.Builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            AutofacContainerProvider.InitContainer();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(AutofacContainerProvider.Container);
            app.UseAutofacMiddleware(AutofacContainerProvider.Container);
            app.UseAutofacWebApi(config);
        }

        private void ConfigureWebApi()
        {
            //Configuro le lettere iniziali minuscoli delle proprietà dei messaggi di ritorno.
            config
                .Formatters
                .JsonFormatter
                .SerializerSettings
                .ContractResolver = new CamelCasePropertyNamesContractResolver();

            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }

        private void ConfigureMvc()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private void ConfigureBundles()
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void ConfigureLog()
        {
            Log4NetConfig.Configure();
        }

        private void ConfigureCors()
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
        }

        #endregion
    }
}
