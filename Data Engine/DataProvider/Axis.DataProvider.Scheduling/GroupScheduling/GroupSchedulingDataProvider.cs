using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Logging;

namespace Axis.DataProvider.Scheduling.GroupScheduling
{
    public class GroupSchedulingDataProvider : IGroupSchedulingDataProvider
    {
        #region Class Varaiables

        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppointmentDataProvider _appointmentDataProvider;
        private readonly IRecurrenceDataProvider _recurrenceDataProvider;
        private readonly ILogger _logger = null;

        #endregion

        #region Constructors

        public GroupSchedulingDataProvider(IUnitOfWork unitOfWork, IAppointmentDataProvider appointmentDataProvider, IRecurrenceDataProvider recurrenceDataProvider, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _appointmentDataProvider = appointmentDataProvider;
            _recurrenceDataProvider = recurrenceDataProvider;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        public Response<GroupSchedulingModel> GetGroupByID(long groupID)
        {
            var groupSchedulingRepository = _unitOfWork.GetRepository<GroupSchedulingModel>(SchemaName.Scheduling);
            SqlParameter groupHeaderIDParam = new SqlParameter("GroupHeaderID", groupID);
            List<SqlParameter> procParams = new List<SqlParameter>() { groupHeaderIDParam };
            var result = groupSchedulingRepository.ExecuteStoredProc("usp_GetGroupSchedulingHeader", procParams);

            if (result.ResultCode != 0 || result.DataItems.Count == 0)
            {
                return result;
            }

            //Load the group details
            var detailResult = GetGroupDetailByID(result.DataItems[0].GroupDetailID);
            result.DataItems[0].GroupDetails = detailResult.DataItems;

            return result;
        }

        public Response<GroupSchedulingResourceModel> GetGroupSchedulingResource(long groupID)
        {
            var groupSchedulingRepository = _unitOfWork.GetRepository<GroupSchedulingResourceModel>(SchemaName.Scheduling);
            SqlParameter groupHeaderIDParam = new SqlParameter("GroupHeaderID", groupID);
            List<SqlParameter> procParams = new List<SqlParameter>() { groupHeaderIDParam };
            var result = groupSchedulingRepository.ExecuteStoredProc("usp_GetGroupSchedulingResource", procParams);

            if (result.ResultCode != 0)
            {
                return result;
            }

            foreach (var resource in result.DataItems)
            {
                var resourceAppointments = _appointmentDataProvider.GetAppointmentByResourceID(resource.ResourceID, resource.ResourceTypeID);
                resource.Appointments = resourceAppointments.DataItems;
            }

            return result;
        }

        public Response<AppointmentModel> GetAppointmentByGroupID(long groupID)
        {
            var groupSchedulingRepository = _unitOfWork.GetRepository<AppointmentModel>(SchemaName.Scheduling);
            SqlParameter groupHeaderIDParam = new SqlParameter("GroupHeaderID", groupID);
            List<SqlParameter> procParams = new List<SqlParameter>() { groupHeaderIDParam };
            var appointment = groupSchedulingRepository.ExecuteStoredProc("usp_GetAppointmentByGroupID", procParams);

            if (appointment.DataItems.Count > 0 && appointment.DataItems[0].RecurrenceID != null)
                appointment.DataItems[0].Recurrence = _recurrenceDataProvider.GetRecurrence(appointment.DataItems[0].RecurrenceID);

            return appointment;
        }

        public Response<AppointmentContactModel> GetAllContactResourceNamesByAppointment(long appointmentID)
        {
            var groupSchedulingRepository = _unitOfWork.GetRepository<AppointmentContactModel>(SchemaName.Scheduling);
            SqlParameter appointmentIDParam = new SqlParameter("AppointmentID", appointmentID);
            List<SqlParameter> procParams = new List<SqlParameter>() { appointmentIDParam };
            var result = groupSchedulingRepository.ExecuteStoredProc("usp_GetContactResourceByAppointmentID", procParams);

            return result;
        }

        public Response<GroupSchedulingModel> AddGroupData(GroupSchedulingModel group)
        {
            var response = new Response<GroupSchedulingModel>() { ResultCode = -1, ResultMessage = "Error while saving the group's data" };
            using (var transactionScope = _unitOfWork.BeginTransactionScope())
            {
                try
                {
                    var groupSchedulingRepository = _unitOfWork.GetRepository<GroupSchedulingModel>(SchemaName.Scheduling);
                    var groupDetail = group.GroupDetails[0]; // This is only being used as a signle detail record for now
                    SqlParameter groupNameParam = new SqlParameter("GroupName", groupDetail.GroupName);
                    SqlParameter groupCapacityParam = new SqlParameter("GroupCapacity", groupDetail.GroupCapacity);
                    SqlParameter groupTypeIDParam = new SqlParameter("GroupTypeID", groupDetail.GroupTypeID);
                    SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now.ToUniversalTime());
                    List<SqlParameter> procParams = new List<SqlParameter>() { groupNameParam, groupCapacityParam, groupTypeIDParam, modifiedOnParam };
                    var detailResult = _unitOfWork.EnsureInTransaction(groupSchedulingRepository.ExecuteNQStoredProc, "usp_AddGroupDetails", procParams,
                        forceRollback: group.ForceRollback.GetValueOrDefault(false), adonResult: false, idResult: true);

                    if (detailResult.ResultCode != 0)
                        goto end;

                    group.GroupDetailID = detailResult.ID;
                    var headerResult = AddGroupHeader(group);
                    if (headerResult.ResultCode != 0)
                        goto end;

                    detailResult.ID = headerResult.ID;//the group header id will need to be passed back to the UI
                    response = detailResult;
                    if (!group.ForceRollback.GetValueOrDefault(false))
                        _unitOfWork.TransactionScopeComplete(transactionScope);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    response.ResultCode = -1;
                    response.ResultMessage = string.Empty;
                }
            }

            end:
            return response;
        }

