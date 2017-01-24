using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Service.Registration
{
    public interface IQuickRegistrationService
    {
        Response<QuickRegistrationModel> GetQuickRegistration();
        Response<QuickRegistrationModel> Add(QuickRegistrationModel patient);
        Response<QuickRegistrationModel> GetMrnMpi();
    }
}
