using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace Axis.RuleEngine.Service.DependencyInjection
{
    public class AutofacScopeContainer : IDependencyScope
    {
        protected AutofacWebApiDependencyResolver container;

        public AutofacScopeContainer(AutofacWebApiDependencyResolver container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            if (container.Container.IsRegistered(serviceType))
            {
                return this.container.GetService(serviceType);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (container.Container.IsRegistered(serviceType))
            {
                return this.container.GetServices(serviceType);
            }
            else
            {
                return new List<object>();
            }
        }

        public void Dispose()
        {
            container.Container.Dispose();
        }
    }
}