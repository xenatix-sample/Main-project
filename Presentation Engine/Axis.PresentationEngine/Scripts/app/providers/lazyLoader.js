angular.module("xenatixApp")
    .provider("lazyLoader", [function() {
            var $providers = [];
            var $invokeQueue = null;
            var $lastQueueLength = 0;
            var providerCache = [];

            function setInvokeQueue(invokeQueue) {
                $invokeQueue = invokeQueue;
            };

            function setProviders($controllerProvider, $compileProvider, $provide, $filterProvider) {
                $providers = {
                    $controllerProvider: $controllerProvider,
                    $compileProvider: $compileProvider,
                    $provide: $provide,
                    $filterProvider: $filterProvider
                };
            };

            function lazyLoad() {
                if (!$invokeQueue)
                    return;
                for (var i = $lastQueueLength; i < $invokeQueue.length; i++) {
                    var call = $invokeQueue[i];
                    var provider = $providers[call[0]];
                    if (provider && (providerCache.indexOf(call[2][0]) < 0)) {
                        provider[call[1]].apply(provider, call[2]);
                        providerCache.push(call[2][0]);
                    }
                }
                $lastQueueLength = $invokeQueue.length;
            };


            function getScriptPromise($http, scriptUrl) {
                return $http({ method: 'GET', url: scriptUrl, lazyLoader: true }).then(function (response) {
                    if (response)
                        eval(response.data);
                    lazyLoad();
                });
            }

            return {
                setProviders: setProviders,
                setInvokeQueue: setInvokeQueue,
                getScriptPromise: getScriptPromise,
                $get: function() {
                    return {
                        //TBD
                    };
                }
            };
        }
    ]);