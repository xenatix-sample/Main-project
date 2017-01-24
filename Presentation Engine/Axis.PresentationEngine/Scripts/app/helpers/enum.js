// ****************** Enums ************************//
var ACTION = {
    READ: "Read",
    CREATE: "Create",
    DELETE: "Delete",
    UPDATE: "Update"
};

var ADDRESS_TYPE = {
    ResidenceHome: 1,
    Mailing: 2,
    Work: 3,
    BusinessAddress: 4
};

var CONTACT_TYPE = {
    Patient: 1,
    Supporting: 2,
    Emergency: 3,
    Collateral: 4,
    Referral: 5,
    ECI_Referral: 6,
    Referral_Requestor: 7,
    Family_Relationship: 8,
    Existing: 15,
    Crisis_Line: 9,
    Law_Liason: 10,
    Anonymous_Adult: 11,
    Anonymous_Child: 12,
    New: 13,
    Anonymous: 14
};

var REFERRAL_STATUS = {
    Closed: 3
};
var REFERRALTYPE = {
    External: 2
};
var PROGRAM_TYPE = {
    ECI: 1,
    IDD: 3,
    MH: 4
};

var RACE = {
    DeclineToSpecify: 6
};

var DOB_AGE = {
    MaxAge: 120
};

var Other_TYPE = {
    RaceID: 7,
    EthnicityID: 6,
    LanguagesID: 12,
    LegalStatusID: 8
};

var Constant_ID = {
    TKidsID: 12, //BUGFIX:12489
    CareID: 2
};

var ResourceType = {
    Room: 1,
    Provider: 2,
    CoProvider: 3,
    External: 4
};

var NoteType = {
    InitialContactAttempt: 1,
    InitialContact: 2,
    CrisisLine: 3,
    GroupNote: 4,
    DischargeNote: 5
};

var PREFERRED_CONTACT_METHOD = {
    Email: 1,
    Mail: 2,
    Phone: 3
};

var VETERAN_STATUS = {
    NeverServed: 1,
    Reserve: 2,
    HonorableDischarge: 3,
    ActiveDuty: 4,
    DishonorableDischarge: 5,
    Retired: 6,
    Unknown: 7,
    Other: 8
};

//fill data dynamically in main app.js
var LOOKUPTYPE = {};

var ASSESSMENT_TYPE = {
    PreBDIECINeeds: 1,
    PostBDINeeds: 2,
    IndividualizedFamilyServicePlan: 3,
    ReviewOfSystems: 4,
    MentalStatusExam: 5,
    CrisisAssessment: 6,
    ColumbiaSuicideSeverityRatingScale: 7,
    CrisisAdultScreening: 8,
    CrisisChildScreening: 9,
    LawLiaisonScreening: 10,
    CrisisLineProgressNote: 11,
    LawLiaisonProgressNote: 12,
    EHRPhotoID: 13,
    HIPAAAcknowlegement: 14,
    AssignmentofBenefits: 15,
    ConsentforServices: 16,
    ExplanationofIDDServicesandSupports: 17,
    ConsenttoSearch: 18,
    GeneralRelease: 19,
    AuthorizationtoDiscloseHealthInformation: 20,
    RevocationofAuthorizationtoDiscloseHealthInformation: 21,
    ProtectedHealthInformationAmendment: 22,
    ConsumerRights: 23,
    RightsAbuseandNeglectComplaintProcedures: 24,
    EmergencyMedicalCare: 25,
    OpportunitytoRegistertoVote: 26,
    DeclinationofVoterRegistration: 27,
    ConsentforPublication: 28,
    ConsentforAudioRecording: 29,
    AgainstProfessionalAdvice: 30,
    ConsentExpiration: 31,
    BenefitAssessmentsProgressNote: 32,
    IDDIntakeForms: 40
};
var ASSESSMENT_SECTION = {
    RiskFactors: 27,
    LawLiaisonScreening: 37,
    LawLiaisonProgressNote: 39,
    IDDFormsNote: 81,
    IdentificationofPreferencesDADS8648: 75,
    InterestListforGRServices: 76,
    ServiceCoordinationAssessmentDADS8647: 77,
    InterestListQuestionnaireDADS8577: 78,
    BAPNNote: 79,
    ColumbiaSuicideScale: 28,
    CrisisAssessment: 27,
    AdultScreening: 29,
    ChildScreening: 30,
    CrisisLineProgressNote: 38
};
var LEGAL_STATUS = {
    AdultNoGuardian: 1,
    Minor: 2
};

