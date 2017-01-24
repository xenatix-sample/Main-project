(function() {
    angular.module('xenatixApp').controller('navigationController', [
        '$scope', '$state', 'formService', '$stateParams', '$rootScope', 'navigationService', 'alertService', 'offlineData', function ($scope, $state, formService, $stateParams, $rootScope, navigationService,  alertService, offlineData) {
            $scope.offlineUserName = '';
            $scope.offlinePassword = '';
            $scope.passwordHash = '';
            $scope.ShowDaystoExpire = function (days) {
                if (days != '' && parseInt(days) < 10) {
                    alertService.warning('Password to Expire in' + days);
                   
                }
            };
            $rootScope.Goto = function(goto, urlParams, reload) {
                var isDirty = formService.isAnyFormDirty();
                var load = reload ? true : false;
                if (($state.current.name != goto) || ($state.current.params != urlParams)) {
                    if (!isDirty) {
                        $scope.stateChanged(goto, urlParams, load);
                    } else {
                        $rootScope.isStopKeyEvents = true;
                        bootbox.confirm("Any unsaved data will be lost. Do you want to continue?", function(result) {
                            $rootScope.isStopKeyEvents = false;
                            if (result == true) {
                                $scope.stateChanged(goto, urlParams, load);
                            }
                        });
                    }
                    return false;
                }
            };

            $scope.stateChanged = function(url, urlParams, reload) {

                if (urlParams != undefined && urlParams != "") {
                    $state.go(url, eval(urlParams), { reload: reload });
                } else {
                    $state.go(url, {}, { reload: reload });
                }
            }

            $scope.toggleTheme = function(theme) {
                if (theme === 'Dark') {
                    $rootScope.useDarkTheme = true;
                } else {
                    $rootScope.useDarkTheme = false;
                }
            };

            $scope.authenticateOffline = function() {
                //offlineData.setPasswordHash(sjcl.codec.base64.fromBits(sjcl.hash.sha256.hash($scope.offlinePassword)));
                navigationService.get().then(function(data) {
                    if ((data !== undefined) && ('DataItems' in data) && (data.DataItems.length === 1) && ($scope.offlineUserName.toUpperCase() === data.DataItems[0].UserName.toUpperCase())) {
                        alertService.success('Welcome back!');
                        $scope.UserRolePrimary = data.DataItems[0].UserRolePrimary;
                        $scope.UserFullName = data.DataItems[0].UserFullName;
                        $state.go('contacts');
                    } else
                        alertService.error('Login failed!');
                });
            };

            $scope.init = function () {
                navigationService.get().then(function (data) {
                    if (data.DataItems.length === 1) {
                        $scope.UserRolePrimary = data.DataItems[0].UserRolePrimary;
                        $scope.UserFullName = data.DataItems[0].UserFullName;
                        $scope.offlinePassword = data.DataItems[0].PasswordHash;
                        $scope.ThumbnailBLOB = data.DataItems[0].ThumbnailBLOB;
                        if ($scope.ThumbnailBLOB !== undefined && $scope.ThumbnailBLOB !== null)
                            angular.element("#userPhoto").attr("src", "data:image/jpeg;base64," + $scope.ThumbnailBLOB);
                        //offlineData.setPasswordHash($scope.offlinePassword);
                        appInsights.setAuthenticatedUserContext(data.DataItems[0].UserName);
                        // Do not attempt to redirect to an online-only state when offline
                        if ((data.DataItems[0].IsProfileComplete === false || data.DataItems[0].IsSecurityQuestionComplete === false)) {
                            $state.go('myprofile.nav.security');
                        }
                    } else
                        $state.go('offlineLogin');
                });
            }

            $scope.init();
        }
    ]);
}());
