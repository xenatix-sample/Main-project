using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.RuleEngine.ESignature;
using Axis.Service.ESignature;

namespace Axis.RuleEngine.Plugins.ESignature
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<ESignatureRuleEngine>().As<IESignatureRuleEngine>();
            builder.RegisterType<ESignatureService>().As<IESignatureService>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        public int Order
        {
            get { return 3; }
        }
    }
}
