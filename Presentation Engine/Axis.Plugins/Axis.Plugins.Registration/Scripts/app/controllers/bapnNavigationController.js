(function() {
    angular.module("xenatixApp")
        .controller("bapnNavigationController",
        [
            "$q", "$scope", "$stateParams", "assessmentService", "roleSecurityService", "$rootScope", "$state",
            "$timeout",
            "benefitsAssistanceProgressNoteService", "$filter", "serviceRecordingService",
            function($q,
                $scope,
                $stateParams,
                assessmentService,
                roleSecurityService,
                $rootScope,
                $state,
                $timeout,
                benefitsAssistanceProgressNoteService,
                $filter,
                serviceRecordingService) {
                $scope.bapnWorkFlowOptions = { enableWorkflow: 0 };
                var init = function() {
                    var sectionStateName =
                        "patientprofile.benefits.bapn.benefitsAssistanceProgressNote.bapnNavigation.section";
                    var workFlowItems = [];
                    var responseID = $stateParams.ResponseID;
                    var arrPromise = [];
                    workFlowItems.push({
                        title: "Services",
                        stateName: "bapnService",
                        stateParams:
                            "{ SectionID: $stateParams.SectionID,ContactID: $stateParams.ContactID, ResponseID: $stateParams.ResponseID, " +
                                "ReadOnly:$stateParams.ReadOnly, BenefitsAssistanceID:$stateParams.BenefitsAssistanceID, DocumentStatusID: $stateParams.DocumentStatusID}",
                        initActive: "initializeBapnService"
                    });

                    assessmentService.getAssessmentSections(ASSESSMENT_TYPE.BenefitAssessmentsProgressNote)
                        .then(function(data) {
                            if (data.ResultCode === 0) {
                                for (var iIdx = 0; iIdx < data.DataItems.length; iIdx++) {
                                    if (!data.DataItems[iIdx].PermissionKey ||
                                        roleSecurityService.hasPermission(data.DataItems[iIdx].PermissionKey, "read")) {
                                        workFlowItems.push({
                                            title: data.DataItems[iIdx].SectionName,
                                            stateName: sectionStateName,
                                            stateKey: data.DataItems[iIdx].AssessmentSectionID,
                                            stateParams:
                                                "{ ContactID: $stateParams.ContactID, ResponseID: $stateParams.ResponseID, SectionID: " +
                                                    data.DataItems[iIdx].AssessmentSectionID +
                                                    ", ReadOnly:$stateParams.ReadOnly,BenefitsAssistanceID:$stateParams.BenefitsAssistanceID, DocumentStatusID: $stateParams.DocumentStatusID }"
                                        });
                                        arrPromise.push(assessmentService
                                            .getAssessmentResponseDetails(responseID,
                                                data.DataItems[iIdx].AssessmentSectionID));
                                    }
                                    if (iIdx == data.DataItems.length - 1) {
                                        workFlowItems.push({
                                            title: "PDF View",
                                            onWorkflowClick: $scope.printBapn
                                        });
                                    }
                                }


                                if (!($state.current.name == "initializeBapnService")) {
                                    $q.all(arrPromise)
                                        .then(function(responseData) {
                                            var totalResponses = responseData.length;
                                            for (iIdx = 0; iIdx < totalResponses; iIdx++) {
                                                $rootScope
                                                    .$broadcast(sectionStateName +
                                                        responseData[iIdx].config.params.sectionId,
                                                        {
                                                            validationState: hasData(responseData[iIdx]
                                                                    .data)
                                                                ? "valid"
                                                                : "warning"
                                                        });
                                            }
                                        })
                                        .finally(function() {
                                            var stateDetail = { stateName: "bapnService" };
                                            $rootScope.$broadcast("rightNavigationBAPNHandler", stateDetail);
                                        });
                                }
                            }
                        });

                    $scope.workFlowModel = {
                        workFlowItems: workFlowItems
                    };
                };
                $scope.printBapn = function() {
                    var serviceRecordingID = null;
                    serviceRecordingService
                        .getServiceRecording($stateParams.BenefitsAssistanceID, SERVICE_RECORDING_SOURCE.BAPN)
                        .then(function(data) {
                            var serviceData, serviceDate, displaySignature;
                            if (hasData(data)) {
                                serviceData = data.DataItems[0];
                                serviceDate = $filter("toMMDDYYYYDate")(serviceData.ServiceStartDate);
                                data.ServiceRecordingID = serviceData.ServiceRecordingID;
                                displaySignature = true;
                            }

                            benefitsAssistanceProgressNoteService
                                .initReport(ASSESSMENT_TYPE.BenefitAssessmentsProgressNote,
                                    $stateParams.ResponseID,
                                    undefined,
                                    $stateParams.ContactID,
                                    serviceDate,
                                    displaySignature,
                                    serviceData.ServiceRecordingID,$state.current.data.workflowDataKey, $stateParams.BenefitsAssistanceID)
                                .then(function(reportModel) {
                                    reportModel.HasLoaded = true;
                                    $scope.reportModel = reportModel;
                                    $("#reportModalBAPN").modal("show");
                                });
                        });
                };
                $rootScope.$on("rightNavigationBAPNHandler",
                    function(event, args) {
                        if ($scope.workflowActions && $scope.workflowActions.hasOwnProperty(args.stateName)) {
                            if (args.stateName == "bapnService") {
                                $scope.bapnWorkFlowOptions.enableWorkflow = null;
                            }
                            if (args.validationState) {
                                $scope.workflowActions[args.stateName].validationState = args.validationState;
                                $scope.$broadcast(args.stateName, { validationState: args.validationState });
                            }
                        }
                    });

                init();
            }
        ]);
}());