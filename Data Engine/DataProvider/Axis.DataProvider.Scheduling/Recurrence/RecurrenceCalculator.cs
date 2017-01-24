using Axis.Model.Scheduling;
using System;
using System.Collections.Generic;

namespace Axis.DataProvider.Scheduling
{
    public class RecurrenceCalculator
    {
        private const int RECURRENCE_FREQ_DAY = 1;
        private const int RECURRENCE_FREQ_WEEK = 2;
        private const int RECURRENCE_FREQ_MONTH = 3;
        private const int RECURRENCE_FREQ_YEAR = 3;

        private const int YEARLY_FREQ_BY_DATE = 1;
        private const int YEARLY_FREQ_BY_DAY_WEEK = 2;

        private const int MONTHLY_FREQ_DAY = 1;
        private const int MONTHLY_FREQ_DAY_WEEK = 2;

        private const int EVERY_DAY = 1;
        private const int EVERY_WEEKDAY = 2;

        private const int ENDING_NONE = 1;
        private const int ENDING_AFTER = 2;
        private const int ENDING_BY_DATE = 3;

        private const int DEFAULT_ONE_YEAR_DAYS = 365;
        private const int DEFAULT_FIVE_YEARS_DAYS = 1825;
        private const int DEFAULT_INFINITY_DAYS = 999999;

        private const int WEEK_OF_MONTH_LAST = 5;


        private AppointmentModel appointment;

        public RecurrenceCalculator(AppointmentModel appt)
        {
            this.appointment = appt;
        }

        public List<AppointmentModel> GetAppts()
        {
            switch (appointment.Recurrence.Frequency)
            {

                case RECURRENCE_FREQ_DAY: return GetApptsFromDayRecurrence();
                case RECURRENCE_FREQ_WEEK: return GetApptsFromWeekRecurrence();
                case RECURRENCE_FREQ_MONTH: return GetApptsFromMonthRecurrence();
                default: return GetApptsFromYearRecurrence();   // yearly
            }
        }

        #region Get appointments switches

        public List<AppointmentModel> GetApptsFromYearRecurrence()
        {
            // 1. Recurrence.YearlyRecurrence:  ex: 2 years
            // 2. Recurrence.YearlyFrequency:   1 = 'On' a particular month
            // 3. Recurrence.YearlyMonth:       Month lookup, ex: January, March, etc.
            // 4. Recurrence.DayOfMonth:        Particular day of the month, integer, ex: 3rd, 4th
            // Ex: Every 2 years, On Feruary 8th --> 

            // 1. Recurrence.YearlyRecurrence:  ex: 3 years
            // 2. Recurrence.YearlyFrequency:   2 = 'On the' a week of the month/day of the week combo
            // 3. Recurrence.WeekOfMonthID:     Week of the month: 'First', 'Second'....
            // 4. Recurrence.DayOfWeekID:       Day of the week: 'Friday', 'Saturday'...
            // 5. Recurrence.MonthID:           The particular month
            // Ex: Every 2 years, On third Sunday of March --> 

            if (this.appointment.Recurrence.YearlyFrequency == YEARLY_FREQ_BY_DATE)
            {
                return CreateApptsForYearsByDate();
            }
            else if (appointment.Recurrence.YearlyFrequency == YEARLY_FREQ_BY_DAY_WEEK)
            {
                return CreateApptsForYearsByDayOfWeek();
            }
            return new List<AppointmentModel>();
        }

        public List<AppointmentModel> GetApptsFromMonthRecurrence()
        {
            if (this.appointment.Recurrence.MonthlyFrequency == MONTHLY_FREQ_DAY)
            {
                return CreateApptsForMonthsByDay();
            }
            else if (appointment.Recurrence.MonthlyFrequency == MONTHLY_FREQ_DAY_WEEK)
            {
                return CreateApptsForMonthsByDayAndWeek();
            }
            return new List<AppointmentModel>();
        }

