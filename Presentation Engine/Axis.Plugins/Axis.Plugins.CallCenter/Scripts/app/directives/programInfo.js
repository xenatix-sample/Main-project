angular.module('xenatixApp')
.directive("programInfo", ['lookupService', function (lookupService) {
    return {
        restrict: "E",
        transclude: true,
        scope: {
            programUnit : '=',
            programUnitId: '=',
            countyId: '=',
            suicideHomicideId: '=',
            callPriorityId: '='
        },
        link: function (scope) {
            scope.getLookupsByType = function (type) {
                return lookupService.getLookupsByType(type);
            }
        },
        templateUrl: '/Plugins/Axis.Plugins.CallCenter/Scripts/app/template/ProgramInfo.html'
    }
}]);