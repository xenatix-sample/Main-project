using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.GroupType;

namespace Axis.DataProvider.Common.Lookups.GroupType
{
    public class GroupTypeDataProvider : IGroupTypeDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors

        public GroupTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        public Response<GroupTypeModel> GetGroupType()
        {
            var repository = _unitOfWork.GetRepository<GroupTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetGroupType");

            return results;
        }

        #endregion
    }
}
