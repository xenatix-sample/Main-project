using Axis.Data.Repository;
using Axis.DataProvider.Common.Lookups.Assesssment;
using Axis.DataProvider.Common.Lookups.FamilyRelationship;
using Axis.DataProvider.Common.Lookups.GroupService;
using Axis.DataProvider.Common.Lookups.GroupType;
using Axis.DataProvider.Common.Lookups.Services;
using Axis.DataProvider.Scheduling;
using Axis.Model.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// Data provider for lookup tables
    /// </summary>
    public class LookupDataProvider : ILookupDataProvider
    { 
        #region initializations

        private IUnitOfWork _unitOfWork;
        private IAddressTypeDataProvider _addressTypeDataProvider;
        private ICountyDataProvider _countyDataProvider;
        private IStateProvinceDataProvider _stateProvinceDataProvider;
        private IMailPermissionTypeDataProvider _mailPermissionTypeDataProvider;
        private IDOBStatusDataProvider _dobStatusDataProvider;
        private IGenderDataProvider _genderDataProvider;
        private IPayorTypeDataProvider _payorTypeDataProvider;
        private IReferralSourceDataProvider _referralSourceDataProvider;
        private ISmokingStatusDataProvider _smokingStatusDataProvider;
        private IPhonePermissionDataProvider _phonePermissionDataProvider;
        private IContactTypeDataProvider _contactTypeDataProvider;
        private IClientTypeDataProvider _clientTypeDataProvider;
        private IClientIdentifierTypeDataProvider _clientIdentifierTypeDataProvider;
        private ISSNStatusDataProvider _ssnStatusDataProvider;
        private IContactMethodDataProvider _contactMethodDataProvider;
        private IMaritalStatusDataProvider _maritalStatusDataProvider;
        private IPrefixTypeDataProvider _prefixTypeDataProvider;
        private ICategoryDataProvider _categoryDataProvider;
        private IFinanceFrequencyDataProvider _frequencyDataProvider;
        private IExpirationReasonDataProvider _expirationReasonDataProvider;
        private IRaceDataProvider _raceDataProvider;
        private IEthnicityDataProvider _ethnicityDataProvider;
        private ILegalStatusDataProvider _legalStatusDataProvider;
        private ILetterOutcomeDataProvider _letterOutcomeDataProvider;
        private ILivingArrangementDataProvider _livingArrangementDataProvider;
        private IEducationStatusDataProvider _educationStatusDataProvider;
        private ICitizenshipDataProvider _citizenshipDataProvider;
        private IEmploymentStatusDataProvider _employmentStatusDataProvider;
        private IReligionDataProvider _religionDataProvider;
        private IVeteranStatusDataProvider _veteranStatusDataProvider;
        private IPresentingProblemTypeDataProvider _presentingProblemTypeDataProvider;
        private ILanguageDataProvider _languageDataProvider;
        private ISchoolDistrictDataProvider _schoolDistrictDataProvider;
        private IEmailPermissionDataProvider _emailPermissionDataProvider;
        private IPhoneTypeDataProvider _phoneTypeDataProvider;
        private IPayorDataProvider _payorDataProvider;
        private IPolicyHolderDataProvider _policyHolderDataProvider;
        private ISuffixDataProvider _suffixDataProvider;
        private ISecurityQuestionDataProvider _securityQuestionDataProvider;
        private IRelationshipTypeDataProvider _relationshipTypeDataProvider;
        private ICollateralTypeDataProvider _collateralTypeDataProvider;
        private ICredentialDataProvider _credentialDataProvider;
        private IReceiveCorrespondenceDataProvider _receiveCorrespondenceDataProvider;
        private IEligibilityTypeDataProvider _eligibilityTypeDataProvider;
        private IDurationDataProvider _durationDataProvider;
        private IEligibilityCategoryDataProvider _eligibilityCategoryDataProvider;
        private IScreeningNameDataProvider _screeningNameDataProvider;
        private IScreeningResultDataProvider _screeningResultDataProvider;
        private IScreeningTypeDataProvider _screeningTypeDataProvider;
        private IServiceCoordinatorDataProvider _serviceCoordinatorDataProvider;
        private IIFSPTypeDataProvider _ifspTypeDataProvider;
        private IReasonForDelayDataProvider _reasonForDelayDataProvider;
        private IProgramDataProvider _programProvider;
        private IFacilityDataProvider _facilityProvider;
        private IDayOfWeekDataProvider _dayOfWeekProvider;
        private IWeekOfMonthDataProvider _weekOfMonthProvider;
        private IMonthDataProvider _monthProvider;
        private IDocumentTypeDataProvider _documentTypeProvider;
        private IScheduleTypeDataProvider _scheduleTypeProvider;
        private IUsersDataProvider _usersDataProvider;
        private IUserFacilityDataProvider _userFacilityDataProvider;
        private IReportDataProvider _reportDataProvider;
        private IBPMethodDataProvider _bpMethodDataProvider;
        private INoteTypeDataProvider _noteTypeDataProvider;
        private INoteStatusDataProvider _noteStatusDataProvider;
        private IAllergyLookupDataProvider _allergyLookupDataProvider;
        private IAssessmentDataProvider _assessmentDataProvider;
        private IDrugLookupDataProvider _drugLookupDataProvider;
        private IMedicalConditionLookupDataProvider _medicalConditionLookupDataProvider;
        private IFamilyRelationshipDataProvider _familyRelationshipDataProvider;
        private IReferralStatusDataProvider _referralStatusDataProvider;

        private IReferralTypeDataProvider _referralTypeDataProvider;
        private IReferralResourceTypeDataProvider _referralResourceTypeDataProvider;
        private IResourceTypeDataProvider _resourceTypeDataProvider;
        private IReferralDispositionTypeDataProvider _referralDispositionTypeDataProvider;
        private IReferralDispositionOutcomeTypeDataProvider _referralDispositionOutcomeTypeDataProvider;
        private IReferralOriginDataProvider _referralOriginDataProvider;
        private IReferralCategoryDataProvider _referralCategoryDataProvider;

        private IImmunizationStatusDataProvider _immunizationStatusDataProvider;
        private IReferralConcernTypeDataProvider _referralConcernTypeDataProvider;
        private IReferralPriorityDataProvider _referralPriorityDataProvider;
        private IDischargeReasonDataProvider _dischargeReasonDataProvider;
        private IReferralOrganizationDataProvider _organizationDataProvider;
        private IReferralCategorySourceDataProvider _referralCategorySourceDataProvider;
        private IReferralDispositionStatusTypeDataProvider _referralDispositionStatusTypeDataProvider;
        private IProgramClientIdentifierDataProvider _programClientIdentifierDataProvider;
        private IAppointmentDataProvider _appointmentDataProvider;

        private IServiceItemDataProvider _serviceItemDataProvider;
        private IAttendanceStatusDataProvider _attendanceStatusDataProvider;
        private IDeliveryMethodDataProvider _deliveryMethodDataProvider;
        private IServiceStatusDataProvider _serviceStatusDataProvider;
        private IServiceLocationDataProvider _serviceLocationDataProvider;
        private IRecipientCodeDataProvider _recipientCodeDataProvider;
        private IConversionStatusDataProvider _conversionStatusDataProvider;

        private IServiceLocationDataProvider _locationServiceDataProvider;
        private IContactTypeCallCenterDataProvider _contactTypeCallCenterDataProvider;
        private ICallStatusDataProvider _callStatusDataProvider;
        private ICCPriorityDataProvider _cCPriorityDataProvider;
        private IProgramUnitDataProvider _programUnitDataProvider;
        private ISuicidalHomicidalDataProvider _sHIDDataProvider;
        private ICallTypeDataProvider _callTypeProvider;
        private IClientStatusDataProvider _clientStatusDataProvider;
        private IBehavioralCategoryDataProvider _behavioralCategoryDataProvider;
        private ICancelReasonDataProvider _cancelReasonDataProvider;
        private IReferralAgencyDataProvider _referralAgencyDataProvider;
        private IAdvancedDirectiveTypeDataProvider _advancedDirectiveTypeDataProvider;
        private IServiceDataProvider _serviceDataProvider;
        private IGroupTypeDataProvider _groupTypeDataProvider;
        private IGroupServiceDataProvider _groupServiceDataProvider;
        private IPayorExpirationReasonDataProvider _payorExpirationReasonDataProvider;
        private IServiceTypeDataProvider _serviceTypeDataProvider;
        private IServiceConfigTypeDataProvider _serviceConfigTypeDataProvider;
        private IServiceWorkflowTypeDataProvider _serviceWorkflowTypeDataProvider;
        private ISignatureStatusDataProvider _signatureStatusDataProvider;
        private IVoidRecordedServiceReasonDataProvider _voidRecordedServiceReasonDataProvider;
        private IOrganizationsDataProvider _organizationsDataProvider;
        private IEntityTypeDataProvider _entityTypeDataProvider;
        private IServiceRecordingSourceDataProvider _serviceRecordingSourceDataProvider;
        private IDocumentTypeGroupDataProvider _documentTypeGroupDataProvider;
        private IUserIdentifierTypeDataProvider _userIdentifierTypeDataProvider;
        private IConfirmationStatementDataProvider _confirmationStatementDataProvider;
        private ITrackingFieldDataProvider _trackingFieldDataProvider;

        private IServicesDataProvider _servicesDataProvider;
        private ICauseOfDeathDataProvider _causeOfDeathDataProvider;
        private IAdmissionReasonDataProvider _admissionReasonDataProvider;
        private IServiceDurationDataProvider _serviceDurationDataProvider;

        public LookupDataProvider(IUnitOfWork unitOfWork,
        IAddressTypeDataProvider addressTypeDataProvider,
        ICountyDataProvider countyDataProvider,
        IStateProvinceDataProvider stateProvinceDataProvider,
        IMailPermissionTypeDataProvider mailPermissionTypeDataProvider,
        IDOBStatusDataProvider dobStatusDataProvider,
        IGenderDataProvider genderDataProvider,
        IPayorTypeDataProvider payorTypeDataProvider,
        IReferralSourceDataProvider referralSourceDataProvider,
        ISmokingStatusDataProvider smokingStatusDataProvider,
        IPhonePermissionDataProvider phonePermissionDataProvider,
        IContactTypeDataProvider contactTypeDataProvider,
        IContactTypeCallCenterDataProvider contactTypeCallCenterDataProvider,
        IClientTypeDataProvider clientTypeDataProvider,
        IClientIdentifierTypeDataProvider clientIdentifierTypeDataProvider,
        ISSNStatusDataProvider ssnStatusDataProvider,
        IContactMethodDataProvider contactMethodDataProvider,
        IMaritalStatusDataProvider maritalStatusDataProvider,
        IPrefixTypeDataProvider prefixTypeDataProvider,
        ICategoryDataProvider categoryDataProvider,
        IFinanceFrequencyDataProvider frequencyDataProvider,
        IExpirationReasonDataProvider expirationReasonDataProvider,
        IRaceDataProvider raceDataProvider,
        IEthnicityDataProvider ethnicityDataProvider,
        ILegalStatusDataProvider legalStatusDataProvider,
        ILetterOutcomeDataProvider letterOutcomeDataProvider,
        ILivingArrangementDataProvider livingArrangementDataProvider,
        IEducationStatusDataProvider educationStatusDataProvider,
        ICitizenshipDataProvider citizenshipDataProvider,
        IEmploymentStatusDataProvider employmentStatusDataProvider,
        IReligionDataProvider religionDataProvider,
        IVeteranStatusDataProvider veteranStatusDataProvider,
        IPresentingProblemTypeDataProvider presentingProblemTypeDataProvider,
        ILanguageDataProvider languageDataProvider,
        ISchoolDistrictDataProvider schoolDistrictDataProvider,
        IEmailPermissionDataProvider emailPermissionDataProvider,
        IPhoneTypeDataProvider phoneTypeDataProvider,
        IPayorDataProvider payorDataProvider,
        IPolicyHolderDataProvider policyHolderDataProvider,
        ISuffixDataProvider suffixDataProvider,
        ISecurityQuestionDataProvider securityQuestionDataProvider,
        IRelationshipTypeDataProvider relationshipTypeDataProvider,
        ICollateralTypeDataProvider collateralTypeDataProvider,
        IReceiveCorrespondenceDataProvider receiveCorrespondenceDataProvider,
        ICredentialDataProvider credentialDataProvider,
        IEligibilityTypeDataProvider eligibilityTypeDataProvider,
        IDurationDataProvider durationDataProvider,
        IEligibilityCategoryDataProvider eligibilityCategoryDataProvider,
        IScreeningNameDataProvider screeningNameDataProvider,
        IScreeningResultDataProvider screeningResultDataProvider,
        IScreeningTypeDataProvider screeningTypeDataProvider,
        IServiceCoordinatorDataProvider serviceCoordinatorDataProvider,
        IFSPTypeDataProvider ifspTypeDataProvider,
        IReasonForDelayDataProvider reasonForDelayDataProvider,
        IProgramDataProvider programProvider,
        IFacilityDataProvider facilityProvider,
        IUsersDataProvider usersDataProvider,
        IUserFacilityDataProvider userFacilityDataProvider,
        IReportDataProvider reportDataProvider,
        IBPMethodDataProvider bpMethodDataProvider,
        INoteTypeDataProvider noteTypeDataProvider,
        INoteStatusDataProvider noteStatusDataProvider,
        IAllergyLookupDataProvider allergyLookupDataProvider,
        IAssessmentDataProvider assessmentDataProvider,
        IDrugLookupDataProvider drugLookupDataProvider,
        IMedicalConditionLookupDataProvider medicalConditionLookupDataProvider,
        IFamilyRelationshipDataProvider familyRelationshipDataProvider,
        IReferralStatusDataProvider referralStatusDataProvider,
        IReferralTypeDataProvider referralTypeDataProvider,
        IReferralDispositionTypeDataProvider referralDispositionTypeDataProvider,
        IReferralDispositionOutcomeTypeDataProvider referralDispositionOutcomeTypeDataProvider,
        IReferralResourceTypeDataProvider referralResourceTypeDataProvider,
        IReferralOriginDataProvider referralOriginDataProvider,
        IReferralCategoryDataProvider referralCategoryDataProvider,
        IImmunizationStatusDataProvider immunizationStatusDataProvider,
        IReferralConcernTypeDataProvider referralConcernTypeDataProvider,
        IDischargeReasonDataProvider dischargeReasonDataProvider,
        IReferralOrganizationDataProvider organizationDataProvider,
        IReferralCategorySourceDataProvider referralCategorySourceDataProvider,
        IReferralDispositionStatusTypeDataProvider referralDispositionStatusTypeDataProvider,
        IReferralPriorityDataProvider referralPriorityDataProvider,
        IAppointmentDataProvider appointmentDataProvider,
        IProgramClientIdentifierDataProvider programClientIdentifierDataProvider,
        IServiceItemDataProvider serviceItemDataProvider,
        IAttendanceStatusDataProvider attendanceStatusDataProvider,
        IDeliveryMethodDataProvider deliveryMethodDataProvider,
        IServiceStatusDataProvider serviceStatusDataProvider,
        IServiceLocationDataProvider serviceLocationDataProvider,
        IRecipientCodeDataProvider recipientCodeDataProvider,
        IConversionStatusDataProvider conversionStatusDataProvider,
        IServiceLocationDataProvider locationServiceDataProvider,
        ICallStatusDataProvider callStatusDataProvider,
        ICCPriorityDataProvider cCPriorityDataProvider,
        IProgramUnitDataProvider programUnitDataProvider,
        ISuicidalHomicidalDataProvider sHIDDataProvider,
        ICallTypeDataProvider callTypeProvider,
        IClientStatusDataProvider clientStatusDataProvider,
        IServiceDataProvider serviceDataProvider,
        IBehavioralCategoryDataProvider behavioralCategoryDataProvider,
        ICancelReasonDataProvider cancelReasonDataProvider,
        IDayOfWeekDataProvider dayOfWeekDataProvider,
        IWeekOfMonthDataProvider weekOfMonthDataProvider,
        IMonthDataProvider monthDataProvider,
        IScheduleTypeDataProvider scheduleTypeDataProvider,
        IReferralAgencyDataProvider referralAgencyDataProvider,
        IAdvancedDirectiveTypeDataProvider advancedDirectiveTypeDataProvider,
        IDocumentTypeDataProvider documentTypeDataProvider,
        IResourceTypeDataProvider resourceTypeDataProvider,
        IGroupTypeDataProvider groupTypeDataProvider,
        IGroupServiceDataProvider groupServiceDataProvider,
        IPayorExpirationReasonDataProvider payorExpirationReasonDataProvider,
        IServiceTypeDataProvider serviceTypeDataProvider,
        IServiceConfigTypeDataProvider serviceConfigTypeDataProvider,
        IServiceWorkflowTypeDataProvider serviceWorkflowTypeDataProvider,
        ISignatureStatusDataProvider signatureStatusDataProvider,
        IOrganizationsDataProvider organizationsDataProvider,
        IVoidRecordedServiceReasonDataProvider voidRecordedServiceReasonDataProvider,
        IEntityTypeDataProvider entityTypeDataProvider,
        IServiceRecordingSourceDataProvider serviceRecordingSourceDataProvider,
        IDocumentTypeGroupDataProvider documentTypeGroupDataProvider,
        IUserIdentifierTypeDataProvider userIdentifierTypeDataProvider,
        IConfirmationStatementDataProvider confirmationStatementDataProvider,
        ITrackingFieldDataProvider trackingFieldDataProvider,
        IServicesDataProvider servicesDataProvider,
        ICauseOfDeathDataProvider causeOfDeathDataProvider,
        IAdmissionReasonDataProvider admissionReasonDataProvider,
        IServiceDurationDataProvider serviceDurationDataProvider

        )
        {
            _unitOfWork = unitOfWork;
            _addressTypeDataProvider = addressTypeDataProvider;
            _countyDataProvider = countyDataProvider;
            _stateProvinceDataProvider = stateProvinceDataProvider;
            _mailPermissionTypeDataProvider = mailPermissionTypeDataProvider;
            _dobStatusDataProvider = dobStatusDataProvider;
            _genderDataProvider = genderDataProvider;
            _payorTypeDataProvider = payorTypeDataProvider;
            _referralSourceDataProvider = referralSourceDataProvider;
            _smokingStatusDataProvider = smokingStatusDataProvider;
            _phonePermissionDataProvider = phonePermissionDataProvider;
            _contactTypeDataProvider = contactTypeDataProvider;
            _contactTypeCallCenterDataProvider = contactTypeCallCenterDataProvider;
            _clientTypeDataProvider = clientTypeDataProvider;
            _clientIdentifierTypeDataProvider = clientIdentifierTypeDataProvider;
            _ssnStatusDataProvider = ssnStatusDataProvider;
            _contactMethodDataProvider = contactMethodDataProvider;
            _maritalStatusDataProvider = maritalStatusDataProvider;
            _prefixTypeDataProvider = prefixTypeDataProvider;
            _categoryDataProvider = categoryDataProvider;
            _frequencyDataProvider = frequencyDataProvider;
            _expirationReasonDataProvider = expirationReasonDataProvider;
            _raceDataProvider = raceDataProvider;
            _ethnicityDataProvider = ethnicityDataProvider;
            _legalStatusDataProvider = legalStatusDataProvider;
            _letterOutcomeDataProvider = letterOutcomeDataProvider;
            _livingArrangementDataProvider = livingArrangementDataProvider;
            _educationStatusDataProvider = educationStatusDataProvider;
            _citizenshipDataProvider = citizenshipDataProvider;
            _employmentStatusDataProvider = employmentStatusDataProvider;
            _religionDataProvider = religionDataProvider;
            _veteranStatusDataProvider = veteranStatusDataProvider;
            _presentingProblemTypeDataProvider = presentingProblemTypeDataProvider;
            _languageDataProvider = languageDataProvider;
            _schoolDistrictDataProvider = schoolDistrictDataProvider;
            _emailPermissionDataProvider = emailPermissionDataProvider;
            _phoneTypeDataProvider = phoneTypeDataProvider;
            _payorDataProvider = payorDataProvider;
            _policyHolderDataProvider = policyHolderDataProvider;
            _suffixDataProvider = suffixDataProvider;
            _securityQuestionDataProvider = securityQuestionDataProvider;
            _relationshipTypeDataProvider = relationshipTypeDataProvider;
            _collateralTypeDataProvider = collateralTypeDataProvider;
            _receiveCorrespondenceDataProvider = receiveCorrespondenceDataProvider;
            _credentialDataProvider = credentialDataProvider;
            _eligibilityTypeDataProvider = eligibilityTypeDataProvider;
            _durationDataProvider = durationDataProvider;
            _eligibilityCategoryDataProvider = eligibilityCategoryDataProvider;
            _screeningNameDataProvider = screeningNameDataProvider;
            _screeningTypeDataProvider = screeningTypeDataProvider;
            _serviceCoordinatorDataProvider = serviceCoordinatorDataProvider;
            _screeningResultDataProvider = screeningResultDataProvider;
            _ifspTypeDataProvider = ifspTypeDataProvider;
            _reasonForDelayDataProvider = reasonForDelayDataProvider;
            _programProvider = programProvider;
            _facilityProvider = facilityProvider;
            _usersDataProvider = usersDataProvider;
            _userFacilityDataProvider = userFacilityDataProvider;
            _reportDataProvider = reportDataProvider;
            _bpMethodDataProvider = bpMethodDataProvider;
            _noteTypeDataProvider = noteTypeDataProvider;
            _noteStatusDataProvider = noteStatusDataProvider;
            _allergyLookupDataProvider = allergyLookupDataProvider;
            _assessmentDataProvider = assessmentDataProvider;
            _drugLookupDataProvider = drugLookupDataProvider;
            _medicalConditionLookupDataProvider = medicalConditionLookupDataProvider;
            _familyRelationshipDataProvider = familyRelationshipDataProvider;
            _referralStatusDataProvider = referralStatusDataProvider;
            _referralTypeDataProvider = referralTypeDataProvider;
            _referralResourceTypeDataProvider = referralResourceTypeDataProvider;
            _resourceTypeDataProvider = resourceTypeDataProvider;
            _referralDispositionTypeDataProvider = referralDispositionTypeDataProvider;
            _referralDispositionOutcomeTypeDataProvider = referralDispositionOutcomeTypeDataProvider;
            _referralOriginDataProvider = referralOriginDataProvider;
            _referralCategoryDataProvider = referralCategoryDataProvider;
            _immunizationStatusDataProvider = immunizationStatusDataProvider;
            _referralConcernTypeDataProvider = referralConcernTypeDataProvider;
            _referralPriorityDataProvider = referralPriorityDataProvider;
            _dischargeReasonDataProvider = dischargeReasonDataProvider;
            _organizationDataProvider = organizationDataProvider;
            _referralCategorySourceDataProvider = referralCategorySourceDataProvider;
            _referralDispositionStatusTypeDataProvider = referralDispositionStatusTypeDataProvider;
            _programClientIdentifierDataProvider = programClientIdentifierDataProvider;
            _appointmentDataProvider = appointmentDataProvider;
            _serviceItemDataProvider = serviceItemDataProvider;
            _attendanceStatusDataProvider = attendanceStatusDataProvider;
            _deliveryMethodDataProvider = deliveryMethodDataProvider;
            _serviceStatusDataProvider = serviceStatusDataProvider;
            _serviceLocationDataProvider = serviceLocationDataProvider;
            _recipientCodeDataProvider = recipientCodeDataProvider;
            _conversionStatusDataProvider = conversionStatusDataProvider;
            _locationServiceDataProvider = locationServiceDataProvider;
            _callStatusDataProvider = callStatusDataProvider;
            _cCPriorityDataProvider = cCPriorityDataProvider;
            _programUnitDataProvider = programUnitDataProvider;
            _sHIDDataProvider = sHIDDataProvider;
            _callTypeProvider = callTypeProvider;
            _clientStatusDataProvider = clientStatusDataProvider;
            _behavioralCategoryDataProvider = behavioralCategoryDataProvider;
            _cancelReasonDataProvider = cancelReasonDataProvider;
            _dayOfWeekProvider = dayOfWeekDataProvider;
            _weekOfMonthProvider = weekOfMonthDataProvider;
            _monthProvider = monthDataProvider;
            _scheduleTypeProvider = scheduleTypeDataProvider;
            _referralAgencyDataProvider = referralAgencyDataProvider;
            _advancedDirectiveTypeDataProvider = advancedDirectiveTypeDataProvider;
            _serviceDataProvider = serviceDataProvider;
            _groupTypeDataProvider = groupTypeDataProvider;
            _groupServiceDataProvider = groupServiceDataProvider;
            _documentTypeProvider = documentTypeDataProvider;
            _payorExpirationReasonDataProvider = payorExpirationReasonDataProvider;
            _serviceTypeDataProvider = serviceTypeDataProvider;
            _serviceConfigTypeDataProvider = serviceConfigTypeDataProvider;
            _serviceWorkflowTypeDataProvider = serviceWorkflowTypeDataProvider;
            _signatureStatusDataProvider = signatureStatusDataProvider;
            _voidRecordedServiceReasonDataProvider = voidRecordedServiceReasonDataProvider;
            _organizationsDataProvider = organizationsDataProvider;
            _entityTypeDataProvider = entityTypeDataProvider;
            _serviceRecordingSourceDataProvider = serviceRecordingSourceDataProvider;
            _documentTypeGroupDataProvider = documentTypeGroupDataProvider;
            _userIdentifierTypeDataProvider = userIdentifierTypeDataProvider;
            _confirmationStatementDataProvider = confirmationStatementDataProvider;
            _trackingFieldDataProvider = trackingFieldDataProvider;
            _servicesDataProvider = servicesDataProvider;
            _causeOfDeathDataProvider = causeOfDeathDataProvider;
            _admissionReasonDataProvider = admissionReasonDataProvider;
            _serviceDurationDataProvider = serviceDurationDataProvider;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the lookups.
        /// </summary>
        /// <param name="lookupTypes">The lookup types.</param>
        /// <returns></returns>
        public Response<Dictionary<string, List<dynamic>>> GetLookups(List<LookupType> lookupTypes)
        {
            var response = new Response<Dictionary<string, List<dynamic>>>();
            var returnValue = new Dictionary<string, List<dynamic>>();
            response.DataItems = new List<Dictionary<string, List<dynamic>>>() { };
            response.ResultCode = 0;
            response.ResultMessage = string.Empty;

            //AddressType
            if (lookupTypes.Contains(LookupType.AddressType))
            {
                var addressTypeResults = _addressTypeDataProvider.GetAddressTypes();

                if (addressTypeResults.ResultCode != 0)
                {
                    response.ResultCode = addressTypeResults.ResultCode;
                    response.ResultMessage = addressTypeResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.AddressType.ToString(),
                    addressTypeResults.DataItems.Select(
                        x => new { ID = x.AddressTypeID, Name = x.AddressType }).Cast<object>().ToList());
            }

            //County
            if (lookupTypes.Contains(LookupType.County))
            {
                var countyResults = _countyDataProvider.GetCounty();

                if (countyResults.ResultCode != 0)
                {
                    response.ResultCode = countyResults.ResultCode;
                    response.ResultMessage = countyResults.ResultMessage;
                    return response;
                }
                //TODO: Optionally have a system setting to precache only certain states' counties
                returnValue.Add(LookupType.County.ToString(),
                    countyResults.DataItems.Select(
                        x => new { ID = x.CountyID, StateProvinceID = x.StateProvinceID, Name = x.CountyName }).Cast<object>().ToList());
            }

            //OrganizationCounty
            if (lookupTypes.Contains(LookupType.OrganizationCounty))
            {
                var orgCountyResults = _countyDataProvider.GetOrganizationCounty();

                if (orgCountyResults.ResultCode != 0)
                {
                    response.ResultCode = orgCountyResults.ResultCode;
                    response.ResultMessage = orgCountyResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.OrganizationCounty.ToString(),
                    orgCountyResults.DataItems.Select(
                        x => new { ID = x.CountyID, StateProvinceID = x.StateProvinceID, Name = x.CountyName, StateProvinceName = x.StateProvinceName, OrganizationID = x.OrganizationID }).Cast<object>().ToList());
            }

            //StateProvince
            if (lookupTypes.Contains(LookupType.StateProvince))
            {
                var stateProvinceResults = _stateProvinceDataProvider.GetStateProvince();

                if (stateProvinceResults.ResultCode != 0)
                {
                    response.ResultCode = stateProvinceResults.ResultCode;
                    response.ResultMessage = stateProvinceResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.StateProvince.ToString(),
                    stateProvinceResults.DataItems.Select(
                        x => new { ID = x.StateProvinceID, Code = x.StateProvinceCode, Name = x.StateProvinceName }).Cast<object>().ToList());
            }

            //MailPermission
            if (lookupTypes.Contains(LookupType.MailPermissionType))
            {
                var mailPermissionResults = _mailPermissionTypeDataProvider.GetMailPermissionType();

                if (mailPermissionResults.ResultCode != 0)
                {
                    response.ResultCode = mailPermissionResults.ResultCode;
                    response.ResultMessage = mailPermissionResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.MailPermissionType.ToString(),
                    mailPermissionResults.DataItems.Select(
                        x => new { ID = x.MailPermissionID, Name = x.MailPermission }).Cast<object>().ToList());
            }

            //DOBStatus
            if (lookupTypes.Contains(LookupType.DOBStatus))
            {
                var dobResults = _dobStatusDataProvider.GetDOBStatuses();

                if (dobResults.ResultCode != 0)
                {
                    response.ResultCode = dobResults.ResultCode;
                    response.ResultMessage = dobResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.DOBStatus.ToString(),
                    dobResults.DataItems.Select(
                        x => new { ID = x.DOBStatusID, Name = x.DOBStatus }).Cast<object>().ToList());
            }

            //Gender
            if (lookupTypes.Contains(LookupType.Gender))
            {
                var genderResults = _genderDataProvider.GetGenders();

                if (genderResults.ResultCode != 0)
                {
                    response.ResultCode = genderResults.ResultCode;
                    response.ResultMessage = genderResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.Gender.ToString(),
                    genderResults.DataItems.Select(
                        x => new { ID = x.GenderID, Name = x.Gender }).Cast<object>().ToList());
            }


            //PayorType
            if (lookupTypes.Contains(LookupType.PayorType))
            {
                var payorTypeResults = _payorTypeDataProvider.GetPayorType();

                if (payorTypeResults.ResultCode != 0)
                {
                    response.ResultCode = payorTypeResults.ResultCode;
                    response.ResultMessage = payorTypeResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.PayorType.ToString(),
                    payorTypeResults.DataItems.Select(
                        x => new { ID = x.PayorTypeID, Name = x.PayorType }).Cast<object>().ToList());
            }

            //ReferralSource
            if (lookupTypes.Contains(LookupType.ReferralSource))
            {
                var referralResults = _referralSourceDataProvider.GetReferralSources();

                if (referralResults.ResultCode != 0)
                {
                    response.ResultCode = referralResults.ResultCode;
                    response.ResultMessage = referralResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.ReferralSource.ToString(),
                    referralResults.DataItems.Select(
                        x => new { ID = x.ReferralSourceID, Name = x.ReferralSource }).Cast<object>().ToList());
            }

            //SmokingStatus
            if (lookupTypes.Contains(LookupType.SmokingStatus))
            {
                var smokingResults = _smokingStatusDataProvider.GetSmokingStatuses();

                if (smokingResults.ResultCode != 0)
                {
                    response.ResultCode = smokingResults.ResultCode;
                    response.ResultMessage = smokingResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.SmokingStatus.ToString(),
                    smokingResults.DataItems.Select(
                        x => new { ID = x.SmokingStatusID, Name = x.SmokingStatus }).Cast<object>().ToList());
            }

            //PhonePermission
            if (lookupTypes.Contains(LookupType.PhonePermission))
            {
                var permissionResults = _phonePermissionDataProvider.GetPhonePermissions();

                if (permissionResults.ResultCode != 0)
                {
                    response.ResultCode = permissionResults.ResultCode;
                    response.ResultMessage = permissionResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.PhonePermission.ToString(),
                    permissionResults.DataItems.Select(
                        x => new { ID = x.PhonePermissionID, Name = x.PhonePermission }).Cast<object>().ToList());
            }

            //ContactType
            if (lookupTypes.Contains(LookupType.ContactType))
            {
                var contactResults = _contactTypeDataProvider.GetContactTypes();

                if (contactResults.ResultCode != 0)
                {
                    response.ResultCode = contactResults.ResultCode;
                    response.ResultMessage = contactResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.ContactType.ToString(),
                    contactResults.DataItems.Select(
                        x => new { ID = x.ContactTypeID, Name = x.ContactType }).Cast<object>().ToList());
            }

            //ContactTypeCallCenter
            if (lookupTypes.Contains(LookupType.ContactTypeCallCenter))
            {
                var contactResults = _contactTypeCallCenterDataProvider.GetContactTypes();

                if (contactResults.ResultCode != 0)
                {
                    response.ResultCode = contactResults.ResultCode;
                    response.ResultMessage = contactResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.ContactTypeCallCenter.ToString(),
                    contactResults.DataItems.Select(
                        x => new { ID = x.ContactTypeID, Name = x.ContactType }).Cast<object>().ToList());
            }

            //ClientType
            if (lookupTypes.Contains(LookupType.ClientType))
            {
                var clientResults = _clientTypeDataProvider.GetClientTypes();

                if (clientResults.ResultCode != 0)
                {
                    response.ResultCode = clientResults.ResultCode;
                    response.ResultMessage = clientResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.ClientType.ToString(),
                    clientResults.DataItems.Select(
                        x => new { ID = x.ClientTypeID, Name = x.ClientType, Division = x.Division,RegistrationState= x.RegistrationState, OrganizationDetailID = x.OrganizationDetailID }).Cast<object>().ToList());
            }

            //ClientIdentifierType
            if (lookupTypes.Contains(LookupType.ClientIdentifierType))
            {
                var clientIdentifierTypeResults = _clientIdentifierTypeDataProvider.GetClientIdentifierTypes();

                if (clientIdentifierTypeResults.ResultCode != 0)
                {
                    response.ResultCode = clientIdentifierTypeResults.ResultCode;
                    response.ResultMessage = clientIdentifierTypeResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.ClientIdentifierType.ToString(),
                    clientIdentifierTypeResults.DataItems.Select(
                        x => new { ID = x.ClientIdentifierTypeID, Name = x.ClientIdentifierType }).Cast<object>().ToList());
            }
            //SSNStatus
            if (lookupTypes.Contains(LookupType.SSNStatus))
            {
                var ssnStatusResults = _ssnStatusDataProvider.GetSSNStatuses();

                if (ssnStatusResults.ResultCode != 0)
                {
                    response.ResultCode = ssnStatusResults.ResultCode;
                    response.ResultMessage = ssnStatusResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.SSNStatus.ToString(),
                    ssnStatusResults.DataItems.Select(
                        x => new { ID = x.SSNStatusID, Name = x.SSNStatus }).Cast<object>().ToList());
            }

            //ContactMethod
            if (lookupTypes.Contains(LookupType.ContactMethod))
            {
                var contactMethodResults = _contactMethodDataProvider.GetContactMethods();

                if (contactMethodResults.ResultCode != 0)
                {
                    response.ResultCode = contactMethodResults.ResultCode;
                    response.ResultMessage = contactMethodResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ContactMethod.ToString(),
                    contactMethodResults.DataItems.Select(
                        x => new { ID = x.ContactMethodID, Name = x.ContactMethod, IsSystem = x.IsSystem }).Cast<object>().ToList());
            }

            //MaritalStatus
            if (lookupTypes.Contains(LookupType.MaritalStatus))
            {
                var maritalStatusResults = _maritalStatusDataProvider.GetMaritalStatuses();

                if (maritalStatusResults.ResultCode != 0)
                {
                    response.ResultCode = maritalStatusResults.ResultCode;
                    response.ResultMessage = maritalStatusResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.MaritalStatus.ToString(),
                    maritalStatusResults.DataItems.Select(
                        x => new { ID = x.MaritalStatusID, Name = x.MaritalStatus }).Cast<object>().ToList());
            }

            //PrefixType
            if (lookupTypes.Contains(LookupType.PrefixType))
            {
                var prefixStatusResults = _prefixTypeDataProvider.GetPrefixType();

                if (prefixStatusResults.ResultCode != 0)
                {
                    response.ResultCode = prefixStatusResults.ResultCode;
                    response.ResultMessage = prefixStatusResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.PrefixType.ToString(),
                    prefixStatusResults.DataItems.Select(
                        x => new { ID = x.TitleID, Name = x.Title }).Cast<object>().ToList());
            }

            //Suffix
            if (lookupTypes.Contains(LookupType.Suffix))
            {
                var suffixResults = _suffixDataProvider.GetSuffixes();

                if (suffixResults.ResultCode != 0)
                {
                    response.ResultCode = suffixResults.ResultCode;
                    response.ResultMessage = suffixResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.Suffix.ToString(),
                    suffixResults.DataItems.Select(
                        x => new { ID = x.SuffixID, Name = x.Suffix }).Cast<object>().ToList());
            }

            //CategoryType
            if (lookupTypes.Contains(LookupType.CategoryType))
            {
                var categoryTypeResults = _categoryDataProvider.GetCategoriesType();

                if (categoryTypeResults.ResultCode != 0)
                {
                    response.ResultCode = categoryTypeResults.ResultCode;
                    response.ResultMessage = categoryTypeResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.CategoryType.ToString(),
                    categoryTypeResults.DataItems.Select(
                        x => new { ID = x.CategoryTypeID, Name = x.CategoryType }).Cast<object>().ToList());
            }

            //Category
            if (lookupTypes.Contains(LookupType.Category))
            {
                var categoryResults = _categoryDataProvider.GetCategories();

                if (categoryResults.ResultCode != 0)
                {
                    response.ResultCode = categoryResults.ResultCode;
                    response.ResultMessage = categoryResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.Category.ToString(),
                    categoryResults.DataItems.Select(
                        x => new { ID = x.CategoryID, Name = x.Category, CategoryTypeID = x.CategoryTypeID }).Cast<object>().ToList());
            }

            //Frequency
            if (lookupTypes.Contains(LookupType.FinanceFrequency))
            {
                var frequencyResults = _frequencyDataProvider.GetFrequencies();

                if (frequencyResults.ResultCode != 0)
                {
                    response.ResultCode = frequencyResults.ResultCode;
                    response.ResultMessage = frequencyResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.FinanceFrequency.ToString(),
                    frequencyResults.DataItems.Select(
                        x => new { ID = x.FinanceFrequencyID, Name = x.FinanceFrequency, Type = x.FinanceFrequencyType, FrequencyFactor = x.FrequencyFactor }).Cast<object>().ToList());
            }

            //Expiration Reason
            if (lookupTypes.Contains(LookupType.ExpirationReason))
            {
                var expirationReasonResults = _expirationReasonDataProvider.GetExpirationReasons();

                if (expirationReasonResults.ResultCode != 0)
                {
                    response.ResultCode = expirationReasonResults.ResultCode;
                    response.ResultMessage = expirationReasonResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ExpirationReason.ToString(),
                    expirationReasonResults.DataItems.Select(
                        x => new { ID = x.ExpirationReasonID, Name = x.ExpirationReason }).Cast<object>().ToList());
            }

            //Expiration Reason
            if (lookupTypes.Contains(LookupType.AssessmentExpirationReason))
            {
                var expirationReasonResults = _expirationReasonDataProvider.GetAssessmentExpirationReasons();

                if (expirationReasonResults.ResultCode != 0)
                {
                    response.ResultCode = expirationReasonResults.ResultCode;
                    response.ResultMessage = expirationReasonResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.AssessmentExpirationReason.ToString(),
                    expirationReasonResults.DataItems.Select(
                        x => new { ID = x.AssessmentExpirationReasonID, Name = x.AssessmentExpirationReason }).Cast<object>().ToList());
            }

            //Other ID Expiration Reasons
            if (lookupTypes.Contains(LookupType.OtherIDExpReasons))
            {
                var expirationReasonResults = _expirationReasonDataProvider.GetOtherIDExpirationReasons();

                if (expirationReasonResults.ResultCode != 0)
                {
                    response.ResultCode = expirationReasonResults.ResultCode;
                    response.ResultMessage = expirationReasonResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.OtherIDExpReasons.ToString(),
                    expirationReasonResults.DataItems.Select(
                        x => new { ID = x.ExpirationReasonID, Name = x.ExpirationReason }).Cast<object>().ToList());
            }

            //Race
            if (lookupTypes.Contains(LookupType.Race))
            {
                var raceResults = _raceDataProvider.GetRaces();

                if (raceResults.ResultCode != 0)
                {
                    response.ResultCode = raceResults.ResultCode;
                    response.ResultMessage = raceResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.Race.ToString(),
                    raceResults.DataItems.Select(
                        x => new { ID = x.RaceID, Name = x.Race }).Cast<object>().ToList());
            }

            //Ethnicity
            if (lookupTypes.Contains(LookupType.Ethnicity))
            {
                var ethnicityResults = _ethnicityDataProvider.GetEthnicities();

                if (ethnicityResults.ResultCode != 0)
                {
                    response.ResultCode = ethnicityResults.ResultCode;
                    response.ResultMessage = ethnicityResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.Ethnicity.ToString(),
                    ethnicityResults.DataItems.Select(
                        x => new { ID = x.EthnicityID, Name = x.Ethnicity }).Cast<object>().ToList());
            }

            //Primary Language and Secondary Language
            if (lookupTypes.Contains(LookupType.Language))
            {
                var languageResults = _languageDataProvider.GetLanguages();

                if (languageResults.ResultCode != 0)
                {
                    response.ResultCode = languageResults.ResultCode;
                    response.ResultMessage = languageResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.Language.ToString(),
                    languageResults.DataItems.Select(
                        x => new { ID = x.LanguageID, Name = x.LanguageName }).Cast<object>().ToList());
            }
            //LegalStatus
            if (lookupTypes.Contains(LookupType.LegalStatus))
            {
                var legalStatusResults = _legalStatusDataProvider.GetLegalStatuses();

                if (legalStatusResults.ResultCode != 0)
                {
                    response.ResultCode = legalStatusResults.ResultCode;
                    response.ResultMessage = legalStatusResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.LegalStatus.ToString(),
                    legalStatusResults.DataItems.Select(
                        x => new { ID = x.LegalStatusID, Name = x.LegalStatus }).Cast<object>().ToList());
            }

            //LetterOutcome
            if (lookupTypes.Contains(LookupType.LetterOutcome))
            {
                var letterOutcomeResults = _letterOutcomeDataProvider.GetLetterOutcome();

                if (letterOutcomeResults.ResultCode != 0)
                {
                    response.ResultCode = letterOutcomeResults.ResultCode;
                    response.ResultMessage = letterOutcomeResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.LetterOutcome.ToString(),
                    letterOutcomeResults.DataItems.Select(
                        x => new { ID = x.LetterOutcomeID, Name = x.LetterOutcome }).Cast<object>().ToList());
            }

            //Living Arrangement
            if (lookupTypes.Contains(LookupType.LivingArrangement))
            {
                var livingArrangementResults = _livingArrangementDataProvider.GetLivingArrangements();

                if (livingArrangementResults.ResultCode != 0)
                {
                    response.ResultCode = livingArrangementResults.ResultCode;
                    response.ResultMessage = livingArrangementResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.LivingArrangement.ToString(),
                    livingArrangementResults.DataItems.Select(
                        x => new { ID = x.LivingArrangementID, Name = x.LivingArrangement }).Cast<object>().ToList());
            }

            //Citizenship
            if (lookupTypes.Contains(LookupType.Citizenship))
            {
                var citizenshipResults = _citizenshipDataProvider.GetCitizenships();

                if (citizenshipResults.ResultCode != 0)
                {
                    response.ResultCode = citizenshipResults.ResultCode;
                    response.ResultMessage = citizenshipResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.Citizenship.ToString(),
                    citizenshipResults.DataItems.Select(
                        x => new { ID = x.CitizenshipID, Name = x.Citizenship }).Cast<object>().ToList());
            }

            //EducationStatus
            if (lookupTypes.Contains(LookupType.EducationStatus))
            {
                var educationResults = _educationStatusDataProvider.GetEducationStatuses();

                if (educationResults.ResultCode != 0)
                {
                    response.ResultCode = educationResults.ResultCode;
                    response.ResultMessage = educationResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.EducationStatus.ToString(),
                    educationResults.DataItems.Select(
                        x => new { ID = x.EducationStatusID, Name = x.EducationStatus }).Cast<object>().ToList());
            }

            //Employment Status
            if (lookupTypes.Contains(LookupType.EmploymentStatus))
            {
                var employmentStatusResults = _employmentStatusDataProvider.GetEmploymentStatuses();

                if (employmentStatusResults.ResultCode != 0)
                {
                    response.ResultCode = employmentStatusResults.ResultCode;
                    response.ResultMessage = employmentStatusResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.EmploymentStatus.ToString(),
                    employmentStatusResults.DataItems.Select(
                        x => new { ID = x.EmploymentStatusID, Name = x.EmploymentStatus }).Cast<object>().ToList());
            }

            //Veteran Status
            if (lookupTypes.Contains(LookupType.VeteranStatus))
            {
                var veteranStatusResults = _veteranStatusDataProvider.GetVeteranStatuses();

                if (veteranStatusResults.ResultCode != 0)
                {
                    response.ResultCode = veteranStatusResults.ResultCode;
                    response.ResultMessage = veteranStatusResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.VeteranStatus.ToString(),
                    veteranStatusResults.DataItems.Select(
                        x => new { ID = x.VeteranStatusID, Name = x.VeteranStatus }).Cast<object>().ToList());
            }

            //PresentingProblem Type
            if (lookupTypes.Contains(LookupType.PresentingProblemType))
            {
                var presentingProblemTypeResults = _presentingProblemTypeDataProvider.GetPresentingProblemTypes();

                if (presentingProblemTypeResults.ResultCode != 0)
                {
                    response.ResultCode = presentingProblemTypeResults.ResultCode;
                    response.ResultMessage = presentingProblemTypeResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.PresentingProblemType.ToString(),
                    presentingProblemTypeResults.DataItems.Select(
                        x => new { ID = x.PresentingProblemTypeID, Name = x.PresentingProblemType }).Cast<object>().ToList());
            }

            //Religion
            if (lookupTypes.Contains(LookupType.Religion))
            {
                var religionResults = _religionDataProvider.GetReligions();

                if (religionResults.ResultCode != 0)
                {
                    response.ResultCode = religionResults.ResultCode;
                    response.ResultMessage = religionResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.Religion.ToString(),
                    religionResults.DataItems.Select(
                        x => new { ID = x.ReligionID, Name = x.Religion }).Cast<object>().ToList());
            }

            //School District
            if (lookupTypes.Contains(LookupType.SchoolDistrict))
            {
                var schoolDistrcitResults = _schoolDistrictDataProvider.GetSchoolDistricts();

                if (schoolDistrcitResults.ResultCode != 0)
                {
                    response.ResultCode = schoolDistrcitResults.ResultCode;
                    response.ResultMessage = schoolDistrcitResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.SchoolDistrict.ToString(),
                    schoolDistrcitResults.DataItems.Select(
                        x => new { ID = x.SchoolDistrictID, Name = x.SchoolDistrictName }).Cast<object>().ToList());
            }

            //Email Permission
            if (lookupTypes.Contains(LookupType.EmailPermission))
            {
                var emailPermissionResults = _emailPermissionDataProvider.GetEmailPermissions();

                if (emailPermissionResults.ResultCode != 0)
                {
                    response.ResultCode = emailPermissionResults.ResultCode;
                    response.ResultMessage = emailPermissionResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.EmailPermission.ToString(),
                    emailPermissionResults.DataItems.Select(
                        x => new { ID = x.EmailPermissionID, Name = x.EmailPermission }).Cast<object>().ToList());
            }

            //PhoneType
            if (lookupTypes.Contains(LookupType.PhoneType))
            {
                var phoneTypeResults = _phoneTypeDataProvider.GetPhoneType();

                if (phoneTypeResults.ResultCode != 0)
                {
                    response.ResultCode = phoneTypeResults.ResultCode;
                    response.ResultMessage = phoneTypeResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.PhoneType.ToString(),
                    phoneTypeResults.DataItems.Select(
                        x => new { ID = x.PhoneTypeID, Name = x.PhoneType }).Cast<object>().ToList());
            }

            //Payor
            if (lookupTypes.Contains(LookupType.Payor))
            {
                var payorStatusResult = _payorDataProvider.GetPayors();

                if (payorStatusResult.ResultCode != 0)
                {
                    response.ResultCode = payorStatusResult.ResultCode;
                    response.ResultMessage = payorStatusResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.Payor.ToString(),
                    payorStatusResult.DataItems.Select(
                    x => new { ID = x.PayorID, Name = x.PayorName, PayorCode = x.PayorCode }).Cast<object>().ToList());
            }

            //PolicyHolder
            if (lookupTypes.Contains(LookupType.PolicyHolder))
            {
                var policyHolderStatusResult = _policyHolderDataProvider.GetPolicyHolders();

                if (policyHolderStatusResult.ResultCode != 0)
                {
                    response.ResultCode = policyHolderStatusResult.ResultCode;
                    response.ResultMessage = policyHolderStatusResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.PolicyHolder.ToString(),
                    policyHolderStatusResult.DataItems.Select(
                    x => new { ID = x.PolicyHolderID, Name = x.PolicyHolder }).Cast<object>().ToList());
            }

            //Security Question
            if (lookupTypes.Contains(LookupType.SecurityQuestion))
            {
                var securityQuestionResult = _securityQuestionDataProvider.GetSecurityQuestions();

                if (securityQuestionResult.ResultCode != 0)
                {
                    response.ResultCode = securityQuestionResult.ResultCode;
                    response.ResultMessage = securityQuestionResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.SecurityQuestion.ToString(),
                    securityQuestionResult.DataItems.Select(
                    x => new { ID = x.SecurityQuestionID, Name = x.Question }).Cast<object>().ToList());
            }

            //RelationshipType
            if (lookupTypes.Contains(LookupType.RelationshipType))
            {
                var relationshipTypeResult = _relationshipTypeDataProvider.GetRelationshipTypeDetails();

                if (relationshipTypeResult.ResultCode != 0)
                {
                    response.ResultCode = relationshipTypeResult.ResultCode;
                    response.ResultMessage = relationshipTypeResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.RelationshipType.ToString(),
                    relationshipTypeResult.DataItems.Select(
                    x => new { ID = x.RelationshipTypeID, Name = x.RelationshipType, RelationshipGroupID = x.RelationshipGroupID }).Cast<object>().ToList());
            }

            //CollateralType
            if (lookupTypes.Contains(LookupType.CollateralType))
            {
                var collateralTypeResult = _collateralTypeDataProvider.GetCollateralType();

                if (collateralTypeResult.ResultCode != 0)
                {
                    response.ResultCode = collateralTypeResult.ResultCode;
                    response.ResultMessage = collateralTypeResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.CollateralType.ToString(),
                    collateralTypeResult.DataItems.Select(
                    x => new { ID = x.CollateralTypeID, Name = x.CollateralType, RelationshipGroupID = x.RelationshipGroupID }).Cast<object>().ToList());
            }

            //Receive Correspondence
            if (lookupTypes.Contains(LookupType.ReceiveCorrespondence))
            {
                var receiveCorrespondenceDataProvider = _receiveCorrespondenceDataProvider.GetReceiveCorrespondence();

                if (receiveCorrespondenceDataProvider.ResultCode != 0)
                {
                    response.ResultCode = receiveCorrespondenceDataProvider.ResultCode;
                    response.ResultMessage = receiveCorrespondenceDataProvider.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ReceiveCorrespondence.ToString(),
                    receiveCorrespondenceDataProvider.DataItems.Select(
                    x => new { ID = x.ReceiveCorrespondenceID, Name = x.ReceiveCorrespondence }).Cast<object>().ToList());
            }

            //Credentials
            if (lookupTypes.Contains(LookupType.Credential))
            {
                var credentialResults = _credentialDataProvider.GetCredentials();

                if (credentialResults.ResultCode != 0)
                {
                    response.ResultCode = credentialResults.ResultCode;
                    response.ResultMessage = credentialResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.Credential.ToString(),
                    credentialResults.DataItems.Select(
                        x => new
                        {
                            ID = x.CredentialID,
                            Name = x.CredentialName,
                            CredentialID = x.CredentialID,
                            CredentialName = x.CredentialName,
                            CredentialCode = x.CredentialCode,
                            CredentialAbbreviation = x.CredentialAbbreviation,
                            LicenseRequired = x.LicenseRequired,
                            EffectiveDate = x.EffectiveDate,
                            ExpirationDate = x.ExpirationDate,
                            IsInternal = x.IsInternal,
                        }).Cast<object>().ToList());
            }

            //Eligibility Type
            if (lookupTypes.Contains(LookupType.EligibilityType))
            {
                var eligibilityTypeResults = _eligibilityTypeDataProvider.GetEligibilityTypes();

                if (eligibilityTypeResults.ResultCode != 0)
                {
                    response.ResultCode = eligibilityTypeResults.ResultCode;
                    response.ResultMessage = eligibilityTypeResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.EligibilityType.ToString(),
                   eligibilityTypeResults.DataItems.Select(
                       x => new { ID = x.EligibilityTypeID, Name = x.EligibilityType }).Cast<object>().ToList());
            }

            //Eligibility Duration
            if (lookupTypes.Contains(LookupType.EligibilityDuration))
            {
                var durationResults = _durationDataProvider.GetQuarterDurations();

                if (durationResults.ResultCode != 0)
                {
                    response.ResultCode = durationResults.ResultCode;
                    response.ResultMessage = durationResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.EligibilityDuration.ToString(),
                   durationResults.DataItems.Select(
                       x => new { ID = x.EligibilityDurationID, Name = x.EligibilityDuration }).Cast<object>().ToList());
            }

            //Eligibility Category
            if (lookupTypes.Contains(LookupType.EligibilityCategory))
            {
                var eligibilityCategoryResults = _eligibilityCategoryDataProvider.GetEligibilityCategories();

                if (eligibilityCategoryResults.ResultCode != 0)
                {
                    response.ResultCode = eligibilityCategoryResults.ResultCode;
                    response.ResultMessage = eligibilityCategoryResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.EligibilityCategory.ToString(),
                   eligibilityCategoryResults.DataItems.Select(
                       x => new { ID = x.EligibilityCategoryID, Name = x.EligibilityCategory }).Cast<object>().ToList());
            }

            //Screening Name
            if (lookupTypes.Contains(LookupType.ScreeningName))
            {
                var screeningNameDataProvider = _screeningNameDataProvider.GetScreeningName();

                if (screeningNameDataProvider.ResultCode != 0)
                {
                    response.ResultCode = screeningNameDataProvider.ResultCode;
                    response.ResultMessage = screeningNameDataProvider.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ScreeningName.ToString(),
                    screeningNameDataProvider.DataItems.Select(
                    x => new { ID = x.ScreeningNameID, Name = x.ScreeningName, ScreeningTypeID = x.ScreeningTypeID }).Cast<object>().ToList());
            }

            //Screening Type
            if (lookupTypes.Contains(LookupType.ScreeningType))
            {
                var screeningTypeDataProvider = _screeningTypeDataProvider.GetScreeningTypes();

                if (screeningTypeDataProvider.ResultCode != 0)
                {
                    response.ResultCode = screeningTypeDataProvider.ResultCode;
                    response.ResultMessage = screeningTypeDataProvider.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ScreeningType.ToString(),
                    screeningTypeDataProvider.DataItems.Select(
                    x => new { ID = x.ScreeningTypeID, Name = x.ScreeningType }).Cast<object>().ToList());
            }

            //Service Coordinator
            if (lookupTypes.Contains(LookupType.ServiceCoordinator))
            {
                var serviceCoordinatorDataProvider = _serviceCoordinatorDataProvider.GetServiceCoordinators();

                if (serviceCoordinatorDataProvider.ResultCode != 0)
                {
                    response.ResultCode = serviceCoordinatorDataProvider.ResultCode;
                    response.ResultMessage = serviceCoordinatorDataProvider.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ServiceCoordinator.ToString(),
                    serviceCoordinatorDataProvider.DataItems.Select(
                    x => new { ID = x.SpecialistID, Name = x.Specialist }).Cast<object>().ToList());
            }

            //Screening Result
            if (lookupTypes.Contains(LookupType.ScreeningResult))
            {
                var screeningResultDataProvider = _screeningResultDataProvider.GetScreeningResults();

                if (screeningResultDataProvider.ResultCode != 0)
                {
                    response.ResultCode = screeningResultDataProvider.ResultCode;
                    response.ResultMessage = screeningResultDataProvider.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ScreeningResult.ToString(),
                    screeningResultDataProvider.DataItems.Select(
                    x => new { ID = x.ScreeningResultID, Name = x.ScreeningResult }).Cast<object>().ToList());
            }

            //IFSP Type
            if (lookupTypes.Contains(LookupType.IFSPType))
            {
                var ifspTypeDataProvider = _ifspTypeDataProvider.GetIFSPType();
                if (ifspTypeDataProvider.ResultCode != 0)
                {
                    response.ResultCode = ifspTypeDataProvider.ResultCode;
                    response.ResultMessage = ifspTypeDataProvider.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.IFSPType.ToString(),
                    ifspTypeDataProvider.DataItems.Select(
                        x => new { ID = x.IFSPTypeID, Name = x.IFSPType }).Cast<object>().ToList());
            }

            //IFSP Type
            if (lookupTypes.Contains(LookupType.ReasonForDelay))
            {
                var reasonForDelayDataProvider = _reasonForDelayDataProvider.GetReasonForDelay();
                if (reasonForDelayDataProvider.ResultCode != 0)
                {
                    response.ResultCode = reasonForDelayDataProvider.ResultCode;
                    response.ResultMessage = reasonForDelayDataProvider.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ReasonForDelay.ToString(),
                    reasonForDelayDataProvider.DataItems.Select(
                        x => new { ID = x.ReasonForDelayID, Name = x.ReasonForDelay }).Cast<object>().ToList());
            }

            //Program
            if (lookupTypes.Contains(LookupType.Program))
            {
                var programResults = _programProvider.GetProgram();

                if (programResults.ResultCode != 0)
                {
                    response.ResultCode = programResults.ResultCode;
                    response.ResultMessage = programResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.Program.ToString(),
                    programResults.DataItems.Select(
                        x => new { ID = x.ProgramID, Name = x.ProgramName }).Cast<object>().ToList());
            }

            //Facility
            if (lookupTypes.Contains(LookupType.Facility))
            {
                var facilityResults = _facilityProvider.GetFacility();

                if (facilityResults.ResultCode != 0)
                {
                    response.ResultCode = facilityResults.ResultCode;
                    response.ResultMessage = facilityResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.Facility.ToString(),
                    facilityResults.DataItems.Select(
                        x => new { ID = x.FacilityID, Name = x.FacilityName }).Cast<object>().ToList());
            }

            //Users
            if (lookupTypes.Contains(LookupType.Users))
            {
                var usersResult = _usersDataProvider.GetUsers();

                if (usersResult.ResultCode != 0)
                {
                    response.ResultCode = usersResult.ResultCode;
                    response.ResultMessage = usersResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.Users.ToString(),
                    usersResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.UserID,
                                x.PhoneID,
                                x.Number,
                                x.Name,
                                x.FacilityID,
                                x.ProgramID,
                                x.CredentialID,
                                x.CredentialAbbreviation,
                                x.IsActive
                            }).Cast<object>().ToList());
            }

            //Reports
            if (lookupTypes.Contains(LookupType.Reports))
            {
                var reportsResult = _reportDataProvider.GetReportsByType(ReportType.PDFPrintable.ToString());
                if (reportsResult.ResultCode != 0)
                {
                    response.ResultCode = reportsResult.ResultCode;
                    response.ResultMessage = reportsResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.Reports.ToString(),
                    reportsResult.DataItems.Select(
                        x => new
                        {
                            Name = x.ReportName,
                            Model = Encoding.Unicode.GetString(x.ReportModel)
                        }).Cast<object>().ToList());
            }

            //BP Methods
            if (lookupTypes.Contains(LookupType.BPMethod))
            {
                var bpMethodResult = _bpMethodDataProvider.GetBPMethods();
                if (bpMethodResult.ResultCode != 0)
                {
                    response.ResultCode = bpMethodResult.ResultCode;
                    response.ResultMessage = bpMethodResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.BPMethod.ToString(),
                  bpMethodResult.DataItems.Select(
                      x => new { ID = x.BPMethodID, Name = x.BPMethod }).Cast<object>().ToList());
            }

            //ReferenceNoteType
            if (lookupTypes.Contains(LookupType.ReferenceNoteType))
            {
                var noteTypeResult = _noteTypeDataProvider.GetNoteTypes();
                if (noteTypeResult.ResultCode != 0)
                {
                    response.ResultCode = noteTypeResult.ResultCode;
                    response.ResultMessage = noteTypeResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ReferenceNoteType.ToString(),
                  noteTypeResult.DataItems.Select(
                      x => new { ID = x.NoteTypeID, Name = x.NoteType }).Cast<object>().ToList());
            }

            //Note Type
            if (lookupTypes.Contains(LookupType.NoteType))
            {
                var noteTypeResult = _noteTypeDataProvider.GetNoteType();
                if (noteTypeResult.ResultCode != 0)
                {
                    response.ResultCode = noteTypeResult.ResultCode;
                    response.ResultMessage = noteTypeResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.NoteType.ToString(),
                  noteTypeResult.DataItems.Select(
                      x => new { ID = x.NoteTypeID, Name = x.NoteType }).Cast<object>().ToList());
            }

            //Note Status
            if (lookupTypes.Contains(LookupType.NoteStatus))
            {
                var noteStatusResult = _noteStatusDataProvider.GetNoteStatus();
                if (noteStatusResult.ResultCode != 0)
                {
                    response.ResultCode = noteStatusResult.ResultCode;
                    response.ResultMessage = noteStatusResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.NoteStatus.ToString(),
                  noteStatusResult.DataItems.Select(
                      x => new { ID = x.DocumentStatusID, Name = x.DocumentStatus }).Cast<object>().ToList());
            }

            //Allergies
            if (lookupTypes.Contains(LookupType.Allergy))
            {
                var allergyResults = _allergyLookupDataProvider.GetAllergies();

                if (allergyResults.ResultCode != 0)
                {
                    response.ResultCode = allergyResults.ResultCode;
                    response.ResultMessage = allergyResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.Allergy.ToString(),
                    allergyResults.DataItems.Select(
                        x => new { ID = x.AllergyID, Name = x.AllergyName }).Cast<object>().ToList());
            }

            //Allergy Symptoms
            if (lookupTypes.Contains(LookupType.AllergySymptom))
            {
                var allergyResults = _allergyLookupDataProvider.GetAllergySymptoms();

                if (allergyResults.ResultCode != 0)
                {
                    response.ResultCode = allergyResults.ResultCode;
                    response.ResultMessage = allergyResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.AllergySymptom.ToString(),
                    allergyResults.DataItems.Select(
                        x => new { ID = x.AllergySymptomID, Name = x.AllergySymptom }).Cast<object>().ToList());
            }

            //Allergy Types
            if (lookupTypes.Contains(LookupType.AllergyType))
            {
                var allergyResults = _allergyLookupDataProvider.GetAllergyTypes();

                if (allergyResults.ResultCode != 0)
                {
                    response.ResultCode = allergyResults.ResultCode;
                    response.ResultMessage = allergyResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.AllergyType.ToString(),
                    allergyResults.DataItems.Select(
                        x => new { ID = x.AllergyTypeID, Name = x.AllergyType }).Cast<object>().ToList());
            }

            //Allergy Severities
            if (lookupTypes.Contains(LookupType.AllergySeverity))
            {
                var allergyResults = _allergyLookupDataProvider.GetAllergySeverities();

                if (allergyResults.ResultCode != 0)
                {
                    response.ResultCode = allergyResults.ResultCode;
                    response.ResultMessage = allergyResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.AllergySeverity.ToString(),
                    allergyResults.DataItems.Select(
                        x => new { ID = x.AllergySeverityID, Name = x.AllergySeverity }).Cast<object>().ToList());
            }

            //ClinicalAssessment
            if (lookupTypes.Contains(LookupType.ClinicalAssessment))
            {
                var assessmentResults = _assessmentDataProvider.GetAssessment(5);// 5 for clinical

                if (assessmentResults.ResultCode != 0)
                {
                    response.ResultCode = assessmentResults.ResultCode;
                    response.ResultMessage = assessmentResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ClinicalAssessment.ToString(),
                    assessmentResults.DataItems.Select(
                        x => new { ID = x.AssessmentID, Name = x.AssessmentName }).Cast<object>().ToList());
            }

            //Assessment
            if (lookupTypes.Contains(LookupType.Assessment))
            {
                var assessmentResults = _assessmentDataProvider.GetAssessmentList();// 5 for clinical

                if (assessmentResults.ResultCode != 0)
                {
                    response.ResultCode = assessmentResults.ResultCode;
                    response.ResultMessage = assessmentResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.Assessment.ToString(),
                    assessmentResults.DataItems.Select(
                        x => new { ID = x.AssessmentID, Name = x.AssessmentName }).Cast<object>().ToList());
            }

            //Drugs
            if (lookupTypes.Contains(LookupType.Drug))
            {
                var drugResults = _drugLookupDataProvider.GetDrugs();

                if (drugResults.ResultCode != 0)
                {
                    response.ResultCode = drugResults.ResultCode;
                    response.ResultMessage = drugResults.ResultMessage;
                    return response;
                }

                List<object> drugList = new List<object>();
                //TODO: Implement type-ahead search and tag _certain_ drugs for full offline caching
                foreach (DrugModel d in drugResults.DataItems.Take(10000))
                {
                    drugList.Add(d.ProductName.Length > 75
                        ? new { ID = d.DrugID, Name = d.ProductName.Substring(0, 75) + "..." }
                        : new { ID = d.DrugID, Name = d.ProductName });
                }

                returnValue.Add(LookupType.Drug.ToString(), drugList);
            }

            if (lookupTypes.Contains(LookupType.MedicalCondition))
            {
                var medicalConditionResults = _medicalConditionLookupDataProvider.GetMedicalConditions();

                if (medicalConditionResults.ResultCode != 0)
                {
                    response.ResultCode = medicalConditionResults.ResultCode;
                    response.ResultMessage = medicalConditionResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.MedicalCondition.ToString(),
                    medicalConditionResults.DataItems.Select(
                        x => new { ID = x.MedicalConditionID, Name = x.MedicalCondition }).Cast<object>().ToList());
            }

            //Family Relationships
            if (lookupTypes.Contains(LookupType.FamilyRelationship))
            {
                var familyRelationshipResults = _familyRelationshipDataProvider.GetFamilyRelationships();

                if (familyRelationshipResults.ResultCode != 0)
                {
                    response.ResultCode = familyRelationshipResults.ResultCode;
                    response.ResultMessage = familyRelationshipResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.FamilyRelationship.ToString(),
                    familyRelationshipResults.DataItems.Select(
                        x => new { ID = x.RelationshipTypeID, Name = x.RelationshipType }).Cast<object>().ToList());
            }

            //Referral Status
            if (lookupTypes.Contains(LookupType.ReferralStatus))
            {
                var referralStatusResults = _referralStatusDataProvider.GetReferralStatuses();

                if (referralStatusResults.ResultCode != 0)
                {
                    response.ResultCode = referralStatusResults.ResultCode;
                    response.ResultMessage = referralStatusResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ReferralStatus.ToString(),
                    referralStatusResults.DataItems.Select(
                        x => new { ID = x.ReferralStatusID, Name = x.ReferralStatus }).Cast<object>().ToList());
            }

            //Referral Type
            if (lookupTypes.Contains(LookupType.ReferralType))
            {
                var referralTypeResults = _referralTypeDataProvider.GetReferralType();

                if (referralTypeResults.ResultCode != 0)
                {
                    response.ResultCode = referralTypeResults.ResultCode;
                    response.ResultMessage = referralTypeResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ReferralType.ToString(),
                    referralTypeResults.DataItems.Select(
                        x => new { ID = x.ReferralTypeID, Name = x.ReferralType }).Cast<object>().ToList());
            }

            //Referral Category
            if (lookupTypes.Contains(LookupType.ReferralCategory))
            {
                var referralCategoryResults = _referralCategoryDataProvider.GetReferralCategory();

                if (referralCategoryResults.ResultCode != 0)
                {
                    response.ResultCode = referralCategoryResults.ResultCode;
                    response.ResultMessage = referralCategoryResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ReferralCategory.ToString(),
                    referralCategoryResults.DataItems.Select(
                        x => new { ID = x.ReferralCategoryID, Name = x.ReferralCategory }).Cast<object>().ToList());
            }

            //Referral Resource Type
            if (lookupTypes.Contains(LookupType.ReferralResourceType))
            {
                var referralResourceTypeResults = _referralResourceTypeDataProvider.GetReferralResourceType();

                if (referralResourceTypeResults.ResultCode != 0)
                {
                    response.ResultCode = referralResourceTypeResults.ResultCode;
                    response.ResultMessage = referralResourceTypeResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ReferralResourceType.ToString(),
                    referralResourceTypeResults.DataItems.Select(
                        x => new { ID = x.ReferralResourceTypeID, Name = x.ReferralResourceType }).Cast<object>().ToList());
            }

            //Resource Type
            if (lookupTypes.Contains(LookupType.ResourceType))
            {
                var res = _resourceTypeDataProvider.GetResourceTypeDetails();

                if (res.ResultCode != 0)
                {
                    response.ResultCode = res.ResultCode;
                    response.ResultMessage = res.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ResourceType.ToString(),
                    res.DataItems.Select(
                        x => new { ID = x.ResourceTypeID, Name = x.ResourceType }).Cast<object>().ToList());
            }

            //Referral Disposition
            if (lookupTypes.Contains(LookupType.ReferralDispositionType))
            {
                var referralDispositionTypeResults = _referralDispositionTypeDataProvider.GetReferralDispositionType();

                if (referralDispositionTypeResults.ResultCode != 0)
                {
                    response.ResultCode = referralDispositionTypeResults.ResultCode;
                    response.ResultMessage = referralDispositionTypeResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ReferralDispositionType.ToString(),
                    referralDispositionTypeResults.DataItems.Select(
                        x => new { ID = x.ReferralDispositionID, Name = x.ReferralDisposition }).Cast<object>().ToList());
            }

            //Referral Disposition Outcome
            if (lookupTypes.Contains(LookupType.ReferralDispositionOutcome))
            {
                var referralDispositionOutcomeTypeResults = _referralDispositionOutcomeTypeDataProvider.GetReferralDispositionOutcomeType();

                if (referralDispositionOutcomeTypeResults.ResultCode != 0)
                {
                    response.ResultCode = referralDispositionOutcomeTypeResults.ResultCode;
                    response.ResultMessage = referralDispositionOutcomeTypeResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ReferralDispositionOutcome.ToString(),
                    referralDispositionOutcomeTypeResults.DataItems.Select(
                        x => new { ID = x.ReferralDispositionOutcomeID, Name = x.ReferralDispositionOutcome }).Cast<object>().ToList());
            }

            //User Facility
            if (lookupTypes.Contains(LookupType.UserFacility))
            {
                var userFacilityResult = _userFacilityDataProvider.GetUserFacility();

                if (userFacilityResult.ResultCode != 0)
                {
                    response.ResultCode = userFacilityResult.ResultCode;
                    response.ResultMessage = userFacilityResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.UserFacility.ToString(),
                    userFacilityResult.DataItems.Select(
                        x =>
                            new
                            {
                                Name = x.UserName,
                                ID = x.UserFacilityID
                            }).Cast<object>().ToList());
            }

            //Referral Origin
            if (lookupTypes.Contains(LookupType.ReferralOrigin))
            {
                var userFacilityResult = _referralOriginDataProvider.GetReferralOrigin();

                if (userFacilityResult.ResultCode != 0)
                {
                    response.ResultCode = userFacilityResult.ResultCode;
                    response.ResultMessage = userFacilityResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ReferralOrigin.ToString(),
                    userFacilityResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.ReferralOriginID,
                                Name = x.ReferralOrigin
                            }).Cast<object>().ToList());
            }

            //Immunization Status
            if (lookupTypes.Contains(LookupType.ImmunizationStatus))
            {
                var immunizationStatusResult = _immunizationStatusDataProvider.GetImmunizationStatuses();

                if (immunizationStatusResult.ResultCode != 0)
                {
                    response.ResultCode = immunizationStatusResult.ResultCode;
                    response.ResultMessage = immunizationStatusResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ImmunizationStatus.ToString(),
                    immunizationStatusResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.ImmunizationStatusID,
                                Name = x.ImmunizationStatus
                            }).Cast<object>().ToList());
            }

            //Referral Concerns
            if (lookupTypes.Contains(LookupType.ReferralConcernType))
            {
                var referralConcernTypeResult = _referralConcernTypeDataProvider.GetReferralConcerns();

                if (referralConcernTypeResult.ResultCode != 0)
                {
                    response.ResultCode = referralConcernTypeResult.ResultCode;
                    response.ResultMessage = referralConcernTypeResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ReferralConcernType.ToString(),
                    referralConcernTypeResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.ReferralConcernID,
                                Name = x.ReferralConcern
                            }).Cast<object>().ToList());
            }

            //Referral Problems
            if (lookupTypes.Contains(LookupType.ReferralProblemType))
            {
                var referralProblemTypeResult = _referralConcernTypeDataProvider.GetReferralProblems();

                if (referralProblemTypeResult.ResultCode != 0)
                {
                    response.ResultCode = referralProblemTypeResult.ResultCode;
                    response.ResultMessage = referralProblemTypeResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ReferralProblemType.ToString(),
                    referralProblemTypeResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.ReferralConcernID,
                                Name = x.ReferralConcern
                            }).Cast<object>().ToList());
            }

            //Referral Priorities
            if (lookupTypes.Contains(LookupType.ReferralPriority))
            {
                var referralPriorityResult = _referralPriorityDataProvider.GetReferralPriorities();

                if (referralPriorityResult.ResultCode != 0)
                {
                    response.ResultCode = referralPriorityResult.ResultCode;
                    response.ResultMessage = referralPriorityResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ReferralPriority.ToString(),
                    referralPriorityResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.ReferralPriorityID,
                                Name = x.ReferralPriority
                            }).Cast<object>().ToList());
            }

            //Discharge Reason
            if (lookupTypes.Contains(LookupType.DischargeReason))
            {
                var dischargeReasonResult = _dischargeReasonDataProvider.GetDischargeReason();
                if (dischargeReasonResult.ResultCode != 0)
                {
                    response.ResultCode = dischargeReasonResult.ResultCode;
                    response.ResultMessage = dischargeReasonResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.DischargeReason.ToString(),
                    dischargeReasonResult.DataItems.Select(
                        x => new { ID = x.DischargeReasonID, Name = x.DischargeReason }).Cast<object>().ToList());
            }

            //Referral Organization
            if (lookupTypes.Contains(LookupType.ReferralOrganization))
            {
                var referralOrganizationResult = _organizationDataProvider.GetOrganizations();

                if (referralOrganizationResult.ResultCode != 0)
                {
                    response.ResultCode = referralOrganizationResult.ResultCode;
                    response.ResultMessage = referralOrganizationResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ReferralOrganization.ToString(),
                    referralOrganizationResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.ReferralOrganizationID,
                                Name = x.ReferralOrganization
                            }).Cast<object>().ToList());
            }

            //Referral Category Source
            if (lookupTypes.Contains(LookupType.ReferralCategorySource))
            {
                var referralCategorySourceResult = _referralCategorySourceDataProvider.GetReferralCategorySource();

                if (referralCategorySourceResult.ResultCode != 0)
                {
                    response.ResultCode = referralCategorySourceResult.ResultCode;
                    response.ResultMessage = referralCategorySourceResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ReferralCategorySource.ToString(),
                    referralCategorySourceResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.ReferralCategorySourceID,
                                Name = x.ReferralSource,
                                CategoryID = x.ReferralCategoryID
                            }).Cast<object>().ToList());
            }

            //Referral Disposition Status
            if (lookupTypes.Contains(LookupType.ReferralDispositionStatus))
            {
                var referralDispositionStatusResult = _referralDispositionStatusTypeDataProvider.GetReferralDispositionStatusType();

                if (referralDispositionStatusResult.ResultCode != 0)
                {
                    response.ResultCode = referralDispositionStatusResult.ResultCode;
                    response.ResultMessage = referralDispositionStatusResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ReferralDispositionStatus.ToString(),
                    referralDispositionStatusResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.ReferralDispositionStatusID,
                                Name = x.ReferralDispositionStatus
                            }).Cast<object>().ToList());
            }
            //Program Client Identifier
            if (lookupTypes.Contains(LookupType.ProgramClientIdentifier))
            {
                var programClientIdentifiers = _programClientIdentifierDataProvider.GetProgramClientIdentifiers();

                if (programClientIdentifiers.ResultCode != 0)
                {
                    response.ResultCode = programClientIdentifiers.ResultCode;
                    response.ResultMessage = programClientIdentifiers.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ProgramClientIdentifier.ToString(),
                    programClientIdentifiers.DataItems.Select(
                        x =>
                            new
                            {
                                ProgramID = x.ClientTypeID,
                                ClientIdentifierTypeID = x.ClientIdentifierTypeID
                            }).Cast<object>().ToList());
            }

            //Progress Note Type
            if (lookupTypes.Contains(LookupType.ProgressNoteType))
            {
                var noteTypeResult = _noteTypeDataProvider.GetProgressNoteType();
                if (noteTypeResult.ResultCode != 0)
                {
                    response.ResultCode = noteTypeResult.ResultCode;
                    response.ResultMessage = noteTypeResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ProgressNoteType.ToString(),
                  noteTypeResult.DataItems.Select(x => new { ID = x.NoteTypeID, Name = x.NoteType }).Cast<object>().ToList());
            }

            //Appointment Types
            if (lookupTypes.Contains(LookupType.AppointmentTypes))
            {
                var noteTypeResult = _appointmentDataProvider.GetAppointmentType(0);
                if (noteTypeResult.ResultCode != 0)
                {
                    response.ResultCode = noteTypeResult.ResultCode;
                    response.ResultMessage = noteTypeResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.AppointmentTypes.ToString(),
                  noteTypeResult.DataItems.Select(x => new
                  {
                      AppointmentTypeID = x.AppointmentTypeID,
                      AppointmentType = x.AppointmentType,
                      ProgramID = x.ProgramID
                  }).Cast<object>().ToList());
            }

            //Appointment Status
            if (lookupTypes.Contains(LookupType.AppointmentStatus))
            {
                var apptstatus = _appointmentDataProvider.GetAppointmentStatus();
                if (apptstatus.ResultCode != 0)
                {
                    response.ResultCode = apptstatus.ResultCode;
                    response.ResultMessage = apptstatus.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.AppointmentStatus.ToString(),
                  apptstatus.DataItems.Select(x => new
                  {
                      AppointmentStatusID = x.AppointmentStatusID,
                      AppointmentStatus = x.AppointmentStatus
                  }).Cast<object>().ToList());
            }

            // Appointment Mapping.

            if (lookupTypes.Contains(LookupType.AppointmentMapping))
            {
                var apptstatusMapping = _appointmentDataProvider.GetAppointmentTypeMapping();
                if (apptstatusMapping.ResultCode != 0)
                {
                    response.ResultCode = apptstatusMapping.ResultCode;
                    response.ResultMessage = apptstatusMapping.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.AppointmentMapping.ToString(),
                  apptstatusMapping.DataItems.Select(x => new
                  {
                      ServiceID = x.ServiceID,
                      ServiceName = x.ServiceName,
                      AppointmentTypeID = x.AppointmentTypeID
                  }).Cast<object>().ToList());
            }

            //Document Types
            if (lookupTypes.Contains(LookupType.DocumentType))
            {
                var result = _documentTypeProvider.GetDocumentType();
                if (result.ResultCode != 0)
                {
                    response.ResultCode = result.ResultCode;
                    response.ResultMessage = result.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.DocumentType.ToString(),
                  result.DataItems.Select(x => new
                  {
                      DocumentTypeID = x.DocumentTypeID,
                      DocumentType = x.DocumentType
                  }).Cast<object>().ToList());
            }

            //Service Items
            if (lookupTypes.Contains(LookupType.ServiceItem))
            {
                var serviceItemResult = _serviceItemDataProvider.GetServiceItems();

                if (serviceItemResult.ResultCode != 0)
                {
                    response.ResultCode = serviceItemResult.ResultCode;
                    response.ResultMessage = serviceItemResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ServiceItem.ToString(),
                    serviceItemResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.ServiceID,
                                Name = x.ServiceName,
                                ProgramID = x.ProgramID,
                            }).Cast<object>().ToList());
            }

            //Attendance Status
            if (lookupTypes.Contains(LookupType.AttendanceStatus))
            {
                var attendanceStatusResult = _attendanceStatusDataProvider.GetAttendanceStatuses();

                if (attendanceStatusResult.ResultCode != 0)
                {
                    response.ResultCode = attendanceStatusResult.ResultCode;
                    response.ResultMessage = attendanceStatusResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.AttendanceStatus.ToString(),
                    attendanceStatusResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.AttendanceStatusID,
                                Name = x.AttendanceStatus                               
                            }).Cast<object>().ToList());
            }

            //Attendance Status Configured
            if (lookupTypes.Contains(LookupType.AttendanceStatusConfigured))
            {
                var attendanceStatusResult = _attendanceStatusDataProvider.GetAttendanceStatusesConfigured();

                if (attendanceStatusResult.ResultCode != 0)
                {
                    response.ResultCode = attendanceStatusResult.ResultCode;
                    response.ResultMessage = attendanceStatusResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.AttendanceStatusConfigured.ToString(),
                    attendanceStatusResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.AttendanceStatusID,
                                Name = x.AttendanceStatus,
                                ServiceID = x.ServicesID,
                                ModuleComponentID = x.ModuleComponentID,
                                IsActive = x.IsActive,
                            }).Cast<object>().ToList());
            }

            //Delivery Method
            if (lookupTypes.Contains(LookupType.DeliveryMethod))
            {
                var deliveryMethodResult = _deliveryMethodDataProvider.GetDeliveryMethods();

                if (deliveryMethodResult.ResultCode != 0)
                {
                    response.ResultCode = deliveryMethodResult.ResultCode;
                    response.ResultMessage = deliveryMethodResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.DeliveryMethod.ToString(),
                    deliveryMethodResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.DeliveryMethodID,
                                Name = x.DeliveryMethod
                            }).Cast<object>().ToList());
            }

            //Service Status
            if (lookupTypes.Contains(LookupType.ServiceStatus))
            {
                var serviceStatusResult = _serviceStatusDataProvider.GetServiceStatuses();

                if (serviceStatusResult.ResultCode != 0)
                {
                    response.ResultCode = serviceStatusResult.ResultCode;
                    response.ResultMessage = serviceStatusResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ServiceStatus.ToString(),
                    serviceStatusResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.ServiceStatusID,
                                Name = x.ServiceStatus
                            }).Cast<object>().ToList());
            }

            //Service Status Configured
            if (lookupTypes.Contains(LookupType.ServiceStatusConfigured))
            {
                var serviceStatusResult = _serviceStatusDataProvider.GetServiceStatusesConfigured();

                if (serviceStatusResult.ResultCode != 0)
                {
                    response.ResultCode = serviceStatusResult.ResultCode;
                    response.ResultMessage = serviceStatusResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ServiceStatusConfigured.ToString(),
                    serviceStatusResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.ServiceStatusID,
                                Name = x.ServiceStatus,
                                ServiceID = x.ServicesID,
                                ModuleComponentID = x.ModuleComponentID,
                                IsActive = x.IsActive,
                            }).Cast<object>().ToList());
            }

            //Service To Service Status
            if (lookupTypes.Contains(LookupType.ServiceToServiceStatus))
            {
                var serviceStatusResult = _serviceStatusDataProvider.GetServiceToServiceStatuses();

                if (serviceStatusResult.ResultCode != 0)
                {
                    response.ResultCode = serviceStatusResult.ResultCode;
                    response.ResultMessage = serviceStatusResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ServiceToServiceStatus.ToString(),
                    serviceStatusResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.ServiceStatusID,
                                Name = x.ServiceStatus,
                                ServiceID = x.ServiceID
                            }).Cast<object>().ToList());
            }

            //Service - TFS-7269.
            if (lookupTypes.Contains(LookupType.Service))
            {
                var serviceResult = _serviceDataProvider.GetService();

                if (serviceResult.ResultCode != 0)
                {
                    response.ResultCode = serviceResult.ResultCode;
                    response.ResultMessage = serviceResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.Service.ToString(),
                    serviceResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.ServiceID,
                                Name = x.ServiceName,
                                ProgramID = x.ProgramID
                            }).Cast<object>().ToList());
            }

            if (lookupTypes.Contains(LookupType.ServiceDetails))
            {
                var serviceResult = _serviceDataProvider.GetServiceDetails();

                if (serviceResult.ResultCode != 0)
                {
                    response.ResultCode = serviceResult.ResultCode;
                    response.ResultMessage = serviceResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ServiceDetails.ToString(),
                    serviceResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.ServiceID,
                                Name = x.ServiceName
                            }).Cast<object>().ToList());
            }

            //Service Location
            if (lookupTypes.Contains(LookupType.ServiceLocation))
            {
                var serviceLocationResult = _serviceLocationDataProvider.GetServiceLocation();

                if (serviceLocationResult.ResultCode != 0)
                {
                    response.ResultCode = serviceLocationResult.ResultCode;
                    response.ResultMessage = serviceLocationResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ServiceLocation.ToString(),
                    serviceLocationResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.ServiceLocationID,
                                Name = x.ServiceLocation
                            }).Cast<object>().ToList());
            }

            //Recipient Code
            if (lookupTypes.Contains(LookupType.RecipientCode))
            {
                var recipientCodeResult = _recipientCodeDataProvider.GetRecipientCodes();

                if (recipientCodeResult.ResultCode != 0)
                {
                    response.ResultCode = recipientCodeResult.ResultCode;
                    response.ResultMessage = recipientCodeResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.RecipientCode.ToString(),
                    recipientCodeResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.CodeID,
                                Name = x.CodeDescription
                            }).Cast<object>().ToList());
            }

            //Conversion Status
            if (lookupTypes.Contains(LookupType.ConversionStatus))
            {
                var conversionStatusResult = _conversionStatusDataProvider.GetConversionStatuses();

                if (conversionStatusResult.ResultCode != 0)
                {
                    response.ResultCode = conversionStatusResult.ResultCode;
                    response.ResultMessage = conversionStatusResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ConversionStatus.ToString(),
                    conversionStatusResult.DataItems.Select(
                        x =>
                            new
                            {
                                ID = x.ConversionStatusID,
                                Name = x.ConversionStatus
                            }).Cast<object>().ToList());
            }

            //Call Center Call Status
            if (lookupTypes.Contains(LookupType.CallStatus))
            {
                var callStatusResult = _callStatusDataProvider.GetCallStatus();
                if (callStatusResult.ResultCode != 0)
                {
                    response.ResultCode = callStatusResult.ResultCode;
                    response.ResultMessage = callStatusResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.CallStatus.ToString(),
                  callStatusResult.DataItems.Select(x => new { ID = x.CallStatusID, Name = x.CallStatus }).Cast<object>().ToList());
            }

            //Call Center Priority
            if (lookupTypes.Contains(LookupType.CallCenterPriority))
            {
                var cCprirotyResult = _cCPriorityDataProvider.GetCCPriorities();
                if (cCprirotyResult.ResultCode != 0)
                {
                    response.ResultCode = cCprirotyResult.ResultCode;
                    response.ResultMessage = cCprirotyResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.CallCenterPriority.ToString(),
                  cCprirotyResult.DataItems.Select(x => new { ID = x.CallCenterPriorityID, Name = x.CallCenterPriority }).Cast<object>().ToList());
            }
            //ProgramUnit
            if (lookupTypes.Contains(LookupType.ProgramUnit))
            {
                var programUnitResult = _programUnitDataProvider.GetProgramUnit();
                if (programUnitResult.ResultCode != 0)
                {
                    response.ResultCode = programUnitResult.ResultCode;
                    response.ResultMessage = programUnitResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ProgramUnit.ToString(),
                  programUnitResult.DataItems.Select(x => new { ID = x.OrganizationID, Name = x.Name, ServicesID = x.ServicesID, ServiceName = x.ServiceName }).Cast<object>().ToList());
            }
            //WorkflowProgramUnit
            if (lookupTypes.Contains(LookupType.WorkflowProgramUnit))
            {
                var workflowProgramUnitResult = _programUnitDataProvider.GetWorkflowProgramUnits();
                if (workflowProgramUnitResult.ResultCode != 0)
                {
                    response.ResultCode = workflowProgramUnitResult.ResultCode;
                    response.ResultMessage = workflowProgramUnitResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.WorkflowProgramUnit.ToString(),
                  workflowProgramUnitResult.DataItems.Select(x => new {
                      OrganizationID = x.OrganizationID,
                      Name = x.Name,
                      DetailID = x.DetailID,
                      DataKey = x.DataKey,
                      IsActive = x.IsActive
                  }).Cast<object>().ToList());
            }

            //SHID
            if (lookupTypes.Contains(LookupType.SHID))
            {
                var shIDResult = _sHIDDataProvider.GetSuicidalHomicidal();
                if (shIDResult.ResultCode != 0)
                {
                    response.ResultCode = shIDResult.ResultCode;
                    response.ResultMessage = shIDResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.SHID.ToString(),
                  shIDResult.DataItems.Select(x => new { ID = x.SuicideHomicideID, Name = x.SuicideHomicide }).Cast<object>().ToList());
            }

            //CallType
            if (lookupTypes.Contains(LookupType.CallType))
            {
                var callTypeResult = _callTypeProvider.GetCallType();
                if (callTypeResult.ResultCode != 0)
                {
                    response.ResultCode = callTypeResult.ResultCode;
                    response.ResultMessage = callTypeResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.CallType.ToString(),
                  callTypeResult.DataItems.Select(x => new { ID = x.CallTypeID, Name = x.CallType }).Cast<object>().ToList());
            }

            //Client Status
            if (lookupTypes.Contains(LookupType.ClientStatus))
            {
                var clientStatusResult = _clientStatusDataProvider.GetClientStatus();
                if (clientStatusResult.ResultCode != 0)
                {
                    response.ResultCode = clientStatusResult.ResultCode;
                    response.ResultMessage = clientStatusResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ClientStatus.ToString(),
                  clientStatusResult.DataItems.Select(x => new { ID = x.ClientStatusID, Name = x.ClientStatus }).Cast<object>().ToList());
            }
            //Behavioral Category
            if (lookupTypes.Contains(LookupType.BehavioralCategory))
            {
                var behaviorStatusResult = _behavioralCategoryDataProvider.GetBehavioralCategories();
                if (behaviorStatusResult.ResultCode != 0)
                {
                    response.ResultCode = behaviorStatusResult.ResultCode;
                    response.ResultMessage = behaviorStatusResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.BehavioralCategory.ToString(),
                  behaviorStatusResult.DataItems.Select(x => new { ID = x.BehavioralCategoryID, Name = x.BehavioralCategory }).Cast<object>().ToList());
            }

            //Cancel Reason
            if (lookupTypes.Contains(LookupType.CancelReason))
            {
                var cancelReasonResult = _cancelReasonDataProvider.GetCancelReasons();
                if (cancelReasonResult.ResultCode != 0)
                {
                    response.ResultCode = cancelReasonResult.ResultCode;
                    response.ResultMessage = cancelReasonResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.CancelReason.ToString(),
                  cancelReasonResult.DataItems.Select(x => new { ID = x.ReasonID, Name = x.Reason }).Cast<object>().ToList());
            }

            //DayOfWeek
            if (lookupTypes.Contains(LookupType.DayOfWeek))
            {
                var dayOfWeekResults = _dayOfWeekProvider.GetDayOfWeek();

                if (dayOfWeekResults.ResultCode != 0)
                {
                    response.ResultCode = dayOfWeekResults.ResultCode;
                    response.ResultMessage = dayOfWeekResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.DayOfWeek.ToString(),
                    dayOfWeekResults.DataItems.Select(
                        x => new { ID = x.DayOfWeekID, Name = x.Name, ShortName = x.ShortName }).Cast<object>().ToList());
            }

            //WeekOfMonth
            if (lookupTypes.Contains(LookupType.WeekOfMonth))
            {
                var weekOfMonthResults = _weekOfMonthProvider.GetWeekOfMonth();

                if (weekOfMonthResults.ResultCode != 0)
                {
                    response.ResultCode = weekOfMonthResults.ResultCode;
                    response.ResultMessage = weekOfMonthResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.WeekOfMonth.ToString(),
                    weekOfMonthResults.DataItems.Select(
                        x => new { ID = x.WeekOfMonthID, Name = x.Name }).Cast<object>().ToList());
            }

            //Month
            if (lookupTypes.Contains(LookupType.Month))
            {
                var monthResults = _monthProvider.GetMonth();

                if (monthResults.ResultCode != 0)
                {
                    response.ResultCode = monthResults.ResultCode;
                    response.ResultMessage = monthResults.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.Month.ToString(),
                    monthResults.DataItems.Select(
                        x => new { ID = x.MonthID, Name = x.Name }).Cast<object>().ToList());
            }

            //Enitty Type
            if (lookupTypes.Contains(LookupType.EntityType))
            {
                var res = _entityTypeDataProvider.GetEntityType();

                if (res.ResultCode != 0)
                {
                    response.ResultCode = res.ResultCode;
                    response.ResultMessage = res.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.EntityType.ToString(),
                    res.DataItems.Select(
                        x => new { EntityTypeID = x.EntityTypeID, EntityType = x.EntityType }).Cast<object>().ToList());
            }

            //ScheduleType
            if (lookupTypes.Contains(LookupType.ScheduleType))
            {
                var results = _scheduleTypeProvider.GetScheduleType();

                if (results.ResultCode != 0)
                {
                    response.ResultCode = results.ResultCode;
                    response.ResultMessage = results.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ScheduleType.ToString(),
                    results.DataItems.Select(
                        x => new { ID = x.ScheduleTypeID, ScheduleType = x.ScheduleType }).Cast<object>().ToList());
            }
            //Referral Agency
            if (lookupTypes.Contains(LookupType.ReferralAgency))
            {
                var referralAgencyResult = _referralAgencyDataProvider.GetReferralAgency();
                if (referralAgencyResult.ResultCode != 0)
                {
                    response.ResultCode = referralAgencyResult.ResultCode;
                    response.ResultMessage = referralAgencyResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.ReferralAgency.ToString(),
                  referralAgencyResult.DataItems.Select(x => new { ID = x.ReferralAgencyID, Name = x.ReferralAgency }).Cast<object>().ToList());
            }

            //AdvancedDirectiveType
            if (lookupTypes.Contains(LookupType.AdvancedDirectiveType))
            {
                var directiveTypeResult = _advancedDirectiveTypeDataProvider.GetDirectiveTypes();
                if (directiveTypeResult.ResultCode != 0)
                {
                    response.ResultCode = directiveTypeResult.ResultCode;
                    response.ResultMessage = directiveTypeResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.AdvancedDirectiveType.ToString(),
                  directiveTypeResult.DataItems.Select(x => new { ID = x.AdvancedDirectiveTypeID, Name = x.AdvancedDirectiveType }).Cast<object>().ToList());
            }

            //Group Type
            if (lookupTypes.Contains(LookupType.GroupType))
            {
                var groupTypeResult = _groupTypeDataProvider.GetGroupType();
                if (groupTypeResult.ResultCode != 0)
                {
                    response.ResultCode = groupTypeResult.ResultCode;
                    response.ResultMessage = groupTypeResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.GroupType.ToString(),
                    groupTypeResult.DataItems.Select(x => new { ID = x.GroupTypeID, Name = x.GroupType }).Cast<object>().ToList());
            }

            //Group Service
            if (lookupTypes.Contains(LookupType.GroupService))
            {
                var groupServiceResult = _groupServiceDataProvider.GetGroupService();
                if (groupServiceResult.ResultCode != 0)
                {
                    response.ResultCode = groupServiceResult.ResultCode;
                    response.ResultMessage = groupServiceResult.ResultMessage;
                    return response;
                }

                returnValue.Add(LookupType.GroupService.ToString(),
                    groupServiceResult.DataItems.Select(x => new { ID = x.ServicesID, Name = x.ServiceName, x.GroupTypeID }).Cast<object>().ToList());
            }
            //Payor Expiration Reason
            if (lookupTypes.Contains(LookupType.PayorExpirationReason))
            {
                var payorExpirationReasonResults = _payorExpirationReasonDataProvider.GetPayorExpirationReason();

                if (payorExpirationReasonResults.ResultCode != 0)
                {
                    response.ResultCode = payorExpirationReasonResults.ResultCode;
                    response.ResultMessage = payorExpirationReasonResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.PayorExpirationReason.ToString(),
                    payorExpirationReasonResults.DataItems.Select(
                        x => new { ID = x.PayorExpirationReasonID, Name = x.PayorExpirationReason }).Cast<object>().ToList());
            }

            //Service Type
            if (lookupTypes.Contains(LookupType.ServiceType))
            {
                var serviceTypeResults = _serviceTypeDataProvider.GetServiceType();

                if (serviceTypeResults.ResultCode != 0)
                {
                    response.ResultCode = serviceTypeResults.ResultCode;
                    response.ResultMessage = serviceTypeResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.ServiceType.ToString(),
                    serviceTypeResults.DataItems.Select(
                        x => new { ID = x.ServiceTypeID, Name = x.ServiceType }).Cast<object>().ToList());
            }

            //ServiceConfigType
            if (lookupTypes.Contains(LookupType.ServiceConfigType))
            {
                var serviceConfigTypeResults = _serviceConfigTypeDataProvider.GetServiceConfigTypes();

                if (serviceConfigTypeResults.ResultCode != 0)
                {
                    response.ResultCode = serviceConfigTypeResults.ResultCode;
                    response.ResultMessage = serviceConfigTypeResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.ServiceConfigType.ToString(),
                    serviceConfigTypeResults.DataItems.Select(
                        x => new { ID = x.ServiceConfigServiceTypeID, Name = x.ServiceConfigServiceType }).Cast<object>().ToList());
            }

            //ServiceWorkflowType
            if (lookupTypes.Contains(LookupType.ServiceWorkflowType))
            {
                var serviceWorkflowTypeResults = _serviceWorkflowTypeDataProvider.GetServiceWorkflowTypes();

                if (serviceWorkflowTypeResults.ResultCode != 0)
                {
                    response.ResultCode = serviceWorkflowTypeResults.ResultCode;
                    response.ResultMessage = serviceWorkflowTypeResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.ServiceWorkflowType.ToString(),
                    serviceWorkflowTypeResults.DataItems.Select(
                        x => new { ID = x.ModuleComponentID, Name = x.Feature }).Cast<object>().ToList());
            }

            //Signature Status
            if (lookupTypes.Contains(LookupType.SignatureStatus))
            {
                var signatureStatusResults = _signatureStatusDataProvider.GetSignatureStatus();

                if (signatureStatusResults.ResultCode != 0)
                {
                    response.ResultCode = signatureStatusResults.ResultCode;
                    response.ResultMessage = signatureStatusResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.SignatureStatus.ToString(),
                    signatureStatusResults.DataItems.Select(
                        x => new { ID = x.SignatureStatusID, Name = x.SignatureStatus }).Cast<object>().ToList());
            }

            //VoidRecordedServiceReason
            if (lookupTypes.Contains(LookupType.VoidRecordedServiceReason))
            {
                var voidRecordedServiceReasonResults = _voidRecordedServiceReasonDataProvider.GetVoidServiceRecordingReasonDetails();

                if (voidRecordedServiceReasonResults.ResultCode != 0)
                {
                    response.ResultCode = voidRecordedServiceReasonResults.ResultCode;
                    response.ResultMessage = voidRecordedServiceReasonResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.VoidRecordedServiceReason.ToString(),
                    voidRecordedServiceReasonResults.DataItems.Select(
                        x => new { ID = x.ServiceRecordingVoidReasonID, Name = x.ServiceRecordingVoidReason }).Cast<object>().ToList());
            }

            //Organizations
            if (lookupTypes.Contains(LookupType.Organizations))
            {
                var organizationsResults = _organizationsDataProvider.GetOrganizations();

                if (organizationsResults.ResultCode != 0)
                {
                    response.ResultCode = organizationsResults.ResultCode;
                    response.ResultMessage = organizationsResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.Organizations.ToString(),
                    organizationsResults.DataItems.Select(
                        x => new { ID = x.MappingID, Name = x.Name, ParentID = x.ParentID, DataKey = x.DataKey }).Cast<object>().ToList());
            }

            //OrganizationDetails
            if (lookupTypes.Contains(LookupType.OrganizationDetails))
            {
                var organizationsResults = _organizationsDataProvider.GetOrganizationDetails();

                if (organizationsResults.ResultCode != 0)
                {
                    response.ResultCode = organizationsResults.ResultCode;
                    response.ResultMessage = organizationsResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.OrganizationDetails.ToString(),
                    organizationsResults.DataItems.Select(
                        x => new { ID = x.DetailID, Name = x.Name, DataKey = x.DataKey }).Cast<object>().ToList());
            }

            //ServiceRecordingSource
            if (lookupTypes.Contains(LookupType.ServiceRecordingSource))
            {
                var serviceRecordingSourceResults = _serviceRecordingSourceDataProvider.GetServiceRecordingSource();

                if (serviceRecordingSourceResults.ResultCode != 0)
                {
                    response.ResultCode = serviceRecordingSourceResults.ResultCode;
                    response.ResultMessage = serviceRecordingSourceResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.ServiceRecordingSource.ToString(),
                    serviceRecordingSourceResults.DataItems.Select(
                    x => new { ID = x.ServiceRecordingSourceID, Name = x.ServiceRecordingSource, DisplayText = x.DisplayText }).Cast<object>().ToList());
            }

            //Document Type Group
            if (lookupTypes.Contains(LookupType.DocumentTypeGroup))
            {
                var res = _documentTypeGroupDataProvider.GetDocumentTypeGroup();

                if (res.ResultCode != 0)
                {
                    response.ResultCode = res.ResultCode;
                    response.ResultMessage = res.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.DocumentTypeGroup.ToString(),
                    res.DataItems.Select(
                        x => new { ID = x.DocumentTypeGroupID, Name = x.DocumentTypeGroup }).Cast<object>().ToList());
            }

            //Service Type
            if (lookupTypes.Contains(LookupType.UserIdentifierType))
            {
                var res = _userIdentifierTypeDataProvider.GetUserIdentifierType();

                if (res.ResultCode != 0)
                {
                    response.ResultCode = res.ResultCode;
                    response.ResultMessage = res.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.UserIdentifierType.ToString(),
                    res.DataItems.Select(
                        x => new { ID = x.UserIdentifierTypeID, Name = x.UserIdentifierType }).Cast<object>().ToList());
            }

            //ConfirmationStatement
            if (lookupTypes.Contains(LookupType.ConfirmationStatement))
            {
                var confirmationStatementResults = _confirmationStatementDataProvider.GetConfirmationStatement();

                if (confirmationStatementResults.ResultCode != 0)
                {
                    response.ResultCode = confirmationStatementResults.ResultCode;
                    response.ResultMessage = confirmationStatementResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.ConfirmationStatement.ToString(),
                    confirmationStatementResults.DataItems.Select(
                        x => new { ID = x.ConfirmationStatementID, Name = x.ConfirmationStatement, ConfirmationStatementGroupID = x.ConfirmationStatementGroupID, DocumentTypeID = x.DocumentTypeID, IsSignatureRequired = x.IsSignatureRequired, OrganizationID = x.OrganizationID }).Cast<object>().ToList());
            }

            //ConfirmationStatementGroup
            if (lookupTypes.Contains(LookupType.ConfirmationStatementGroup))
            {
                var confirmationStatementGroupResults = _confirmationStatementDataProvider.GetConfirmationStatementGroup();

                if (confirmationStatementGroupResults.ResultCode != 0)
                {
                    response.ResultCode = confirmationStatementGroupResults.ResultCode;
                    response.ResultMessage = confirmationStatementGroupResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.ConfirmationStatementGroup.ToString(),
                    confirmationStatementGroupResults.DataItems.Select(
                        x => new { ID = x.ConfirmationStatementGroupID, Name = x.ConfirmationStatementGroup }).Cast<object>().ToList());
            }

            //TrackingField
            if (lookupTypes.Contains(LookupType.TrackingField)) {
                var trackingFieldResults = _trackingFieldDataProvider.GetTrackingFields();

                if (trackingFieldResults.ResultCode != 0)
                {
                    response.ResultCode = trackingFieldResults.ResultCode;
                    response.ResultMessage = trackingFieldResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.TrackingField.ToString(),
                    trackingFieldResults.DataItems.Select(
                        x => new { ID = x.TrackingFieldID, Name = x.TrackingField }).Cast<object>().ToList());
            }

            //TrackingField Configured
            if (lookupTypes.Contains(LookupType.TrackingFieldConfigured))
            {
                var trackingFieldResults = _trackingFieldDataProvider.GetTrackingFieldsConfigured();

                if (trackingFieldResults.ResultCode != 0)
                {
                    response.ResultCode = trackingFieldResults.ResultCode;
                    response.ResultMessage = trackingFieldResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.TrackingFieldConfigured.ToString(),
                    trackingFieldResults.DataItems.Select(
                        x => new
                        { ID = x.TrackingFieldID,
                            Name = x.TrackingField,
                            ServiceID = x.ServicesID,
                            ModuleComponentID = x.ModuleComponentID,
                            IsActive = x.IsActive,
                        }).Cast<object>().ToList());
            }


            //Recording Services
            if (lookupTypes.Contains(LookupType.RecordingServices))
            {
                var servicesResults = _servicesDataProvider.GetServicesModuleComponents();

                if (servicesResults.ResultCode != 0)
                {
                    response.ResultCode = servicesResults.ResultCode;
                    response.ResultMessage = servicesResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.RecordingServices.ToString(),
                    servicesResults.DataItems.Select(
                        x => new
                        {
                            ServicesModuleComponentID = x.ServicesModuleComponentID,
                            ServiceID = x.ServiceID,
                            ServiceName = x.ServiceName,
                            ModuleComponentID = x.ModuleComponentID,
                            DataKey = x.DataKey,
                            EffectiveDate = x.EffectiveDate,
                            ExpirationDate = x.ExpirationDate
                        }).Cast<object>().ToList());
            }


            //Recording Service Location
            if (lookupTypes.Contains(LookupType.RecordingServiceLocation))
            {
                var servicesLocationResults = _serviceLocationDataProvider.GetServiceLocationModuleComponents();

                if (servicesLocationResults.ResultCode != 0)
                {
                    response.ResultCode = servicesLocationResults.ResultCode;
                    response.ResultMessage = servicesLocationResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.RecordingServiceLocation.ToString(),
                    servicesLocationResults.DataItems.Select(
                        x => new
                        {
                            ServiceLocationModuleComponentID = x.ServiceLocationModuleComponentID,
                            ID = x.ServiceLocationID,
                            Name = x.ServiceLocation,
                            ModuleComponentID = x.ModuleComponentID,
                            DataKey = x.DataKey,
                            ServiceID = x.ServiceID,
                            IsActive = x.IsActive,
                        }).Cast<object>().ToList());
            }

            //Recording Delivery Method
            if (lookupTypes.Contains(LookupType.RecordingDeliveryMethod))
            {
                var deliveryMethodResults = _deliveryMethodDataProvider.GetDeliveryMethodModuleComponents();

                if (deliveryMethodResults.ResultCode != 0)
                {
                    response.ResultCode = deliveryMethodResults.ResultCode;
                    response.ResultMessage = deliveryMethodResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.RecordingDeliveryMethod.ToString(),
                    deliveryMethodResults.DataItems.Select(
                        x => new
                        {
                            DeliveryMethodModuleComponentID = x.DeliveryMethodModuleComponentID,
                            ID = x.DeliveryMethodID,
                            Name = x.DeliveryMethod,
                            ModuleComponentID = x.ModuleComponentID,
                            DataKey = x.DataKey,
                            ServiceID = x.ServiceID,
                            IsActive = x.IsActive,
                        }).Cast<object>().ToList());
            }


            //Recording Recipent Code
            if (lookupTypes.Contains(LookupType.RecordingRecipientCode))
            {
                var recipientCodeResults = _recipientCodeDataProvider.GetRecipientCodeModuleComponents();

                if (recipientCodeResults.ResultCode != 0)
                {
                    response.ResultCode = recipientCodeResults.ResultCode;
                    response.ResultMessage = recipientCodeResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.RecordingRecipientCode.ToString(),
                    recipientCodeResults.DataItems.Select(
                        x => new
                        {
                            RecipientCodeModuleComponentID = x.RecipientCodeModuleComponentID,
                            ID = x.CodeID,
                            Code = x.Code,
                            Name = x.CodeDescription,
                            ModuleComponentID = x.ModuleComponentID,
                            DataKey = x.DataKey,
                            ServiceID = x.ServiceID,
                            IsActive = x.IsActive,
                        }).Cast<object>().ToList());
            }

            //Cause of Death
            if (lookupTypes.Contains(LookupType.CauseOfDeath))
            {
                var causeOfDeathResults = _causeOfDeathDataProvider.GetCauseOfDeath();

                if (causeOfDeathResults.ResultCode != 0)
                {
                    response.ResultCode = causeOfDeathResults.ResultCode;
                    response.ResultMessage = causeOfDeathResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.CauseOfDeath.ToString(),
                    causeOfDeathResults.DataItems.Select(
                        x => new { ID = x.CauseOfDeathID, Name = x.CauseOfDeathName }).Cast<object>().ToList());
            }

            //Admission Reason
            if (lookupTypes.Contains(LookupType.AdmissionReason))
            {
                var admissionReasonResults = _admissionReasonDataProvider.GetAdmissionReasonTypes();

                if (admissionReasonResults.ResultCode != 0)
                {
                    response.ResultCode = admissionReasonResults.ResultCode;
                    response.ResultMessage = admissionReasonResults.ResultMessage;
                    return response;
                }
                returnValue.Add(LookupType.AdmissionReason.ToString(),
                    admissionReasonResults.DataItems.Select(
                        x => new { ID = x.AdmissionReasonID, Name = x.AdmissionReason }).Cast<object>().ToList());
            }

            if (lookupTypes.Contains(LookupType.Duration))
            {
                var durationResults = _serviceDurationDataProvider.GetServiceDurations();

                if (durationResults.ResultCode != 0)
                {
                    response.ResultCode = durationResults.ResultCode;
                    response.ResultMessage = durationResults.ResultMessage;
                    return response;
                }
                //TODO: Optionally have a system setting to precache only certain states' counties
                returnValue.Add(LookupType.Duration.ToString(),
                    durationResults.DataItems.Select(
                        x => new { value = x.ServiceDurationID, StartDuration = x.ServiceDurationStart, EndDuration = x.ServiceDurationEnd ,text=x.ServiceDurationDisplay}).Cast<object>().ToList());
            }

            response.DataItems.Add(returnValue);
            return response;
        }



        #endregion exposed functionality
    }
}
