angular.module('xenatixApp')
    .controller('financialAssessmentController', ['$scope', '$modal', 'alertService', 'financialAssessmentService', 'lookupService', '$filter', '$stateParams', '$state', '$rootScope', 'formService', 'roleSecurityService', '$q', 'action',
function ($scope, $modal, alertService, financialAssessmentService, lookupService, $filter, $stateParams, $state, $rootScope, formService, roleSecurityService, $q, action) {
    $scope.isDisabled = false;
    var financialsTable = $("#financialsTable");
    var activeFinancialAssessment;
    var headerFormName = 'ctrl.financialAssessmentForm.header';
    var detailFormName = 'ctrl.financialAssessmentForm.financialAssessmentDetailsForm';
    $scope.permissionKey = $state.current.data.permissionKey;
    var resetMainForm = function () {
        $rootScope.formReset($scope.ctrl.financialAssessmentForm, $scope.ctrl.financialAssessmentForm.$name);
    };

    var resetDetailForm = function () {
        $rootScope.formReset($scope.ctrl.financialAssessmentForm.financialAssessmentDetailsForm, $scope.ctrl.financialAssessmentForm.financialAssessmentDetailsForm.$name);
    };

    var resetForms = function () {
        resetMainForm();
        resetDetailForm();
    };

    var checkFormStatus = function () {
        $scope.$watch('ctrl.financialAssessmentForm.$valid', function (newValue) {
            $rootScope.$broadcast($state.current.name, {
                validationState: ($scope.assessmentDetailsList.length > 0 ? 'valid' : 'warning')
            });
        });
    };

    $scope.controlsVisible = true;
    var financialAssessmentGroupID = RELATIONSHIP_TYPE_GROUPID.FinancialAssessment;

    $scope.initializeBootstrapTable = function () {

        $scope.tableoptions = {
            pagination: true,
            pageSize: 10,
            pageList: [10, 25, 50, 100],
            search: false,
            showColumns: true,
            data: [],
            undefinedText: '',
            //onClickRow: function (e, row, $element) {
            //    row.find("[data-default-action]").triggerHandler('click');
            //},
            columns: [
            {
                field: 'FinancialCategoryType',
                title: 'Type'
            },
            {
                field: 'Amount',
                title: 'Amount'
                    , formatter: function (value, row, index) {
                        return $filter('currency')(value, '$', 2);
                    }
            },
            {
                field: 'Frequency',
                title: 'Frequency'
            },
            {
                field: 'Category',
                title: 'Category'
            },
            {
                field: 'RelationshipTypeID',
                title: 'Family Member',
                formatter: function (value, row, index) {
                    if (value) {
                        return lookupService.getText('RelationshipType', value);
                    }
                    else {
                        return "";
                    }
                }
            },
            {
                field: 'FinancialAssessmentDetailsID',
                title: '',
                sortable: false,
                formatter: function (value, row, index) {
                    var hasUpdatePermission = roleSecurityService.hasPermission($scope.permissionKey, PERMISSION.UPDATE)
                    return !$scope.isDisabled ? ('<a href="javascript:void(0)" data-default-action id="editAssessment" name="editAssessment" data-toggle="modal" data-target="#adminModel" ng-click="editAssessment(' + value + ')" title="' + (hasUpdatePermission ? "Edit" : "View") + '" security permission-key="{{permissionKey}}" permission="' + (hasUpdatePermission ? 'update' : 'read') + '" space-key-press><i class="fa ' + (hasUpdatePermission ? 'fa-pencil' : 'fa-eye') + ' fa-fw" /></a>' +
                        '<a href="javascript:void(0)" data-default-no-action ng-click="remove(' + value + ')" id="deactivateAssessment" name="deactivateAssessment" title="Deactivate" security permission-key="{{permissionKey}}" permission="delete" space-key-press><i class="fa fa-trash fa-fw" ></i></a>') :
                        ('<a href="javascript:void(0)" data-default-action ng-click="editAssessment(' + value + ')" id="viewAssessment" name="viewAssessment" security permission-key="{{permissionKey}}" permission="read" space-key-press><i title="view" class="fa fa-eye fa-fw border-left margin-left padding-left-small padding-right-small" /></a>');
                }
            }
            ]
        };
    };

    //To get Financial Assessment 
    $scope.getFinancialAssessment = function () {
        var get = $q.defer();
        $scope.isLoading = true;
        return financialAssessmentService.get($scope.contactID, $scope.financialAssessmentID || 0).then(function (data) {
            if (hasData(data)) {
                var sortedData = ($filter('orderBy')(data.DataItems, 'ModifiedOn', true))[0];
                $scope.financialAssessmentID = sortedData.FinancialAssessmentID;
                $scope.financialAssessment = sortedData;
                activeFinancialAssessment = angular.copy($scope.financialAssessment);
                if (action == ACTION.CREATE) {
                    $scope.financialAssessment.AssessmentDate = $filter('toMMDDYYYYDate')(new Date());
                    $scope.financialAssessment.ExpirationDate = $filter('toMMDDYYYYDate')((new Date()).setFullYear((new Date()).getFullYear() + 1));
                }
                else {
                    $scope.financialAssessment.AssessmentDate = $filter('toMMDDYYYYDate')($scope.financialAssessment.AssessmentDate, 'MM/DD/YYYY');
                    $scope.financialAssessment.ExpirationDate = $scope.financialAssessment.ExpirationDate ? $filter('toMMDDYYYYDate')($scope.financialAssessment.ExpirationDate, 'MM/DD/YYYY') : null;
                    $scope.isDisabled = $scope.isDisabled || isHouseholdExpired($scope.financialAssessment.ExpirationDate);
                }

                return financialAssessmentService.getDetails($scope.contactID, $scope.financialAssessmentID).then(function (data) {
                    $scope.assessmentDetailsList = data.DataItems || [];
                    financialsTable.bootstrapTable('load', $scope.assessmentDetailsList);
                    //setting Family Member as SELF in the drop down by default.
                    $scope.assessmentDetailModel = { FinancialAssessmentDetailsID: 0, RelationshipTypeID: $filter('filter')($scope.financialRelationshipLookups, { Name: 'Self' }, true)[0].ID };
               });
            }
            else {
                initializeModels();
            }
        },
         function (errorStatus) {
             $scope.isLoading = false;
             alertService.error('Unable to connect to server');
         }).finally(function () {
             if (!roleSecurityService.hasPermission($scope.permissionKey, PERMISSION.CREATE + '|' + PERMISSION.UPDATE))
                 $scope.cannotAlterDate = true;
             else
                 $scope.cannotAlterDate = $scope.assessmentDetailsList.length > 0;
             resetForms();
             checkFormStatus();
             createWatch();
         });
    };

    var initializeModels = function () {
        //$scope.financialAssessment = { FamilySize: 1, AssessmentDate: new Date(), ExpirationDate: $filter('toMMDDYYYYDate')((new Date(), 'MM/DD/YYYY', 'useLocal').setFullYear((new Date()).getFullYear() + 1)) };
        $scope.financialAssessment = { FamilySize: 1, AssessmentDate: $filter('toMMDDYYYYDate')(new Date()), ExpirationDate: $filter('toMMDDYYYYDate')((new Date()).setFullYear((new Date()).getFullYear() + 1)) };
        //setting Family Member as SELF in the drop down by default.
        $scope.assessmentDetailModel = { FinancialAssessmentDetailsID: 0, RelationshipTypeID: $filter('filter')($scope.financialRelationshipLookups, { Name: 'Self' }, true)[0].ID };
        $scope.assessmentDetailsList = [];
        financialsTable.bootstrapTable('removeAll');
    }
    var saveNewAssessment = function (saveData, promiseArray, dfd) {
        saveData.FinancialAssessmentID = 0;
        promiseArray.push(financialAssessmentService.add(saveData));
        $q.all(promiseArray).then(function (data) {
            var detail = [];
            var indx = data.length - 1;
            var assessID = data[indx].data.ID;
            angular.forEach($scope.assessmentDetailsList, function (item) {
                if (item.FinancialAssessmentDetailsID !== $scope.assessmentDetailModel.FinancialAssessmentDetailsID) {
                    item.FinancialAssessmentID = assessID;
                    item.FinancialAssessmentDetailsID = 0;
                    detail.push(financialAssessmentService.addDetail(item));
                }
            })
            // incase of new assessment insert new details.
            $scope.assessmentDetailModel.FinancialAssessmentDetailsID = 0;
            $scope.assessmentDetailModel.FinancialAssessmentID = 0;

            $scope.financialAssessment.FinancialAssessmentID = data[indx].data.ID;
            $q.all(detail).then(function () {
                dfd.resolve(data[indx]);
            })
        });
    }


    $scope.saveAssessment = function () {
        var savedfd = $q.defer();
        var expired;
        var promise = [];
        if (formService.isDirty(headerFormName) || !$scope.financialAssessment.financialAssessmentID) {
            var saveData = {};
            saveData.ContactID = $scope.contactID;
            saveData.AssessmentDate = moment($scope.financialAssessment.AssessmentDate).format('MM/DD/YYYY');
            if ($scope.financialAssessment.ExpirationDate != null)
                saveData.ExpirationDate = moment($scope.financialAssessment.ExpirationDate).format('MM/DD/YYYY');
            saveData.FamilySize = $scope.financialAssessment.FamilySize;
            saveData.FinancialAssessmentID = $scope.financialAssessment.FinancialAssessmentID;
            saveData.ExpirationReasonID = $scope.financialAssessment.ExpirationReasonID;
            saveData.TotalIncome = $scope.financialAssessment.TotalIncome;
            saveData.TotalExpenses = $scope.financialAssessment.TotalExpenses;
            saveData.TotalExtraOrdinaryExpenses = $scope.financialAssessment.TotalExtraOrdinaryExpenses;
            saveData.TotalOther = $scope.financialAssessment.TotalOther;
            saveData.AdjustedGrossIncome = $scope.financialAssessment.AdjustedGrossIncome;

            if (activeFinancialAssessment && isHouseholdExpired(activeFinancialAssessment.ExpirationDate)) {
                expired = true;
            }

            if (action === ACTION.CREATE && activeFinancialAssessment && !expired) {

                bootbox.dialog({
                    message: "Are you sure you want to save?  This will expire the previous Household Income assessment.",
                    buttons: {
                        success: {
                            label: "Yes",
                            className: "btn-success",
                            callback: function () {
                                activeFinancialAssessment.ExpirationDate = $filter('toMMDDYYYYDate')(new Date(moment($scope.financialAssessment.AssessmentDate).add(-1, 'days')), 'MM/DD/YYYY');
                                activeFinancialAssessment.ExpirationReasonID = 2;
                                promise.push(financialAssessmentService.update(activeFinancialAssessment));
                                saveNewAssessment(saveData, promise, savedfd);
                            }
                        },
                        danger: {
                            label: "No",
                            className: "btn-danger"
                        }
                    }
                });


            }

            else if (action === ACTION.CREATE) {
                saveNewAssessment(saveData, promise, savedfd);
            }
            else {
                if (saveData.FinancialAssessmentID !== undefined && saveData.FinancialAssessmentID != 0) {
                    financialAssessmentService.update(saveData).then(function (response) {
                        savedfd.resolve(response);
                    });
                }
                else {
                    financialAssessmentService.add(saveData).then(function (response) {
                        savedfd.resolve(response);;
                    });
                }
            }
        } else {
            savedfd.resolve({ headerNotDirty: true });
        }
        return savedfd.promise;
    }

    $scope.saveAssessmentDetail = function (assessmentID) {
        if (formService.isDirty(detailFormName)) {
            var saveDetailData = {};
            saveDetailData = angular.copy($scope.assessmentDetailModel);
            saveDetailData.FinancialAssessmentID = assessmentID;
            saveDetailData.ContactID = $scope.contactID;
            // Grab the descriptions, too!
            saveDetailData.FinancialCategoryType = $filter('filter')($scope.categoryTypeLookups, { ID: saveDetailData.CategoryTypeID })[0].Name;
            saveDetailData.Frequency = $filter('filter')($scope.financeFrequencyLookups, {
                ID: saveDetailData.FinanceFrequencyID
            })[0].Name;
            saveDetailData.Category = $filter('filter')($scope.categoryLookups, {
                ID: saveDetailData.CategoryID, CategoryTypeID: saveDetailData.CategoryTypeID
            })[0].Name;
            if (saveDetailData.FinancialAssessmentDetailsID !== undefined && saveDetailData.FinancialAssessmentDetailsID != 0) {
                return financialAssessmentService.updateDetail(saveDetailData);
            } else {
                return financialAssessmentService.addDetail(saveDetailData);
            }
        }
        else
            return $scope.promiseNoOp();
    };



    //To ADD and update financial assessment and its details
    $scope.save = function (isNext, mandatory, hasErrors) {
        // if there are errors, the screen is optional, and the user is trying to go to the next screen, don't bother trying to save anything
        // Vipul Singhal - As per Json if user modified the optional screen and click enter then it should display the validation error 
        // and if user don't don any modification then user can move to next screen.
        if (!mandatory && isNext && hasErrors) {
            $scope.next();
        }

        // if both forms are valid and something has changed, try to save it
        var isDirty = formService.isAnyFormDirty();
        if (isDirty && !hasErrors) {



            //These lines is use when calculation logic will be work from UI.
            $scope.calculateFinancialAssessment($scope.assessmentDetailModel, false);
            var successMessage = 'HouseHold Income has been updated successfully.';
            $scope.saveAssessment().then(function (response) {
                if (((response !== undefined) && ('data' in response) && ('ResultCode' in response.data) && (response.data.ResultCode == 0)) || (response.headerNotDirty && $scope.financialAssessment.FinancialAssessmentID)) {
                    resetMainForm();
                    var assessmentID = (($scope.financialAssessment.FinancialAssessmentID === undefined) || ($scope.financialAssessment.FinancialAssessmentID === 0)) ? response.data.ID : $scope.financialAssessment.FinancialAssessmentID;
                    var isNewDetail = ($scope.assessmentDetailModel.FinancialAssessmentDetailsID === undefined) || ($scope.assessmentDetailModel.FinancialAssessmentDetailsID === 0);
                    
                    // make sure we have everything we need in order to save a detail, then save it
                    if ($scope.assessmentDetailModel.RelationshipTypeID > 0 && $scope.assessmentDetailModel.Amount != undefined &&
                        $scope.assessmentDetailModel.CategoryTypeID > 0 && $scope.assessmentDetailModel.CategoryID > 0) {
                        $scope.saveAssessmentDetail(assessmentID).then(function (response) {
                            if (!((response !== undefined) && ('data' in response) && ('ResultCode' in response.data) && (response.data.ResultCode !== 0))) {
                                if ((response !== undefined) && ('data' in response)) {
                                    $scope.assessmentDetailModel = {};
                                    successMessage = 'HouseHold Income Detail has been ' + (isNewDetail ? 'added' : 'updated') + ' successfully.';
                                }
                                alertService.success(successMessage);
                                performPostSave(isNext);
                            } else
                                $scope.errorCallback();
                        },
                        $scope.errorCallback
                        );
                    } else {
                        performPostSave(isNext);
                        alertService.success(successMessage);
                    }
                }
            },
                $scope.errorCallback,
                $scope.notificationCallback);
        }
    };
    var performPostSave = function (isNext) {        
        if (action === ACTION.CREATE && !($state.current.name === 'registration.financial' || $state.current.name === 'eciregistration.financial')) {
            $state.go('patientprofile.benefits.financial.financialdetails', {
                FinancialAssessmentID: $scope.financialAssessment.FinancialAssessmentID, ReadOnly: 'edit'
            });
        } else {
            $scope.postSave(isNext);
        }
    }

    $scope.errorCallback = function (errorStatus) {
        alertService.error('OOPS! Something went wrong');
    };

    $scope.notificationCallback = function (notification) {
        alertService.warning(notification);
    };

    $scope.postSave = function (isNext) {
        $scope.getFinancialAssessment().then(function () {
            $scope.newAssessmentDetails();
            if (isNext)
                $scope.next();
        });
    };

    $scope.saveNext = function () {
        $scope.save(true);
    }

    //To edit the income and expense data
    $scope.editAssessment = function (financialAssessmentDetailsID, index) {
        $scope.selectedID = financialAssessmentDetailsID;

        angular.forEach($scope.assessmentDetailsList, function (financialAssessmentItems) {
            if (financialAssessmentItems.FinancialAssessmentDetailsID == financialAssessmentDetailsID) {
                $scope.assessmentDetailModel = angular.copy(financialAssessmentItems);
                $scope.assessmentDetailModel.Amount = parseFloat($scope.assessmentDetailModel.Amount).toFixed(2);
                $scope.$parent['autoFocus'] = false;
                $scope.$parent['autoFocusMember'] = true;
                if (action !== ACTION.CREATE) {
                    resetDetailForm();
                }
                return;
            }
        });
    };

    //To add new assessemnt for contact id, it will clean the screen.
    $scope.addNewAssessment = function () {
        $scope.financialAssessment = {};
        $scope.selectedID = null;
        $scope.newAssessmentDetails();
    };

    //To add new assessemnt details for contact id, it will clean controls for add new financial assessment details(income/expense)
    $scope.newAssessmentDetails = function () {
        $scope.assessmentDetailModel = {
            FinancialAssessmentDetailsID: 0, RelationshipTypeID: $filter('filter')($scope.financialRelationshipLookups, {
                Name: 'Self'
            }, true)[0].ID
        };
        resetForms();
        $scope.$parent['autoFocus'] = false;
        $scope.$parent['autoFocusMember'] = true;
    };

    //To calculate the total income, total expenses and adjusted amount
    $scope.calculateFinancialAssessment = function (tempFinancialAssessmentDetail, isDelete) {
        var grossIncome = 0;
        var grossExpense = 0;
        var grossExtraordinaryExpenses = 0;
        var grossOther = 0;
        var detailsCopy = angular.copy($scope.assessmentDetailsList);
        //check if adding new
        if ((tempFinancialAssessmentDetail !== undefined) && (tempFinancialAssessmentDetail.FinancialAssessmentDetailsID === 0) && tempFinancialAssessmentDetail.Amount) {
            detailsCopy.push(tempFinancialAssessmentDetail);
        }
        //loop through assessment details
        angular.forEach(detailsCopy, function (financialAssessmentDetail) {
            //row being edited
            if ((tempFinancialAssessmentDetail != null) && (tempFinancialAssessmentDetail.FinancialAssessmentDetailsID === financialAssessmentDetail.FinancialAssessmentDetailsID)) {
                financialAssessmentDetail = isDelete ? { Amount: 0, FinanceFrequencyID: 0, CategoryTypeID: 0 } : tempFinancialAssessmentDetail;
            }
            var financeFrequency = $filter('filter')($scope.financeFrequencyLookups, { ID: financialAssessmentDetail.FinanceFrequencyID })[0];
            var categoryType = $filter('filter')($scope.categoryTypeLookups, { ID: financialAssessmentDetail.CategoryTypeID })[0];
            if ((financeFrequency !== undefined) && (categoryType !== undefined)) {
                //To calculate Income
                if (categoryType.Name === $scope.income) {
                    grossIncome += financialAssessmentDetail.Amount * financeFrequency.FrequencyFactor;
                }
                    //To calculate expenses
                else if (categoryType.Name === $scope.expense) {
                    grossExpense += financialAssessmentDetail.Amount * financeFrequency.FrequencyFactor;
                }
                    //To calculate Extraordinary Expenses
                else if (categoryType.Name === $scope.extraordinaryExpenses) {
                    grossExtraordinaryExpenses += financialAssessmentDetail.Amount * financeFrequency.FrequencyFactor;
                }
                    //To calculate Extraordinary Expenses
                else if (categoryType.Name === $scope.Other) {
                    grossOther += financialAssessmentDetail.Amount * financeFrequency.FrequencyFactor;
                }
            }
        });

        $scope.financialAssessment.TotalIncome = grossIncome || 0;
        $scope.financialAssessment.TotalExpenses = grossExpense || 0;
        $scope.financialAssessment.TotalExtraOrdinaryExpenses = grossExtraordinaryExpenses || 0;
        $scope.financialAssessment.TotalOther = grossOther || 0;
        $scope.financialAssessment.AdjustedGrossIncome = grossIncome - (grossExpense + grossExtraordinaryExpenses) || 0;
    };

    //On click on next button redirect user to next screen
    $scope.next = function () {
        // TODO : Change to selfpay after selfpay implementation 
        //$state.go('registration.consent', { ContactID: $scope.contactID });
    };

    $scope.remove = function (id) {
        var assessmentID = $scope.financialAssessment.FinancialAssessmentID;
        bootbox.confirm("Selected Household Income will be deactivated. Do you want to continue?", function (result) {
            if (result === true) {
                var tempDetail = $filter('filter')($scope.assessmentDetailsList, {
                    FinancialAssessmentDetailsID: id
                })[0];
                $scope.calculateFinancialAssessment(tempDetail, true);

                if (action === ACTION.CREATE) {
                    $scope.assessmentDetailsList = $filter('filter')($scope.assessmentDetailsList, function (item) {
                        return item.FinancialAssessmentDetailsID !== id
                    });
                    financialsTable.bootstrapTable('load', $scope.assessmentDetailsList);
                    alertService.success('Household Income detail has been deactivated.');
                    $rootScope.setform(true, headerFormName);
                    $scope.isDetailRequired = false;
                }

                else {
                    financialAssessmentService.removeDetail($scope.contactID, assessmentID, id).then(function (response) {
                        var data = response.data;
                        if (data.ResultCode === 0) {
                            alertService.success('Household Income detail has been deactivated.');
                            formService.initForm(true, $scope.ctrl.financialAssessmentForm.$name);
                            $scope.saveAssessment().then(function () {
                                $scope.getFinancialAssessment();
                            });
                        } else {
                            alertService.error('OOPS! Something went wrong');
                            $scope.calculateFinancialAssessment();
                        }
                    },
                        function (errorStatus) {
                            alertService.error('OOPS! Something went wrong');
                            $scope.calculateFinancialAssessment();
                        },
                        function (notification) {
                            alertService.warning(notification);
                        }
                    ).finally(function () {
                    });
                    $scope.$apply();
                }

            }
        });
    };

    $scope.init = function () {
        $scope.contactID = $stateParams.ContactID;
        $scope.financialAssessmentID = eval($stateParams.FinancialAssessmentID || 0);
        $scope.$parent['autoFocus'] = true; //for Focus
        $scope.$parent['autoFocusMember'] = false;
        //TODO: These four need to be IDs, not strings
        $scope.income = "Income";
        $scope.expense = "Expense";
        $scope.extraordinaryExpenses = "Extraordinary Expenses";
        $scope.Other = "Other";
        $scope.dateOptions = { // used by datepickers
            formatYear: 'yy',
            startingDay: 1,
            showWeeks: false
        };
        $scope.financialAssessment = {
            AssessmentDate: null, ExpirationDate: null
        }; // main financial assessment model
        $scope.assessmentDetailModel = {
        }; // financial assessment detail model --> Relationship default to SELF....
        $scope.assessmentDetailsList = []; // collection of financial assessment details
        $scope.cannotAlterDate = false;
        $scope.categoryTypeLookups = financialAssessmentService.getLookups('CategoryType');
        $scope.financeFrequencyLookups = financialAssessmentService.getLookups("FinanceFrequency");
        $scope.categoryLookups = financialAssessmentService.getLookups('Category');

        var relationshipLookups = financialAssessmentService.getLookups('RelationshipType');
        $scope.financialRelationshipLookups = $filter('filter')(relationshipLookups, {
            RelationshipGroupID: financialAssessmentGroupID
        });

        $scope.getFinancialAssessment();

        $scope.initializeBootstrapTable();

        if ($state.current.name.toLowerCase().indexOf('patientprofile') >= 0) {
            $scope.controlsVisible = false;
            $scope.enterKeyStop = true;
            $scope.stopNext = false;
            $scope.saveOnEnter = true;
        }
        else {
            $scope.controlsVisible = true;
            $scope.enterKeyStop = false;
            $scope.stopNext = false;
            $scope.saveOnEnter = false;

        }
        if ($stateParams.ReadOnly == "view") {
            $scope.isDisabled = true;
        }
        $scope.selectedID = null;
    };
    var createWatch = function () {
        $scope.$watch('[' + headerFormName + ', ' + detailFormName + ']', function (newValue, oldValue) {
            if (oldValue[0] !== newValue[0] || oldValue[1] !== newValue[1]) {
                if (!$scope.assessmentDetailsList || ($scope.assessmentDetailsList && $scope.assessmentDetailsList.length === 0)) {
                    $scope.isDetailRequired = true;
                    return;
                }
                if (formService.isDirty(detailFormName)) {
                    $scope.isDetailRequired = true;
                } else if (formService.isDirty(headerFormName)) {
                    $scope.isDetailRequired = false;
                }
                else {
                    $scope.isDetailRequired = true;
                }
            }
        }, true);
    }

    $scope.init();
}]);
