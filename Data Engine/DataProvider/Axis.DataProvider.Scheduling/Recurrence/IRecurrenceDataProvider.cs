using System;
using Axis.Model.Recurrence;

namespace Axis.DataProvider.Scheduling
{
    public interface IRecurrenceDataProvider
    {
        long? AddRecurrence(RecurrenceModel recurrence);
        void UpdateRecurrence(RecurrenceModel recurrence);
        RecurrenceModel GetRecurrence(long? recurrenceID);
        void DeleteRecurrence(long appointmentID, DateTime modifiedOn);
    }
}
