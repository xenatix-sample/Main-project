using Axis.Model.Recurrence;
using Axis.PresentationEngine.Models;

namespace Axis.Plugins.Scheduling.Translator
{
    public static class RecurrenceTranslator
    {
        public static RecurrenceViewModel ToViewModel(this RecurrenceModel entity)
        {
            if (entity == null)
                return null;

            var model = new RecurrenceViewModel
            {
                RecurrenceID = entity.RecurrenceID,
                IsRecurring = entity.IsRecurring,
                Frequency = entity.Frequency,
                DailyFrequency = entity.DailyFrequency,
                DailyDays = entity.DailyDays,
                DayOfMonth = entity.DayOfMonth,
                WeeklyRecurrence = entity.WeeklyRecurrence,
                WeekDayMon = entity.WeekDayMon,
                WeekDayTue = entity.WeekDayTue,
                WeekDayWed = entity.WeekDayWed,
                WeekDayThur = entity.WeekDayThur,
                WeekDayFri = entity.WeekDayFri,
                WeekDaySat = entity.WeekDaySat,
                WeekDaySun = entity.WeekDaySun,
                MonthlyFrequency = entity.MonthlyFrequency,
                MonthlyDay = entity.MonthlyDay,
                DayMonths = entity.DayMonths,
                WeekOfMonthID = entity.WeekOfMonthID,
                DayOfWeekID = entity.DayOfWeekID,
                TheMonths = entity.TheMonths,
                YearlyRecurrence = entity.YearlyRecurrence,
                YearlyFrequency = entity.YearlyFrequency,
                YearlyMonthID = entity.YearlyMonthID,
                YearlyMonthOnTheID = entity.YearlyMonthOnTheID,

                StartDate = entity.StartDate,
                Ending = entity.Ending,
                EndAfter = entity.EndAfter,
                EndDate = entity.EndDate
            };

            return model;
        }

        public static RecurrenceModel ToModel(this RecurrenceViewModel model)
        {
            if (model == null)
                return null;
            var entity = new RecurrenceModel
            {
                RecurrenceID = model.RecurrenceID,
                IsRecurring = model.IsRecurring,
                Frequency = model.Frequency,
                DailyFrequency = model.DailyFrequency,
                DailyDays = model.DailyDays,
                DayOfMonth = model.DayOfMonth,
                WeeklyRecurrence = model.WeeklyRecurrence,
                WeekDayMon = model.WeekDayMon,
                WeekDayTue = model.WeekDayTue,
                WeekDayWed = model.WeekDayWed,
                WeekDayThur = model.WeekDayThur,
                WeekDayFri = model.WeekDayFri,
                WeekDaySat = model.WeekDaySat,
                WeekDaySun = model.WeekDaySun,
                MonthlyFrequency = model.MonthlyFrequency,
                MonthlyDay = model.MonthlyDay,
                DayMonths = model.DayMonths,
                WeekOfMonthID = model.WeekOfMonthID,
                DayOfWeekID = model.DayOfWeekID,
                TheMonths = model.TheMonths,
                YearlyRecurrence = model.YearlyRecurrence,
                YearlyFrequency = model.YearlyFrequency,
                YearlyMonthID = model.YearlyMonthID,
                YearlyMonthOnTheID = model.YearlyMonthOnTheID,

                StartDate = model.StartDate,
                Ending = model.Ending,
                EndAfter = model.EndAfter,
                EndDate = model.EndDate
            };
            return entity;
        }
    }
}