        public Response<GroupSchedulingResourceModel> AddResources(List<GroupSchedulingResourceModel> resources)
        {
            var response = new Response<GroupSchedulingResourceModel>()
            {
                ResultCode = -1,
                ResultMessage = "Error while saving the group resources"
            };
            using (var transactionScope = _unitOfWork.BeginTransactionScope())
            {
                try
                {
                    var groupSchedulingRepository = _unitOfWork.GetRepository<GroupSchedulingResourceModel>(SchemaName.Scheduling);
                    SqlParameter groupResourceXmlParam = new SqlParameter("GroupResourceXML", GenerateGroupResourceXml(resources, false));
                    groupResourceXmlParam.DbType = DbType.Xml;
                    SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now.ToUniversalTime());
                    List<SqlParameter> procParams = new List<SqlParameter>() { groupResourceXmlParam, modifiedOnParam };
                    var groupResourceResult = _unitOfWork.EnsureInTransaction(groupSchedulingRepository.ExecuteNQStoredProc, "usp_AddGroupSchedulingResource", procParams,
                        forceRollback: resources[0].ForceRollback.GetValueOrDefault(false));

                    if (groupResourceResult.ResultCode != 0)
                        goto end;

                    var apptResources = resources.Select(groupResource => new AppointmentResourceModel() { AppointmentID = groupResource.PrimaryAppointmentID ?? 0, ResourceTypeID = groupResource.ResourceTypeID, ResourceID = groupResource.ResourceID, GroupHeaderID = groupResource.GroupHeaderID, IsNoShow = false }).ToList();
                    var resourceAppointments =
                        _appointmentDataProvider.AddAppointmentResource(apptResources);
                    if (resourceAppointments.ResultCode != 0)
                        goto end;

                    response = groupResourceResult;
                    if (!resources[0].ForceRollback.GetValueOrDefault(false))
                        _unitOfWork.TransactionScopeComplete(transactionScope);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    response.ResultCode = -1;
                    response.ResultMessage = string.Empty;
                }
            }

            end:
            return response;
        }

