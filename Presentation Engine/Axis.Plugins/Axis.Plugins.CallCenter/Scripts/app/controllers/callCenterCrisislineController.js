angular.module("xenatixApp")
    .controller("callCenterCrisislineController", ["$scope", "$stateParams", "$rootScope", "callerInformationService", "$state", "alertService", "serviceRecordingService", "$q", "cacheService", "formService", "$controller", "$filter", "navigationService", 'assessmentPrintService', 'callCenterAssessmentPrintService', 'lookupService', 'registrationService', 'contactPhoneService', 'WorkflowHeaderService', 'callCenterAssessmentPrintService',
function ($scope, $stateParams, $rootScope, callerInformationService, $state, alertService, serviceRecordingService, $q, cacheService, formService, $controller, $filter, navigationService, printService, callCenterPrintService, lookupService, registrationService, contactPhoneService, WorkflowHeaderService, callCenterAssessmentPrintService) {
		$scope.callCenterWorkFlowOptions = { enableWorkflow: 0, enableAdditionalWorkflow: true };
		$controller('callCenterQuickRegistrationController', { $scope: $scope });
		$scope.CallMinStartDate = new Date(70, 1, 1);
		if (cacheService.get('approvalData') == null)
				cacheService.add('approvalData', $state.get('callcenter.crisisline').data.approvalData);
		var approvalItems = cacheService.get('approvalData');
		if (cacheService.get('currentApprovalIndex') == null) {
				setCurrentApprovalIndex(1);
		}

		var isSigned;

		var servicesData = {
				ServiceRecordingID: 0,
				ServiceRecordingSourceID: SERVICE_RECORDING_SOURCE.CallCenter,
				CallCenterHeaderID: 0,
				DeliveryMethodID: 1,
				ServiceLocationID: 1,
				UserID: 0,
				ServiceStartDate: $filter('formatDate')(new Date(), 'MM/DD/YYYY HH:mm:ss'),
				ServiceEndDate: null,
				ModifiedOn: $filter('formatDate')(new Date(), 'MM/DD/YYYY HH:mm:ss'),
		}

		navigationService.get().then(function (data) {
				if (hasData(data)) {
						servicesData.UserID = data.DataItems[0].UserID;
				}
		})
		$scope.currentApprovalItem = getCurrentApprovalIndex();
		$scope.totalApprovalItems = null;
		$scope.isBackEnabled = true;
		$scope.isNextEnabled = true;
		$scope.isManager = cacheService.get('IsManagerAccess');
		$scope.isCreator = cacheService.get('IsCreatorAccess');
		$scope.$on('quickRegMRN', function (event, args) {
				$scope.isCallCenterConvertToRegistration = args.Data;
		});

		if (approvalItems != undefined) {
				$scope.totalApprovalItems = Object.keys(approvalItems).length;
				$scope.isApprovalWorkflow = ($scope.totalApprovalItems > 0) ? true : false;
				callerInformationService.get(approvalItems[0].CallCenterHeaderID, '').then(function (callerData) {
						$scope.isApproved = (callerData.DataItems[0].CallStatusID == CALL_STATUS.COMPLETE) ? true : false;
				});
		};

		function getCurrentApprovalIndex() {
				return cacheService.get('currentApprovalIndex');
		};

		function setCurrentApprovalIndex(value) {
				$scope.currentApprovalItem = value;
				cacheService.add('currentApprovalIndex', value);
		};
		$scope.loadReport = false;
		$scope.printCrisisLine = function () {
				$scope.loadReport = true;
		};

		$scope.closeReport = function () {
				$scope.loadReport = false;
		};

		$scope.printCrisisLineReport = function () {
				var dfd = $q.defer();
				var reportModel = {
						HasLoaded: false,
						ReportHeader: 'CrisisLineAssessments'
				};
				$stateParams = $state.params;
				var responseArr = [];
				responseArr.push(callerInformationService.getCallCenterAssessmentResponse($stateParams.CallCenterHeaderID, ASSESSMENT_TYPE.ColumbiaSuicideSeverityRatingScale));
				responseArr.push(callerInformationService.getCallCenterAssessmentResponse($stateParams.CallCenterHeaderID, ASSESSMENT_TYPE.CrisisAssessment));
				responseArr.push(callerInformationService.getCallCenterAssessmentResponse($stateParams.CallCenterHeaderID, ASSESSMENT_TYPE.CrisisAdultScreening));
				responseArr.push(callerInformationService.getCallCenterAssessmentResponse($stateParams.CallCenterHeaderID, ASSESSMENT_TYPE.CrisisChildScreening));
				var reportArr = [];
				$q.all(responseArr).then(function (response) {
						reportArr.push(hasData(response[0]) ? printService.initReports(ASSESSMENT_TYPE.ColumbiaSuicideSeverityRatingScale, response[0].DataItems[0].ResponseID) : true);
						reportArr.push(hasData(response[1]) ? printService.initReports(ASSESSMENT_TYPE.CrisisAssessment, response[1].DataItems[0].ResponseID) : true);
						reportArr.push(hasData(response[2]) ? printService.initReports(ASSESSMENT_TYPE.CrisisAdultScreening, response[2].DataItems[0].ResponseID) : true);
						reportArr.push(hasData(response[3]) ? printService.initReports(ASSESSMENT_TYPE.CrisisChildScreening, response[3].DataItems[0].ResponseID) : true);
						reportArr.push(callCenterPrintService.getContactReprotInformation($stateParams.ContactID));
						reportArr.push(serviceRecordingService.getServiceRecording($stateParams.CallCenterHeaderID, SERVICE_RECORDING_SOURCE.CallCenter));
						reportArr.push(WorkflowHeaderService.GetWorkflowHeader($state.current.data.workflowDataKey, $stateParams.CallCenterHeaderID));
						$q.all(reportArr).then(function (data) {
								reportModel.incidentID = $stateParams.CallCenterHeaderID.toString();

								if (hasData(data[5])) {
										reportModel.contactProgramUnit = data[5].DataItems[0].OrganizationID ? lookupService.getText('ProgramUnit', data[5].DataItems[0].OrganizationID) : '';
										if (data[1] && !data[1][3530]) // set send MCOT date to service end date if MCOT date is blank
												data[1][3530] = data[5].DataItems[0].ServiceEndDate ? $filter('toMMDDYYYYDate')(data[5].DataItems[0].ServiceEndDate) : null;
								}
								if (data[4]) {
										angular.extend(reportModel, data[4]);
								}
								if (hasData(data[6])) {
										contactData = data[6].DataItems[0];
										callCenterAssessmentPrintService.getCallCenterPrintHeaderDetails(reportModel, contactData);
								}

								if (data[0]["976"] || data[1]["3610"] || data[2]["3600"] || data[3]["3620"]) {
										callerInformationService.get($stateParams.CallCenterHeaderID).then(function (resp) {
												if (hasData(resp)) {
														var callerDetails = resp.DataItems[0];
														if (data[0]["976"]) {       //if cssrs is signed
																reportModel.printCSSRS = true;
																angular.extend(reportModel, data[0]);
																reportModel.cssrs = {
																		screeningDate: ($filter('toMMDDYYYYDate')(callerDetails.CallStartTime, 'MM/DD/YYYY')).toString(),
																		screener: lookupService.getText('Users', callerDetails.ProviderID),
																		screeningTime: ($filter('toMMDDYYYYDate')(callerDetails.CallStartTime, 'hh:mm A')).toString(),
																		sentToMCOTDate: data[1][3530] ? data[1][3530] : '',
																		sentToMCOTTime: data[1][3531] ? data[1][3531] : '',
																};
														}
														if (data[2]["3600"]) {       //if adult screening is signed
																reportModel.printAdultScreening = true;
																angular.extend(reportModel, data[2]);
																reportModel.adultScreening = {
																		screeningDate: ($filter('toMMDDYYYYDate')(callerDetails.CallStartTime, 'MM/DD/YYYY')).toString(),
																		screener: lookupService.getText('Users', callerDetails.ProviderID),
																		screeningTime: ($filter('toMMDDYYYYDate')(callerDetails.CallStartTime, 'hh:mm A')).toString(),
																};
														}
														if (data[3]["3620"]) {       //if child screening is signed
																reportModel.printChildScreening = true;
																angular.extend(reportModel, data[3]);
																reportModel.childScreening = {
																		screeningDate: ($filter('toMMDDYYYYDate')(callerDetails.CallStartTime, 'MM/DD/YYYY')).toString(),
																		screener: lookupService.getText('Users', callerDetails.ProviderID),
																		screeningTime: ($filter('toMMDDYYYYDate')(callerDetails.CallStartTime, 'hh:mm A')).toString(),
																};
														}
														reportModel.ReportHeader = 'CrisisLineAssessments';
														reportModel.ReportName = 'CrisisLineAssessments';
														if (data[1]["3610"]) {
																reportModel.printCrisisAssessment = true;
																angular.extend(reportModel, data[1]);
																reportModel.crisisAssessment = {
																		atRiskFirstName: data[4].contactFirstName,
																		atRiskLastName: data[4].contactLastName,
																		atRiskID: data[4].contactID,
																		atRiskDOB: data[4].contactDOB ? data[4].contactDOB : '',
																		atRiskAge: data[4].contactAge ? data[4].contactAge : '',
																		atRiskGender: data[4].atRiskGender ? data[4].atRiskGender : '',
																		atRiskMaritalStatus: data[4].atRiskMaritalStatus ? data[4].atRiskMaritalStatus : '',
																		atRiskContactNumber: reportModel.contactPhone ? reportModel.contactPhone : '',
																		residenceLine1: reportModel.contactAddressLine1 ? reportModel.contactAddressLine1 : '',
																		residenceLine2: reportModel.contactAddressLine2 ? reportModel.contactAddressLine2 : '',
																		residenceCity: reportModel.contactCity ? reportModel.contactCity : '',
																		residenceState: reportModel.contactState ? reportModel.contactState : '',
																		residenceZip: reportModel.contactZip ? reportModel.contactZip : '',
																		residenceCounty: reportModel.contactCounty ? reportModel.contactCounty : '',
																		callDate: callerDetails.CallStartTime ? ($filter('toMMDDYYYYDate')(callerDetails.CallStartTime, 'MM/DD/YYYY')).toString() : '',
																		callTime: callerDetails.CallStartTime ? ($filter('toMMDDYYYYDate')(callerDetails.CallStartTime, 'hh:mm')).toString() : ''
																};
																reportModel.ReportHeader = 'CrisisLineAssessments';
																reportModel.ReportName = 'CrisisLineAssessments';
																registrationService.get(callerDetails.CallerContactID).then(function (callerResp) {
																		if (hasData(callerResp)) {
																				var caller = callerResp.DataItems[0];
																				reportModel.crisisAssessment.callerFirstName = caller.FirstName;
																				reportModel.crisisAssessment.callerLastName = caller.LastName;
																				contactPhoneService.get(callerDetails.CallerContactID, caller.ContactTypeID).then(function (phoneResp) {
																						if (hasData(phoneResp)) {
																								var phone = phoneResp.DataItems[0];
																								reportModel.crisisAssessment.callerContactNumber = phone.Number ? ($filter('toPhone')(phone.Number)).toString() : '';
																								dfd.resolve(reportModel);
																						}
																				});
																		} else {
																				dfd.resolve(reportModel);
																		}
																});
														} else {
																dfd.resolve(reportModel);
														}
												} else {
														dfd.resolve(reportModel);
												}
										})
								} else {
										dfd.resolve(reportModel);
								}

								if (data[1]["3610"]) {      //only if Crisis Assessment is signed
										//Region yet to populate
										reportModel.callerRelationship = '';       //In report callerRelationship is in reportModel not in crisisAssessment of reportModel
										//reportModel.crisisAssessment.callerRelationship = '';       //Kyle to confirm from Rachel C
								}
						});
				});
				reportModel.ReportName = 'CrisisLineAssessments';
				reportModel.ReportHeader = 'CrisisLineAssessments';
				return dfd.promise;
		};
		$scope.init = function () {
				var workFlowItems = [
							{ title: "Caller Information", stateName: "callcenter.crisisline.callerinformation", stateParams: "{ CallCenterHeaderID:$stateParams.CallCenterHeaderID,ContactID:$stateParams.ContactID }", initActive: "callcenter.crisisline.initialcallerinformation" },
							{ title: "Services", stateName: "callcenter.crisisline.services", stateParams: "{ CallCenterHeaderID:$stateParams.CallCenterHeaderID,ContactID:$stateParams.ContactID }" },
							{ title: "Columbia Suicide Scale", stateName: "callcenter.crisisline.columbiasuicidescale", stateParams: "{ CallCenterHeaderID:$stateParams.CallCenterHeaderID,ContactID:$stateParams.ContactID }" },
							{ title: "Crisis Assessment", stateName: "callcenter.crisisline.crisisAssessment", stateParams: "{ CallCenterHeaderID:$stateParams.CallCenterHeaderID,ContactID:$stateParams.ContactID }" },
							{ title: "Adult Screening", stateName: "callcenter.crisisline.adultScreening", stateParams: "{ CallCenterHeaderID:$stateParams.CallCenterHeaderID,ContactID:$stateParams.ContactID }" },
							{ title: "Child Screening", stateName: "callcenter.crisisline.childScreening", stateParams: "{ CallCenterHeaderID:$stateParams.CallCenterHeaderID,ContactID:$stateParams.ContactID }" },
							{ title: "Progress Notes", stateName: "callcenter.crisisline.progressnotes", stateParams: "{ CallCenterHeaderID:$stateParams.CallCenterHeaderID,ContactID:$stateParams.ContactID }" },
							{ title: "PDF View", onWorkflowClick: $scope.printCrisisLine }
				];
				// commented for the release 1.1 - replace it back on the above along with array items.
				//{ title: "Appointment", stateName: "callcenter.crisisline.appointment", stateParams: "{ CallCenterHeaderID:$stateParams.CallCenterHeaderID,ContactID:$stateParams.ContactID }" }

				$scope.workFlowModel = {};
				$scope.workFlowModel.workFlowItems = workFlowItems;

				$scope.isCallCenterEndCall = $state.params.CallCenterHeaderID ? false : true;
				$state.params.CallCenterHeaderID && getServiceRecordingData($state.params.CallCenterHeaderID).then(function (data) {
						if (data && data.DataItems && data.DataItems.length > 0) {
								$scope.isCallCenterEndCall = data.DataItems[0].ServiceEndDate ? true : false;
						}
				});
				$scope.isCrisisLine = true;
				$scope.isBackEnabled = (getCurrentApprovalIndex() == 1) ? false : true;
				$scope.isNextEnabled = (getCurrentApprovalIndex() == $scope.totalApprovalItems) ? false : true;
		};

		$scope.$on('rightNavigationCallCenterHandler', function (event, args) {
				if ($rootScope.workflowActions != undefined && $rootScope.workflowActions.hasOwnProperty(args.stateName)) {
						if (args.stateName == "callcenter.crisisline.callerinformation" && args.validationState == VALIDATION_STATE.Valid) {
								$scope.callCenterWorkFlowOptions.enableWorkflow = null;
						}
						else if (args.stateName == "callcenter.lawliaison.lawenforcement" && args.validationState == VALIDATION_STATE.Valid) {
								$scope.callCenterWorkFlowOptions.enableWorkflow = null;
						}
						else if (args.stateName == "callcenter.crisisline.services") {
								if (args.validationState == VALIDATION_STATE.Valid) {
										isSigned = true;
								}
								else {
										args.validationState = VALIDATION_STATE.Invalid;
										isSigned = false;
								}

						}
						$rootScope.workflowActions[args.stateName].validationState = args.validationState;
				}
				$rootScope.$broadcast(args.stateName, { validationState: args.validationState });
		});

		$scope.$on('isCallEnded', function (event, isCallended) {
				$scope.isCallCenterEndCall = isCallended;
		});
		function getServiceRecordingData(CallCenterHeaderID) {
				var dfd = $q.defer();
				serviceRecordingService.getServiceRecording(CallCenterHeaderID, SERVICE_RECORDING_SOURCE.CallCenter).then(function (data) {
						dfd.resolve(data);
				}, function (errorStatus) {
						alertService.error('OOPS! Something went wrong');
						dfd.reject();
				});
				return dfd.promise;
		};
		$scope.init();

		$scope.callCenterEndCall = function () {
				var confirmObj = {
						message: "Are you sure you want to end the call?",
						buttons: {
								cancel: {
										label: "Cancel",
										className: "btn-default"
								},
								endSign: {
										label: "End & Sign",
										className: "btn-success",
										callback: function () {
												endCall(servicesData).then(function (response) {
														if ($state.current.name.indexOf('callcenter.crisisline.services') == -1) {
																$scope.Goto('callcenter.crisisline.services', $state.params);
														}
												});
										}
								}
						}
				};
				getServiceRecordingData($state.params.CallCenterHeaderID).then(function (data) {
						if ($state.current.name.indexOf('callcenter.crisisline.services') === 0) {
								var start = $filter('formatDate')(serviceRecordingService.getServiceRecordingTime().serviceStartDateTime, 'MM/DD/YYYY HH:mm:ss');
								var end = $filter('formatDate')(serviceRecordingService.getServiceRecordingTime().serviceEndDateTime, 'MM/DD/YYYY HH:mm:ss');
								if (start) {
										servicesData.ServiceStartDate = start;
								}
								if (end) {
										servicesData.ServiceEndDate = end;
								}
						}
						if (hasData(data)) {
								servicesData = data.DataItems[0];
								bootbox.dialog(confirmObj);
						}
						else {
								servicesData.CallCenterHeaderID = $state.params.CallCenterHeaderID;
								serviceRecordingService.addServiceRecording(servicesData).then(function (data) {
										getServiceRecordingData($state.params.CallCenterHeaderID).then(function (data) {
												if (hasData(data)) {
														servicesData = data.DataItems[0];
														bootbox.dialog(confirmObj);
												}
										});
								});
						}
				});
		}
		var endCall = function (servicesData) {
				if (!servicesData.ServiceEndDate) {
						servicesData.ServiceEndDate = $filter('formatDate')(new Date(), 'MM/DD/YYYY HH:mm:ss');
				}
				return serviceRecordingService.updateServiceRecording(servicesData).then(function () {
						$scope.isCallCenterEndCall = true;
						$rootScope.$broadcast('callCenterEndCall', servicesData.ServiceEndDate, servicesData.ServiceRecordingID);
				})
		}
		$scope.changeApprovalItem = function (type) {
				if (formService.isDirty() == true) {
						bootbox.confirm("Any unsaved data will be lost, Do you want to continue ?", function (result) {
								if (result === true) {
										formService.reset();
										processApprovalItsms(type);
								}
						});
				}
				else {
						processApprovalItsms(type);
				}
		}

		$scope.$on('onFollowUpCall', function (event, args) {
				$scope.isFollowUpCall = args.data;
		});

		function processApprovalItsms(type) {
				var nextpage = (type == 'next' ? parseInt(getCurrentApprovalIndex()) + 1 : parseInt(getCurrentApprovalIndex()) - 1);
				angular.forEach(approvalItems, function (item, index) {
						if (nextpage == index + 1) {
								setCurrentApprovalIndex(nextpage);
								callerInformationService.get(item.CallCenterHeaderID, '').then(function (callerData) {
										$scope.isApproved = (callerData.DataItems[0].CallStatusID == CALL_STATUS.COMPLETE) ? true : false;
								});
								$scope.Goto('callcenter.crisisline.callerinformation', {
										CallCenterHeaderID: item.CallCenterHeaderID, ContactID: item.ContactID
								});
						}
				});

				if (getCurrentApprovalIndex() == 1) {
						$scope.isNextEnabled = true;
						$scope.isBackEnabled = false;
				}
				else if (getCurrentApprovalIndex() == $scope.totalApprovalItems) {
						$scope.isNextEnabled = false;
						$scope.isBackEnabled = true;
				}
				else {
						$scope.isBackEnabled = true;
						$scope.isNextEnabled = true;
				}
		}

		$scope.approve = function () {
				angular.forEach(approvalItems, function (item, index) {
						if (getCurrentApprovalIndex() == index + 1) {
								callerInformationService.get(item.CallCenterHeaderID, '').then(function (callerData) {
										if (callerData.DataItems.length > 0) {
												callerData.DataItems[0].CallStatusID = 1;
												callerInformationService.update(callerData.DataItems[0], '').then(function (data) {
														if (data.ResultCode == 0) {
																alertService.success('Data approved successfully.');
																$scope.isApproved = true;

																angular.extend($stateParams, {
																		ContactID: item.ContactID,
																		CallCenterHeaderID: item.CallCenterHeaderID,
																		ReadOnly: "view"
																});

																$state.transitionTo('callcenter.crisisline.callerinformation', $stateParams);
														}
												});
										}
								});
						}
				});
		}
}]);