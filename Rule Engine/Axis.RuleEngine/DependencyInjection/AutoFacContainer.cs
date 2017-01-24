using Autofac.Integration.WebApi;
using System.Web.Http.Dependencies;

namespace Axis.RuleEngine.Service.DependencyInjection
{
    public class AutoFacContainer : AutofacScopeContainer, IDependencyResolver
    {
        public AutoFacContainer(AutofacWebApiDependencyResolver container)
            : base(container)
        {
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }
    }
}