        public List<AppointmentModel> GetApptsFromDayRecurrence()
        {
            //var frequency = recurrence.Frequency;           // 1= Daily
            //var dailyfrequency = recurrence.DailyFrequency; // 1= every day, 2 = every weekday
            //var dailydays = recurrence.DailyDays;           // Number = how many days???

            //var startdate = recurrence.StartDate;           // Appt start date
            //var ending = recurrence.Ending;                 // 1= no end date, 2= there's an end, use end after, 3= there's an end date, use end date
            //var enddate = recurrence.EndDate;               // End date of appts
            //var endafter = recurrence.EndAfter;             // Number of appts to create up to

            if (this.appointment.Recurrence.DailyFrequency == EVERY_DAY)
            {
                return CreateApptsForDays();
            }
            else if (appointment.Recurrence.DailyFrequency == EVERY_WEEKDAY)
            {
                return CreateApptsForWeekDays();
            }
            return new List<AppointmentModel>();
        }

        public List<AppointmentModel> GetApptsFromWeekRecurrence()
        {
            return CreateApptsForWeeks();
        }

        #endregion


        #region Create appointments based on end criteria

        private List<AppointmentModel> CreateApptsForYearsByDayOfWeek()
        {
            switch (appointment.Recurrence.Ending)
            {
                case ENDING_NONE:
                    return GetApptsForYearDayOfWeek(DEFAULT_FIVE_YEARS_DAYS, DateTime.MinValue);
                case ENDING_AFTER:
                    return GetApptsForYearDayOfWeek(DEFAULT_INFINITY_DAYS, DateTime.MinValue, (int)appointment.Recurrence.EndAfter);
                default:
                    // Ending by date
                    return GetApptsForYearDayOfWeek((DateTime)appointment.Recurrence.EndDate);
            }
        }

        private List<AppointmentModel> CreateApptsForYearsByDate()
        {
            switch (appointment.Recurrence.Ending)
            {
                case ENDING_NONE:
                    return GetApptsForYearDate(DEFAULT_FIVE_YEARS_DAYS);
                case ENDING_AFTER:
                    return GetApptsForYearDate(DEFAULT_INFINITY_DAYS, (int)appointment.Recurrence.EndAfter);
                default:
                    // Ending by date
                    return GetApptsForYearDate((DateTime)appointment.Recurrence.EndDate);
            }
        }

        private List<AppointmentModel> CreateApptsForMonthsByDayAndWeek()
        {
            switch (appointment.Recurrence.Ending)
            {
                case ENDING_NONE:
                    return GetApptsForMonthsDayWeek(DEFAULT_ONE_YEAR_DAYS, DateTime.MinValue);
                case ENDING_AFTER:
                    return GetApptsForMonthsDayWeek(DEFAULT_INFINITY_DAYS, DateTime.MinValue, (int)appointment.Recurrence.EndAfter);
                default:
                    // Ending by date
                    return GetApptsForMonthsDayWeek((DateTime)appointment.Recurrence.EndDate);
            }
        }

        private List<AppointmentModel> CreateApptsForMonthsByDay()
        {
            switch (appointment.Recurrence.Ending)
            {
                case ENDING_NONE:
                    return GetApptsForMonthsDay(DEFAULT_ONE_YEAR_DAYS);
                case ENDING_AFTER:
                    return GetApptsForMonthsDay(DEFAULT_INFINITY_DAYS, (int)appointment.Recurrence.EndAfter);
                default:
                    // Ending by date
                    return GetApptsForMonthsDay((DateTime)appointment.Recurrence.EndDate);
            }
        }

        private List<AppointmentModel> CreateApptsForWeeks()
        {
            switch (appointment.Recurrence.Ending)
            {
                case ENDING_NONE:
                    return GetApptsForWeeks(DEFAULT_ONE_YEAR_DAYS);
                case ENDING_AFTER:
                    return GetApptsForWeeks(DEFAULT_INFINITY_DAYS, (int)appointment.Recurrence.EndAfter);
                default:
                    // Ending by date
                    return GetApptsForWeeks((DateTime)appointment.Recurrence.EndDate);
            }
        }

        private List<AppointmentModel> CreateApptsForWeekDays()
        {
            switch (appointment.Recurrence.Ending)
            {
                case ENDING_NONE:
            return GetApptsForDays(DEFAULT_ONE_YEAR_DAYS, true);
                case ENDING_AFTER:
                    return GetApptsForDays(DEFAULT_INFINITY_DAYS, true, (int)appointment.Recurrence.EndAfter);
                default:
                    return GetApptsForDays((DateTime)appointment.Recurrence.EndDate, true);
            }

            
        }

