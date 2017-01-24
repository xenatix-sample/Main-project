using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Registration
{
    public interface IQuickRegistrationRuleEngine
    {
        Response<QuickRegistrationModel> GetQuickRegistration();
        Response<QuickRegistrationModel> AddQuickRegistration(QuickRegistrationModel patient);
        Response<QuickRegistrationModel> GetMrnMpi();

    }
}
