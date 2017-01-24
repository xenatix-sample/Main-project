// Service is used to maintain form modification status.
(function () {
    angular.module('xenatixApp')
        .service('formService', ['$rootScope', function ($rootScope) {
            var defaultFormName = 'default';
            var isDataChanged = {}
            isDataChanged[defaultFormName] = false;

            this.initForm = function (val, formName) {
                formName = formName || defaultFormName;
                isDataChanged[formName] = val;
            };

            this.isDirty = function (formName) {
                formName = formName || defaultFormName;
                return isDataChanged[formName];
            }

            this.isAnyFormDirty = function (formName) {
                formName = formName || defaultFormName;
            
                var isChanged = isDataChanged[formName];
                if (!isChanged)
                {
                    //If the defaultFormName is not undefined, then also check the defaultFormName is dirty or not
                    if ($rootScope.defaultFormName) {
                        isChanged= isDataChanged[$rootScope.defaultFormName];
                    }

                    if (!isChanged)
                    {
                        //traverse all forms to check dirty
                        for (var key in isDataChanged) {
                            isChanged = isDataChanged[key];
                            if (isChanged)
                                break;
                        }
                    }
                }
                
                return isChanged;
            }

            this.reset = function (formName) {
                if (!formName) {
                    isDataChanged = {}
                }
                formName = formName || defaultFormName;
                isDataChanged[formName] = false;
            }
        }]);
}());