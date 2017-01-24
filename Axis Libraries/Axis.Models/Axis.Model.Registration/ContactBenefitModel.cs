using Axis.Model.Address;
using Axis.Model.Common;
using System;

namespace Axis.Model.Registration
{
    /// <summary>
    ///
    /// </summary>
    public class ContactBenefitModel : PayorDetailModel, IContactBenefitBaseModel
    {
        /// <summary>
        /// Gets or sets the contact payor identifier.
        /// </summary>
        /// <value>
        /// The contact payor identifier.
        /// </value>
        public long? ContactPayorID { get; set; }

       
        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long? ContactID { get; set; }

        /// <summary>
        /// Gets or sets the contact payor rank.
        /// </summary>
        /// <value>
        /// The contact payor rank.
        /// </value>
        public int? ContactPayorRank { get; set; }

        /// <summary>
        /// Gets or sets the policy holder identifier.
        /// </summary>
        /// <value>
        /// The policy holder identifier.
        /// </value>
        public long? PolicyHolderID { get; set; }

        /// <summary>
        /// Gets or sets the policy holder.
        /// </summary>
        /// <value>
        /// The policy holder.
        /// </value>
        public string PolicyHolderName { get; set; }

        /// <summary>
        /// Gets or sets the policy identifier.
        /// </summary>
        /// <value>
        /// The policy identifier.
        /// </value>
        public string PolicyID { get; set; }

        /// <summary>
        /// Gets or sets the effective date.
        /// </summary>
        /// <value>
        /// The effective date.
        /// </value>
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// PayorGroupName
        /// </summary>
        public string PayorGroupName { get; set; }

        
        /// <summary>
        /// Gets or sets the first name of the policy holder.
        /// </summary>
        /// <value>
        /// The first name of the policy holder.
        /// </value>
        public string PolicyHolderFirstName { get; set; }

        /// <summary>
        /// Gets or sets the name of the policy holder middle.
        /// </summary>
        /// <value>
        /// The name of the policy holder middle.
        /// </value>
        public string PolicyHolderMiddleName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the policy holder.
        /// </summary>
        /// <value>
        /// The last name of the policy holder.
        /// </value>
        public string PolicyHolderLastName { get; set; }

        /// <summary>
        /// Gets or sets the policy holder suffix identifier.
        /// </summary>
        /// <value>
        /// The policy holder suffix identifier.
        /// </value>
        public int? PolicyHolderSuffixID { get; set; }

        ///////////////////////////////////////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        /// <summary>
        /// Gets or sets the deductible.
        /// </summary>
        /// <value>
        /// The deductible.
        /// </value>
        public decimal? Deductible { get; set; }

        /// <summary>
        /// Gets or sets the payor expiration reason identifier.
        /// </summary>
        /// <value>
        /// The payor expiration reason identifier.
        /// </value>
        public int? PayorExpirationReasonID { get; set; }

        /// <summary>
        /// Gets or sets the expiration reason.
        /// </summary>
        /// <value>
        /// The expiration reason.
        /// </value>
        public string ExpirationReason { get; set; }

        /// <summary>
        /// Gets or sets the add retro date.
        /// </summary>
        /// <value>
        /// The add retro date.
        /// </value>
        public DateTime? AddRetroDate { get; set; }

   
        /// <summary>
        /// Gets or sets the copay.
        /// </summary>
        /// <value>The copay.</value>
        public decimal? Copay { get; set; }

        /// <summary>
        /// Gets or sets the coinsurance.
        /// </summary>
        /// <value>
        /// The coinsurance.
        /// </value>
        public decimal? CoInsurance { get; set; }

        /// <summary>
        /// Gets or sets the policy holder employer.
        /// </summary>
        /// <value>
        /// The policy holder employer.
        /// </value>
        public string PolicyHolderEmployer { get; set; }


        /// <summary>
        /// Gets or sets the Billing Contact First Name.
        /// </summary>
        /// <value>
        /// The Billing Contact First Name.
        /// </value>
        public string BillingFirstName { get; set; }

        /// <summary>
        /// Gets or sets the Billing Contact Middle Name.
        /// </summary>
        /// <value>
        /// The Billing Contact Middle Name.
        /// </value>
        public string BillingMiddleName { get; set; }

        /// <summary>
        /// Gets or sets the Billing Contact Last Name.
        /// </summary>
        /// <value>
        /// The Billing Contact Last Name.
        /// </value>
        public string BillingLastName { get; set; }

        /// <summary>
        /// Gets or sets the Billing Contact suffix identifier.
        /// </summary>
        /// <value>
        /// The Billing Contact suffix identifier.
        /// </value>
        public int? BillingSuffixID { get; set; }

        /// <summary>
        /// Gets or sets the Additional Information.
        /// </summary>
        /// <value>
        /// The Additional Information identifier.
        /// </value>
        public string AdditionalInformation { get; set; }


          /// <summary>
        /// Gets or sets the has policy holder same card name.
        /// </summary>
        /// <value>
        /// The has policy holder same card name.
        /// </value>
        public bool? HasPolicyHolderSameCardName { get; set; }
        

    }
}