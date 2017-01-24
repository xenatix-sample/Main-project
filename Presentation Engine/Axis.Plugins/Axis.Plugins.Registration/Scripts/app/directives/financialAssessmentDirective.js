angular.module('xenatixApp')
.directive("financialAssessment", function () {
    return {
        restrict: "E",
        scope: {
            model: "=",
            title: "@",
            categoryoptions: "=",
            expirationreasons: "=",
            frequencyoptions: "=",
            onsave: "&onsave",
            onreset: "&onreset",
            onclick: "&onclick"

        },
        templateUrl: "/plugins/FinancialAssessment/_FinancialAssessment"
        //link: function (scope, element, attrs, ngModel) {
        //    scope.onReset = function () {
        //        alert("ok");
        //        scope = {};
        //    };
        //}



    };
});




