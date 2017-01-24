using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.Scheduling.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AppointmentDetailViewModel : AppointmentViewModel
    {
        /// <summary>
        /// Gets or sets the type of the appointment.
        /// </summary>
        /// <value>
        /// The type of the appointment.
        /// </value>
        public new string AppointmentType { get; set; }
        /// <summary>
        /// Gets or sets the provider identifier.
        /// </summary>
        /// <value>
        /// The provider identifier.
        /// </value>
        public int ProviderId { get; set; }
        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        /// <value>
        /// The name of the provider.
        /// </value>
        public string ProviderName { get; set; }
        /// <summary>
        /// Gets or sets the location identifier.
        /// </summary>
        /// <value>
        /// The location identifier.
        /// </value>
        public int RoomId { get; set; }
        /// <summary>
        /// Gets or sets the name of the room.
        /// </summary>
        /// <value>
        /// The name of the room.
        /// </value>
        public string RoomName { get; set; }

        /// <summary>
        /// Gets or sets the location identifier.
        /// </summary>
        /// <value>
        /// The location identifier.
        /// </value>
        public int LocationID { get; set; }
        /// <summary>
        /// Gets or sets the name of the location.
        /// </summary>
        /// <value>
        /// The name of the location.
        /// </value>
        public string LocationName { get; set; }
    }
}
