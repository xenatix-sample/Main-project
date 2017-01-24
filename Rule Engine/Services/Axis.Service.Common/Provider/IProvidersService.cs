﻿using Axis.Model.Common;

namespace Axis.Service.Common.Provider
{
    public interface IProvidersService
    {
        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <param name="filterCriteria">The filter criteria.</param>
        /// <returns></returns>
        Response<ProvidersModel> GetProviders(int filterCriteria);
        /// <summary>
        /// Gets the provider by id.
        /// </summary>
        /// <param name="providerID">The provider ID.</param>
        /// <returns></returns>
        Response<ProvidersModel> GetProviderByid(int providerID);
    }
}