        private List<AppointmentModel> CreateApptsForDays()
        {
            switch (appointment.Recurrence.Ending)
            {
                case ENDING_NONE:
                    return GetApptsForDays(DEFAULT_ONE_YEAR_DAYS);
                case ENDING_AFTER:
                    return GetApptsForDays(DEFAULT_INFINITY_DAYS, false, (int)appointment.Recurrence.EndAfter);
                default:
                    // Ending by date
                    return GetApptsForDays((DateTime)appointment.Recurrence.EndDate);
            }
        }

        #endregion

        #region Get appointments specific methods

        private List<AppointmentModel> GetApptsForYearDayOfWeek(DateTime enddate)
        {
            TimeSpan diff = enddate - (DateTime)appointment.Recurrence.StartDate;
            return GetApptsForYearDayOfWeek(diff.Days, enddate);
        }

        private List<AppointmentModel> GetApptsForYearDayOfWeek(int numdays, DateTime enddate, int numoccurrences = 0)
        {
            //var month = 1;
            //var week = 1;
            //List<AppointmentModel> appts = new List<AppointmentModel>();
            //for (int i = 0; i < numdays; i++)
            //{
            //    DateTime date = ((DateTime)appointment.Recurrence.StartDate).AddDays(i);    // Ex: Every 2 years, On third Sunday of March
            //    week = (int)Math.Ceiling((double)date.Day / 7.0);
            //    if (date.Day == 1 && i > 0) month++;
            //    if (IsYearIncluded(date.Year) && IsMonthIncludedInYear(month) && IsWeekIncludedInMonth(week))
            //        appts.Add(GetAppointmentForDay(date, appointment.AppointmentStartTime, appointment.AppointmentLength));
            //    if (numoccurrences > 0 && appts.Count == numoccurrences)
            //        return appts;
            //}
            //return appts;


            // Go through each year
            List<AppointmentModel> appts = new List<AppointmentModel>();
            var years = (int)Math.Ceiling((double)numdays / (double)365);
            var yearct = 1;
            for (int j = 1; j <= years; j++)
            {
                if (IsYearIncluded(yearct))
                {
                    var dt = appointment.Recurrence.StartDate.Value.AddYears(j - 1);
                    var appt = ((int)appointment.Recurrence.WeekOfMonthID == WEEK_OF_MONTH_LAST) ?
                        GetAppointmentForDayAndLastWeek(dt.Year, (int)appointment.Recurrence.YearlyMonthOnTheID) :
                        GetAppointmentForDayAndWeek(dt.Year, (int)appointment.Recurrence.YearlyMonthOnTheID, (int)appointment.Recurrence.WeekOfMonthID);

                    // Add this appt as long as it's on or after the start date
                    if (appt != null && appt.AppointmentDate >= appointment.Recurrence.StartDate.Value)
                    {
                        // Make sure this appt date doesn't fall past the specified end date, if any
                        if (enddate == DateTime.MinValue || (enddate != DateTime.MinValue && appt.AppointmentDate <= enddate))
                            appts.Add(appt);
                    }
                    if (numoccurrences > 0 && appts.Count == numoccurrences)
                        return appts;
                }
                if (appts.Count != 0) yearct++;
            }
            return appts;
        }

        private List<AppointmentModel> GetApptsForYearDate(DateTime enddate)
        {
            TimeSpan diff = enddate - (DateTime)appointment.Recurrence.StartDate;
            return GetApptsForYearDate(diff.Days);
        }

        private List<AppointmentModel> GetApptsForYearDate(int numdays, int numoccurrences = 0)
        {
            var month = 1;
            List<AppointmentModel> appts = new List<AppointmentModel>();
            var year = appointment.Recurrence.StartDate.Value.Year;
            var yearct = 1;
            for (int i = 0; i < numdays; i++)
            {
                DateTime date = ((DateTime)appointment.Recurrence.StartDate).AddDays(i);    // Ex: Every 2 years, On February 8th
                if (date.Day == 1 && i > 0) month++;
                if (date.Year > year)
                {
                    yearct++;
                    year = date.Year;
                }
                if (IsYearIncluded(yearct) && IsMonthIncludedInYear(date.Month) && date.Day == appointment.Recurrence.DayOfMonth)
                    appts.Add(GetAppointmentForDay(date, appointment.AppointmentStartTime, appointment.AppointmentLength));
                if (numoccurrences > 0 && appts.Count == numoccurrences)
                    return appts;
            }
            return appts;
        }

