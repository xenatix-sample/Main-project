angular.module('xenatixApp')
    .controller('userSecurityController', [
        '$http', '$scope', '$q', '$state', '$rootScope', '$filter', '$injector', '$stateParams', 'lookupService', 'userSecurityService', 'userProfileService', 'alertService', 'eSignatureService', 'formService', '$timeout',
            function ($http, $scope, $q, $state, $rootScope, $filter, $injector, $stateParams, lookupService, userSecurityService, userProfileService, alertService, eSignatureService, formService, $timeout) {

                $scope.securityQuestions = [];
                $scope.entitySignatureID = 0;
                $scope.signatureID = 0;
                $scope.signatureBLOB = '';
                $scope.enableChangeSecurityQuestionButton = false;
                

                // Get entity type for user
                $scope.entityType = $filter('filter')(lookupService.getLookupsByType('EntityType'), { EntityType: 'User' }, true)[0].EntityTypeID;

                resetForm = function () {
                    $rootScope.formReset($scope.ctrl.userSecurityForm);
                    $rootScope.formReset($scope.ctrl.roleForm);
                };

            $scope.get = function () {
                return userProfileService.get(true).then(function (response) {
                    //$scope.isTempPassword = false;
                    if (response && response.ResultCode == 0 && response.DataItems.length > 0) {
                        $scope.userID = response.DataItems[0].UserID;
                        $scope.userProfile = response.DataItems[0];
                        if ($scope.userProfile.IsTemporaryPassword === false || $scope.userProfile.ADFlag) {
                            //disable all password fields and display the enable button
                            $scope.enablePasswordsButton = true;                            
                        }                         
                        else {
                            $scope.isTempPassword = true;
                        }
                        if ($scope.userProfile.ADFlag) { $scope.enableChangePasswordButton = false; }

                            //// disable all digital pwd fields and enable the digital pwd button by default
                            //$scope.enableDigitalPasswordsButton = true;

                            if ($scope.userProfile.PrintSignature == null)
                                $scope.userProfile.PrintSignature = '';

                            // If there is a current digital pwd set, ALLOW for enabling the field, disable otherwise
                            if ($scope.userProfile.CurrentDigitalPassword != '')
                                $scope.disableCurrentDigitalPassword = false;
                            else
                                $scope.disableCurrentDigitalPassword = true;
                            $scope.userProfile.CurrentDigitalPassword = '';
                            //if ($scope.userProfile.PrintSignature == '' || $scope.userProfile.CurrentDigitalPassword == '')
                            //    $scope.enableDigitalPasswordsButton = false;
                            resetForm();
                        }
                    }, function (errorStatus) {
                        alertService.error('Error while getting data: ' + errorStatus);
                    });
                };
                $scope.getQuestions = function () {
                    userSecurityService.get($scope.userID).then(function (response) {
                        if (response && response.ResultCode == 0 && response.DataItems.length > 0) {
                            if (!$scope.isTempPassword) {
                                var obj = { stateName: $state.current.name, validationState: 'valid' };
                                $rootScope.myProfileRightNavigationHandler(obj);
                            } else {
                                var obj = { stateName: $state.current.name, validationState: 'invalid' };
                                $rootScope.myProfileRightNavigationHandler(obj);
                            }
                            $scope.securityQuestions = [];
                            angular.forEach(response.DataItems, function (item, index) {
                                item.SecurityAnswer = '******';
                                $scope.securityQuestions.push(item);
                            });
                            $scope.enableSecurityQuestionButton = true;
                        } else {
                            $scope.enableSecurityQuestionButton = false;
                            var obj = { stateName: $state.current.name, validationState: 'invalid' };
                            $rootScope.myProfileRightNavigationHandler(obj);
                        }
                        resetForm();
                    }, function (error) {
                        alertService.error(error);
                    });
                }

                $scope.init = function () {
                    $scope.initUserProfile();
                    for (var i = 0; i < 2; i++) {
                        $scope.securityQuestions.push({ UserID: 0, UserSecurityQuestionID: 0, SecurityQuestionID: 0, SecurityAnswer: '' });
                    }
                    $scope.get().then(function () {
                        $scope.getQuestions();
                        $scope.getIssuesToAddressWarnings();
                        //$scope.getTopazSig();                        
                    });
                };

            $scope.initUserProfile = function () {
                $scope.UserProfile = {};
                $scope.enablePasswordsButton = false;
                $scope.enableChangePasswordButton = true;
            };

                $scope.enableSecurityQuestions = function () {
                    if ($scope.securityQuestions.ADFlag) {
                        alertService.info($scope.securityQuestions.ADUserPasswordResetMessage);
                    } else {
                        $scope.enableSecurityQuestionButton = false;
                        angular.forEach($scope.securityQuestions, function (item, index) {
                            item.SecurityAnswer = '';
                        });
                    }
                };

                $scope.enablePasswordFields = function () {
                    if ($scope.userProfile.ADFlag) {
                        alertService.info($scope.userProfile.ADUserPasswordResetMessage);
                    } else {
                        $scope.enablePasswordsButton = false;
                    }
                };
                $scope.veriftySecurityQuestion = function () {

                    eSignatureService.verifySecurityDetails($scope.getSecurityDetailsModel()).then(function (data) {
                        if (data.ResultCode == 0 && data.ID != 0) {
                            $scope.enableDigitalPasswordsButton = false;
                            $('#securityQestionVerifyModal').modal('hide');
                            $('body,html').removeClass('modal-open');
                            $rootScope.defaultFormName = getDefaultFormName();
                            $scope.selectedSecurityAnswer = '';
                            $scope.disableCurrentDigitalPassword = true;
                            // defect 10452, answering security questions will allow users who do not know their current digital password, to change it
                        }
                        else {
                            alertService.error('Error verifying security answers! Please try again.');
                            $scope.selectedSecurityAnswer = '';
                        }
                    },
                    function (errorSttaus) {
                        alertService.error('Error verifying security answers! Please try again.');
                    });
                }

                $scope.cancelSecurityVerification = function () {
                    $scope.enableDigitalPasswordsButton = true;
                    $scope.selectedSecurityAnswer = null;
                    $('#securityQestionVerifyModal').modal('hide');
                    $rootScope.defaultFormName = getDefaultFormName();
                    $rootScope.formReset($scope.ctrl.roleForm, $scope.ctrl.roleForm.$name);
                }

                $scope.save = function (isNext, mandatory, hasErrors) {
                    var isValid = true;
                    //if ($('#securityQestionVerifyModal').css('display') != 'none')
                    //    return $scope.veriftySecurityQuestion();

                    if (!mandatory && isNext && hasErrors) {
                        $scope.postSave(isNext);
                    }
                    //[bug:12254]. user form name directly instead of form service.
                    if (!formService.isDirty('ctrl.userSecurityForm') && isNext && !hasErrors) {
                        $scope.postSave(isNext);
                    }
                    if (formService.isDirty('ctrl.userSecurityForm') && !hasErrors) {

                        // Make sure current digital password is specified IFF new digital pwd fields are filled AND a digital pwd already exists
                        if ($scope.userProfile.NewDigitalPassword != '' && $scope.userProfile.ConfirmDigitalPassword != '' &&
                            $scope.disableCurrentDigitalPassword == false && ($scope.userProfile.CurrentDigitalPassword == '' || $scope.userProfile.CurrentDigitalPassword == null)) {
                            alertService.error('Please specify the current digital password');
                            $('#currentDigitalPassword').focus();
                            isValid = false;
                        }

                        //validate that the digital password fields are matching
                        if ($scope.userProfile.NewDigitalPassword !== $scope.userProfile.ConfirmDigitalPassword) {
                            alertService.error('The Digital password fields must match');
                            $('#newDigitalPassword').focus();
                            isValid = false;
                        }

                        //validate that the password fields are matching
                        if ($scope.userProfile.NewPassword !== $scope.userProfile.ConfirmPassword) {
                            alertService.error('The password fields must match');
                            $('#newPassword').focus();
                            isValid = false;
                        }

                        //Current Password cannot be same as New Password
                        if ($scope.userProfile.CurrentPassword && $scope.userProfile.NewPassword && ($scope.userProfile.CurrentPassword == $scope.userProfile.NewPassword)) {
                            alertService.error('New password can not be same as current password');
                            $('#newPassword').focus();
                            isValid = false;
                        }

                        $scope.questionAnswers = $scope.securityQuestions;
                        if ($scope.userProfile.PrintSignature == null)
                            $scope.userProfile.PrintSignature = '';
                        for (var i = 0; i < $scope.questionAnswers.length; i++) {
                            var duplicate = $filter('filter')($scope.questionAnswers, { SecurityQuestionID: $scope.questionAnswers[i].SecurityQuestionID }, true);
                            if (duplicate.length > 1) {
                                alertService.error('You must select two different security questions');
                                isValid = false;
                                break;
                            }
                        }

                        if (!isValid) {
                            return false;
                        }
                        
                        userSecurityService.saveUserPassword($scope.userProfile).then(function (data) {
                            if (data.ResultCode == 0) {
                        if ($scope.userProfile.CurrentDigitalPassword && $scope.userProfile.CurrentDigitalPassword != '')
                            $scope.userProfile.CurrentDigitalPassword = hex_md5($scope.userProfile.CurrentDigitalPassword);
                        if ($scope.userProfile.NewDigitalPassword && $scope.userProfile.NewDigitalPassword != '')
                            $scope.userProfile.NewDigitalPassword = hex_md5($scope.userProfile.NewDigitalPassword);
                        if ($scope.userProfile.ConfirmDigitalPassword && $scope.userProfile.ConfirmDigitalPassword != '')
                            $scope.userProfile.ConfirmDigitalPassword = hex_md5($scope.userProfile.ConfirmDigitalPassword);
                                var dfd = $q.defer();
                                var promiseArray = [];

                                //promiseArray.push(userSecurityService.updateUserSignatureDetails($scope.userProfile));
                                var questions = $filter('filter')($scope.questionAnswers, function (item) {
                                    if (item.SecurityAnswer != '******')
                                        return item;
                                });
                                if (questions.length > 0) {
                                    promiseArray.push(userSecurityService.save(questions));
                                }

                                $q.all(promiseArray).then(function (servicesData) {
                                    dfd.resolve(servicesData)
                                    alertService.success('Security settings updated successfully');
                                    $scope.postSave(isNext);
                                }, function (errorMessage) {
                                    alertService.error(errorMessage);
                                    dfd.resolve(null);
                         });
                                return dfd.promise;
                            }
                            else {
                                    alertService.error(data.ResultMessage);
                                return false;
                            }
                        },
                         function (errorStatus) {
                             alertService.error('Error while saving data: ' + errorStatus);
                             return false;
                         });
                    }

                };

                $scope.postSave = function (isNext) {
                    $('#securityQestionVerifyModal').modal('hide');
                    $('body,html').removeClass('modal-open');
                    if (isNext) {
                        $scope.handleNextState();
                    } else {
                        $scope.get().then(function () {
                            $scope.getQuestions();
                            $scope.getIssuesToAddressWarnings();
                            //$scope.getTopazSig();
                        });
                    }
                };

                $scope.handleNextState = function () {
                    var nextState = angular.element("li[data-state-name].list-group-item.active").next("li[data-state-name].list-group-item");
                    resetForm();
                    if (nextState.length === 0) {
                        $scope.Goto('^');
                    }
                    else {
                        $timeout(function () {
                            $rootScope.setform(false);
                            var nextStateName = nextState.attr('data-state-name');
                            $scope.Goto(nextStateName, { UserID: $scope.userID });
                        });
                    }
                };
               
                $scope.getSecurityDetailsModel = function () {
                    var model = {};
                    model.SecurityQuestionID = $scope.selectedSecurityQuestionID;
                    model.SecurityAnswer = $scope.selectedSecurityAnswer;
                    model.UserID = $scope.userID;
                    model.IsDigitalPassword = true;
                    return model;
                }

                $scope.getEntitySignatureModel = function (isUpdate) {
                    $scope.sigModel = {};
                    $scope.sigModel.SignatureID = $scope.signatureID;
                    $scope.sigModel.EntityTypeId = $scope.entityType;
                    $scope.sigModel.EntityID = $scope.userID;
                    $scope.sigModel.SignatureBlob = '';
                    $scope.sigModel.SignatureTypeId = 1;
                    if (isUpdate)
                        $scope.sigModel.EntitySignatureID = $scope.entitySignatureID;
                    // TODO: Other stuff here
                    return $scope.sigModel;
                }

                $scope.getIssuesToAddressWarnings = function () {
                    //We need to document these requirements better plus Kishan is working on a PBI related to this which is changing the logic.
                    //If Is Temporary Password force to change the password
                    //If Security Questions not set then force to set Questions
                    if ($scope.userProfile.IsTemporaryPassword === true && !$scope.userProfile.SecurityQuestionID1 && !$scope.userProfile.SecurityQuestionID2 && !$scope.userProfile.ADFlag) {
                        alertService.warning('Please create a permanent password and enter security questions');
                    }
                    else if (!$scope.userProfile.SecurityQuestionID1 && !$scope.userProfile.SecurityQuestionID2) {
                        alertService.warning('Please enter security questions');
                    }
                }

                $scope.init();
            }
    ]);