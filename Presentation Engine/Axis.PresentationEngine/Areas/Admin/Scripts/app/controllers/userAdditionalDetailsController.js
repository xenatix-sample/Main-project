angular.module('xenatixApp')
    .controller('userAdditionalDetailsController', [
        '$http', '$scope', '$q', '$rootScope', '$filter', '$state', '$stateParams', '$timeout', 'alertService', 'formService', 'lookupService', 'adminService', 'userDetailService', 'userCredentialService',
        function ($http, $scope, $q, $rootScope, $filter, $state, $stateParams, $timeout, alertService, formService, lookupService, adminService, userDetailService, userCredentialService) {

            // Const vars
            $scope.NPI_NUMBER = 'NPI Number';
            $scope.DEA_NUMBER = 'DEA Number';
            $scope.DPS_NUMBER = 'DPS Number';
            $scope.TOTAl_GETS_NUM = 3;
            
            // Scope vars
            $scope.userID = $stateParams.UserID;
            $scope.isEmployeeTypeInternal = true;
            $scope.isEmployeeInternalChanged = false;
            $scope.coSignatures = {
                Notes: [],
                Assessments: []
            };
            $scope.employeeInfos = [];
            $scope.employeeInfoNums = [];
            $scope.contractEmployees = [];
            $scope.coSignaturesForDbDelete = [];
            $scope.userIdentifiersForDbDelete = [];
            $scope.userAdditionalDetailsForDbDelete = [];
            $scope.coSignaturesChanged = false;
            $scope.userIdentifiersChanged = false;
            $scope.userAdditionalDetailsChanged = false;
            var invalid = false;
            ///////////////////////
            // Initialization stuff
            ///////////////////////
            $scope.init = function () {

                // Get lookup values
                $scope.usersLookupNotes = $scope.getLookupsByType('Users');
                angular.forEach($scope.usersLookupNotes, function (item) {
                    item.IsActive = true;
                })
                $scope.usersLookupAssessments = $scope.getLookupsByType('Users');
                angular.forEach($scope.usersLookupAssessments, function (item) {
                    item.IsActive = true;
                })

                $scope.documentTypeGroupIDAssesments = $filter('filter')(lookupService.getLookupsByType('DocumentTypeGroup'), { Name: 'Assessments' }, true)[0].ID;
                $scope.documentTypeGroupIDNotes = $filter('filter')(lookupService.getLookupsByType('DocumentTypeGroup'), { Name: 'Notes' }, true)[0].ID;

                var identifierTypes = lookupService.getLookupsByType('UserIdentifierType');
                $scope.userIdentifierNPI_ID = $filter('filter')(identifierTypes, { Name: $scope.NPI_NUMBER }, true)[0].ID;
                $scope.userIdentifierDEA_ID = $filter('filter')(identifierTypes, { Name: $scope.DEA_NUMBER }, true)[0].ID;
                $scope.userIdentifierDPS_ID = $filter('filter')(identifierTypes, { Name: $scope.DPS_NUMBER }, true)[0].ID;
                $scope.userIdentifierTypes = $filter('filter')(identifierTypes, function (item, key) {
                    if (item.Name != $scope.NPI_NUMBER && item.Name != $scope.DEA_NUMBER && item.Name != $scope.DPS_NUMBER)
                        return item.Name;
                });

                // Init stuff
                $scope.addEmptyNote();
                $scope.addEmptyAssessment();
                $scope.addEmptyEmployeeInfo();
                $scope.addEmptyContractEmployee();

                // Static rows
                var infonum1 = {
                    UserIdentifierDetailsID: 0,
                    Type: $scope.NPI_NUMBER,
                    UserIdentifierType: {
                        ID: $scope.userIdentifierNPI_ID,
                        Name: $scope.NPI_NUMBER
                    },
                    IDNumber: '',
                    EffectiveDate: '',
                    ExpirationDate: ''
                }
                var infonum2 = {
                    UserIdentifierDetailsID: 0,
                    Type: $scope.DEA_NUMBER,
                    UserIdentifierType: {
                        ID: $scope.userIdentifierDEA_ID,
                        Name: $scope.DEA_NUMBER
                    },
                    IDNumber: '',
                    EffectiveDate:'',
                    ExpirationDate: ''
                }
                var infonum3 = {
                    UserIdentifierDetailsID: 0,
                    Type: $scope.DPS_NUMBER,
                    UserIdentifierType: {
                        ID: $scope.userIdentifierDPS_ID,
                        Name: $scope.DPS_NUMBER
                    },
                    IDNumber: '',
                    EffectiveDate: '',
                    ExpirationDate: ''
                }
                $scope.employeeInfoNums.push(infonum1);
                $scope.employeeInfoNums.push(infonum2);
                $scope.employeeInfoNums.push(infonum3);
                $scope.get($scope.userID);
            };

            $scope.dateOptions = {
                formatYear: 'yy',
                startingDay: 1,
                showWeeks: 'false'
            };

            $scope.triggerTypeaheademployeeInfoType = function (index) {
                var element = 'employeeInfoType' + index;
                $('[name="' + element + '"]').focus();
                $('[name="' + element + '"]').trigger('forcelyOpenTypeaheadPopup');
                $scope.typeaheademployeeInfoTypeChange(index);
            };

            $scope.typeaheademployeeInfoTypeChange = function (index) {
                var id = '#employeeDiv' + index;
                setTimeout(function () { $scope.reposTypeahead(id); }, 500);
            };

            $scope.triggerTypeaheadnotes = function (index) {
                var element = 'noteSignedBy' + index;
                $('[name="' + element + '"]').focus();
                $('[name="' + element + '"]').trigger('forcelyOpenTypeaheadPopup');
                $scope.typeaheadnotesChange(index);
            };

            $scope.typeaheadnotesChange = function (index) {
                var id = '#notesDiv' + index;
                setTimeout(function () { $scope.reposTypeahead(id); }, 500);
            };

            $scope.triggerTypeaheadassessments = function (index) {
                var element = 'assessmentSignedBy' + index;
                $('[name="' + element + '"]').focus();
                $('[name="' + element + '"]').trigger('forcelyOpenTypeaheadPopup');
                $scope.typeaheadassessmentsChange(index);
            };

            $scope.typeaheadassessmentsChange = function (index) {
                var id = '#assessmentDiv' + index;
                setTimeout(function () { $scope.reposTypeahead(id); }, 500);
            };

            $scope.onSelect = function ($item, $model, $label, lookuptype) {
                // On select, validate dupes
                if (lookuptype == 'Notes') {
                    var ct = 0;
                    angular.forEach($scope.coSignatures.Notes, function (item, key) {
                        if (item.CoSignee.ID == $model.ID) {
                            ct++;
                            if (ct > 1) {
                                // Throw up alert and reset this model value
                                $scope.coSignatures.Notes[key].CoSignee = { ID: 0, Name: '' };
                                alertService.error("You cannot select duplicate Notes CoSignees.  Please make another selection.");
                                return;
                            }
                        }
                    });
                }
                else if (lookuptype == 'Assessments') {
                    var ct = 0;
                    angular.forEach($scope.coSignatures.Assessments, function (item, key) {
                        if (item.CoSignee.ID == $model.ID) {
                            ct++;
                            if (ct > 1) {
                                // Throw up alert and reset this model value
                                $scope.coSignatures.Assessments[key].CoSignee = { ID: 0, Name: '' };
                                alertService.error("You cannot select duplicate Assessment CoSignees.  Please make another selection.");
                                return;
                            }
                        }
                    });
                }
            }

            $scope.addEmptyNote = function () {
                var note = {
                    CoSignatureID: 0,
                    Item: 'Co-Signature of Notes',
                    DocumentTypeGroupID: $scope.documentTypeGroupIDNotes,
                    SignedBy: '',
                    CoSignee: {
                        ID: 0,
                        Name: ''
                    },
                    EffectiveDate:'',
                    ExpirationDate: ''
                }
                $scope.coSignatures.Notes.push(note);
            }

            $scope.addEmptyAssessment = function () {
                var ass = {
                    CoSignatureID: 0,
                    Item: 'Co-Signature of Assessments',
                    DocumentTypeGroupID: $scope.documentTypeGroupIDAssesments,
                    SignedBy: '',
                    CoSignee: {
                        ID: 0,
                        Name: ''
                    },
                    EffectiveDate: '',
                    ExpirationDate: ''
                }
                $scope.coSignatures.Assessments.push(ass);
            }

            $scope.addEmptyEmployeeInfo = function () {
                var info = {
                    UserIdentifierDetailsID: 0,
                    Type: '',
                    UserIdentifierType: {
                        ID: $scope.userIdentifierDPS_ID,
                        Name: ''
                    },
                    IDNumber: '',
                    EffectiveDate: '',
                    ExpirationDate: ''
                }
                $scope.employeeInfos.push(info);
            }

            $scope.addEmptyContractEmployee = function () {
                var emp = {
                    UserAdditionalDetailsID: 0,
                    ContractingEntity: '',
                    IDNumber: '',
                    EffectiveDate: '',
                    ExpirationDate: ''
                }
                $scope.contractEmployees.push(emp);
            }

            $scope.getUserIdentifierTypes = function () {
                return $scope.userIdentifierTypes;
            }

            $scope.getLookupsByType = function (typeName) {
                var list = lookupService.getLookupsByType(typeName);
                var newlist = [];
                angular.forEach(list, function (item, key) {
                    var cosignee = new Object();
                    cosignee.ID = item.ID;
                    cosignee.Name = item.Name;
                    newlist.push(cosignee);
                });
                return newlist;
            };

            $scope.getCustomLookupsByType = function (typeName) {
                if (typeName == 'Notes') {
                    var list = $filter('filter')($scope.usersLookupNotes, function (item, key) {
                        if (item.IsActive == true)
                            return item;
                    });
                    return list;
                }
                else if (typeName == 'Assessments') {
                    var list = $filter('filter')($scope.usersLookupAssessments, function (item, key) {
                        if (item.IsActive == true)
                            return item;
                    });
                    return list;
                }
                else {
                    var list = $filter('filter')($scope.userIdentifierTypes, function (item, key) {
                        if (item.IsActive == true)
                            return item;
                    });
                    return list;
                }
            };

            $scope.validateEffectiveDateLessThanExpirationDate = function (index, type) {

                if (type == 'note') {
                    if ($scope.coSignatures.Notes[index])
                        var effectiveDate = new Date($scope.coSignatures.Notes[index].EffectiveDate);
                    if ($scope.coSignatures.Notes[index])
                        var expirationDate = new Date($scope.coSignatures.Notes[index].ExpirationDate);
                    if ($scope.coSignatures.Notes[index] && $scope.coSignatures.Notes[index].EffectiveDate && $scope.coSignatures.Notes[index].ExpirationDate) {
                        if (effectiveDate > expirationDate) {
                            alertService.error('Expiration date should be greater than effective date');
                            $scope.coSignatures.Notes[index].ExpirationDate = null;
                        }
                    }
                }
                else if (type == 'assessment') {
                    if ($scope.coSignatures.Assessments[index])
                        var effectiveDate = new Date($scope.coSignatures.Assessments[index].EffectiveDate);
                    if ($scope.coSignatures.Assessments[index])
                        var expirationDate = new Date($scope.coSignatures.Assessments[index].ExpirationDate);
                    if ($scope.coSignatures.Assessments[index] && $scope.coSignatures.Assessments[index].EffectiveDate && $scope.coSignatures.Assessments[index].ExpirationDate) {
                        if (effectiveDate > expirationDate) {
                            alertService.error('Expiration date should be greater than effective date');
                            $scope.coSignatures.Assessments[index].ExpirationDate = null;
                        }
                    }
                }
                else if (type == 'employeeinfo') {
                    if ($scope.employeeInfos[index])
                        var effectiveDate = new Date($scope.employeeInfos[index].EffectiveDate);
                    if ($scope.employeeInfos[index])
                        var expirationDate = new Date($scope.employeeInfos[index].ExpirationDate);
                    if ($scope.employeeInfos[index] && $scope.employeeInfos[index].EffectiveDate && $scope.employeeInfos[index].ExpirationDate) {
                        if (effectiveDate > expirationDate) {
                            alertService.error('Expiration date should be greater than effective date');
                            $scope.employeeInfos[index].ExpirationDate = null;
                        }
                    }
                }
                else if (type == 'contractemployee') {
                    if ($scope.contractEmployees[index])
                        var effectiveDate = new Date($scope.contractEmployees[index].EffectiveDate);
                    if ($scope.contractEmployees[index])
                        var expirationDate = new Date($scope.contractEmployees[index].ExpirationDate);
                    if ($scope.contractEmployees[index] && $scope.contractEmployees[index].EffectiveDate && $scope.contractEmployees[index].ExpirationDate) {
                        if (effectiveDate > expirationDate) {
                            alertService.error('Expiration date should be greater than effective date');
                            $scope.contractEmployees[index].ExpirationDate = null;
                        }
                    }
                }
                else {
                    if ($scope.employeeInfoNums[index])
                        var effectiveDate = new Date($scope.employeeInfoNums[index].EffectiveDate);
                    if ($scope.employeeInfoNums[index])
                        var expirationDate = new Date($scope.employeeInfoNums[index].ExpirationDate);
                    if ($scope.employeeInfoNums[index] && $scope.employeeInfoNums[index].EffectiveDate && $scope.employeeInfoNums[index].ExpirationDate) {
                        if (effectiveDate > expirationDate) {
                            alertService.error('Expiration date should be greater than effective date');
                            $scope.employeeInfoNums[index].ExpirationDate = null;
                        }
                    }
                }

            };

            //////////////////////
            // Add/Remove Handlers
            //////////////////////
            $scope.addCosignatureNotesRow = function (event) {
                var note1 = {
                    CoSignatureID: 0,
                    Item: 'Co-Signature of Notes',
                    DocumentTypeGroupID: $scope.documentTypeGroupIDNotes,
                    SignedBy: '',
                    CoSignee: {
                        ID: 0,
                        Name: ''
                    },
                    EffectiveDate: '',
                    ExpirationDate: ''
                }
                $scope.coSignatures.Notes.push(note1);
                $scope.coSignaturesChanged = true;
            }

            $scope.deleteCosignatureNotesRow = function (note, index, event) {
                if (note.CoSignatureID != 0)
                    $scope.coSignaturesForDbDelete.push(note.CoSignatureID);
                $scope.coSignatures.Notes.splice(index, 1);
            }

            $scope.addCosignatureAssessmentsRow = function (event) {

                var ass1 = {
                    CoSignatureID: 0,
                    Item: 'Co-Signature of Assessments',
                    DocumentTypeGroupID: $scope.documentTypeGroupIDAssesments,
                    SignedBy: '',
                    CoSignee: {
                        ID: 0,
                        Name: ''
                    },
                    EffectiveDate: '',
                    ExpirationDate: ''
                }
                $scope.coSignatures.Assessments.push(ass1);
                $scope.coSignaturesChanged = true;
            }

            $scope.deleteCosignatureAssessmentsRow = function (assessment, index, event) {
                if (assessment.CoSignatureID != 0)
                    $scope.coSignaturesForDbDelete.push(assessment.CoSignatureID);
                $scope.coSignatures.Assessments.splice(index, 1);
            }

            $scope.addEmployeeInfoRow = function (event) {
                var info = {
                    UserIdentifierDetailsID: 0,
                    Type: '',
                    UserIdentifierType: {
                        ID: $scope.userIdentifierDPS_ID,
                        Name: ''
                    },
                    IDNumber: '',
                    EffectiveDate: '',
                    ExpirationDate: ''
                }
                $scope.employeeInfos.push(info);
                $scope.userIdentifiersChanged = true;
            }

            $scope.deleteEmployeeInfoRow = function (info, index, event) {
                if (info.UserIdentifierDetailsID != 0)
                    $scope.userIdentifiersForDbDelete.push(info.UserIdentifierDetailsID);
                $scope.employeeInfos.splice(index, 1);
            }

            $scope.addContractEmployeeRow = function (event) {
                var emp = {
                    UserAdditionalDetailsID: 0,
                    ContractingEntity: '',
                    IDNumber: '',
                    EffectiveDate: '',
                    ExpirationDate: ''
                }
                $scope.contractEmployees.push(emp);
                $scope.userAdditionalDetailsChanged = true;
            }

            $scope.deleteContractEmployeeRow = function (emp, index, event) {
                if (emp.UserAdditionalDetailID != 0)
                    $scope.userAdditionalDetailsForDbDelete.push(emp.UserAdditionalDetailID);
                $scope.contractEmployees.splice(index, 1);
            }

            $scope.isInternalChanged = function () {
                $scope.isEmployeeInternalChanged = true;
            }


            $scope.notesInputChange = function (index) {
                $scope.coSignaturesInputChange();
                $scope.typeaheadnotesChange(index);
            }

            $scope.assessmentsInputChange = function (index) {
                $scope.coSignaturesInputChange();
                $scope.typeaheadassessmentsChange(index);
            }

            $scope.employeeInputChange = function (index) {
                $scope.userIdentifiersInputChange();
                $scope.typeaheademployeeInfoTypeChange(index);
            }

            $scope.coSignaturesInputChange = function () {
                $scope.coSignaturesChanged = true;
            }

            $scope.userIdentifiersInputChange = function () {
                $scope.userIdentifiersChanged = true;
            }

            $scope.userAdditionalDetailsChange = function () {
                $scope.userAdditionalDetailsChanged = true;
            }

            $scope.$watch('coSignatures', function (newValue, oldValue) {
                $scope.coSignaturesChanged = true;
            }, true);

            $scope.$watch('employeeInfos', function (newValue, oldValue) {
                $scope.userIdentifiersChanged = true;
            }, true);

            $scope.$watch('employeeInfoNums', function (newValue, oldValue) {
                $scope.userIdentifiersChanged = true;
            }, true);

            $scope.$watch('contractEmployees', function (newValue, oldValue) {
                $scope.userAdditionalDetailsChanged = true;
            }, true);

            //////////////////////
            // Get/Save Methods
            //////////////////////
            $scope.get = function (userID) {

                $scope.isInValidct = 0;
                $scope.isValidDone = false;

                // Get user IsInternal info and additional details if needed
                userDetailService.get(userID).then(function (response) {
                    if (response.ResultCode === 0 && response.DataItems !== null && response.DataItems.length === 1) {
                        $scope.isEmployeeTypeInternal = response.DataItems[0].IsInternal;
                        $scope.currentUser = response.DataItems[0];
                        userDetailService.getUserAdditionalDetails(userID).then(function (response) {
                            if (response.ResultCode === 0 && response.DataItems != null) {
                                if (response.DataItems.length > 0 && response.DataItems[0].UserDetails.length > 0) {

                                    if (!$scope.isValidDone) {
                                        $scope.validateRightNav();
                                        $scope.isValidDone = true;
                                    }

                                    // Load contract employees
                                    $scope.contractEmployees = [];
                                    angular.forEach(response.DataItems[0].UserDetails, function (item, key) {
                                        item.EffectiveDate = (new moment(item.EffectiveDate))._d;
                                        item.ExpirationDate = (item.ExpirationDate == null) ? '' : (new moment(item.ExpirationDate))._d;
                                        $scope.contractEmployees.push(item);
                                    });
                                    $scope.resetForm();
                                }
                                else {
                                    $scope.isInValidct++;
                                    if ($scope.isInValidct == $scope.TOTAl_GETS_NUM)
                                        $scope.inValidateRightNav();
                                }
                            } else {
                                alertService.error('Error while retrieving user additional details! Please reload the page and try again.');
                            }
                            $timeout(function () { $scope.resetForm(); }, 500, false);
                        })
                    } else {
                        alertService.error('Error while loading the user\'s details! Please reload the page and try again.');
                    }
                    $scope.resetForm();
                });


                // Get cosignatures
                userDetailService.getCoSignatures(userID).then(function (response) {
                    if (response.ResultCode === 0 && response.DataItems != null) {
                        if (response.DataItems.length > 0 && response.DataItems[0].CoSignatures.length > 0) {

                            if (!$scope.isValidDone) {
                                $scope.validateRightNav();
                                $scope.isValidDone = true;
                            }
                            $scope.coSignatures = {
                                Notes: [],
                                Assessments: []
                            };

                            // Load co signatures
                            angular.forEach(response.DataItems[0].CoSignatures, function (item, key) {

                                // Adjust values from the server                                
                                var CoSignee = {};
                                CoSignee.ID = item.CoSigneeID,
                                CoSignee.Name = $filter('filter')(lookupService.getLookupsByType('Users'), { ID: item.CoSigneeID }, true)[0].Name
                                item.CoSignee = CoSignee;
                                item.EffectiveDate = (item.EffectiveDate && moment(item.EffectiveDate).isValid()) ? (new moment(item.EffectiveDate))._d : '';
                                item.ExpirationDate = (item.ExpirationDate == null) ? '' : (new moment(item.ExpirationDate))._d;

                                if (item.DocumentTypeGroupID == $scope.documentTypeGroupIDNotes) {
                                    $scope.coSignatures.Notes.push(item);
                                }
                                else if (item.DocumentTypeGroupID == $scope.documentTypeGroupIDAssesments) {
                                    $scope.coSignatures.Assessments.push(item);
                                }
                            });

                            if ($scope.coSignatures.Notes.length == 0)
                                $scope.addEmptyNote();
                            if ($scope.coSignatures.Assessments.length == 0)
                                $scope.addEmptyAssessment();
                        }
                        else {
                            $scope.isInValidct++;
                            if ($scope.isInValidct == $scope.TOTAl_GETS_NUM)
                                $scope.inValidateRightNav();
                        }
                    } else {
                        alertService.error('Error while retrieving co signatures! Please reload the page and try again.');
                    }
                    $scope.resetForm();
                })

                // Get user identifier details (Employee Information)
                userDetailService.getUserIdentifierDetails(userID).then(function (response) {
                    if (response.ResultCode === 0 && response.DataItems != null) {
                        if (response.DataItems.length > 0 && response.DataItems[0].UserDetails.length > 0) {

                            if (!$scope.isValidDone) {
                                $scope.validateRightNav();
                                $scope.isValidDone = true;
                            }
                            $scope.employeeInfos = [];

                            // Load user identifiers
                            angular.forEach(response.DataItems[0].UserDetails, function (item, key) {

                                // Adjust values from the server
                                var UserIdentifierType = {};
                                UserIdentifierType.ID = item.UserIdentifierTypeID,
                                UserIdentifierType.Name = $filter('filter')(lookupService.getLookupsByType('UserIdentifierType'), { ID: item.UserIdentifierTypeID }, true)[0].Name
                                item.UserIdentifierType = UserIdentifierType;
                                
                                item.EffectiveDate = (item.EffectiveDate && moment(item.EffectiveDate).isValid()) ? (new moment(item.EffectiveDate))._d : '';
                                item.ExpirationDate = (item.ExpirationDate == null) ? '' : (new moment(item.ExpirationDate))._d;


                                if (item.UserIdentifierType.ID == $scope.userIdentifierNPI_ID) {
                                    item.Type = $scope.NPI_NUMBER;
                                    $scope.employeeInfoNums[0] = item;
                                }
                                else if (item.UserIdentifierType.ID == $scope.userIdentifierDEA_ID) {
                                    item.Type = $scope.DEA_NUMBER;
                                    $scope.employeeInfoNums[1] = item;
                                }
                                else if (item.UserIdentifierType.ID == $scope.userIdentifierDPS_ID) {
                                    item.Type = $scope.DPS_NUMBER;
                                    $scope.employeeInfoNums[2] = item;
                                }
                                else
                                    $scope.employeeInfos.push(item);
                            });

                            if ($scope.employeeInfos.length == 0)
                                $scope.addEmptyEmployeeInfo();
                        }
                        else {
                            $scope.isInValidct++;
                            if ($scope.isInValidct == $scope.TOTAl_GETS_NUM)
                                $scope.inValidateRightNav();
                        }
                    } else {
                        alertService.error('Error while retrieving user identifiers! Please reload the page and try again.');
                    }
                    $scope.resetForm();
                })
            };

            $scope.validateRightNav = function () {
                isValid = true;
                var obj = { stateName: $state.current.name, validationState: 'valid' };
                $rootScope.staffManagementRightNavigationHandler(obj);
            }

            $scope.inValidateRightNav = function () {
                var invalidObj = { stateName: $state.current.name, validationState: 'warning' };
                $rootScope.staffManagementRightNavigationHandler(invalidObj);
            }

            $scope.save = function (isNext, mandatory, hasErrors) {
                if (formService.isDirty() && !hasErrors) {
                    $scope.removePendingDbDeletes();

                } else if (!formService.isDirty() && isNext) {
                    $scope.handleNextState();
                } else {
                    return $scope.promiseNoOp();
                }
            };

            $scope.removePendingDbDeletes = function () {

                // Queue up all the pending deletes
                $scope.pendingDbDeleteCt = $scope.coSignaturesForDbDelete.length +
                    $scope.userIdentifiersForDbDelete.length +
                    $scope.userAdditionalDetailsForDbDelete.length;
                if ($scope.pendingDbDeleteCt == 0) {
                    //$scope.resetForm();
                    $scope.postSave();
                } else {
                    var pendingDbDeletesDeffered = $q.defer();
                    var pendingDeletes = [];
                    angular.forEach($scope.coSignaturesForDbDelete, function (item, key) {
                        pendingDeletes.push(userDetailService.deleteCoSignatures(item));
                    });
                    angular.forEach($scope.userIdentifiersForDbDelete, function (item, key) {
                        pendingDeletes.push(userDetailService.deleteUserIdentifierDetails(item));
                    });
                    angular.forEach($scope.userAdditionalDetailsForDbDelete, function (item, key) {
                        pendingDeletes.push(userDetailService.deleteUserAdditionalDetails(item));
                    });
                    $q.all(pendingDeletes).then(function (response) {
                        $scope.coSignaturesForDbDelete = [];
                        $scope.userIdentifiersForDbDelete = [];
                        $scope.userAdditionalDetailsForDbDelete = [];
                        $scope.resetForm();
                        $scope.postSave();
                    });
                    pendingDbDeletesDeffered.resolve();
                }
            }

            $scope.checkForPostSave = function () {
                $scope.pendingDbDeleteDone++
                if ($scope.pendingDbDeleteDone == $scope.pendingDbDeleteCt)
                    $scope.postSave();
            }

            $scope.resetForm = function () {
                if ($scope.ctrl)
                    $rootScope.formReset($scope.ctrl.userAdditionalDetailsForm);
                $scope.coSignaturesChanged = false;
                $scope.userIdentifiersChanged = false;
                $scope.userAdditionalDetailsChanged = false;
            }

            $scope.postSave = function (isNext) {

                angular.forEach($scope.employeeInfos, function (item) {
                    if (item.UserIdentifierType != null && item.UserIdentifierType.Name != '' && item.IDNumber == '')
                        invalid = true;
                });

                if (invalid) {
                    alertService.error("Please be sure to fill ID Number for Employee Information section.");
                    invalid = false;
                    return false;
                }

                // Queue up items to save
                var pendingSavesDeffered = $q.defer();
                var pendingSaves = [];

                // User IsInternal update
                if ($scope.isEmployeeInternalChanged == true) {
                    $scope.currentUser.IsInternal = $scope.isEmployeeTypeInternal;
                    pendingSaves.push(userDetailService.update($scope.currentUser));
                }
                $scope.isEmployeeInternalChanged = false;

                // CoSignatures
                var CoSignaturesModelUpdates = {};
                CoSignaturesModelUpdates.CoSignatures = [];
                var CoSignaturesModelAdds = {};
                CoSignaturesModelAdds.CoSignatures = [];
                if ($scope.coSignaturesChanged == true) {
                    angular.forEach($scope.coSignatures.Notes.concat($scope.coSignatures.Assessments), function (item) {
                        if (item.CoSignee.ID != 0) {
                            if (item.EffectiveDate == '')
                                invalid = true;
                            else {
                                item.EffectiveDate = $filter('formatDate')(item.EffectiveDate);
                            }
                            if (item.ExpirationDate)
                                item.ExpirationDate = $filter('formatDate')(item.ExpirationDate);
                            item.CoSigneeID = item.CoSignee.ID;
                            item.UserID = $scope.userID;
                            if (item.CoSignatureID == 0)
                                CoSignaturesModelAdds.CoSignatures.push(item);
                            else
                                CoSignaturesModelUpdates.CoSignatures.push(item);
                        }
                    });
                }

                if (invalid)
                {
                    invalid = false;
                    alertService.warning("Please be sure to fill effective date for CoSignature.");
                    return false;
                }

                if (CoSignaturesModelAdds.CoSignatures.length > 0)
                    pendingSaves.push(userDetailService.addCoSignatures(CoSignaturesModelAdds));
                if (CoSignaturesModelUpdates.CoSignatures.length > 0)
                    pendingSaves.push(userDetailService.updateCoSignatures(CoSignaturesModelUpdates));

                // User identifiers (employee information section)
                var UserIdentifierDetailsModelUpdates = {};
                UserIdentifierDetailsModelUpdates.UserDetails = [];
                var UserIdentifierDetailsModelAdds = {};
                UserIdentifierDetailsModelAdds.UserDetails = [];
                if ($scope.userIdentifiersChanged == true) {
                    angular.forEach($scope.employeeInfos.concat($scope.employeeInfoNums), function (item) {
                        
                        // Only save those identifiers with filled out IDNumber/EffectiveDate fields
                        if (item.UserIdentifierTypeID != 0 && item.IDNumber != '') {
                            item.UserIdentifierTypeID = item.UserIdentifierType.ID;
                            item.UserID = $scope.userID;
                            if (item.EffectiveDate)
                                item.EffectiveDate = $filter('formatDate')(item.EffectiveDate);
                            if (item.ExpirationDate)
                                item.ExpirationDate = $filter('formatDate')(item.ExpirationDate);
                            if (item.UserIdentifierDetailsID == 0)
                                UserIdentifierDetailsModelAdds.UserDetails.push(item);
                            else
                                UserIdentifierDetailsModelUpdates.UserDetails.push(item);
                        }
                    });
                }

                if (UserIdentifierDetailsModelAdds.UserDetails.length > 0)
                    pendingSaves.push(userDetailService.addUserIdentifierDetails(UserIdentifierDetailsModelAdds));
                if (UserIdentifierDetailsModelUpdates.UserDetails.length > 0)
                    pendingSaves.push(userDetailService.updateUserIdentifierDetails(UserIdentifierDetailsModelUpdates));

                // User Additional details (contract employee section) - only for external employees
                var UserAdditionalDetailsModelUpdates = {};
                UserAdditionalDetailsModelUpdates.UserDetails = [];
                var UserAdditionalDetailsModelAdds = {};
                UserAdditionalDetailsModelAdds.UserDetails = [];
                if ($scope.userAdditionalDetailsChanged == true) {
                    pendingSaves.push(userDetailService.update($scope.currentUser));
                    angular.forEach($scope.contractEmployees, function (item) {
                        if (item.ContractingEntity != '' && item.IDNumber != '') {
                            item.UserID = $scope.userID;
                            if (item.EffectiveDate)
                                item.EffectiveDate = $filter('formatDate')(item.EffectiveDate);
                            if (item.ExpirationDate)
                                item.ExpirationDate = $filter('formatDate')(item.ExpirationDate);
                            if (item.UserAdditionalDetailsID == 0)
                                UserAdditionalDetailsModelAdds.UserDetails.push(item);
                            else
                                UserAdditionalDetailsModelUpdates.UserDetails.push(item);
                        }
                    });
                }
                if (UserAdditionalDetailsModelAdds.UserDetails.length > 0)
                    pendingSaves.push(userDetailService.addUserAdditionalDetails(UserAdditionalDetailsModelAdds));
                if (UserAdditionalDetailsModelUpdates.UserDetails.length > 0)
                    pendingSaves.push(userDetailService.updateUserAdditionalDetails(UserAdditionalDetailsModelUpdates));

                $q.all(pendingSaves).then(function (response) {
                    var resultCode = 0;
                    //ensure all request has been processed successfully
                    for (var i = 0; i < response.length; i++) {
                        if (response[i].ResultCode == -1) {
                            resultCode = response[i].ResultCode;
                            break;
                        }
                    }
                    if (resultCode == 0) {
                        $scope.validateRightNav();
                        alertService.success("User's additional details has been saved successfully.");
                        $scope.resetForm();
                    }
                    else {
                        alertService.error("Error while saving User's additional details.");
                        $scope.resetForm();
                    }
                  
                    if (isNext) {
                        $scope.handleNextState();
                    } else {
                        // TODO: Need to do this???
                        $scope.get($scope.userID);
                        $scope.resetForm();
                    }
                });
                pendingSavesDeffered.resolve();
            };

            $scope.reposTypeahead = function (id) {
                var typeaheadElement = angular.element(document.querySelector(id + ' .dropdown-menu'));
                if (typeaheadElement !== null && typeaheadElement !== undefined && typeaheadElement[0] !== undefined)
                    $scope.repositionElement(typeaheadElement);
            }

            $scope.reposDatepicker = function (id) {
                var datepickerElement = angular.element(document.querySelector('#' + id + ' .dropdown-menu'));
                if (datepickerElement !== null && datepickerElement !== undefined && datepickerElement[0] !== undefined)
                    $scope.repositionElement(datepickerElement);
            }

            $scope.repositionElement = function (reposElement) {
                windowHeight = $(window)[0].screen.availHeight;
                elementHeight = reposElement[0].getBoundingClientRect().height;
                var top = reposElement.parent()[0].getBoundingClientRect().top + 37;
                var left = reposElement.parent()[0].getBoundingClientRect().left;

                if (top + elementHeight < windowHeight) {
                    reposElement.css('top', top + 'px');
                    reposElement.css('left', left + 'px');
                }
                else {
                    var bottom = reposElement.parent()[0].getBoundingClientRect().top - elementHeight;
                    reposElement.css('top', bottom + 'px');
                    reposElement.css('left', left + 'px');
                }
            }

            $scope.init();
        }
    ]);