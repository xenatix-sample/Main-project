using Axis.Model.Common;
using Axis.Model.ESignature;
using System;
using System.Collections.Generic;

namespace Axis.Model.Registration
{
    public class FinancialSummaryModel : BaseEntity
    {
        public FinancialSummaryModel()
        {
        }

        /// <summary>
        /// Gets or sets the financial summary identifier.
        /// </summary>
        /// <value>
        /// The financial summary identifier.
        /// </value>
        public long FinancialSummaryID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the assessment start date.
        /// </summary>
        /// <value>
        /// The assessment start date.
        /// </value>
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// Gets or sets the assessment end date.
        /// </summary>
        /// <value>
        /// The assessment end date.
        /// </value>
        public DateTime? AssessmentEndDate { get; set; }

        /// <summary>
        /// Gets or sets the assessment Expiration date.
        /// </summary>
        /// <value>
        /// The assessment Expiration date.
        /// </value>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        /// <value>
        /// The organization identifier.
        /// </value>
        public long? OrganizationID { get; set; }

        /// <summary>
        /// Gets or sets the financial assessment XML.
        /// </summary>
        /// <value>
        /// The financial assessment XML.
        /// </value>
        public string FinancialAssessmentXML { get; set; }

        /// <summary>
        /// Gets or sets the date signed.
        /// </summary>
        /// <value>
        /// The date signed.
        /// </value>
        public DateTime? DateSigned { get; set; }

        /// <summary>
        /// Gets or sets the signature status identifier.
        /// </summary>
        /// <value>
        /// The signature status identifier.
        /// </value>
        public int? SignatureStatusID { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int? UserID { get; set; }

        /// <summary>
        /// Gets or sets the user phone identifier.
        /// </summary>
        /// <value>
        /// The user phone identifier.
        /// </value>
        public long? UserPhoneID { get; set; }

        /// <summary>
        /// Gets or sets the credential identifier.
        /// </summary>
        /// <value>
        /// The credential identifier.
        /// </value>
        public long? CredentialID { get; set; }

        /// <summary>
        /// Gets or sets the employment status identifier.
        /// </summary>
        /// <value>
        /// The employment status identifier.
        /// </value>
        public int? EmploymentStatusID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [looking for work].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [looking for work]; otherwise, <c>false</c>.
        /// </value>
        public bool? LookingForWork { get; set; }

        /// <summary>
        /// Gets or sets the FAStaffName.
        /// </summary>
        /// <value>
        /// The FAStaffName.
        /// </value>
        public string FAStaffName { get; set; }

        /// <summary>
        /// Gets or sets the FAStaffCredential.
        /// </summary>
        /// <value>
        /// The FAStaffCredential.
        /// </value>
        public string FAStaffCredential { get; set; }

        /// <summary>
        /// Gets or sets the FAStaffPhone.
        /// </summary>
        /// <value>
        /// The FAStaffPhone.
        /// </value>
        public string FAStaffPhone { get; set; }

        /// <summary>
        /// Gets or sets the FAStaffExtension.
        /// </summary>
        /// <value>
        /// The FAStaffExtension.
        /// </value>
        public string FAStaffExtension { get; set; }

        /// <summary>
        /// Gets or sets the financial assessment.
        /// </summary>
        /// <value>
        /// The financial assessment.
        /// </value>
        public FinancialAssessmentModel FinancialAssessment { get; set; }

        /// <summary>
        /// Gets or sets the financial assessment details.
        /// </summary>
        /// <value>
        /// The financial assessment details.
        /// </value>
        public List<FinancialAssessmentDetailsModel> FinancialAssessmentDetails { get; set; }

        /// <summary>
        /// Gets or sets the self pay.
        /// </summary>
        /// <value>
        /// The self pay.
        /// </value>
        public FinancialSummarySelfPayModel SelfPay { get; set; }

        /// <summary>
        /// Gets or sets the payors.
        /// </summary>
        /// <value>
        /// The payors.
        /// </value>
        public List<BenefitModel> Payors { get; set; }

        /// <summary>
        /// Gets or sets the financial summary signatures.
        /// </summary>
        /// <value>
        /// The financial summary signatures.
        /// </value>
        public List<FinancialSummaryConfirmationStatementModel> FinancialSummaryConfirmationStatements { get; set; }

        /// <summary>
        /// Gets or sets the client signature.
        /// </summary>
        /// <value>
        /// The client signature.
        /// </value>
        public DocumentEntitySignatureModel ClientSignature { get; set; }

        /// <summary>
        /// Gets or sets the staff signature.
        /// </summary>
        /// <value>
        /// The staff signature.
        /// </value>
        public DocumentEntitySignatureModel StaffSignature { get; set; }

        /// <summary>
        /// Gets or sets the legally authorized representative signature.
        /// </summary>
        /// <value>
        /// The legally authorized representative signature.
        /// </value>
        public DocumentEntitySignatureModel LegalAuthRepresentativeSignature { get; set; }
    }

