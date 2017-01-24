angular.module('xenatixApp')
.filter('excludeSelect', function () {
    return function (input, select, selection) {
            var newInput = [];
            for (var i = 0; i < input.length; i++) {
                var addToArray = true;
                for (var j = 0; j < select.length; j++) {
                    if (select[j].SecurityQuestionID === input[i].ID) {
                        addToArray = false;
                    }
                }
                if (addToArray || input[i].ID === selection.SecurityQuestionID) {
                    newInput.push(input[i]);
                }
            }
            return newInput;
        }
    })