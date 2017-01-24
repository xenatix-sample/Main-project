using Axis.PresentationEngine.Helpers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Axis.Model.Scheduling;
using Axis.Plugins.Scheduling.Translator;

namespace Axis.Plugins.Scheduling.Models
{
    public class CalendarViewModel : AppointmentViewModel
    {
        /// <summary>
        /// Gets or sets the appointment contacts
        /// </summary>
        /// <value>
        /// The appointment contacts
        /// </value>
        public List<AppointmentResourceViewModel> Contacts { get; set; }

        /// <summary>
        /// Gets or sets the appointment resources
        /// </summary>
        /// <value>
        /// The appointment resources.
        /// </value>
        public List<AppointmentResourceViewModel> Resources { get; set; }

        public static CalendarViewModel ToViewModel(CalendarModel model)
        {
            return new CalendarViewModel
            {
                AppointmentID = model.AppointmentID,
                AppointmentDate = model.AppointmentDate,
                AppointmentLength = model.AppointmentLength,
                AppointmentStartTime = model.AppointmentStartTime,
                AppointmentTypeID = model.AppointmentTypeID,
                FacilityID = model.FacilityID,
                ProgramID = model.ProgramID,
                ReasonForVisit = model.ReasonForVisit,
                ReferredBy = model.ReferredBy,
                ServicesID = model.ServicesID,
                SupervisionVisit = model.SupervisionVisit,
                IsCancelled = model.IsCancelled,
                IsActive = model.IsActive,
                ModifiedBy = model.ModifiedBy,
                ModifiedOn = model.ModifiedOn,
                ForceRollback = model.ForceRollback,
                Contacts = model.Contacts.Select(x => x.ToViewModel()).ToList(),
                Resources = model.Resources.Select(x => x.ToViewModel()).ToList(),
                IsGroupAppointment = model.IsGroupAppointment,
                GroupName = model.GroupName,
                GroupType = model.GroupType,
                AppointmentType = model.AppointmentType,
                ServiceName = model.ServiceName,
                Comments = model.Comments,
                ProgramName = model.ProgramName,
                FacilityName = model.FacilityName,
                GroupID = model.GroupID,
                GroupDetailID = model.GroupDetailID,
                GroupComments = model.GroupComments
            };
        }
    }
}