    public class BenefitModel : IContactBenefitBaseModel
    {
        /// <summary>
        /// Gets or sets the contact payor identifier.
        /// </summary>
        /// <value>
        /// The contact payor identifier.
        /// </value>
        public long? ContactPayorID { get; set; }

        /// <summary>
        /// Gets or sets the name of the payor.
        /// </summary>
        /// <value>
        /// The name of the payor.
        /// </value>
        public string PayorName { get; set; }

        /// <summary>
        /// Gets or sets the payor code.
        /// </summary>
        /// <value>
        /// The payor code.
        /// </value>
        public int? PayorCode { get; set; }

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
        /// Gets or sets the relationship type identifier.
        /// </summary>
        /// <value>
        /// The relationship type identifier.
        /// </value>
        public int? RelationshipTypeID { get; set; }

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
        /// <value>
        /// The name of the payor group.
        /// </value>
        public string PayorGroupName { get; set; }

        /// <summary>
        /// Gets or sets the plan identifier.
        /// </summary>
        /// <value>
        /// The plan identifier.
        /// </value>
        public string PlanID { get; set; }

        /// <summary>
        /// Gets or sets the name of the plan.
        /// </summary>
        /// <value>
        /// The name of the plan.
        /// </value>
        public string PlanName { get; set; }

        /// <summary>
        /// Gets or sets the payor identifier.
        /// </summary>
        /// <value>
        /// The payor identifier.
        /// </value>
        public int? PayorID { get; set; }

        /// <summary>
        /// Gets or sets the payor group identifier.
        /// </summary>
        /// <value>
        /// The payor group identifier.
        /// </value>
        public int? PayorGroupID { get; set; }

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
        /// <value>
        /// The group identifier.
        /// </value>
        public string GroupID { get; set; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>
        /// The name of the group.
        /// </value>
        public string GroupName { get; set; }

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
    }

    public class FinancialSummarySelfPayModel : BaseEntity, ISelfPayOverrideReasonModel
    {
        /// <summary>
        /// Gets or sets the self pays.
        /// </summary>
        /// <value>
        /// The self pays.
        /// </value>
        public List<MonthlyAbilityToPayModel> MonthlyAbilities { get; set; }

        /// <summary>
        /// Gets or sets the isChild in conservatorship
        /// </summary>
        /// <value>
        /// The isChild in conservatorship.
        /// </value>
        public bool? ISChildInConservatorship { get; set; }

        /// <summary>
        /// Gets or sets the isNot attested
        /// </summary>
        /// <value>
        /// The isNot attested
        /// </value>
        public bool? IsNotAttested { get; set; }

        /// <summary>
        /// Gets or sets the isEnrolled in public benefits
        /// </summary>
        /// <value>
        /// The isEnrolled in public benefits
        /// </value>
        public bool? IsEnrolledInPublicBenefits { get; set; }

        /// <summary>
        /// Gets or sets the isRequesting reconsideration
        /// </summary>
        /// <value>
        /// The isRequesting reconsideration
        /// </value>
        public bool? IsRequestingReconsideration { get; set; }

        /// <summary>
        /// Gets or sets the IsNot giving consent
        /// </summary>
        /// <value>
        /// The IsNot giving consent
        /// </value>
        public bool? IsNotGivingConsent { get; set; }

        /// <summary>
        /// Gets or sets the isOther child enrolled
        /// </summary>
        /// <value>
        /// The isOther child enrolled
        /// </value>
        public bool? IsOtherChildEnrolled { get; set; }

        /// <summary>
        /// Gets or sets the isApplying for public benefits
        /// </summary>
        /// <value>
        /// The isApplying for public benefits
        /// </value>
        public bool? IsApplyingForPublicBenefits { get; set; }

        /// <summary>
        /// Gets or sets the isReconsideration of adjustment
        /// </summary>
        /// <value>
        /// The isReconsideration of adjustment
        /// </value>
        public bool? IsReconsiderationOfAdjustment { get; set; }
    }

    public class MonthlyAbilityToPayModel : ISelfPayModel
    {
        /// <summary>
        /// Gets or sets the self pay identifier.
        /// </summary>
        /// <value>
        /// The self pay identifier.
        /// </value>
        public long SelfPayID { get; set; }

        /// <summary>
        /// Gets or sets the organisation detail identifier.
        /// </summary>
        /// <value>
        /// The organisation detail identifier.
        /// </value>
        public long OrganizationDetailID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public decimal? SelfPayAmount { get; set; }

        /// <summary>
        /// Gets or sets the type of the amount.
        /// </summary>
        /// <value>
        /// The type of the amount.
        /// </value>
        public bool? IsPercent { get; set; }

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
    }
}