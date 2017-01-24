(function () {
    angular.module('xenatixApp')
    .directive('flyoutRecordedServiceVoid', function ($compile) {

        var voidServiceCtrl = function (alertService, voidService, $rootScope, $stateParams, assessmentService, formService, $state, $filter) {
            var vs = this;

            vs.reasonStatusDataEntry = 2; //data entry
            vs.voidRecordedServiceReasons = $rootScope.getLookupsByType("VoidRecordedServiceReason");
            vs.PERMISSION = PERMISSION.CREATE;
            vs.closeFlyout = function () {
                if (vs.voidServiceForm.$dirty) {
                    bootbox.confirm("Any unsaved data will be lost. Do you want to continue?", function (result) {
                        if (result == true) {
                            closeVoidedFlyout();
                        }
                    });
                }
                else
                    closeVoidedFlyout();
            };

            var closeVoidedFlyout = function () {
                angular.element('.row-offcanvas').removeClass('active');
                //instead of reninitializing voidmodel just change the model property itself.
                //vs.voidModel = {
                //    IsCreateCopyToEditHide: ($state.current.name.indexOf('lawliaison') >= 0)
                //};
                vs.voidModel.IsCreateCopyToEditHide = ($state.current.name.indexOf('lawliaison') >= 0);

                $rootScope.isVoidedFlyout = false;
                vs.voidServiceForm.$valid = true;
                vs.voidServiceForm.$setPristine();
                $rootScope.formReset(vs.voidServiceForm, vs.voidServiceForm.$name);
                $rootScope.defaultFormName = getDefaultFormName();
            }

            vs.changeVoidedReason = function () {
                vs.voidModel.IncorrectOrganization = false;
                vs.voidModel.IncorrectServiceType = false;
                vs.voidModel.IncorrectServiceItem = false;
                vs.voidModel.IncorrectServiceStatus = false;
                vs.voidModel.IncorrectSupervisor = false;
                vs.voidModel.IncorrectAdditionalUser = false;
                vs.voidModel.IncorrectAttendanceStatus = false;
                vs.voidModel.IncorrectStartDate = false;
                vs.voidModel.IncorrectStartTime = false;
                vs.voidModel.IncorrectEndDate = false;
                vs.voidModel.IncorrectEndTime = false;
                vs.voidModel.IncorrectDeliveryMethod = false;
                vs.voidModel.IncorrectServiceLocation = false;
                vs.voidModel.IncorrectRecipientCode = false;
                vs.voidModel.IncorrectTrackingField = false;
                if (vs.voidServiceForm) {
                    vs.voidServiceForm.modified = true;
                    $rootScope.setform(true);
                }
            };

            vs.voidService = function () {
                if (!angular.element('.bootbox-confirm').is(':visible')) {
                    bootbox.confirm("Are you sure you want to void this service?", function (result) {
                        if (result === true) {
                            var voidModel = vs.voidModel;
                            if (voidModel.ServiceRecordingVoidReasonID === vs.reasonStatusDataEntry) {
                                var isAnyCheckBoxChecked = false;
                                for (var prop in voidModel) {
                                    if (prop.indexOf('Incorrect') >= 0 && voidModel[prop] === true) {
                                        isAnyCheckBoxChecked = true; break;
                                    }
                                }
                                if (!isAnyCheckBoxChecked) {
                                    alertService.error('Please select at least one error reason');
                                    return;
                                }
                            }
                            if (voidModel.ServiceRecordingID && voidModel.ServiceRecordingID != 0 && voidModel.ServiceRecordingVoidReasonID > 0) {
                                //if call center service voided
                                if (voidModel.IsCallCenterVoided) {
                                    voidService.voidCallCenterRecordedService(voidModel).then(function (resp) {
                                        if (resp && resp.ResultCode >= 0) {
                                            $rootScope.$broadcast('voidServiceReloadGrid', { ID: vs.voidModel.ID, ServiceRecordingID: vs.voidModel.ServiceRecordingID, isCreateCopy: vs.voidModel.IsCreateCopyToEdit, AssessmentResponseID: vs.voidModel.AssessmentResponseID });
                                        }
                                    }, function (error) {
                                        alertService.error(error);
                                    }).finally(function () {
                                        vs.voidServiceForm.$setPristine();
                                        closeVoidedFlyout();
                                    });
                                }
                                else {
                                    voidService.voidRecordedService(voidModel).then(function (resp) {
                                        if (resp && resp.ResultCode >= 0) {
                                            if (vs.voidModel.AssessmentResponseID) {
                                                vs.voidModel.AssessmentResponseID && vs.cloneAllWorkFlow();
                                            }
                                            else {
                                                $rootScope.$broadcast('voidServiceReloadGrid', { ID: vs.voidModel.ID, ServiceRecordingID: vs.voidModel.ServiceRecordingID, isCreateCopy: vs.voidModel.IsCreateCopyToEdit, AssessmentResponseID: vs.voidModel.AssessmentResponseID });
                                            }
                                        } else {
                                            alertService.error("OOPS! Something went wrong");
                                        }
                                    }, function (error) {
                                        alertService.error(error);
                                    }).finally(function () {
                                        if (!vs.voidModel.AssessmentResponseID) {
                                            vs.voidServiceForm.$setPristine();
                                            closeVoidedFlyout();
                                        }
                                    });
                                }
                            }
                        }
                    });
                }
            };


            vs.cloneAllWorkFlow = function () {
                var assessemntId = vs.voidModel.AssessmentID;
                if (vs.voidModel.AssessmentResponseID) {
                    saveAssessmentResponse(assessemntId);
                }
            };

            var saveAssessmentResponse = function (assessemntId) {
                var assessmentResponse = {
                    ResponseID: 0,
                    ContactID: $stateParams.ContactID,
                    AssessmentID: assessemntId
                };

                assessmentService.addAssessmentResponse(assessmentResponse).then(function (newResponseData) {
                    var responseID = newResponseData.data.ID;
                    $rootScope.$broadcast('voidServiceReloadGrid', { ID: vs.voidModel.ID, ServiceRecordingID: vs.voidModel.ServiceRecordingID, isCreateCopy: vs.voidModel.IsCreateCopyToEdit, AssessmentResponseID: responseID });
                    var previousResponseID = vs.voidModel.AssessmentResponseID;
                    closeVoidedFlyout();
                    assessmentService.getAssessmentSections(assessemntId).then(function (assessmentSections) {
                        if (hasData(assessmentSections)) {
                            var assessmentSectionsdataItems = assessmentSections.DataItems;
                            angular.forEach(assessmentSectionsdataItems, function (assessmentSectionItem, key) {
                                var assessemntSectionId = assessmentSectionItem.AssessmentSectionID;
                                saveAssessmentResponseDetails(assessemntSectionId, responseID, previousResponseID);
                            });
                        }
                    });
                });
            }

            var saveAssessmentResponseDetails = function (assessemntSectionId, responseID, previousResponseID) {
                assessmentService.getAssessmentQuestions(assessemntSectionId).then(function (assessmentQuestions) {
                    if (hasData(assessmentQuestions)) {
                        assessmentService.getAssessmentResponseDetails(previousResponseID, assessemntSectionId).then(function (assessmentResponseDetailData) {
                            if (hasData(assessmentResponseDetailData.data)) {
                                var assessmentArray = [];
                                for (var i = 0; i < assessmentResponseDetailData.data.DataItems.length; i++) {
                                    var questionItem = $filter('filter')(assessmentQuestions.DataItems, { QuestionID: assessmentResponseDetailData.data.DataItems[i].QuestionID }, true)[0];
                                    if (questionItem && (!questionItem.Attributes || questionItem.Attributes.indexOf("removeOnCopy") == -1)) {
                                        assessmentArray.push(assessmentResponseDetailData.data.DataItems[i]);
                                    }
                                }
                                assessmentService.saveAssessmentResponseDetails(responseID, assessemntSectionId, assessmentArray);
                            }
                        });
                    }
                });
            }

            //todo: Required in future.
            vs.initVoidService = function (serviceRecordingID) {
                voidService.getVoidRecordedService(serviceRecordingID).then(function (voidServiceResponse) {
                    if (hasData(voidServiceResponse)) {
                        vs.voidModel = voidServiceResponse.DataItems[0];
                        //angular.extend(vs.voidModel, voidServiceDefaultModel);
                    }

                }, function (errorStatus) {
                    alertService("OOPS! Something went wrong");
                });

            };
        };
        voidServiceCtrl.$inject = ['alertService', 'voidService', '$rootScope', '$stateParams', 'assessmentService', 'formService', '$state', '$filter'];

        return {
            restrict: 'E',
            scope: {
                voidModel: '=',
                reloadGrid: '&'
            },
            templateUrl: '/Scripts/app/Template/VoidService.html',
            controller: voidServiceCtrl,
            controllerAs: "vs",
            bindToController: true
        };
    });
})();
