angular.module('xenatixApp')
    .controller('referralParentController', [
        '$scope', '$rootScope', '$state', '$q', '$stateParams', 'referralHeaderService', 'referralDispositionService', 'referralFollowupService', 'referralReferredInformationService', 'referralForwardedService', 'referralClientInformationService', '$filter', '$timeout', 'appointmentService', 'alertService',
    function ($scope, $rootScope, $state, $q, $stateParams, referralHeaderService, referralDispositionService, referralFollowupService, referralReferredInformationService, referralForwardedService, referralClientInformationService, $filter, $timeout, appointmentService, alertService) {
        $scope.referralStatusID = null;
        $scope.endDate = new Date();
        var stateCollections = [];
        $scope.ReferralDate = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal');
        $scope.isReadOnlyForm = $stateParams.ReadOnly.toString().toLowerCase() == 'view' ? true : false;
        var requestorState = 'referrals.requestor';
        var validState = 'valid';
        var invalidState = 'invalid';
        var warningState = 'warning';

        $scope.referralWorkFlowOptions = { enableWorkflow: 0 };

        $scope.initReferralParent = function (headerID, contactID) {
            if (headerID != undefined) {
                referralHeaderService.getReferralHeader(headerID, contactID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                        var resp = data.DataItems[0];
                        $scope.referralStatusID = resp.ReferralStatusID;
                        $scope.ReferralDate = $filter('toMMDDYYYYDate')(new Date(resp.ReferralDate ? resp.ReferralDate : new Date()), 'MM/DD/YYYY');
                    }
                    $("input[name='referralDate']").trigger("focus");
                },
                function (error) {
                });
            }
            $timeout(function () {
                $("input[name='referralDate']").trigger("focus");
            });
        }

        $scope.CancelReferral = function () {
            $rootScope.Goto('referralsearch');
        }

        $scope.createNavigation = function () {
            var workFlowItems = [
                { title: "Referrer", stateName: "referrals.requestor", stateParams: "{ ContactID:$stateParams.ContactID, ReferralHeaderID: $stateParams.ReferralHeaderID }", initActive: "referrals.main" },
                { title: "Contact", stateName: "referrals.client", stateParams: "{ ContactID:$stateParams.ContactID, ReferralHeaderID: $stateParams.ReferralHeaderID }" },
                { title: "Disposition", stateName: "referrals.disposition", stateParams: "{ ContactID:$stateParams.ContactID, ReferralHeaderID: $stateParams.ReferralHeaderID }" },
                { title: "Forwarded To", stateName: "referrals.forwardedto", stateParams: "{ ContactID:$stateParams.ContactID, ReferralHeaderID: $stateParams.ReferralHeaderID }" },
                { title: "Referred To", stateName: "referrals.referredto", stateParams: "{ ContactID:$stateParams.ContactID, ReferralHeaderID: $stateParams.ReferralHeaderID }" },
                { title: "Follow Up/Outcome", stateName: "referrals.followup", stateParams: "{ ContactID:$stateParams.ContactID, ReferralHeaderID: $stateParams.ReferralHeaderID }" },
                { title: "Appointment", stateName: "referrals.appointment", stateParams: "{ ContactID:$stateParams.ContactID,ReferralHeaderID: $stateParams.ReferralHeaderID }" }
            ];

            $scope.workFlowModel = {};
            $scope.workFlowModel.workFlowItems = workFlowItems;
        };

        $scope.initNavigation = function (headerID, contactID, setWarning) {
            $scope.createNavigation();
            var processQ = [];
            var validationState = ' ';
            $timeout(function () {
                var stateCollections = $("xen-workflow-action[data-state-name^=referral]").map(function () { return this.attributes['data-state-name'].value; }).get();
                if (headerID != undefined) {
                    referralClientInformationService.getReferralClientInformation(headerID).then(function (referralClientData) {
                        processQ.push(referralHeaderService.getReferralHeader(headerID, contactID));
                        processQ.push(referralClientData);
                        processQ.push(referralDispositionService.getReferralDispositionDetail(headerID));
                        processQ.push(referralForwardedService.getReferralForwardedDetails(headerID));
                        processQ.push(referralReferredInformationService.get(headerID));
                        processQ.push(referralFollowupService.getReferralFollowups(headerID));

                        if (referralClientData && referralClientData.DataItems && referralClientData.DataItems.length > 0) {
                            processQ.push(appointmentService.getAppointmentByResource(referralClientData.DataItems[0].clientDemographicsModel.ContactID, DOCUMENT_TYPE.AppointmentIndividualNote));
                        }
                        $q.all(processQ).then(function (processData) {
                            var indx = 0;
                            angular.forEach(stateCollections, function (state) {
                                var data = processData.slice(indx, indx + 1);
                                if ((data != undefined && data.length > 0 && data[0] != null) && (data[0].DataItems && (data[0].hasOwnProperty('DataItems') && data[0].DataItems.length > 0) || (data[0].hasOwnProperty('ID') && data[0].ID != 0))) {
                                    validationState = validState;
                                    /// Fix for client
                                    if (data[0].hasOwnProperty('DataItems') && data[0].DataItems && data[0].DataItems.length > 0 && data[0].DataItems[0].hasOwnProperty("ID") && data[0].DataItems[0].ID == 0)
                                        validationState = warningState;
                                }
                                else {
                                    validationState = warningState;
                                }

                                var stateDetail = { stateName: state, validationState: validationState };
                                $rootScope.$broadcast('rightNavigationReferralHandler', stateDetail);
                                indx = indx + 1;
                            })
                        })
                    },
                    function (errorStatus) {
                        alertService.error('OOPS! Something went wrong');
                    });
                }
                else {
                    $rootScope.$evalAsync(function () {
                        var newState = validationState;
                        if (setWarning) {
                            stateCollections = stateCollections.remove(requestorState);
                            var stateDetail = { stateName: requestorState, validationState: validState };
                            $rootScope.$broadcast('rightNavigationReferralHandler', stateDetail);
                            newState = warningState;
                        }
                        angular.forEach(stateCollections, function (state) {
                            var stateDetail = { stateName: state, validationState: newState };
                            $rootScope.$broadcast('rightNavigationReferralHandler', stateDetail);
                        });
                    });
                }
            });
        }

        $scope.$on('rightNavigationReferralHandler', function (event, args) {
            if ($rootScope.workflowActions != undefined && $rootScope.workflowActions.hasOwnProperty(args.stateName)) {
                if ((args.stateName == "referrals.requestor" || args.stateName == "referrals.main") && args.validationState == "valid") {
                    $scope.referralWorkFlowOptions.enableWorkflow = null;
                }
                $rootScope.workflowActions[args.stateName].validationState = args.validationState;
            }
            $rootScope.$broadcast(args.stateName, { validationState: args.validationState });
        });

        $scope.initNavigation($rootScope.ReferralHeaderID, $rootScope.ReferralContactID);
    }]);