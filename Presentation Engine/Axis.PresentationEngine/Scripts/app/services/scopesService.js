angular.module('xenatixApp').factory('scopesService', function ($rootScope) {
    var mem = {},
        changeSubs = [];

    var callChange = function (key) {
        var sub;
        for (var i = 0; i < changeSubs.length; i++) {
            sub = changeSubs[i];
            if (sub && sub.key == key) {
                if (sub.callback)
                    sub.callback();
                else
                    changeSubs.slice(i, 1);
            }
        }
    };

    return {
        store: function (key, value) {
            if (!key) return;

            mem[key] = value;
            callChange(key);
        },
        get: function (key) {
            return mem[key];
        },
        clear: function (key) {
            if (!key) return;

            mem[key] = null;
            callChange(key);
        },
        clearAll: function () {
            mem = {};
            changeSubs = [];
        },
        subscribe: function (key, callback, name) {
            changeSubs.push({ key: key, name:name, callback: callback });
        },
        unsubscribe: function (name) {
            for (var i = 0; i < changeSubs.length; i++) {
                if (changeSubs[i] && changeSubs[i].name == name)
                    changeSubs.slice(i, 1);
            }
        }
    };
});

