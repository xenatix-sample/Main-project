using Axis.Model.Common.Lookups.PayorPlan;
using System.Collections.Generic;
using Axis.PresentationEngine.Helpers.Model;
using AddressViewModel = Axis.Plugins.Registration.Model.AddressViewModel;
using Axis.Model.Common.Lookups.PayorPlanGroup;

namespace Axis.Plugins.Registration.Models
{
    public class PlanGroupAndAddressModelViewModel : BaseViewModel
    {
        public List<PayorPlanGroup> PlanGroups { get; set; }
        public List<AddressViewModel> PayorAddresses { get; set; }
    }
}
