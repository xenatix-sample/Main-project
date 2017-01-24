(function() {
angular.module('xenatixApp').
    directive('xenDigitalSignature', ['$q', '$rootScope', '$filter', 'alertService', 'navigationService', 'eSignatureService', 'credentialSecurityService', 'lookupService',
        function ($q, $rootScope, $filter, alertService, navigationService, eSignatureService, credentialSecurityService, lookupService) {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    signatureModel: '=',                //Model to populate signature details
                    credentialActionForm: '@',          //Current form name for which credentials are to be populated
                    isConditionalRequired: '@',         //true to show †, by default it will show *
                    serviceId: '='                      //Need to pass if credentials will be filtered by service selected on screen. By default credentials dropdown will remain empty
                },
                templateUrl: '/Scripts/app/Template/DigitalSignature.html',
                link: function (scope, ele, attrs) {
                    //variable need to set or replace
                    //isInactive                Set to true when signed and hide password and show credential as label(Only discharge uses the pattern now)
                    //isPasswordHide            Set to true when need to hide password(Only discharge uses the pattern now)
                    //isCredentialReadonly      Removed, replaced with signatureModel.isSigned
                    //isConditionalRequired     Done
                    //isPasswordReadonly        Removed, replaced with signatureModel.isSigned
                    //hasPermission             Done

                    //when passing signatureModel should have following default values:
                    //DocumentId
                    //DocumentTypeId

                    var userDigitalPassword;
                    var credentialList;
                    var resetDigitalSign = function () {
                        if (scope.signatureForm)
                            $rootScope.formReset(scope.signatureForm);
                    }
                    var initSignature = function () {
                        var dfd = $q.defer();
                        var promiseArr = [];
                        promiseArr.push(navigationService.get());
                        promiseArr.push(scope.signatureModel.DocumentId ? eSignatureService.getDocumentSignatures(scope.signatureModel.DocumentTypeId, scope.signatureModel.DocumentId) : true);

                        $q.all(promiseArr).then(function (response) {
                            if (hasData(response[0])) {
                                var navigationData = response[0].DataItems[0];
                                scope.signatureModel.UserFullName = navigationData.UserFullName;
                                scope.signatureModel.SignatureName = navigationData.PrintSignature;
                                userDigitalPassword = navigationData.DigitalPassword;
                                scope.signatureModel.resetForm = function () {
                                    resetDigitalSign();
                                }
                            }

                            if (hasData(response[1])) {
                                angular.extend(scope.signatureModel,response[1].DataItems[0]);
                                scope.signatureModel.DateSigned = $filter('toMMDDYYYYDate')(scope.signatureModel.ModifiedOn);
                                scope.signatureModel.UserFullName = lookupService.getText("Users", scope.signatureModel.EntityId);
                                scope.userCredentials = credentialList = lookupService.getLookupsByType('Credential');       //If signed, populate with all credentials
                                
                                scope.signatureModel.IsSigned = true;
                                dfd.resolve(true);
                            } else {      //If not signed, populate with credentials which can sign form
                                var filter = { CredentialActionForm: scope.credentialActionForm, CredentialActionID: CREDENTIAL_ACTION.DigitalSignature };
                                if (attrs.serviceId) {
                                    //don't load by default and load on selection of service
                                    scope.$watch('serviceId', function (newVal) {
                                        if (newVal) {
                                            angular.extend(filter, { ServicesID: newVal });
                                            getCredentials(filter);
                                        }
                                    });
                                    dfd.resolve(true);
                                } else {
                                    getCredentials(filter).then(function () {
                                        dfd.resolve(true);
                                    });
                                }
                            }
                        });
                        return dfd.promise;
                    };

                    var getCredentials = function (filter) {
                        var dfd = $q.defer();
                        if (credentialList) {
                            populateCredential(filter);
                            dfd.resolve(true);
                        } else {
                            credentialSecurityService.getUserCredentialSecurity().then(function (data) {
                                credentialList = data.DataItems;
                                populateCredential(filter);
                            }).finally(function () {
                                dfd.resolve(true);
                            });
                        }
                        return dfd.promise;
                    };

                    var populateCredential = function (filter) {
                        scope.userCredentials = $filter('filter')(credentialList, filter, true);
                        if (scope.userCredentials && scope.userCredentials.length == 1) {
                            scope.signatureModel.CredentialID = scope.userCredentials[0].CredentialID;
                            scope.checkPermission();
                        }
                    }

                    scope.verifySignature = function () {
                        if (scope.signatureModel.CredentialID && scope.signatureModel.Password && userDigitalPassword) {
                            if (hex_md5(scope.signatureModel.Password) === userDigitalPassword) {
                                scope.signatureModel.IsSigned = true;
                                scope.signatureModelVerified = true;
                                scope.signatureModel.DateSigned = $filter('toMMDDYYYYDate')(new Date());
                                typeof scope.signatureModel.signedCallBack == 'function' && scope.signatureModel.signedCallBack();
                            } else {
                                alertService.error('Invalid password.');
                            }
                        } else if (!userDigitalPassword) {
                            alertService.error('Before signing the document digitally, you need to save digital signature.');
                        } else {
                            if (!scope.signatureModel.CredentialID)
                                alertService.error('Please fill out the Credential field.');
                            if (!scope.signatureModel.Password)
                                alertService.error('Please fill out the Digital Password field');
                        }
                    };

                    scope.checkPermission = function () {
                        var allCredentialList = lookupService.getLookupsByType('Credential');
                        var result = allCredentialList.filter(
                                            function (obj, value) {
                                                return (obj.CredentialID == scope.signatureModel.CredentialID);
                                            }
                                        );
                        if (hasDetails(result)) {
                            var credentialName = result[0].CredentialName;                      //get credential name from selected credential id
                            var credentialActionID = CREDENTIAL_ACTION.DigitalSignature;          //Action shall be "Digital Signature" always for current module
                            scope.hasPermission = credentialSecurityService.hasCredentialActionPermissionByForm(credentialName, scope.credentialActionForm, credentialActionID);
                        } else {
                            scope.hasPermission = false;
                        }
                    };

                    initSignature().then(function () {
                        resetDigitalSign();
                    });
                }
            }
        }]);
})();