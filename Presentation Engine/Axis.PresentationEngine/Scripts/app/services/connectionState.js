angular
    .module('xenatixApp')
    .service('connectionStateService', [
        '$window', '$rootScope', function($window, $rootScope) {
            var onEvent = function(event, listener) {
                $window.addEventListener(event, function() {
                    $rootScope.$apply(listener);
                });
            };

            this.isOnline = function () {
//                return (new Date().getSeconds() < 30);
                return $window.navigator.onLine;
            };

            this.onOnline = function(listener) {
                onEvent('online', listener);
            };

            this.onOffline = function(listener) {
                onEvent('offline', listener);
            };
        }
    ]);
