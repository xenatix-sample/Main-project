angular.module('xenatixApp')
    .factory('cacheService', ['$localStorage', function ($localStorage) {
        function add(key, value) {
            $localStorage[key] = value;
        };

        function get(key) {
            return $localStorage[key];
        };

        function remove(key) {
            //Why are we logging null? Changing the order.
            $localStorage[key] = null;
        };

        return {
            add: add,
            get: get,
            remove: remove
        };
    }]);