using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace Axis.PresentationEngine.DependencyInjection
{
    public class AutofacScopeContainer : IDependencyScope
    {
        protected AutofacDependencyResolver container;

        public AutofacScopeContainer(AutofacDependencyResolver container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            if (container.ApplicationContainer.IsRegistered(serviceType))
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
            if (container.ApplicationContainer.IsRegistered(serviceType))
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
            container.ApplicationContainer.Dispose();
        }
    }
}