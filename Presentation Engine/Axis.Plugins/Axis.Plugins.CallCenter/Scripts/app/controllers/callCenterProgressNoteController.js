angular.module('xenatixApp')
.controller('callCenterProgressNoteController', ['$scope', 'alertService', 'lookupService', '$filter', '$stateParams', '$rootScope', 'formService', '$state', 'callCenterProgressNoteService', 'serviceRecordingService', 'sectionID', 'responseID', 'viewMode', 'callerInformationService', 'registrationService', 'contactPhoneService', 'navigationService', 'credentialSecurityService', 'eSignatureService', '$q', 'cacheService', 'roleSecurityService', 'assessmentPrintService', 'contactBenefitService', 'additionalDemographyService', 'contactRaceService', 'contactAddressService', 'contactSSNService', 'userProfileService', 'dateTimeValidatorService', 'WorkflowHeaderService',
function ($scope, alertService, lookupService, $filter, $stateParams, $rootScope, formService, $state, callCenterProgressNoteService, serviceRecordingService, sectionID, responseID, viewMode, callerInformationService, registrationService, contactPhoneService, navigationService, credentialSecurityService, eSignatureService, $q, cacheService, roleSecurityService, printService, contactBenefitService, additionalDemographyService, contactRaceService, contactAddressService, contactSSNService, userProfileService, dateTimeValidatorService, WorkflowHeaderService) {
		var self = this;
		//TODO: Get some better idea to get rid of this
		var crisisLineContactTypeID = 9;
		var lawLiaisonContactTypeID = 10;
		var activeClientStatusID = 1;
		var inActiveClientStatusID = 2;
		var serviceRecordingSourceID = SERVICE_RECORDING_SOURCE.LawLiaison;
		var callTypeOther = CALL_TYPE.Other;
		var documentTypeID = DOCUMENT_TYPE.CallCenterProgressNote;
		$scope.permissionKey = $state.current.data.permissionKey;
		$scope.hasUpdatePrivillege = roleSecurityService.hasPermission(CallCenterPermissionKey.CallCenter_LawLiaison, PERMISSION.UPDATE);
		self.phoneTypeOtherId = 4;
		self.lawLiaisonType = 2;
		var defaultTime = "00:00:00";
		var adjustedTime = "00:00:01";
		self.crisisCallType = 1;
		self.pageDisableSecurity = undefined;
		var lawLiaisonFollowUp = cacheService.get('lawLiaisonFollowUp');
		if (lawLiaisonFollowUp) {
				$scope.isFollowup = lawLiaisonFollowUp.followupRequired;
		}
		self.ProviderDate = 'ProviderDate';
		//Get rid till here
		$scope.stopEnter = true;
		if ($state.current.name.indexOf('lawliaison') >= 0) {
				$scope.isLawLiaison = true;
				var serviceName = 'Law Liaison';
				$scope.disableISCallerSameAsContact = false;
				$scope.isReadOnly = cacheService.get('IsReadOnlyLLScreens');
				$scope.assessmentNoAccess = $scope.isReadOnly ? true : false;
		}
		else {
				$scope.assessmentNoAccess = false;
				$scope.isReadOnly = false;
				$scope.disableISCallerSameAsContact = false;
		}

		$scope.isCrisisLine = $state.current.name.indexOf('crisisline') >= 0 ? true : false;
		self.dateOptions = {
				formatYear: 'yy',
				startingDay: 1,
				showWeeks: false
		};
		if ($scope.isCrisisLine) {
				$scope.isManager = cacheService.get('IsManagerAccess');
				$scope.isCreator = cacheService.get('IsCreatorAccess');
				$scope.disableComments = $stateParams.ReadOnly == 'view' ? true : false;
		}
		else {
				$scope.isManager = true;
				$scope.isCreator = true;
		}
		self.callCenterTypeID = $state.current.name.indexOf('crisisline') >= 0 ? self.crisisCallType : self.lawLiaisonType;
		$scope.stopNext = (self.callCenterTypeID !== self.crisisCallType);
		var init = function () {
				$scope.isAssessmentIntegrated = true;
				self.headerID = $stateParams.CallCenterHeaderID;
				self.enableLiasion = ($state.current.name.indexOf('crisisline') >= 0);

				//gets assessment part of the page
				self.disableControl = (self.callCenterTypeID != self.lawLiaisonType);
				initFormDetails();
				get();

				initSignature();
				alertForServiceCompletion();

				angular.extend($stateParams, {
						SectionID: sectionID,
						ResponseID: responseID
				});
				$state.transitionTo($state.current.name, $stateParams);

		};

		$scope.preventStopSave = $stateParams.ReadOnly == 'view' ? true : false;
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

				navigationService.get().then(function (response) {
						var data = response.DataItems[0];
						//Get UserID, digital password
						$scope.userID = data.UserID;
						$scope.signature.UserFullName = data.UserFullName;
						$scope.signature.digitalPassword = data.DigitalPassword;
						//Get User Credentials
						$scope.userCredentials = data.UserCredentials;
						if ($scope.userCredentials && $scope.userCredentials.length == 1) {
								$scope.signature.CredentialID = $scope.userCredentials[0].CredentialID;
								$scope.checkPermission();
						}
						signatureFormReset();
						getSignature();
				});
		};

		var signatureFormReset = function () {
				$rootScope.formReset($scope.signatureForm, "signatureForm");
		};

		var getSignature = function () {
				eSignatureService.getDocumentSignatures(documentTypeID, self.headerID).then(function (response) {
						if (hasData(response)) {
								$scope.signature = response.DataItems[0];
								$scope.signature.DateSigned = $filter('formatDate')($scope.signature.ModifiedOn, 'MM/DD/YYYY');
								$scope.signature.UserFullName = lookupService.getText("Users", $scope.signature.EntityId);
								$scope.isCredentialReadonly = true;
								$scope.isPasswordReadonly = true;
								$scope.isSigned = true;

								var userCredentials = $filter('filter')($scope.userCredentials, { CredentialID: $scope.signature.CredentialID }, true);
								if (!userCredentials || userCredentials.length == 0) {
										$scope.userCredentials = $filter('filter')(lookupService.getLookupsByType('Credential'), { CredentialID: $scope.signature.CredentialID }, true);
								}

								progressNoteSigned();
								signatureFormReset();
						}
				});
		};
		var progressNoteSigned = function () {
				if (roleSecurityService.hasPermission($scope.permissionKey, $rootScope.resolvePermission($scope.pageDisableSecurity))) {
						if ($scope.isLawLiaison)
								$scope.isPageDisable = true;
				}
				$scope.assessmentNoAccess = true;
		};

		var alertForServiceCompletion = function () {
				if (self.callCenterTypeID == self.lawLiaisonType && !$scope.isReadOnly) {
						eSignatureService.getDocumentSignatures(documentTypeID, self.headerID).then(function (response) {
								if (hasData(response)) {
										serviceRecordingService.getServiceRecording(self.headerID, SERVICE_RECORDING_SOURCE.LawLiaison).then(function (data) {
												var serviceRecordingID = hasData(data) ? data.DataItems[0].ServiceRecordingID : 0;
												eSignatureService.getDocumentSignatures(DOCUMENT_TYPE.ServiceRecording, serviceRecordingID).then(function (response) {
														if (!hasData(response)) {
																alertService.warning('Please be sure to complete Service Details information.');
														}
												});
										});
								}
						});
				}
		};
	$rootScope.$on('callCenterEndCall', function (event, callEndDateTime) {
				if (callEndDateTime != null) {
						self.providerDetail.ProviderEndTime = $filter('formatDate')(callEndDateTime, 'hh:mm');
						self.providerDetail.CallEndAMPM = $filter('toStandardTimeAMPM')(dateTimeValidatorService.getCurrentMeridian(moment(callEndDateTime).toDate()));
				}
		});

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

		//Validate the Digital Signature
		var validateSignature = function () {
				var dfd = $q.defer();
				if (formService.isDirty('signatureForm')) {
						if (!$scope.signatureVerified) {
								alertService.error('Document is not signed with valid credentials.');
								dfd.resolve(false);
						}
						else {
								return saveDSignature();
						}
				}
				else {
						dfd.resolve(true);
				}
				return dfd.promise;
		};

		var saveDSignature = function () {
				var dfd = $q.defer();
				var signature = {
						DocumentId: self.headerID,      //Primary key of record against which signature will be saved
						DocumentTypeId: documentTypeID,             //Type it could be CallCenter, Discharge Note etc
						EntitySignatureId: null,                    //Should be passed NULL for D-Signature, should be evaluated with in sproc for user
						EntityId: $scope.userID,                    //User Id
						EntityTypeId: 1,                            //Reference Id for type of entity. Here it is for User
						SignatureBlob: null,                         //Signature in byte format
						ModifiedOn: $filter('formatDate')(new Date(), 'MM/DD/YYYY'),
						CredentialID: $scope.signature.CredentialID
				};
				eSignatureService.saveDocumentSignature(signature).then(function (response) {
						if (response.data.ResultCode === 0) {
								setFormStatus(true);
								alertService.success('Signature status saved successfully!');
								dfd.resolve(true);
						}
						else {
								var msg = 'Error while saving signature! Please reload the page and try again.';
								alertService.error(msg);
								deferred.resolve(false);
						}
				},
				function (errorStatus) {
						alertService.error('OOPS! Something went wrong.');
						deferred.resolve(false);
				});
				return dfd.promise;
		};

		var initFormDetails = function () {
				self.ClientPhones = [];
				self.ClientPhones.push(objPhone());

				self.callerInformation = {
						ContactID: null,    //AxisID is ContactID
						FirstName: '',
						LastName: '',
						DOB: null,
						ContactTypeID: ($state.current.name.indexOf('crisisline') >= 0) ? crisisLineContactTypeID : lawLiaisonContactTypeID,
						ModifiedOn: new Date(),
						THSAID: null
				};

				self.clientDetails = {
						ContactID: null,
						ContactTypeID: ($state.current.name.indexOf('crisisline') >= 0) ? crisisLineContactTypeID : lawLiaisonContactTypeID,
						FirstName: '',
						LastName: '',
						ClientTypeID: null,
						DOB: null,
						ModifiedOn: new Date()
				};

				initPhones();

				self.callerDetails = {
						CallCenterHeaderID: 0,
						ProviderID: null,
						ReasonCalled: '',
						OtherInformation: '',
						CallStatusID: null,
						preComments: '',
						newComment: '',
						Disposition: '',
						ProgramUnitID: null,
						SuicideHomicideID: 2,
						CallCenterPriorityID: 1,
						CallCenterTypeID: self.callCenterTypeID,
						CallerID: null,
						ContactID: null,
						CallStartTime: new Date(),
						CallEndTime: null,
						CountyID: null,
						ModifiedOn: new Date(),
						IsCallerClientSame: false,
						FollowUpRequired: false
				};

				self.ProgressNoteDetails = {
						NoteHeaderID: 0,
						CallCenterHeaderID: 0,
						NatureofCall: '',
						CallTypeID: null,
						CallTypeOther: '',
						FollowupPlan: '',
						Disposition: '',
						Comments: '',
						newFollowupPlan: '',
						preFollowupPlan: '',
						newNatureofCall: '',
						preNatureofCall: '',
						ClientStatusID: null,
						ClientProviderID: null,
						ModifiedOn: new Date(),
						THSAID: null,               //TODO: Populate THSAID, AxisID, BehaviourCategoryID
						AxisID: null,
						BehavioralCategoryID: null
				};

				self.providerDetail = {
						ProviderDate: $filter('formatDate')(new Date(), 'MM/DD/YYYY'),
						ProviderStartTime: $filter('formatDate')(new Date(), 'hh:mm'),
						ProviderEndTime: '',
						disableEndTime: false,
						CallEndAMPM: null,
						CallStartAMPM: $filter('toStandardTimeAMPM')(dateTimeValidatorService.getCurrentMeridian(new Date())),
						enableProviderDate: false,
						enableProviderStartTime: false,
						enableProviderEndTime: false
				};
				if (!self.UserID) {
						navigationService.get().then(function (data) {
								$scope.cmtReporter = data.DataItems[0].UserFullName;
								self.UserID = data.DataItems[0].UserID;
								self.providerDetail.ProviderBy = self.UserID;      //default taken by to the logged in user
						}).finally(function () {
								resetForm();
						});
				}
				else {
						self.providerDetail.ProviderBy = self.UserID;         //default taken by to the logged in user
				}
		};

		var objPhone = function () {
				var obj = {
						ContactPhoneID: 0,
						ContactID: null,
						PhoneTypeID: null,
						Number: '',
						PhonePermissionID: null,
						IsPrimary: true,
						IsActive: true
				};
				return obj;
		};

		var resetForm = function () {
				resetProgressNote();
				resetCallerDetails();
				resetTakenDetail();
				resetAssessmentForm();
				resetHeader();
				resetFooter();
		};

		var resetProgressNote = function () {
				if (self.callCenterProgressNoteForm)
						$rootScope.formReset(self.callCenterProgressNoteForm);
		};

		var resetCallerDetails = function () {
				if (self.callerDetailsForm)
						$rootScope.formReset(self.callerDetailsForm);
		};

		var resetHeader = function () {
				if (self.headerDetailsForm)
						$rootScope.formReset(self.headerDetailsForm, self.headerDetailsForm.$name);
		};

		var resetFooter = function () {
				if (self.footerDetailsForm)
						$rootScope.formReset(self.footerDetailsForm, self.footerDetailsForm.$name);
		};

		var resetAssessmentForm = function () {
				if (self.assessmentForm)
						$rootScope.formReset(self.assessmentForm, self.assessmentForm.$name);
		};

		var resetTakenDetail = function () {
				if (self.takenDetails)
						$rootScope.formReset(self.takenDetails, self.takenDetails.$name);
		};

		var setFormStatus = function (value) {
				eSignatureService.getDocumentSignatures(DOCUMENT_TYPE.CallCenterProgressNote, $stateParams.CallCenterHeaderID).then(function (response) {
						value = (response && response.DataItems && response.DataItems.length > 0) ? true : false;
						var stateDetail = { stateName: (self.callCenterTypeID === self.crisisCallType) ? "callcenter.crisisline.progressnotes" : "callcenter.lawliaison.progressnotes", validationState: value ? 'valid' : 'warning' };
						$rootScope.$broadcast('rightNavigationCallCenterHandler', stateDetail);
				});
		};

		var successMethod = function (data) {
				if (hasData(data)) {
						self.ProgressNoteDetails = data.DataItems[0];
						if (self.callerInformation.ContactTypeID == crisisLineContactTypeID || self.callerInformation.ContactTypeID == lawLiaisonContactTypeID) {
								self.ProgressNoteDetails.ClientStatusID = activeClientStatusID;
						}
						if (self.ProgressNoteDetails.IsCallerSame || (self.callCenterTypeID != self.lawLiaisonType)) {
								$scope.GetLawEnforcementContact(false, false, true);
						}
						else if (self.ProgressNoteDetails.NewCallerID && self.ProgressNoteDetails.NewCallerID != 0) {
								registrationService.get(self.ProgressNoteDetails.NewCallerID).then(callerSuccess.bind(this, true), errorMethod, notifyMethod);
								self.disableControl = false;
						}
						if (self.callCenterTypeID == self.lawLiaisonType) {
								if (self.ProgressNoteDetails.FollowupPlan) {
										self.ProgressNoteDetails.preFollowupPlan = parseJSON(self.ProgressNoteDetails.FollowupPlan);
								}
								if (self.ProgressNoteDetails.NatureofCall) {
										self.ProgressNoteDetails.preNatureofCall = parseJSON(self.ProgressNoteDetails.NatureofCall);
								}
						}
						if (hasData(data) && data.DataItems[0].ProgressNoteID) {
								setFormStatus(true);
						}
						else {
								setFormStatus(false);
						}
				} else {
						self.ProgressNoteDetails.NoteHeaderID = 0;
						alertService.error('Unable to get Progress Note information!');
				};
				for (var prop in $scope.responses) {
						var responselength = 1;
						break;
				}
				if (self.ProgressNoteDetails.ProgressNoteID != null || responselength) {
						$scope.pageDisableSecurity = 1;
				}
				else {
						$scope.pageDisableSecurity = 0;
				}
		};

		var failureMethod = function (data) {
				alertService.error('Unable to get caller information.');
				setFormStatus(false);
		};

		var errorMethod = function (err) {
				isSaving = false;
				alertService.error('OOPs something went wrong');
		};

		var notifyMethod = function (notify) {
		};

		var get = function () {
				var getDfr = $q.defer();
				var callerDfr = $q.defer();
				var noteDfr = $q.defer();

				self.isLoading = true;
				var promise = [];
				promise.push(callerDfr.promise);
				promise.push(noteDfr.promise);
				callerInformationService.get(self.headerID).then(getCallerInfoSuccessMethod, failureMethod, notifyMethod).finally(function () {
						callerDfr.resolve();
				});

				//TODO: Get Provider note details and populate the form
				callCenterProgressNoteService.get(self.headerID).then(successMethod, failureMethod, notifyMethod).finally(function () {
						noteDfr.resolve();
				});
				$q.all(promise).finally(function () {
						self.isLoading = false;
						resetForm();
						getDfr.resolve();
				});
				return getDfr.promise;
		};

		var getCallerInfoSuccessMethod = function (data) {
				if (hasData(data)) {
						self.callerDetails = data.DataItems[0];
						// Bug#21532 Program Unit should be fetched from services screen for law liaison
						// Bug#21836 Program Unit should be fetched from services screen first if empty than from callerinformation for crisis line
						if ($scope.isLawLiaison) {
								self.callerDetails.CountyID = null;
								self.callerDetails.ProgramUnitID = null;
						}
						serviceRecordingService.getServiceRecording(self.callerDetails.CallCenterHeaderID, $scope.isLawLiaison ? SERVICE_RECORDING_SOURCE.LawLiaison : SERVICE_RECORDING_SOURCE.CallCenter).then(serviceRecordingSuccess, errorMethod, notifyMethod);

						setLawEnforcementCheckbox();
						if (self.callerDetails.Comments) {
								self.callerDetails.preComments = parseJSON(self.callerDetails.Comments);
						}
						var contactID = self.callerDetails.ClientContactID;
						getHeader();
						//Gets client details
						registrationService.get(contactID).then(contactSuccess, errorMethod, notifyMethod);
				} else {
						alertService.error('Unable to get caller information!');
				};
		};

		var getHeader = function () {
				if (self.callerDetails.CallStartTime) {
						self.providerDetail.ProviderStartTime = searchString(self.callerDetails.CallStartTime, adjustedTime) ? "" : $filter('formatDate')(self.callerDetails.CallStartTime, 'hh:mm');
						self.providerDetail.CallStartAMPM = searchString(self.callerDetails.CallStartTime, defaultTime) ? "AM" : $filter('toStandardTimeAMPM')(dateTimeValidatorService.getCurrentMeridian(moment(self.callerDetails.CallStartTime).toDate()));
						self.providerDetail.ProviderDate = $filter('formatDate')(self.callerDetails.CallStartTime, 'MM/DD/YYYY');
				}
				if (self.callerDetails.CallEndTime) {
						self.providerDetail.ProviderEndTime = searchString(self.callerDetails.CallEndTime, adjustedTime) ? "" : $filter('formatDate')(self.callerDetails.CallEndTime, 'hh:mm');
						self.providerDetail.CallEndAMPM = searchString(self.callerDetails.CallEndTime, defaultTime) ? "AM" : $filter('toStandardTimeAMPM')(dateTimeValidatorService.getCurrentMeridian(moment(self.callerDetails.CallEndTime).toDate()));
				}
				self.providerDetail.ProviderBy = self.callerDetails.ProviderID;
		};
	$scope.GetLawEnforcementContact = function (checkboxValue, storePrevious, formReset) {
				if (!checkboxValue && storePrevious) {
						$scope.previousCaller = {
								CallerFirstName: self.callerInformation.FirstName,
								CallerLastName: self.callerInformation.LastName,
								Phone: self.Phones
						};
				}
				self.callerInformation.FirstName = '';
				self.callerInformation.LastName = '';
				initPhones();
				if (!checkboxValue) {
						var callerID = self.callerDetails.CallerContactID;
						//Gets caller details
						if (callerID) {
								$scope.disableISCallerSameAsContact = true;
								registrationService.get(callerID).then(callerSuccess.bind(this, formReset), errorMethod, notifyMethod);
						} else {
								$scope.disableISCallerSameAsContact = true;
								callerInformationService.get(self.headerID, true).then(function (callerData) {
										if (hasData(callerData)) {
												self.callerDetails.CallerContactID = callerData.DataItems[0].CallerContactID;
												if (self.callerDetails.CallerContactID != undefined && self.callerDetails.CallerContactID != null)
														$scope.disableISCallerSameAsContact = true;
												registrationService.get(self.callerDetails.CallerContactID).then(callerSuccess.bind(this, formReset), errorMethod, notifyMethod);
										}
								}, failureMethod, notifyMethod);
						}
						self.disableControl = true;
				}
				else {
						self.disableControl = (self.callCenterTypeID != self.lawLiaisonType);
						if ($scope.previousCaller) {
								self.callerInformation.FirstName = $scope.previousCaller.CallerFirstName;
								self.callerInformation.LastName = $scope.previousCaller.CallerLastName;
								self.Phones = $scope.previousCaller.Phone;
						}
						else if (self.ProgressNoteDetails.NewCallerID && self.ProgressNoteDetails.NewCallerID != 0) {
								$scope.disableISCallerSameAsContact = true;
								registrationService.get(self.ProgressNoteDetails.NewCallerID).then(callerSuccess.bind(this, formReset), errorMethod, notifyMethod);
						}
				}
		};
	var setLawEnforcementCheckbox = function () {
				var callerID = self.callerDetails.CallerContactID;
				//Gets caller details
				if (callerID) {
						registrationService.get(callerID).then(enforcementContactSuccess, errorMethod, notifyMethod);
				} else {
						callerInformationService.get(self.headerID, true).then(function (callerData) {
								if (hasData(callerData)) {
										if (callerData.DataItems[0].CallerContactID)
												registrationService.get(callerData.DataItems[0].CallerID).then(enforcementContactSuccess, errorMethod, notifyMethod);
								}
						}, failureMethod, notifyMethod);
				}
		};
	var enforcementContactSuccess = function (data) {
				if (roleSecurityService.hasPermission(CallCenterPermissionKey.CallCenter_LawLiaison, PERMISSION.CREATE + '|' + PERMISSION.UPDATE) && !$scope.isSigned && !$scope.isReadOnly)
						self.LawContactExists = (hasData(data) && ((data.DataItems[0].FirstName && data.DataItems[0].FirstName.length > 0) || (data.DataItems[0].LastName && data.DataItems[0].LastName.length > 0)));
				self.LawContactExists = self.LawContactExists ? self.LawContactExists : false;

		};
	var initPhones = function () {
				self.Phones = [];
				self.Phones.push(objPhone());
		};
	var callerSuccess = function (formReset, data) {
				if (hasData(data)) {
						//Gets contact phone details
						self.callerInformation = data.DataItems[0];
						//$scope.disableISCallerSameAsContact = false;
						if (self.callerInformation.ContactTypeID == crisisLineContactTypeID || self.callerInformation.ContactTypeID == lawLiaisonContactTypeID) {
								self.ProgressNoteDetails.ClientStatusID = activeClientStatusID;
						}
						contactPhoneService.get(self.callerInformation.ContactID, self.callerInformation.ContactTypeID).then(phoneSuccess.bind(this, formReset), errorMethod, notifyMethod);
				}
				if (formReset)
						resetForm();
		};

		var contactSuccess = function (data) {
				if (hasData(data)) {
						self.clientDetails = data.DataItems[0];
						//TODO: If phone details required for client, give the call to contactPhoneService.get with client ID and populate the details on the form
						self.clientDetails = data.DataItems[0];
						if (self.clientDetails.DOB)
								self.clientDetails.DOB = new Date($filter('formatDate')(self.clientDetails.DOB));
						if ($scope.isLawLiaison) {
								contactAddressService.get(self.clientDetails.ContactID, self.clientDetails.ContactTypeID).then(clientAddressSuccess, errorMethod, notifyMethod);
						}
						contactPhoneService.get(self.clientDetails.ContactID, self.callerInformation.ContactTypeID).then(clientPhoneSuccess, errorMethod, notifyMethod);
				}
		};

		var clientAddressSuccess = function (data) {
				if (hasData(data)) {
						addressDetails = getPrimaryOrLatestData(data.DataItems);
						self.callerDetails.CountyID = addressDetails[0].County;
				}
				resetForm();
		};
	var serviceRecordingSuccess = function (data) {
				if (hasData(data)) {
						if (data.DataItems[0].OrganizationID) {
								self.callerDetails.ProgramUnitID = data.DataItems[0].OrganizationID;
						}
				}
				else {
						if ($scope.isLawLiaison) {
								self.callerDetails.ProgramUnitID = populateProgramUnit(serviceName);
						}
				}
				resetForm();
		};

	function populateProgramUnit(serviceType) {
				var serviceNameList = $filter('filter')(lookupService.getLookupsByType("ProgramUnit"), function (item) {
						return (item.Name == serviceType);
				});
				if (serviceNameList && serviceNameList.length > 0) {
						return serviceNameList[0].ID;
				}
		}

		var phoneSuccess = function (formReset, data) {
				$scope.disableISCallerSameAsContact = false;
				if (hasData(data)) {
						self.Phones = getPrimaryOrLatestData(data.DataItems);
						if (formReset)
								resetForm();
				}
		};

		var clientPhoneSuccess = function (data) {
				if (hasData(data)) {
						self.ClientPhones = getPrimaryOrLatestData(data.DataItems);
				}
				resetForm();
		};

		var saveSuccessMethod = function (data) {
				var dfd = $q.defer();
				if (data && data.ResultCode == 0) {
						var msg = (self.ProgressNoteDetails.NoteHeaderID == 0) ? 'saved' : 'updated';
						var promise = [];
						if (data.ResultMessage || formService.isDirty()) {
								promise.push($scope.saveAssessment(self.isNext, self.mandatory, self.hasErrors, self.keepForm, self.next));
								promise.push(callerInformationService.updateModifiedOn($stateParams.CallCenterHeaderID));
								$q.all(promise).then(function (data) {
										onSuccess(dfd, 'Progress Note ' + msg + ' successfully.');
										//save workflow Header details.
										//$rootScope.$broadcast('onWorkflowHeaderSave', { workflowDataKey: $state.current.data.workflowDataKey, workflowHeaderID: $stateParams.CallCenterHeaderID });
										WorkflowHeaderService.AddWorkflowHeader({ WorkflowDataKey: $state.current.data.workflowDataKey, RecordHeaderID: $stateParams.CallCenterHeaderID, ContactID: $stateParams.ContactID });
								});
						} else {
								onSuccess(dfd);
						}

				}

				return dfd.promise;
		};

		var onSuccess = function (dfd, msg) {
				setFormStatus(true);
				get().then(function () {
						if (msg) {
								alertService.success(msg);
						}
						if (self.isNext) {
								alertForServiceCompletion();
								$scope.next();
								dfd.resolve(true); ``
						}
						else {
								init();
								dfd.resolve(true);
						}
				});

		};
	var progressNotesaveorUpdate = function () {
				if (!self.ProgressNoteDetails.NoteHeaderID)
						return callCenterProgressNoteService.add(self.ProgressNoteDetails).then(saveSuccessMethod, failureMethod, notifyMethod);
				else
						return callCenterProgressNoteService.update(self.ProgressNoteDetails).then(saveSuccessMethod, failureMethod, notifyMethod);
		};
	$scope.saveProgressNote = function (isNext, mandatory, hasErrors, keepForm, next) {
				//Saving/Updating Progress Note data
				self.isNext = isNext;
				self.mandatory = mandatory;
				self.hasErrors = hasErrors;
				self.keepForm = keepForm;
				self.next = next;
				var isanyformDirty = formService.isDirty();
				if ($stateParams.ReadOnly && $stateParams.ReadOnly.toString().toLowerCase() == "view" && isNext) {
						$scope.next();
				}
				else if (!hasErrors && isanyformDirty) {
						return validateSignature().then(function (response) {
								if (response) {
										if (!self.ProgressNoteDetails.IsCallerSame && self.callerInformation.FirstName && self.callerInformation.FirstName != '' && (self.callCenterTypeID == self.lawLiaisonType)) {
												var contactObj = { FirstName: self.callerInformation.FirstName, LastName: self.callerInformation.LastName, ContactTypeID: self.callCenterTypeID };
												if (self.ProgressNoteDetails.NewCallerID && self.ProgressNoteDetails.NewCallerID != 0) {
														contactObj.ContactID = self.ProgressNoteDetails.NewCallerID;
														return registrationService.update(contactObj).then(saveProgressNoteDetails, failureMethod, notifyMethod);
												}
												else
														return registrationService.add(contactObj).then(saveProgressNoteDetails, failureMethod, notifyMethod);
										}
										else {
												self.ProgressNoteDetails.NewCallerID = null;
												return saveProgressNoteDetails({});
										}
								}
								else {
										return false;
								}
						});
				} else if (!hasErrors && isNext) {
						$scope.next();
				}
		};

		var emptySuccessMethod = function () { };
	var saveProgressNoteDetails = function (resp) {
				if (resp.data) {
						if (resp.data.ID && resp.data.ID != 0)
								self.ProgressNoteDetails.NewCallerID = resp.data.ID;
						if (self.ProgressNoteDetails.NewCallerID > 0) {
								var phones = $filter('filter')(self.Phones, function (item) {
										if (item.Number != '' || (item.ContactPhoneID != null && item.ContactPhoneID != 0)) {
												item.ContactID = self.ProgressNoteDetails.NewCallerID;
												return true;
										}
										else {
												return false;
										}
								});
								if (phones && phones.length > 0)
										contactPhoneService.save(phones[0]).then(emptySuccessMethod, failureMethod, notifyMethod);
						}
				}
				if (self.ProgressNoteDetails.CallTypeID != self.phoneTypeOtherId) {
						self.ProgressNoteDetails.CallTypeOther = '';
				}

				self.ProgressNoteDetails.Disposition = self.callerDetails.Disposition;
				self.ProgressNoteDetails.Comments = prepareXenCommentHistory(self.callerDetails.preComments, self.callerDetails.newComment, $scope.cmtReporter);
				if (self.callCenterTypeID == self.lawLiaisonType) {
						self.ProgressNoteDetails.FollowupPlan = prepareXenCommentHistory(self.ProgressNoteDetails.preFollowupPlan, self.ProgressNoteDetails.newFollowupPlan, $scope.cmtReporter);
						self.ProgressNoteDetails.NatureofCall = prepareXenCommentHistory(self.ProgressNoteDetails.preNatureofCall, self.ProgressNoteDetails.newNatureofCall, $scope.cmtReporter);
				}
				var dateVal = moment(self.providerDetail.ProviderDate).toDate();
				var timeVal = $filter('toStandardTime')(self.providerDetail.ProviderStartTime);
				var hr = timeVal.substring(0, timeVal.indexOf(':'));
				if (self.providerDetail.CallStartAMPM == "PM" && hr != 12) {     //checks if PM, adds 12 hours
						hr = +hr + 12;
				}
				else if (self.providerDetail.CallStartAMPM == "AM" && hr == 12) { // BUGFIX - 13633
						hr = "0";
				}
				timeVal = timeVal.substring(timeVal.indexOf(':') + 1, timeVal.length);
		var min = timeVal.substring(0, timeVal.length);

				var dateTime = dateVal.setHours(hr, min);
				self.ProgressNoteDetails.CallStartTime = $filter('formatDate')(moment(dateTime).toDate(), 'MM/DD/YYYY hh:mm A');

				if (self.providerDetail.ProviderEndTime) {
						var timeValEnd = $filter('toStandardTime')(self.providerDetail.ProviderEndTime || "00:00");
						var hr1 = timeValEnd.substring(0, timeValEnd.indexOf(':'));
						if (self.providerDetail.CallEndAMPM == "PM" && hr1 != 12)      //checks if PM, adds 12 hours
								hr1 = +hr1 + 12;
						else if (self.providerDetail.CallStartAMPM == "AM" && hr1 == 12) { // BUGFIX - 13633
								hr1 = "0";
						}
						timeValEnd = timeValEnd.substring(timeValEnd.indexOf(':') + 1, timeValEnd.length);
					var minEnd = timeValEnd.substring(0, timeValEnd.length);
						var dateTimeEnd = dateVal.setHours(hr1, minEnd);

						self.ProgressNoteDetails.CallEndTime = $filter('formatDate')(dateTimeEnd, 'MM/DD/YYYY hh:mm A');
				}
				else { self.ProgressNoteDetails.CallEndTime = self.providerDetail.ProviderEndTime }
				self.ProgressNoteDetails.ProviderID = self.providerDetail.ProviderBy;
				self.ProgressNoteDetails.NoteAdded = true;
				if ($scope.isFollowup) {
						callCenterProgressNoteService.get(lawLiaisonFollowUp.parentCallCenterHeaderID).then(function (progressNoteResponse) {
								if (hasData(progressNoteResponse)) {
										var parentProgressNote = progressNoteResponse.DataItems[0];
										parentProgressNote.FollowupPlan = self.ProgressNoteDetails.FollowupPlan;
										parentProgressNote.NatureofCall = self.ProgressNoteDetails.NatureofCall;
										parentProgressNote.Comments = self.ProgressNoteDetails.Comments;
										callCenterProgressNoteService.update(parentProgressNote);
								}
						});
				}
				return progressNotesaveorUpdate();
		};

		self.initReport = function () {
				return $q.when($scope.saveProgressNote(false, false, false)).then(function (response) {
						if (response == undefined || response == true) {
								return serviceRecordingService.getServiceRecording($stateParams.CallCenterHeaderID, (($scope.isLawLiaison) ? SERVICE_RECORDING_SOURCE.LawLiaison : SERVICE_RECORDING_SOURCE.CallCenter))
												.then(function (serviceDataModel) {
														var printType = $scope.isCrisisLine ? 'crisisProgressNote' : 'lawLiaisonProgressNote';
														var serviceModel = {};
														if (hasData(serviceDataModel))
																serviceModel = serviceDataModel.DataItems[0];
														return callCenterProgressNoteService.initReport($scope.AssessmentID, responseID, sectionID, $stateParams.CallCenterHeaderID, $scope.signature, printType, serviceModel).then(function (reportModel) {
																reportModel.HasLoaded = true;
																reportModel.isDigitalSignature = true;
																$scope.reportModel = reportModel;
																$('#reportModal').modal('show');
														});
												});
						}
						else
								return null;
				});
		};
	self.clearDescribeOther = function (callTypeID) {
				if (callTypeID != callTypeOther) {
						self.ProgressNoteDetails.CallTypeOther = '';
				}
		};
	init();
}]);
