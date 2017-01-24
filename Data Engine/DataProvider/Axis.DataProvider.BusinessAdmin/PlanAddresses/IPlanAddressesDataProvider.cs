using Axis.Model.BusinessAdmin;
using Axis.Model.Common;

namespace Axis.DataProvider.BusinessAdmin.PlanAddresses
{
    public interface IPlanAddressesDataProvider
    {
       
        Response<PlanAddressesModel> GetPlanAddresses(int payorPlanID);

        
        Response<PlanAddressesModel> GetPlanAddress(int payorAddressID);

        
        Response<PlanAddressesModel> AddPlanAddress(PlanAddressesModel payorDetails);

        
        Response<PlanAddressesModel> UpdatePlanAddress(PlanAddressesModel payorDetails);
    }
}