var LANGUAGE = {
    English: 4
};

var COLLATERAL_RELATIONSHIP_TYPE = {
    //change OtherRelationship from 58 to 7. 
    OtherRelationship: 7,
    OtherPhysician: 49,
    OtherProvider: 125
};

var RELATIONSHIP_TYPE_GROUPID = {
    Collateral: 1,
    FinancialAssessment: 4,
    Family: 5
};

var PAYOR_EXPIRATION_REASON = {
    Other: 2
};

var COLLATERAL_TYPE = {
    Family: 5
};

var SERVICE_RECORDING_SOURCE = {
    CallCenter: 1,
    Appointment: 2,
    Billing: 3,
    BAPN: 4,
    IDDForms: 5,
    LawLiaison: 6
};

var CREDENTIAL_ACTION_FORM = {
    BAPNServices: "Benefits Assessment - Recorded Services",
    IDDFormServices: "IDD Forms - Recorded Services"
};

var APPOINTMENT_TYPE = {
    NonMHMRAppointment: 29
};

var SIGNATURE_STATUS = {
    NotSigned: 1,
    Signed: 2
};
var DOCUMENT_TYPE = {
    Assessment: 1,
    Screening: 2,
    Consent: 3,
    Evaluation: 4,
    Clinical: 5,
    GroupNote: 6,
    AppointmentIndividualNote: 7,
    ContactDischargeNote: 8,
    ServiceRecording: 9,
    CallCenterProgressNote: 10,
    FinancialAssessment: 11,
    ConfirmationStatement: 12,
    CallCenter: 13,
    BenefitsAssistanceProgressNote: 14,
    Letter: 16,
    Form: 17
};
var REFERRAL_AGENCY = {
    Other: 40
};
var INPUT_TYPE = {
    Inactive: 0,
    Button: 1,
    Checkbox: 2,
    Radio: 3,
    Textbox: 4,
    Select: 5,
    MultiSelect: 6,
    None: 7,
    DatePicker: 8,
    TextArea: 9,
    TimePicker: 10,
    ESignature: 11,
    Label: 13,
    Comments: 15,
    Hidden: 16,
    ExtendedDropdown: 17,
    CheckboxList: 18,
    DSignature: 12
};

var CREDENTIAL_ACTION = {
    FilloutForms: 1,
    DigitalSignature: 2
};


var CREDENTIAL_PERMISSION = {
    QMHP: 37
};

var PERMISSION = {
    READ: "read",
    CREATE: "create",
    DELETE: "delete",
    UPDATE: "update",
    NONE: "none"
};

var PERMISSION_LEVEL = {
    Company: 1,
    ProgramUnit: 2
};

var PERMISSION_MODE = {
    READONLY: "readonly",
    ATTACH_ACTION: "attach-action",
    HIDE: "hide",
    REMOVE: "remove"
};

var CALL_STATUS = {
    COMPLETE: 1,
    DRAFT: 2,
    NEEDS_REVIEW: 3,
    NEW: 4,
    PENDING: 5,
    VOID: 6
};

var SERVICE_TYPE = {
    Planned: 1,
    Unplanned: 2
};

var Assessment_Expire_Reason = {
    DateBased: 1,
    Death: 2,
    Discharge: 3,
    EntryError: 4,
    FormRescinded: 5,
    Revoke: 6
};
var Employment_Status = {
    FullTimeStudent: 1,
    PartTimeStudent: 2,
    FullTimeEmployment: 3,
    PartTimeEmployment: 4,
    SelfEmployed: 5,
    TemporaryEmployment: 6,
    Unemployed: 7,
    Volunteer: 8,
    NotinLaborForce: 9,
    Retired: 10
};
var AGENCY_Data = {
    ADDRESS: "3840 Hulen St",
    CITY: "Fort Worth",
    NAME: "MHMR Tarrant",
    STATE: "TX",
    ZIP: "76107"
};
var BAPN_SECTIONS = {
    BenefitsScreening: 60,
    Fee_Assessment_Benefits_Research: 61,
    SSI_SSDI: 62,
    Medicare: 63,
    Traditional_Medicaid: 64,
    SNAP_CHIP_JPS_TANF: 65,
    Other: 66,
    Signature: 67,
    Note: 79
};

