using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.GroupService;

namespace Axis.DataProvider.Common.Lookups.GroupService
{
    public class GroupServiceDataProvider : IGroupServiceDataProvider
    {
        #region Class Variables

        readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors

        public GroupServiceDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        public Response<GroupServiceModel> GetGroupService()
        {
            var repository = _unitOfWork.GetRepository<GroupServiceModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetGroupServices");

            return results;
        }

        #endregion
    }
}
