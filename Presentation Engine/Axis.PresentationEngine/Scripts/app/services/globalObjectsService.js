(function () {
    angular.module('xenatixApp')
        .service('globalObjectsService', function () {
            this.isViewContentLoaded = false;
            this.setViewContent = function () {
                if (!this.isViewContentLoaded) {
                    this.isViewContentLoaded = true;
                }
            }
        });
}());