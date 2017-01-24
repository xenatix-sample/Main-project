using Axis.Model.Common;
using System;
using System.Collections.Generic;

namespace Axis.Model.Registration
{
    /// <summary>
    /// Model to hold Financial Assessment Detail data
    /// </summary>
    public class FinancialAssessmentModel : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialAssessmentModel"/> class.
        /// </summary>
        public FinancialAssessmentModel()
        {
            FinancialAssessmentDetails = new List<FinancialAssessmentDetailsModel>();
        }

        /// <summary>
        /// Gets or sets the financial assessment identifier.
        /// </summary>
        /// <value>
        /// The financial assessment identifier.
        /// </value>
        public long FinancialAssessmentID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the total income.
        /// </summary>
        /// <value>
        /// The total income.
        /// </value>
        public decimal TotalIncome { get; set; }

        /// <summary>
        /// Gets or sets the total expenses.
        /// </summary>
        /// <value>
        /// The total expenses.
        /// </value>
        public decimal TotalExpenses { get; set; }

        /// <summary>
        /// Gets or sets the TotalExtraOrdinary Expenses
        /// </summary>
        /// <value>
        /// The total income.
        /// </value>
        public decimal TotalExtraOrdinaryExpenses { get; set; }

        /// <summary>
        /// Gets or sets the total other.
        /// </summary>
        /// <value>
        /// The total expenses.
        /// </value>
        public decimal TotalOther { get; set; }

        /// <summary>
        /// Gets or sets the adjusted gross income.
        /// </summary>
        /// <value>
        /// The adjusted gross income.
        /// </value>
        public decimal AdjustedGrossIncome { get; set; }

        /// <summary>
        /// Gets or sets the assessment date.
        /// </summary>
        /// <value>
        /// The assessment date.
        /// </value>
        public DateTime AssessmentDate { get; set; }

        /// <summary>
        /// Gets or sets the size of the family.
        /// </summary>
        /// <value>
        /// The size of the family.
        /// </value>
        public int FamilySize { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the expiration reason identifier.
        /// </summary>
        /// <value>
        /// The expiration reason identifier.
        /// </value>
        public int? ExpirationReasonID { get; set; }

        /// <summary>
        /// Gets or sets the financial assessment details.
        /// </summary>
        /// <value>
        /// The financial assessment details.
        /// </value>
        public List<FinancialAssessmentDetailsModel> FinancialAssessmentDetails { get; set; }

        /// <summary>
        /// Gets or sets the view financial assessemnt.
        /// </summary>
        /// <value>
        /// The view financial assessemnt.
        /// </value>
        public bool? IsViewFinanicalAssessment { get; set; }
    }
}