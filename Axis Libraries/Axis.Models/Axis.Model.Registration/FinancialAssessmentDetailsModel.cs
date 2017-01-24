using Axis.Model.Common;
using System;

namespace Axis.Model.Registration
{
    /// <summary>
    /// Model to hold Financial Assessment Detail data
    /// </summary>
    public class FinancialAssessmentDetailsModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the financial assessment details identifier.
        /// </summary>
        /// <value>
        /// The financial assessment details identifier.
        /// </value>
        public long FinancialAssessmentDetailsID { get; set; }

        /// <summary>
        /// Gets or sets the financial assessment identifier.
        /// </summary>
        /// <value>
        /// The financial assessment identifier.
        /// </value>
        public long FinancialAssessmentID { get; set; }

        /// <summary>
        /// Gets or sets the type of the financial assessment.
        /// </summary>
        /// <value>
        /// The type of the financial assessment.
        /// </value>
        public int CategoryTypeID { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public decimal? Amount { get; set; }

        /// <summary>
        /// Gets or sets the finance frequency identifier.
        /// </summary>
        /// <value>
        /// The finance frequency identifier.
        /// </value>
        public int FinanceFrequencyID { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        public int CategoryID { get; set; }
              
        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        public string Frequency { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the FinancialCategoryType.
        /// </summary>
        /// <value>
        /// The Financial Category Type.
        /// </value>
        public string FinancialCategoryType { get; set; }

        /// <summary>
        /// Gets or sets the RelationshipTypeID.
        /// </summary>
        /// <value>
        /// The RelationshipTypeID
        /// </value>
        public int RelationshipTypeID { get; set; }
    }
}