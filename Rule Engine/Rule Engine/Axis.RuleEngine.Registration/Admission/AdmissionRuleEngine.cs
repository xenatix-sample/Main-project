using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Registration.Admission
{
    public class AdmissionRuleEngine : IAdmissionRuleEngine
    {

        private readonly IAdmissionService _admissionService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="admissionService"></param>
        public AdmissionRuleEngine(IAdmissionService admissionService)
        {
            this._admissionService = admissionService;
        }

        /// <summary>
        /// To get list of admissiona
        /// </summary>
        /// <param name="contactID"></param>       
        /// <returns></returns>
        public Response<AdmissionModal> GetAdmission(long contactID)
        {
            return _admissionService.GetAdmission(contactID);
        }

        /// <summary>
        /// To add admission
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Response<AdmissionModal> AddAdmission(AdmissionModal model)
        {
            return _admissionService.AddAdmission(model);
        }

        /// <summary>
        /// To update admission
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Response<AdmissionModal> UpdateAdmission(AdmissionModal model)
        {
            return _admissionService.UpdateAdmission(model);
        }        

    }
}
