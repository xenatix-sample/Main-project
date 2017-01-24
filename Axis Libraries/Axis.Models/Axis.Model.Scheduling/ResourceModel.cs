﻿using Axis.Model.Common;
using System.Collections.Generic;
namespace Axis.Model.Scheduling
{
    /// <summary>
    ///
    /// </summary>
    public class ResourceModel : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceViewModel"/> class.
        /// </summary>
        public ResourceModel()
        {
            ResourceAvailabilities = new List<ResourceAvailabilityModel>();
            ResourceOverrides = new List<ResourceOverridesModel>();
        }

        /// <summary>
        /// Gets or sets the resource identifier.
        /// </summary>
        /// <value>
        /// The resource identifier.
        /// </value>
        public int? ResourceID { get; set; }

        /// <summary>
        /// Gets or sets the resource type identifier.
        /// </summary>
        /// <value>
        /// The resource type identifier.
        /// </value>
        public short? ResourceTypeID { get; set; }

        /// <summary>
        /// Gets or sets the name of the resource.
        /// </summary>
        /// <value>
        /// The name of the resource.
        /// </value>
        public string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the resource availability.
        /// </summary>
        /// <value>
        /// The resource availability.
        /// </value>
        public List<ResourceAvailabilityModel> ResourceAvailabilities { get; set; }

        /// <summary>
        /// Gets or sets the resource overrides.
        /// </summary>
        /// <value>
        /// The resource overrides.
        /// </value>
        public List<ResourceOverridesModel> ResourceOverrides { get; set; }


        /// <summary>
        /// Gets or sets the FacilityID identifier.
        /// </summary>
        /// <value>
        /// The FacilityID identifier.
        /// </value>
        public int FacilityID { get; set; }
    }
}