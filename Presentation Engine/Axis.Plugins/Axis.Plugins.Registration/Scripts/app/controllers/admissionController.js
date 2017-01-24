(function () {

    angular.module('xenatixApp')
    .controller('admissionController', ['$scope', '$filter', 'admissionService', 'alertService', 'formService', '$rootScope', '$stateParams', '$state', 'navigationService', 'dateTimeValidatorService', 'roleSecurityService',
    function ($scope, $filter, admissionService, alertService, formService, $rootScope, $stateParams, $state, navigationService, dateTimeValidatorService, roleSecurityService) {
        var effectiveDate;
        $scope.stopNext = true;
        $scope.providerKey = PROVIDER_KEY.Admission;
        $scope.statusList = [{ Value: 1, Name: "Active" }, { Value: 0, Name: "Inactive" }];
        $scope.programs = [];

        $scope.programUnits = [];

        $scope.getPrograms = function () {
            $scope.programs = $scope.admission.DivisionID ? $rootScope.getOrganizationByDataKey('Program', $scope.admission.DivisionID) : [];
        };

        $scope.getProgramUnits = function () {

            $scope.programUnits = $scope.admission.ProgramID ? $rootScope.getOrganizationByDataKey('ProgramUnit', $scope.admission.ProgramID) : [];
        };

        var dataKey = "Company";

        var admissionTable = $("#admissionTable");

        var setDateTime = function (date, isSetTime) {
            var datetime = moment(date).toDate();
            var currentHour = datetime.getHours();
            var currentMinute = datetime.getMinutes();
            var period = currentHour >= 12 ? 'pm' : 'am';
            var ampm = $filter('toMilitaryTime')(pad(currentHour, 2) + ":" + pad(currentMinute, 2), period);

            if (!isSetTime)
                $scope.todayDate = $filter('toMMDDYYYYDate')(datetime, 'MM/DD/YYYY', 'useLocal');

            $scope.AMPM = $filter('toStandardTimeAMPM')(ampm);
            $scope.todayTime = $filter('toMMDDYYYYDate')(datetime, 'hh:mm', 'useLocal');
        };
        $scope.date = 'Date';
        $scope.dateOptions = {
            formatYear: "yy",
            startingDay: 1,
            showWeeks: false
        };

        $scope.endDate = new Date();

        var resetForm = function () {
            $scope.ctrl.admissionForm && $rootScope.formReset($scope.ctrl.admissionForm);
        };

        var checkFormStatus = function (state) {
            var stateDetail = { stateName: "patientprofile.general.admissionDischarge.admission", validationState: state };
            $rootScope.$broadcast('rightNavigationAdmissionHandler', stateDetail);
        };

        var resetFields = function () {
            admissionService.initAdmission($stateParams.ContactID).then(function (data) {
                $scope.admission = data;
                angular.extend($scope.admission, { ProgramUnitID: null });
                $scope.getPrograms();
                $scope.getProgramUnits();
                $scope.disableCompanyStatus = true;
                $scope.disableCheckBox = false;
                setDateTime(new Date());
            });
        };

        $scope.newAdmission = function () {
            angular.extend($stateParams, { ContactAdmissionID: null });
            init(); // BUGFIX-13077
        };

        var getOrganizationText = function (type, id) {
            var organization = $filter('filter')($rootScope.getOrganizationByDataKey(type), { ID: id }, true);
            return organization.length > 0 && organization[0].Name ? organization[0].Name : '';
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
                columns: [
                    {
                        field: 'EffectiveDate',
                        title: 'Effective Date/Time',
                        formatter: function (value, row, index) {
                            return moment(value).format('MM/DD/YYYY hh:mm A');
                        }
                    },
                    {
                        field: 'CompanyID',
                        title: 'Company',
                        formatter: function (value, row, index) {
                            return getOrganizationText('Company', value);
                        }
                    },
                    {
                        field: 'DivisionID',
                        title: 'Division',
                        formatter: function (value, row, index) {
                            return getOrganizationText('Division', value);
                        }
                    },
                    {
                        field: 'ProgramID',
                        title: 'Program',
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
                        field: 'ProgramUnitID',
                        title: 'Program Unit',
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
                        field: 'IsDischarged',
                        title: 'Admission Status',
                        formatter: function (value, row, index) {
                            return value ? 'Inactive' : 'Active';
                        }

                    },
                    {
                        field: 'ContactAdmissionID',
                        title: '',
                        formatter: function (value, row, index) {
                            return '<a href="javascript:void(0)" data-default-action security permission-key="General-General-Admission" permission="update" id="editBenefit" name="editBenefit" data-toggle="modal" ng-click="edit(' + value + ')" title="' + (row.IsDischarged && row.DataKey == 'Company' ? 'Edit' : 'View') + '" space-key-press><i class="fa ' + (row.IsDischarged && row.DataKey == 'Company' ? 'fa-pencil' : 'fa-eye') + ' fa-fw" /></a>';
                        }
                    }
                ]
            };
        };

        $scope.isRequiredCompany = function () {
            $scope.requiredInProgram = $filter('filter')($scope.admissionList, { CompanyID: $scope.admission.CompanyID }, true).length > 0;
            if (!$scope.requiredInProgram)
                $scope.admission.IsCompanyActive = 0;
            else
                $scope.admission.IsCompanyActive = 1;
        };

        var get = function () {

           return admissionService.get($stateParams.ContactID).then(function (data) {

                $scope.admissionList = data.DataItems;
                var company = $filter('filter')(data.DataItems, { DataKey: dataKey }, true);
                if (hasDetails(company)) {
                    $scope.companyAdmissionDate = effectiveDate = company[0].EffectiveDate;
                }
                $scope.isRequiredCompany();
                if ($scope.admissionList != null) {
                    admissionTable.bootstrapTable('load', $scope.admissionList);
                } else {
                    admissionTable.bootstrapTable('removeAll');
                }
                $scope.isLoading = false;
                resetForm();
            }, function (errorStatus) { resetForm(); });
        };

        var init = function () {
            $scope.admission = {
                ContactAdmissionID: 0
            };
            $scope.requiredInProgram = false;
            resetFields();
            $scope.$parent['autoFocus'] = true; //for Focus
            $scope.confirm = false;
            $scope.initializeBootstrapTable();
            checkFormStatus('valid');
            $scope.isCompanyEdit = false;
            $scope.hasPermission = roleSecurityService.hasPermission(GeneralPermissionKey.General_General_Admission, PERMISSION.UPDATE) ||
                                    roleSecurityService.hasPermission(GeneralPermissionKey.General_General_Admission, PERMISSION.CREATE);
            $scope.companyAdmissionDate = effectiveDate;
            get().then(function () {
                  $stateParams.ContactAdmissionID && setGridItem(admissionTable, 'ContactAdmissionID', $stateParams.ContactAdmissionID);
            });
        }

        var returnDateTime = function () {
            var date = moment($scope.todayDate).format('MM/DD/YYYY');
            var time = $filter("toMilitaryTime")($scope.todayTime, $scope.AMPM);
            var standardTime = moment(pad(time, 4), 'HH:mm').format('HH:mm');
            var dateTime = moment(date + ' ' + standardTime, 'MM/DD/YYYY HH:mm').format('MM/DD/YYYY HH:mm');
            return dateTime;
        }

        function showErrorDate(valditionName) {
            if (valditionName) {
                angular.element('#todayDateError').addClass('has-error');
                angular.element(valditionName).removeClass('ng-hide').addClass('ng-show');
                angular.element(valditionName).show();
            }
        }

        function hideErrorDate(valditionName) {
            if (valditionName) {
                angular.element('#todayDateError').removeClass('has-error');
                angular.element(valditionName).removeClass('ng-show').addClass('ng-hide');
                angular.element(valditionName).hide();
            }
        }

        function showErrorTime(validatorName) {
            if (validatorName) {
                angular.element('#admissionTimeError').addClass('has-error');
                angular.element(validatorName).removeClass('ng-hide').addClass('ng-show');
            }
        }

        function hideErrorTime(validatorName) {
            if (validatorName) {
                angular.element('#admissionTimeError').removeClass('has-error');
                angular.element(validatorName).removeClass('ng-show').addClass('ng-hide');
            }
        }

        $scope.checkTimeValidations = function () {
            checkTimeValidation();
        };

        var checkTimeValidation = function () {
            var time = $filter("toMilitaryTime")($scope.todayTime, $scope.AMPM);
            var standardTime = moment(pad(time, 4), 'HH:mm').format('HH:mm');
            var isValid = moment(standardTime, 'HH:mm').isValid();
            if (!isValid) {
                alertService.error('Time is not valid.');
                setDateTime(new Date(), true);
            } else if (!$scope.isCompanyEdit) {
                //construct date time from controls and compare with effectiveDate
                var selectedDate = moment(moment($scope.todayDate).format('MM/DD/YYYY') + ' ' + standardTime + ' ' + $scope.AMPM).toDate();
                var isTimevalid = moment(effectiveDate).toDate() > selectedDate;
                setFormValidations("admissionTime", 'mask', !isTimevalid);
            }

            return isValid;
        };

        var setFormValidations = function (elemSelector, error, isValid, msg) {
            if ($scope.ctrl.admissionForm[elemSelector]) {
                $scope.ctrl.admissionForm[elemSelector].$setValidity(error, isValid);
                $scope.customErrorMessage = msg;
                $scope.ctrl.admissionForm[elemSelector].xenErrorMessage = msg;
                $scope.ctrl.admissionForm[elemSelector].$invalid = !isValid;
                $scope.ctrl.admissionForm[elemSelector].$valid = isValid;
            }
        };

        var checkDateValidation = function () {
            var isValidDate = moment($scope.todayDate).isValid();
            if (!isValidDate) {
                alertService.error('Date is not valid.');
                setDateTime(new Date());
            }
            return isValidDate;
        }

        function checkDateTime() {
            if ($scope.admissionList) {
                var currentTimeControl = moment($filter('formatDate')($filter('formatDate')($scope.todayDate, 'MM/DD/YYYY') + ' ' + $filter('toStandardTime')($scope.todayTime) + ' ' + $scope.AMPM, 'MM/DD/YYYY HH:mm')).toDate();
                var company = $filter('filter')($scope.admissionList, {
                    CompanyID: $scope.admission.CompanyID
                }, true);

                var isCompanyInsert = true;
                var Notdischargeditems = $filter('filter')($scope.admissionList, { IsDischarged: false }, true);
                angular.forEach(Notdischargeditems, function (value, key) {
                    if ($scope.admission.CompanyID == value.CompanyID && value.DataKey == "Company" && value.IsDischarged == false) {
                        isCompanyInsert = false;
                        companyTime = moment($filter('formatDate')(value.EffectiveDate, 'MM/DD/YYYY HH:mm')).toDate();
                        return;
                    }
                });
                if (isCompanyInsert) {
                    var lastEffectiveDate = moment($filter('formatDate')($scope.admissionList[0].EffectiveDate, 'MM/DD/YYYY HH:mm')).toDate();
                    if (currentTimeControl < lastEffectiveDate) { // When Adding new company after inactive 
                        setFormValidations("admissionTime", 'customErrorMessage', false, "New admission can't be before last inactive time");
                        return true;
                    }
                }
                else {

                    /* 1.When adding program unit, then admission time can't be before company admission time */
                    if (companyTime > currentTimeControl && $scope.confirm) { 
                        setFormValidations("admissionTime", 'customErrorMessage', false, "Admission time can't be before Company Admission Time");
                        return true;
                    }

                    /* 2.When Updating Company Time canot be last program unit date and time*/
                    else if (validatePUDate(currentTimeControl, Notdischargeditems) && $scope.isCompanyEdit) { 
                        setFormValidations("admissionTime", 'customErrorMessage', false, "Admission time can't be after Program Unit Admission Time");
                        return true;
                    }
                    else {
                        resetValidations();
                        return false;
                    }
                }
            }
            else {
                return false;
            }
        }

        function resetValidations() {
            if ($scope.ctrl.admissionForm["admissionTime"]) {
                $scope.ctrl.admissionForm["admissionTime"].$setValidity('customErrorMessage', true);
                $scope.ctrl.admissionForm["admissionTime"].$setValidity('date', true);
                $scope.ctrl.admissionForm["admissionTime"].xenErrorMessage = undefined;
        }
        }

        $scope.admissionDateValidation = function () {
            resetValidations();
            if ($scope.todayDate && $scope.todayTime) {
                var isDateTimeInvalid = checkDateTime();
                if (!isDateTimeInvalid) {
                    var errorControlBlock = angular.element("#AdmissionTimeError");
                    var errorControl = angular.element("#startTimeFutureError");
                    var formControl = $scope.ctrl.admissionForm.admissionTime;
                    var formName = $scope.ctrl.admissionForm;
                    var selector = 'admissionTime';
                    var dateControl = $scope.todayDate;
                    dateTimeValidatorService.validateFutureDateTime(errorControlBlock, errorControl, formControl, dateControl, $scope.todayTime, $scope.AMPM, selector, formName);
                    /* if future date validation has error then show custom message */
                    !$.isEmptyObject($scope.ctrl.admissionForm["admissionTime"].$error) && setFormValidations("admissionTime", 'date', false, "admission start Time can not be of future");
            }
            }
        };



        $scope.admissionValidateTime = function () {
            var time = $scope.todayTime;
            if (time) {
                if (!checkTimeValidation()) {
                    showErrorTime();
                } else {
                    hideErrorTime();
                }
            }
        };

        function validatePUDate(currentTimeControl, dataKey) {
            var isCurrentTimeGreater = false;
            for (i = 0; i < dataKey.length; i++) {
                if (dataKey[i].DataKey == "ProgramUnit") {
                    var PUtime = moment($filter('formatDate')(dataKey[i].EffectiveDate, 'MM/DD/YYYY HH:mm')).toDate();
                    if (currentTimeControl > PUtime) {
                        isCurrentTimeGreater = true;
                        break;

                    }
                    else {
                        isCurrentTimeGreater = false;
                    }
                }
            }
            return isCurrentTimeGreater;
        }

        $scope.save = function (isNext, mandatory, hasErrors) {

            var isDirty = formService.isDirty();

            if (isDirty && !hasErrors && ($scope.confirm || $scope.isCompanyEdit)) {

                if ($scope.admission.IsCompanyActive == 0) {
                    alertService.error('Please change company status to Active.');
                    return false;
                }
                var modelToSave = angular.copy($scope.admission);
                modelToSave.EffectiveDate = returnDateTime();
                var company = $filter('filter')($scope.admissionList, {
                    CompanyID: $scope.admission.CompanyID
                }, true);

                var isCompanyInsert = true;
                var Notdischargeditems = $filter('filter')($scope.admissionList, { IsDischarged: false }, true);
                var currentTimeControl = moment($filter('formatDate')($filter('formatDate')($scope.todayDate, 'MM/DD/YYYY') + ' ' + $filter('toStandardTime')($scope.todayTime) + ' ' + $scope.AMPM, 'MM/DD/YYYY HH:mm')).toDate();
                angular.forEach(Notdischargeditems, function (value, key) {
                    if ($scope.admission.CompanyID == value.CompanyID && value.DataKey == "Company" && value.IsDischarged == false) {
                        isCompanyInsert = false;
                        companyTime = moment($filter('formatDate')(value.EffectiveDate, 'MM/DD/YYYY HH:mm')).toDate();
                        return;
                    }
                });

                if (isCompanyInsert) {
                    var companyToSave1 = angular.copy($scope.admission);
                    companyToSave1.DataKey = "Company";
                    companyToSave1.DivisionID = null;
                    companyToSave1.ProgramID = null;
                    companyToSave1.ProgramUnitID = $scope.admission.CompanyID;
                    companyToSave1.AdmissionReasonID = 0;
                    companyToSave1.Comments = "";
                    $scope.add(isNext, companyToSave1, true);
                }

                if (company.length == 0) {
                    var companyToSave = angular.copy($scope.admission);
                    angular.merge(companyToSave, { EffectiveDate: returnDateTime(), ProgramUnitID: $scope.admission.CompanyID, DivisionID: null, ProgramID: null, Comments: '' });
                    admissionService.add(companyToSave).then(function (response) {
                        if (modelToSave.DivisionID && modelToSave.ProgramID && modelToSave.ProgramUnitID) {
                            $scope.add(isNext, modelToSave);
                        } else {
                            alertService.success('Admission has been saved in Company.');
                            init();
                        }
                    }, function (errorStatus) {
                        alertService.error('Unable to save the company.');
                    });
                }
                else if ($scope.isCompanyEdit) {
                    var companyToSave = angular.copy($scope.admission);
                    angular.merge(companyToSave, { EffectiveDate: returnDateTime(), ProgramUnitID: $scope.admission.CompanyID, DivisionID: null, ProgramID: null, Comments: '' });
                    admissionService.update(companyToSave).then(function (response) {
                        if (modelToSave.DivisionID && modelToSave.ProgramID && modelToSave.ProgramUnitID) {
                            $scope.add(isNext, modelToSave);
                        } else {
                            alertService.success('Company has been updated.');
                            init();
                        }
                    }, function (errorStatus) {
                        alertService.error('Unable to update the company.');
                    });
                }
                else {
                    //Check if active admission record already exists
                    var admissionResult = $filter("filter")($scope.admissionList, {
                        DivisionID: modelToSave.DivisionID,
                        ProgramID: modelToSave.ProgramID,
                        ProgramUnitID: modelToSave.ProgramUnitID,
                        IsCompanyActive: true,
                        IsDischarged: false
                    });

                    //Stop duplicate program unit saving
                    if (admissionResult.length > 0) {
                        alertService.error('Duplicate Episode: Contact is currently active in ' + getOrganizationText("ProgramUnit", modelToSave.ProgramUnitID)+'.');
                        } else {
                        if (modelToSave.DivisionID && modelToSave.ProgramID && modelToSave.ProgramUnitID) {
                            $scope.add(isNext, modelToSave);
                        }
                    }
                }
            }
            else if (!isDirty && isNext) {
                $scope.next();
            }
        };

        $scope.add = function (isNext, modelToSave, isCompany) {

            admissionService.add(modelToSave).then(function (response) {
                if (response.ResultCode == 0) {
                    $scope.admission.ContactAdmissionID = (($scope.admission !== undefined) && ($scope.admission.ContactAdmissionID !== undefined) && ($scope.admission.ContactAdmissionID != 0))
                                        ? $scope.admission.ContactAdmissionID : response.ID;
                    if (isCompany == undefined)
                        alertService.success('Admission has been saved.');
                    else
                        alertService.success('Company has been saved.')
                    init();
                }
                if (isNext)
                    $scope.next();
            },
            function (errorStatus) {
                alertService.error('Unable to save the admission.');
            },
            function (notification) {
                alertService.warning(notification);
            }).then(function () {
            });

        };

        $scope.update = function (isNext, modelToSave) {
            admissionService.update(modelToSave).then(function (response) { },
            function (errorStatus) {
                alertService.error('Unable to update the admission.');
            },
            function (notification) {
                alertService.warning(notification);
            }).then(function () {
            });
        };

        $scope.next = function () {
            var nextState = angular.element("xen-workflow-action[data-state-name] > li.list-group-item.active").parents('.work-flow').next().find("xen-workflow-action[data-state-name]");

            if (nextState.length !== 1) {
                nextState = $scope.returnState;
            } else {
                nextState = nextState.attr('data-state-name');
            }
            $state.go(nextState);
        };

        //$scope.hasPermission = roleSecurityService.hasPermission(GeneralPermissionKey.General_General_Admission, PERMISSION.UPDATE) ||
        //                            roleSecurityService.hasPermission(GeneralPermissionKey.General_General_Admission, PERMISSION.CREATE);

        $scope.edit = function (id) {
            var admissionData = $filter('filter')($scope.admissionList, { ContactAdmissionID: id }, true)[0];
            setDateTime(admissionData.EffectiveDate);
            $scope.admission = admissionData;
            $scope.isAdmissionDisabled = true;
            $scope.isRequiredCompany();
            if ($scope.admission.DataKey == dataKey)
                $scope.admission.IsCompanyActive = admissionData.IsDischarged ? 0 : 1;
            $scope.getPrograms();
            $scope.getProgramUnits();
            if ($scope.admission.IsDischarged && $scope.admission.DataKey == dataKey) {
                $scope.confirm = true;
                $scope.requiredInProgram = false;
                $scope.disableCheckBox = false;
            } else {
                $scope.confirm = false;
                $scope.disableCheckBox = true;
            }
            $scope.hasPermission = roleSecurityService.hasPermission(GeneralPermissionKey.General_General_Admission, PERMISSION.UPDATE);

            $scope.isCompanyEdit = ($scope.admission.DataKey == dataKey);
            if ($scope.isCompanyEdit) {
                $scope.companyAdmissionDate = null;
            }
            resetForm();
        }

        init();

    }]);

}());
