(function () {
    angular.module('xenatixApp')
    .controller('lettersDetailsController', ['$scope', '$q', '$stateParams', '$state', '$filter', 'lettersService', 'formService', 'alertService', 'navigationService', 'registrationService', 'contactAddressService', 'lookupService', 'contactBenefitService', 'assessmentPrintService','WorkflowHeaderService',
    function ($scope, $q, $stateParams, $state, $filter, lettersService, formService, alertService, navigationService, registrationService, contactAddressService, lookupService, contactBenefitService, assessmentPrintService, WorkflowHeaderService) {
        var Staff_Id;
        var contactID = $stateParams.ContactID;
        $scope.GET_DATE = $filter('toMMDDYYYYDate')(new Date, 'MM/DD/YYYY', 'useLocal');
        $scope.AGENCY_ADDRESS = AGENCY_Data.ADDRESS;
        $scope.AGENCY_CITY = AGENCY_Data.CITY;
        $scope.AGENCY_NAME = AGENCY_Data.NAME;
        $scope.AGENCY_STATE = AGENCY_Data.STATE;
        $scope.AGENCY_ZIP = AGENCY_Data.ZIP;
        $scope.MEDICAID_NUMBER = 'N/A';

        var getNavigationDefaultData = function () {
            var dfd = $q.defer();
            navigationService.get().then(function (response) {
                if (hasData(response)) {
                    $scope.CredentialList = $filter('orderBy')(response.DataItems[0].UserCredentials, 'CredentialName');
                    if ($scope.CredentialList && $scope.CredentialList.length == 1) {
                        $scope.CREDENTIAL_ID = $scope.CredentialList[0].CredentialID;
                    }
                    $scope.STAFF_NAME = response.DataItems[0].UserFullName;
                    Staff_Id = response.DataItems[0].UserID;
                    dfd.resolve(response);
                    var stateCheck = $('.list-group-item.active[data-title=Summary]')
                    if (stateCheck.length) {
                        $scope.hideNextAssessment = true;
                        $scope.stopNext = true;
                    }
                }
                else {
                    dfd.resolve(null);
                }
            });
            return dfd.promise;
        };

        var getContactBenefitDefaultData = function (contactID) {
            var dfd = $q.defer();
            contactBenefitService.get(contactID).then(function (cbdata) {
                if (hasData(cbdata)) {
                    var payors = $filter('filter')(cbdata.DataItems, function (itm) {
                        return itm.PayorName.toString().toLowerCase().indexOf('medicaid') > -1;
                    });
                    if (hasData(payors)) {
                        $scope.MEDICAID_NUMBER = payors[0].PolicyID;
                        dfd.resolve(cbdata);
                    }
                    else {
                        dfd.resolve(cbdata);
                    }
                }
                else {
                    dfd.resolve(null);
                }
            });
            return dfd.promise;
        };

        var getRegistrationDefaultData = function (contactID) {
            var dfd = $q.defer();
            registrationService.get(contactID).then(function (regdata) {
                if (hasData(regdata)) {
                    var contactDetails = regdata.DataItems[0];
                    $scope.MRN = (contactDetails.MRN) ? contactDetails.MRN : '';
                    $scope.CLIENT_NAME = contactDetails.FirstName + ' ' + contactDetails.LastName;
                    $scope.DOB = (contactDetails.DOB) ? $filter('toMMDDYYYYDate')(contactDetails.DOB, 'MM/DD/YYYY') : '';
                    contactAddressService.get(contactID, contactDetails.ContactTypeID).then(function (addressdata) {
                        if (hasData(addressdata)) {
                            var contactAddresses = $filter('filter')(addressdata.DataItems, { IsPrimary: true }, true);
                            if (contactAddresses && contactAddresses.length == 0) {
                                contactAddresses = $filter('orderBy')(addressdata.DataItems, function (item) {
                                    return new Date(item.ModifiedOn);
                                }, true);
                            }
                            var contactAddress = contactAddresses[0];
                            var line1 = contactAddress.Line1 ? contactAddress.Line1 : '';
                            var line2 = contactAddress.Line2 ? contactAddress.Line2 : '';
                            $scope.CLIENT_ADDRESS = line1 + (line1 ? ' ' : '') + line2;
                            var stateName = lookupService.getText('StateProvince', contactAddress.StateProvince);
                            var stateProvince = ((contactAddress.City && contactAddress.StateProvince) ? ', ' : '') + stateName;
                            var zipCode = (((contactAddress.StateProvince || contactAddress.City) && contactAddress.Zip) ? ', ' : '') + contactAddress.Zip
                            $scope.CLIENT_CITYSTATEZIP = (contactAddress.City ? contactAddress.City : '') + (stateName ? stateProvince : '') + (zipCode ? zipCode : '');
                        }
                        dfd.resolve(regdata);
                    });
                }
                else {
                    dfd.resolve(null);
                }
            });
            return dfd.promise;
        };

        var prepopulatedDataFromServices = function () {
            var dfd = $q.defer();
            var servicesPromises = [];
            servicesPromises.push(getNavigationDefaultData());
            servicesPromises.push(getRegistrationDefaultData(contactID));
            servicesPromises.push(getContactBenefitDefaultData(contactID));
            $q.all(servicesPromises).then(function (servicesData) {
                dfd.resolve(servicesData);
            }, function (errorMessage) {
                alertService.error(errorMessage);
            });
            return dfd.promise;
        };

        $scope.prepopulatedData = function () {
            return prepopulatedDataFromServices().then(function () {
                //disable letter screen except summary screen
                lettersService.get($stateParams.ContactID).then(function (lettersData) {
                    if (hasData(lettersData)) {
                        var letterInformation = $filter('filter')(lettersData.DataItems, { ContactID: $stateParams.ContactID, ContactLettersID: $stateParams.ContactLettersID, AssessmentID: $stateParams.AssessmentID }, true);
                        if (letterInformation && letterInformation.length > 0 && ($.inArray($stateParams.SectionID, [70, 72, 74]) === -1)) {
                            var isDisabledScreen=letterInformation[0].SentDate ? true : false;
                            $scope.hasLetterSentDate = isDisabledScreen;
                            $scope.disableAssessmentSave = isDisabledScreen;
                        }
                    }
                });
            });
        };

        $scope.saveletters = function (isNext, mandatory, hasErrors, keepForm, next) {
            var isFormDirty = formService.isDirty();
            if (isFormDirty)
            {
                if (isCheckBoxListSelected()){
                    $scope.saveAssessment(isNext, mandatory, hasErrors, keepForm, next).then(function (response) {
                        saveLetterDetails(isNext, response);
                    });
                }
            }
            else if (isNext) {
                $scope.next();
            }
        };

        var saveLetterDetails = function (isNext, response) {
            var deferred = $q.defer();
            if (response != -1) {
                var lettersModel = {
                    ContactLettersID: $stateParams.ContactLettersID,
                    ContactID: contactID,
                    AssessmentID: $stateParams.AssessmentID,
                    ResponseID: $scope.responseId,
                    UserID: Staff_Id,
                    ModifiedOn: $scope.ModifiedOn
                }

                angular.forEach($scope.responses, function (value, key) {
                    if (value) {
                        if (value['4199']) {
                            lettersModel.SentDate = $filter('formatDate')(value['4199'], 'MM/DD/YYYY');
                        }
                        if (value['4200']) {
                            lettersModel.LetterOutcomeID = value['4200'];
                        }
                        if (value['4201']) {
                            lettersModel.Comments = value['4201'];
                        }
                    }
                });

                //Code to save letters 
                var readOnly = "edit";
                if ($stateParams.ContactLettersID && $stateParams.ContactLettersID > 0) {
                    lettersService.update(lettersModel).then(function (response) {
                        if (response.data.ID != "-1") {
                            alertService.success('Letter updated successfully.');
                            //save workflow header.
                            saveWorkflowHeader($stateParams.AssessmentID,$stateParams.ContactLettersID);
                            if (isNext)
                                $scope.next();

                            deferred.resolve(response);
                        }
                        else {
                            alertService.error('OOPS! Something went wrong');
                            deferred.reject();
                        }
                    },
            function (errorStatus) {
                alertService.error('OOPS! Something went wrong');
                deferred.reject();
            });
                }
                else {
                    lettersService.add(lettersModel).then(function (response) {
                        if (response.data.ID != "-1") {
                            alertService.success('Letter saved successfully.');
                            deferred.resolve(response);
                            angular.extend($stateParams, { ResponseID: lettersModel.ResponseID, ContactLettersID: response.data.ID, ReadOnly: readOnly });
                            //save workflow header.
                            saveWorkflowHeader($stateParams.AssessmentID, $stateParams.ContactLettersID);
                            if (isNext)
                                $scope.next($stateParams);
                            else {
                                $state.transitionTo($state.current.name, $stateParams);
                            }
                        }
                        else {
                            alertService.error('OOPS! Something went wrong');
                            deferred.reject();
                        }
                    },
            function (errorStatus) {
                alertService.error('OOPS! Something went wrong');
                deferred.reject();
            });
                }
            }
            else {
                deferred.resolve(response);
            }
            return deferred.promise;
        }

        var isCheckBoxListSelected = function () {
            var val = false;
            var elements = angular.element($("[multi-Checkbox]"));
            var messageText;
            if (elements.length == 0) {
                val = true;
            } else {
                angular.forEach(elements, function (item) {
                    messageText = item.attributes["data-validation-name"].value;
                    if (!messageText) {
                        messageText = item.attributes["validation-name"].value;
                    }
                    if (item.attributes["disabled"] || item.attributes["class"].value.indexOf("selected") > -1) {
                        //If item is disaled or is checked
                        val = true;
                    }
                });
            }
            if (!val) {
                if (!messageText) {
                    messageText = 'Select';
                }
                alertService.error('Please fill out the ' + messageText + ' field.');
            }
            return val;
        };

        $scope.initLetterReport = function (response) {
            if (isCheckBoxListSelected()) {
                return saveLetterDetails(false, response)
                    .then(function(response) {
                        let workflowDataKey = GetContactLettersWorkFlowDataKey($stateParams.AssessmentID);
                        //$stateParams.workflowDataKey = workflowDataKey;
                        //$stateParams.workflowHeaderID = $stateParams.ContactLettersID;
                        return assessmentPrintService
                            .initReports(undefined,
                                undefined,
                                undefined,
                                workflowDataKey,
                                $stateParams.ContactLettersID);
                    });
            }
        }

        var saveWorkflowHeader = function(assessmentID, headerID)
        {
            var workflowDatakey = GetContactLettersWorkFlowDataKey(assessmentID, headerID);

            //$rootScope.$broadcast('onWorkflowHeaderSave', { workflowDataKey: workflowDatakey, workflowHeaderID: headerID });
            WorkflowHeaderService.AddWorkflowHeader({ WorkflowDataKey: workflowDatakey, RecordHeaderID: headerID, ContactID: $stateParams.ContactID });
        }
    }]);
}());
