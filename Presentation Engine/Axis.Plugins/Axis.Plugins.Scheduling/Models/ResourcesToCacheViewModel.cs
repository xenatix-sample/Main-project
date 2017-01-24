using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.Scheduling.Models
{
    /// <summary>
    /// Resources to cache
    /// </summary>
    public class ResourcesToCacheViewModel
    {
        /// <summary>
        /// Get set Rooms
        /// </summary>
        public string Rooms { get; set; }

        /// <summary>
        /// Get set CredentialByAppointmentType
        /// </summary>
        public string CredentialByAppointmentType { get; set; }

        /// <summary>
        /// Get set ProviderByCredential
        /// </summary>
        public string ProviderByCredential { get; set; }

        /// <summary>
        /// Get set Resources type
        /// </summary>
        public string ResourcesType { get; set; }

        /// <summary>
        /// Get set ResourceDetails
        /// </summary>
        public string ResourceDetails { get; set; }

        /// <summary>
        /// Get set ResourceAvailability
        /// </summary>
        public string ResourceAvailability { get; set; }

        /// <summary>
        /// Get set ResourceOverrides
        /// </summary>
        public string ResourceOverrides { get; set; }

    }
}
