using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Recurrence;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Scheduling
{
    /// <summary>
    /// Contact address data provider
    /// </summary>
    public class RecurrenceDataProvider : IRecurrenceDataProvider
    {
        #region initializations

        private readonly IUnitOfWork _unitOfWork;

        public RecurrenceDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public RecurrenceModel GetRecurrence(long? recurrenceID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("RecurrenceID", recurrenceID) };

            var repository = _unitOfWork.GetRepository<RecurrenceModel>(SchemaName.Scheduling);
            var results = repository.ExecuteStoredProc("usp_GetRecurrence", spParameters);

            if (results.DataItems.Count > 0)
            {
                results.DataItems[0].IsRecurring = true;
            }

            return results.DataItems.Count > 0 ? results.DataItems[0] : null;
        }

        public long? AddRecurrence(RecurrenceModel recurrence)
        {
            var repository = _unitOfWork.GetRepository<RecurrenceModel>(SchemaName.Scheduling);

            var spParameters = new List<SqlParameter>()
            {
                new SqlParameter("Frequency", (object)recurrence.Frequency ?? DBNull.Value),
                new SqlParameter("DailyFrequency", (object)recurrence.DailyFrequency ?? DBNull.Value),
                new SqlParameter("DailyDays", (object)recurrence.DailyDays ?? DBNull.Value),
                new SqlParameter("DayOfMonth", (object)recurrence.DayOfMonth ?? DBNull.Value),
                new SqlParameter("WeeklyRecurrence", (object)recurrence.WeeklyRecurrence ?? DBNull.Value),
                new SqlParameter("WeekDayMon", (object)recurrence.WeekDayMon ?? DBNull.Value),
                new SqlParameter("WeekDayTue", (object)recurrence.WeekDayTue ?? DBNull.Value),
                new SqlParameter("WeekDayWed", (object)recurrence.WeekDayWed ?? DBNull.Value),
                new SqlParameter("WeekDayThur", (object)recurrence.WeekDayThur ?? DBNull.Value),
                new SqlParameter("WeekDayFri", (object)recurrence.WeekDayFri ?? DBNull.Value),
                new SqlParameter("WeekDaySat", (object)recurrence.WeekDaySat ?? DBNull.Value),
                new SqlParameter("WeekDaySun", (object)recurrence.WeekDaySun ?? DBNull.Value),
                new SqlParameter("MonthlyFrequency", (object)recurrence.MonthlyFrequency ?? DBNull.Value),
                new SqlParameter("MonthlyDay", (object)recurrence.MonthlyDay ?? DBNull.Value),
                new SqlParameter("DayMonths", (object)recurrence.DayMonths ?? DBNull.Value),
                new SqlParameter("WeekOfMonthID", (object)recurrence.WeekOfMonthID ?? DBNull.Value),
                new SqlParameter("DayOfWeekID", (object)recurrence.DayOfWeekID ?? DBNull.Value),
                new SqlParameter("TheMonths", (object)recurrence.TheMonths ?? DBNull.Value),

                new SqlParameter("YearlyRecurrence", (object)recurrence.YearlyRecurrence ?? DBNull.Value),
                new SqlParameter("YearlyFrequency", (object)recurrence.YearlyFrequency ?? DBNull.Value),
                new SqlParameter("YearlyMonthID", (object)recurrence.YearlyMonthID ?? DBNull.Value),
                new SqlParameter("YearlyMonthOnTheID", (object)recurrence.YearlyMonthOnTheID ?? DBNull.Value),
                
                new SqlParameter("StartDate", (object)recurrence.StartDate ?? DBNull.Value),
                new SqlParameter("Ending", (object)recurrence.Ending ?? DBNull.Value),
                new SqlParameter("EndAfter", (object)recurrence.EndAfter ?? DBNull.Value),
                new SqlParameter("EndDate", (object)recurrence.EndDate ?? DBNull.Value),
                new SqlParameter("ModifiedOn", (object)recurrence.ModifiedOn ?? DateTime.Now)
            };

            return repository.ExecuteNQStoredProc("usp_AddRecurrence", spParameters, idResult: true).ID;
        }

        public void UpdateRecurrence(RecurrenceModel recurrence)
        {
            var results = new Response<RecurrenceModel>();
            results.ResultCode = 0;

            var repository = _unitOfWork.GetRepository<RecurrenceModel>(SchemaName.Scheduling);

            var spParameters = new List<SqlParameter>()
            {
                new SqlParameter("RecurrenceID", recurrence.RecurrenceID),
                new SqlParameter("Frequency", (object)recurrence.Frequency ?? DBNull.Value),
                new SqlParameter("DailyFrequency", (object)recurrence.DailyFrequency ?? DBNull.Value),
                new SqlParameter("DailyDays", (object)recurrence.DailyDays ?? DBNull.Value),
                new SqlParameter("DayOfMonth", (object)recurrence.DayOfMonth ?? DBNull.Value),
                new SqlParameter("WeeklyRecurrence", (object)recurrence.WeeklyRecurrence ?? DBNull.Value),
                new SqlParameter("WeekDayMon", (object)recurrence.WeekDayMon ?? DBNull.Value),
                new SqlParameter("WeekDayTue", (object)recurrence.WeekDayTue ?? DBNull.Value),
                new SqlParameter("WeekDayWed", (object)recurrence.WeekDayWed ?? DBNull.Value),
                new SqlParameter("WeekDayThur", (object)recurrence.WeekDayThur ?? DBNull.Value),
                new SqlParameter("WeekDayFri", (object)recurrence.WeekDayFri ?? DBNull.Value),
                new SqlParameter("WeekDaySat", (object)recurrence.WeekDaySat ?? DBNull.Value),
                new SqlParameter("WeekDaySun", (object)recurrence.WeekDaySun ?? DBNull.Value),
                new SqlParameter("MonthlyFrequency", (object)recurrence.MonthlyFrequency ?? DBNull.Value),
                new SqlParameter("MonthlyDay", (object)recurrence.MonthlyDay ?? DBNull.Value),
                new SqlParameter("DayMonths", (object)recurrence.DayMonths ?? DBNull.Value),
                new SqlParameter("WeekOfMonthID", (object)recurrence.WeekOfMonthID ?? DBNull.Value),
                new SqlParameter("DayOfWeekID", (object)recurrence.DayOfWeekID ?? DBNull.Value),
                new SqlParameter("TheMonths", (object)recurrence.TheMonths ?? DBNull.Value),

                new SqlParameter("YearlyRecurrence", (object)recurrence.YearlyRecurrence ?? DBNull.Value),
                new SqlParameter("YearlyFrequency", (object)recurrence.YearlyFrequency ?? DBNull.Value),
                new SqlParameter("YearlyMonthID", (object)recurrence.YearlyMonthID ?? DBNull.Value),
                new SqlParameter("YearlyMonthOnTheID", (object)recurrence.YearlyMonthOnTheID ?? DBNull.Value),
                
                new SqlParameter("StartDate", (object)recurrence.StartDate ?? DBNull.Value),
                new SqlParameter("Ending", (object)recurrence.Ending ?? DBNull.Value),
                new SqlParameter("EndAfter", (object)recurrence.EndAfter ?? DBNull.Value),
                new SqlParameter("EndDate", (object)recurrence.EndDate ?? DBNull.Value),
                new SqlParameter("IsActive", recurrence.IsRecurring),
                new SqlParameter("ModifiedOn", (object)recurrence.ModifiedOn ?? DateTime.Now)
            };

            repository.ExecuteNQStoredProc("usp_UpdateRecurrence", spParameters);
        }

        public void DeleteRecurrence(long appointmentID, DateTime modifiedOn)
        {
            var spParameters = new List<SqlParameter>
            {
                new SqlParameter("RecurrenceID", appointmentID), 
                new SqlParameter("ModifiedOn", modifiedOn)
            };

            var repository = _unitOfWork.GetRepository<RecurrenceModel>(SchemaName.Scheduling);
            repository.ExecuteNQStoredProc("usp_DeleteRecurrence", spParameters);

        }

        #endregion exposed functionality
    }
}
