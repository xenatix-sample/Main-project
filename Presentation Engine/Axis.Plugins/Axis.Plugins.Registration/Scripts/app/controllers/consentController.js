angular.module('xenatixApp')
    .controller('consentController', [
        '$scope', '$state', '$modal', 'consentService', 'registrationService', 'alertService', '$stateParams', '$filter', 'navigationService', function($scope, $state, $modal, consentService, registrationService, alertService, $stateParams, $filter, navigationService) {
            $scope.OnTopazReady = function(value) {
                if (value === true) {
                    $scope.topazModel.Init();
                    $scope.topazModel.ImageCallback = $scope.sigImageCallback;
                    $scope.topazModel.NoPointsCallback = $scope.noPointsCallback;
                }
            };

            $scope.Init = function() {
                $scope.ContactID = $stateParams.ContactID;
                $scope.errmsg = '';
                $scope.consentModel = {
                    SignatureId: 0,
                    SignatureBlob: '',
                    ContactID: $scope.ContactID,
                    IsActive: true
                };
                $scope.reportModel = {
                    clientSigUrl: '',
                    clientSigDate: ''
                    //staffSigDate: ''
                };
                $scope.Get();

                $scope.isTopazReady = false;

                $scope.topazModel = {
                    modelNumber: '',
                    b64ImageData: '',
                    DeviceMessage: 'You do not have application permissions to electronically sign!'
                };

                $scope.$watch('isTopazReady', $scope.OnTopazReady);
            };

            $scope.noPointsCallback = function() {
                alertService.error("Please sign before submitting the form");
            };

            $scope.sigImageCallback = function(str) {
                $scope.consentModel.SignatureBlob = str;
                consentService.addConsentSignature($scope.consentModel)
                    .then(
                        function(response) {
                            var data = response.data;
                            if (data.ResultCode != 0) {
                                alertService.error('Error while saving the signature');
                            } else {
                                if (data.ResultMessage === 'OFFLINE') {
                                    $scope.consentModel.ModifiedOn = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal');
                                    navigationService.get().then(function(data) {
                                        $scope.consentModel.AuthorizedBy = data.DataItems[0].UserFullName;
                                        consentService.addConsentSignature($scope.consentModel).then(function() {
                                            $scope.Get();
                                        });
                                    });
                                } else {
                                    $scope.Get();
                                }
                                alertService.success('The consent form has been saved');
                            }

                            $scope.topazModel.Clear();
                        },
                        function(errorStatus) {
                            alertService.error('Error while saving the consent form');
                        },
                        function(notification) {
                            alertService.warning(notification);
                        });
            };

            $scope.Get = function() {
                if ($scope.ContactID != null) {
                    consentService.getConsentSignature($scope.ContactID).then(function(data) {
                            $scope.topazModel.b64ImageData = '';
                            if (data.ResultCode != 0) {
                                $scope.errmsg = 'The consent form is not available until registration has been completed';
                            } else {
                                $scope.DisplayForm = true;
                                if (data.DataItems.length > 0) {
                                    $scope.topazModel.b64ImageData = data.DataItems[0].SignatureBlob;
                                    $scope.ModifiedOn = data.DataItems[0].ModifiedOn;
                                    $scope.AuthorizedBy = data.DataItems[0].AuthorizedBy;
                                    $scope.ContactName = data.DataItems[0].ContactName;
                                    $scope.ContactDateofBirth = data.DataItems[0].ContactDateofBirth;
                                    if ($scope.topazModel.b64ImageData.length > 0) {
                                        $scope.reportModel = {
                                            clientSigDate: $filter('toMMDDYYYYDate')(new Date($scope.ModifiedOn), 'MM/DD/YYYY', 'useLocal'),
                                            clientSigUrl: $scope.topazModel.b64ImageData
                                        };
                                    }
                                }
                                if (data.ResultMessage === 'OFFLINE') {
                                    registrationService.get($scope.ContactID).then(function(regdata) {
                                        if ((regdata.DataItems && regdata.DataItems.length === 1)) {
                                            if ($scope.ContactDateofBirth != regdata.DataItems[0].DOB)
                                                $scope.ContactDateofBirth = regdata.DataItems[0].DOB;
                                            if ($scope.ContactName != (regdata.DataItems[0].FirstName + ' ' + regdata.DataItems[0].LastName)) // Name format??
                                                $scope.ContactName = regdata.DataItems[0].FirstName + ' ' + regdata.DataItems[0].LastName;
                                        }
                                    });
                                }
                                if ( !$scope.AuthorizedBy || ($scope.AuthorizedBy === '')) {
                                    navigationService.get().then(function(data) {
                                        $scope.AuthorizedBy = data.DataItems[0].UserFullName;
                                    });
                                }
                                if (!$scope.ModifiedOn || ($scope.ModifiedOn === '')) {
                                    $scope.ModifiedOn = new Date();
                                }
                            }
                        },
                        function(errorStatus) {
                            alertService.error('Error while retrieving the signature');
                        });
                }
            };

            $scope.openPrintModal = function() {
                $("#consentFormModal .modal-body").html('');
                $("#consentFormModal .modal-body").html($("<center>").html($("#consentReport").parent().html()));
            };

            $scope.closePrintModal = function() {
                $('#consentFormModal').modal('hide');
            };

            $scope.Init();
        }
    ]);