angular.module('xenatixApp')
    .controller('dischargeCompanyController', ['$http', '$scope', "$stateParams", "$state", '$q', '$filter', '$rootScope', 'formService', 'alertService', 'dischargeService', 'admissionService', 'lookupService','roleSecurityService',
function ($http, $scope, $stateParams, $state, $q, $filter, $rootScope, formService, alertService, dischargeService, admissionService, lookupService, roleSecurityService) {

    $scope.admissionStatus = [{ ID: 1, Name: "Active" }, { ID: 0, Name: "Inactive" }];
    $scope.get = function () {
        return admissionService.get($stateParams.ContactID).then(function (data) {
            if (data.DataItems && data.DataItems != undefined && data.DataItems.length > 0) {
                $scope.admissionList = data.DataItems;
                var admissionCompanyList = $filter('filter')(data.DataItems, { DataKey: "Company" }, false);
                var admissionCompanyListOrderByActive = $filter('orderBy')(admissionCompanyList, 'IsDischarged', false);
                $scope.dischargeCompanyTable.bootstrapTable('load', admissionCompanyListOrderByActive);
            } else {
                $scope.admissionList = [];
                $scope.dischargeCompanyTable.bootstrapTable('removeAll');
            }
            $scope.dischargeCompany.ContactAdmissionID = 0;
            applyDropupOnGrid(true);
            resetForm();
        });

    };

    $scope.newDischargeCompany = function () {
        $scope.dischargeCompany = { IsDischarged: 1 };
    };

    var getOrganizationText = function (type, id) {
        return $filter('filter')($rootScope.getOrganizationByDataKey(type), {ID: id }, true)[0].Name;
    }

    $scope.save = function (isNext, mandatory, hasErrors, keepForm) {
        var isDirty = formService.isDirty();
        if (isDirty && !hasErrors) {
            var deferred = $q.defer();
            var isUpdate = $scope.dischargeNote.ContactDischargeNoteID != undefined && $scope.dischargeNote.ContactDischargeNoteID !== 0;

            var isNotDischaredFromProgramUnits = $filter('filter')($scope.admissionList, { DataKey: "ProgramUnit", IsDischarged: "false", CompanyID: $scope.dischargeCompany.CompanyID }, false).length > 0;
            if (isNotDischaredFromProgramUnits) {
                alertService.error("Unable to discharge from company.  Contact is admitted at Program Unit level.");
            }
            else {
                if (!$scope.dischargeCompany.IsDischarged && !$scope.IsCompanyInactive) {

                    bootbox.confirm("Are you sure you would like to continue with Discharging this contact at Company Level? Any future appointment for this specific company level will be cancelled.", function (result) {
                        if (result === true) {
                            $scope.addDischargeCompany(isUpdate).then(function (response) {
                                $scope.get().finally(function () {
                                    postSave(response, isNext);
                                    deferred.resolve(response);
                                });
                            },
                             function (errorStatus) {
                                 alertService.error('OOPS! Something went wrong');
                                 deferred.reject();
                             },
                             function (notification) {
                                 alertService.warning(notification);
                             });
                            return deferred.promise;
                        }
                    });
                }
                else if (!isDirty && isNext) {
                    $scope.next();
                }
            }
        }
    };

    $scope.addDischargeCompany = function (isUpdate) {

        $scope.dischargeNote.DischargeDate = $filter("formatDate")($scope.dischargeNote.DischargeDate);
        if (!isUpdate) {
            return dischargeService.addContactDischargeNote($scope.dischargeNote);
        }
        else {
            return dischargeService.updateContactDischargeNote($scope.dischargeNote);
        }
        return
    };

    $scope.editDischargeComapany = function (contactAdmissionId, event) {
        event.stopPropagation();
        var admissionList = angular.copy($scope.admissionList);
        $scope.dischargeCompany = $filter('filter')(admissionList, { ContactAdmissionID: contactAdmissionId }, true)[0];
        $scope.dischargeCompanyID = 0;
        $scope.dischargeNote.DischargeReasonID = 1; //default for admisitration
        $scope.dischargeNote.SignatureStatusID = 1;
        $scope.dischargeNote.ContactAdmissionID = $scope.dischargeCompany.ContactAdmissionID;
        $scope.dischargeCompany.IsDischarged = !($scope.dischargeCompany.IsDischarged) ? 1 : 0;
        if (roleSecurityService.hasPermission('General-General-CompanyDischarge', PERMISSION.CREATE)) {
            $scope.IsCompanyInactive = $scope.dischargeCompany.IsDischarged == 0;
            $scope.isDischargeCompanyDisabled = $scope.IsCompanyInactive ? true : undefined;
            if ($scope.IsCompanyInactive)
                $scope.dischargeCompanyID = undefined;
        }
        resetForm();
    };


    var postSave = function (response, isNext) {
        var data = response.data;
        if (data.ResultCode !== 0) {
            alertService.error(data.ResultMessage);
        } else {
            alertService.success('Discharged from the company.');
            $scope.dischargeNote.ContactDischargeNoteID =
                (($scope.dischargeNote !== undefined) && ($scope.dischargeNote.ContactDischargeNoteID !== undefined) && ($scope.dischargeNote.ContactDischargeNoteID != 0))
                ? $scope.dischargeNote.ContactDischargeNoteID
                : response.ID;
            $scope.isDischargeCompanyDisabled = true;
            $scope.dischargeCompanyID = undefined;
            if (isNext) {
                $scope.next();
            }
            else {
                $scope.init();
            }
        }
    };

    $scope.initializeBootstrapTable = function () {
        $scope.tableoptions = {
            pagination: true,
            pageSize: 10,
            pageList: [10, 25, 50, 100],
            search: false,
            showColumns: true,
            data: [],
            undefinedText: '',
            onClickRow: function (e, row, $element) {
                //$scope.prepRowEditData(e);
            },
            columns: [
                {
                    field: "CompanyID",
                    title: "Company",
                    formatter: function (value, row, index) {
                        return getOrganizationText('Company', value);
                    }
                },
                {
                    field: "IsDischarged",
                    title: "Company Status",
                    formatter: function (value, row, index) {
                        if (value)
                            return "Inactive";
                        else
                            return "Active";

                    }
                },
                {
                    field: "IsProgramUnitActiveForCompany",
                    title: "Program Unit Status",
                    formatter: function (value, row, index) {
                        if (value)
                            return "Active";
                        else
                            return "Inactive";

                    }
                },
                {
                    field: "EffectiveDate",
                    title: "Admission Date",
                    formatter: function (value, row, index) {
                        if (value) {
                            var formattedDate = $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY', 'useLocal');
                            return formattedDate;
                        } else
                            return '';
                    }
                },
                {
                    field: "DischargeDate",
                    title: "Discharge Date",
                    formatter: function (value, row, index) {
                        if (value) {
                            var formattedDate = $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY');
                            return formattedDate;
                        } else
                            return '';
                    }
                },
                {
                    field: "ContactAdmissionID",
                    title: "",
                    formatter: function (value, row, index) {
                        var isActive = !(row["IsDischarged"]);
                        return '<span class="text-nowrap">' +
                                 '<div style="display:none;"><a href="javascript:void(0)" data-default-action ng-click="editDischargeComapany(' + value + ', $event)" title="Edit" security permission-key="General-General-CompanyDischarge" permission="create" space-key-press><i class="fa fa-pencil fa-fw" /></a></div>' +
                             (isActive ? '<a href="javascript:void(0)" data-default-no-action ng-click="editDischargeComapany(' + value + ', $event)" title="Deactivate" security permission-key="General-General-CompanyDischarge" permission="create" ><i class="fa fa-minus-circle fa-fw"></i></a>' : '') +
                                 '</span>';
                    }
                }
            ]
        };
    };

    //Initialize the Discharge Note
    var initDischargeNote = function () {
        $scope.dischargeNote = {
            ContactDischargeNoteID: 0,
            ContactID: $stateParams.ContactID,
            ContactAdmissionID: $scope.dischargeCompany.ContactAdmissionID,
            DischargeDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY'),
            NoteText: 'Discharged from Company',
            DischargeReasonID: 1,
            NoteTypeID: $filter('filter')($rootScope.getLookupsByType(LOOKUPTYPE.ReferenceNoteType), { ID: NoteType.DischargeNote }, false)[0].ID
        };
    };

    function resetForm() {
        if ($scope.ctrl.dischargeCompanyForm) {
            $rootScope.formReset($scope.ctrl.dischargeCompanyForm, $scope.ctrl.dischargeCompanyForm.$name);
        }
    }

    $scope.init = function () {
        $scope.$parent['autoFocus'] = true;
        $scope.dischargeCompany = { IsDischarged: 1 };
        initDischargeNote();
        $scope.dischargeCompanyTable = angular.element("#dischargeCompanyTable");
        $scope.initializeBootstrapTable();
        $scope.isDischargeCompanyDisabled = true;
        $scope.dischargeCompanyID = undefined;
        $scope.get();
    }
    $scope.init();


}]);

