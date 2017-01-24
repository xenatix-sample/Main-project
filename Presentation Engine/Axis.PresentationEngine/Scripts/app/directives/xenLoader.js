angular
  .module('xenatixApp')
  .directive('xenLoader', [
    '$rootScope',
    '$parse',
    '$timeout',
    function ($rootScope, $parse, $timeout) {
        return {
            restrict: 'E',
            scope: {
                timeToLive: '@'
            },
            template: '<div class="overlay" data-ng-show="isLoading">' +
                        '<div class="working">' +
                            '<i class="fa fa-spinner fa-pulse fa-3x fa-fw margin-bottom"></i>' +
                            '<p>Loading....</p>' +
                        '</div>' +
                    '</div>',
            link: function ($scope, element) {
                $scope.isLoading = false;
                var timer;
                var waitTimer;

                var timeToLive = $parse($scope.timeToLive)() || $scope.timeToLive;
                timeToLive = Number(timeToLive) * 1000;

                var onForceStop = function () {
                    $scope.isLoading = false;
                }

                var cancelTimers = function () {
                    if (waitTimer) {
                        $timeout.cancel(waitTimer);
                        waitTimer = null;
                    }
                    if (timer) {
                        $timeout.cancel(timer);
                        timer = null;
                    }
                }

                var onTriggerLoader = function (event, args) {
                    if (!args.isLoading) {
                        waitTimer = $timeout(function () {
                            $scope.isLoading = false;
                            cancelTimers();
                        }, 500);
                    }
                    else {
                        $scope.isLoading = true;
                        cancelTimers();
                    }

                    if (timer) {
                        return;
                    }

                    timer = $timeout(function () {
                        if ($scope.isLoading) {
                            $scope.isLoading = false;
                        }

                        cancelTimers();
                    }, timeToLive);
                }

                $rootScope.$on('triggerLoader', onTriggerLoader);
                $rootScope.$on('forceStopLoader', onForceStop);
            }
        };
    }
  ]);
