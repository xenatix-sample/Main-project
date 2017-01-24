namespace Axis.Constant
{
    public class RegistrationPermissionKey
    {
        public const string Registration_Demography = "Registration-Registration-Demographics";
        public const string Registration_AdditionalDemography = "Registration-Registration-AdditionalDemographics";
        public const string Registration_Registration_Referral = "Registration-Registration-Referral";
        public const string Registration_Collateral = "Registration-Registration-Collateral";
        public const string Registration_Payors = "Registration-Registration-Payors";
        public const string Registration_HouseholdIncome = "Registration-Registration-HouseholdIncome";
    }

    public class ECIPermissionKey
    {
        public const string ECI_Registration_AdditionalDemographics = "ECI-Registration-AdditionalDemographics";
        public const string ECI_Registration_Demographics = "ECI-Registration-Demographics";
        public const string ECI_IFSP_IFSP = "ECI-IFSP-IFSP";
        public const string ECI_ProgressNote_ProgressNote = "ECI-ProgressNote-ProgressNote";
        public const string ECI_Screening_Screening = "ECI-Screening-Screening";
        public const string ECI_Eligibility_Eligibility = "ECI-Eligibility-Eligibility";
        public const string ECI_Registration_Collateral = "ECI-Registration-Collateral";
        public const string ECI_Registration_Referral = "ECI-Registration-Referral";
        public const string ECI_Registration_Payors = "ECI-Registration-Payors";
        public const string ECI_Registration_HouseholdIncome = "ECI-Registration-HouseholdIncome";
    }

    public class SchedulingPermissionKey
    {
        public const string Scheduling_Appointment_SingleAppointment = "Scheduling-Appointment-SingleAppointment";
        public const string Scheduling_Appointment_GroupAppointment = "Scheduling-Appointment-GroupAppointment";
    }

    public class CallCenterPermissionKey
    {
        public const string CallCenter_CrisisLine = "CrisisLine-CrisisLine-CrisisLine";
        public const string CallCenter_LawLiaison = "LawLiaison-LawLiaison-LawLiaison";
        public const string CrisisLine_CrisisLine_Void = "CrisisLine-CrisisLine-Void";
    }

    public class ClinicalPermissionKey
    {
        public const string Clinical_Allergy_Allergy = "Clinical-Allergy-Allergy";
        public const string Clinical_ChiefComplaint_ChiefComplaint = "Clinical-ChiefComplaint-ChiefComplaint";
        public const string Clinical_Note_Note = "Clinical-Note-Note";
        public const string Clinical_ReviewOfSystems_ReviewofSystems = "Clinical-ReviewOfSystems-ReviewofSystems";
        public const string Clinical_Assessment_Assessment = "Clinical-Assessment-Assessment";
        public const string Clinical_SocialRelationshipHistory_SocialRelationshipHistory = "Clinical-SocialRelationshipHistory-SocialRelationshipHistory";
        public const string Clinical_Vitals_Vitals = "Clinical-Vitals-Vitals";
        public const string Clinical_PresentIllness_PresentIllness = "Clinical-PresentIllness-PresentIllness";
    }

    public class GeneralPermissionKey
    {
        public const string General_General_Address = "General-General-Address";
        public const string General_General_Phone = "General-General-Phone";
        public const string General_General_Email = "General-General-Email";
        public const string General_General_Referral = "General-General-Referral";
        public const string General_General_Collateral = "General-General-Collateral";
        public const string General_General_Admission = "General-General-Admission";
        public const string General_General_CompanyDischarge = "General-General-CompanyDischarge";
        public const string General_General_ProgramUnitDischarge = "General-General-ProgramUnitDischarge";
        public const string General_General_Demographics = "General-General-Demographics";
        public const string General_General_AdditionalDemographics = "General-General-AdditionalDemographics";
        public const string General_General_Admission_Discharge = "General-General-Admission/Discharge";
    }

    public class BenefitsPermissionKey
    {
        public const string Benefits_Payors_Payors = "Benefits-Payors-Payors";
        public const string Benefits_HouseholdIncome_HouseholdIncome = "Benefits-HouseholdIncome-HouseholdIncome";
        public const string Benefits_SelfPay_SelfPay = "Benefits-SelfPay-SelfPay";
        public const string Benefits_FinancialAssessment_FinancialAssessment = "Benefits-FinancialAssessment-FinancialAssessment";
        public const string Benefits_BAPN_BAPN = "Benefits-BenefitsAssistanceProgressNote-BenefitsAssistanceProgressNote";
        public const string Benefits_BAPN_Void = "Benefits-BenefitsAssistanceProgressNote-Void";
        public const string Benefits_BAPN_Addendum = "Benefits-BenefitsAssistanceProgressNote-Addendum";
    }

    public class SiteAdministrationPermissionKey
    {
        //Need to review with Rajiv - This is causing problem
        public const string SiteAdministration_MyProfile_Security_DigitalSignature = "SiteAdministration-MyProfile-SecurityDigitalSignature";
        public const string SiteAdministration_MyProfile_UserProfile = "SiteAdministration-MyProfile-UserProfile";
        public const string SiteAdministration_MyProfile_DivisionPrograms = "SiteAdministration-MyProfile-DivisionPrograms";
        public const string SiteAdministration_MyProfile_Credentials = "SiteAdministration-MyProfile-Credentials";
        public const string SiteAdministration_MyProfile_Scheduling = "SiteAdministration-MyProfile-Scheduling";
        public const string SiteAdministration_MyProfile_DirectReports = "SiteAdministration-MyProfile-DirectReports";
        public const string SiteAdministration_MyProfile_UserPhoto = "SiteAdministration-MyProfile-UserPhoto";

        public const string SiteAdministration_StaffManagement_UserRoles = "SiteAdministration-StaffManagement-UserRoles";
        public const string SiteAdministration_StaffManagement_DivisionPrograms = "SiteAdministration-StaffManagement-DivisionPrograms";
        public const string SiteAdministration_StaffManagement_BlockedTime = "SiteAdministration-StaffManagement-BlockedTime";
        public const string SiteAdministration_StaffManagement_Scheduling = "SiteAdministration-StaffManagement-Scheduling";
        public const string SiteAdministration_StaffManagement_UserProfile = "SiteAdministration-StaffManagement-UserProfile";
        public const string SiteAdministration_StaffManagement_UserDetails = "SiteAdministration-StaffManagement-UserDetails";
        public const string SiteAdministration_StaffManagement_AdditionalDetails = "SiteAdministration-StaffManagement-AdditionalDetails";
        public const string SiteAdministration_StaffManagement_Credentials = "SiteAdministration-StaffManagement-Credentials";
        public const string SiteAdministration_StaffManagement_DirectReports = "SiteAdministration-StaffManagement-DirectReports";
        public const string SiteAdministration_StaffManagement_UserPhoto = "SiteAdministration-StaffManagement-UserPhoto";

        public const string SiteAdministration_Settings_Settings = "SiteAdministration-Settings-Settings";
        public const string SiteAdministration_RoleManagement_RoleDetails = "SiteAdministration-RoleManagement-RoleDetails";
    }

    public class BusinessAdministrationPermissionKey
    {
        public const string BusinessAdministration_ClientMerge_ClientMerge = "BusinessAdministration-ClientMerge-ClientMerge";
    }
    
    public class ConsentsPermissionKey
    {
        public const string Consents_Assessment_Agency = "Consents-Assessment-Agency";
    }

    public class ReferralsPermissionKey
    {
        public const string Referrals_Referral_Referrer = "Referrals-Referral-Referrer";
        public const string Referrals_Referral_Contact = "Referrals-Referral-Contact";
        public const string Referrals_Referral_Disposition = "Referrals-Referral-Disposition";
        public const string Referrals_Referral_ForwardedTo = "Referrals-Referral-ForwardedTo";
        public const string Referrals_Referral_ReferredTo = "Referrals-Referral-ReferredTo";
        public const string Referrals_Referral_FollowUpOutcome = "Referrals-Referral-FollowUpOutcome";
        public const string Referrals_Referral_SingleAppointment = "Referrals-Referral-SingleAppointment";
    }

    public class LettersPermissionKey
    {
        public const string Intake_IDD_Letters = "Intake-IDDLetters-Letters";
    }
    public class IntakeFormsPermissionKey
    {
        public const string Intake_IDD_Forms = "Intake-IDDForms-Forms";
        public const string Intake_IDDForms_Void = "Intake-IDDForms-Void";

    }
    public class ChartPermissionKey
    {
        public const string  Chart_Chart_RecordedServices= "Chart-Chart-RecordedServices";
        public const string  Chart_Chart_Assessment = "Chart-Chart-Assessment";
        public const string Chart_Chart_Note = "Chart-Chart-Note";
    }
}