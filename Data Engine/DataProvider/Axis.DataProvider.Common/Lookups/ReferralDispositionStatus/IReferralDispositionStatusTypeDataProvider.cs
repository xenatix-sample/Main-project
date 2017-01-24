using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.DataProvider.Common
{
    public interface IReferralDispositionStatusTypeDataProvider
    {
        /// <summary>
        /// Gets the type of the referral.
        /// </summary>
        /// <returns></returns>
        Response<ReferralDispositionStatusTypeModel> GetReferralDispositionStatusType();
    }
}
