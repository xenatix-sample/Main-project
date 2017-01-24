(function () {
    angular.module("xenatixApp")
        .controller("clientMergeNavigationController", ["$q", "$scope", "$stateParams", '$rootScope', '$state',
            function ($q, $scope, $stateParams, $rootScope, $state) {
                $scope.clientMergeWorkFlowOptions = { enableWorkflow: null };
                var initNavigation = function () {
                    var workFlowItems = [];
                    workFlowItems.push({ title: "Potential Matches", stateName: "businessadministration.clientmerge.clientmergeNavigation.potentialmatches"},
                        { title: "Merged Contacts", stateName: "businessadministration.clientmerge.clientmergeNavigation.mergedContacts"});

                    $scope.workFlowModel = {
                        workFlowItems: workFlowItems
                    };
                };
                initNavigation();

            }]);
}());