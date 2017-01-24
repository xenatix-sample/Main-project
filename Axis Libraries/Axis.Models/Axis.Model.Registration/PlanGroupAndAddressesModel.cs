using System.Collections.Generic;
using Axis.Model.Address;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlanGroup;

namespace Axis.Model.Registration
{
    public class PlanGroupAndAddressesModel : BaseEntity
    {
        public List<PayorPlanGroup> PlanGroups { get; set; }
        public List<AddressModel> PayorAddresses { get; set; }
    }
}
