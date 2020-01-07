//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using log4net;

namespace Meti.Infrastructure.Configurations
{
    /// <summary>
    /// Class Log4NetConfig.
    /// </summary>
    public static class Log4NetConfig
    {
        /// <summary>
        /// Gets or sets the application log.
        /// </summary>
        /// <value>The application log.</value>
        public static ILog ApplicationLog { get; set; }

        /// <summary>
        /// Configures the log4 net.
        /// </summary>
        public static void Configure()
        {
            log4net.Config.XmlConfigurator.Configure();

            ApplicationLog = LogManager.GetLogger("ApplicationLogger");
        }
    }
}