        private List<AppointmentModel> GetApptsForMonthsDay(DateTime enddate)
        {
            TimeSpan diff = enddate - (DateTime)appointment.Recurrence.StartDate;
            return GetApptsForMonthsDay(diff.Days);
        }

        private List<AppointmentModel> GetApptsForMonthsDay(int numdays, int numoccurrences = 0)
        {
            var month = 1;
            List<AppointmentModel> appts = new List<AppointmentModel>();
            for (int i = 0; i < numdays; i++)
            {
                DateTime date = ((DateTime)appointment.Recurrence.StartDate).AddDays(i);    // ex.: Day 4 of every 2nd month
                if (date.Day == 1 && i > 0 && appts.Count > 0) month++;
                if (IsMonthIncluded(month) && date.Day == appointment.Recurrence.MonthlyDay)
                    appts.Add(GetAppointmentForDay(date, appointment.AppointmentStartTime, appointment.AppointmentLength));
                if (numoccurrences > 0 && appts.Count == numoccurrences)
                    return appts;
            }
            return appts;
        }

        private List<AppointmentModel> GetApptsForMonthsDayWeek(DateTime enddate)
        {
            TimeSpan diff = enddate - (DateTime)appointment.Recurrence.StartDate;
            return GetApptsForMonthsDayWeek(diff.Days, enddate);
        }

        private List<AppointmentModel> GetApptsForMonthsDayWeek(int numdays, DateTime enddate, int numoccurrences = 0)
        {
            //var week = 1;
            //var month = 1;
            //var dayoffset = GetDayOffset((DateTime)appointment.Recurrence.StartDate);
            //List<AppointmentModel> appts = new List<AppointmentModel>();
            //for (int i = 0; i < numdays; i++)                                               
            //{
            //    DateTime date = ((DateTime)appointment.Recurrence.StartDate).AddDays(i);    // ex.: On the Third Tuesday of every 2nd month
            //    if (date.Day == 1) dayoffset = GetDayOffset(date.DayOfWeek);
            //    week = (int)Math.Ceiling((double)(date.Day + dayoffset) / 7.0);
            //    if (date.Day == 1 && i > 0 && appts.Count > 0) month++;
            //    if (IsMonthIncluded(month) && IsWeekIncludedInMonth(week) && IsDayIncludedInMonth(date.DayOfWeek))
            //        appts.Add(GetAppointmentForDay(date, appointment.AppointmentStartTime, appointment.AppointmentLength));
            //    if (numoccurrences > 0 && appts.Count == numoccurrences)
            //        return appts;
            //}
            //return appts;

            // Go through each month
            List<AppointmentModel> appts = new List<AppointmentModel>();
            var months = (int)Math.Ceiling((double)numdays / (double)30);
            var monthct = 1;
            for (int j = 1; j <= months; j++)
            {
                if (IsMonthIncluded(monthct))
                {
                    var dt = appointment.Recurrence.StartDate.Value.AddMonths(j - 1);
                    var appt = ((int)appointment.Recurrence.WeekOfMonthID == WEEK_OF_MONTH_LAST) ?
                        GetAppointmentForDayAndLastWeek(dt.Year, dt.Month) :
                        GetAppointmentForDayAndWeek(dt.Year, dt.Month, (int)appointment.Recurrence.WeekOfMonthID);

                    // Add this appt as long as it's on or after the start date
                    if (appt != null && appt.AppointmentDate >= appointment.Recurrence.StartDate.Value)
                    {
                        // Make sure this appt date doesn't fall past the specified end date, if any
                        if (enddate == DateTime.MinValue || (enddate != DateTime.MinValue && appt.AppointmentDate <= enddate))
                            appts.Add(appt);
                    }
                    if (numoccurrences > 0 && appts.Count == numoccurrences)
                        return appts;
                }
                if (appts.Count != 0) monthct++;
            }
            return appts;
        }

        private AppointmentModel GetAppointmentForDayAndLastWeek(int year, int month)
        {
            var numdays = DateTime.DaysInMonth(year, month);
            for (int k = numdays; k >= 1; k--)
            {
                var date = new DateTime(year, month, k);
                if (IsDayIncludedInMonth(date.DayOfWeek))
                    return GetAppointmentForDay(date, appointment.AppointmentStartTime, appointment.AppointmentLength);
            }
            return null;
        }

