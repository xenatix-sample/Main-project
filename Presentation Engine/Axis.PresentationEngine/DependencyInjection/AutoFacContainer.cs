using Autofac.Integration.Mvc;
using System.Web.Http.Dependencies;

namespace Axis.PresentationEngine.DependencyInjection
{
    public class AutoFacContainer : AutofacScopeContainer, IDependencyResolver
    {
        public AutoFacContainer(AutofacDependencyResolver container)
            : base(container)
        {
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }
    }
}