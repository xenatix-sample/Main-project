
(function () {
    angular.module('xenatixApp')
    .factory('_', ['$window', function ($window) {
        var _ = $window._;
        delete ($window._);

        if (!_.firstOrDefault)
            _.firstOrDefault = function (list, defVal) {
                return (!list || !list[0]) ? (_.isUndefined(defVal) ? null : defVal) : list[0];
            };

        // Adding this instead of _.find to ensure we never get an undefined return
        if (!_.firstWhere)
            _.firstWhere = function (list, conditionObj) {
                if (!list || !conditionObj)
                    return null;

                return _.firstOrDefault(_.where(list, conditionObj));
            };

        if (!_.distinct)
            _.distinct = function (list, attr) {
                var newList = [];

                if (list && _.isArray(list)) {
                    if (attr) {
                        for (var x = 0; x < list.length; x++) {
                            if (_.isUndefined(list[x][attr]) || _.isNull(list[x][attr]))
                                continue;

                            if (newList.length < 1)
                                newList.push(list[x]);
                            else {
                                var isInNewList = false;
                                for (var y = 0; y < newList.length; y++) {
                                    if (list[x][attr] == newList[y][attr])
                                        isInNewList = true;
                                }

                                if (!isInNewList)
                                    newList.push(list[x]);
                            }
                        }
                    }
                    else {
                        for (var x = 0; x < list.length; x++) {
                            if (_.indexOf(newList, list[x]) < 0) {
                                newList.push(list[x]);
                            }
                        }
                    }
                }

                return newList;
            };

        // Modified verion of sortBy to return descending order
        var cb = function (value, context, argCount) {
            if (value == null) return _.identity;
            if (_.isFunction(value)) return optimizeCb(value, context, argCount);
            if (_.isObject(value)) return _.matcher(value);
            return _.property(value);
        };

        if (!_.sortByDesc)
            _.sortByDesc = function (obj, iteratee, context) {
                iteratee = cb(iteratee, context);
                return _.pluck(_.map(obj, function (value, index, list) {
                    return {
                        value: value,
                        index: index,
                        criteria: iteratee(value, index, list)
                    };
                }).sort(function (left, right) {
                    var a = left.criteria;
                    var b = right.criteria;
                    if (a !== b) {
                        if (a < b || a === void 0) return 1;
                        if (a > b || b === void 0) return -1;
                    }
                    return right.index - left.index;
                }), 'value');
            };

        return (_);
    }]);
}());