        private AppointmentModel GetAppointmentForDayAndWeek(int year, int month, int numoccur)
        {
            var numdays = DateTime.DaysInMonth(year, month);
            var occurencecount = 0;
            for (int k = 1; k <= numdays; k++)
            {
                var date = new DateTime(year, month, k);
                if (IsDayIncludedInMonth(date.DayOfWeek))
                    occurencecount++;
                if (occurencecount == numoccur)
                    return GetAppointmentForDay(date, appointment.AppointmentStartTime, appointment.AppointmentLength);
            }
            return null;
        }

        private int GetDayOffset(DateTime startdate)
        {
            DateTime dt = new DateTime(startdate.Year, startdate.Month, 1);
            return (int)(dt.DayOfWeek - DayOfWeek.Sunday);
        }

        private int GetDayOffset(DayOfWeek dayOfWeek)
        {
            return (int)(dayOfWeek - DayOfWeek.Sunday);
        }

        private List<AppointmentModel> GetApptsForWeeks(DateTime enddate)
        {
            TimeSpan diff = enddate - (DateTime)appointment.Recurrence.StartDate;
            return GetApptsForWeeks(diff.Days);
        }

        private List<AppointmentModel> GetApptsForWeeks(int numdays, int numoccurrences = 0)
        {
            var week = 1;
            List<AppointmentModel> appts = new List<AppointmentModel>();
            for (int i = 0; i < numdays; i++)
            {
                DateTime date = ((DateTime)appointment.Recurrence.StartDate).AddDays(i);
                if (date.DayOfWeek == DayOfWeek.Sunday && i > 0 && appts.Count > 0) week++;
                if (IsWeekIncluded(week) && IsDayIncludedInWeek(date.DayOfWeek))
                    appts.Add(GetAppointmentForDay(date, appointment.AppointmentStartTime, appointment.AppointmentLength));
                if (numoccurrences > 0 && appts.Count == numoccurrences)
                    return appts;
            }
            return appts;
        }

        private List<AppointmentModel> GetApptsForDays(DateTime enddate, bool skipnonweekday = false)
        {
            TimeSpan diff = enddate - (DateTime)appointment.Recurrence.StartDate;
            return GetApptsForDays(diff.Days, skipnonweekday);
        }

        private List<AppointmentModel> GetApptsForDays(int numdays, bool skipnonweekday = false, int numoccurrences = 0)
        {
            List<AppointmentModel> appts = new List<AppointmentModel>();
            for (int i = 0; i <= numdays; i++)
            {
                DateTime date = ((DateTime)appointment.Recurrence.StartDate).AddDays(i);
                if (IsDayIncluded(i + 1) && (!skipnonweekday || (skipnonweekday && IsWeekDay(date))) && (appointment.Recurrence.EndDate == null || date <= appointment.Recurrence.EndDate))
                    appts.Add(GetAppointmentForDay(date, appointment.AppointmentStartTime, appointment.AppointmentLength));
                if (numoccurrences > 0 && appts.Count == numoccurrences)
                    return appts;
            }
            return appts;
        }

        private AppointmentModel GetAppointmentForDay(DateTime date, short starttime, short length)
        {
            AppointmentModel ret = new AppointmentModel();
            DateTime dt = new DateTime(date.Year, date.Month, date.Day);

            ret.AppointmentID = appointment.AppointmentID;
            ret.AppointmentDate = dt.AddHours((double)starttime / 100.0);
            ret.AppointmentStartTime = starttime;
            ret.AppointmentLength = length;
            ret.AppointmentTypeID = appointment.AppointmentTypeID;
            ret.FacilityID = appointment.FacilityID;
            ret.ContactID = appointment.ContactID;
            ret.ProgramID = appointment.ProgramID;
            ret.ReasonForVisit = appointment.ReasonForVisit;
            ret.RecurrenceID = appointment.RecurrenceID;
            ret.ReferredBy = appointment.ReferredBy;
            ret.ServicesID = appointment.ServicesID;
            ret.SupervisionVisit = appointment.SupervisionVisit;
            ret.IsCancelled = appointment.IsCancelled;
            ret.CancelReasonID = appointment.CancelReasonID;
            ret.CancelComment = appointment.CancelComment;
            ret.Comments = appointment.Comments;

            return ret;
        }

        #endregion

        #region Helper methods

