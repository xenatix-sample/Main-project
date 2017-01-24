using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Axis.DataProvider.ESignature;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;

namespace Axis.DataEngine.Plugins.ESignature
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// To register dependencies for e-signature plugins
        /// </summary>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<ESignatureDataProvider>().As<IESignatureDataProvider>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        public int Order
        {
            get { return 3; }
        }
    }
}
