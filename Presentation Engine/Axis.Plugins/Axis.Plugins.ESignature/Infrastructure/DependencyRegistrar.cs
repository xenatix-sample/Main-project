using Autofac;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.Plugins.ESignature.Repository;

namespace Axis.Plugins.ESignature.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<ESignatureRepository>().As<IESignatureRepository>();
        }

        public int Order
        {
            get { return 3; }
        }
    }
}