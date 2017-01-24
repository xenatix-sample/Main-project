using Axis.Model.BusinessAdmin;
using Axis.Model.Common;

namespace Axis.DataProvider.BusinessAdmin.Payors
{
    public interface IPayorsDataProvider
    {
        
        Response<PayorsModel> GetPayors(string searchText);
        
        Response<PayorsModel> AddPayor(PayorsModel payorDetails);


        
        Response<PayorsModel> UpdatePayor(PayorsModel payorDetails);


        Response<PayorsModel> GetPayorByID(int payorID);
    }
}
