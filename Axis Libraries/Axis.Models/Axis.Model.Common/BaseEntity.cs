using System;

namespace Axis.Model.Common
{
    /// <summary>
    ///
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the is active.
        /// </summary>
        /// <value>
        /// The is active.
        /// </value>
        public Nullable<bool> IsActive { get; set; }

        /// <summary>
        /// Gets or sets the modified on.
        /// </summary>
        /// <value>
        /// The modified on.
        /// </value>
        public Nullable<DateTime> ModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        /// <value>
        /// The modified by.
        /// </value>
        public Nullable<int> ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public Nullable<int> CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>
        /// The created on.
        /// </value>
        public Nullable<DateTime> CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the system created on.
        /// </summary>
        /// <value>
        /// The system created on.
        /// </value>
        public Nullable<DateTime> SystemCreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the system modified on.
        /// </summary>
        /// <value>
        /// The system modified on.
        /// </value>
        public Nullable<DateTime> SystemModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the force rollback.
        /// </summary>
        /// <value>
        /// The force rollback.
        /// </value>
        public Nullable<bool> ForceRollback { get; set; }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public long TransactionID { get; set; }

        /// <summary>
        /// Gets or sets the screen identifier.
        /// </summary>
        /// <value>
        /// The screen identifier.
        /// </value>
        public int ScreenID { get; set; }
    }
}