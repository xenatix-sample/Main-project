angular.module('xenatixApp')
    .controller('recordingServiceController', ['$scope', '$filter', 'alertService', '$stateParams', '$rootScope', '$q', '$state', '$injector', '$timeout', 'formService', 'lookupService', 'alertService', 'serviceRecordingService', 'navigationService', 'credentialSecurityService', 'eSignatureService', 'ServiceRecordingSourceID', 'voidService', 'FormName', 'roleSecurityService', 'dateTimeValidatorService', 'userCredentialService', 'providersService', 'helperService', '_',
function ($scope, $filter, alertService, $stateParams, $rootScope, $q, $state, $injector, $timeout, formService, lookupService, alertService, serviceRecordingService, navigationService, credentialSecurityService, eSignatureService, ServiceRecordingSourceID, voidService, FormName, roleSecurityService, dateTimeValidatorService, userCredentialService, providersService, helperService, _) {

    var DocumentTypeID = DOCUMENT_TYPE.ServiceRecording;
    var SourceHeaderID = 0;
    var START_TIME_STR = 'startTime';
    var providerData = null;
    var END_TIME_STR = 'endTime';
    var ignoreTime = "12:00:01 AM";
    var licenseIssueDate = 'LicenseIssueDate';
    var licenseExpirationDate = 'LicenseExpirationDate';
    var defaultTime = "00:00:00";
    var adjustedTime = "00:00:01";
    $scope.CallMinStartDate = new Date(70, 1, 1);
    if ($stateParams.CallCenterHeaderID)
        SourceHeaderID = $stateParams.CallCenterHeaderID;
    else if ($stateParams.BenefitsAssistanceID)
        SourceHeaderID = $stateParams.BenefitsAssistanceID;
    else if ($stateParams.ContactFormsID)
        SourceHeaderID = $stateParams.ContactFormsID;

    var defaultDropdown = {                 //TODO: Change hardcode to values from enums
        ServiceLocationID: 5,
        AttendanceStatusID: 2,
        DeliveryMethodID: 1,
    };

    var moduleComponentID;

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1,
        showWeeks: false
    };

    $scope.permissionKey = $state.current.data.permissionKey;
    var programUnits = lookupService.getLookupsByTypeAll("WorkflowProgramUnit");
    $scope.programUnits = $filter('filter')(programUnits, { DataKey: $scope.permissionKey, IsActive: true }, true);
    var hasCompanyPermission = roleSecurityService.hasPermission($scope.permissionKey, PERMISSION.READ, PERMISSION_LEVEL.Company);
    var isCredentialNeedService = false;

    var resetForm = function () {
        $rootScope.formReset($scope.ctrl.serviceRecordingForm);
        $scope.formDetailsChanged = false;
    };
    var serviceName = '';

    var init = function () {

        if ($state.current.name.toLowerCase().indexOf('bapnservice') >= 0) {
            $scope.providerKey = PROVIDER_KEY.BAPN_Service;
            $scope.supervisorproviderKey = PROVIDER_KEY.BAPN_Service;
        }
        else {
            $scope.providerKey = PROVIDER_KEY.IDD_Intake;
            $scope.supervisorproviderKey = PROVIDER_KEY.IDD_Intake;
        }

        if ($state.current.name.toLowerCase().indexOf('service') !== -1) {
            $scope.isTopazReady = false;

            $scope.topazModel = {
                modelNumber: '',
                b64ImageData: '',
                DeviceMessage: 'You do not have application permissions to electronically sign!'
            };
            $scope.topazModel.hideSignatureBtns = true;    //By default the signature buttons will remain hidden
            $scope.$watch('isTopazReady', $scope.OnTopazReady);
        }
        providersService.getProviders($scope.providerKey).then(function (Response) {
            providerData = Response.DataItems;
        });
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
            ServiceRecordingHeaderID: null,
            ParentServiceRecordingID: null,
            ServiceRecordingVoidID: null,
            TrackingFieldID: null,
            ServiceRecordingID: 0,
            ServiceRecordingSourceID: ServiceRecordingSourceID,
            CallCenterHeaderID: null,
            ServiceItemID: null,
            AttendanceStatusID: null,//defaultDropdown.AttendanceStatusID,
            DeliveryMethodID: null, //defaultDropdown.DeliveryMethodID,
            ServiceStatusID: null,
            ServiceLocationID: null,//defaultDropdown.ServiceLocationID,
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
            CallStartDate: $filter('formatDate')(new Date(), 'MM/DD/YYYY'),
            CallStartTime: $filter('formatDate')(new Date(), 'hhmm'),
            CallEndDate: null,
            CallEndTime: null,
            CallStartAMPM: $filter('toStandardTimeAMPM')(getCurrentTime(new Date())),
            CallEndAMPM: null,
            Duration: "0mins",
            AttendedList: [],
            AdditionalUserList: [],
            Provider: null,
            CoProvider: null,
            SentToCMHCDate: null
        };

        $scope.signature = {
            UserFullName: '',
            digitalPassword: '',
            CredentialID: null,
            Password: null,
            DateSigned: $filter('formatDate')(new Date(), 'MM/DD/YYYY')
        };

        //Set the serviceName variable to filter Program Unit as per the screen
        if ($state.current.name.indexOf('crisisline') >= 0) {
            serviceName = SERVICES.Crisis_Line;
        }
        else if ($state.current.name.indexOf('lawliaison') >= 0) {
            serviceName = SERVICES.Law_Liaison;
            $scope.isLawLiaison = true;
        }
        else if ($state.current.name.toLowerCase().indexOf('bapnservice') >= 0 || $state.current.name.toLowerCase().indexOf('formservice') >= 0) {
            serviceName = SERVICES.BAPN;
            $scope.bapnScreen = true;
            $scope.hideTracking = true;
            if ($state.current.name.toLowerCase().indexOf('bapnservice') >= 0) {
                isCredentialNeedService = true;
                moduleComponentID = MODULE_COMPONENT.BAPN;
                if ($stateParams.ReadOnly == "view")
                    $scope.topazModel.b64ImageData = true;
            } else {
                moduleComponentID = MODULE_COMPONENT.IntakeForms;
            }
        }
        var allServiceLookups = serviceRecordingService.getAllServiceLookups(moduleComponentID);
        angular.extend($scope, allServiceLookups);
        $scope.getAllServiceData();

        $state.transitionTo($state.current.name, $stateParams);
        resetForm();
    };

    $scope.getAllServiceData = function () {
        var dfdGet = $q.defer();
        var promiseAll = [];
        promiseAll.push(getProgramUnit(serviceName));
        promiseAll.push(getUserInfo());
        promiseAll.push(getServiceRecording(SourceHeaderID));
        $q.all(promiseAll).finally(function () {
            $scope.programUnits = $.grep($scope.programUnits, function (item, indx) {
                var result = _.find($scope.programunitAdmission, {
                    OrganizationID: item.OrganizationID
                })
                if (result) {
                    return item;
                }
            });
            initSignature().then(function () {
                dfdGet.resolve(true);
            });
        })
        return dfdGet.promise;
    }

    $scope.topazModel = [];
    $scope.OnTopazReady = function (value) {
        if (value === true) {
            $scope.topazModel.Init();
            $scope.topazModel.ImageCallback = $scope.sigImageCallback;
            $scope.topazModel.NoPointsCallback = $scope.noPointsCallback;
            $scope.topazModel.ClearCallback = $scope.clearImageCallback;
        }
    };

    var signatureBLOB;
    $scope.sigImageCallback = function (str) {
        signatureBLOB = str;
        $scope.topazModel.b64ImageData = str;
        $scope.signatureVerified = true;
    };

    $scope.clearImageCallback = function (str) {
        signatureBLOB = null;
        $scope.signatureVerified = false;
    };
    var setDefaultCredential = function () {
        if ($scope.userCredentials && $scope.userCredentials.length == 1) {
            $scope.signature.CredentialID = $scope.userCredentials[0].CredentialID;
            $scope.checkPermission();
            $scope.signatureFormReset();
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
        var signProm = $q.defer();
        $scope.isServiceCoordinatorReadonly = true;
        $scope.isCredentialReadonly = false;
        $scope.isPasswordReadonly = false;
        $scope.isSigned = false;
        $scope.signatureVerified = false;
        if (!$scope.isSignatureExist)
            getSignature().finally(function () {
                signProm.resolve(true);
            });
        else {
            signProm.resolve(true);
        }

        return signProm.promise;
    };

    var getUserInfo = function () {
        return navigationService.get().then(function (response) {
            if (hasData(response)) {
                var data = response.DataItems[0];
                //Get UserID, digital password
                $scope.userID = data.UserID;
                $scope.signature.UserFullName = data.UserFullName;
                $scope.signature.digitalPassword = data.DigitalPassword;
                //Get User Credentials
                return userCredentialService.getwithServiceID($scope.userID, moduleComponentID).then(function (data) {
                    if (hasData(data)) {
                        $scope.allUserCredentials = $filter('orderBy')(filterFutureOrExpiredRecords(data.DataItems, licenseExpirationDate, licenseIssueDate), 'CredentialName');
                        $scope.userCredentials = $filter('filter')($scope.allUserCredentials, { ServicesID: $scope.serviceRecording.ServiceItemID }, true)
                        setDefaultCredential();
                    }
                    else {
                        $scope.allUserCredentials = [];
                    }
                });
            }
        });

    }



    $scope.signatureFormReset = function () {
        if ($scope.signatureForm)
            $rootScope.formReset($scope.signatureForm, $scope.signatureForm.$name);
    };

    var getSignature = function () {
        if ($scope.serviceRecording.ServiceRecordingID) {
            return eSignatureService.getDocumentSignatures(DocumentTypeID, $scope.serviceRecording.ServiceRecordingID).then(function (response) {
                if (hasData(response)) {
                    checkFormStatus(true);
                    var signatureDetails = response.DataItems[0];
                    angular.extend($scope.signature, signatureDetails);
                    $scope.signature.DateSigned = $filter('formatDate')(signatureDetails.ModifiedOn, 'MM/DD/YYYY');
                    $scope.signature.UserFullName = lookupService.getText("Users", signatureDetails.EntityId);
                    $scope.topazModel.b64ImageData = signatureDetails.SignatureBlob;
                    $scope.userCredentials = $filter('filter')($scope.userCredentials, { CredentialID: $scope.signature.CredentialID }, true);
                    if (!$scope.userCredentials || $scope.userCredentials.length == 0) {
                        $scope.userCredentials = $filter('filter')(lookupService.getLookupsByType('Credential'), { CredentialID: $scope.signature.CredentialID }, true);
                    }
                    $scope.isCredentialReadonly = true;
                    $scope.isPasswordReadonly = true;
                    $scope.isSigned = true;
                    $scope.signatureFormReset();
                }
                else {
                    checkFormStatus(false);
                }
            });
        }
        else {
            return $scope.promiseNoOp();
        }
    };

    $scope.validateRequiredCallStartDate = function () {
        $scope.calcDuration();
        $scope.validateMinStartDate();
    }

    //Executes when the CallEndDate field is changed
    $scope.validateRequiredCallEndDate = function () {
        $scope.calcDuration();
        $scope.validateMinStartDate();
    }

    // Validate the start date against the minimum allowed date object, wich is set to 1/1/1970
    $scope.validateMinStartDate = function () {
        if (new moment($scope.serviceRecording.CallStartDate)._d < $scope.CallMinStartDate) {
            $scope.ctrl.serviceRecordingForm.recordingServiceStartDate.$error.lessThanMinValidDate = true;
            $scope.ctrl.serviceRecordingForm.recordingServiceStartDate.$invalid = true;
            $scope.ctrl.serviceRecordingForm.recordingServiceStartDate.$valid = false;
            $scope.ctrl.serviceRecordingForm['recordingServiceStartDate'].$setValidity('lessThanMinValidDate', false);
        }
    };

    $scope.checkPermission = function () {
        $scope.topazModel.hideSignatureBtns = $scope.signature.CredentialID ? false : true
        if ($scope.topazModel.hideSignatureBtns) {
            $scope.topazModel.b64ImageData = signatureBLOB = '';
            $timeout(function () {
                $scope.topazModel.Reset();
            }, 0, false);
        }
    };

    $scope.verifySignature = function () {
        if (!$scope.signature.digitalPassword) {
            alertService.error('Digital password not set. Please set from user profile.');
            return;
        }
        if ($scope.signature.CredentialID && $scope.signature.Password) {
            bootbox.confirm("Are you sure you have completed all areas needed in your workflow?", function (result) {
                if (result) {
                    if (hex_md5($scope.signature.Password) == $scope.signature.digitalPassword) {
                        $scope.isSigned = true;
                        $scope.signatureVerified = true;
                        $scope.isCredentialReadonly = true;
                        $scope.isPasswordReadonly = true;
                        $scope.signature.DateSigned = $filter('formatDate')(new Date(), 'MM/DD/YYYY');
                    } else {
                        alertService.error('Invalid password.');
                    }
                }
            });

        } else {
            if (!$scope.signature.CredentialID)
                alertService.error('Please fill out the Credential field.');
            if (!$scope.signature.Password)
                alertService.error('Please fill out the Digital Password field.');
        }
    };

    var saveDSignature = function (sourceHeaderID) {
        var signature = {
            DocumentId: sourceHeaderID,      //Primary key of record against which signature will be saved
            DocumentTypeId: DocumentTypeID,             //Type it could be CallCenter, Discharge Note etc
            EntitySignatureId: null,                    //Should be passed NULL for D-Signature, should be evaluated with in sproc for user
            EntityId: $scope.userID,                    //User Id
            EntityTypeId: 1,                            //Reference Id for type of entity. Here it is for User. update it to fetch from enum
            SignatureBlob: signatureBLOB,                         //Signature in byte format
            ModifiedOn: $scope.signature.DateSigned,
            CredentialID: $scope.signature.CredentialID,
            SignatureID: null
        };

        return eSignatureService.saveDocumentSignature(signature).then(function (response) {
            if (response.data.ResultCode === 0) {
                $scope.signatureFormReset();
                //alertService.success('Signature status saved successfully!');
            } else {
                var msg = 'Error while saving signature! Please reload the page and try again.';
                alertService.error(msg);
            }
        });
    };

    var checkFormStatus = function (status) {
        var stateName = $state.current.name.indexOf('bapn') > -1 ? 'bapnService' : 'formservice';
        $rootScope.$broadcast(stateName, { validationState: status ? VALIDATION_STATE.Valid : VALIDATION_STATE.Invalid });
    };

    $scope.onServiceItemChanged = function (serviceItemID) {
        serviceItemID = serviceItemID || 0;
        if (!$scope.isSigned) {
            if (hasDetails($scope.allUserCredentials)) {
                $scope.userCredentials = $filter('filter')($scope.allUserCredentials, {
                    ServicesID: $scope.serviceRecording.ServiceItemID ? $scope.serviceRecording.ServiceItemID : -1
                }, true)
                setDefaultCredential();
            }
        }
        $scope.hideTracking = ($scope.serviceRecording.ServiceItemID !== SERVICE_ITEM.Benefits_Assistance);
        if ($scope.hideTracking)
            $scope.serviceRecording.TrackingFieldID = null;
        if (isCredentialNeedService)
            $scope.checkPermission();
    };

    var reinitSignature = function () {
        if (!$scope.isSigned) {
            $scope.signature = {
                UserFullName: '',
                digitalPassword: '',
                CredentialID: null,
                Password: null,
                DateSigned: null
            };
        }
    };

    var getProviderbyid = function (item) {
        return providersService.getProviderbyid(item.UserID).then(function (data) {
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
    }

    var getServiceRecording = function () {
        $scope.isLoading = true;
        $scope.noAccess = null;
        return serviceRecordingService.getServiceRecording(SourceHeaderID, ServiceRecordingSourceID).then(function (data) {
            if (hasData(data)) {
                $scope.pageSecurity = ServiceRecordingSourceID;

                if (data.DataItems[0].UserID != $scope.userID) {
                    $scope.noAccess = true;
                    reinitSignature();
                }

                angular.extend($scope.serviceRecording, data.DataItems[0]);

                $scope.onServiceItemChanged($scope.serviceRecording.ServiceItemID);
                if ($scope.serviceRecording.SentToCMHCDate)
                    $scope.serviceRecording.SentToCMHCDate = moment($scope.serviceRecording.SentToCMHCDate).format('MM/DD/YYYY HH:mm:ss');
                if ($scope.serviceRecording.ServiceRecordingVoidID) {
                    $scope.isVoidedShown = true;
                    voidService.getVoidRecordedService($scope.serviceRecording.ServiceRecordingID).then(function (voidServiceResponse) {
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
                }

                if ($scope.serviceRecording.ServiceStartDate) {
                    var _rVal = toDateTime($scope.serviceRecording.ServiceStartDate);
                    $scope.serviceRecording.ServiceStartDate = $scope.serviceRecording.CallStartDate = _rVal.date;
                    $scope.serviceRecording.ServiceStartTime = $scope.serviceRecording.CallStartTime = _rVal.time;
                    $scope.serviceRecording.ServiceStartAMPM = $scope.serviceRecording.CallStartAMPM = _rVal.meridiem;
                }
                else {
                    $scope.serviceRecording.ServiceStartDate = $scope.serviceRecording.CallStartDate = null;
                    $scope.serviceRecording.ServiceStartTime = $scope.serviceRecording.CallStartTime = null;
                    $scope.serviceRecording.ServiceStartAMPM = $scope.serviceRecording.CallStartAMPM = null;
                }

                if ($scope.serviceRecording.ServiceEndDate) {
                    var _rVal = toDateTime($scope.serviceRecording.ServiceEndDate);
                    $scope.serviceRecording.ServiceEndDate = $scope.serviceRecording.CallEndDate = _rVal.date;
                    $scope.serviceRecording.ServiceEndTime = $scope.serviceRecording.CallEndTime = _rVal.time;
                    $scope.serviceRecording.ServiceEndAMPM = $scope.serviceRecording.CallEndAMPM = _rVal.meridiem;
                }
                else {

                    $scope.serviceRecording.ServiceEndDate = $scope.serviceRecording.CallEndDate = null;
                    $scope.serviceRecording.ServiceEndTime = $scope.serviceRecording.CallEndTime = null;
                    $scope.serviceRecording.ServiceEndAMPM = $scope.serviceRecording.CallEndAMPM = null;
                }

                if (!$scope.serviceRecording.AttendedList)
                    $scope.serviceRecording.AttendedList = [];

                if (!$scope.serviceRecording.AdditionalUserList)
                    $scope.serviceRecording.AdditionalUserList = [];

                var additionalUserList = [];

                var promiseAll = [];

                if ($scope.serviceRecording.OrganizationID) {
                    helperService.updateLookupList($scope.programUnits, programUnits,
                                { OrganizationID: $scope.serviceRecording.OrganizationID, DataKey: $scope.permissionKey });
                    $scope.organizationDetails = $filter('filter')($scope.programUnits, { OrganizationID: $scope.serviceRecording.OrganizationID }, true)[0];
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
                    $scope.calcDuration();
                    resetForm();
                });
            }
            else {
                $scope.serviceRecording.ServiceRecordingID = 0;
                $scope.pageSecurity = 0;

                navigationService.get().then(function (data) {
                    if (hasData(data)) {
                        $scope.serviceRecording.UserID = data.DataItems[0].UserID;
                    }
                }).finally(function () {
                    resetForm();
                });

            }

            if (!$scope.serviceRecording.OrganizationID && $scope.isLawLiaison) {
                var serviceNameList = $filter('filter')($scope.programUnits, function (item) { return (item.Name == serviceName); });
                if (serviceNameList && serviceNameList.length > 0) {
                    $scope.serviceRecording.OrganizationID = serviceNameList[0].ID;

                }
            }
            resetForm();
        },
        function (errorStatus) {
            alertService.error('Unable to get Service Recording: ' + errorStatus + '.');
        }).finally(function () {
            $scope.isLoading = false;
            resetForm();
        });
    };

    $scope.filterDataEntryReasons = function (items) {
        var result = {};
        angular.forEach(items, function (value, key) {
            if (key.indexOf('Incorrect') > -1 && value == true) {
                result[key.replace(/([a-z])([A-Z])/g, "$1 $2")] = value;
            }
        });
        return result;
    }

    var filterDataEntryReasons = function (viewModel) {
        var result = {};
        angular.forEach(viewModel, function (value, key) {
            if (key.indexOf('Incorrect') > -1 && value == true) {
                result[key] = value;
            }
        });
        return result;
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
            CallEndDate: $filter('formatDate')(new Date(), 'MM/DD/YYYY'),
            CallEndTime: $filter('formatDate')(new Date(), 'hh:mm')
        };
        angular.extend($scope.serviceRecording, serviceRecordingDefault);
    };

    $scope.noPointsCallback = function () {
        alertService.error("Please sign before submitting the form.");
    };

    var getProgramUnit = function (serviceName) {
        var dfdProg = $q.defer();
        if (serviceName == SERVICES.BAPN) {
            //code to get all prog units that individual is admitted to
            if ($injector.has('admissionService')) {
                admissionService = $injector.get('admissionService');
                admissionService.get($stateParams.ContactID).then(function (data) {
                    if (hasData(data)) {
                        $scope.programunitAdmission = [];
                        angular.forEach(data.DataItems, function (item, idx) {
                            if ($stateParams.BenefitsAssistanceID || $stateParams.ContactFormsID) {
                                populateProgramUnit(item);
                            }
                            else if (!item.IsDischarged) {
                                populateProgramUnit(item);
                            }
                            dfdProg.resolve(true);
                        });
                        resetForm();
                    }
                });
            }
            else {
                dfdProg.resolve(true);
            }
        } else {
            var ProgUnits = lookupService.getLookupsByType("ProgramUnit");
            $scope.programUnits = $filter('filter')(ProgUnits, { ServiceName: serviceName });
            dfdProg.resolve(true);
        }
        return dfdProg.promise;
    };

    var populateProgramUnit = function (item) {
        if (item.ProgramUnitID && item.ProgramUnitID > 0) {
            var puItem = { OrganizationID: item.ProgramUnitID, Name: getOrganizationText('ProgramUnit', item.ProgramUnitID) };
            $scope.programunitAdmission.push(puItem);
        }
    };

    $scope.validateStartEndTime = function (timeSelector) {
        var time = $("#" + timeSelector).val();
        time = time + (timeSelector === "startTime" ? $scope.serviceRecording.CallStartAMPM : $scope.serviceRecording.CallEndAMPM);
        $scope.serviceRecording.CallStartDate = angular.isDate($scope.serviceRecording.CallStartDate) ? $filter('formatDate')($scope.serviceRecording.CallStartDate, 'MM/DD/YYYY') : $scope.serviceRecording.CallStartDate;

        $scope.serviceRecording.CallEndDate = angular.isDate($scope.serviceRecording.CallEndDate) ? $filter('formatDate')($scope.serviceRecording.CallEndDate, 'MM/DD/YYYY') : $scope.serviceRecording.CallEndDate;

        var fullStartDateTime = new Date($scope.serviceRecording.CallStartDate + ' ' + $filter('toStandardTime')($scope.serviceRecording.CallStartTime) + ' ' + $scope.serviceRecording.CallStartAMPM);

        var fullEndDateTime = new Date($scope.serviceRecording.CallEndDate + ' ' + $filter('toStandardTime')($scope.serviceRecording.CallEndTime) + ' ' + $scope.serviceRecording.CallEndAMPM);
        //The line below needs to be checked as the match function returns string and not boolean and we are trying to use them as boolean in line 379
        //http://stackoverflow.com/questions/3183322/one-line-match-in-js-regex
        //We need to use search instead of match and may be consider adding Condition for Placeholder to be ok
        //var validTime = (time.search(/^([1-9]|1[0-2]|0[1-9]){1}(:[0-5][0-9][aApP][mM]){1}$/) != -1) || (time === "hh:mmAM" || time === "hh:mmPM");
        var validTime = time.match(/^([1-9]|1[0-2]|0[1-9]){1}(:[0-5][0-9][aApP][mM]){1}$/)
                            && (!moment(fullEndDateTime).isValid() || (moment(fullEndDateTime).isValid() && fullStartDateTime <= fullEndDateTime));
        //var isFutureDate = (timeSelector === "startTime" ? (fullStartDateTime > new Date()) : (fullEndDateTime > new Date())) && !$scope.bapnScreen;
        var isFutureDate = false;
        //Same Updates to time should be made here.
        if (validTime == true) {
            $('#' + timeSelector + 'Container').removeClass('has-error');
            $('#' + timeSelector + 'Error').removeClass('ng-show').addClass('ng-hide');
            $('#' + timeSelector + 'FutureError').removeClass('ng-show').addClass('ng-hide');
            $scope.ctrl.serviceRecordingForm[timeSelector].$invalid = false;
            $scope.ctrl.serviceRecordingForm[timeSelector].$valid = true;
        }
        else if (validTime == false) {
            $('#' + timeSelector + 'Container').addClass('has-error');
            isFutureDate ? $('#' + timeSelector + 'FutureError').removeClass('ng-hide').addClass('ng-show') : $('#' + timeSelector + 'Error').removeClass('ng-hide').addClass('ng-show');
            $scope.ctrl.serviceRecordingForm[timeSelector].$invalid = true;
            $scope.ctrl.serviceRecordingForm[timeSelector].$valid = false;
        }
    }

    //Vaidate the Service Call Start/End Time
    $scope.validateCallTime = function () {
        $scope.serviceRecording.CallStartDate = angular.isDate($scope.serviceRecording.CallStartDate) ? $filter('formatDate')($scope.serviceRecording.CallStartDate, 'MM/DD/YYYY') : $scope.serviceRecording.CallStartDate;
        $scope.serviceRecording.CallEndDate = angular.isDate($scope.serviceRecording.CallEndDate) ? $filter('formatDate')($scope.serviceRecording.CallEndDate, 'MM/DD/YYYY') : $scope.serviceRecording.CallEndDate;
        var fullStartDateTime = new Date($scope.serviceRecording.CallStartDate + ' ' + $filter('toStandardTime')($scope.serviceRecording.CallStartTime) + ' ' + $scope.serviceRecording.CallStartAMPM);
        var fullEndDateTime = new Date($scope.serviceRecording.CallEndDate + ' ' + $filter('toStandardTime')($scope.serviceRecording.CallEndTime) + ' ' + $scope.serviceRecording.CallEndAMPM);
        var currentTime = new Date(moment($filter('toMilitaryTime')($filter('date')(new Date(), "HH:mm:ss"), $scope.serviceRecording.CallStartAMPM), 'HH:mm:ss'));

        //The currenttime needed to be formatted correctly
        var validTime = (!moment(fullStartDateTime).isValid() || !moment(fullEndDateTime).isValid())
                            ||
                         (fullEndDateTime <= currentTime);

        if ($scope.serviceRecording.CallEndTime != "" && $scope.serviceRecording.CallEndTime != null) {
            validTime = (fullStartDateTime <= fullEndDateTime);
        }

        return validTime;
    }

    var getOrganizationText = function (type, id) {
        var organization = $filter('filter')($rootScope.getOrganizationByDataKey(type, undefined, hasCompanyPermission), { ID: id }, true);
        return organization.length > 0 && organization[0].Name ? organization[0].Name : '';
    }


    var saveServiceRecording = function (isUpdate, referralClientInformation) {

        if (!isUpdate) {
            return serviceRecordingService.addServiceRecording(referralClientInformation)
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
        if ($state.current.name.indexOf('crisisline') >= 0) {
            angular.extend($stateParams, {
                CallCenterHeaderID: SourceHeaderID,
                ContactID: $stateParams.ContactID
            });
            $state.go("callcenter.crisisline.columbiasuicidescale", $stateParams);
        } else if ($state.current.name.indexOf('lawliaison') >= 0) {
            angular.extend($stateParams, {
                CallCenterHeaderID: SourceHeaderID,
                ContactID: $stateParams.ContactID
            });
            $state.go("callcenter.lawliaison.progressnotes", $stateParams);
        } else if ($state.current.name.toLowerCase().indexOf('bapnService') >= 0) {
            angular.extend($stateParams, {
                ContactID: ContactID,
                AssessmentID: ASSESSMENT_TYPE.BenefitAssessmentsProgressNote,
                ResponseID: $scope.responseId ? $scope.responseId : 0,
                BenefitsAssistanceID: SourceHeaderID,
                ReadOnly: readOnly ? readOnly : 'edit',
                DocumentStatusID: $stateParams.DocumentStatusID ? $stateParams.DocumentStatusID : DOCUMENT_STATUS.Draft
            });
            // Use the assessment service to navigate to correct section
            assessmentService.navigateToSection('bapnService', params);

        }
    };

    $scope.saveRecordingService = function (sourceHeaderID) {
        var deferred = $q.defer();
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

        if (!SourceHeaderID)
            SourceHeaderID = sourceHeaderID;

        $scope.serviceRecording.CallCenterHeaderID = SourceHeaderID;
        $scope.serviceRecording.SourceHeaderID = SourceHeaderID;
        $scope.serviceRecording.ServiceRecordingSourceID = ServiceRecordingSourceID;
        if ($scope.serviceRecording.SupervisorUser)
            $scope.serviceRecording.SupervisorUserID = $scope.serviceRecording.SupervisorUser.ID;

        var isUpdate = ($scope.serviceRecording.ServiceRecordingID && $scope.serviceRecording.ServiceRecordingID !== 0) ? true : false;
        saveServiceRecording(isUpdate, $scope.serviceRecording).then(function (response) {
            if (response.ResultCode == 15) {
                bootbox.confirm("Data has been changed. Do you want to reload?", function (result) {
                    if (result)
                        init();
                });
                deferred.reject(response);
                return;
            } else if (response.ResultCode == 0) {
                checkFormStatus(true);
                var action = isUpdate ? 'updated' : 'added';
                if (!isUpdate)
                    $scope.serviceRecording.ServiceRecordingID = response.ID;

                if (signatureBLOB && $scope.serviceRecording.ServiceRecordingID) {
                    //Save signature
                    saveDSignature($scope.serviceRecording.ServiceRecordingID).then(function () {
                        $scope.signatureFormReset();
                        $scope.getAllServiceData().then(function () {
                            deferred.resolve(response);
                        });
                    });
                }
                else {
                    $scope.signatureFormReset();
                    $scope.getAllServiceData().then(function () {
                        deferred.resolve(response);
                    });
                }
            }
            else {
                deferred.resolve(response);
            }
        },
        function (errorStatus) {
            deferred.reject(errorStatus);
        },
        function (notification) {
            alertService.warning(notification);
        });
        //}
        return deferred.promise;
    };

    var postSave = function (response, action, isNext) {
        if (response.ResultCode !== 0) {
            alertService.error(response.ResultMessage);
            return $scope.promiseNoOp();
        } else {
            alertService.success('Service Recording has been ' + action + ' successfully.');
            if ($state.current.name.toLowerCase().indexOf('bapnService') >= 0) {
                // SourceHeaderID
                angular.extend($stateParams, {
                    ContactID: $stateParams.ContactID ? $stateParams.ContactID : 0,
                    AssessmentID: ASSESSMENT_TYPE.BenefitAssessmentsProgressNote,
                    ResponseID: $stateParams.responseId ? $stateParams.responseId : 0,
                    BenefitsAssistanceID: $stateParams.BenefitsAssistanceID ? $stateParams.BenefitsAssistanceID : 0,
                    ReadOnly: $stateParams.ReadOnly ? $stateParams.ReadOnly : 'edit',
                    DocumentStatusID: $stateParams.DocumentStatusID ? $stateParams.DocumentStatusID : DOCUMENT_STATUS.Draft
                });
                $state.transitionTo('bapnService', $stateParams);
            }
            if (isNext) {
                next();
            }
            else {
                init();
                resetDateTimeValidations('startTime');
                // resetDateTimeValidations('endTime');
                return true;
            }
        }
    };

    //Validate the Digital Signature
    $scope.validateSignature = function () {
        return signatureBLOB ? true : false;
    };

    var getFormattedDate = function (dateVal, timeVal) {
        var hr = timeVal.substring(0, timeVal.indexOf(':'));
        if (timeVal.substring(timeVal.indexOf(' ') + 1, timeVal.length) == "PM" && hr != 12)      //checks if PM, adds 12 hours
            hr = +hr + +12;
        var min = timeVal.substring(timeVal.indexOf(':') + 1, timeVal.indexOf(' '));
        return dateVal.setHours(hr, min);
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
        var isAlreadyInList = $filter('filter')($scope.serviceRecording.AdditionalUserList, function (addCoPro) { return (addCoPro.ID == copyOfItem.ID) && addCoPro.IsActive == 1; }, true);
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

    function dateRequired(isRequired) {
        if (isRequired) {
            angular.element("#endDate").addClass('has-error');
        }
        else {
            angular.element("#endDate").removeClass('has-error');
        }
    }

    $scope.validateProviderFutureDate = function () {
        var formName = $scope.ctrl.serviceRecordingForm;
        if (formName) {
            var errorControlBlock = angular.element("#startTimeContainer");
            var errorControl = angular.element("#startTimeFutureError");
            var formControl = $scope.ctrl.serviceRecordingForm.startTime;
            var dateControl = $filter('formatDate')($scope.serviceRecording.CallStartDate, 'MM/DD/YYYY');
            var datePart = $filter('formatDate')(dateControl, 'MM/DD/YYYY');
            var isRequired = formControl ? formControl.$error.required : false;
            var isDate = formControl ? formControl.$error.date : false;
            var selector = START_TIME_STR;
            if (!isRequired || (isRequired && isDate))
                dateTimeValidatorService.validateFutureDateTime(errorControlBlock, errorControl, formControl, dateControl, $scope.serviceRecording.CallStartTime, $scope.serviceRecording.CallStartAMPM, selector, formName);

            errorControlBlock = angular.element("#endTimeContainer");
            errorControl = angular.element("#endTimeFutureError");
            formControl = $scope.ctrl.serviceRecordingForm.endTime;
            dateControl = $filter('formatDate')($scope.serviceRecording.CallEndDate, 'MM/DD/YYYY');
            datePart = $filter('formatDate')(dateControl, 'MM/DD/YYYY');
            isRequired = formControl ? formControl.$error.required : false;
            isDate = formControl ? formControl.$error.date : false;
            selector = END_TIME_STR;
            if (!isRequired || (isRequired && isDate))
                dateTimeValidatorService.validateFutureDateTime(errorControlBlock, errorControl, formControl, dateControl, $scope.serviceRecording.CallEndTime, $scope.serviceRecording.CallEndAMPM, selector, formName);
        }
    };

    $scope.calcDuration = function () {
        //Clear out the duration at the beginning so that if its an invalid date or duration is negative because of changes we do not retain the old calculated duration in the model
        $scope.serviceRecording.Duration = '';
        $scope.validateProviderFutureDate();
        try {
            var start = formatTimeToDate($scope.serviceRecording.CallStartDate, $scope.serviceRecording.CallStartTime, $scope.serviceRecording.CallStartAMPM, START_TIME_STR);
            var end;
            if ($scope.serviceRecording.CallEndDate)
                end = formatTimeToDate($scope.serviceRecording.CallEndDate, $scope.serviceRecording.CallEndTime, $scope.serviceRecording.CallEndAMPM, END_TIME_STR);
            if (start && end) {
                $scope.serviceRecording.Duration = calculateDuration(start, end);
            }
        }
        catch (err) {
            $scope.serviceRecording.Duration = '';
        };
    };

    var formatTimeToDate = function (dateVal, timeVal, AMPM, timeSelector) { //format should be hh:mm tt
        if (dateVal == null)
            return;

        var d = new Date(dateVal);
        timeVal = timeVal || '';
        if (timeVal.indexOf(':') == -1) {
            timeVal = timeVal.substring(0, 2) + ':' + timeVal.substring(2, timeVal.length);
        }
        var hr = timeVal.substring(0, timeVal.indexOf(':'));
        var min = timeVal.substring(timeVal.indexOf(':') + 1, timeVal.length);
        //Validate the Start/End Time
        validateTime(hr, min, timeSelector);

        if (AMPM == "PM" && hr != 12)      //checks if PM, adds 12 hours
            hr = +hr + +12;
        if (AMPM == "AM" && hr == 12)      //checks if PM, adds 12 hours
            hr = +hr - +12;

        //Return the Start/End DateTime
        return new Date(d.setHours(hr, min));
    };

    var validateTime = function (hour, minute, timeSelector) {
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

    //Set the form validations
    var setFormValidations = function (elemSelector, error, isValid) {
        if ($scope.ctrl.serviceRecordingForm && $scope.ctrl.serviceRecordingForm[elemSelector] && !$scope.ctrl.serviceRecordingForm[elemSelector].$error.required) {
            $scope.ctrl.serviceRecordingForm[elemSelector].$setValidity(error, isValid);
            $scope.ctrl.serviceRecordingForm[elemSelector].$invalid = !isValid;
            $scope.ctrl.serviceRecordingForm[elemSelector].$valid = isValid;
        }
    };

    var millisToMinutesAndSeconds = function (millis) {
        var minutes = Math.floor(millis / 60000);
        var seconds = ((millis % 60000) / 1000).toFixed(0);
        return minutes + " mins " + (seconds < 10 ? '0' : '') + seconds + " secs ";
    }

    init();

}]);
