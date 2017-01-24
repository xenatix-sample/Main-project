using System;

namespace Axis.Model.Registration
{
    /// <summary>
    ///
    /// </summary>
    public interface IContactBenefitBaseModel
    {
        /// <summary>
        /// Gets or sets the contact payor identifier.
        /// </summary>
        /// <value>
        /// The contact payor identifier.
        /// </value>
        long? ContactPayorID { get; set; }

        /// <summary>
        /// Gets or sets the name of the payor.
        /// </summary>
        /// <value>
        /// The name of the payor.
        /// </value>
        string PayorName { get; set; }

        /// <summary>
        /// Gets or sets the payor code.
        /// </summary>
        /// <value>
        /// The payor code.
        /// </value>
        int? PayorCode { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        long? ContactID { get; set; }

        /// <summary>
        /// Gets or sets the contact payor rank.
        /// </summary>
        /// <value>
        /// The contact payor rank.
        /// </value>
        int? ContactPayorRank { get; set; }

        /// <summary>
        /// Gets or sets the policy holder identifier.
        /// </summary>
        /// <value>
        /// The policy holder identifier.
        /// </value>
        long? PolicyHolderID { get; set; }

        /// <summary>
        /// Gets or sets the policy holder.
        /// </summary>
        /// <value>
        /// The policy holder.
        /// </value>
        string PolicyHolderName { get; set; }

        /// <summary>
        /// Gets or sets the policy identifier.
        /// </summary>
        /// <value>
        /// The policy identifier.
        /// </value>
        string PolicyID { get; set; }

        /// <summary>
        /// Gets or sets the effective date.
        /// </summary>
        /// <value>
        /// The effective date.
        /// </value>
        DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// PayorGroupName
        /// </summary>
        /// <value>
        /// The name of the payor group.
        /// </value>
        string PayorGroupName { get; set; }

        /// <summary>
        /// Gets or sets the plan identifier.
        /// </summary>
        /// <value>
        /// The plan identifier.
        /// </value>
        string PlanID { get; set; }

        /// <summary>
        /// Gets or sets the name of the plan.
        /// </summary>
        /// <value>
        /// The name of the plan.
        /// </value>
        string PlanName { get; set; }

        /// <summary>
        /// Gets or sets the payor identifier.
        /// </summary>
        /// <value>
        /// The payor identifier.
        /// </value>
        int? PayorID { get; set; }

        /// <summary>
        /// Gets or sets the payor group identifier.
        /// </summary>
        /// <value>
        /// The payor group identifier.
        /// </value>
        int? PayorGroupID { get; set; }

        /// <summary>
        /// Gets or sets the payor plan identifier.
        /// </summary>
        /// <value>
        /// The payor plan identifier.
        /// </value>
        int? PayorPlanID { get; set; }

        /// <summary>
        /// Gets or sets the group identifier.
        /// </summary>
        /// <value>
        /// The group identifier.
        /// </value>
        string GroupID { get; set; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>
        /// The name of the group.
        /// </value>
        string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the first name of the policy holder.
        /// </summary>
        /// <value>
        /// The first name of the policy holder.
        /// </value>
        string PolicyHolderFirstName { get; set; }

        /// <summary>
        /// Gets or sets the name of the policy holder middle.
        /// </summary>
        /// <value>
        /// The name of the policy holder middle.
        /// </value>
        string PolicyHolderMiddleName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the policy holder.
        /// </summary>
        /// <value>
        /// The last name of the policy holder.
        /// </value>
        string PolicyHolderLastName { get; set; }

        /// <summary>
        /// Gets or sets the policy holder suffix identifier.
        /// </summary>
        /// <value>
        /// The policy holder suffix identifier.
        /// </value>
        int? PolicyHolderSuffixID { get; set; }

        /// <summary>
        /// Gets or sets the policy holder employer.
        /// </summary>
        /// <value>
        /// The policy holder employer.
        /// </value>
        string PolicyHolderEmployer { get; set; }


        /// <summary>
        /// Gets or sets the Billing Contact First Name.
        /// </summary>
        /// <value>
        /// The Billing Contact First Name.
        /// </value>
        string BillingFirstName { get; set; }

        /// <summary>
        /// Gets or sets the Billing Contact Middle Name.
        /// </summary>
        /// <value>
        /// The Billing Contact Middle Name.
        /// </value>
        string BillingMiddleName { get; set; }

        /// <summary>
        /// Gets or sets the Billing Contact Last Name.
        /// </summary>
        /// <value>
        /// The Billing Contact Last Name.
        /// </value>
        string BillingLastName { get; set; }

        /// <summary>
        /// Gets or sets the Billing Contact suffix identifier.
        /// </summary>
        /// <value>
        /// The Billing Contact suffix identifier.
        /// </value>
        int? BillingSuffixID { get; set; }

        /// <summary>
        /// Gets or sets the Additional Information.
        /// </summary>
        /// <value>
        /// The Additional Information identifier.
        /// </value>
        string AdditionalInformation { get; set; }
    }
}