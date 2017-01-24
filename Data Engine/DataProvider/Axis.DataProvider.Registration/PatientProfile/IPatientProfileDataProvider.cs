using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.DataProvider.Registration
{
    /// <summary>
    /// Interface for Financial Assessment Data provider
    /// </summary>
    public interface IPatientProfileDataProvider
    {
        Response<PatientProfileModel> GetPatientProfile(long contactID);        
    }
}
