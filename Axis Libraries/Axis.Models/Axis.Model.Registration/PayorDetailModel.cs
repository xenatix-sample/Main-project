﻿using Axis.Model.Address;
using Axis.Model.Registration;
using System;

namespace Axis.Model.Common
{
    /// <summary>
    /// Represents payor details
    /// </summary>
    public class PayorDetailModel : AddressModel
    {
        /// <summary>
        /// Gets or sets the payor identifier.
        /// </summary>
        /// <value>The payor identifier.</value>
        public int? PayorID { get; set; }


        /// <summary>
        /// Gets or sets the name of the payor.
        /// </summary>
        /// <value>The name of the payor.</value>
        public string PayorName { get; set; }

        /// <summary>
        /// Gets or sets the payor code.
        /// </summary>
        /// <value>The payor code.</value>
        public int? PayorCode { get; set; }

        /// <summary>
        /// Gets or sets the plan identifier.
        /// </summary>
        /// <value>
        /// The plan identifier.
        /// </value>
        public string PlanID { get; set; }

        /// <summary>
        /// Gets or sets the payor plan identifier.
        /// </summary>
        /// <value>
        /// The payor plan identifier.
        /// </value>
        public int? PayorPlanID { get; set; }



        /// <summary>
        /// Gets or sets the group identifier.
        /// </summary>
        /// <value>The group identifier.</value>
        public string GroupID { get; set; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>The name of the group.</value>
        public string GroupName { get; set; }


        /// <summary>
        /// Gets or sets the payor group identifier.
        /// </summary>
        /// <value>
        /// The payor group identifier.
        /// </value>
        public int? PayorGroupID { get; set; }

        /// <summary>
        /// Gets or sets the electronic payor identifier.
        /// </summary>
        /// <value>
        /// The electronic payor identifier.
        /// </value>
        public string ElectronicPayorID { get; set; }

        /// <summary>
        /// Gets or sets the payor address identifier.
        /// </summary>
        /// <value>
        /// The payor address identifier.
        /// </value>
        public long? PayorAddressID { get; set; }

        /// <summary>
        /// Gets or sets the name of the plan.
        /// </summary>
        /// <value>
        /// The name of the plan.
        /// </value>
        public string PlanName { get; set; }

        /// <summary>
        /// Gets or sets the name of the state province.
        /// </summary>
        /// <value>
        /// The name of the state province.
        /// </value>
        public string StateProvinceName { get; set; }

    }
}