        public Response<GroupSchedulingModel> UpdateGroupData(GroupSchedulingModel group)
        {
            var response = new Response<GroupSchedulingModel>() { ResultCode = -1, ResultMessage = "Error while saving the group's data" };
            using (var transactionScope = _unitOfWork.BeginTransactionScope())
            {
                try
                {
                    var groupSchedulingRepository = _unitOfWork.GetRepository<GroupSchedulingModel>(SchemaName.Scheduling);
                    var groupDetail = group.GroupDetails[0];
                    SqlParameter groupDetailIDParam = new SqlParameter("GroupDetailID", groupDetail.GroupDetailID);
                    SqlParameter groupNameParam = new SqlParameter("GroupName", groupDetail.GroupName);
                    SqlParameter groupCapacityParam = new SqlParameter("GroupCapacity", groupDetail.GroupCapacity);
                    SqlParameter groupTypeIDParam = new SqlParameter("GroupTypeID", groupDetail.GroupTypeID);
                    SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now.ToUniversalTime());
                    List<SqlParameter> procParams = new List<SqlParameter>() { groupDetailIDParam, groupNameParam, groupCapacityParam, groupTypeIDParam, modifiedOnParam };
                    var detailResult = _unitOfWork.EnsureInTransaction(groupSchedulingRepository.ExecuteNQStoredProc, "usp_UpdateGroupDetails", procParams,
                        forceRollback: group.ForceRollback.GetValueOrDefault(false));

                    if (detailResult.ResultCode != 0)
                        goto end;

                    var headerResult = UpdateGroupHeader(group);
                    if (headerResult.ResultCode != 0)
                        goto end;

                    response = detailResult;
                    if (!group.ForceRollback.GetValueOrDefault(false))
                        _unitOfWork.TransactionScopeComplete(transactionScope);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    response.ResultCode = -1;
                    response.ResultMessage = string.Empty;
                }
            }

