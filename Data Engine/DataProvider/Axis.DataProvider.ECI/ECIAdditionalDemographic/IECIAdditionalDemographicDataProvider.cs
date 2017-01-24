using Axis.Model.Common;
using Axis.Model.ECI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.DataProvider.ECI
{
    public interface IECIAdditionalDemographicDataProvider
    {
        /// <summary>
        /// Gets the additional demographic.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        Response<ECIAdditionalDemographicsModel> GetAdditionalDemographic(long contactId);

        /// <summary>
        /// Adds the additional demographic.
        /// </summary>
        /// <param name="additional">The additional.</param>
        /// <returns></returns>
        Response<ECIAdditionalDemographicsModel> AddAdditionalDemographic(ECIAdditionalDemographicsModel additional);

        /// <summary>
        /// Updates the additional demographic.
        /// </summary>
        /// <param name="additional">The additional.</param>
        /// <returns></returns>
        Response<ECIAdditionalDemographicsModel> UpdateAdditionalDemographic(ECIAdditionalDemographicsModel additional);

        /// <summary>
        /// Deletes the additional demographic.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ECIAdditionalDemographicsModel> DeleteAdditionalDemographic(long id, DateTime modifiedOn);
    }
}
