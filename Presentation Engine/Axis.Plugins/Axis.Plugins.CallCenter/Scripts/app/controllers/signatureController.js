(function () {
    angular.module('xenatixApp')
        .controller('signatureController',
            ['$scope', 'alertService', '$stateParams', '$rootScope', 'formService', '$state', 'navigationService', 'userCredentialService', 'eSignatureService', 'DocumentTypeID', '$q', '$filter', 'credentialSecurityService', 'lookupService', '$timeout','helperService',
            function ($scope, alertService, $stateParams, $rootScope, formService, $state, navigationService, userCredentialService, eSignatureService, DocumentTypeID, $q, $filter, credentialSecurityService, lookupService, $timeout, helperService) {
                //TODO: make userCredentialService service to support offline
                var licenseIssueDate = 'LicenseIssueDate';
                var licenseExpirationDate = 'LicenseExpirationDate';

                var initSignature = function () {
                    $scope.signature = {
                        UserFullName: '',
                        digitalPassword: '',
                        CredentialID: null,
                        Password: null,
                        DateSigned: null
                    };
                    $scope.isServiceCoordinatorReadonly = true;
                    $scope.isCredentialReadonly = false;
                    $scope.isPasswordReadonly = false;
                    $scope.isSigned = false;
                    $scope.signatureVerified = false;
                    //Set the document ID as primary key for each module
                    if ($state.current.name.indexOf('crisisline') >= 0 || $state.current.name.indexOf('lawliaison') >= 0)
                        $scope.DocumentID = $stateParams.CallCenterHeaderID;

                    navigationService.get().then(function (response) {
                        var data = response.DataItems[0];
                        //Get UserID, digital password
                        $scope.userID = data.UserID;
                        $scope.signature.UserFullName = data.UserFullName;
                        $scope.signature.digitalPassword = data.DigitalPassword;
                        //Get User Credentials
                        userCredentialService.get(data.UserID).then(function (data) {
                            $scope.allCredentials = filterFutureOrExpiredRecords(data.DataItems, licenseExpirationDate, licenseIssueDate);
                            $scope.userCredentials = $scope.allCredentials;
                            signatureFormReset();
                        });
                    });

                    if (!$scope.isSignatureExist)
                        getSignature();
                };

                var signatureFormReset = function () {
                    $rootScope.formReset($scope.signatureForm, $scope.signatureForm.$name);
                };

                var setFormStatus = function (value) {
                    var stateDetail = {
                        stateName: $state.current.name, validationState: value ? 'valid' : 'warning'
                    };
                    $rootScope.$broadcast('rightNavigationCallCenterHandler', stateDetail);
                };

                var getSignature = function () {
                    eSignatureService.getDocumentSignatures(DocumentTypeID, $scope.DocumentID).then(function (response) {
                        setFormStatus(false);
                        if (response && response.DataItems && response.DataItems.length > 0) {
                            $scope.signature = response.DataItems[0];
                            $scope.signature.DateSigned = $filter('toMMDDYYYYDate')($scope.signature.ModifiedOn, 'MM/DD/YYYY', 'useLocal');
                            $scope.signature.UserFullName = lookupService.getText("Users", $scope.signature.EntityId);
                            $scope.isCredentialReadonly = true;
                            $scope.isPasswordReadonly = true;
                            $scope.isSigned = true;
                            setFormStatus(true);
                            signatureFormReset();
                        }
                    });
                };

                $scope.verifySignature = function () {
                    if ($scope.signature.CredentialID && $scope.signature.Password) {
                        if (hex_md5($scope.signature.Password) == $scope.signature.digitalPassword) {
                            $scope.isSigned = true;
                            $scope.signatureVerified = true;
                            $scope.isCredentialReadonly = true;
                            $scope.isPasswordReadonly = true;
                            $scope.signature.DateSigned = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal');
                        } else {
                            alertService.error('Invalid password.');
                        }
                    } else {
                        if (!$scope.signature.CredentialID)
                            alertService.error('Please fill out the Credential field.');
                        if (!$scope.signature.Password)
                            alertService.error('Please fill out the Digital Password field');
                    }
                };

                var signatureNext = function () {
                    angular.extend($stateParams, {
                        ContactID: $scope.userID,
                        CallCenterHeaderID: $scope.DocumentID
                    });
                    if ($state.current.name.indexOf('crisisline') >= 0) {
                        $scope.Goto('callcenter.crisisline.appointment', $stateParams);
                    }
                    else {
                        $scope.Goto('callcenter.lawliaison.appointment', $stateParams);
                    }
                };

                $scope.saveDSignature = function (isNext, mandatory, hasErrors, keepForm, next) {
                    var dfd = $q.defer();
                    if ($stateParams.ReadOnly && $stateParams.ReadOnly.toString().toLowerCase() == "view" && isNext) {
                        signatureNext();
                        dfd.resolve("");
                        return dfd.promise;
                    }
                    else if ($scope.signatureVerified) {     //Save signature only if verified in current session
                        var signature = {
                            DocumentId: $scope.DocumentID,      //Primary key of record against which signature will be saved
                            DocumentTypeId: DocumentTypeID,             //Type it could be CallCenter, Discharge Note etc
                            EntitySignatureId: null,                    //Should be passed NULL for D-Signature, should be evaluated with in sproc for user
                            EntityId: $scope.userID,                    //User Id
                            EntityTypeId: 1,                            //Reference Id for type of entity. Here it is for User
                            SignatureBlob: null,                         //Signature in byte format
                            ModifiedOn: helperService.getFormattedDate($scope.signature.DateSigned),
                            CredentialID: $scope.signature.CredentialID
                        };
                        eSignatureService.saveDocumentSignature(signature).then(function (response) {
                            if (response.data.ResultCode === 0) {
                                signatureFormReset();
                                setFormStatus(true);
                                alertService.success('Signature status saved successfully!');
                                dfd.resolve(response);
                                if (isNext) {
                                    $timeout(function () {
                                        signatureNext();
                                    }, 1000);
                                }
                            } else {
                                var msg = 'Error while saving signature! Please reload the page and try again.';
                                alertService.error(msg);
                                dfd.reject(msg);
                            }
                        });
                    }
                    else if (isNext)
                        signatureNext();
                    return dfd.promise;
                };

                $scope.hasPermission = false;

                $scope.checkPermission = function () {
                    var credentialList = lookupService.getLookupsByType('Credential');
                    var result = credentialList.filter(
                                        function (obj, value) {
                                            return (obj.CredentialID == $scope.signature.CredentialID);
                                        }
                                    );
                    if (result && result.length > 0) {
                        var credentialName = result[0].CredentialName;                      //get credential name from selected credential id
                        var credentialActionID = CREDENTIAL_ACTION.DigitalSignature;          //Action will be Digital Signature always for current module
                        $scope.hasPermission = credentialSecurityService.hasCredentialPermission(credentialName, credentialActionID);
                    } else {
                        $scope.hasPermission = false;
                    }
                };

                initSignature();
            }]);
}());
