(function () {
angular.module("xenatixApp")
    .controller("callCenterQuickRegistrationController", ["$scope", "$stateParams", "$rootScope", "$q", "$filter", "lookupService", "registrationService", "scopesService", function ($scope, $stateParams, $rootScope, $q, $filter, lookupService, registrationService, scopesService) {
        $scope.isDuplicateCheckRequired = true;
        var duplicateContactsTable = $("#duplicateContactsTable");

        var formReset = function () {
            if ($scope.ctrl.quickRegForm)
                $rootScope.setform(false, $scope.ctrl.quickRegForm.$name);
        };


        $scope.openFlyout = function () {
            formReset();
            $stateParams.checkDefaultForm = true;
            $('.row-offcanvas').addClass('active');
            // set quick registration as a default form
            $rootScope.defaultFormName = 'ctrl.quickRegForm';
            $scope.noRaceDetailResults = false;

            if ($stateParams.CallCenterHeaderID) {
                $scope.get($stateParams.ContactID).then(function (data) {
                     formReset();
                });
            }
            else {
                clearQuickRegFormFields();
                FillQuickRegFormDetails();
            }
        };

        var clearQuickRegFormFields = function () {
            $scope.selectedRaceDetails = [];
            $scope.additionalDemographicQuickReg.EthnicityID = null;
            $scope.newDemographyQuickReg.DOB = null;
            $rootScope.formReset($scope.ctrl.quickRegForm, $scope.ctrl.quickRegForm.$name);
        }

        //Fill the Flyout Quick Registration form fields
        var FillQuickRegFormDetails = function () {
            var clientDetails;
            var phoneDetails;
            var fromFieldName;
            
            //Declare the Mapping fields for Client Details
            var mappingClientFields = [
                { LawLiaisonFromField: 'FirstName', CrisisLineFromField: 'FirstName', ToField: 'FirstName' },
                { LawLiaisonFromField: 'LastName', CrisisLineFromField: 'LastName', ToField: 'LastName' },
                { LawLiaisonFromField: 'dobDateName', CrisisLineFromField: 'DOB', ToField: 'DOB' },
                { LawLiaisonFromField: 'gender', CrisisLineFromField: 'GenderID', ToField: 'GenderID' },
                { LawLiaisonFromField: 'SSN', CrisisLineFromField: 'SSN', ToField: 'SSN' }
            ];

            //Declare the Mapping fields for Phone Details
            var mappingPhoneFields = [
                { LawLiaisonFromField: 'PhoneNumber', CrisisLineFromField: 'Number', ToField: 'Number' },
                { LawLiaisonFromField: 'PhoneTypeID', CrisisLineFromField: 'PhoneTypeID', ToField: 'PhoneTypeID' }
            ];

            //Fill the Details from the shared scope service
            clientDetails = scopesService.get($scope.isCrisisLine ? CRISISLINE_QUICK_REG_DATA.ClientData : LAWLIAISON_QUICK_REG_DATA.ClientData);
            phoneDetails = scopesService.get($scope.isCrisisLine ? CRISISLINE_QUICK_REG_DATA.PhoneData : LAWLIAISON_QUICK_REG_DATA.PhoneData);

            //Fill the Client Details
            if (clientDetails) {
                angular.forEach(mappingClientFields, function (mappingField) {
                    fromFieldName = $scope.isCrisisLine ? mappingField.CrisisLineFromField : mappingField.LawLiaisonFromField;

                    if (clientDetails.hasOwnProperty(fromFieldName)) {
                        if (mappingField.ToField.toUpperCase() === 'DOB') {
                            if (!$scope.isCrisisLine) {
                                var tempDate = getModelValue(clientDetails, fromFieldName);
                                clientDetails[fromFieldName].$modelValue = checkModel(tempDate) && moment(tempDate).isValid() ? tempDate : null;
                            }
                            else if($scope.isCrisisLine && clientDetails[fromFieldName]){
                                clientDetails[fromFieldName] = $filter('formatDate')(clientDetails[fromFieldName]);
                            }
                        }

                        $scope.newDemographyQuickReg[mappingField.ToField] = getModelValue(clientDetails, fromFieldName);
                    }
                });
            }
            
            //Fill the Phone Details
            if (phoneDetails) {
                $scope.phone = {};
                angular.forEach(mappingPhoneFields, function (mappingField) {
                    fromFieldName = $scope.isCrisisLine ? mappingField.CrisisLineFromField : mappingField.LawLiaisonFromField;

                    if (phoneDetails.hasOwnProperty(fromFieldName)) {
                        $scope.phone[mappingField.ToField] = getModelValue(phoneDetails, fromFieldName);
                    }
                });
            }
        };

        //Get the property value
        var getModelValue = function(modelObj, propName) {
            return typeof modelObj[propName] === 'object' && modelObj[propName]
                        ? (checkModel(modelObj[propName].$modelValue) ? modelObj[propName].$modelValue : null)
                        : modelObj[propName];
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

                columns: [
                    {
                        field: 'FirstName',
                        title: 'First Name'
                    },
                    {
                        field: 'LastName',
                        title: 'Last Name'
                    },
                    {
                        field: 'Middle',
                        title: 'Middle Name'
                    },
                     {
                         field: 'GenderText',
                         title: 'Gender'
                     },
                    {
                        field: 'DOB',
                        title: 'DOB',
                        formatter: function (value, row, index) {
                            if (value) {
                                var formattedDate = $filter('formatDate')(value, 'MM/DD/YYYY');
                                return formattedDate;
                            } else
                                return '';
                        }
                    },
                    {
                        field: 'SSN',
                        title: 'SSN',
                        formatter: function (value, row, index) {
                            var formattedSNN = $filter('toMaskSSN')(value);
                            return formattedSNN;
                        }
                    },
                    {
                        field: 'ContactID',
                        title: 'Actions',
                        formatter: function (value, row, index) {
                            return '<a href="javascript:void(0)" data-default-action data-ng-click="populateContact(' + value + ')" alt="View Contact" security permission-key="Registration-Registration-Demographics" permission="update" title="Edit"><i class="fa fa-plus-circle fa-fw padding-left-small padding-right-small" /></a>';
                        }
                    }
                ]
            };
        };

        $scope.initializeBootstrapTable();

        $scope.cancelModal = function () {
            $('#duplicateContactListModel').modal('hide');
        };

        $scope.populateContact = function (contactID) {
            $scope.ctrl.quickRegForm.SSN.$pristine = true;;
            $scope.get(contactID).finally(function () {
                $('#duplicateContactListModel').modal('hide');
                $('#FirstName').focus();
                $scope.newDemographyQuickReg.ContactID = contactID;
                $scope.setShortcutKey(false, false, false, false);
            })
        };

        $scope.continueWithRegistration = function () {
            $('#duplicateContactListModel').modal('hide');
            $scope.isDuplicateCheckRequired = false;
            $scope.register(false, true, false);
        };

        var bindDataModel = function (model, showCurrentUser) {
            var listToBind = model;
            angular.forEach(listToBind, function (contact) {
                contact.DOB = contact.DOB ? $filter('formatDate')(contact.DOB, 'MM/DD/YYYY') : "";
                if (contact.GenderID > 0)
                    contact.GenderText = lookupService.getSelectedText('Gender', contact.GenderID)[0].Name;
            });
            return listToBind;
        };

        $scope.verifyDuplicateContacts = function () {
            var deferred = $q.defer();
            if ($scope.isDuplicateCheckRequired == true) {
                registrationService.verifyDuplicateContacts($scope.newDemographyQuickReg).then(function (data) {
                    return deferred.resolve(data);
                }, function (errorStatus) {
                    alertService.error('Unable to connect to server');
                });
            }
            else
                deferred.resolve();
            return deferred.promise;
        };

        $scope.showPotentialDuplicates = function (responseDetail) {
            $scope.duplicateContactList = bindDataModel(responseDetail, false);
            if ($scope.duplicateContactList != null && $scope.duplicateContactList.length > 0) {
                duplicateContactsTable.bootstrapTable('load', $scope.duplicateContactList);
                $('#duplicateContactListModel').modal('show');
                $('#duplicateContactListModel').on('shown.bs.modal', function () {
                    $scope.setShortcutKey(true, false, false, true);
                    $rootScope.setFocusToGrid('duplicateContactsTable');
                });
            }
            else {
                duplicateContactsTable.bootstrapTable('removeAll');
            }
        };
    }]);
})();