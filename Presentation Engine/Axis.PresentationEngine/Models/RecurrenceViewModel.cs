using Axis.PresentationEngine.Helpers.Model;
using System;

namespace Axis.PresentationEngine.Models
{
    /// <summary>
    ///
    /// </summary>
    public class RecurrenceViewModel : BaseViewModel
    {
        public long RecurrenceID { get; set; }
        public bool IsRecurring { get; set; }
        public int? Frequency { get; set; }
        
        public int? DailyFrequency { get; set; }
        public int? DailyDays { get; set; }
        public int? DayOfMonth { get; set; }

        public int? WeeklyRecurrence { get; set; }
        public bool? WeekDayMon { get; set; }
        public bool? WeekDayTue { get; set; }
        public bool? WeekDayWed { get; set; }
        public bool? WeekDayThur { get; set; }
        public bool? WeekDayFri { get; set; }
        public bool? WeekDaySat { get; set; }
        public bool? WeekDaySun { get; set; }

        public int? MonthlyFrequency { get; set; }
        public int? MonthlyDay { get; set; }
        public int? DayMonths { get; set; }
        public int? WeekOfMonthID { get; set; }
        public int? DayOfWeekID { get; set; }
        public int? TheMonths { get; set; }

        public int? YearlyRecurrence { get; set; }
        public int? YearlyFrequency { get; set; }
        public int? YearlyMonthID { get; set; }
        public int? YearlyMonthOnTheID { get; set; }
        
        public DateTime? StartDate { get; set; }
        public int? Ending { get; set; }
        public int? EndAfter { get; set; }
        public DateTime? EndDate { get; set; }

    }
}