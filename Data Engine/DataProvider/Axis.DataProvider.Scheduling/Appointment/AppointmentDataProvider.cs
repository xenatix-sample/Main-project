using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Logging;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Axis.DataProvider.Scheduling
{
    /// <summary>
    ///
    /// </summary>
    public class AppointmentDataProvider : IAppointmentDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork;

        private IRecurrenceDataProvider _recurrenceDataProvider;

        private ILogger _logger = null;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public AppointmentDataProvider(IUnitOfWork unitOfWork, IRecurrenceDataProvider recurrenceDataProvider, ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            _recurrenceDataProvider = recurrenceDataProvider;
            _logger = logger;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the appointments by date.
        /// </summary>
        /// <param name="scheduleDate">The date for which to retrieve appointments.</param>
        /// <returns></returns>
        async public Task<Response<AppointmentModel>> GetAppointmentsByDate(DateTime scheduleDate)
        {
            var appointmentRepository = unitOfWork.GetRepository<AppointmentModel>(SchemaName.Scheduling);

            List<SqlParameter> procParams = new List<SqlParameter>()
            {
                new SqlParameter("StartDate", scheduleDate),
                new SqlParameter("EndDate", DBNull.Value)
            };

            var appointments = await appointmentRepository.ExecuteStoredProcAsync("usp_GetAppointmentsByDate", procParams);

            return appointments;
        }

        /// <summary>
        /// Gets the appointments by week
        /// </summary>
        /// <param name="startDate">The week start date for which to retrieve appointments.</param>
        /// <returns></returns>
        async public Task<Response<CalendarModel>> GetResourceAppointmentsByWeek(int resourceId, int resourceTypeId, DateTime startDate)
        {
            var calendarRepository = unitOfWork.GetRepository<CalendarModel>(SchemaName.Scheduling);
            List<SqlParameter> procParams = new List<SqlParameter>()
            {
                new SqlParameter("ResourceID", resourceId),
                new SqlParameter("ResourceTypeID", resourceTypeId),
                new SqlParameter("StartDate", startDate),
                new SqlParameter("EndDate", startDate.AddDays(7).AddSeconds(-1))
            };

            var appointments = await calendarRepository.ExecuteStoredProcAsync("usp_GetResourceAppointmentsByDate", procParams);

            foreach (var appt in appointments.DataItems)
            {
                appt.Resources = new List<AppointmentResourceModel>();
                appt.Contacts = new List<AppointmentResourceModel>();
                var resources = (await GetAppointmentResource(appt.AppointmentID)).DataItems;
                foreach (var resource in resources)
                {
                    if (resource.ResourceTypeID == 7)
                    {
                        //var contact = new AppointmentContactModel { ContactID = resource.ResourceID, AppointmentID = resource.AppointmentID };
                        appt.Contacts.Add(resource);
                    }
                    else
                    {
                        appt.Resources.Add(resource);
                    }
                }
            }

            return appointments;
        }

        /// <summary>
        /// Gets the contacts by appointment ID.
        /// </summary>
        /// <param name="appointmentId">The appointment ID for which to retrieve contacts.</param>
        /// <returns></returns>
        async public Task<Response<AppointmentContactModel>> GetContactsByAppointment(long appointmentId)
        {
            var appointmentRepository = unitOfWork.GetRepository<AppointmentContactModel>(SchemaName.Scheduling);

            List<SqlParameter> procParams = new List<SqlParameter>()
            {
                new SqlParameter("AppointmentID", appointmentId)
            };

            var appointments = await appointmentRepository.ExecuteStoredProcAsync("usp_GetSchedulingContactByAppointment", procParams);

            return appointments;
        }

        /// <summary>
        /// Gets the appointments.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<AppointmentModel> GetAppointments(long contactId)
        {
            var appointmentRepository = unitOfWork.GetRepository<AppointmentModel>(SchemaName.Scheduling);

            SqlParameter contactIdParam = new SqlParameter("ContactID", contactId);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactIdParam };

            var appointments = appointmentRepository.ExecuteStoredProc("usp_GetSchedulingContactByContact", procParams);

            return appointments;
        }

        public Response<AppointmentModel> DeleteAppointmentsByRecurrence(long recurrenceID)
        {
            var appointmentRepository = unitOfWork.GetRepository<AppointmentModel>(SchemaName.Scheduling);

            SqlParameter contactIdParam = new SqlParameter("RecurrenceID", recurrenceID);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactIdParam };

            return appointmentRepository.ExecuteStoredProc("usp_DeleteAppointmentsByRecurrenceID", procParams);
        }

        /// <summary>
        /// Get the appointment
        /// </summary>
        /// <param name="appiontmentId">The appintment identifier.</param>
        /// <returns></returns>
        public Response<AppointmentModel> GetAppointment(long appiontmentId)
        {
            var appointmentRepository = unitOfWork.GetRepository<AppointmentModel>(SchemaName.Scheduling);

            SqlParameter appintmentIdParam = new SqlParameter("AppointmentID", appiontmentId);
            List<SqlParameter> procParams = new List<SqlParameter>() { appintmentIdParam };

            var appointment = appointmentRepository.ExecuteStoredProc("usp_GetAppointment", procParams);

            if (appointment.DataItems.Count > 0 && appointment.DataItems[0].RecurrenceID != null)
                appointment.DataItems[0].Recurrence = _recurrenceDataProvider.GetRecurrence(appointment.DataItems[0].RecurrenceID);

            return appointment;
        }

        /// <summary>
        /// Gets the appointment by resource.
        /// </summary>
        /// <param name="resourceID">The resource identifier.</param>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        public Response<AppointmentModel> GetAppointmentByResource(int resourceId, short resourceTypeId)
        {
            var appointmentRepository = unitOfWork.GetRepository<AppointmentModel>(SchemaName.Scheduling);

            SqlParameter resourceIdParam = new SqlParameter("ResourceID", resourceId);
            SqlParameter resourceTypeIdParam = new SqlParameter("ResourceTypeID", resourceTypeId);
            List<SqlParameter> procParams = new List<SqlParameter>() { resourceIdParam, resourceTypeIdParam };

            var appointment = appointmentRepository.ExecuteStoredProc("usp_GetAppointmentByResourceID", procParams);

            if (appointment.DataItems.Count > 0)
            {
                List<AppointmentModel> sortedList = appointment.DataItems.OrderByDescending(o => o.AppointmentDate).ToList();
                appointment.DataItems = sortedList;
                appointment.DataItems.ForEach(x =>
                {
                    if (x.RecurrenceID != null)
                        x.Recurrence = _recurrenceDataProvider.GetRecurrence(x.RecurrenceID);
                });
            }

            return appointment;
        }

        /// <summary>
        /// Gets the appointment by resource.
        /// </summary>
        /// <param name="resourceID">The resource identifier.</param>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        public Response<AppointmentModel> GetAppointmentByResourceID(int resourceId, short resourceTypeId)
        {
            var appointmentRepository = unitOfWork.GetRepository<AppointmentModel>(SchemaName.Scheduling);

            SqlParameter resourceIdParam = new SqlParameter("ResourceID", resourceId);
            SqlParameter resourceTypeIdParam = new SqlParameter("ResourceTypeID", resourceTypeId);
            List<SqlParameter> procParams = new List<SqlParameter>() { resourceIdParam, resourceTypeIdParam };

            var appointment = appointmentRepository.ExecuteStoredProc("usp_GetAppointmentByResourceID", procParams);

            return appointment;
        }

        /// <summary>
        /// Gets the appointment resource.
        /// </summary>
        /// <param name="appiontmentId">The appintment identifier.</param>
        /// <returns></returns>
        async public Task<Response<AppointmentResourceModel>> GetAppointmentResource(long appiontmentId)
        {
            var appointmentResourceRepository = unitOfWork.GetRepository<AppointmentResourceModel>(SchemaName.Scheduling);

            SqlParameter appointmentIdParam = new SqlParameter("AppointmentID", appiontmentId);
            List<SqlParameter> procParams = new List<SqlParameter>() { appointmentIdParam };

            var appointmentResource = await appointmentResourceRepository.ExecuteStoredProcAsync("usp_GetSchedulingResourceByAppointment", procParams);

            return appointmentResource;
        }

        /// <summary>
        /// Gets the appointment resource by contact.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<AppointmentResourceModel> GetAppointmentResourceByContact(long contactId)
        {
            var appointmentResourceRepository = unitOfWork.GetRepository<AppointmentResourceModel>(SchemaName.Scheduling);

            SqlParameter appointmentIdParam = new SqlParameter("ContactID", contactId);
            List<SqlParameter> procParams = new List<SqlParameter>() { appointmentIdParam };

            var appointmentResource = appointmentResourceRepository.ExecuteStoredProc("usp_GetSchedulingResourceByContact", procParams);

            return appointmentResource;
        }

        /// <summary>
        /// Gets the length of the appointment.
        /// </summary>
        /// <param name="appointmentTypeID">The appointment type identifier.</param>
        /// <returns></returns>
        public Response<AppointmentLengthModel> GetAppointmentLength(long appointmentTypeID)
        {
            var appointmentLengthRepository = unitOfWork.GetRepository<AppointmentLengthModel>(SchemaName.Scheduling);

            SqlParameter appointmentTypeIdParam = new SqlParameter("AppointmentTypeID", appointmentTypeID);
            List<SqlParameter> procParams = new List<SqlParameter>() { appointmentTypeIdParam };

            var appointmentLength = appointmentLengthRepository.ExecuteStoredProc("usp_GetAppointmentLengths", procParams);

            return appointmentLength;
        }

        /// <summary>
        /// Gets the type of the appointment.
        /// </summary>
        /// <param name="programId">The program identifier.</param>
        /// <returns></returns>
        public Response<AppointmentTypeModel> GetAppointmentType(long programId)
        {
            var appointmentTypeRepository = unitOfWork.GetRepository<AppointmentTypeModel>(SchemaName.Scheduling);

            SqlParameter programIdParam = new SqlParameter("ProgramID", programId);
            List<SqlParameter> procParams = new List<SqlParameter>() { programIdParam };

            var appointmentType = appointmentTypeRepository.ExecuteStoredProc("usp_GetAppointmentTypes", procParams);

            return appointmentType;
        }

        /// <summary>
        /// Gets the status of the appointment.
        /// </summary>
        /// <returns></returns>
        public Response<AppointmentStatusModel> GetAppointmentStatus()
        {
            var repo = unitOfWork.GetRepository<AppointmentStatusModel>(SchemaName.Reference);
            return repo.ExecuteStoredProc("usp_GetAppointmentStatus");
        }

        /// <summary>
        /// Get AppointmentTypes Mapping.
        /// </summary>
        /// <returns></returns>
        public Response<AppointmentTypeModel> GetAppointmentTypeMapping()
        {
            var appointmentTypeMappingRepository = unitOfWork.GetRepository<AppointmentTypeModel>(SchemaName.Reference);

            var appointmentTypeMapping = appointmentTypeMappingRepository.ExecuteStoredProc("usp_GetAppointmentTypeServicesMapping", null);

            return appointmentTypeMapping;
        }

        /// <summary>
        /// Adds the appointment
        /// </summary>
        /// <param name="appointment">appointment</param>
        /// <returns></returns>
        public Response<AppointmentModel> AddAppointment(AppointmentModel appointment)
        {
            var response = new Response<AppointmentModel>();
            response.DataItems = new List<AppointmentModel>();

            if (appointment.Recurrence != null && appointment.Recurrence.IsRecurring)
            {
                appointment.RecurrenceID = _recurrenceDataProvider.AddRecurrence(appointment.Recurrence);

                var recurrenceCalculator = new RecurrenceCalculator(appointment);
                var appts = recurrenceCalculator.GetAppts();

                if (appts.Count > 0)
                {
                    using (var transactionScope = unitOfWork.BeginTransactionScope())
                    {
                        try
                        {
                            appts.ForEach(appt =>
                            {
                                var appointmentParameters = BuildAppointmentSpParams(appt, false);
                                var appointmentRepository =
                                    unitOfWork.GetRepository<AppointmentModel>(SchemaName.Scheduling);

                                var apptWithID =
                                    unitOfWork.EnsureInTransaction(appointmentRepository.ExecuteNQStoredProc,
                                        "usp_AddAppointment", appointmentParameters,
                                        forceRollback: appointment.ForceRollback.GetValueOrDefault(false),
                                        idResult: true);

                                // if any of the creation of appointments fail, rollback, and return a response indicating what failed
                                if (apptWithID.ResultCode != 0)
                                    return;
                                else
                                    appt.AppointmentID = apptWithID.ID;
                            });

                            response.DataItems = appts;
                            response.ID = appts.Min(x => x.AppointmentID);

                            // no errors creating any appointments, so lock in all appointments created
                            if (!appointment.ForceRollback.GetValueOrDefault(false))
                                unitOfWork.TransactionScopeComplete(transactionScope);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex);
                            response.ResultCode = -1;
                            response.ResultMessage = "Error while saving the user's profile";
                        }
                    }
                }
            }
            else
            {
                var appointmentParameters = BuildAppointmentSpParams(appointment, false);
                var appointmentRepository = unitOfWork.GetRepository<AppointmentModel>(SchemaName.Scheduling);
                var appointmentResponse = appointmentRepository.ExecuteNQStoredProc("usp_AddAppointment", appointmentParameters, idResult: true);
                response.ID = appointment.AppointmentID = appointmentResponse.ID;
                response.DataItems.Add(appointment);
            }

            return response;
        }

        /// <summary>
        /// Adds the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        public Response<AppointmentContactModel> AddAppointmentContact(AppointmentContactModel appointmentContact)
        {
            var appointmentContactParameters = BuildAppointmentContactSpParams(appointmentContact, false);
            var appointmentContactRepository = unitOfWork.GetRepository<AppointmentContactModel>(SchemaName.Scheduling);
            return appointmentContactRepository.ExecuteNQStoredProc("usp_AddAppointmentContactDetails", appointmentContactParameters);
        }

        /// <summary>
        /// Adds the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        public Response<AppointmentResourceModel> AddAppointmentResource(List<AppointmentResourceModel> appointmentResourceDetails)
        {
            var requestXMLValueParam = new SqlParameter("AppointmentResourceXML", GenerateAppointmentResourceXml(appointmentResourceDetails));
            var responseDetailsSpParameters = new List<SqlParameter>() { requestXMLValueParam };
            var appointmentResourceRepository = unitOfWork.GetRepository<AppointmentResourceModel>(SchemaName.Scheduling);
            return appointmentResourceRepository.ExecuteNQStoredProc("usp_AddAppointmentResourceDetails", responseDetailsSpParameters);
        }

        /// <summary>
        /// Update the appointment
        /// </summary>
        /// <param name="appointment">appointment</param>
        /// <returns></returns>
        public Response<AppointmentModel> UpdateAppointment(AppointmentModel appointment)
        {
            var response = new Response<AppointmentModel>();
            response.DataItems = new List<AppointmentModel>();

            if (appointment.Recurrence != null && appointment.Recurrence.IsRecurring && appointment.IsRecurringAptEdit)
            {
                using (var transactionScope = unitOfWork.BeginTransactionScope())
                {
                    try
                    {
                        // NOTE: when cancelling the recurrence, the sp will cancel all the appointments in the recurrence
                        if (appointment.RecurrenceID != null ||
                            (appointment.RecurrenceID == null && appointment.Recurrence != null))
                        {
                            // Delete by recurrene id
                            if (appointment.RecurrenceID != null)
                                DeleteAppointmentsByRecurrence((long)appointment.RecurrenceID);
                            else if (appointment.Recurrence.RecurrenceID != 0)
                                DeleteAppointmentsByRecurrence(appointment.Recurrence.RecurrenceID);
                            else
                                DeleteAppointment(appointment.AppointmentID, DateTime.Now);

                            //if (appointment.Recurrence.IsRecurring && appointment.RecurrenceID != null)
                            //    _recurrenceDataProvider.UpdateRecurrence(appointment.Recurrence);
                            //else
                            //{
                            //    var recurrenceID = _recurrenceDataProvider.AddRecurrence(appointment.Recurrence);
                            //    appointment.RecurrenceID = recurrenceID;
                            //    if (recurrenceID != null) appointment.Recurrence.RecurrenceID = (long)recurrenceID;
                            //}

                            // Create a new recurrence every time, this way if the recurrence update results in removing
                            // future appts and leaving priors in the back there will be a different recurrence id for each one
                            var recurrenceID = _recurrenceDataProvider.AddRecurrence(appointment.Recurrence);
                            appointment.RecurrenceID = recurrenceID;
                            if (recurrenceID != null) appointment.Recurrence.RecurrenceID = (long)recurrenceID;
                        }
                        else
                        {
                            DeleteAppointment(appointment.AppointmentID, DateTime.Now);
                            var recurrenceID = _recurrenceDataProvider.AddRecurrence(appointment.Recurrence);
                            appointment.RecurrenceID = recurrenceID;
                            if (recurrenceID != null) appointment.Recurrence.RecurrenceID = (long)recurrenceID;
                        }

                        var recurrenceCalculator = new RecurrenceCalculator(appointment);
                        var apptsToAdd = recurrenceCalculator.GetAppts();

                        // delete the future appointments and recreate new appointments
                        apptsToAdd.ForEach(appt =>
                        {
                            // we only want to update/cancel future appointments
                            if (appt.AppointmentDate >= DateTime.Now)
                            {
                                var appointmentParameters = BuildAppointmentSpParams(appt, false);
                                var appointmentRepository = unitOfWork.GetRepository<AppointmentModel>(SchemaName.Scheduling);

                                var apptWithID = unitOfWork.EnsureInTransaction(appointmentRepository.ExecuteNQStoredProc, "usp_AddAppointment", appointmentParameters,
                                    forceRollback: appointment.ForceRollback.GetValueOrDefault(false), idResult: true);

                                // if any of the creation of appointments fail, rollback, and return a response indicating what failed
                                if (apptWithID.ResultCode != 0)
                                    return;
                                else
                                    appt.AppointmentID = apptWithID.ID;
                            }
                        });

                        response.DataItems = apptsToAdd;

                        // no errors creating any appointments, so lock in all appointments created
                        if (!appointment.ForceRollback.GetValueOrDefault(false))
                            unitOfWork.TransactionScopeComplete(transactionScope);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                        response.ResultCode = -1;
                        response.ResultMessage = "Error while saving the user's profile";
                    }
                }
            }
            else if (appointment.Recurrence != null && !appointment.Recurrence.IsRecurring && appointment.IsRecurringAptEdit) // appoint is being switched to a non-recurring appointment
            {
                if (appointment.RecurrenceID != null)
                    DeleteAppointmentsByRecurrence((long)appointment.RecurrenceID);

                if (!appointment.Recurrence.IsRecurring && appointment.RecurrenceID != null)
                {
                    appointment.RecurrenceID = null;
                    appointment.Recurrence = null;
                }

                var appointmentParameters = BuildAppointmentSpParams(appointment, false);
                var appointmentRepository = unitOfWork.GetRepository<AppointmentModel>(SchemaName.Scheduling);
                var resp = appointmentRepository.ExecuteNQStoredProc("usp_AddAppointment", appointmentParameters, idResult: true);
                appointment.AppointmentID = resp.ID;
                response.DataItems.Add(appointment);
            }
            else
            {
                var appointmentParameters = BuildAppointmentSpParams(appointment, true);
                appointmentParameters.Add(new SqlParameter("ModifiedBy", AuthContext.Auth.User.UserID));
                var appointmentRepository = unitOfWork.GetRepository<AppointmentModel>(SchemaName.Scheduling);
                response = appointmentRepository.ExecuteStoredProc("usp_UpdateAppointment", appointmentParameters);
                response.DataItems.Add(appointment);
            }

            return response;
        }

        /// <summary>
        /// Updates the appointment contact.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        public Response<AppointmentContactModel> UpdateAppointmentContact(AppointmentContactModel appointmentContact)
        {
            var appointmentContactParameters = BuildAppointmentContactSpParams(appointmentContact, true);
            var appointmentContactRepository = unitOfWork.GetRepository<AppointmentContactModel>(SchemaName.Scheduling);
            return appointmentContactRepository.ExecuteNQStoredProc("usp_UpdateAppointmentContactDetails", appointmentContactParameters);
        }

        /// <summary>
        /// Updates the appointment resource.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        public Response<AppointmentResourceModel> UpdateAppointmentResource(List<AppointmentResourceModel> appointmentResourceDetails)
        {
            var requestXMLValueParam = new SqlParameter("AppointmentResourceXML", GenerateAppointmentResourceXml(appointmentResourceDetails));
            var responseDetailsSpParameters = new List<SqlParameter>() { requestXMLValueParam };
            var appointmentResourceRepository = unitOfWork.GetRepository<AppointmentResourceModel>(SchemaName.Scheduling);
            return appointmentResourceRepository.ExecuteNQStoredProc("usp_UpdateAppointmentResourceDetails", responseDetailsSpParameters);
        }

        /// <summary>
        /// Delete appointment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<AppointmentModel> DeleteAppointment(long id, DateTime modifiedOn)
        {
            _recurrenceDataProvider.DeleteRecurrence(id, modifiedOn);

            var appointmentRepository = unitOfWork.GetRepository<AppointmentModel>(SchemaName.Scheduling);
            var param = new SqlParameter("AppointmentID", id);
            var modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            var procParams = new List<SqlParameter> { param, modifiedOnParam };
            var result = appointmentRepository.ExecuteNQStoredProc("usp_DeleteAppointment", procParams);
            return result;
        }

        /// <summary>
        /// Add appt note
        /// </summary>
        /// <param name="appointmentNote"></param>
        /// <returns></returns>
        public Response<AppointmentNoteModel> AddAppointmentNote(AppointmentNoteModel appointmentNote)
        {
            var param = BuildAppointmentNoteSpParams(appointmentNote, false);
            var repo = unitOfWork.GetRepository<AppointmentNoteModel>(SchemaName.Scheduling);
            return repo.ExecuteNQStoredProc("usp_AddAppointmentNote", param, false, true);
        }

        /// <summary>
        /// Get appt note
        /// </summary>
        /// <param name="appointmentID"></param>
        /// <param name="resourceID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public Response<AppointmentNoteModel> GetAppointmentNote(long appointmentID, long? contactID, long? groupID, long? userID)
        {
            var repo = unitOfWork.GetRepository<AppointmentNoteModel>(SchemaName.Scheduling);

            List<SqlParameter> procParams = new List<SqlParameter>()
            {
                new SqlParameter("AppointmentID", appointmentID),
                new SqlParameter("ContactID", (object)contactID ?? DBNull.Value),
                new SqlParameter("GroupID", (object)groupID ?? DBNull.Value),
                new SqlParameter("UserID", (object)userID ?? DBNull.Value)
            };

            var ret = repo.ExecuteStoredProc("usp_GetAppointmentNote", procParams);
            return ret;
        }

        /// <summary>
        /// Delete appt note
        /// </summary>
        /// <param name="appointmentNoteID"></param>
        /// <returns></returns>
        public Response<AppointmentNoteModel> DeleteAppointmentNote(long appointmentNoteID, DateTime modifiedOn)
        {
            var repo = unitOfWork.GetRepository<AppointmentNoteModel>(SchemaName.Scheduling);
            var param = new SqlParameter("AppointmentNoteID", appointmentNoteID);
            var modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            var procParams = new List<SqlParameter> { param, modifiedOnParam };
            var result = repo.ExecuteNQStoredProc("usp_DeleteAppointmentNote", procParams);
            return result;
        }

        /// <summary>
        /// Delete appt resource
        /// </summary>
        /// <param name="appointmentNoteID"></param>
        /// <returns></returns>
        public Response<AppointmentResourceModel> DeleteAppointmentResource(long id, DateTime modifiedOn)
        {
            var repo = unitOfWork.GetRepository<AppointmentResourceModel>(SchemaName.Scheduling);
            var param = new SqlParameter("AppointmentResourceID", id);
            var modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            var procParams = new List<SqlParameter> { param, modifiedOnParam };
            var result = repo.ExecuteNQStoredProc("usp_DeleteAppointmentResource", procParams);
            return result;
        }

        /// <summary>
        /// UPdate appt note
        /// </summary>
        /// <param name="appointmentNote"></param>
        /// <returns></returns>
        public Response<AppointmentNoteModel> UpdateAppointmentNote(AppointmentNoteModel appointmentNote)
        {
            var param = BuildAppointmentNoteSpParams(appointmentNote, true);
            var repo = unitOfWork.GetRepository<AppointmentNoteModel>(SchemaName.Scheduling);
            return repo.ExecuteNQStoredProc("usp_UpdateAppointmentNote", param);
        }

        /// <summary>
        /// Update the appointment resource no show flag.
        /// </summary>
        /// <param name="appointmentResource"></param>
        /// <returns></returns>
        public Response<AppointmentResourceModel> UpdateAppointmentNoShow(AppointmentResourceModel appointmentResource)
        {
            var idparam = new SqlParameter("AppointmentResourceID", appointmentResource.AppointmentResourceID);
            var isnoshowparam = new SqlParameter("IsNoShow", appointmentResource.IsNoShow);
            var modifiedOnParam = new SqlParameter("ModifiedOn", appointmentResource.ModifiedOn);
            var procParams = new List<SqlParameter> { idparam, isnoshowparam, modifiedOnParam };
            var repo = unitOfWork.GetRepository<AppointmentResourceModel>(SchemaName.Scheduling);
            return repo.ExecuteNQStoredProc("usp_UpdateAppointmentNoShow", procParams);
        }

        /// <summary>
        /// Add appt status dtail
        /// </summary>
        /// <param name="appointmentStatus"></param>
        /// <returns></returns>
        public Response<AppointmentStatusDetailModel> AddAppointmentStatusDetail(AppointmentStatusDetailModel appointmentStatus)
        {
            var idparam = new SqlParameter("AppointmentResourceID", appointmentStatus.AppointmentResourceID);
            var statusparam = new SqlParameter("AppointmentStatusID", appointmentStatus.AppointmentStatusID);
            var iscancelledparam = new SqlParameter("IsCancelled", (object)appointmentStatus.IsCancelled ?? DBNull.Value);
            var cancelreasonparam = new SqlParameter("CancelReasonID", (object)appointmentStatus.CancelReasonID ?? DBNull.Value);
            var commentsparam = new SqlParameter("Comments", (object)appointmentStatus.Comments ?? DBNull.Value);
            var modifiedOnParam = new SqlParameter("ModifiedOn", appointmentStatus.ModifiedOn ?? DateTime.Now);
            var procParams = new List<SqlParameter> { idparam, statusparam, iscancelledparam, cancelreasonparam, commentsparam, modifiedOnParam };
            var repo = unitOfWork.GetRepository<AppointmentStatusDetailModel>(SchemaName.Scheduling);
            return repo.ExecuteNQStoredProc("usp_AddAppointmentStatusDetail", procParams, false, true);
        }

        /// <summary>
        /// Update appt status detail
        /// </summary>
        /// <param name="appointmentStatus"></param>
        /// <returns></returns>
        public Response<AppointmentStatusDetailModel> UpdateAppointmentStatusDetail(AppointmentStatusDetailModel appointmentStatus)
        {
            var detailidparam = new SqlParameter("AppointmentStatusDetailID", appointmentStatus.AppointmentStatusDetailID);
            var idparam = new SqlParameter("AppointmentResourceID", appointmentStatus.AppointmentResourceID);
            var statusparam = new SqlParameter("AppointmentStatusID", appointmentStatus.AppointmentStatusID);
            var iscancelledparam = new SqlParameter("IsCancelled", (object)appointmentStatus.IsCancelled ?? DBNull.Value);
            var cancelreasonparam = new SqlParameter("CancelReasonID", (object)appointmentStatus.CancelReasonID ?? DBNull.Value);
            var commentsparam = new SqlParameter("Comments", (object)appointmentStatus.Comments ?? DBNull.Value);
            var modifiedOnParam = new SqlParameter("ModifiedOn", appointmentStatus.ModifiedOn ?? DateTime.Now);
            var procParams = new List<SqlParameter> { detailidparam, idparam, statusparam, iscancelledparam, cancelreasonparam, commentsparam, modifiedOnParam };
            var repo = unitOfWork.GetRepository<AppointmentStatusDetailModel>(SchemaName.Scheduling);
            return repo.ExecuteNQStoredProc("usp_UpdateAppointmentStatusDetail", procParams);
        }

        /// <summary>
        /// Get appt status detail
        /// </summary>
        /// <param name="appointmentResourceID"></param>
        /// <returns></returns>
        public Response<AppointmentStatusDetailModel> GetAppointmentStatusDetail(int appointmentResourceID)
        {
            var repo = unitOfWork.GetRepository<AppointmentStatusDetailModel>(SchemaName.Scheduling);

            List<SqlParameter> procParams = new List<SqlParameter>()
            {
                new SqlParameter("AppointmentResourceID", appointmentResourceID)
            };

            var ret = repo.ExecuteStoredProc("usp_GetAppointmentStatusDetail", procParams);
            return ret;
        }

        /// <summary>
        /// Update the appointment
        /// </summary>
        /// <param name="appointment">appointment</param>
        /// <returns></returns>
        public Response<AppointmentModel> CancelAppointment(AppointmentModel appointment)
        {
            var apptRepository = unitOfWork.GetRepository<AppointmentModel>(SchemaName.Scheduling);

            var procParams = BuildCancelParams(appointment);

            return unitOfWork.EnsureInTransaction(
                    apptRepository.ExecuteNQStoredProc,
                    "usp_CancelSingleAndRecurringAppointmentsforIndividual",
                    procParams,
                    forceRollback: appointment.ForceRollback.GetValueOrDefault(false)
                );
        }

        /// <summary>
        /// GetBlockedTimeAppointments
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="resourceTypeId"></param>
        /// <returns></returns>
        public Response<AppointmentModel> GetBlockedTimeAppointments(int resourceId, short resourceTypeId)
        {
            var appointmentRepository = unitOfWork.GetRepository<AppointmentModel>(SchemaName.Scheduling);

            SqlParameter resourceIdParam = new SqlParameter("ResourceID", resourceId);
            SqlParameter resourceTypeIdParam = new SqlParameter("ResourceTypeID", resourceTypeId);
            List<SqlParameter> procParams = new List<SqlParameter>() { resourceIdParam, resourceTypeIdParam };

            var appointment = appointmentRepository.ExecuteStoredProc("usp_GetBlockedTimeAppointments", procParams);

            return appointment;
        }

        #endregion Public Methods

            #region Helpers

            /// <summary>
            /// Builds the appointment sp parameters.
            /// </summary>
            /// <param name="appointment">The appointment.</param>
            /// <returns></returns>
        private List<SqlParameter> BuildAppointmentSpParams(AppointmentModel appointment, bool isUpdate)
        {
            var spParameters = new List<SqlParameter>();

            if (isUpdate)
                spParameters.Add(new SqlParameter("AppointmentID", appointment.AppointmentID));

            spParameters.Add(new SqlParameter("ProgramID", (object)appointment.ProgramID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("FacilityID", (object)appointment.FacilityID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("AppointmentTypeID", (object)appointment.AppointmentTypeID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ServicesID", (object)appointment.ServicesID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ServiceStatusID", (object)appointment.ServiceStatusID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("AppointmentDate", (object)appointment.AppointmentDate ?? DBNull.Value));
            spParameters.Add(new SqlParameter("AppointmentStartTime", (object)appointment.AppointmentStartTime ?? DBNull.Value));
            spParameters.Add(new SqlParameter("AppointmentLength", (object)appointment.AppointmentLength ?? DBNull.Value));
            spParameters.Add(new SqlParameter("SupervisionVisit", (object)appointment.SupervisionVisit ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ReferredBy", (object)appointment.ReferredBy ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ReasonForVisit", (object)appointment.ReasonForVisit ?? DBNull.Value));
            spParameters.Add(new SqlParameter("RecurrenceID", (object)appointment.RecurrenceID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("CancelReasonID", (object)appointment.CancelReasonID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("CancelComment", (object)appointment.CancelComment ?? DBNull.Value));
            spParameters.Add(new SqlParameter("IsCancelled", appointment.IsCancelled));
            spParameters.Add(new SqlParameter("Comments", (object)appointment.Comments ?? DBNull.Value));
            spParameters.Add(new SqlParameter("IsInterpreterRequired", (object)appointment.IsInterpreterRequired ?? DBNull.Value));
            spParameters.Add(new SqlParameter("NonMHMRAppointment", (object)appointment.NonMHMRAppointment ?? DBNull.Value));
            spParameters.Add(new SqlParameter("IsGroupAppointment", (object)appointment.IsGroupAppointment ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", appointment.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        /// <summary>
        /// Build appt note params
        /// </summary>
        /// <param name="appointmentNote"></param>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        private List<SqlParameter> BuildAppointmentNoteSpParams(AppointmentNoteModel appointmentNote, bool isUpdate)
        {
            var spParameters = new List<SqlParameter>();

            if (isUpdate)
                spParameters.Add(new SqlParameter("AppointmentNoteID", appointmentNote.AppointmentNoteID));

            spParameters.Add(new SqlParameter("ContactID", (appointmentNote.ContactID == 0) ? DBNull.Value : (object)appointmentNote.ContactID));
            spParameters.Add(new SqlParameter("GroupID", (appointmentNote.GroupID == 0) ? DBNull.Value : (object)appointmentNote.GroupID));
            spParameters.Add(new SqlParameter("UserID", (appointmentNote.UserID == 0) ? DBNull.Value : (object)appointmentNote.UserID));
            spParameters.Add(new SqlParameter("NoteTypeID", (object)appointmentNote.NoteTypeID ?? DBNull.Value));

            if (isUpdate)
                spParameters.Add(new SqlParameter("IsActive", (object)appointmentNote.IsActive ?? true));
            spParameters.Add(new SqlParameter("AppointmentID", (object)appointmentNote.AppointmentID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("NoteText", (object)appointmentNote.NoteText ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", appointmentNote.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        /// <summary>
        /// Builds the appointment contact sp parameters.
        /// </summary>
        /// <param name="appointmentContact">The appointment contact.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildAppointmentContactSpParams(AppointmentContactModel appointmentContact, bool isUpdate)
        {
            var spParameters = new List<SqlParameter>();
            if (isUpdate)
                spParameters.Add(new SqlParameter("AppointmentContactID", appointmentContact.AppointmentContactID));

            spParameters.Add(new SqlParameter("AppointmentID", appointmentContact.AppointmentID));
            spParameters.Add(new SqlParameter("ContactID", (object)appointmentContact.ContactID));
            spParameters.Add(new SqlParameter("ModifiedOn", appointmentContact.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        /// <summary>
        /// Builds the appointment resource sp parameters.
        /// </summary>
        /// <param name="appointmentResource">The appointment resource.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildAppointmentResourceSpParams(AppointmentResourceModel appointmentResource)
        {
            var spParameters = new List<SqlParameter>();
            spParameters.Add(new SqlParameter("AppointmentID", appointmentResource.AppointmentID));
            spParameters.Add(new SqlParameter("AppointmentResourceID", appointmentResource.AppointmentResourceID));
            spParameters.Add(new SqlParameter("ResourceID", (object)appointmentResource.ResourceID));
            spParameters.Add(new SqlParameter("ResourceTypeID", (object)appointmentResource.ResourceTypeID));

            return spParameters;
        }

        private string GenerateAppointmentResourceXml(List<AppointmentResourceModel> appointmentResourceDetails)
        {
            var xmlString =
                    new XElement("Appointment",
                            from appointmentResourceDetail in appointmentResourceDetails
                            select new XElement("AppointmentResourceDetails",
                                    new XElement("AppointmentResourceID", appointmentResourceDetail.AppointmentResourceID),
                                    new XElement("AppointmentID", appointmentResourceDetail.AppointmentID),
                                    new XElement("ResourceID", appointmentResourceDetail.ResourceID),
                                    new XElement("ResourceTypeID", appointmentResourceDetail.ResourceTypeID),
                                    new XElement("ParentID", appointmentResourceDetail.ParentID),
                                    new XElement("GroupHeaderID", appointmentResourceDetail.GroupHeaderID),
                                    new XElement("ModifiedOn", appointmentResourceDetail.ModifiedOn ?? DateTime.Now),
                                    new XElement("IsActive", appointmentResourceDetail.IsActive ?? true)
                                )
                            );
            return xmlString.ToString();
        }

        private List<SqlParameter> BuildCancelParams(AppointmentModel model)
        {
            var spParameters = new List<SqlParameter>();

            spParameters.AddRange(new List<SqlParameter>{
                new SqlParameter("AppointmentID", model.AppointmentID),
                new SqlParameter("RecurrenceID", (object)model.RecurrenceID ?? DBNull.Value),
                new SqlParameter("CancelReasonID", model.CancelReasonID),
                new SqlParameter("CancelComment", (object)model.CancelComment ?? DBNull.Value),
                new SqlParameter("IsSelectedAppointment", !model.IsCancelAllAppoitment),
                new SqlParameter("ModifiedOn", (object)model.ModifiedOn ?? DateTime.Now)
            });
            return spParameters;
        }
        #endregion Helpers


    }
}
