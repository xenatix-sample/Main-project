(function () {
		angular.module('xenatixApp')
						.controller('serviceRecordingController', ['$scope', '$filter', 'alertService', '$stateParams', '$rootScope', '$q', '$state', 'formService', 'lookupService', 'serviceRecordingService', 'callerInformationService', 'navigationService', 'credentialSecurityService', 'ServiceRecordingSourceID', 'eSignatureService', 'userCredentialService', 'voidService', 'cacheService', 'roleSecurityService', 'recordingServicePrintService', 'registrationService', 'dateTimeValidatorService', 'providersService', '$log', 'helperService', 'WorkflowHeaderService',
										function ($scope, $filter, alertService, $stateParams, $rootScope, $q, $state, formService, lookupService, serviceRecordingService, callerInformationService, navigationService, credentialSecurityService, ServiceRecordingSourceID, eSignatureService, userCredentialService, voidService, cacheService, roleSecurityService, printService, registrationService, dateTimeValidatorService, providersService, $log, helperService, WorkflowHeaderService) {
												var CallCenterHeaderID = $stateParams.CallCenterHeaderID;
												var DocumentTypeID = DOCUMENT_TYPE.ServiceRecording;
												var defaultDropdown = {                 //TODO: Change hardcode to values from enums - We can utilise AngularJS Constants to solve this as well. 
														ServiceLocationID: 1,
														AttendanceStatusID: 9,
														DeliveryMethodID: 1   //Enum won't work
												};

												var ignoreTime = "12:00:01 AM";
												var licenseIssueDate = 'LicenseIssueDate';
												var licenseExpirationDate = 'LicenseExpirationDate';
												var documentID = 0;
												var isMRN = false;
												var contactTypeID = null;
												$scope.CallMinStartDate = new Date(70, 1, 1);
												$scope.permissionKey = $state.current.data.permissionKey;
												var programUnits = lookupService.getLookupsByTypeAll("WorkflowProgramUnit");
												$scope.programUnits = $filter('filter')(programUnits, { DataKey: $scope.permissionKey, IsActive: true }, true);
												var START_TIME_STR = 'startTime';
												var END_TIME_STR = 'endTime';
												$scope.StatusNotSelected = SERVICE_STATUS.StatusNotSelected;
												$scope.calltoMCOTID = SERVICE_STATUS.Call_to_MCOT;
												$scope.staffOnlyID = RECIPIENT.Staff_Only;
												var isCallEnded = false;
												var providerData = null;
												$scope.isDigitalSignatureConditionallyRequired = true;
												$scope.crisisServiceID = SERVICE_ITEM.Crisis_Services;
												$scope.dateOptions = {
														formatYear: 'yy',
														startingDay: 1,
														showWeeks: false
												};
												var serviceName = '';
												var getPromises = [];

												var moduleComponentID;

												$scope.activeRecordingServices = [];
												var init = function () {

														$scope.serviceProviders = lookupService.getLookupsByType("Users");
														$('#ServiceStartTime,#ServiceEndTime').timepicker({
																minuteStep: 1,
																showInputs: false,
																//disableFocus: true,
																change: function (time) {
																		$scope.calcDuration();
																}
														});

														$scope.$parent['autoFocus'] = true;
														$scope.serviceRecording = {
																ServiceRecordingID: 0,
																ServiceRecordingSourceID: ServiceRecordingSourceID,
																CallCenterHeaderID: null,
																ServiceItemID: null,
																AttendanceStatusID: $scope.isLawLiaison ? defaultDropdown.AttendanceStatusID : null, //defaultDropdown.AttendanceStatusID,
																DeliveryMethodID: $scope.isCrisisLine ? defaultDropdown.DeliveryMethodID : null, //defaultDropdown.DeliveryMethodID,
																ServiceStatusID: null,
																ServiceLocationID: $scope.isCrisisLine ? defaultDropdown.ServiceLocationID : null,
																RecipientCodeID: null,
																RecipientCode: null,
																NumberOfRecipients: null,
																ConversionStatusID: null,
																ConversionDateTime: null,
																EndDate: null,
																UserID: null,
																OrganizationID: null,
																ServiceTypeID: null,
																ServiceStartDate: null,
																ServiceStartTime: null,
																ServiceEndDate: null,
																ServiceEndTime: null,
																SupervisorUserID: null,
																ModifiedOn: null,
																CallStartAMPM: $filter('toStandardTimeAMPM')(getCurrentTime(new Date())),
																CallEndAMPM: null, //$filter('toStandardTimeAMPM')(getCurrentTime(new Date())),
																AttendedList: [],
																AdditionalUserList: [],
																Provider: null,
																SentToCMHCDate: null
														};

														//Set the serviceName variable to filter Program Unit as per the screen
														if ($state.current.name.indexOf('crisisline') >= 0) {
																moduleComponentID = MODULE_COMPONENT.Crisis_Line;
																$scope.isCrisisLine = true;
																$scope.isManager = cacheService.get('IsManagerAccess');
																$scope.isReadOnly = false;
																$scope.providerKey = PROVIDER_KEY.CrisisLine_Service;
																$scope.supervisorproviderKey = PROVIDER_KEY.CrisisLine_Service_Supervising;
														}
														else if ($state.current.name.indexOf('lawliaison') >= 0) {
																serviceName = 'Law Liaison';
																moduleComponentID = MODULE_COMPONENT.Law_Liaison;
																$scope.isLawLiaison = true;
																$scope.isReadOnlyForm = $scope.isReadOnly = cacheService.get('IsReadOnlyLLScreens');
																$scope.providerKey = PROVIDER_KEY.LawLiaison_Service;
																$scope.supervisorproviderKey = PROVIDER_KEY.LawLiaison_Service_Supervising;
														}
														providersService.getProviders($scope.providerKey).then(function (Response) {
																providerData = Response.DataItems;
														});

														if (CallCenterHeaderID) {
																getHeader(CallCenterHeaderID);
														}
														$state.transitionTo($state.current.name, $stateParams);
														var allServiceLookups = serviceRecordingService.getAllServiceLookups(moduleComponentID);
														angular.extend($scope, allServiceLookups);
												};
												var setProgramUnitModel = function (organizationID) {
														$scope.organizationDetails = $filter('filter')($scope.programUnits, { OrganizationID: organizationID }, true)[0];
												};
												var resetForm = function () {
														$rootScope.formReset($scope.ctrl.serviceRecordingForm);
														$scope.formDetailsChanged = false;
														if ($scope.ctrl.serviceRecordingForm) {
																$scope.ctrl.serviceRecordingForm.modified = false;
														}
												};

												var signatureFormReset = function () {
														if ($scope.signatureForm)
																$rootScope.formReset($scope.signatureForm, $scope.signatureForm.$name);
												};

												var setDefaultCredential = function () {
														if ($scope.userCredentials && $scope.userCredentials.length == 1) {
																$scope.signature.CredentialID = $scope.userCredentials[0].CredentialID;
																$scope.checkPermission();
																signatureFormReset();
														}
												};

												$scope.onProgramUnitChanged = function (organizationDetails) {
														if (organizationDetails && organizationDetails.DetailID) {
																serviceRecordingService.getActiveServices(organizationDetails.DetailID, organizationDetails.OrganizationID, $scope.permissionKey).then(function (activeServices) {
																		if (hasDetails(activeServices)) {
																				$scope.activeRecordingServices = activeServices;
																		}
																});
																$scope.serviceRecording.OrganizationID = organizationDetails.OrganizationID;
														}
														else {
																$scope.serviceRecording.OrganizationID = null;
																$scope.activeRecordingServices = [];
														}
												};

												var initSignature = function () {
														$scope.signature = {
																UserFullName: '',
																digitalPassword: '',
																CredentialID: null,
																Password: null,
																DateSigned: null
														};
														$scope.isServiceCoordinatorReadonly = true;
														$scope.isCredentialReadonly = false;
														$scope.isPasswordReadonly = false;
														$scope.isSigned = false;
														$scope.isPageDisable = false;
														$scope.signatureVerified = false;
														$scope.DocumentID = CallCenterHeaderID;

														navigationService.get()
														.then(function (response) {
																var data = response.DataItems[0];
																//Get UserID, digital password
																$scope.userID = data.UserID;
																$scope.signature.UserFullName = data.UserFullName;
																$scope.signature.digitalPassword = data.DigitalPassword;
																//Get User Credentials
																userCredentialService.getwithServiceID(data.UserID, moduleComponentID).then(function (data) {
																		if (hasData(data)) {
																				$scope.allUserCredentials = $filter('orderBy')(filterFutureOrExpiredRecords(data.DataItems, licenseExpirationDate, licenseIssueDate), 'CredentialName');
																				$scope.userCredentials = $filter('filter')($scope.allUserCredentials, { ServicesID: $scope.serviceRecording.ServiceItemID }, true);
																				setDefaultCredential();
																		}
																		else {
																				$scope.allUserCredentials = [];
																		}
																		if (!$scope.isSignatureExist)
																				getSignature();
																		signatureFormReset();
																});
														})
														.finally(function () {
																$log.debug("Finally Navigation Service Get Completed");
														});
												};

												$scope.onServiceItemChanged = function () {
														if (!$scope.isSigned) {
																if (hasDetails($scope.allUserCredentials)) {
																		$scope.userCredentials = $filter('filter')($scope.allUserCredentials, {
																				ServicesID: $scope.serviceRecording.ServiceItemID ? $scope.serviceRecording.ServiceItemID : -1
																		}, true);
																		setDefaultCredential();
																}
														}
												};

												var getSignature = function () {
														if (documentID && DocumentTypeID) {
																eSignatureService.getDocumentSignatures(DocumentTypeID, documentID).then(function (response) {
																		if (hasData(response)) {
																				$scope.signature = response.DataItems[0];
																				$scope.signature.DateSigned = $filter('formatDate')($scope.signature.ModifiedOn, 'MM/DD/YYYY');
																				$scope.signature.UserFullName = lookupService.getText("Users", $scope.signature.EntityId);
																				$scope.isCredentialReadonly = true;
																				$scope.isPasswordReadonly = true;
																				if ($scope.isLawLiaison)
																						$scope.isPageDisable = true;
																				var userCredentials = $filter('filter')($scope.userCredentials, { CredentialID: $scope.signature.CredentialID }, true);
																				if (!userCredentials || userCredentials.length == 0) {
																						$scope.userCredentials = $filter('filter')(lookupService.getLookupsByType('Credential'), { CredentialID: $scope.signature.CredentialID }, true);
																				}

																				$scope.isSigned = true;
																				signatureFormReset();
																		}
																		else { disableCredentialFields(); }
																});
														}
														else { disableCredentialFields(); }
												};

												var disableCredentialFields = function () {
														if ($scope.isReadOnlyForm) {
																$scope.isCredentialReadonly = true;
																$scope.isPasswordReadonly = true;
														}
												};

												var alertLawLiaisonProgressNoteCompletion = function () {
														if ($scope.isLawLiaison && !$scope.isReadOnly) {
																eSignatureService.getDocumentSignatures(DocumentTypeID, documentID).then(function (response) {
																		if (hasData(response)) {
																				eSignatureService.getDocumentSignatures(DOCUMENT_TYPE.CallCenterProgressNote, $scope.DocumentID).then(function (response) {
																						if (!hasData(response)) {
																								alertService.warning('Please be sure to complete the Progress note for this record.');
																						}
																				});
																		}
																});
														}
												};

												$scope.checkPermission = function () {
														var credentialList = lookupService.getLookupsByType('Credential');
														var result = credentialList.filter(
																																		function (obj, value) {
																																				return (obj.CredentialID == $scope.signature.CredentialID);
																																		}
																														);
														if (result && result.length > 0) {
																var credentialName = result[0].CredentialName;                      //get credential name from selected credential id
																var credentialActionID = CREDENTIAL_ACTION.DigitalSignature;          //Action will be Digital Signature always for current module
																$scope.hasPermission = credentialSecurityService.hasCredentialPermission(credentialName, credentialActionID);
														} else {
																$scope.hasPermission = false;
														}
												};

												$scope.verifySignature = function () {
														if ($scope.isCrisisLine && !isCallEnded) { //only in case of crisis line 
																alertService.error("Please End Call before completing the service.");
																return false;
														}
														if ($scope.signature.CredentialID && $scope.signature.Password) {
																if (hex_md5($scope.signature.Password) == $scope.signature.digitalPassword) {
																		$scope.signatureVerified = true;
																		$scope.isCredentialReadonly = true;
																		$scope.isPasswordReadonly = true;
																		$scope.signature.DateSigned = $filter('formatDate')(new Date(), 'MM/DD/YYYY');
																} else {
																		alertService.error('Invalid password.');
																}
														} else {
																if (!$scope.signature.CredentialID)
																		alertService.error('Please fill out the Credential field.');
																if (!$scope.signature.Password)
																		alertService.error('Please fill out the Digital Password field.');
														}
												};

												var saveDSignature = function (credentialId) {
														if (documentID) {
																var signature = {
																		DocumentId: documentID,      //Primary key of record against which signature will be saved
																		DocumentTypeId: DocumentTypeID,             //Type it could be CallCenter, Discharge Note etc
																		EntitySignatureId: null,                    //Should be passed NULL for D-Signature, should be evaluated with in sproc for user
																		EntityId: $scope.userID,                    //User Id
																		EntityTypeId: 1,                            //Reference Id for type of entity. Here it is for User
																		SignatureBlob: null,                         //Signature in byte format
																		ModifiedOn: $filter('formatDate')(new Date(), 'MM/DD/YYYY'),
																		CredentialID: $scope.signature.CredentialID ? $scope.signature.CredentialID : credentialId
																};
																return eSignatureService.saveDocumentSignature(signature).then(function (response) {
																		if (response.data.ResultCode === 0) {
																				signatureFormReset();
																				setFormStatus(true);
																				alertService.success('Signature status saved successfully!');
																		} else {
																				var msg = 'Error while saving signature! Please reload the page and try again.';
																				alertService.error(msg);
																		}
																});
														} else {
																return $scope.promiseNoOp();
														}
												};

												$scope.clearServiceStatus = function () {
														if ($scope.serviceRecording.ServiceItemID != $scope.crisisServiceID) {
																$scope.serviceRecording.ServiceStatusID = null;
														}
												};

												function populateProgramUnit(serviceType) {
														var lawLiaisonProgramUnit = $filter('filter')($scope.programUnits, function (item) { return (item.Name == serviceType); });
														if (hasDetails(lawLiaisonProgramUnit)) {
																$scope.organizationDetails = lawLiaisonProgramUnit[0];
																$scope.onProgramUnitChanged($scope.organizationDetails);
														}
												};

												function getProviderOfServive() {
														navigationService.get()
														.then(function (data) {
																if (hasData(data)) {
																		$scope.serviceRecording.UserID = data.DataItems[0].UserID;
																}
														})
														.finally(function () {
																resetForm();
														});
												};

												var getCurrentTime = function (date) {
														var currentHour = date.getHours();
														var currentMinute = date.getMinutes();
														var period = currentHour >= 12 ? 'pm' : 'am';
														return $filter('toMilitaryTime')(pad(currentHour, 2) + ":" + pad(currentMinute, 2), period);
												};

												var getDefaultServiceRecording = function () {
														var serviceRecordingDefault = {
																CallStartDate: $filter('formatDate')(new Date(), 'MM/DD/YYYY'),
																CallStartTime: $filter('formatDate')(new Date(), 'hh:mm'),
																CallEndDate: null,
																CallEndTime: null
														};
														angular.extend($scope.serviceRecording, serviceRecordingDefault);
												};

												var getProviderbyid = function (item) {
														return providersService.getProviderbyid(item.UserID)
														.then(function (data) {
																userInfo = {
																		ServiceRecordingAdditionalUserID: item.ServiceRecordingAdditionalUserID,
																		ServiceRecordingID: item.ServiceRecordingID,
																		ID: item.UserID,
																		UserID: item.UserID,
																		IsActive: true,
																		Name: data.DataItems[0].Name
																};
																return userInfo;
														});
												};

												var getDuration = function (startTime, endTime) {
														if (startTime && endTime) {
																$scope.serviceRecording.Duration = $scope.Duration = calculateDuration(startTime, endTime);
														}
														serviceRecordingService.setServiceRecordingTime(startTime, endTime);
												};

												var setStartDateTime = function (startDateTime) {
														var _rVal = toDateTime(startDateTime);
														$scope.serviceRecording.ServiceStartDate = $scope.serviceRecording.CallStartDate = _rVal.date;
														$scope.serviceRecording.ServiceStartTime = $scope.serviceRecording.CallStartTime = _rVal.time;
														$scope.serviceRecording.ServiceStartAMPM = $scope.serviceRecording.CallStartAMPM = _rVal.meridiem;
												};

												var setEndDateTime = function (endDateTime) {
														var _rVal = toDateTime(endDateTime);
														$scope.serviceRecording.ServiceEndDate = $scope.serviceRecording.CallEndDate = _rVal.date;
														$scope.serviceRecording.ServiceEndTime = $scope.serviceRecording.CallEndTime = _rVal.time;
														$scope.serviceRecording.ServiceEndAMPM = $scope.serviceRecording.CallEndAMPM = _rVal.meridiem;
												};

												var getServiceRecording = function (callCenterID, setFormtoInit) {
														$scope.isLoading = true;
														return serviceRecordingService.getServiceRecording(callCenterID, ServiceRecordingSourceID)
														.then(function (data) {
																if (hasData(data)) {
																		angular.extend($scope.serviceRecording, data.DataItems[0]);
																		documentID = $scope.serviceRecording.ServiceRecordingID;
																		isCallEnded = $scope.serviceRecording.ServiceEndDate ? true : false;
																		
																		if ($scope.serviceRecording.SentToCMHCDate) {
																				$scope.serviceRecording.SentToCMHCDate = moment($scope.serviceRecording.SentToCMHCDate).format('MM/DD/YYYY HH:mm:ss');
																		}
																		$scope.pageSecurity = $scope.serviceRecording.ServiceRecordingID;
																		getDuration($scope.serviceRecording.ServiceStartDate, $scope.serviceRecording.ServiceEndDate);
																		if ($scope.serviceRecording.ServiceStartDate) {
																				setStartDateTime($scope.serviceRecording.ServiceStartDate);
																		}
																		if ($scope.serviceRecording.ServiceEndDate) {
																				setEndDateTime($scope.serviceRecording.ServiceEndDate);
																				$rootScope.$broadcast('isCallEnded', true);
																		}
																		if (!$scope.serviceRecording.AttendedList) {
																				$scope.serviceRecording.AttendedList = [];
																		}
																		if (!$scope.serviceRecording.AdditionalUserList) {
																				$scope.serviceRecording.AdditionalUserList = [];
																		}
																		var additionalUserList = [];
																		var promiseAll = [];
																		if ($scope.serviceRecording.OrganizationID) {
																				helperService.updateLookupList($scope.programUnits, programUnits,
																								{ OrganizationID: $scope.serviceRecording.OrganizationID, DataKey: $scope.permissionKey });
																				setProgramUnitModel($scope.serviceRecording.OrganizationID);
																				if ($scope.organizationDetails) {
																						serviceRecordingService.getActiveServicesOnGet($scope.organizationDetails.DetailID, $scope.organizationDetails.OrganizationID, $scope.permissionKey, $scope.serviceRecording.ServiceItemID)
																										.then(function (activeServices) {
																												if (hasDetails(activeServices)) {
																														$scope.activeRecordingServices = activeServices;
																												}
																										});
																				}
																		}
																		var serviceLookupsOnGet = serviceRecordingService.getAllServiceLookupsOnGet($scope.serviceRecording, moduleComponentID);
																		angular.extend($scope, serviceLookupsOnGet);
																		angular.forEach($scope.serviceRecording.AdditionalUserList, function (item) {
																				var userbyID = $filter('filter')(providerData, { ID: item.UserID }, true);
																				var user = angular.copy(userbyID);
																				if (user && user.length > 0) {
																						user[0].ServiceRecordingAdditionalUserID = item.ServiceRecordingAdditionalUserID;
																						user[0].ServiceRecordingID = item.ServiceRecordingID;
																						user[0].UserID = user[0].ID;
																						user[0].IsActive = true;
																						additionalUserList.push(user[0]);
																				}
																				else {
																						promiseAll.push(getProviderbyid(item));
																				}
																		});
																		$q.all(promiseAll).then(function (data) {
																				angular.forEach(data, function (item1) {
																						additionalUserList.push(item1);
																				});

																				$scope.serviceRecording.AdditionalUserList = additionalUserList;

																				$scope.serviceRecording.ServiceRecordingVoidID && getVoidedServiceDetails();

																		});
																}
																else {
																		$scope.pageSecurity = 0;
																		getProviderOfServive();
																}
																initSignature();
																if (!$scope.serviceRecording.OrganizationID && $scope.isLawLiaison) {
																		populateProgramUnit(serviceName);
																}
																if (!$scope.serviceRecording.ServiceTypeID) {
																		$scope.serviceRecording.ServiceTypeID = SERVICE_TYPE.Unplanned;
																}
														}, function (errorStatus) {
																alertService.error('Unable to get Service Recording: ' + errorStatus + '.');
																setFormStatus(false);
														})
														.finally(function () {
																$scope.isLoading = false;
																if (!setFormtoInit) {
																		resetForm();
																}
														});
												};

												var getHeader = function (callCenterID, setFormtoInit) {
														$scope.isLoading = true;
														return callerInformationService.get(callCenterID)
														.then(function (data) {
																if (hasData(data)) {
																		var callerInfo = data.DataItems[0];
																		if (callerInfo.CallStartTime) {
																				setStartDateTime(callerInfo.CallStartTime);
																		}
																		if (callerInfo.CallEndTime) {
																				setEndDateTime(callerInfo.CallEndTime);
																		}
																		getDuration(callerInfo.CallStartTime, callerInfo.CallEndTime);
																		return getServiceRecording(callCenterID, setFormtoInit);
																}
																else {
																		getDefaultServiceRecording();
																}
														}, function (errorStatus) {
																getDefaultServiceRecording();
																alertService.error('Unable to get Service Recording: ' + errorStatus);
														})
														.finally(function () {
																$scope.isLoading = false;
														});
												};

												//Vaidate the Service Call Start/End Time
												$scope.validateCallTime = function () {
														$scope.serviceRecording.CallStartDate = angular.isDate($scope.serviceRecording.CallStartDate) ? $filter('formatDate')($scope.serviceRecording.CallStartDate, 'MM/DD/YYYY') : $scope.serviceRecording.CallStartDate;
														$scope.serviceRecording.CallEndDate = angular.isDate($scope.serviceRecording.CallEndDate) ? $filter('formatDate')($scope.serviceRecording.CallEndDate, 'MM/DD/YYYY') : $scope.serviceRecording.CallEndDate;
														var fullStartDateTime = new Date($scope.serviceRecording.CallStartDate + ' ' + $filter('toStandardTime')($scope.serviceRecording.CallStartTime) + ' ' + $scope.serviceRecording.CallStartAMPM);
														var fullEndDateTime = new Date($scope.serviceRecording.CallEndDate + ' ' + $filter('toStandardTime')($scope.serviceRecording.CallEndTime) + ' ' + $scope.serviceRecording.CallEndAMPM);
														var currentTime = new Date();
														//The currenttime needed to be formatted correctly
														var validTime =
																		(!moment(fullStartDateTime).isValid() || !moment(fullEndDateTime).isValid()) ||
														(fullEndDateTime <= currentTime);
														if ($scope.serviceRecording.CallEndTime != "" && $scope.serviceRecording.CallEndTime != null) {
																validTime = (fullStartDateTime <= fullEndDateTime);
														}
														return validTime;
												};

												//Validate whether the Time Format is correct
												var formatTimeToDate = function (dateVal, timeVal, AMPM, timeSelector) { //format should be hh:mm tt
														var d = new Date(dateVal);
														timeVal = timeVal || '';
														if (timeVal.indexOf(':') == -1) {
																timeVal = timeVal.substring(0, 2) + ':' + timeVal.substring(2, timeVal.length);
														}
														var hr = timeVal.substring(0, timeVal.indexOf(':'));
														var min = timeVal.substring(timeVal.indexOf(':') + 1, timeVal.length);
														//Validate the Start/End Time
														validateTimeFormat(hr, min, timeSelector);
														if (AMPM == "PM" && hr != 12)      //checks if PM, adds 12 hours
																hr = +hr + +12;
														if (AMPM == "AM" && hr == 12)      //checks if PM, adds 12 hours
																hr = +hr - +12;
														//Return the Start/End DateTime
														return new Date(d.setHours(hr, min));
												};

												//Check for the valid time
												var validateTimeFormat = function (hour, minute, timeSelector) {
														var isValidTime = true;
														if (hour && hour.length > 0 && minute && minute.length > 0) {
																if (hour < 1 || hour > 12) {
																		isValidTime = false;
																}
																if (minute < 0 || minute > 59) {
																		isValidTime = false;
																}
																//Validate the Start/End Time
																setFormValidations(timeSelector, 'pattern', isValidTime);
														}
														else {
																//Mark Start/End Time valid
																setFormValidations(timeSelector, 'pattern', true);
														}
														//Return the valid/invalid result
														return isValidTime;
												};

												//Executes when the CallEndDate field is changed
												$scope.validateRequiredCallEndDate = function () {
														$scope.calcDuration();
														$scope.validateMinStartDate();
												};

												//Executes when the CallStartDate field is changed
												$scope.validateRequiredCallStartDate = function () {
														$scope.calcDuration();
														$scope.validateMinStartDate();
												};

												// Validate the start date against the minimum allowed date object, wich is set to 1/1/1970
												$scope.validateMinStartDate = function () {
														if (new moment($scope.serviceRecording.CallStartDate)._d < $scope.CallMinStartDate) {
																$scope.ctrl.serviceRecordingForm.recordingServiceStartDate.$error.lessThanMinValidDate = true;
																$scope.ctrl.serviceRecordingForm.recordingServiceStartDate.$invalid = true;
																$scope.ctrl.serviceRecordingForm.recordingServiceStartDate.$valid = false;
																$scope.ctrl.serviceRecordingForm['recordingServiceStartDate'].$setValidity('lessThanMinValidDate', false);
														}
												};

												//Set the form validations
												var setFormValidations = function (elemSelector, error, isValid) {
														if ($scope.ctrl.serviceRecordingForm[elemSelector]) {
																$scope.ctrl.serviceRecordingForm[elemSelector].$setValidity(error, isValid);

																var errorControlBlock = angular.element("#" + elemSelector + "Container");
																if ($.isEmptyObject($scope.ctrl.serviceRecordingForm[elemSelector].$error)) {
																		errorControlBlock.removeClass('has-error');
																}
																else {
																		errorControlBlock.addClass('has-error');
																		$scope.ctrl.serviceRecordingForm[elemSelector].$invalid = true;
																		$scope.ctrl.serviceRecordingForm[elemSelector].$valid = false;
																}
														}
												};

												var setFormStatus = function (value) {
														var stateDetail = {
																stateName: $state.current.name, validationState: value ? VALIDATION_STATE.Valid : VALIDATION_STATE.Invalid
														};
														$rootScope.$broadcast('rightNavigationCallCenterHandler', stateDetail);
												};

												var saveServiceRecording = function (isUpdate, referralClientInformation) {
														if (!isUpdate) {
																return serviceRecordingService.addServiceRecording(referralClientInformation);
														}
														else {
																return serviceRecordingService.updateServiceRecording(referralClientInformation);
														}
												};

												var resetDateTimeValidations = function (timeSelector) {
														$('#' + timeSelector + 'Container').removeClass('has-error');
														$('#' + timeSelector + 'Error').removeClass('ng-show').addClass('ng-hide');
														$('#' + timeSelector + 'FutureError').removeClass('ng-show').addClass('ng-hide');
														$scope.ctrl.serviceRecordingForm[timeSelector].$invalid = false;
														$scope.ctrl.serviceRecordingForm[timeSelector].$valid = true;
												};

												var next = function () {
														angular.extend($stateParams, {
																CallCenterHeaderID: CallCenterHeaderID,
																ContactID: $stateParams.ContactID
														});
														if ($state.current.name.indexOf('crisisline') >= 0) {
																$state.go("callcenter.crisisline.columbiasuicidescale", $stateParams);
														}
														else {
																$state.go("callcenter.lawliaison.progressnotes", $stateParams);
														}
												};

												var saveRecording = function (isNext, mandatory, hasErrors) {
														var dfd = $q.defer();
														if ((!mandatory && !hasErrors && !isNext) || (mandatory && !hasErrors && !isNext) || (!mandatory && !hasErrors && isNext) || (mandatory && !hasErrors && isNext)) {
																var isDirty = formService.isDirty();
																if (!isDirty && isNext) {
																		next();
																		dfd.resolve(true);
																}
																else if (isDirty || $scope.formDetailsChanged) {
																		$scope.serviceRecording.EndDate = $filter('formatDate')(new Date(), 'MM/DD/YYYY HH:mm:ss');
																		$scope.serviceRecording.CallStartDate = angular.isDate($scope.serviceRecording.CallStartDate) ? $filter('formatDate')($scope.serviceRecording.CallStartDate, 'MM/DD/YYYY') : $scope.serviceRecording.CallStartDate;
																		$scope.serviceRecording.CallEndDate = angular.isDate($scope.serviceRecording.CallEndDate) ? $filter('formatDate')($scope.serviceRecording.CallEndDate, 'MM/DD/YYYY') : $scope.serviceRecording.CallEndDate;
																		if ($scope.serviceRecording.CallEndDate && $scope.serviceRecording.CallEndTime) {
																				$scope.serviceRecording.ServiceEndDate = $scope.serviceRecording.CallEndDate + ' ' + $filter('toStandardTime')($scope.serviceRecording.CallEndTime) + ' ' + $scope.serviceRecording.CallEndAMPM;
																		}
																		else if ($scope.serviceRecording.CallEndDate) {
																				$scope.serviceRecording.ServiceEndDate = $scope.serviceRecording.CallEndDate + ' ' + ignoreTime;;
																				$scope.serviceRecording.ServiceEndTime = null;
																		}
																		else {
																				$scope.serviceRecording.ServiceEndDate = $scope.serviceRecording.ServiceEndTime = null;
																		}
																		if ($scope.serviceRecording.CallStartDate && $scope.serviceRecording.CallStartTime) {
																				$scope.serviceRecording.ServiceStartDate = $scope.serviceRecording.CallStartDate + ' ' + $filter('toStandardTime')($scope.serviceRecording.CallStartTime) + ' ' + $scope.serviceRecording.CallStartAMPM;
																		}
																		else if ($scope.serviceRecording.CallStartDate) {
																				$scope.serviceRecording.ServiceStartDate = $scope.serviceRecording.CallStartDate + ' ' + ignoreTime;
																				$scope.serviceRecording.ServiceStartTime = null;
																		}
																		else {
																				$scope.serviceRecording.ServiceStartDate = $scope.serviceRecording.ServiceStartTime = null;
																		}
																		$scope.serviceRecording.CallCenterHeaderID = CallCenterHeaderID;
																		$scope.serviceRecording.ServiceRecordingSourceID = ServiceRecordingSourceID;
																		
																		$rootScope.$broadcast('isCallEnded', $scope.serviceRecording.CallEndDate ? true : false);
																		var isUpdate = ($scope.serviceRecording.ServiceRecordingID && $scope.serviceRecording.ServiceRecordingID !== 0) ? true : false;
																		
																		return saveRecordingData(isNext, isUpdate, $scope.serviceRecording);
																}
																else {
																		dfd.resolve(null);
																}
														}
														else {
																dfd.resolve(true);
														}
														return dfd.promise;
												};

												$scope.validateServiceRecordingWhenSigned = function () {
														var isValidServiceRecordingWhenSigned = false;
														if ($scope.isSigned) {
																if (
																				($scope.serviceRecording.CallStartDate == null || $scope.serviceRecording.CallStartDate == undefined || $scope.serviceRecording.CallStartDate === "") ||
																				($scope.serviceRecording.CallEndDate == null || $scope.serviceRecording.CallEndDate == undefined || $scope.serviceRecording.CallEndDate === "") ||
																				($scope.serviceRecording.CallStartTime == null || $scope.serviceRecording.CallStartTime == undefined || $scope.serviceRecording.CallStartTime === "") ||
																				($scope.serviceRecording.CallEndTime == null || $scope.serviceRecording.CallEndTime == undefined || $scope.serviceRecording.CallEndTime === "") ||
																				($scope.serviceRecording.CallStartAMPM == null || $scope.serviceRecording.CallStartAMPM == undefined || $scope.serviceRecording.CallStartAMPM === "") ||
																				($scope.serviceRecording.CallEndAMPM == null || $scope.serviceRecording.CallEndAMPM == undefined || $scope.serviceRecording.CallEndAMPM === "")
																) {
																		return isValidServiceRecordingWhenSigned;
																}
																else {
																		isValidServiceRecordingWhenSigned = true;
																}
														}
														else {
																isValidServiceRecordingWhenSigned = true;
														}
														return isValidServiceRecordingWhenSigned;
												};

												$scope.save = function (isNext, mandatory, hasErrors) {
														if (!$scope.validateServiceRecordingWhenSigned()) {
																alertService.error("Service Recording Date & Time Fields cannot be blank once the service is signed.");
																return;
														}
														var dfd = $q.defer();
														var action = ($scope.serviceRecording.ServiceRecordingID && $scope.serviceRecording.ServiceRecordingID !== 0) ? 'updated' : 'added';
														var isDirty = formService.isDirty();
														//Vaidate the Call Start/End Time
														if (!$scope.validateCallTime()) {
																alertService.error("Please enter valid End Time.");
																dfd.resolve(false);
																return false;
														}
														if (!mandatory && isNext && hasErrors) {
																next();
																dfd.resolve(true);
														}
														else if (!hasErrors && $scope.signature.CredentialID && formService.isDirty($scope.signatureForm.$name)) { //Validate the Digital Signature
																var credentialId = $scope.signature.CredentialID;
																return validateSignature()
																.then(function (response) {
																		if (response) {
																				return saveRecording(isNext, mandatory, hasErrors)
																				.then(function (res) {
																						if (isSaveSignature) {
																								isSaveSignature = false;
																								return saveSignature(credentialId)
																								.then(function (resp) {
																										return postSave(res, action, isNext, isDirty);
																								});
																						}
																						else {
																								return postSave(res, action, isNext, isDirty);
																						}
																				});
																		}
																		else {
																				dfd.resolve();
																		}
																});
														}
														else {
																return saveRecording(isNext, mandatory, hasErrors)
																.then(function (res) {
																		return postSave(res, action, isNext, isDirty);
																});
														}
														return dfd.promise;
												};

												function saveRecordingData(isNext, isUpdate, serviceRecording) {
														var deferred = $q.defer();
														saveServiceRecording(isUpdate, serviceRecording)
														.then(function (response) {
																if (response.ResultCode == 15) {
																		bootbox.confirm("Data has been changed. Do you want to reload?", function (result) {
																				if (result) {
																						init();
																				}
																		});
																		deferred.reject(response.ResultMessage);
																		return;
																}
																if (!isUpdate) {
																		$scope.serviceRecording.ServiceRecordingID = response.ID;
																}
																documentID = $scope.serviceRecording.ServiceRecordingID;
																resetForm();
																deferred.resolve(response);
														}, function (errorStatus) {
																alertService.error('OOPS! Something went wrong.');
																deferred.reject();
														}, function (notification) {
																alertService.warning(notification);
														}
														);
														return deferred.promise;
												};

												var postSave = function (response, action, isNext, isDirty) {
														if (angular.isObject(response) && response.ResultCode !== 0) {
																alertService.error(response.ResultMessage);
																return $scope.promiseNoOp();
														}
														else {
																//save workflow Header details.                            
																WorkflowHeaderService.AddWorkflowHeader({ WorkflowDataKey: $state.current.data.workflowDataKey, RecordHeaderID: $stateParams.CallCenterHeaderID, ContactID: $stateParams.ContactID });
																if (isDirty) {
																		callerInformationService.updateModifiedOn($stateParams.CallCenterHeaderID);
																		alertService.success('Service Recording has been ' + action + ' successfully.');
																		alertLawLiaisonProgressNoteCompletion();
																}
																if (isNext) {
																		next();
																}
																else {
																		init();
																		resetDateTimeValidations(START_TIME_STR);
																}
																return $scope.promiseNoOp();
														}
												};

												var checkServiceComplete = function () {
														var deferred = $q.defer();
														var registrationDeferred = $q.defer();
														getPromises.push(registrationDeferred.promise);
														registrationService.get($stateParams.ContactID)
														.then(function (data) {
																if (hasData(data)) {
																		isMRN = data.DataItems[0].MRN ? true : false;
																}
																registrationDeferred.resolve(data);
														});
														var CallerInfoDeferred = $q.defer();
														getPromises.push(CallerInfoDeferred.promise);
														callerInformationService.get($stateParams.CallCenterHeaderID)
														.then(function (callData) {
																if (hasData(callData)) {
																		contactTypeID = callData.DataItems[0].ContactTypeID;
																}
																CallerInfoDeferred.resolve(callData);
														});
														$q.all(getPromises)
														.then(function (data) {
																return deferred.resolve(data);
														});
														return deferred.promise;
												};

												//Validate the Digital Signature
												var validateSignature = function () {
														if (!$scope.signatureVerified) {
																if ($scope.signature.Password) {
																		alertService.error('Document is not signed with valid credentials.');
																}
																else if ($scope.signature.CredentialID) {
																		alertService.error('Please enter Digital Password.');
																}
																return $scope.promiseNoOp();
														}
														var deferred = $q.defer();
														checkServiceComplete()
														.then(function (response) {
																if (!isMRN && (contactTypeID == CONTACT_TYPE.New || contactTypeID == CONTACT_TYPE.Existing)) {
																		alertService.error('This contact does not have an MRN.  Please register the contact or change their Contact Type.');
																		initSignature();
																		deferred.resolve();
																}
																else {
																		isSaveSignature = true;
																		deferred.resolve(true);
																}
														});
														return deferred.promise;
												};

												var saveSignature = function (credentialId) {
														var deferred = $q.defer();
														saveDSignature(credentialId)
														.then(function (response) {
																deferred.resolve(response);
														});
														return deferred.promise;
												};

												$scope.addWhoAttendedToList = function () {
														//the Maximum length of the attendees is 5  
														var activeList = $scope.serviceRecording.AttendedList.filter(function (item) { return item.IsActive }).length;
														if (activeList < 5) {
																//Code to add text in $scope.WhoAttended to $scope.serviceRecording.AttendedList
																if ($scope.WhoAttended && $scope.WhoAttended.length > 0) {
																		var attended = {
																				ServiceRecordingAttendeeID: 0, Name: $scope.WhoAttended, IsActive: true, ModifiedOn: new Date()
																		};
																		$scope.serviceRecording.AttendedList.push(attended);
																		$scope.WhoAttended = '';
																		$scope.formDetailsChanged = true;
																}
														}
														else {
																alertService.error("Attendees cannot exceed more than 5.");
																return;
														}
												};

												$scope.removeAttendee = function (attended) {
														var index = $scope.serviceRecording.AttendedList.indexOf(attended);
														if (index !== -1 && $scope.serviceRecording.AttendedList[index]) {
																if ($scope.serviceRecording.AttendedList[index].ServiceRecordingAttendeeID > 0) {   //If existing record
																		$scope.serviceRecording.AttendedList[index].IsActive = false;
																}
																else {  //If newly created, no need to save deleted record
																		$scope.serviceRecording.AttendedList.splice(index, 1);
																}
																$scope.formDetailsChanged = true;
														}
												};

												$scope.addCoProviderToList = function (item) {
														var copyOfItem = angular.copy(item);
														var isAlreadyInList = $filter('filter')($scope.serviceRecording.AdditionalUserList, function (addCoPro) {
																return (addCoPro.ID == copyOfItem.ID) && addCoPro.IsActive == 1;
														}, true);
														if (copyOfItem && isAlreadyInList.length == 0) {
																copyOfItem.IsActive = true;
																copyOfItem.UserID = copyOfItem.ID;
																$scope.serviceRecording.AdditionalUserList.push(copyOfItem);
																$scope.formDetailsChanged = true;
														}
														$scope.serviceRecording.CoProvider = null;
												};

												$scope.removeCoProvider = function (item) {
														var index = $scope.serviceRecording.AdditionalUserList.indexOf(item);
														if (index !== -1 && $scope.serviceRecording.AdditionalUserList[index]) {
																if ($scope.serviceRecording.AdditionalUserList[index].ServiceRecordingAdditionalUserID > 0) {   //If existing record
																		$scope.serviceRecording.AdditionalUserList[index].IsActive = false;
																}
																else {  //If newly created, no need to save deleted record
																		$scope.serviceRecording.AdditionalUserList.splice(index, 1);
																}
														}
												};

												$scope.validateProviderFutureDate = function () {
														if (!$scope.isCrisisLine && !$scope.serviceRecording.CallEndDate && !$scope.serviceRecording.CallEndTime && $scope.signatureVerified) {
																dateRequired(true);
														}
														else {
																dateRequired(false);
														}
														var errorControlBlock = angular.element("#startTimeContainer");
														var errorControl = angular.element("#startTimeFutureError");
														var formControl = $scope.ctrl.serviceRecordingForm.startTime;
														var formName = $scope.ctrl.serviceRecordingForm;
														var dateControl = $filter('formatDate')($scope.serviceRecording.CallStartDate, 'MM/DD/YYYY');
														var datePart = $filter('formatDate')(dateControl, 'MM/DD/YYYY');
														var selector = START_TIME_STR;
														dateTimeValidatorService.validateFutureDateTime(errorControlBlock, errorControl, formControl, dateControl, $scope.serviceRecording.CallStartTime, $scope.serviceRecording.CallStartAMPM, selector, formName);
														errorControlBlock = angular.element("#endTimeContainer");
														errorControl = angular.element("#endTimeFutureError");
														formControl = $scope.ctrl.serviceRecordingForm.endTime;
														dateControl = $filter('formatDate')($scope.serviceRecording.CallEndDate, 'MM/DD/YYYY');
														datePart = $filter('formatDate')(dateControl, 'MM/DD/YYYY');
														var isRequired = formControl ? $scope.ctrl.serviceRecordingForm.endTime.$error.required : false;
														var isDate = formControl ? $scope.ctrl.serviceRecordingForm.endTime.$error.date : false;
														selector = END_TIME_STR;
														if (!isRequired || (isRequired && isDate)) {
																dateTimeValidatorService.validateFutureDateTime(errorControlBlock, errorControl, formControl, dateControl, $scope.serviceRecording.CallEndTime, $scope.serviceRecording.CallEndAMPM, selector, formName);
														}
												};

												function dateRequired(isRequired) {
														if (isRequired) {
																angular.element("#endDate").addClass('has-error');
														}
														else {
																angular.element("#endDate").removeClass('has-error');
														}
												};

												$scope.calcDuration = function (isCallEnd) {
														//Clear out the duration at the beginning so that if its an invalid date or duration is negative because of changes we do not retain the old calculated duration in the model
														$scope.serviceRecording.Duration = '';
														if (!isCallEnd) {
																$scope.validateProviderFutureDate();
														}
														try {
																var start = formatTimeToDate($scope.serviceRecording.CallStartDate, $scope.serviceRecording.CallStartTime, $scope.serviceRecording.CallStartAMPM, START_TIME_STR);
																//The following if condition ensures we don't get NaN and - Duration. I would have prefered this code to be inside the condition block above. These changes need to be made to the crisis line controller.
																if (start == 'Invalid Date' || start == undefined) {
																		return;
																}
																
																var end = formatTimeToDate($scope.serviceRecording.CallEndDate, $scope.serviceRecording.CallEndTime, $scope.serviceRecording.CallEndAMPM, END_TIME_STR);
																if (end == 'Invalid Date' || end == undefined) {
																		serviceRecordingService.setServiceRecordingTime(start, null);
																		return;
																}
																var elapsed = end - start; // time in milliseconds
																if (elapsed < 0) {
																		return;
																}
																serviceRecordingService.setServiceRecordingTime(start, end);
																$scope.smallTimeError = elapsed >= 0 ? false : true;
																$scope.serviceRecording.Duration = calculateDuration(start, end);
														}
														catch (err) {
																$log.debug("Error in Calculation: " + err.toString());
																$scope.serviceRecording.Duration = '';
														};
												};

												var millisToMinutesAndSeconds = function (millis) {
														var minutes = Math.floor(millis / 60000);
														var seconds = ((millis % 60000) / 1000).toFixed(0);
														return minutes + " mins " + (seconds < 10 ? '0' : '') + seconds + " secs ";
												};

												var getVoidedServiceDetails = function () {
														$scope.isVoidedShown = true;
														if ($state.current.name.indexOf('crisisline') >= 0) {
																if (roleSecurityService.hasPermission('CrisisLine-CrisisLine-Approver', PERMISSION.UPDATE)) {
																		$scope.isReadOnlyForm = false;
																}
																else {
																		$scope.isReadOnlyForm = true;
																}
														}
														voidService.getVoidRecordedService($scope.serviceRecording.ServiceRecordingID)
														.then(function (voidServiceResponse) {
																if (hasData(voidServiceResponse)) {
																		$scope.voidRecordedServiceReasons = lookupService.getLookupsByType(LOOKUPTYPE.VoidRecordedServiceReason);
																		$scope.voidModel = voidServiceResponse.DataItems[0];
																		$scope.voidModel.DataEntryErrorCheck = true;
																		$scope.voidModel.DataEntryErrorID = 2;
																		$scope.voidModel.VoidDate = $filter('formatDate')($scope.voidModel.ModifiedOn, 'MM/DD/YYYY');
																		$scope.voidModel.VoidTime = $filter('formatDate')($scope.voidModel.ModifiedOn, 'hh:mm A');
																		var item = $filter('filter')(lookupService.getLookupsByType("Users"), { ID: $scope.voidModel.ModifiedBy }, true)[0];
																		$scope.voidModel.UserName = item.Name;
																		resetForm();
																}
														});
												};

												$rootScope.$on('callCenterEndCall', function (event, callEndDateTime, serviceRecordingID) {
														isCallEnded = true;
														if (formService.isDirty()) {
																var serviceCopy = angular.copy($scope.serviceRecording);
																getHeader(CallCenterHeaderID, true)
																.then(function () {
																		serviceCopy.SystemModifiedOn = $scope.serviceRecording.SystemModifiedOn;
																		angular.extend($scope.serviceRecording, serviceCopy);
																		$scope.serviceRecording.CallEndAMPM = $filter('toStandardTimeAMPM')(getCurrentTime(moment(callEndDateTime).toDate()));
																		$scope.serviceRecording.ServiceRecordingID = serviceRecordingID;
																		$scope.calcDuration(true);
																});
														}
														else {
																getHeader(CallCenterHeaderID);
														}
												});

												$scope.printReport = function (isNext, mandatory, hasErrors, keepForm, next) {
														$scope.save(isNext, mandatory, hasErrors, keepForm, next).then(function (response) {
																printService.initPrint(CallCenterHeaderID, ServiceRecordingSourceID, documentID, $state.current.data.workflowDataKey).then(function (data) {
																		$scope.reportModel = data;
																		$scope.reportModel.isDigitalSignature = true;
																		$scope.reportModel.HasLoaded = true;
																		$('#reportModal').modal('show');
																});
														});
												};

												$scope.checkServiceStatus = function () {
														return (
																		$scope.serviceRecording.ServiceStatusID === $scope.calltoMCOTID ||
																		$scope.serviceRecording.ServiceStatusID === $scope.StatusNotSelected ||
																		!$scope.serviceRecording.ServiceStatusID // This checks for null, undefined and 0 
														);
												};

												init();
										}]);
})();