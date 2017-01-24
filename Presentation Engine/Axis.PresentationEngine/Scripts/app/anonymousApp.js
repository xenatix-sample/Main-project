angular.module("xenatixApp", ['ui.router', 'xenatixApp.settings'], function () {
})
    .config([
        '$stateProvider', '$httpProvider', 'settings', '$controllerProvider', '$compileProvider', '$provide', '$filterProvider',  function ($stateProvider, $httpProvider, settings, $controllerProvider, $compileProvider, $provide, $filterProvider) {

            if (!$httpProvider.defaults.headers.get) {
                $httpProvider.defaults.headers.get = {};
            }
            $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
            $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
        }
    ])
    .run([
        '$rootScope', function ($rootScope) {
            $rootScope.globalConstant = {
                "emailPattern": /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9]+(\.[a-z0-9]+)*(\.[a-z]{2,4})$/i
            };
        }
    ]);