        private bool IsYearIncluded(int year)
        {
            if (year == 1 || appointment.Recurrence.YearlyRecurrence == 1)
                return true;
            return ((year - 1) % appointment.Recurrence.YearlyRecurrence == 0);
        }

        private bool IsDayIncluded(int day)
        {
            if (day == 1 || appointment.Recurrence.DailyDays == 1 || appointment.Recurrence.DailyDays == null)
                return true;
            return ((day - 1) % appointment.Recurrence.DailyDays == 0);
        }

        private bool IsMonthIncluded(int month)
        {
            if (month == 1 || appointment.Recurrence.DayMonths == 1 || appointment.Recurrence.TheMonths == 1)
                return true;
            if (appointment.Recurrence.DayMonths != null)
                return ((month - 1)%appointment.Recurrence.DayMonths == 0);
            else
                return ((month - 1) % appointment.Recurrence.TheMonths == 0);
        }

        private bool IsWeekIncluded(int week)
        {
            if (week == 1 || appointment.Recurrence.WeeklyRecurrence == 1)
                return true;
            return ((week - 1) % appointment.Recurrence.WeeklyRecurrence == 0);
        }

        private bool IsMonthIncludedInYear(int month)
        {
            // TODO: Conversion needed here for 'YearlyMonth'??
            int monthofyear = (int)appointment.Recurrence.YearlyMonthID;
            return month == monthofyear;
        }

        private bool IsWeekIncludedInMonth(int week)
        {
            int weekofmonth = GetWeekOfMonthByID(appointment.Recurrence.WeekOfMonthID);
            return week == weekofmonth;
        }

        private bool IsDayIncludedInMonth(DayOfWeek dayOfWeek)
        {
            var day = GetDayOfWeekByID(appointment.Recurrence.DayOfWeekID);
            return day == dayOfWeek;
        }

        private bool IsDayIncludedInWeek(DayOfWeek dayOfWeek)
        {
            if (dayOfWeek == DayOfWeek.Monday && appointment.Recurrence.WeekDayMon.HasValue && (bool)appointment.Recurrence.WeekDayMon) return true;
            else if (dayOfWeek == DayOfWeek.Tuesday && appointment.Recurrence.WeekDayTue.HasValue && (bool)appointment.Recurrence.WeekDayTue) return true;
            else if (dayOfWeek == DayOfWeek.Wednesday && appointment.Recurrence.WeekDayWed.HasValue && (bool)appointment.Recurrence.WeekDayWed) return true;
            else if (dayOfWeek == DayOfWeek.Thursday && appointment.Recurrence.WeekDayThur.HasValue && (bool)appointment.Recurrence.WeekDayThur) return true;
            else if (dayOfWeek == DayOfWeek.Friday && appointment.Recurrence.WeekDayFri.HasValue && (bool)appointment.Recurrence.WeekDayFri) return true;
            else if (dayOfWeek == DayOfWeek.Saturday && appointment.Recurrence.WeekDaySat.HasValue && (bool)appointment.Recurrence.WeekDaySat) return true;
            else if (dayOfWeek == DayOfWeek.Sunday && appointment.Recurrence.WeekDaySun.HasValue && (bool)appointment.Recurrence.WeekDaySun) return true;
            return false;
        }

        private bool IsWeekDay(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    return true;
                default:
                    return false;
            }
        }

        private DayOfWeek GetDayOfWeekByID(int? dayofweek)
        {
            // TODO: Replace this w/the conversion from 'DayOfWeek' lookup
            if (dayofweek == 1) return DayOfWeek.Monday;
            else if (dayofweek == 2) return DayOfWeek.Tuesday;
            else if (dayofweek == 3) return DayOfWeek.Wednesday;
            else if (dayofweek == 4) return DayOfWeek.Thursday;
            else if (dayofweek == 5) return DayOfWeek.Friday;
            else if (dayofweek == 6) return DayOfWeek.Saturday;
            else return DayOfWeek.Sunday;   //(dayofweek == 7) or something else????
        }

        private int GetWeekOfMonthByID(int? weekofmonthid)
        {
            // TODO: Replace this w/the conversion from 'WeekOfMonth' lookup and filter 
            // Map 'First', 'Second', 'Third', 'Fourth' and 'Last' by id
            return (int)weekofmonthid;
        }

        #endregion
    }
}