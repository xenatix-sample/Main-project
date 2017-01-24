using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Data.Repository;
using Axis.Model.Common;
using Axis.Model.RecordedServices;
using System.Data.SqlClient;
using Axis.Data.Repository.Schema;

namespace Axis.DataProvider.RecordedServices.VoidServices
{
    /// <summary>
    /// 
    /// </summary>
    public class VoidServiceDataProvider : IVoidServiceDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork = null;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VoidServiceDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public VoidServiceDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// set service as void
        /// </summary>
        /// <param name="voidService">The voidService identifier.</param>
        /// <returns></returns>
        public Response<VoidServiceModel> VoidService(VoidServiceModel voidService)
        {
            var voidServiceRepository = unitOfWork.GetRepository<VoidServiceModel>(SchemaName.Core);
            var procParams = BuildParams(voidService,false);
            var response = unitOfWork.EnsureInTransaction(voidServiceRepository.ExecuteNQStoredProc, "usp_AddServiceRecordingVoid", procParams,
                                idResult: true, forceRollback: voidService.ForceRollback.GetValueOrDefault(false));

            return response;
        }

        /// <summary>
        /// set call center service as void
        /// </summary>
        /// <param name="voidService">The voidService identifier.</param>
        /// <returns></returns>
        public Response<VoidServiceModel> VoidServiceCallCenter(VoidServiceModel voidService)
        {
            var voidServiceRepository = unitOfWork.GetRepository<VoidServiceModel>(SchemaName.Core);
            var procParams = BuildParams(voidService,true);
            var response = unitOfWork.EnsureInTransaction(voidServiceRepository.ExecuteNQStoredProc, "usp_AddServiceRecordingVoidCallCenter", procParams,
                                idResult: true, forceRollback: voidService.ForceRollback.GetValueOrDefault(false));

            return response;
        }

        /// <summary>
        /// get void service
        /// </summary>
        /// <param name="serviceRecordingID">service recording identifier.</param>
        /// <returns></returns>
        public Response<VoidServiceModel> GetVoidService(int serviceRecordingID)
        {
            var voidServiceRepository = unitOfWork.GetRepository<VoidServiceModel>(SchemaName.Core);
            var procParams = new List<SqlParameter> { new SqlParameter("ServiceRecordingID", serviceRecordingID) };
            var response = voidServiceRepository.ExecuteStoredProc("usp_GetServiceRecordingVoid", procParams);
            return response;
        }

        #endregion Public Methods

        #region Private Methods

        private List<SqlParameter> BuildParams(VoidServiceModel voidServiceModel, bool isCreateCopyToEdit)
        {
            var spParameters = new List<SqlParameter>();

            if (isCreateCopyToEdit)
                spParameters.Add(new SqlParameter("IsCreateCopyToEdit", voidServiceModel.IsCreateCopyToEdit));

            spParameters.AddRange(new List<SqlParameter>{
                new SqlParameter("ServiceRecordingID",  voidServiceModel.ServiceRecordingID),
                new SqlParameter("ServiceRecordingVoidReasonID", voidServiceModel.ServiceRecordingVoidReasonID),
                new SqlParameter("ContactID", (object) voidServiceModel.ContactID ?? DBNull.Value),
                new SqlParameter("IncorrectOrganization", voidServiceModel.IncorrectOrganization),
                new SqlParameter("IncorrectServiceType", voidServiceModel.IncorrectServiceType),                
                new SqlParameter("IncorrectServiceItem", voidServiceModel.IncorrectServiceItem),
                new SqlParameter("IncorrectServiceStatus", voidServiceModel.IncorrectServiceStatus),
                new SqlParameter("IncorrectSupervisor", voidServiceModel.IncorrectSupervisor),
                new SqlParameter("IncorrectAdditionalUser", voidServiceModel.IncorrectAdditionalUser),
                new SqlParameter("IncorrectAttendanceStatus", voidServiceModel.IncorrectAttendanceStatus),
                new SqlParameter("IncorrectStartDate", voidServiceModel.IncorrectStartDate),
                new SqlParameter("IncorrectStartTime", voidServiceModel.IncorrectStartTime),
                new SqlParameter("IncorrectEndDate", (object)voidServiceModel.IncorrectEndDate ?? DBNull.Value),
                new SqlParameter("IncorrectEndTime", voidServiceModel.IncorrectEndTime),
                new SqlParameter("IncorrectDeliveryMethod", voidServiceModel.IncorrectDeliveryMethod),
                new SqlParameter("IncorrectServiceLocation", voidServiceModel.IncorrectServiceLocation),
                new SqlParameter("IncorrectRecipientCode", voidServiceModel.IncorrectRecipientCode),
                new SqlParameter("IncorrectTrackingField", voidServiceModel.IncorrectTrackingField),
                new SqlParameter("Comments", (object)voidServiceModel.Comments ?? DBNull.Value),
                new SqlParameter("NoteHeaderID", (object)voidServiceModel.NoteHeaderID ?? DBNull.Value),
                new SqlParameter("ModifiedOn", voidServiceModel.ModifiedOn)
            });
            return spParameters;
        }

        #endregion Private Methods
    }
}
