﻿using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Axis.DataProvider.ECI.EligibilityStatement;
using Axis.DataProvider.ECI.Screening;
using Axis.Helpers.Infrastructure;
using Axis.Helpers.Infrastructure.DependencyManagement;
using Axis.DataProvider.ECI;
using System.Reflection;
using Axis.DataProvider.ECI.EligibilityDetermination;

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
            builder.RegisterType<EligibilityDeterminationDataProvider>().As<IEligibilityDeterminationDataProvider>();
            builder.RegisterType<ScreeningDataProvider>().As<IScreeningDataProvider>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        public int Order
        {
            get { return 2; }
        }
    }
}
