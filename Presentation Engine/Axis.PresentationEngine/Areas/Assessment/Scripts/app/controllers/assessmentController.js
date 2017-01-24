angular.module('xenatixApp')
    .controller('assessmentController', [
            '$scope', '$rootScope', '$q', '$state', '$stateParams', '$filter', 'assessmentService', 'alertService', 'formService', '$compile', '$timeout', 'lookupService', 'helperService',
            function ($scope, $rootScope, $q, $state, $stateParams, $filter, assessmentService, alertService, formService, $compile, $timeout, lookupService, helperService) {
                $scope.eSignature = {};
                $scope.dSignature = {};
                $scope.isTopazReady = false;
                $scope.enablePrint = true;
                $scope.allowNext = true; //Added to stop move next shortcut on last step of workflow.
                $scope.disableAssessmentSave = false;
                $scope.todayDate = new Date();
                var documentTypeID;
                var nextState;
                var userCredentialList;
                function Numbering() {
                    this.numberingIndex = 0;
                }

                Numbering.prototype.reset = function () {
                    this.numberingIndex = 1;
                };

                Numbering.prototype.increment = function () {
                    this.numberingIndex++;
                };

                Numbering.prototype.current = function () {
                    return this.numberingIndex;
                };

                function SectionInformation() {
                    this.sectionId = 0;
                    this.sectionTitle = '';
                }

                SectionInformation.prototype.set = function (sectionId, sectionTitle) {
                    this.sectionId = sectionId;
                    this.sectionTitle = sectionTitle;
                };

                SectionInformation.prototype.getSectionTitle = function () {
                    return (this.sectionTitle === 'None') ? '' : this.sectionTitle;
                };

                SectionInformation.prototype.getControlTitle = function (parentSectionId, optionText) {
                    return ((parentSectionId === this.sectionId) && (optionText === this.sectionTitle)) ? '' : optionText;
                };

                $scope.contactId = $stateParams.ContactID;
                $scope.sectionId = $stateParams.SectionID;
                $scope.responseId = $stateParams.ResponseID;
                $scope.assessmentQuestions = [];
                $scope.assessmentResponse = {};
                $scope.assessmentResponseDetails = [];
                $scope.responses = {};

                if ($state.current.data && $state.current.data.permissionKey)
                    $scope.permissionKey = $state.current.data.permissionKey;

                var assessmentSection = assessmentService.getAssessmentSectionById($scope.sectionId);
                if (hasData(assessmentSection) && assessmentSection.DataItems[0].PermissionKey) {
                    $scope.permissionKey = assessmentSection.DataItems[0].PermissionKey;
                }

                $scope.assessmentPermissionKey = $scope.permissionKey;
                if ($state.current.data && $state.current.data.credentialKey)
                    $scope.credentialKey = $state.current.data.credentialKey;

                // used as a local state variable by the view
                $scope.sectionTitle = '';
                $scope.sectionQuestionId = 0;
                $scope.controlTitle = '';
                $scope.isPrintReportRequired = false;

                $scope.QuestionNumbering = new Numbering();
                $scope.QuestionSection = new SectionInformation();

                $scope.NoneOptionID = 17; // For shame ...

                $scope.questionType = {
                    Header: 1,
                    Section: 2,
                    Subsection: 3,
                    Question: 4,
                    SubQuestion: 5,
                    SubTotals: 6,
                    Totals: 7,
                    Block: 8,
                    Print: 9,
                    Hidden: 12
                };

                $scope.inputType = INPUT_TYPE;

                $scope.inputPositionType = {
                    Right: 1,
                    Left: 2,
                    Inline: 3,
                    Bottom: 4
                };

                $scope.getAssessmentQuestions = function () {
                    return assessmentService.getAssessmentQuestions($scope.sectionId).then(function (data) {
                        $scope.assessmentQuestions = $filter('orderBy')(((data.DataItems) && (data.DataItems.length > 0)) ? data.DataItems : [], 'SortOrder', false);

                        // Bug Fix 13268 code should remove and should take care from seed.
                        /* BUGFIX - 13268 */
                        $scope.assessmentQuestions.filter(function (q) { return q.Question == "Describe" }).forEach(function (v) {
                            v.InputTypeID = INPUT_TYPE.TextArea;
                            v.Attributes = JSON.stringify({ maxlength: "4000" });
                        });

                        $scope.isPrintReportRequired = ($filter('filter')($scope.assessmentQuestions, function (question) { return parseInt(question.QuestionTypeID) === parseInt($scope.questionType.Print); }).length == 0 ? false : true);

                        $scope.defaultResponseList = [];        //Array for keeping track of default responses and assigning the value
                        var hasDigitalSign = false;
                        if ($scope.assessmentQuestions) {
                            angular.forEach($scope.assessmentQuestions, function (question) {
                                if (question.Attributes && question.Attributes.indexOf("default-model") !== -1) {
                                    var attrs = parseJSON(question.Attributes);
                                    var optionsId = (question.AssessmentQuestionOptions && question.AssessmentQuestionOptions.length > 0) ? question.AssessmentQuestionOptions[0].OptionsID : '';
                                    if (attrs && attrs["default-model"])
                                        $scope.defaultResponseList.push({ QuestionID: question.QuestionID, OptionsID: optionsId, InputTypeID: question.InputTypeID, DefaultValue: attrs["default-model"] });
                                }
                                if (question.InputTypeID == INPUT_TYPE.DSignature) {
                                    hasDigitalSign = true;
                                }
                            });
                        }
                        if (hasDigitalSign) {
                            //To ensure if assessmentId is always available
                            if (!$scope.AssessmentID) {
                                $scope.AssessmentID = assessmentService.getAssessmentID($scope.sectionId);
                            }
                            return assessmentService.getDocumentTypeID($scope.AssessmentID).then(function (typeData) {
                                if (typeData && hasData(typeData.data)) {
                                    documentTypeID = typeData.data.DataItems[0].Result;
                                }
                                return $scope.getAssessmentResponse();
                            });
                        }
                        else {
                            return $scope.getAssessmentResponse();
                        }

                        return $scope.getAssessmentResponse();
                    },
                    function (errorStatus) {
                        alertService.error('Unable to connect to server');
                    });
                };

                var resetForm = function () {
                    $scope.ctrl.assessmentForm && $rootScope.formReset($scope.ctrl.assessmentForm, $scope.ctrl.assessmentForm.$name);
                    angular.forEach($scope.dSignature, function (item) {
                        if (item.resetForm) {
                            item.resetForm();
                        }
                    })
                };

                $scope.getAssessmentResponse = function () {
                    // Ensure AssessmentID
                    if ((!$scope.AssessmentID || $scope.AssessmentID == 0) && $scope.assessmentQuestions.length > 0) {
                        $scope.AssessmentID = assessmentService.getAssessmentID($scope.assessmentQuestions[0].AssessmentSectionID);
                    }
                    return assessmentService.getAssessmentResponse($scope.contactId, $scope.AssessmentID, $scope.responseId).then(function (data) {
                        $scope.assessmentResponse = hasData(data) ? data.DataItems[0] : {};
                        $scope.assessmentResponeID=$scope.responseId?$scope.responseId:0;

                        return $scope.getAssessmentResponseDetails();
                    },
                    function (errorStatus) {
                        alertService.error('Unable to connect to server');
                    });
                };

                var commentOption = function (optionID) {
                    return optionID + 'H';
                };

                $scope.getAssessmentResponseDetails = function () {
                    // set prepopopulated data
                    if ($scope.defaultResponseList && typeof $scope.preDefinedData == 'function') {
                        return $scope.preDefinedData().then(function () {
                            angular.forEach($scope.defaultResponseList, function (item) {
                                var inputTypeId = item.InputTypeID;
                                //{ QuestionID, OptionsID, InputTypeID, DefaultValue}
                                if (inputTypeId === $scope.inputType.Textbox || inputTypeId === $scope.inputType.TextArea || inputTypeId === $scope.inputType.DatePicker || inputTypeId === INPUT_TYPE.CheckboxList ||
                                    inputTypeId === $scope.inputType.TimePicker || inputTypeId === $scope.inputType.Label || inputTypeId === $scope.inputType.Hidden || inputTypeId === $scope.inputType.ExtendedDropdown) {
                                    if (!(item.QuestionID in $scope.responses))
                                        $scope.responses[item.QuestionID] = {};
                                    $scope.responses[item.QuestionID][item.OptionsID] = getValueFromDefault(item.DefaultValue);
                                }
                                else if (inputTypeId === $scope.inputType.Comments) {
                                    if (!(item.QuestionID in $scope.responses))
                                        $scope.responses[item.QuestionID] = {};
                                    $scope.responses[item.QuestionID][item.OptionsID] = '';
                                    $scope.responses[item.QuestionID][commentOption(item.OptionsID)] = getValueFromDefault(item.DefaultValue);
                                }
                                else if (inputTypeId === $scope.inputType.Radio || inputTypeId === $scope.inputType.Select) {
                                    $scope.responses[item.QuestionID] = getValueFromDefault(item.DefaultValue);
                                }
                                else if (inputTypeId === $scope.inputType.Checkbox || inputTypeId === $scope.inputType.MultiSelect) {
                                    if (!(item.QuestionID in $scope.responses)) {
                                        $scope.responses[item.QuestionID] = {};
                                    }
                                    $scope.responses[item.QuestionID][item.OptionsID] = getValueFromDefault(item.DefaultValue);
                                }
                                else if (inputTypeId === $scope.inputType.ESignature) {
                                    if (!(item.QuestionID in $scope.responses)) {
                                        $scope.responses[item.QuestionID] = {
                                        };
                                    }
                                    $scope.responses[item.QuestionID][item.OptionsID] = getValueFromDefault(item.DefaultValue);;
                                    if (item.SignatureBLOB) {
                                        var keyName = elementName(item.QuestionID, item.OptionsID);
                                        if (!$scope.eSignature[keyName]) {
                                            $scope.eSignature[keyName] = topazModel(keyName, item.QuestionID, item.OptionsID);
                                        }
                                        $scope.eSignature[keyName].topazModel.imageBLOB = getValueFromDefault(item.DefaultValue);
                                    }
                                }
                            });
                            return getAssessmentResponseDetails();
                        })
                    }
                    else {
                        return getAssessmentResponseDetails();
                    }
                };

                var getAssessmentResponseDetails = function () {
                    return assessmentService.getAssessmentResponseDetails($scope.responseId, $scope.sectionId).then(function (response) {
                        if (response && response.data && response.data.DataItems) {
                            var data = response.data;
                            $scope.assessmentResponseDetails = (data.DataItems.length > 0) ? data.DataItems : [];
                            if ($scope.assessmentResponseDetails.length > 0)
                                $scope.assessmentResponeID = $stateParams.ResponseID;
                            else
                                $scope.assessmentResponeID = 0;

                            if (!$scope.isAssessmentIntegrated) {
                                var stateName = getAssessmentState($state.current.name, $stateParams.SectionID);
                                $rootScope.$broadcast(stateName, {
                                    validationState: (data.DataItems.length > 0) ? 'valid' : 'warning'
                                });  // to show right side validation
                            }
                            angular.forEach($scope.assessmentResponseDetails, function (item) {
                                var inputTypeId = $scope.getQuestionInputType(item.QuestionID);
                                if (inputTypeId === $scope.inputType.Textbox || inputTypeId === $scope.inputType.TextArea || inputTypeId === $scope.inputType.DatePicker ||
                                    inputTypeId === $scope.inputType.TimePicker || inputTypeId === $scope.inputType.Hidden || inputTypeId === $scope.inputType.Label || inputTypeId === $scope.inputType.ExtendedDropdown) {
                                    if (!(item.QuestionID in $scope.responses))
                                        $scope.responses[item.QuestionID] = {};
                                    $scope.responses[item.QuestionID][item.OptionsID] = (inputTypeId === $scope.inputType.ExtendedDropdown) ? parseInt(item.ResponseText) : item.ResponseText;
                                }
                                else if (inputTypeId === INPUT_TYPE.CheckboxList) {
                                    if (!(item.QuestionID in $scope.responses))
                                        $scope.responses[item.QuestionID] = {};
                                    $scope.responses[item.QuestionID][item.OptionsID] = '';
                                    $scope.responses[item.QuestionID][item.OptionsID] = parseJSON(item.ResponseText);
                                }
                                else if (inputTypeId === $scope.inputType.Comments) {
                                    if (!(item.QuestionID in $scope.responses))
                                        $scope.responses[item.QuestionID] = {};
                                    $scope.responses[item.QuestionID][item.OptionsID] = '';
                                    $scope.responses[item.QuestionID][commentOption(item.OptionsID)] = parseJSON(item.ResponseText);
                                }
                                else if (item.OptionsID && (inputTypeId === $scope.inputType.Radio || inputTypeId === $scope.inputType.Select)) {
                                    $scope.responses[item.QuestionID] = item.OptionsID.toString();
                                }
                                else if (inputTypeId === $scope.inputType.Checkbox || inputTypeId === $scope.inputType.MultiSelect) {
                                    if (!(item.QuestionID in $scope.responses)) {
                                        $scope.responses[item.QuestionID] = {};
                                    }
                                    $scope.responses[item.QuestionID][item.OptionsID] = true;
                                }
                                else if (inputTypeId === $scope.inputType.ESignature) {
                                    if (!(item.QuestionID in $scope.responses)) {
                                        $scope.responses[item.QuestionID] = {};
                                    }
                                    $scope.responses[item.QuestionID][item.OptionsID] = item.SignatureBLOB;
                                    if (item.SignatureBLOB) {
                                        var keyName = elementName(item.QuestionID, item.OptionsID);
                                        if (item.OptionsID == 3701)
                                            getAllCredentials(item.QuestionID);
                                        if (!$scope.eSignature[keyName]) {
                                            $scope.eSignature[keyName] = topazModel(keyName, item.QuestionID, item.OptionsID);
                                        }
                                        $scope.eSignature[keyName].topazModel.imageBLOB = item.SignatureBLOB;
                                    }
                                }
                                else if (inputTypeId === $scope.inputType.DSignature) {
                                    if (!(item.QuestionID in $scope.responses)) {
                                        $scope.responses[item.QuestionID] = {};
                                    }
                                    $scope.responses[item.QuestionID][item.OptionsID] = item.ResponseText;
                                    var keyName = elementName(item.QuestionID, item.OptionsID);
                                    $scope.dSignature[keyName] = dSignModel(keyName, item.QuestionID, item.OptionsID, item.ResponseDetailsID);
                                }
                            });
                            // onPostAssessmentResponse
                            if (typeof $scope.onPostAssessmentResponse == 'function') {
                                $scope.onPostAssessmentResponse();
                            }
                        }
                        resetForm();
                    },
                    function (errorStatus) {
                        alertService.error('Unable to connect to server');
                    });
                };

                var getAllCredentials = function () {
                    userCredentialList = $scope.CredentialList;
                    $scope.CredentialList = lookupService.getLookupsByType('Credential');
                }

                var getValueFromDefault = function (defaultValue) {
                    switch (defaultValue) {
                        case 'AGENCY_ADDRESS':
                            return $scope.AGENCY_ADDRESS;
                        case 'AGENCY_CITY':
                            return $scope.AGENCY_CITY;
                        case 'AGENCY_NAME':
                            return $scope.AGENCY_NAME;
                        case 'AGENCY_PHONE':
                            return $scope.AGENCY_PHONE;
                        case 'AGENCY_STATE':
                            return $scope.AGENCY_STATE;
                        case 'AGENCY_ZIP':
                            return $scope.AGENCY_ZIP;
                        case 'CLIENT_DOCTOR_MEDICAL_FACILITY':
                            return $scope.CLIENT_DOCTOR_MEDICAL_FACILITY;
                        case 'CLIENT_DOCTOR_NAME':
                            return $scope.CLIENT_DOCTOR_NAME;
                        case 'CLIENT_DOCTOR_PHONE':
                            return $scope.CLIENT_DOCTOR_PHONE;
                        case 'CLIENT_DOCTOR_SPECIALTY':
                            return $scope.CLIENT_DOCTOR_SPECIALTY;
                        case 'CLIENT_NAME':
                            return $scope.CLIENT_NAME;
                        case 'CLIENT_PHONE':
                            return $scope.CLIENT_PHONE;
                        case 'CLIENT_PROGRAM_UNIT':
                            return $scope.CLIENT_PROGRAM_UNIT;
                        case 'DOB':
                            return $scope.DOB;
                        case 'GET_DATE':
                            return $scope.GET_DATE;
                        case 'MEDICAID_NUMBER':
                            return $scope.MEDICAID_NUMBER;
                        case 'MRN':
                            return $scope.MRN;
                        case 'STAFF_NAME':
                            return $scope.STAFF_NAME;
                        case 'CLIENT_FULLNAME':
                            return $scope.CLIENT_FULLNAME;
                        case 'CONSENT_NAME':
                            return $scope.CONSENT_NAME;
                        case 'CLIENT_ADDRESS':
                            return $scope.CLIENT_ADDRESS;
                        case 'CLIENT_CITYSTATEZIP':
                            return $scope.CLIENT_CITYSTATEZIP;
                        case 'SERVICE_START_DATE':
                            return $scope.SERVICE_START_DATE;
                        case 'ASSESSMENT_ID':
                            return $scope.ASSESSMENT_ID;
                        case 'ASSESSMENT_SECTION_ID':
                            return $scope.ASSESSMENT_SECTION_ID;
                        case 'EXPIRE_ASSESSMENT_ID':
                            return $scope.EXPIRE_ASSESSMENT_ID;
                        case 'ALWAYS_TRUE':
                            return $scope.ALWAYS_TRUE;
                        case 'LIST_JSON':
                            return $scope.LIST_JSON;
                        case 'SERVICE_ENDDATE':
                            return $scope.SERVICE_ENDDATE;
                            break;
                        case 'CREDENTIAL_ID':
                            return $scope.CREDENTIAL_ID
                        case 'PROGRAM_UNIT_ID':
                            return $scope.PROGRAM_UNIT_ID
                        default:
                            return '';
                    }
                    return '';
                };

                $scope.saveAssessmentResponse = function () {
                    var deferred = $q.defer();
                    assessmentService.getAssessmentResponse($scope.contactId, $scope.AssessmentID, $scope.responseId).then(function (data) {
                        if (data.DataItems.length > 0) {
                            $scope.assessmentResponse = data.DataItems[0];
                            $scope.assessmentResponse.SectionID = $scope.sectionId;
                        }
                        if ($scope.assessmentResponse.ResponseID === undefined || $scope.assessmentResponse.ResponseID === 0) {
                            $scope.assessmentResponse = {
                                ResponseID: 0,
                                ContactID: $scope.contactId,
                                SectionID: $scope.sectionId,
                                AssessmentID: $scope.AssessmentID
                            };
                            return deferred.resolve(assessmentService.addAssessmentResponse($scope.assessmentResponse));
                        } else {
                            return deferred.resolve(assessmentService.updateAssessmentResponse($scope.assessmentResponse));
                        }
                    });
                    return deferred.promise;
                };

                $scope.saveAssessmentResponseDetails = function (responseId) {
                    var newAssessmentResponseDetails = [];
                    $scope.ModifiedOn = new Date();
                    var copyOfResponse = angular.copy($scope.responses);
                    validateParentChildQuestionConstraint(copyOfResponse);
                    for (var questionId in copyOfResponse) {
                        var response = copyOfResponse[questionId];
                        var inputTypeId = $scope.getQuestionInputType(questionId);
                        var newAssessmentResponseDetail = { ResponseDetailsID: 0, ResponseID: responseId, QuestionID: parseInt(questionId), Rating: 0 };
                        if (inputTypeId !== INPUT_TYPE.Inactive) {// Do nothing with inactive input
                            if (typeof response === "object") {
                                for (var optionsId in response) {
                                    if (response[optionsId] !== false)
                                        if (inputTypeId !== $scope.inputType.Comments && inputTypeId !== INPUT_TYPE.CheckboxList && inputTypeId !== INPUT_TYPE.DSignature && inputTypeId !== INPUT_TYPE.ESignature) {
                                            newAssessmentResponseDetails.push($.extend(true, {}, newAssessmentResponseDetail, {
                                                OptionsID: parseInt(optionsId),
                                                ResponseText: (inputTypeId == $scope.inputType.Textbox || inputTypeId == $scope.inputType.TextArea || inputTypeId == $scope.inputType.TimePicker ||
                                                            inputTypeId == $scope.inputType.Hidden || inputTypeId == $scope.inputType.Label || inputTypeId == $scope.inputType.ExtendedDropdown) ? response[optionsId] :
                                                    ((inputTypeId == $scope.inputType.DatePicker) ? (response[optionsId] ? $filter('formatDate')(response[optionsId]) : null) : null)
                                            }));
                                        } else if (inputTypeId == INPUT_TYPE.ESignature) {
                                            if (response[optionsId]) {
                                                newAssessmentResponseDetails.push($.extend(true, {}, newAssessmentResponseDetail, {
                                                    OptionsID: parseInt(optionsId),
                                                    SignatureBLOB: response[optionsId]
                                                }));
                                            }

                                        }
                                        else if (inputTypeId == INPUT_TYPE.DSignature) {
                                            var keyName = elementName(questionId, optionsId)
                                            if ($scope.dSignature[keyName] && $scope.dSignature[keyName].IsSigned) {
                                                var signModel = $scope.dSignature[keyName];
                                                var dSignData = { UserName: signModel.UserFullName, DateSigned: signModel.DateSigned, SignatureName: signModel.SignatureName, CredentialName: lookupService.getText('Credential', signModel.CredentialID) };
                                                newAssessmentResponseDetails.push($.extend(true, {}, newAssessmentResponseDetail, {
                                                    OptionsID: parseInt(optionsId),
                                                    ResponseText: JSON.stringify([dSignData]),
                                                    DateSigned: signModel.DateSigned,
                                                    CredentialID: signModel.CredentialID
                                                }));
                                            }
                                        }
                                        else if (inputTypeId === INPUT_TYPE.CheckboxList) {
                                            newAssessmentResponseDetails.push($.extend(true, {}, newAssessmentResponseDetail, {
                                                OptionsID: parseInt(optionsId),
                                                ResponseText: JSON.stringify(response[optionsId])
                                            }));
                                        }
                                        else if (!isNaN(optionsId)) {
                                            var comments = response[optionsId];
                                            var history = response[commentOption(optionsId)];
                                            if (comments != '') {
                                                var historyData = { UserName: $scope.STAFF_NAME ? $scope.STAFF_NAME : 'None', CommentDate: new Date(), Comment: comments };
                                                if (history && history.length > 0) {
                                                    history.unshift(historyData);
                                                }
                                                else {
                                                    history = [];
                                                    history.push(historyData);
                                                }
                                            }
                                            newAssessmentResponseDetails.push($.extend(true, {}, newAssessmentResponseDetail, {
                                                OptionsID: parseInt(optionsId),
                                                ResponseText: JSON.stringify(history)
                                            }));
                                        }
                                }
                            } else {
                                newAssessmentResponseDetails.push($.extend(true, {}, newAssessmentResponseDetail, {
                                    OptionsID: ((inputTypeId == $scope.inputType.Radio) || (inputTypeId == $scope.inputType.Select)) ? parseInt(response) : 0,
                                    ResponseText: ''
                                }));
                            }
                        }
                    }
                    return assessmentService.saveAssessmentResponseDetails(responseId, $scope.sectionId, newAssessmentResponseDetails);
                };

                $scope.save = function (isNext, mandatory, hasErrors, keepForm, next) {
                    $scope.saveAssessment(isNext, mandatory, hasErrors, keepForm, next).then(function (response) {
                        if (isNext && !hasErrors)
                            $scope.next();
                        if (response != "-1") {
                            alertService.success('Assessment response saved successfully.');
                        }
                    });
                };

                $scope.saveAssessment = function (isNext, mandatory, hasErrors, keepForm, next) {
                    var deferred = $q.defer();
                    var isDirty = formService.isDirty();
                    if (isDirty && !hasErrors) {
                        if (helperService.validateSignature($scope.ctrl.assessmentForm.signatureForm, $scope.dSignature)) {
                            return $scope.saveAssessmentResponse().then(
                            function (response) {
                                var data = response.data;
                                if (data.ResultCode === 0) {
                                    var responseId = ($scope.assessmentResponse.ResponseID === 0) ? data.ID : $scope.assessmentResponse.ResponseID;
                                    $scope.responseId = responseId;
                                    $scope.assessmentResponse.ResponseID = responseId;
                                    return $scope.saveAssessmentResponseDetails(responseId).then(function (response) {
                                        resetForm();
                                        return $scope.getAssessmentResponse();
                                    },
                                    function (errorStatus) {
                                        alertService.error('OOPS! Something went wrong');
                                        deferred.reject(null);
                                    },
                                    function (notification) {
                                        alertService.warning(notification);
                                        deferred.reject(null);
                                    });
                                } else if (data.ResultCode !== -1) {
                                    alertService.error('OOPS! Something went wrong');
                                    deferred.reject(null);
                                }
                            },
                            function (errorStatus) {
                                alertService.error('OOPS! Something went wrong');
                                deferred.reject(null);
                            });
                        }
                        else {
                            deferred.reject(null);
                        }
                    }
                    else {
                        deferred.resolve(-1);
                    }
                    return deferred.promise;
                };

                $scope.next = function (statePrms) {
                    if (_isNextWorkFlowClickState()) {
                        $timeout(function () {
                            angular.element("xen-workflow-action[data-state-name] > li.list-group-item.active").parents('.work-flow').next().find("xen-workflow-action[on-workflow-click] a").trigger('click');
                        });
                    }
                    else if (_isNextState()) {
                            angular.extend($stateParams, statePrms);
                            var stateParams = $stateParams;
                            stateParams.ResponseID = $scope.responseId;
                            stateParams.SectionID = Math.abs(nextState.attr('data-state-key'));
                            nextState = nextState.attr('data-state-name');
                            $state.go(nextState, stateParams);
                        }
                    
                };
                var _isNextWorkFlowClickState = function () {
                    return angular.element("xen-workflow-action[data-state-name] > li.list-group-item.active").parents('.work-flow').next().find("xen-workflow-action[on-workflow-click]").length === 0 ? false : true;
                }

                var _isNextState = function () {
                    nextState = angular.element("li[data-state-name].list-group-item.active").next("li[data-state-name].list-group-item")
                    if (nextState.length === 0) {
                        nextState = angular.element("xen-workflow-action[data-state-name] > li.list-group-item.active").parents('.work-flow').next().find("xen-workflow-action[data-state-name]");
                            if (nextState.length === 0)
                                return false;
                            else
                                return true;
                        }
                        else
                            return true;
                    }

                 $scope.getQuestionInputType = function (questionId) {
                    var inputType = $filter('filter')($scope.assessmentQuestions, function (question) {
                        return parseInt(question.QuestionID) === parseInt(questionId);
                    });
                    if (inputType.length) {
                        return inputType[0].InputTypeID;
                    }
                    else {
                        return INPUT_TYPE.Inactive;
                    }
                };

                var onTopazReady = function (newValues, oldValues, x) {
                    if (newValues == true) {
                        angular.forEach($scope.eSignature, function (obj, signIndx, signObj) {
                            initSign(obj)
                        });
                    }
                };

                var initSign = function (signObj) {
                    signObj.topazModel.Init();
                    signObj.topazModel.ImageCallback = $scope.sigImageCallback.bind({}, signObj.topazModel);
                    signObj.topazModel.NoPointsCallback = $scope.noPointsCallback;
                }

                $scope.noPointsCallback = function () {
                    $scope.save(false, true);
                };

                $scope.sigImageCallback = function (item, str) {
                    item.b64ImageData = str;
                    if (!formService.isDirty())
                        $rootScope.setform(true, 'default');
                    if (!$scope.responses[item.QuestionID]) {
                        $scope.responses[item.QuestionID] = {
                        };
                    }
                    $scope.responses[item.QuestionID][item.OptionID] = str;
                };

                if (!$scope.stopAutoRun) {
                    $scope.getAssessmentQuestions().then(function () {
                        var mainContainer = angular.element('#aq_0');
                        var parentContainer = angular.element("<div>");
                        mainContainer.attr("class", $scope.templateClassName ? $scope.templateClassName : '');
                        parentContainer.attr('id', mainContainer.attr('id'));
                        var workingContainer;
                        $scope.QuestionNumbering.reset();
                        $scope.QuestionSection.set(0, '');
                        $.each($scope.assessmentQuestions, function (index, question) {
                            var tmpParent = parentContainer.find('#aq_' + question.ParentQuestionID);
                            if (tmpParent.length === 0)
                                tmpParent.push(parentContainer);
                            workingContainer = angular.element(tmpParent[0]);
                            if (question.IsNumberingRequired === false)
                                $scope.QuestionNumbering.reset();
                            var nContent = $scope.getContent(question);
                            if (question.IsNumberingRequired === true) {
                                nContent.find('numbering').append(angular.element('<span>').html($scope.QuestionNumbering.current()));
                                $scope.QuestionNumbering.increment();
                            }
                            if (question.QuestionTypeID === $scope.questionType.SubQuestion)
                                workingContainer = workingContainer.find('.ase-subquestions:first');
                            workingContainer.append(nContent);
                        });
                        mainContainer.html(parentContainer.html());
                        $compile(mainContainer)($scope);
                        $('.timepicker').timepicker({
                            defaultTime: false
                        });
                        if (!$scope.isAssessmentIntegrated)
                            angular.element(mainContainer.find('a,input,select,textarea')[0]).focus();
                        $scope.$watch('isTopazReady', onTopazReady);
                        $scope.$broadcast('getComplete');
                        resetForm();
                        if (!_isNextWorkFlowClickState() && !_isNextState()) {
                                $scope.allowNext = false;
                        }
                    });
                };                

                var topazModel = function (name, questionID, optionID) {
                    var model = {
                            modelNumber: '',
                            b64ImageData: '',
                            DeviceMessage: 'You do not have application permissions to electronically sign!',
                            name: name,
                            QuestionID: questionID,
                            OptionID: optionID
                };
                    model.ClearCallback = clearSign.bind(model);
                    return { topazModel: model }
                };

                var dSignModel = function (name, questionID, optionID, detailID) {
                    if (!$scope.dSignature[name]) {
                        var model = {
                                DocumentTypeId: documentTypeID, //TODO :need to get it from service
                                DocumentId: detailID,
                                FormName: name,
                                QuestionID: questionID,
                                OptionID: optionID
                    };
                        model.signedCallBack = dSignCallBack.bind(model);
                        $scope.dSignature[name] = model;
                }
                    return $scope.dSignature[name];
                };

                var dSignCallBack = function () {
                    if (!$scope.responses[this.QuestionID]) {
                        $scope.responses[this.QuestionID] = {
                    };
                }
                    $scope.responses[this.QuestionID][this.OptionID] = this.SignatureName;
                }
                var clearSign = function () {
                    if (userCredentialList) {
                        $scope.CredentialList = userCredentialList;
                }
                    if ($scope.responses[this.QuestionID] && $scope.responses[this.QuestionID][this.OptionID])
                        $scope.responses[this.QuestionID][this.OptionID] = null;
                };
                var elementName = function (questionID, optionID) {
                    return questionID + '_' + optionID;
                }

                $scope.getContent = function (model) {
                    switch (model.QuestionTypeID) {
                        case $scope.questionType.Header:
                            return $scope.getHeaderContent(model);
                        case $scope.questionType.Section:
                            return $scope.getSectionContent(model);
                        case $scope.questionType.Subsection:
                            return $scope.getSubsectionContent(model);
                        case $scope.questionType.Question:
                            return $scope.getQuestionContent(model);
                        case $scope.questionType.SubQuestion:
                            return $scope.getQuestionContent(model, true);
                }
                    return '';
                };

                $scope.getNamedContainer = function (id) {
                    return angular.element('<div>').attr('id', 'aq_' + id);
                };

                $scope.getHeaderContent = function (model) {
                    return $scope.getNamedContainer(model.QuestionID).addClass('ase-header').append(angular.element('<numbering>')).append(angular.element('<h1>').html(model.Question));
                };

                $scope.getSectionContent = function (model) {
                    $scope.QuestionSection.set(model.QuestionID, model.AssessmentQuestionOptions[0].Options);
                    var sectionTitle = $scope.QuestionSection.getSectionTitle();
                    var sContent = $scope.getNamedContainer(model.QuestionID).addClass('ase-section nopadding').append(angular.element('<div>').addClass('divcell rcell').
                        append(angular.element('<div>').addClass('divtable').append(angular.element('<div>').addClass('divrow').append(angular.element('<div>').addClass('divcell qcell').
                        append(angular.element('<h2>').html(model.Question)).prepend(angular.element('<numbering>')).append(angular.element('<required>').html(model.IsAnswerRequired == 1 ? '*' : ''))))));
                    if (sectionTitle.length > 0) {
                        var oContent = $scope.getNamedContainer(model.QuestionID + '_t').addClass('ase-section-title form-inline').append(angular.element('<span>').html(sectionTitle));
                        switch (model.InputTypePositionID) {
                            case $scope.inputPositionType.Right:
                                sContent.find('div.qcell').addClass('ase-left').after(oContent.addClass('ase-right'));
                                break;
                            case $scope.inputPositionType.Left:
                                sContent.find('div.qcell').addClass('ase-right').before(oContent.addClass('ase-left'));
                                break;
                            case $scope.inputPositionType.Bottom:
                                sContent.find('div.divtable').append(angular.element('<div>').addClass('div.divrow').append(oContent));
                                break;
                            case $scope.inputPositionType.Inline:
                            default:
                                sContent.find('div.rcell').append(oContent);
                                break;
                    };
                }
                    return sContent;
                };

                $scope.getSubsectionContent = function (model) {
                    return $scope.getNamedContainer(model.QuestionID).addClass('ase-subsection section-block nopadding').html(model.Question).prepend(angular.element('<numbering>'));
                };

                $scope.getQuestionContent = function (model, isSubquestion) {
                    var qContent = $scope.getNamedContainer(model.QuestionID);

                    // Set container attribute
                    qContent = setAttributes(qContent, model.ContainerAttributes);

                    qContent = qContent.addClass('ase-question').append(angular.element('<div>').addClass('divcell rcell'));

                    var labelForInput = model.QuestionID;
                    if (model.InputTypeID !== $scope.inputType.Button && model.InputTypeID !== $scope.inputType.Select) {
                        model.AssessmentQuestionOptions.map(function (option) {
                            labelForInput += '_' + option.OptionsID;
                    });
                };

                    var labelStr;
                    if (model.InputTypeID !== $scope.inputType.None) {
                        labelStr = '<label for=' + labelForInput + '>';
                    } else {
                        labelStr = '<p>';
                }

                    var reqHtml = '';
                    switch (model.IsAnswerRequired) {
                        case 1:
                            reqHtml = '<sup> *</sup>'
                            break;
                        case 2:
                            reqHtml = '<sup> &#134</sup>'
                            $scope.cndRequired = true;
                            break;
                        default:
                            reqHtml = ''
                            break;
                }
                    qContent.append(angular.element('<div>'))
                    .append(angular.element('<div>').addClass('divtable')
                        .append(angular.element('<div>').addClass('divrow')
                            .append(angular.element('<div>').addClass('divcell qcell')
                                .append(angular.element(labelStr).html(model.Question + (reqHtml)))
                                .prepend(angular.element('<numbering>'))
                            )
                        )
                    );

                    var uContent = $scope.getNamedContainer(model.QuestionID + '_s').addClass('ase-subquestions divtable');
                    var oContent = $scope.getNamedContainer(model.QuestionID + '_a').addClass('ase-answers form-inline');

                    // Add has-error to display control in red bordered if validdation fails
                    oContent = decorateValidationClass(oContent, model);

                    if (model.InputTypeID !== $scope.inputType.None) {
                        switch (model.InputTypeID) {
                            case $scope.inputType.Checkbox:
                                model.AssessmentQuestionOptions.map(function (option, index) {
                                    var sContent = angular.element('<xen-checkbox>');

                                    // Set element attribute
                                    sContent = setAttributes(sContent, model.Attributes);

                                    // Set option attribute
                                    sContent = setAttributes(sContent, option.Attributes);

                                    oContent.append(sContent
                                        .attr('data-ng-model', 'responses[' + model.QuestionID + '][' + option.OptionsID + ']')
                                        .attr('data-checkbox-id', elementName(model.QuestionID, option.OptionsID))
                                        .attr('name', elementName(model.QuestionID, option.OptionsID))
                                        .attr('data-label', option.OptionsID == $scope.NoneOptionID ? '' : $scope.QuestionSection.getControlTitle(model.ParentQuestionID, option.Options)))
                            });
                                break;
                            case INPUT_TYPE.CheckboxList:
                                model.AssessmentQuestionOptions.map(function (option, index) {
                                    var divEle = angular.element('<div>');
                                    divEle.attr('ng-repeat', 'checkBox in responses[' + model.QuestionID + '][' + option.OptionsID + ']');
                                    var sContent = angular.element('<xen-checkbox>');
                                    // Set element attribute
                                    sContent = setAttributes(sContent, model.Attributes);

                                    // Set option attribute
                                    sContent = setAttributes(sContent, option.Attributes);

                                    divEle.append(sContent
                                        .attr('data-ng-model', 'checkBox.IsSelected')
                                        .attr('data-checkbox-id', 'checkBox.Id')
                                        .attr('name', 'checkBox.Id')
                                        .attr('data-label', '{{checkBox.LabelText}}')
                                    )
                                    oContent.append(divEle);
                            });
                                break;
                            case $scope.inputType.Radio:

                                model.AssessmentQuestionOptions.map(function (option, index) {
                                    var sContent = angular.element('<xen-radio-button>');

                                    // Set element attribute
                                    sContent = setAttributes(sContent, model.Attributes);

                                    // Set option attribute
                                    sContent = setAttributes(sContent, option.Attributes);

                                    oContent.append(sContent
                                        .attr('data-ng-model', 'responses[' + model.QuestionID + ']')
                                        .attr('data-ng-value', option.OptionsID)
                                        .attr('data-radio-button-id', elementName(model.QuestionID, option.OptionsID))
                                        .attr('name', elementName(model.QuestionID, option.OptionsID))
                                        .attr('data-label', option.Options));
                            });
                                break;
                            case $scope.inputType.Textbox:
                                model.AssessmentQuestionOptions.map(function (option) {
                                    var sContent = angular.element('<input>');

                                    // Set element attribute
                                    sContent = setAttributes(sContent, model.Attributes);

                                    oContent.append(sContent
                                        .attr('type', 'text')
                                        .addClass('form-control')
                                        .attr('data-ng-model', 'responses[' + model.QuestionID + '][' + option.OptionsID + ']')
                                        .attr('id', elementName(model.QuestionID, option.OptionsID))
                                        .attr('name', elementName(model.QuestionID, option.OptionsID)));
                            });
                                break;
                            case $scope.inputType.Label:        //Input type for controls that need to be prepopulated and disabled
                                model.AssessmentQuestionOptions.map(function (option) {
                                    var sContent = angular.element('<input>');

                                    // Set element attribute
                                    sContent = setAttributes(sContent, model.Attributes);

                                    oContent.append(sContent
                                        .addClass('form-control displayOnly')
                                        .attr('data-ng-readonly', 'true')
                                        .attr('data-ng-model', 'responses[' + model.QuestionID + '][' + option.OptionsID + ']')
                                        .attr('id', elementName(model.QuestionID, option.OptionsID))
                                        .attr('name', elementName(model.QuestionID, option.OptionsID)));
                            });
                                break;
                            case $scope.inputType.TextArea:
                                model.AssessmentQuestionOptions.map(function (option) {
                                    var sContent = angular.element('<xen-memobox>');

                                    // Set element attribute
                                    sContent = setAttributes(sContent, model.Attributes);
                                    sContent = sContent
                                        .attr('data-ng-model', 'responses[' + model.QuestionID + '][' + option.OptionsID + ']')
                                        .attr('rows', '5')
                                        .attr('name', elementName(model.QuestionID, option.OptionsID))
                                        .attr('id', elementName(model.QuestionID, option.OptionsID))


                                    var attributes = parseJSON(model.Attributes);
                                    if (!attributes || !attributes.hasOwnProperty('maxlength')) {
                                        sContent.attr('maxlength', '500');
                                }

                                    oContent.append(sContent);

                            });
                                break;
                            case $scope.inputType.Comments:
                                model.AssessmentQuestionOptions.map(function (option) {
                                    var sContent = angular.element('<xen-comment>');
                                    var optionHis = commentOption(option.OptionsID);
                                    // Set element attribute
                                    sContent = setAttributes(sContent, model.Attributes);
                                    var attrs = parseJSON(model.Attributes);
                                    if (!attrs || !attrs.hasOwnProperty('maxlength')) {
                                        sContent.attr('maxlength', '500');
                                }
                                    oContent.append(sContent
                                        .attr('data-comment-model', 'responses[' + model.QuestionID + '][' + option.OptionsID + ']')
                                        .attr('rows', '5')
                                        .attr('name', elementName(model.QuestionID, option.OptionsID))
                                        .attr('id', elementName(model.QuestionID, option.OptionsID))
                                        .attr('data-history-model', 'responses[' + model.QuestionID + '][\'' + commentOption(option.OptionsID) + '\']'));

                            });
                                break;
                            case $scope.inputType.Button: //TBD?
                                var sContent = angular.element('<button>');

                                // Set element attribute
                                sContent = setAttributes(sContent, model.Attributes);

                                oContent.append(sContent
                                    .addClass('form-control')
                                    .attr('data-ng-model', 'responses[' + model.QuestionID + ']')
                                    .attr('id', model.QuestionID));
                                break;
                            case $scope.inputType.Select:
                                var sContent = angular.element('<select>');

                                // Set element attribute
                                sContent = setAttributes(sContent, model.Attributes);

                                sContent = sContent.addClass('form-control ellipsis')
                                                .attr('data-ng-model', 'responses[' + model.QuestionID + ']')
                                                .attr('name', model.QuestionID)
                                                .attr('id', model.QuestionID);
                                // add select option
                                sContent.append(angular.element('<option>').attr('value', '').html('Select'));

                                model.AssessmentQuestionOptions.map(function (option) {
                                    var optionContent = angular.element('<option>');

                                    // Set option attribute
                                    optionContent = setAttributes(optionContent, option.Attributes);

                                    sContent.append(optionContent.attr('value', option.OptionsID).html(option.Options));
                            });
                                oContent.append(sContent);
                                break;
                            case $scope.inputType.ExtendedDropdown:
                                model.AssessmentQuestionOptions.map(function (option) {
                                    var sContent = angular.element('<select>');

                                    // Set element attribute
                                    sContent = setAttributes(sContent, model.Attributes);

                                    sContent = sContent.addClass('form-control ellipsis')
                                                    .attr('data-ng-model', 'responses[' + model.QuestionID + '][' + option.OptionsID + ']')
                                                    .attr('name', elementName(model.QuestionID, option.OptionsID))
                                                    .attr('id', elementName(model.QuestionID, option.OptionsID));
                                    // add select option
                                    sContent.append(angular.element('<option>').attr('value', '').html('Select'));

                                    oContent.append(sContent);
                            });

                                break;
                            case $scope.inputType.DatePicker: //Multiple? Really??
                                model.AssessmentQuestionOptions.map(function (option) {
                                    var sContent = angular.element('<xen-date-picker>');

                                    // Set element attribute
                                    sContent = setAttributes(sContent, model.Attributes);

                                    oContent.append(sContent
                                        .attr('ng-model', 'responses[' + model.QuestionID + '][' + option.OptionsID + ']')
                                        .attr('id', elementName(model.QuestionID, option.OptionsID))
                                        .attr('name', elementName(model.QuestionID, option.OptionsID))
                                        .attr('class', 'form-group-lg'));

                                    var sErrorMsg = angular.element('<p>Please select a valid date.</p>');
                                    oContent.append(sErrorMsg
                                       .attr('ng-cloak', '')
                                       .attr('id', elementName(model.QuestionID, option.OptionsID) + 'Error')
                                       .attr('ng-show', 'ctrl.assessmentForm["' + elementName(model.QuestionID, option.OptionsID) + '"].$error.date')
                                       .attr('class', 'error-block'));

                            });
                                break;
                            case $scope.inputType.TimePicker:
                                model.AssessmentQuestionOptions.map(function (option) {
                                    var sContent = angular.element('<xen-time-picker>');

                                    // Set element attribute
                                    sContent = setAttributes(sContent, model.Attributes);

                                    oContent.append(sContent
                                        .attr('ng-model', 'responses[' + model.QuestionID + '][' + option.OptionsID + ']')
                                        .attr('element-name', elementName(model.QuestionID, option.OptionsID))
                                        .attr('element-id', elementName(model.QuestionID, option.OptionsID)));
                            });
                                break;
                            case $scope.inputType.Hidden:
                                model.AssessmentQuestionOptions.map(function (option) {
                                    var sContent = angular.element('<input>');

                                    // Set element attribute
                                    sContent = setAttributes(sContent, model.Attributes);

                                    oContent.append(sContent
                                        .addClass('form-control')
                                        .attr('ng-show', 'false')
                                        .attr('data-ng-model', 'responses[' + model.QuestionID + '][' + option.OptionsID + ']')
                                        .attr('id', elementName(model.QuestionID, option.OptionsID))
                                        .attr('name', elementName(model.QuestionID, option.OptionsID)));
                            });
                                break;
                            case $scope.inputType.ESignature:
                                model.AssessmentQuestionOptions.map(function (option) {
                                    var sContent = angular.element('<e-signature>');

                                    // Set element attribute
                                    sContent = setAttributes(sContent, model.Attributes);

                                    var key = elementName(model.QuestionID, option.OptionsID);
                                    if (!$scope.eSignature[key]) {
                                        $scope.eSignature[key] = topazModel(key, model.QuestionID, option.OptionsID);
                                }
                                    oContent.append(sContent
                                        .attr('data-topaz-model', 'eSignature[\'' + key + '\' ].topazModel')
                                        .attr('data-topaz-ready', 'isTopazReady'))
                            });
                                break;
                            case $scope.inputType.DSignature:
                                model.AssessmentQuestionOptions.map(function (option) {
                                    var sContent = angular.element('<xen-digital-signature>');

                                    // Set element attribute
                                    sContent = setAttributes(sContent, model.Attributes);

                                    var key = elementName(model.QuestionID, option.OptionsID);
                                    $scope.dSignature[key] = dSignModel(key, model.QuestionID, option.OptionsID, 0);

                                    oContent.append(sContent
                                        .attr('signature-model', 'dSignature[\'' + key + '\' ]'))
                            });
                                break;
                    };
                        switch (model.InputTypePositionID) {
                            case $scope.inputPositionType.Right:
                                qContent.find('div.qcell').addClass('ase-left').after(oContent.addClass('ase-right'));
                                break;
                            case $scope.inputPositionType.Left:
                                qContent.find('div.qcell').addClass('ase-right').before(oContent.addClass('ase-left'));
                                break;
                            case $scope.inputPositionType.Bottom:
                                qContent.find('div.divtable').append(angular.element('<div>').addClass('div.divrow').append(oContent));
                                break;
                            case $scope.inputPositionType.Inline:
                            default:
                                qContent.find('div.rcell').append(oContent);
                                break;
                    };
                }
                    qContent.find('div.divtable:first').after(uContent);
                    return qContent;
                };

                var decorateValidationClass = function (container, model) {
                    if (model.Attributes) {
                        var attributes;
                        attributes = parseJSON(model.Attributes)
                        var name = '';
                        var validationExpression = '';
                        switch (model.InputTypeID) {
                            case $scope.inputType.Select:
                                name = model.QuestionID;
                                break;
                            default:
                                model.AssessmentQuestionOptions.map(function (option) {
                                    name = elementName(model.QuestionID, option.OptionsID);
                            });
                    }
                        if (attributes) {
                            if ("ng-required" in attributes || "date-required" in attributes || "xen-comment-required" in attributes) {
                                validationExpression += '(ctrl.assessmentForm["' + name + '"].$invalid && !ctrl.assessmentForm["' + name + '"].$pristine)' + ' || ';
                        }
                            if ("future-date-validate" in attributes) {
                                validationExpression += 'ctrl.assessmentForm["' + name + '"].$error.futureDate' + ' || ';
                        }
                            if ("past-date-validate" in attributes) {
                                validationExpression += 'ctrl.assessmentForm["' + name + '"].$error.pastDate' + ' || ';
                        }
                            if ("greater-than-date-validation" in attributes) {
                                validationExpression += 'ctrl.assessmentForm["' + name + '"].$error.greaterThanDate' + ' || ';
                        }
                            if ("less-than-date-validation" in attributes) {
                                validationExpression += 'ctrl.assessmentForm["' + name + '"].$error.lessThanDate' + ' || ';
                        }
                            if ("ng-pattern" in attributes) {
                                validationExpression += 'ctrl.assessmentForm["' + name + '"].$error.pattern' + ' || ';
                        }
                    }
                        var lastIndexOf = validationExpression.lastIndexOf("||");
                        validationExpression = validationExpression.substring(0, lastIndexOf).trim();

                        container = container.attr("ng-class", "{ 'has-error' : " + ((validationExpression && validationExpression.length > 0) ? validationExpression : false) + " }");
                }

                    return container;
                }

                var setAttributes = function (container, attribute) {
                    if (attribute) {
                        try {
                            container = container.attr(JSON.parse(attribute));
                        } catch (e) {
                    }
                }
                    return container;
                };

                    // Helpers
                var validateParentChildQuestionConstraint = function (copyOfResponse) {
                    // Step 1 - Find parent-identifier attribute & value
                    angular.forEach($scope.assessmentQuestions, function (question) {
                        if (question.Attributes) {
                            var parentAttribute = parseJSON(question.Attributes);
                            if (parentAttribute && 'parent-identifier' in parentAttribute) {
                                var parentIdentifier = parentAttribute['parent-identifier'];

                                // Step 2 - If parent-identifier's value is empty then clear child-identifier's value
                                if (!copyOfResponse[question.QuestionID]) {
                                    angular.forEach($scope.assessmentQuestions, function (childQuestion) {
                                        if (childQuestion.Attributes) {
                                            var childAttribute = parseJSON(childQuestion.Attributes);
                                            if (childAttribute && 'child-identifier' in childAttribute) {
                                                var childIdentifier = childAttribute['child-identifier'];
                                                if (parentIdentifier === childIdentifier) {
                                                    copyOfResponse[childQuestion.QuestionID] = null;
                                            }
                                        }
                                    }
                                });
                            }
                        }
                    }
                });
                }

                $scope.toggleElement = function (target) {
                    $('[identifier=' + target + ']').toggle();
                }

                $scope.removeAnswer = function () {
                    for (i = 0; i < arguments.length; i++) {
                        if (arguments[i]) {
                            $scope.responses[arguments[i]] = null;
                    }
                }
                }

                $scope.calculateScore = function (questionID, optionID) {
                    if ($scope.responses[questionID] == undefined)
                        $scope.responses[questionID] = {
                };
                    $timeout(function () {
                        $scope.responses[questionID][optionID] = $scope.calculateTotalScore();
                });
                }

                $scope.calculateTotalScore = function () {
                    var totalScore = 0;
                    // get score for radiobutton & checkbox
                    $("[scores].selected").each(function (index, item) {
                        totalScore = totalScore + parseInt($(item).attr("scores"));
                });

                    // get score for dropdown
                    $(":selected").each(function (index, item) {
                        if ($(item).attr("scores")) {
                            totalScore = totalScore + parseInt($(item).attr("scores"));
                    }
                });

                    return totalScore;
                };

                $scope.validateCredential = function (questionID) {
                    var dfd = $q.defer();
                    if (angular.element('#' + questionID + '_17').val() != '') {
                        dfd.resolve(null);
                    }
                    else {
                        alertService.error('Select credential to save signature.');
                        dfd.reject(null);
                }
                    return dfd.promise;
                };

                    function getAssessmentState(stateName, stateKey) {
                    if ($rootScope.workflowActions !== null) {
                        if (stateName in $rootScope.workflowActions)
                            return stateName;
                        else if (stateName + stateKey in $rootScope.workflowActions)
                            return stateName + stateKey;
                        else
                            return stateName;
                    } else
                        return stateName;
                };
            }
    ]);
