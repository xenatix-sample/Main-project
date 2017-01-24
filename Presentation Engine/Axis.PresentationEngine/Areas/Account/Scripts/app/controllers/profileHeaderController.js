angular.module('xenatixApp')
    .controller('profileHeaderController', [
        '$http', '$scope', '$rootScope', '$stateParams', '$state', '$filter', 'userProfileService',
        function ($http, $scope, $rootScope, $stateParams, $state, $filter, userProfileService) {
            var isMyProfile = false;
            $scope.init = function () {               
                if ($state.current.name == 'myprofile.nav.profile' || $state.current.name == 'myprofile.nav.security') {
                    isMyProfile = true;
                };
                $scope.unknownUserObj = { FullName: '', UserName: '', IsADUser: '', IsActive: '' };
                $scope.$evalAsync(function () {
                    $scope.getUserHeader();
                });
            };
          
            $scope.getUserHeader = function () {
                return userProfileService.get(isMyProfile).then(function (response) {
                    if (response.ResultCode === 0 && response.DataItems.length === 1) {
                        $scope.header = response.DataItems[0];
                        var tmpUserData = response.DataItems[0];
                        $scope.header.FullName = (tmpUserData.FirstName || '') + ' ' + (tmpUserData.MiddleName || '') + ' ' + (tmpUserData.LastName || '');
                        $scope.header.IsActive = $filter('toYesNo')($scope.header.IsActive);
                        var isADUser = $scope.header.ADFlag; //This should check for ADFlag
                        $scope.header.IsADUser = $filter('toYesNo')(isADUser);
                        var userPhotoHeader = angular.element("#userPhotoHeader");
                        if ($scope.header.ThumbnailBLOB !== undefined && $scope.header.ThumbnailBLOB !== null)
                            userPhotoHeader.attr("src", "data:image/jpeg;base64," + $scope.header.ThumbnailBLOB);
                        else if ($scope.header.GenderID == 2)
                            userPhotoHeader.attr("src", "Images/profile_female.svg");
                        else
                            userPhotoHeader.attr("src", "Images/profile_male.svg");
                    } else {
                        $scope.header = $scope.unknownUserObj;
                    }
                });
            };

            $scope.$on('updateuserheader', function (event, userID) {
                $scope.getUserHeader();
            });

            $scope.init();
        }
    ]);
