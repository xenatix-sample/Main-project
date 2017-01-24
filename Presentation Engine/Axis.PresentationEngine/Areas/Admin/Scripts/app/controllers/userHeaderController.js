angular.module('xenatixApp')
    .controller('userHeaderController', [
        '$http', '$scope', '$rootScope', '$stateParams', '$state', '$filter', 'userHeaderService', 'userDetailService',
        function ($http, $scope, $rootScope, $stateParams, $state, $filter, userHeaderService, userDetailService) {

            $scope.init = function () {
                $scope.userID = $stateParams.UserID;
                $scope.pendingUserObj = { FullName: '', UserName: '', IsADUser: '', IsActive: '' };
                $scope.$evalAsync(function() {
                    $scope.getUserHeader($scope.userID);
                });
            };

            $scope.getUserHeader = function(userID) {
                if (userID !== 0) {
                    return userDetailService.get(userID).then(function (response) {
                        if (response.ResultCode === 0 && response.DataItems.length === 1) {
                            $scope.header = response.DataItems[0];
                            var tmpUserData = response.DataItems[0];
                            $scope.header.FullName = (tmpUserData.FirstName || '') + ' ' + (tmpUserData.MiddleName || '') + ' ' + (tmpUserData.LastName || '');
                            $scope.header.IsActive = $filter('toYesNo')($scope.header.IsActive);
                            var isADUser = $scope.header.UserGUID !== null && $scope.header.UserGUID !== undefined;
                            $scope.header.IsADUser = $filter('toYesNo')(isADUser);
                            var userPhotoHeader = angular.element("#userPhotoHeader");
                            if ($scope.header.ThumbnailBLOB !== undefined && $scope.header.ThumbnailBLOB !== null)
                                userPhotoHeader.attr("src", "data:image/jpeg;base64," + $scope.header.ThumbnailBLOB);
                            else if ($scope.header.GenderID == 2)
                                userPhotoHeader.attr("src", "Images/profile_female.svg");
                            else
                                userPhotoHeader.attr("src", "Images/profile_male.svg");


                        } else {
                            $scope.header = $scope.pendingUserObj;
                        }
                    });
                } else {
                    $scope.header = $scope.pendingUserObj;
                    return $scope.promiseNoOp();
                }
            };

            $scope.$on('updateuserheader', function (event, userID) {
                $scope.getUserHeader(userID);
            });

            $scope.init();
        }
    ]);