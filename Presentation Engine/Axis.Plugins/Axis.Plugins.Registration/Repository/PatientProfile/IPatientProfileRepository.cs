using Axis.Model.Common;
using Axis.Plugins.Registration.Model;
using System.Threading.Tasks;
namespace Axis.Plugins.Registration.Repository.PatientProfile
{
    public interface IPatientProfileRepository
    {
         /// <summary>
        /// Gets the Patient Profile.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Task<Response<PatientProfileViewModel>> GetPatientProfile(long contactID);        
    }
}
