//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using Autofac;
using System.Linq;
using System.Reflection;

namespace Meti.Infrastructure.Configurations
{
    /// <summary>
    /// Class AutofacContainerProvider.
    /// </summary>
    public class AutofacContainerProvider
    {
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>The container.</value>
        public static IContainer Container { get; set; }

        /// <summary>
        /// Gets or sets the builder.
        /// </summary>
        /// <value>The builder.</value>
        public static ContainerBuilder Builder { get; set; }

        /// <summary>
        /// Initializes the container.
        /// </summary>
        public static void InitContainer()
        {
            Container = Builder.Build();
        }

        /// <summary>
        /// Initializes the builder.
        /// </summary>
        public static void InitBuilder()
        {
            ContainerBuilder builder = new ContainerBuilder();

            Assembly assembly = Assembly.Load("Meti");
            Assembly roleClaimAssembly = Assembly.Load("MateSharp.RoleClaim");

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(roleClaimAssembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(roleClaimAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();

            Builder = builder;
        }
    }
}
