(function () {
    angular.module("xenatixApp")
        .controller("intakeFormNavigationController", ["$q", "$scope", "$stateParams", 'assessmentService', 'roleSecurityService', '$rootScope', '$state', '$timeout', function ($q, $scope, $stateParams, assessmentService, roleSecurityService, $rootScope, $state, $timeout) {
            var init = function () {
                $scope.intakeWorkFlowOptions = { enableWorkflow: 0 };
                var sectionStateName = "patientprofile.intake.formnavi.forms.formsnavi.formsSection";
                var workFlowItems = [];
                var arrPromise = [];
                $scope.workFlowModel = {};
                var responseID = $state.params.ResponseID;
                workFlowItems.push({
                    title: "Services", stateName: "formservice", stateParams: "{ SectionID: $stateParams.SectionID,ContactID: $stateParams.ContactID,AssessmentID:$stateParams.AssessmentID, ResponseID: $stateParams.ResponseID, " +
                            "ReadOnly:$stateParams.ReadOnly, ContactFormsID:$stateParams.ContactFormsID, DocumentStatusID: $stateParams.DocumentStatusID}", initActive: "initializeformservice"
                });

                assessmentService.getAssessmentSections(ASSESSMENT_TYPE.IDDIntakeForms).then(function (data) {
                    if (data.ResultCode === 0) {
                        for (var iIdx = 0; iIdx < data.DataItems.length; iIdx++) {
                            if (!data.DataItems[iIdx].PermissionKey || roleSecurityService.hasPermission(data.DataItems[iIdx].PermissionKey, 'read')) {
                                workFlowItems.push({
                                    title: data.DataItems[iIdx].SectionName, stateName: sectionStateName, stateKey: data.DataItems[iIdx].AssessmentSectionID, stateParams: '{ ContactID: $stateParams.ContactID,AssessmentID:$stateParams.AssessmentID, ResponseID: $stateParams.ResponseID, SectionID: ' +
                                                        data.DataItems[iIdx].AssessmentSectionID + ', ReadOnly:$stateParams.ReadOnly,ContactFormsID:$stateParams.ContactFormsID, DocumentStatusID: $stateParams.DocumentStatusID }'
                                });
                                responseID && arrPromise.push(assessmentService.getAssessmentResponseDetails(responseID, data.DataItems[iIdx].AssessmentSectionID));
                            }
                        }



                        if (!($state.current.name == "initializeformservice")) {
                            $q.all(arrPromise).then(function (responseData) {
                                var totalResponses = responseData.length;
                                for (iIdx = 0; iIdx < totalResponses; iIdx++) {
                                    $rootScope.$broadcast(sectionStateName + responseData[iIdx].config.params.sectionId, { validationState: hasData(responseData[iIdx].data) ? 'valid' : 'warning' });
                                }
                            }).finally(function () {
                                var stateDetail = { stateName: 'formservice' };
                                $rootScope.$broadcast('rightNavigationIntakeFormHandler', stateDetail);
                            });
                        }
                    }
                });
                $scope.workFlowModel.workFlowItems = workFlowItems;
            };

            $rootScope.$on('rightNavigationIntakeFormHandler', function (event, args) {
                if ($scope.workflowActions && $scope.workflowActions.hasOwnProperty(args.stateName)) {
                    if (args.stateName == "formservice") {
                        $scope.intakeWorkFlowOptions.enableWorkflow = null;
                    }
                    if (args.validationState) {
                        $scope.workflowActions[args.stateName].validationState = args.validationState;
                        $scope.$broadcast(args.stateName, { validationState: args.validationState });
                }
                }
            });
            init();
        }]);
}());
