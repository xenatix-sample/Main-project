using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Admin;
using Axis.Model.Admin.UserScheduling;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Axis.DataProvider.Admin
{
    public class UserSchedulingDataProvider : IUserSchedulingDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion Class Variables

        #region Constructors

        public UserSchedulingDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        public Response<UserSchedulingModel> GetUserFacilities(int userID)
        {
            
            var repository = _unitOfWork.GetRepository<UserFacilitySchedulingDataModel>(SchemaName.Scheduling);
            var procParams = new List<SqlParameter> { new SqlParameter("ResourceID", userID), 
                                                      new SqlParameter("ResourceTypeID", 2) };
            var userFacilitiesResult = repository.ExecuteStoredProc("usp_GetResourceAvailability", procParams);

            var result = userFacilitiesResult.CloneResponse<UserSchedulingModel>();
            result.ID = userID;
            if (result.ResultCode != 0)
                return result;

            result.DataItems = new List<UserSchedulingModel>();

            var groupByFacilities = from userFacility in userFacilitiesResult.DataItems
                        group userFacility by userFacility.FacilityID;

            foreach (var groupFacility in groupByFacilities)
            {
                var userFacility = new UserSchedulingModel();
                userFacility.FacilityID = groupFacility.Key;

                var groupByDays = groupFacility.GroupBy(x => x.DayOfWeekID);

                foreach (var groupByDay in groupByDays)
                {
                    var userFacilityScheduling = new UserFacilitySchedulingModel();
                    userFacilityScheduling.DayOfWeekID = groupByDay.Key;
                    foreach (var item in groupByDay)
                    {
                        userFacilityScheduling.Days = item.Days;
                        var userFacilityTimeScheduling = new UserFacilityTimeSchedulingModel();
                        userFacilityTimeScheduling.ResourceAvailabilityID = item.ResourceAvailabilityID;
                        userFacilityTimeScheduling.AvailabilityStartTime = item.AvailabilityStartTime;
                        userFacilityTimeScheduling.AvailabilityEndTime = item.AvailabilityEndTime;
                        userFacilityScheduling.UserFacilityTimeSchedule.Add(userFacilityTimeScheduling);
                    }
                    userFacility.UserFacilitySchedule.Add(userFacilityScheduling);
                   
                }

                userFacility.UserFacilitySchedule = userFacility.UserFacilitySchedule.OrderBy(x => x.DayOfWeekID).ToList();
                result.DataItems.Add(userFacility);
            }

            return result;
        }

        public Response<UserSchedulingModel> GetUserFacilitySchedule(int userID, int facilityID)
        {
            var result = new Response<UserSchedulingModel>();
            result.ResultCode = 0;

            var dataItem = new List<UserSchedulingModel>();
            var model = new UserSchedulingModel();
            model.FacilityID = facilityID;

            var repository = _unitOfWork.GetRepository<UserFacilitySchedulingDataModel>(SchemaName.Scheduling);
            var procParams = new List<SqlParameter> { new SqlParameter("ResourceID", userID), 
                                                      new SqlParameter("ResourceTypeID", 2), 
                                                      new SqlParameter("FacilityID", facilityID)};
            var userFacilitiesResult = repository.ExecuteStoredProc("usp_GetResourceAvailabilityByFacilityID", procParams);

            var groupByuserFacilities = userFacilitiesResult.DataItems.GroupBy(x => x.DayOfWeekID);
            foreach (var groupByuserFacility in groupByuserFacilities)
            {
                var userFacilityScheduling = new UserFacilitySchedulingModel();
                userFacilityScheduling.DayOfWeekID = groupByuserFacility.Key;
                foreach (var item in groupByuserFacility)
                {
                    userFacilityScheduling.Days = item.Days;
                    userFacilityScheduling.ScheduleTypeID = item.ScheduleTypeID;
                    var userFacilityTimeScheduling = new UserFacilityTimeSchedulingModel();
                    userFacilityTimeScheduling.ResourceAvailabilityID = item.ResourceAvailabilityID;
                    userFacilityTimeScheduling.AvailabilityStartTime = item.AvailabilityStartTime;
                    userFacilityTimeScheduling.AvailabilityEndTime = item.AvailabilityEndTime;
                    userFacilityScheduling.UserFacilityTimeSchedule.Add(userFacilityTimeScheduling); 
                }
                model.UserFacilitySchedule.Add(userFacilityScheduling);
            }

            if (model.UserFacilitySchedule != null)
                model.UserFacilitySchedule = model.UserFacilitySchedule.OrderBy(x => x.DayOfWeekID).ToList();

            var repositoryFacility = _unitOfWork.GetRepository<UserFacilitySchedulingDataModel>(SchemaName.Scheduling);
            var procParamsFacility = new List<SqlParameter> { new SqlParameter("ResourceID", DBNull.Value), 
                                                      new SqlParameter("ResourceTypeID", 6), 
                                                      new SqlParameter("FacilityID", facilityID)};
            var facilityScheduleResult = repositoryFacility.ExecuteStoredProc("usp_GetResourceAvailabilityByFacilityID", procParamsFacility);

            var groupByFacilities = facilityScheduleResult.DataItems.GroupBy(x => x.DayOfWeekID);
            foreach (var groupByFacility in groupByFacilities)
            {
                var facilityScheduling = new UserFacilitySchedulingModel();
                facilityScheduling.DayOfWeekID = groupByFacility.Key;
                foreach (var item in groupByFacility)
                {
                    facilityScheduling.Days = item.Days;
                    var facilityTimeScheduling = new UserFacilityTimeSchedulingModel();
                    facilityTimeScheduling.ResourceAvailabilityID = item.ResourceAvailabilityID;
                    facilityTimeScheduling.AvailabilityStartTime = item.AvailabilityStartTime;
                    facilityTimeScheduling.AvailabilityEndTime = item.AvailabilityEndTime;
                    facilityScheduling.UserFacilityTimeSchedule.Add(facilityTimeScheduling);
                }
                model.FacilitySchedule.Add(facilityScheduling);
            }

            if (model.FacilitySchedule != null)
                model.FacilitySchedule = model.FacilitySchedule.OrderBy(x => x.DayOfWeekID).ToList();

            dataItem.Add(model);
            result.DataItems = dataItem;
            
            return result;
        }

        public Response<UserSchedulingModel> SaveUserFacilitySchedule(UserSchedulingModel userFacilitySchedule)
        {
             var repository = _unitOfWork.GetRepository<UserSchedulingModel>(SchemaName.Scheduling);

            SqlParameter resourceIDParam = new SqlParameter("ResourceID", userFacilitySchedule.ResourceID);
            SqlParameter resourceTypeIDParam = new SqlParameter("ResourceTypeID", 2);
            SqlParameter facilityIDParam = new SqlParameter("FacilityID", userFacilitySchedule.FacilityID);
            SqlParameter schedulingXMLParam = new SqlParameter("SchedulingXML", GenerateRequestXml(userFacilitySchedule));
            schedulingXMLParam.DbType = System.Data.DbType.Xml;
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", userFacilitySchedule.ModifiedOn ?? DateTime.Now);

            List<SqlParameter> procParams = new List<SqlParameter>() { resourceIDParam, resourceTypeIDParam, facilityIDParam, schedulingXMLParam, modifiedOnParam };
            var result = _unitOfWork.EnsureInTransaction<Response<UserSchedulingModel>>(repository.ExecuteNQStoredProc, "usp_SaveResourceAvailability", procParams, adonResult:true);

            return result;
        }

        #endregion

        #region Helpers
        private string GenerateRequestXml(UserSchedulingModel userFacilitySchedule)
        {
            using (StringWriter sWriter = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                using (XmlWriter XWriter = XmlWriter.Create(sWriter))
                {
                    XWriter.WriteStartElement("RequestXMLValue");

                    if (userFacilitySchedule.UserFacilitySchedule != null)
                    {
                        foreach (UserFacilitySchedulingModel userSchedule in userFacilitySchedule.UserFacilitySchedule)
                        {
                            foreach (UserFacilityTimeSchedulingModel userFacilityTime in userSchedule.UserFacilityTimeSchedule)
                            {
                                XWriter.WriteStartElement("Scheduling");
                                XWriter.WriteElementString("ResourceAvailabilityID", userFacilityTime.ResourceAvailabilityID.ToString());
                                XWriter.WriteElementString("DefaultFacilityID", userFacilitySchedule.FacilityID.ToString());
                                XWriter.WriteElementString("ScheduleTypeID", userSchedule.ScheduleTypeID.ToString());
                                XWriter.WriteElementString("DayOfWeekID", userSchedule.DayOfWeekID.ToString());
                                XWriter.WriteElementString("AvailabilityStartTime", userFacilityTime.AvailabilityStartTime.ToString());
                                XWriter.WriteElementString("AvailabilityEndTime", userFacilityTime.AvailabilityEndTime.ToString());
                                XWriter.WriteEndElement();
                            }
                        }
                    }
                    XWriter.WriteEndElement();
                }

                return sWriter.ToString();
            }
        }

        #endregion


    }
}
