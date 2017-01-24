angular.module('xenatixApp')
    .controller('userDigitalSignatureController', [
        '$http', '$scope', '$q', '$state', '$rootScope', '$filter', '$injector', '$stateParams', 'lookupService', 'userSecurityService', 'userProfileService', 'alertService', 'eSignatureService', 'formService', '$timeout', 'navigationService',
function ($http, $scope, $q, $state, $rootScope, $filter, $injector, $stateParams, lookupService, userSecurityService, userProfileService, alertService, eSignatureService, formService, $timeout, navigationService) {

                $scope.securityQuestions = [];
                $scope.entitySignatureID = 0;
                $scope.signatureID = 0;
                $scope.signatureBLOB = '';
                $scope.enableChangeSecurityQuestionButton = false;
                // Topaz init
                $scope.OnTopazReady = function (value) {
                    if (value === true) {
                        $scope.topazModel.Init();
                        $scope.topazModel.ImageCallback = $scope.sigImageCallback;
                        $scope.topazModel.NoPointsCallback = $scope.noPointsCallback;
                    }
                };
                $scope.isTopazReady = false;
                $scope.topazModel = {
                    modelNumber: '',
                    b64ImageData: '',
                    DeviceMessage: 'You do not have application permissions to electronically sign!',
                    hideTopazModel: false,
                    hideSignatureCanvas: false,
                    hideSignatureBtns: false
                };
                $scope.$watch('isTopazReady', $scope.OnTopazReady);

                // Get entity type for user
                $scope.entityType = $filter('filter')(lookupService.getLookupsByType('EntityType'), { EntityType: 'User' }, true)[0].EntityTypeID;

                resetForm = function () {
                    $rootScope.formReset($scope.ctrl.userDigitalSignatureForm);
                    $rootScope.formReset($scope.ctrl.roleForm);
                };

            $scope.get = function () {
                return userProfileService.get(true).then(function (response) {
                    $scope.isTempPassword = false;
                    if (response && response.ResultCode == 0 && response.DataItems.length > 0) {
                        $scope.userID = response.DataItems[0].UserID;
                        $scope.userProfile = response.DataItems[0];
                        if ($scope.userProfile.IsTemporaryPassword === false || $scope.userProfile.ADFlag) {
                            //disable all password fields and display the enable button
                            $scope.enablePasswordsButton = true;                            
                        }                         
                        else {
                            $scope.isTempPassword = true;
                            alertService.warning('Please create a permanent password and enter security questions!');
                        }
                        if ($scope.userProfile.ADFlag) { $scope.enableChangePasswordButton = false; }

                            // disable all digital pwd fields and enable the digital pwd button by default
                            $scope.enableDigitalPasswordsButton = true;

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
                            if ($scope.isTempPassword === false) {
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

                            if ($scope.isTempPassword === true && $scope.securityQuestions.length === 0 && !$scope.userProfile.ADFlag) {
                                alertService.warning('Please create a permanent password and enter security questions');
                            }
                            //$scope.userProfile.getSecurityQuestions is not defined for first time login when isTempPassword is true
                            if ($scope.isTempPassword === true && !$scope.userProfile.ADFlag) {
                                alertService.warning('Please create a permanent password');
                            }

                        } else {
                            alertService.warning('Please enter security questions');
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
                        $scope.getTopazSig();                        
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
                    resetForm();
                };

                $scope.enablePasswordFields = function () {
                    if ($scope.userProfile.ADFlag) {
                        alertService.info($scope.userProfile.ADUserPasswordResetMessage);
                    } else {
                        $scope.enablePasswordsButton = false;
                    }
                };

                $scope.enableDigitalPasswordFields = function () {
                    if ($scope.securityQuestions.length == 0) {
                        bootbox.alert('Please answer and save your security questions first.');
                        return;
                    }
                    var len = $scope.securityQuestions.length;
                    var idx = Math.floor(Math.random() * (len - 1)) + 0;
                    $scope.selectedSecurityQuestionID = $scope.securityQuestions[idx].SecurityQuestionID;
                    $scope.userProfile.ConfirmDigitalPasswordNonEncrypt = '';
                    $scope.userProfile.NewDigitalPasswordNonEncrypt = '';
                    // Lookup questin text
                    $scope.selectedSecurityQuestion = $filter('filter')(lookupService.getLookupsByType('SecurityQuestion'), { ID: $scope.selectedSecurityQuestionID }, true)[0].Name;
                    $('#securityQestionVerifyModal').modal('show');
                    $rootScope.defaultFormName = $scope.ctrl.roleForm.$name;
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
                            $rootScope.formReset($scope.ctrl.roleForm, $scope.ctrl.roleForm.$name);
                            //resetForm();
                            // defect 10452, answering security questions will allow users who do not know their current digital password, to change it
                        }
                        else {
                            alertService.error('Error verifying security answer! Please try again.');
                            $scope.selectedSecurityAnswer = '';
                        }
                    },
                    function (errorSttaus) {
                        alertService.error('Error verifying security answer! Please try again.');
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
                    if ($('#securityQestionVerifyModal').css('display') != 'none')
                        return $scope.veriftySecurityQuestion();

                    if (!mandatory && isNext && hasErrors) {
                        $scope.postSave(isNext);
                    }
                    //[bug:12254]. user form name directly instead of form service.
                    if (!formService.isDirty($scope.ctrl.userDigitalSignatureForm.$name) && isNext && !hasErrors) {
                        $scope.postSave(isNext);
                    }
                    if (formService.isDirty($scope.ctrl.userDigitalSignatureForm.$name) && !hasErrors) {

                        // Make sure current digital password is specified IFF new digital pwd fields are filled AND a digital pwd already exists
                        if ($scope.userProfile.NewDigitalPasswordNonEncrypt && $scope.userProfile.ConfirmDigitalPasswordNonEncrypt &&
                            $scope.disableCurrentDigitalPassword == false && ($scope.userProfile.CurrentDigitalPassword == '' || $scope.userProfile.CurrentDigitalPassword == null)) {
                            alertService.error('Please specify the current digital password');
                            $('#currentDigitalPassword').focus();
                            isValid = false;
                        }

                        //validate that the digital password fields are matching
                        if ($scope.userProfile.NewDigitalPasswordNonEncrypt !== $scope.userProfile.ConfirmDigitalPasswordNonEncrypt) {
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
                                if ($scope.userProfile.NewDigitalPasswordNonEncrypt && $scope.userProfile.NewDigitalPasswordNonEncrypt != '')
                                    $scope.userProfile.NewDigitalPassword = hex_md5($scope.userProfile.NewDigitalPasswordNonEncrypt);
                                if ($scope.userProfile.ConfirmDigitalPasswordNonEncrypt && $scope.userProfile.ConfirmDigitalPasswordNonEncrypt != '')
                                    $scope.userProfile.ConfirmDigitalPassword = hex_md5($scope.userProfile.ConfirmDigitalPasswordNonEncrypt);
                                var dfd = $q.defer();
                                var promiseArray = [];

                                promiseArray.push(userSecurityService.updateUserSignatureDetails($scope.userProfile));
                                var questions = $filter('filter')($scope.questionAnswers, function (item) {
                                    if (item.SecurityAnswer != '******')
                                        return item;
                                });
                                if (questions.length > 0) {
                                    promiseArray.push(userSecurityService.save(questions));
                                }

                                $q.all(promiseArray).then(function (servicesData) {
                                    dfd.resolve(servicesData)
                                    alertService.success('Digital signature updated successfully.');
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
                        navigationService.get(true);
                        $scope.handleNextState();
                    } else {
                        $scope.get().then(function () {
                            $scope.getQuestions();
                            navigationService.get(true);
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

                // Topaz callback methods
                $scope.noPointsCallback = function () {
                    $scope.save(false, true);
                };

                $scope.sigImageCallback = function (str) {
                    $scope.signatureBLOB = str;
                    var entitySignature = { SignatureBlob: str };
                    eSignatureService.addSignature(entitySignature)
                            .then(
                                function (response) {
                                    if (response.ResultCode != 0) {
                                        alertService.error('Error while creating a signature');
                                    } else {

                                        // Set the signature id and add/update entity signature
                                        $scope.signatureID = response.ID;
                                        $scope.addEntitySignature($scope.entitySignatureID != 0);
                                    }
                                },
                                function (errorStatus) {
                                    alertService.error('Error while creating a signature');
                                },
                                function (notification) {
                                    alertService.warning(notification);
                                });
                };

                $scope.addEntitySignature = function (isUpdate) {
                    if (!isUpdate) {
                        eSignatureService.addEntitySignature($scope.getEntitySignatureModel(isUpdate))
                           .then(
                               function (response) {
                                   if (response.ResultCode != 0) {
                                       alertService.error('Error while updating the electronic signature');
                                   } else {

                                       alertService.success('Electronic Signature added successfully.');
                                       $scope.entitySignatureID = response.ID;
                                       $scope.topazModel.Clear();
                                       $scope.topazModel.hideSignatureCanvas = true;
                                       $scope.topazModel.hideTopazModel = true;
                                       $scope.topazModel.hideSignatureImage = true;
                                       $scope.getTopazSig();
                                   }
                               },
                               function (errorStatus) {
                                   alertService.error('Error while updating the electronic signature');
                               },
                               function (notification) {
                                   alertService.warning(notification);
                               });
                    }
                    else {
                        eSignatureService.updateEntitySignature($scope.getEntitySignatureModel(isUpdate))
                           .then(
                               function (response) {
                                   if (response.ResultCode != 0) {
                                       alertService.error('Error while updating the electronic signature');
                                   } else {

                                       alertService.success('Electronic Signature updated successfully.');
                                       $scope.topazModel.Clear();
                                       $scope.topazModel.hideSignatureCanvas = true;
                                       $scope.topazModel.hideTopazModel = true;
                                       $scope.topazModel.hideSignatureImage = true;
                                       $scope.getTopazSig();
                                   }
                               },
                               function (errorStatus) {
                                   alertService.error('Error while updating the electronic signature');
                               },
                               function (notification) {
                                   alertService.warning(notification);
                               });
                    }
                }

                $scope.getTopazSig = function () {
                    if ($scope.userID != null) {
                        eSignatureService.getEntitySignature($scope.userID, $scope.entityType, 1).then(function (data) {
                            $scope.topazModel.b64ImageData = '';
                            if (data.ResultCode != 0) {
                                $scope.errmsg = 'Error while retrieving the electronic signature';
                            } else {
                                $scope.DisplayForm = true;
                                if (data.DataItems.length > 0) {
                                    $scope.topazModel.b64ImageData = data.DataItems[0].SignatureBlob;
                                    $scope.signatureID = data.DataItems[0].SignatureId;
                                    $scope.entitySignatureID = data.DataItems[0].EntitySignatureId;

                                    // We have a signature to show, hide the topaz model and canvas until needed
                                    $scope.topazModel.hideSignatureImage = false;
                                    $scope.topazModel.hideSignatureBtns = false;
                                    $scope.topazModel.hideSignatureCanvas = true;
                                    $scope.topazModel.hideTopazModel = true;
                                }
                            }
                            $rootScope.formReset($scope.ctrl.userDigitalSignatureForm.signatureCanvasForm, $scope.ctrl.userDigitalSignatureForm.signatureCanvasForm.$name);
                        },
                            function (errorStatus) {
                                alertService.error('Error while retrieving the electronic signature');
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

                $scope.init();
            }
    ]);