            end:
            return response;
        }

        public Response<GroupSchedulingResourceModel> UpdateResources(List<GroupSchedulingResourceModel> resources)
        {
            var response = new Response<GroupSchedulingResourceModel>()
            {
                ResultCode = -1,
                ResultMessage = "Error while saving the group resources"
            };
            var successfulResponse = new Response<GroupSchedulingResourceModel>()
            {
                ResultCode = 0,
                ResultMessage = "Executed Successfully"
            };
            using (var transactionScope = _unitOfWork.BeginTransactionScope())
            {
                try
                {
                    var groupSchedulingRepository = _unitOfWork.GetRepository<GroupSchedulingResourceModel>(SchemaName.Scheduling);
                    SqlParameter groupResourceXmlParam = new SqlParameter("GroupResourceXML",
                        GenerateGroupResourceXml(resources, true))
                    { DbType = DbType.Xml };
                    List<SqlParameter> procParams = new List<SqlParameter>() { groupResourceXmlParam };

                    var groupResourceResult = _unitOfWork.EnsureInTransaction(groupSchedulingRepository.ExecuteNQStoredProc, "usp_UpdateGroupSchedulingResource", procParams,
                        forceRollback: resources[0].ForceRollback.GetValueOrDefault(false));

                    if (groupResourceResult.ResultCode != 0)
                        goto end;

                    var apptResources = resources.Select(groupResource => new AppointmentResourceModel() { AppointmentResourceID = groupResource.AppointmentResourceID ?? 0, AppointmentID = groupResource.PrimaryAppointmentID ?? 0, ResourceTypeID = groupResource.ResourceTypeID, ResourceID = groupResource.ResourceID, GroupHeaderID = groupResource.GroupHeaderID, IsNoShow = false }).ToList();
                    var resourceAppointments =
                        _appointmentDataProvider.UpdateAppointmentResource(apptResources);
                    if (resourceAppointments.ResultCode != 0)
                        goto end;

                    response = successfulResponse;
                    if (!resources[0].ForceRollback.GetValueOrDefault(false))
                        _unitOfWork.TransactionScopeComplete(transactionScope);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    response.ResultCode = -1;
                    response.ResultMessage = string.Empty;
                }
            }

            end:
            return response;
        }

        /// <summary>
        /// Delete group scheduling resource
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<GroupSchedulingResourceModel> DeleteGroupSchedulingResource(long id, DateTime modifiedOn)
        {
            var repo = _unitOfWork.GetRepository<GroupSchedulingResourceModel>(SchemaName.Scheduling);
            var param = new SqlParameter("GroupResourceID", id);
            var modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            var procParams = new List<SqlParameter> { param, modifiedOnParam };
            var result = repo.ExecuteNQStoredProc("usp_DeleteeGroupSchedulingResource", procParams);
            return result;
        }

        #endregion

        #region Private Methods

        private Response<GroupSchedulingDetailModel> GetGroupDetailByID(long? groupDetailID)
        {
            var groupSchedulingDetailRepository = _unitOfWork.GetRepository<GroupSchedulingDetailModel>(SchemaName.Scheduling);
            SqlParameter groupDetailIDParam = new SqlParameter("GroupDetailID", groupDetailID);
            List<SqlParameter> procParams = new List<SqlParameter>() { groupDetailIDParam };
            var result = groupSchedulingDetailRepository.ExecuteStoredProc("usp_GetGroupDetails", procParams);

            return result;
        }

        private Response<GroupSchedulingDetailModel> AddGroupHeader(GroupSchedulingModel group)
        {
            var groupSchedulingRepository = _unitOfWork.GetRepository<GroupSchedulingDetailModel>(SchemaName.Scheduling);
            SqlParameter groupDetailIDParam = new SqlParameter("GroupDetailID", group.GroupDetailID);
            SqlParameter commentsParam = new SqlParameter("Comments", group.Comments);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now.ToUniversalTime());
            List<SqlParameter> procParams = new List<SqlParameter>() { groupDetailIDParam, commentsParam, modifiedOnParam };
            var result = _unitOfWork.EnsureInTransaction(groupSchedulingRepository.ExecuteNQStoredProc, "usp_AddGroupSchedulingHeader", procParams,
                        forceRollback: group.ForceRollback.GetValueOrDefault(false), adonResult: false, idResult: true);

            return result;
        }

        private Response<GroupSchedulingDetailModel> UpdateGroupHeader(GroupSchedulingModel group)
        {
            var groupSchedulingRepository = _unitOfWork.GetRepository<GroupSchedulingDetailModel>(SchemaName.Scheduling);
            SqlParameter groupHeaderIDParam = new SqlParameter("GroupHeaderID", group.GroupHeaderID);
            SqlParameter commentsParam = new SqlParameter("Comments", group.Comments);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now.ToUniversalTime());
            List<SqlParameter> procParams = new List<SqlParameter>() { groupHeaderIDParam, commentsParam, modifiedOnParam };
            var result = _unitOfWork.EnsureInTransaction(groupSchedulingRepository.ExecuteNQStoredProc, "usp_UpdateGroupSchedulingHeader", procParams,
                        forceRollback: group.ForceRollback.GetValueOrDefault(false));

            return result;
        }

        private string GenerateGroupResourceXml(List<GroupSchedulingResourceModel> resources, bool isUpdate)
        {
            using (StringWriter sWriter = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings { OmitXmlDeclaration = true };
                using (XmlWriter xWriter = XmlWriter.Create(sWriter, settings))
                {
                    xWriter.WriteStartElement("GroupResource");
                    foreach (var resource in resources)
                    {
                        xWriter.WriteStartElement("GroupSchedulingResource");
                        if (isUpdate)
                        {
                            xWriter.WriteElementString("GroupResourceID", resource.GroupResourceID.ToString());
                        }
                        xWriter.WriteElementString("GroupHeaderID", resource.GroupHeaderID.ToString());
                        xWriter.WriteElementString("ResourceID", resource.ResourceID.ToString());
                        xWriter.WriteElementString("ResourceTypeID", resource.ResourceTypeID.ToString());
                        xWriter.WriteElementString("IsPrimary", false.ToString());
                        xWriter.WriteElementString("IsActive", true.ToString());
                        xWriter.WriteElementString("ModifiedOn", DateTime.Now.ToUniversalTime().ToString(CultureInfo.InvariantCulture));
                        xWriter.WriteEndElement();
                    }

                    xWriter.WriteEndElement();
                }

                return sWriter.ToString();
            }
        }

        #endregion
    }
}