var DOCUMENT_STATUS = {
    Draft: 1,
    Completed: 2,
    Void: 3
};
var PRESENTING_PROBLEM_TYPE = {
    MH: 1,
    IDD: 2,
    ECI: 3,
    SA: 4,
    RC: 5
};
var Division = {
    ECI: 3,
    IDD: 4,
    BHMH: 5,
    BHSUD: 6
};
var DIVISION_NAME = {
    ECS: "Early Childhood Services",
    IDD: "Intellectual and Developmental Disabilities",
    BHMH: "Mental Health",
    BHSUD: "Substance Use Disorders"
};
var Modules = {
    SiteAdministration: "Site Administration",
    BusinessAdministration: "Business Administration",
    Registration: "Registration",
    ESignature: "ESignature",
    ECI: "ECI",
    Scheduling: "Scheduling",
    Clinical: "Clinical",
    CrisisLine: "Crisis Line",
    LawLiaison: "Law Liaison",
    Sync: "Sync",
    General: "General",
    Reports: "Reports",
    Referrals: "Referrals",
    Benefits: "Benefits",
    Consents: "Consents",
    Intake: "Intake"
};
var RegistrationPermissionKey = {
    Registration_Demography: "Registration-Registration-Demographics",
    Registration_AdditionalDemography: "Registration-Registration-AdditionalDemographics",
    Registration_Registration_Referral: "Registration-Registration-Referral",
    Registration_Collateral: "Registration-Registration-Collateral",
    Registration_Payors: "Registration-Registration-Payors",
    Registration_HouseholdIncome: "Registration-Registration-HouseholdIncome"
};
var ECIPermissionKey = {
    ECI_Registration_AdditionalDemographics: "ECI-Registration-AdditionalDemographics",
    ECI_Registration_Demographics: "ECI-Registration-Demographics",
    ECI_Registration_Referral: "ECI-Registration-Referral",
    ECI_Registration_Collateral: "ECI-Registration-Collateral",
    ECI_Registration_Payors: "ECI-Registration-Payors",
    ECI_Registration_HouseholdIncome: "ECI-Registration-HouseholdIncome",
    ECI_IFSP_IFSP: "ECI-IFSP-IFSP",
    ECI_ProgressNote_ProgressNote: "ECI-ProgressNote-ProgressNote",
    ECI_Screening_Screening: "ECI-Screening-Screening",
    ECI_Eligibility_Eligibility: "ECI-Eligibility-Eligibility"
};
var SchedulingPermissionKey = {
    Scheduling_Appointment_SingleAppointment: "Scheduling-Appointment-SingleAppointment",
    Scheduling_Appointment_GroupAppointment: "Scheduling-Appointment-GroupAppointment"
};
var CallCenterPermissionKey = {
    CallCenter_CrisisLine: "CrisisLine-CrisisLine-CrisisLine",
    CallCenter_LawLiaison: "LawLiaison-LawLiaison-LawLiaison"
};
var ClinicalPermissionKey = {
    Clinical_Allergy_Allergy: "Clinical-Allergy-Allergy",
    Clinical_ChiefComplaint_ChiefComplaint: "Clinical-ChiefComplaint-ChiefComplaint",
    Clinical_Note_Note: "Clinical-Note-Note",
    Clinical_ReviewOfSystems_ReviewofSystems: "Clinical-ReviewOfSystems-ReviewofSystems",
    Clinical_Assessment_Assessment: "Clinical-Assessment-Assessment",
    Clinical_SocialRelationshipHistory_SocialRelationshipHistory:
        "Clinical-SocialRelationshipHistory-SocialRelationshipHistory",
    Clinical_Vitals_Vitals: "Clinical-Vitals-Vitals",
    Clinical_PresentIllness_PresentIllness: "Clinical-PresentIllness-PresentIllness",
};
var GeneralPermissionKey = {
    General_General_Address: "General-General-Address",
    General_General_Phone: "General-General-Phone",
    General_General_Email: "General-General-Email",
    General_General_AdditionalDemographics: "General-General-AdditionalDemographics",
    General_General_Referral: "General-General-Referral",
    General_General_Collateral: "General-General-Collateral",
    General_General_Demographics: "General-General-Demographics",
    General_General_AdmissionDischarge: "General-General-Admission/Discharge",
    General_General_Admission: "General-General-Admission",
    General_General_CompanyDischarge: "General-General-CompanyDischarge",
    General_General_ProgramUnitDischarge: "General-General-ProgramUnitDischarge"
};
var SiteAdministrationPermissionKey = {
    SiteAdministration_MyProfile_UserProfile: "SiteAdministration-MyProfile-UserProfile",
    SiteAdministration_MyProfile_Credentials: "SiteAdministration-MyProfile-Credentials",
    SiteAdministration_MyProfile_DivisionPrograms: "SiteAdministration-MyProfile-DivisionPrograms",
    SiteAdministration_MyProfile_Scheduling: "SiteAdministration-MyProfile-Scheduling",
    SiteAdministration_MyProfile_DirectReports: "SiteAdministration-MyProfile-DirectReports",
    SiteAdministration_MyProfile_UserPhoto: "SiteAdministration-MyProfile-UserPhoto",
    SiteAdministration_MyProfile_Security_DigitalSignature: "SiteAdministration-MyProfile-SecurityDigitalSignature",

    SiteAdministration_StaffManagement_UserRoles: "SiteAdministration-StaffManagement-UserRoles",
    SiteAdministration_StaffManagement_Scheduling: "SiteAdministration-StaffManagement-Scheduling",
    SiteAdministration_StaffManagement_UserProfile: "SiteAdministration-StaffManagement-UserProfile",
    SiteAdministration_StaffManagement_UserDetails: "SiteAdministration-StaffManagement-UserDetails",
    SiteAdministration_StaffManagement_AdditionalDetails: "SiteAdministration-StaffManagement-AdditionalDetails",
    SiteAdministration_StaffManagement_Credentials: "SiteAdministration-StaffManagement-Credentials",
    SiteAdministration_StaffManagement_DivisionPrograms: "SiteAdministration-StaffManagement-DivisionPrograms",
    SiteAdministration_StaffManagement_DirectReports: "SiteAdministration-StaffManagement-DirectReports",
    SiteAdministration_StaffManagement_BlockedTime: "SiteAdministration-StaffManagement-BlockedTime",
    SiteAdministration_StaffManagement_UserPhoto: "SiteAdministration-StaffManagement-UserPhoto",

    SiteAdministration_Settings_Settings: "SiteAdministration-Settings-Settings",

    SiteAdministration_RoleManagement_RoleDetails: "SiteAdministration-RoleManagement-RoleDetails",
    SiteAdministration_RoleManagement_Assignmodules: "SiteAdministration-RoleManagement-Assignmodules"
};
var BusinessAdministrationPermissionKey = {
    BusinessAdministration_ClientMerge_ClientMerge: "BusinessAdministration-ClientMerge-ClientMerge",
    BusinessAdministration_Configuration_Company: "BusinessAdministration-Configuration-Company",
    BusinessAdministration_Configuration_Division: "BusinessAdministration-Configuration-Division",
    BusinessAdministration_Configuration_Program: "BusinessAdministration-Configuration-Program",
    BusinessAdministration_Configuration_ProgramUnit: "BusinessAdministration-Configuration-ProgramUnit",
    BusinessAdministration_Configuration_Payors: "BusinessAdministration-Configuration-Payors",
    BusinessAdministration_Configuration_Services: "BusinessAdministration-Configuration-Services"
};
var BenifitsPermissionKey = {
    Benefits_SelfPay_SelfPay: "Benefits-SelfPay-SelfPay",
    Benefits_FinancialAssessment_FinancialAssessment: "Benefits-FinancialAssessment-FinancialAssessment",
    Benefits_BAPN_BAPN: "Benefits-BenefitsAssistanceProgressNote-BenefitsAssistanceProgressNote",
    Benefits_BAPN_Void: "Benefits-BenefitsAssistanceProgressNote-Void",
    Benefits_BAPN_Addendum: "Benefits-BenefitsAssistanceProgressNote-Addendum",
    Benefits_HouseholdIncome_HouseholdIncome: "Benefits-HouseholdIncome-HouseholdIncome",
    Benefits_Payors_Payors: "Benefits-Payors-Payors"
};
var ConsentsPermissionKey = {
    Consents_Assessment_Agency: "Consents-Assessment-Agency"
};
var ReferralsPermissionKey = {
    Referrals_Referral_Referrer: "Referrals-Referral-Referrer",
    Referrals_Referral_Contact: "Referrals-Referral-Contact",
    Referrals_Referral_Disposition: "Referrals-Referral-Disposition",
    Referrals_Referral_ForwardedTo: "Referrals-Referral-ForwardedTo",
    Referrals_Referral_ReferredTo: "Referrals-Referral-ReferredTo",
    Referrals_Referral_FollowUpOutcome: "Referrals-Referral-FollowUpOutcome",
    Referrals_Referral_SingleAppointment: "Referrals-Referral-SingleAppointment"
};
var LettersPermissionKey = {
    Intake_IDD_Letters: "Intake-IDDLetters-Letters",
    Intake_IDDForms_Forms: "Intake-IDDForms-Forms"
};
var IntakeFormsPermissionKey = {
    Intake_IDD_Forms: "Intake-IDD-Forms"
};
var ChartPermissionKey = {
    Chart_Chart_RecordedServices: "Chart-Chart-RecordedServices",
    Chart_Chart_Assessment: "Chart-Chart-Assessment",
    Chart_Chart_Note: "Chart-Chart-Note"
};
var SERVICES = {
    Crisis_Line: "Crisis Line",
    Law_Liaison: "Law Liaison",
    BAPN: "BAPN",
    FORM: "FORM"
};
var SERVICE_ITEM = {
    Benefits_Assistance: 20,
    Financial_Assessment: 59,
    Safety_And_Monitoring: 122,
    Crisis_Services: 172,
    Information: 173
};
var CRISISLINE_QUICK_REG_DATA = {
    ClientData: "CL_ClientData",
    PhoneData: "CL_PhoneData",
};
var LAWLIAISON_QUICK_REG_DATA = {
    ClientData: "LL_ClientData",
    PhoneData: "LL_PhoneData",
};
var BAPN_ASSESSMENT_SECTION = {
    Addedndum: 82
};
var CALL_TYPE = {
    Other: 4
};
var MODULE_COMPONENT = {
    IntakeForms: 82,
    BAPN: 72,
    Law_Liaison: 44,
    Crisis_Line: 43
};
var SCREEN = {
    Demography: 23
};
var PROVIDER_KEY = {
    Admission: 1,
    IDD_Intake: 2,
    BAPN_Service: 3,
    CrisisLine_Service: 4,
    CrisisLine_Service_Supervising: 5,
    LawLiaison_Service: 6,
    LawLiaison_Service_Supervising: 7,
    ECI_Registration_AdditionalDemography: 8
};
var VALIDATION_STATE = {
    Valid: "valid",
    Warning: "warning",
    Invalid: "invalid"
};
var COUNT_NUMERIC = {
    One: 1,
    Two: 2
};

var SERVICE_STATUS = {
    StatusNotSelected: -1,
    FollowUp_Relapse_Prevention: 1,
    Call_to_MCOT: 2,
    NonRehab: 3
};
var RECIPIENT = {
    Staff_Only: 18
};

var WORKFLOWHEADER_DATAKEY = {
    BAPN_BAPN: 'Benefits-BenefitsAssistanceProgressNote-BenefitsAssistanceProgressNote',
    Callcenter_Lawliaison: 'LawLiaison-LawLiaison-LawLiaison',
    Callcenter_CrisisLine: 'CrisisLine-CrisisLine-CrisisLine',
    Intake_IDD_Forms: 'Intake-IDDForms-Forms'
};