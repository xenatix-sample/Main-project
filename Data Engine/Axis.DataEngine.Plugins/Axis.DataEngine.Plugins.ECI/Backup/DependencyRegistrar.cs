using Autofac;
using Autofac.Integration.WebApi;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.DataProvider.ECI;
using System.Reflection;

namespace Axis.DataEngine.Plugins.ECI
{
    /// <summary>
    /// To register dependencies for registration plugins
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<ECIDataProvider>().As<IECIDataProvider>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        public int Order
        {
            get { return 2; }
        }
    }
}
