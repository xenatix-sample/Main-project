using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.Model.Common.ServiceRecording;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.DataProvider.Common.IServiceRecordingDataProvider" />
    public class ServiceRecordingDataProvider : IServiceRecordingDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRecordingDataProvider" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ServiceRecordingDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the service recording.
        /// </summary>
        /// <param name="SourceHeaderID">The source header identifier.</param>
        /// <param name="ServiceRecordingSourceID">The service recording source identifier.</param>
        /// <returns></returns>
        public Response<ServiceRecordingModel> GetServiceRecording(long SourceHeaderID, int ServiceRecordingSourceID)
        {
            var repository = _unitOfWork.GetRepository<ServiceRecordingModel>(SchemaName.Core);
            var procParams = new List<SqlParameter> { new SqlParameter("SourceHeaderID", SourceHeaderID),
                                                      new SqlParameter("ServiceRecordingSourceID", ServiceRecordingSourceID) };

            var result = repository.ExecuteStoredProc("usp_GetServiceRecording", procParams);
            if (result.DataItems.Count > 0)
            {
                var spParams = new List<SqlParameter> { new SqlParameter("ServiceRecordingID", result.DataItems[0].ServiceRecordingID) };
                var attendeeRepository = _unitOfWork.GetRepository<ServiceRecordingAttendeeModel>(SchemaName.Core);
                var attendeeResult = attendeeRepository.ExecuteStoredProc("usp_GetServiceRecordingAttendees", spParams);
                result.DataItems[0].AttendedList = attendeeResult.DataItems;

                var userParams = new List<SqlParameter> { new SqlParameter("ServiceRecordingID", result.DataItems[0].ServiceRecordingID) };
                var additionalUserRepository = _unitOfWork.GetRepository<ServiceRecordingAdditionalUserModel>(SchemaName.Core);
                var additionalUserResult = additionalUserRepository.ExecuteStoredProc("usp_GetServiceRecordingAdditionalUsers", userParams);
                result.DataItems[0].AdditionalUserList = additionalUserResult.DataItems;
            }
            return result;
        }

        /// <summary>
        /// Adds the Service Recording.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Response<ServiceRecordingModel> AddServiceRecording(ServiceRecordingModel model)
        {
            Response<ServiceRecordingModel> serviceRecordingResult = new Response<ServiceRecordingModel>();
            serviceRecordingResult.ResultCode = 0;
            using (var transactionScope = _unitOfWork.BeginTransactionScope())
            {
                var referralRepository = _unitOfWork.GetRepository<ServiceRecordingModel>(SchemaName.Core);
                var procParams = BuildParams(model);

                serviceRecordingResult = _unitOfWork.EnsureInTransaction(referralRepository.ExecuteNQStoredProc, "usp_AddServiceRecording", procParams,
                                                                          forceRollback: model.ForceRollback.GetValueOrDefault(false), idResult: true);
                if (serviceRecordingResult.ResultCode == 0)
                {
                    // Only call center record should update end time // Hard coded 1,6 should replace with Enum.
                    if (model.ServiceRecordingSourceID == 1 || model.ServiceRecordingSourceID == 6)
                    {
                        Response<ServiceRecordingModel> endTimeResult = UpdateEndTime(model);
                        if (endTimeResult.ResultCode != 0)
                        {
                            serviceRecordingResult.ResultCode = endTimeResult.ResultCode;
                            serviceRecordingResult.ResultMessage = endTimeResult.ResultMessage;
                        }
                    }
                    AddUpdateAdditionalDetails(model, serviceRecordingResult, serviceRecordingResult.ID);
                }
                _unitOfWork.TransactionScopeComplete(transactionScope);
            }
            return serviceRecordingResult;
        }

        /// <summary>
        /// Updates the Service Recording.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Response<ServiceRecordingModel> UpdateServiceRecording(ServiceRecordingModel model)
        {
            Response<ServiceRecordingModel> serviceRecordingResult = new Response<ServiceRecordingModel>();
            serviceRecordingResult.ResultCode = 0;
            using (var transactionScope = _unitOfWork.BeginTransactionScope())
            {
                var repository = _unitOfWork.GetRepository<ServiceRecordingModel>(SchemaName.Core);
                var procParams = BuildParams(model);
                serviceRecordingResult = _unitOfWork.EnsureInTransaction(repository.ExecuteNQStoredProc, "usp_UpdateServiceRecording", procParams,
                                                                            forceRollback: model.ForceRollback.GetValueOrDefault(false));
                if (serviceRecordingResult.ResultCode == 0)
                {
                    // Only call center record should update end time // Hard coded 1,6 should replace with Enum.
                    if (model.ServiceRecordingSourceID == 1 || model.ServiceRecordingSourceID == 6)
                    {
                        Response<ServiceRecordingModel> endTimeResult = UpdateEndTime(model);
                        if (endTimeResult.ResultCode != 0)
                        {
                            serviceRecordingResult.ResultCode = endTimeResult.ResultCode;
                            serviceRecordingResult.ResultMessage = endTimeResult.ResultMessage;
                        }
                    }
                    AddUpdateAdditionalDetails(model, serviceRecordingResult, model.ServiceRecordingID);
                }

                _unitOfWork.TransactionScopeComplete(transactionScope);
            }

            return serviceRecordingResult;
        }

        /// <summary>
        /// Gets the service recordings.
        /// </summary>
        /// <param name="ServiceRecordingSourceID">The service recording source identifier.</param>
        /// <param name="ContactID">The contact identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public Response<ServiceRecordingModel> GetServiceRecordings(int ServiceRecordingSourceID, long ContactID, DateTime? startDate, DateTime? endDate)
        {
            var repository = _unitOfWork.GetRepository<ServiceRecordingModel>(SchemaName.Core);
            var procParams = new List<SqlParameter> { new SqlParameter("ContactID", ContactID),
                                                        new SqlParameter("ServiceRecordingSourceID", ServiceRecordingSourceID),
                                                        new SqlParameter("StartDate",  (object)startDate ?? DBNull.Value),
                                                        new SqlParameter("EndDate", (object) endDate ??DBNull.Value)
            };

            var result = repository.ExecuteStoredProc("usp_GetServiceRecordingList", procParams);
            return result;
        }

        public Response<ProgramUnitModel> GetProgramUnits(string datakey)
        {
            var repository = _unitOfWork.GetRepository<ProgramUnitModel>(SchemaName.Reference);
            var procParams = new List<SqlParameter> { new SqlParameter("DataKey", datakey) };
            var result = repository.ExecuteStoredProc("usp_GetOrganizationDetailsModuleComponent", procParams);
            return result;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Builds the parameters.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildParams(ServiceRecordingModel model)
        {
            var spParameters = new List<SqlParameter>();
            if (model.ServiceRecordingID > 0)
            {
                spParameters.Add(new SqlParameter("ServiceRecordingID", model.ServiceRecordingID));
            }
            else
            {
                spParameters.Add(new SqlParameter("ServiceRecordingHeaderID", (object)model.ServiceRecordingHeaderID ?? DBNull.Value));
            }

            spParameters.AddRange(new List<SqlParameter>
                {
                    new SqlParameter("ParentServiceRecordingID", (object)model.ParentServiceRecordingID ?? DBNull.Value),
                    new SqlParameter("ServiceRecordingSourceID", (object)model.ServiceRecordingSourceID ?? DBNull.Value),
                    new SqlParameter("SourceHeaderID",(object)model.SourceHeaderID ?? model.CallCenterHeaderID),
                    new SqlParameter("OrganizationID", (object)model.OrganizationID ?? DBNull.Value),
                    new SqlParameter("ServiceTypeID", (object)model.ServiceTypeID ?? DBNull.Value),
                    new SqlParameter("ServiceItemID", (object)model.ServiceItemID ?? DBNull.Value),
                    new SqlParameter("AttendanceStatusID", (object)model.AttendanceStatusID ?? DBNull.Value),
                    new SqlParameter("DeliveryMethodID", (object)model.DeliveryMethodID ?? DBNull.Value),
                    new SqlParameter("ServiceStatusID", (object)model.ServiceStatusID ?? DBNull.Value),
                    new SqlParameter("ServiceLocationID", (object)model.ServiceLocationID ?? DBNull.Value),
                    new SqlParameter("ServiceStartDate", (object)model.ServiceStartDate ?? DBNull.Value),
                    new SqlParameter("ServiceEndDate", (object)model.ServiceEndDate ?? DBNull.Value),
                    new SqlParameter("RecipientCodeID", (object)model.RecipientCodeID ?? DBNull.Value),
                    new SqlParameter("RecipientCode", (object)model.RecipientCode ?? DBNull.Value),
                    new SqlParameter("NumberOfRecipients", (object)model.NumberOfRecipients ?? DBNull.Value),
                    new SqlParameter("TrackingFieldID",(object)model.TrackingFieldID ?? DBNull.Value),
                    //new SqlParameter("ConversionStatusID",model.ConversionStatusID),
                    //new SqlParameter("ConversionDateTime",model.ConversionDateTime),
                    new SqlParameter("SupervisorUserID",(object)model.SupervisorUserID ?? DBNull.Value),
                    new SqlParameter("UserID",(object)model.UserID ?? DBNull.Value),
                    new SqlParameter("ModifiedOn", model.ModifiedOn ?? DateTime.Now)
            });
            if (model.ServiceRecordingID > 0)
            {
                spParameters.Add(new SqlParameter("SystemModifiedOn", (object)model.SystemModifiedOn ?? DBNull.Value));
            }

            return spParameters;
        }

        /// <summary>
        /// Adds the update additional details.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="serviceRecordingResult">The service recording result.</param>
        /// <param name="ServiceRecordingID">The service recording identifier.</param>
        private void AddUpdateAdditionalDetails(ServiceRecordingModel model, Response<ServiceRecordingModel> serviceRecordingResult, long ServiceRecordingID)
        {
            var attendeeRepository = _unitOfWork.GetRepository<ServiceRecordingAttendeeModel>(SchemaName.Core);
            Response<ServiceRecordingAttendeeModel> attendeeResult = new Response<ServiceRecordingAttendeeModel>();
            foreach (var item in model.AttendedList)
            {
                //save each
                if (item.ServiceRecordingAttendeeID != 0)
                {
                    if (!((bool)item.IsActive))
                    {       //delete
                        var removeParams = new List<SqlParameter> {
                                                    new SqlParameter("ServiceRecordingAttendeeID", item.ServiceRecordingAttendeeID),
                                                    new SqlParameter("ModifiedOn", model.ModifiedOn ?? DateTime.Now)
                                                };
                        attendeeResult = _unitOfWork.EnsureInTransaction(attendeeRepository.ExecuteNQStoredProc, "usp_DeleteServiceRecordingAttendee", removeParams,
                                                            forceRollback: model.ForceRollback.GetValueOrDefault(false));
                    }
                    else
                    {      //update
                        var attendeeParams = new List<SqlParameter> {
                                                    new SqlParameter("ServiceRecordingAttendeeID", item.ServiceRecordingAttendeeID),
                                                    new SqlParameter("ServiceRecordingID", ServiceRecordingID),
                                                    new SqlParameter("Name", item.Name),
                                                    new SqlParameter("ModifiedOn", model.ModifiedOn ?? DateTime.Now)
                                                };
                        attendeeResult = _unitOfWork.EnsureInTransaction(attendeeRepository.ExecuteNQStoredProc, "usp_UpdateServiceRecordingAttendee", attendeeParams,
                                                            forceRollback: model.ForceRollback.GetValueOrDefault(false));
                    }
                }
                else
                {
                    //add
                    var attendeeParams = new List<SqlParameter> {
                                                    new SqlParameter("ServiceRecordingID", ServiceRecordingID),
                                                    new SqlParameter("Name", item.Name),
                                                    new SqlParameter("ModifiedOn", model.ModifiedOn ?? DateTime.Now)
                                                };
                    attendeeResult = _unitOfWork.EnsureInTransaction(attendeeRepository.ExecuteNQStoredProc, "usp_AddServiceRecordingAttendee", attendeeParams,
                                                            forceRollback: model.ForceRollback.GetValueOrDefault(false), idResult: true);
                }
            }
            if (attendeeResult.ResultCode != 0)
            {
                serviceRecordingResult.ResultCode = attendeeResult.ResultCode;
                serviceRecordingResult.ResultMessage = attendeeResult.ResultMessage;
            }

            var usersRepository = _unitOfWork.GetRepository<ServiceRecordingAdditionalUserModel>(SchemaName.Core);
            Response<ServiceRecordingAdditionalUserModel> usersResult = new Response<ServiceRecordingAdditionalUserModel>();
            foreach (var item in model.AdditionalUserList)
            {
                //save each
                if (item.ServiceRecordingAdditionalUserID > 0)
                {
                    if (!((bool)item.IsActive))
                    {       //delete
                        var removeParams = new List<SqlParameter> {
                                                    new SqlParameter("ServiceRecordingAdditionalUserID", item.ServiceRecordingAdditionalUserID),
                                                    new SqlParameter("ModifiedOn", model.ModifiedOn ?? DateTime.Now)
                                                };
                        usersResult = _unitOfWork.EnsureInTransaction(usersRepository.ExecuteNQStoredProc, "usp_DeleteServiceRecordingAdditionalUser", removeParams,
                                                            forceRollback: model.ForceRollback.GetValueOrDefault(false));
                    }
                    else
                    {      //update
                        var attendeeParams = new List<SqlParameter> {
                                                    new SqlParameter("ServiceRecordingAdditionalUserID", item.ServiceRecordingAdditionalUserID),
                                                    new SqlParameter("ServiceRecordingID", ServiceRecordingID),
                                                    new SqlParameter("UserID", item.UserID),
                                                    new SqlParameter("ModifiedOn", model.ModifiedOn ?? DateTime.Now)
                                                };
                        usersResult = _unitOfWork.EnsureInTransaction(usersRepository.ExecuteNQStoredProc, "usp_UpdateServiceRecordingAdditionalUser", attendeeParams,
                                                            forceRollback: model.ForceRollback.GetValueOrDefault(false));
                    }
                }
                else
                {
                    //add
                    var attendeeParams = new List<SqlParameter> {
                                                    new SqlParameter("ServiceRecordingID", ServiceRecordingID),
                                                    new SqlParameter("UserID", item.UserID),
                                                    new SqlParameter("ModifiedOn", model.ModifiedOn ?? DateTime.Now)
                                                };
                    usersResult = _unitOfWork.EnsureInTransaction(usersRepository.ExecuteNQStoredProc, "usp_AddServiceRecordingAdditionalUser", attendeeParams,
                                                            forceRollback: model.ForceRollback.GetValueOrDefault(false), idResult: true);
                }
                if (usersResult.ResultCode != 0)
                {
                    serviceRecordingResult.ResultCode = usersResult.ResultCode;
                    serviceRecordingResult.ResultMessage = usersResult.ResultMessage;
                }
            }
        }

        /// <summary>
        /// Updates the end time.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        private Response<ServiceRecordingModel> UpdateEndTime(ServiceRecordingModel model)
        {
            var endTimeParams = new List<SqlParameter> {
                new SqlParameter("CallCenterHeaderID", model.CallCenterHeaderID),
                new SqlParameter("CallStartTime", (object)model.ServiceStartDate ?? DBNull.Value),
                new SqlParameter("CallEndTime", (object)model.ServiceEndDate ?? DBNull.Value),
                new SqlParameter("ModifiedOn", model.ModifiedOn ?? DateTime.Now)
            };
            var repository = _unitOfWork.GetRepository<ServiceRecordingModel>(SchemaName.CallCenter);

            return repository.ExecuteNQStoredProc("usp_UpdateCallCenterEndTime", endTimeParams);
        }

        #endregion Private Methods
    }
}