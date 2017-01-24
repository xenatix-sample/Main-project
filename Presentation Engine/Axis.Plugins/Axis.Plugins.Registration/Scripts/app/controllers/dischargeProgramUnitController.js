angular.module('xenatixApp')
    .controller('dischargeProgramUnitController', ['$http', '$scope', "$stateParams", "$state", '$q', '$injector', '$filter', '$timeout', 'formService', 'alertService', '$rootScope', 'dischargeService', 'admissionService', 'lookupService', 'DocumentTypeID', '$controller', 'eSignatureService', 'roleSecurityService',
function ($http, $scope, $stateParams, $state, $q, $injector, $filter, $timeout, formService, alertService, $rootScope, dischargeService, admissionService, lookupService, DocumentTypeID, $controller, eSignatureService, roleSecurityService) {

    $scope.isSignatureExist = true;
    angular.extend($controller('signatureController', { $scope: $scope, DocumentTypeID: DocumentTypeID }));
    $scope.startdate = $scope.enddate = new Date();
    $scope.admissionStatus = [{ ID: 1, Name: "Active" }, { ID: 0, Name: "Inactive" }];
    $scope.IsProgramUnitActiveForCompany = false;
    var setDefaultDatePickerSettings = function () {
        $scope.opened = false;
        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1,
            showWeeks: false
        };
        $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'MM/dd/yyyy'];
        $scope.format = $scope.formats[3];
    };
    var dischargeReason = {
        Deceased_Homicide: 4,
        Deceased_NaturalCauses: 5,
        Deceased_Suicide: 6,
        Deceased_UnknownCause: 7
    }
    $scope.isDischargePUDisabled = true;
    $scope.dischargeProgramUnitID = undefined;
    $scope.hasSignatureFirst = false;
    $scope.IsDeceased = false;

    var getSignature = function () {
        eSignatureService.getDocumentSignatures(DocumentTypeID, $scope.dischargeProgramUnit.ContactAdmissionID).then(function (response) {
            if (response && response.DataItems && response.DataItems.length > 0) {
                var userCredential = $filter('filter')($scope.userCredentials, { CredentialID: response.DataItems[0].CredentialID }, true);
                if (userCredential.length == 0) {
                    $scope.userCredentials = $filter('filter')(lookupService.getLookupsByType('Credential'), { CredentialID: response.DataItems[0].CredentialID }, true);
                }
                $scope.signatureFirst = setSignatureData(response.DataItems[0]);
            }
        });
    };

    var setSignatureData = function (model) {
        var modelResponse = {};
        modelResponse.DateSigned = $filter('toMMDDYYYYDate')(model.ModifiedOn, 'MM/DD/YYYY', 'useLocal');
        modelResponse.UserFullName = lookupService.getText("Users", model.EntityId);
        modelResponse.CredentialID = model.CredentialID;
        return modelResponse;
    };

    var resetDigitalSignature = function () {
        $scope.isCredentialReadonly = false;
        $scope.isPasswordReadonly = false;
        $scope.isSigned = false;
        $scope.isInactive = false;
        $scope.isPasswordHide = false;
        $scope.signature.Password = undefined;
        $scope.signature.DateSigned = null;
        $scope.signature.DateSigned = undefined;
        $scope.signatureVerified = false;
    }

    $scope.newdischargeProgramUnit = function () {
        $scope.dischargeProgramUnit = { ContactID: $stateParams.ContactID, IsDischarged: 1 };
    };


    $scope.save = function (isNext, mandatory, hasErrors, keepForm) {
        $scope.hasSignatureFirst = false;
        var isDirty = formService.isDirty();
        if (isDirty && !hasErrors) {
            var deferred = $q.defer();
            var isUpdate = $scope.dischargeProgramUnit.ContactDischargeNoteID != undefined && $scope.dischargeProgramUnit.ContactDischargeNoteID !== 0;
            if (!$scope.dischargeProgramUnit.IsDischarged
                    && checkModel($scope.IsProgramUnitInactive)
                    && !$scope.IsProgramUnitInactive) {
                if (angular.element('#contactDischargeNoteModel').is(':visible')) {

                    $scope.saveContactDischargeNote();
                }
                else {

                    if ($scope.dischargeProgramUnit.IsDischarged == 0) {

                        if ($scope.dischargeProgramUnit.ContactDischargeNoteID) {
                            dischargeService.getDischargeNote($scope.dischargeProgramUnit.ContactDischargeNoteID).then(function (data) {
                                if (data.DataItems && data.DataItems != undefined) {
                                    $scope.dischargeNote = data.DataItems[0];
                                    $scope.dischargeNote.DischargeDate = $filter('toMMDDYYYYDate')($scope.dischargeNote.DischargeDate, 'MM/DD/YYYY');
                                    resetDigitalSignature();
                                    getSignature();
                                    $scope.DocumentID = $scope.dischargeNote.ContactAdmissionID;
                                }
                                else
                                    initDischargeNote();
                            });
                        }
                        else
                            $scope.IsDeceased = false;
                        initDischargeNote();
                    }
                    angular.element("#contactDischargeNoteModel").modal('show');

                }

            }
        }
        else if (!isDirty && isNext) {
            $scope.next();
        }
    };

    $scope.addDischargeProgramUnit = function (isUpdate) {
        //$scope.dischargeNote.DischargeDate = $filter('toMMDDYYYYDate')($scope.dischargeNote.DischargeDate, 'MM/DD/YYYY hh:mm A');
        var datePart = $filter('formatDate')($scope.dischargeNote.DischargeDate);
        var currentTime = $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal');
        var dateTime = $filter('toMMDDYYYYDate')(datePart + ' ' + currentTime, 'MM/DD/YYYY HH:mm')
        $scope.dischargeNote.DischargeDate = datePart;
        $scope.dischargeNote.IsDeceased = $scope.IsDeceased;
        return dischargeService.addContactDischargeNote($scope.dischargeNote);
    };

    $scope.get = function () {
        return admissionService.get($stateParams.ContactID).then(function (data) {
            if (data.DataItems && data.DataItems != undefined) {
                $scope.admissionList = data.DataItems;
                var programUnitList = $filter('filter')(data.DataItems, {
                    DataKey: "ProgramUnit"
                }, false);

                var admissionListOrderByActive = $filter('orderBy')(programUnitList, 'IsDischarged', false);
                $scope.dischargeProgramUnitTable.bootstrapTable('load', admissionListOrderByActive);
            } else {
                $scope.admissionList = [];
                $scope.dischargeProgramUnitTable.bootstrapTable('removeAll');
            }
            $scope.dischargeProgramUnit.ContactAdmissionID = 0;
            applyDropupOnGrid(true);
        });
    };

    //Initialize the Discharge Note
    var initDischargeNote = function () {
        //Reset the Form
        resetContactDischargeNote();

        //Initialize the object
        $scope.dischargeNote = {
            ContactDischargeNoteID: 0,
            ContactID: $stateParams.ContactID,
            ContactAdmissionID: $scope.dischargeProgramUnit.ContactAdmissionID,
            ContactAdmissionDate: $filter('toMMDDYYYYDate')($scope.dischargeProgramUnit.EffectiveDate, 'MM/DD/YYYY', 'useLocal'),
            DischargeDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
            NoteText: '',
            DischargeReasonID: null,
            NoteTypeID: $filter('filter')($rootScope.getLookupsByType(LOOKUPTYPE.ReferenceNoteType), { ID: NoteType.DischargeNote }, false)[0].ID,
            SignatureStatusID: 2,            //TODO: get this from enum. signed-2, not signed-1
        };

        $scope.DocumentID = $scope.dischargeProgramUnit.ContactAdmissionID;
        //getSignature();
        reInititializeSignature();
    };

    //Cancel
    $scope.cancel = function () {
        angular.element('#contactDischargeNoteModel').modal('hide');
    };

    //Save the Contact Discharge note
    $scope.saveContactDischargeNote = function () {
        //Validate the errors
        iterateErrorFields($scope.dischargeNoteForm.$error.required, null, 'required', alertService);
        iterateErrorFields($scope.dischargeNoteForm.$error.date, null, 'date', alertService);
        iterateErrorFields($scope.dischargeNoteForm.$error.lessThanDate, null, 'lessThanDate', alertService);
        iterateErrorFields($scope.dischargeNoteForm.$error.futureDate, null, 'futureDate', alertService);
        var isUpdate = $scope.dischargeNote.ContactDischargeNoteID != undefined && $scope.dischargeNote.ContactDischargeNoteID !== 0;
        if (formService.isDirty() && ($scope.dischargeNoteForm.$valid == true) && $scope.signatureVerified) { // BUGFIX-13081 
            bootbox.confirm("Are you sure you would like to continue with Discharging this contact at Program Unit? Any future appointments for this specific Program Unit  will be cancelled.", function (result) {
                if (result === true) {
                    saveDischargeDischargeProgramUnit(isUpdate);
                }
            });
        }
    };

    var saveDischargeDischargeProgramUnit = function (isUpdate) {
        //call signature save
        var deferred = $q.defer();
        $scope.saveDSignature().then(function (data) {
            $scope.addDischargeProgramUnit(isUpdate).then(function (response) {
                $scope.get().finally(function () {
                    $scope.cancel();
                    $scope.dischargeProgramUnit = {
                        IsDischarged: 1
                    };
                    postSave(response, false);
                    resetDigitalSignature();
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
        },
        function (signatureError) {
            //Error in saving signature, no need to display message as it has been taken care by directive
            //alertService.error('OOPS! Something went wrong');       
            deferred.reject(signatureError);
        });

        return deferred.promise;
    }

    var resetContactDischargeNote = function () {
        if ($scope.dischargeNoteForm) {
            $rootScope.formReset($scope.dischargeNoteForm, $scope.dischargeNoteForm.$name);
        }
    };

    var resetDischargeProgramUnit = function () {
        if ($scope.ctrl.dischargeProgramUnitForm) {
            $rootScope.formReset($scope.ctrl.dischargeProgramUnitForm, $scope.ctrl.dischargeProgramUnitForm.$name);
        }
    };

    var postSave = function (response, isNext) {
        var data = response.data;
        if (data.ResultCode !== 0) {
            alertService.error(data.ResultMessage);
        } else {
            alertService.success('Discharged from the Program Unit.');
            //Null out the ProgramUnit inactive field
            $scope.IsProgramUnitInactive = null;
            resetDischargeProgramUnit();
            resetContactDischargeNote();
            $scope.dischargeProgramUnit.ContactDischargeNoteID =
                (($scope.dischargeProgramUnit !== undefined) && ($scope.dischargeProgramUnit.ContactDischargeNoteID !== undefined) && ($scope.dischargeProgramUnit.ContactDischargeNoteID != 0))
                ? $scope.dischargeProgramUnit.ContactDischargeNoteID
                : response.ID;
            $scope.isDischargePUDisabled = true;
            $scope.dischargeProgramUnitID = undefined;
            if (isNext) {
                $scope.next();
            }
            else {
                $scope.init();
            }
        }
    };

    var getOrganizationText = function (type, id) {
        if (id != null && id != undefined)
            return $filter('filter')($rootScope.getOrganizationByDataKey(type), { ID: id }, true)[0].Name;
    };

    $scope.editDischargeProgramUnit = function (contactAdmissionId, event) {
        //From server, 0 = Inactive and 1 = Active
        $scope.dischargeProgramUnitID = undefined;
        $timeout(function () {
            $scope.userCredentials = $scope.allCredentials;
            $scope.dischargeProgramUnit = angular.copy($filter('filter')($scope.admissionList, { ContactAdmissionID: contactAdmissionId }, true)[0]);
            $scope.dischargeProgramUnitID = 0;
            $scope.dischargeProgramUnit.IsDischarged = !($scope.dischargeProgramUnit.IsDischarged) ? 1 : 0;
            if (roleSecurityService.hasPermission('General-General-ProgramUnitDischarge', PERMISSION.CREATE)) {
                $scope.IsProgramUnitInactive = $scope.dischargeProgramUnit.IsDischarged == 0 ? true : false;
                $scope.isDischargePUDisabled = $scope.IsProgramUnitInactive ? true : undefined;
                if ($scope.IsProgramUnitInactive)
                    $scope.dischargeProgramUnitID = undefined;
            }
            //Re-initilalize the Digital signature
            reInititializeSignature();
            resetContactDischargeNote();
            //Re-initialize discharge Program Unit
            resetDischargeProgramUnit();
            $scope.IsDeceased = false;
        });

    };

    $scope.viewDischargeProgramUnit = function (contactDischargeNoteId) {
        $scope.hasSignatureFirst = true;
        $scope.IsDeceased = false;
        angular.element("#contactDischargeNoteModel").modal('show');
        dischargeService.getDischargeNote(contactDischargeNoteId).then(function (data) {
            if (data.DataItems && data.DataItems != undefined) {
                $scope.dischargeNote = data.DataItems[0];
                $scope.dischargeNote.DischargeDate = $filter('toMMDDYYYYDate')($scope.dischargeNote.DischargeDate, 'MM/DD/YYYY');
                $scope.dischargeNote.DeceasedDate = $filter('toMMDDYYYYDate')($scope.dischargeNote.DeceasedDate, 'MM/DD/YYYY');
                getSignature();
                $scope.isInactive = true;
                $scope.isSigned = true;
                $scope.isPasswordHide = true;
                $scope.isPasswordReadonly = true;
                $scope.isCredentialReadonly = true;
                $scope.IsDeceased = $scope.dischargeNote.IsDeceased;
                resetDischargeProgramUnit();
                resetContactDischargeNote();
            }
        });
    };

    var reInititializeSignature = function () {
        resetDigitalSignature();
        if ($scope.userCredentials && $scope.userCredentials.length == 1) {
            $scope.signature.CredentialID = $scope.userCredentials[0].CredentialID;
            $scope.checkPermission();
        }
        else {
            $scope.signature.CredentialID = null;
            $scope.hasPermission = false;
        }
    }

    $scope.dischargeReasonChange = function (reason) {
        if ($scope.dischargeNote.DischargeReasonID == dischargeReason.Deceased_Homicide ||
            $scope.dischargeNote.DischargeReasonID == dischargeReason.Deceased_NaturalCauses ||
            $scope.dischargeNote.DischargeReasonID == dischargeReason.Deceased_Suicide ||
            $scope.dischargeNote.DischargeReasonID == dischargeReason.Deceased_UnknownCause) {
            $scope.IsDeceased = true;
            if (!$scope.dischargeNote.DeceasedDate) {
                var registrationService = $injector.get('registrationService');
                if (registrationService) {
                    registrationService.get($stateParams.ContactID).then(function (data) {
                        if (hasData(data))
                            $scope.dischargeNote.DeceasedDate = data.DataItems[0].DeceasedDate ? $filter('toMMDDYYYYDate')(data.DataItems[0].DeceasedDate) : '';
                    })
                }
            }
        }
        else {
            $scope.IsDeceased = false;
            $scope.dischargeNote.DeceasedDate = null;
        }
    }

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
                    field: "DivisionID",
                    title: "Division",
                    formatter: function (value, row, index) {
                        return getOrganizationText('Division', value);
                    }
                },
                {
                    field: "ProgramID",
                    title: "Program",
                    sortable: true,
                    sortName: 'program',
                    formatter: function (value, row, index) {

                        if (value && row.DivisionID) {
                            row.program = getOrganizationText('Program', value);
                            return row.program;
                        } else {
                            row.program = '';
                            return row.program;
                        }

                    }
                },
                {
                    field: "ProgramUnitID",
                    title: "Program Unit",
                    sortable: true,
                    sortName: 'programUnit',
                    formatter: function (value, row, index) {

                        if (value && row.DivisionID && row.ProgramID) {
                            row.programUnit = getOrganizationText('ProgramUnit', value);
                            return row.programUnit;
                        } else {
                            row.programUnit = '';
                            return row.programUnit;
                        }

                    }
                },
                {
                    field: "IsDischarged",
                    title: "Program Unit Status",
                    formatter: function (value, row, index) {
                        if (row["IsDischarged"]) {
                            return "Inactive";
                        }
                        else {
                            return "Active";
                        }
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
                        if (value && row["IsDischarged"]) {
                            return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY');
                        } else
                            return '';
                    }
                },
                {
                    field: "SignedByEntityName",
                    title: "Signed By",
                    formatter: function (value, row, index) {
                        if (row.SignedByEntityName) {
                            return row.SignedByEntityName;
                        } else
                            return '';
                    }
                },
                {
                    field: "DateSigned",
                    title: "Date Signed",
                    formatter: function (value, row, index) {
                        if (value && row["DateSigned"]) {
                            return moment(new Date(value)).format('MM/DD/YYYY');
                        } else
                            return '';
                    }
                },
                {
                    field: "ContactAdmissionID",
                    title: "Actions",
                    formatter: function (value, row, index) {
                        var isActive = !(row["IsDischarged"]);
                        return '<span class="text-nowrap">' +
                        '<div style="display:none;"><a href="javascript:void(0)"  data-default-action ng-click="editDischargeProgramUnit(' + value + ', $event)" title="Edit" security permission-key="General-General-ProgramUnitDischarge" permission="create" space-key-press><i class="fa fa-pencil fa-fw" /></a></div>' +
                        (isActive ? '<a href="javascript:void(0)" data-default-no-action ng-click="editDischargeProgramUnit(' + value + ', $event)" title="Deactivate" security permission-key="General-General-ProgramUnitDischarge" permission="create" ><i class="fa fa-minus-circle fa-fw"></i></a>' : '') +
                            (!isActive ? ' <a data-default-no-action ng-click="viewDischargeProgramUnit(' + row["ContactDischargeNoteID"] + ')" alt="view" security permission-key="General-General-ProgramUnitDischarge" permission="read" space-key-press>'
                            + '<i security permission-key="General-General-ProgramUnitDischarge" permission="read" title="View Progress Note" class="fa fa-fw fa-file-text" /></a>' : '') +
                                        '</span>';
                    }
                }
            ]
        };
    };

    $scope.init = function () {
        setDefaultDatePickerSettings();
        $scope.$parent['autoFocus'] = true;
        $scope.dischargeProgramUnit = {
            IsDischarged: 1
        };
        $scope.dischargeProgramUnitTable = angular.element("#dischargeProgramUnitTable");
        $scope.initializeBootstrapTable();
        $scope.get();

    }
    $scope.init();
}]);

