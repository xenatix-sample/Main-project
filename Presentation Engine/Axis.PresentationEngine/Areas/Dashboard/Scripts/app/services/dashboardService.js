angular.module('xenatixApp')
    .factory('dashboardService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
        var CONFIG = {
            controllerAction: "/Dashboard/Dashboard/",
            offlineApiUrl: '/Dashboard/DashboardItems/'
        };
        function get() {
            var dfd = $q.defer();
            var data = {
                "Queues": [
                    {
                        "QueueTypeId": 1,
                        "QueueName": "Last Accessed",
                        "QueueItems": [
                            {
                                "CreatedBy": 1,
                                "CreatedByName": "Enid Walters",
                                "CreatedDateTime": new Date("1/3/2016 5:12pm"),
                                "AccessedDateTime": new Date("1/3/2016 7:12pm"),
                                "DueDate": null
                            }
                        ]
                    }
                ]
            };
            dfd.resolve(data);
            //$http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'GetDashboardItems', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl) })
            //    .success(function (data, status, header, config) {
            //        dfd.resolve(data);
            //    })
            //    .error(function (data, status, header, config) {
            //        dfd.reject(status);
            //    });
            return dfd.promise;
        };

        return {
            get: get
        };
    }]);
