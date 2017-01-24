angular.module('xenatixApp')
    .controller('userPhotoController', [
        '$scope', '$q', 'alertService', '$filter', '$stateParams', 'photoService', 'userPhotoService', 'navigationService', '$rootScope', '$state', '$timeout',
        function ($scope, $q, alertService, $filter, $stateParams, photoService, userPhotoService, navigationService, $rootScope, $state, $timeout) {
            $scope.image = null;
            $scope.thumbnail = null;
            $scope.userPhotos = [];
            $scope.picture = null;
            $scope.profilePicture = null;
            $scope.isSaving = false;
            $scope.stopEnter = false;
            var defaultImagePath = '/Images/ph_avatar.png';
            var isMyProfile = false;

            var resetForm = function () {
                if ($scope.ctrl && $scope.ctrl.photoForm) {
                    $rootScope.formReset($scope.ctrl.photoForm);
                }
            };

            $scope.init = function () {
                if ($state.current.name == 'myprofile.nav.userphotos')
                    isMyProfile = true;
                else
                    $scope.permissionKey = $state.current.data.permissionKey;
                $scope.userID = $stateParams.UserID || 0;
                $scope.inProfile = false;
                $scope.prepareUserData($scope.userID).then(function () {
                    $scope.getUserPhotos();
                });
            };

            $scope.prepareUserData = function (userID) {
                if (userID === 0) {
                    $scope.inProfile = true;
                    $scope.stopEnter = true;
                    return navigationService.get().then(function (response) {
                        $scope.userID = response.DataItems[0].UserID;
                    });
                }
                return $scope.promiseNoOp();
            };

            $scope.viewLoaded = function () {
                offCanvasNav.init();
            };

            $scope.handleNextState = function () {
                var nextState = angular.element("li[data-state-name].list-group-item.active").next("li[data-state-name].list-group-item");
                if (nextState.length === 0) {
                } else {
                    $timeout(function () {
                        //$scope.getUserPhotos();
                        $rootScope.setform(false);
                        var nextStateName = nextState.attr('data-state-name');
                        $scope.Goto(nextStateName, { UserID: $scope.userID });
                    });
                }
            };

            // Patient Profile Photo
            $scope.save = function (isNext, mandatory, hasErrors) {
                if (($.isEmptyObject(!$scope.picture)) && isNext) {
                    $scope.handleNextState();
                } else {
                    savePhoto().then(function (response) {
                        if (response != undefined) {
                            alertService.success('Profile photo is saved successfully.');
                            $scope.picture = null;
                            $scope.isSaving = false;
                            $scope.getUserPhotoById(response);
                            if (isNext) {
                                $scope.getUserPhotos().then(function (getResponse) {
                                    $scope.handleNextState();
                                });
                            }
                        }
                    });
                }
            };

            $scope.getUserPhotos = function () {
                return userPhotoService.getUserPhoto($scope.userID, isMyProfile).then(function (userPhotoResponse) {
                    var defer = $q.defer();
                    var promises = [];
                    if (userPhotoResponse.DataItems.length > 0) {
                        $scope.photoId = userPhotoResponse.DataItems[0].UserPhotoID;
                        var obj = { stateName: $state.current.name, validationState: 'valid' };
                        $rootScope.staffManagementRightNavigationHandler(obj);
                        $rootScope.myProfileRightNavigationHandler(obj);
                        angular.forEach(userPhotoResponse.DataItems, function (userPhoto) {
                            promises.push(photoService.getPhoto(userPhoto.PhotoID).then(function (photoResponse) {
                                var photo = photoResponse.DataItems[0];
                                var userThumbnail = {
                                    UserPhotoID: userPhoto.UserPhotoID,
                                    UserID: userPhoto.UsertID,
                                    PhotoID: userPhoto.PhotoID,
                                    ThumbnailBLOB: photo.ThumbnailBLOB,
                                    IsPrimary: userPhoto.IsPrimary
                                };
                                return userThumbnail;
                            }));
                        });
                        return $q.all(promises).then(function (photoes) {
                            $scope.userPhotos = photoes;
                            $scope.profilePicture = $filter('filter')(photoes, {
                                IsPrimary: true
                            })[0];
                            resetForm();
                        });
                    }
                    else {
                        $scope.photoId = 0;
                        $scope.userPhotos = [];
                        $scope.profilePicture = null;
                        $scope.preview = false;
                        var obj = { stateName: $state.current.name, validationState: 'warning' };
                        $rootScope.staffManagementRightNavigationHandler(obj);
                        $rootScope.myProfileRightNavigationHandler(obj);
                        resetForm();
                    }
                });
            };

            $scope.updateHeader = function () {
                $scope.$parent.$broadcast('updateuserheader', $scope.userID);
            };

            $scope.getUserPhotoById = function (userPhotoId) {
                return userPhotoService.getUserPhotoById($scope.userID, userPhotoId, isMyProfile).then(function (userPhotoResponse) {
                    $scope.updateHeader();
                    var defer = $q.defer();
                    var promises = [];
                    if (userPhotoResponse.DataItems.length > 0) {
                        var obj = { stateName: $state.current.name, validationState: 'valid' };
                        $rootScope.staffManagementRightNavigationHandler(obj);
                        $rootScope.myProfileRightNavigationHandler(obj);
                        var userPhoto = userPhotoResponse.DataItems[0];
                        promises.push(photoService.getPhoto(userPhoto.PhotoID).then(function (photoResponse) {
                            var photo = photoResponse.DataItems[0];
                            var userThumbnail = {
                                UserPhotoID: userPhoto.UserPhotoID,
                                UserID: userPhoto.UserID,
                                PhotoID: userPhoto.PhotoID,
                                ThumbnailBLOB: photo.ThumbnailBLOB,
                                IsPrimary: userPhoto.IsPrimary
                            };

                            return userThumbnail;
                        }));
                        return $q.all(promises).then(function (photoes) {
                            var userPhotos = angular.copy($scope.userPhotos);

                            var isExists = $filter('filter')(userPhotos, {
                                UserPhotoID: userPhotoId
                            }).length > 0 ? true : false;

                            if (isExists) {
                                angular.forEach(userPhotos, function (userPhotosResponse) {
                                    if (userPhotosResponse.UserPhotoID === userPhotoId) {
                                        userPhotosResponse.ThumbnailBLOB = photoes[0].ThumbnailBLOB;
                                        userPhotosResponse.IsPrimary = photoes[0].IsPrimary;
                                    }
                                });
                            }
                            else {
                                userPhotos.push(photoes[0]);
                            }
                            $scope.userPhotos = userPhotos;
                            $scope.profilePicture = $filter('filter')($scope.userPhotos, {
                                IsPrimary: true
                            })[0];

                            if ($scope.inProfile) {
                                if ($scope.profilePicture !== undefined && $scope.profilePicture !== null) {
                                    angular.element("#userPhoto").attr("src", "data:image/jpeg;base64," + $scope.profilePicture.ThumbnailBLOB);
                                }
                            }
                            resetForm();
                        });
                    }
                    else {
                        $scope.userPhotos = [];
                        $scope.profilePicture = null;
                        resetForm();
                    }
                });
            };

            $scope.delete = function (userPhotoId) {
                bootbox.confirm('Are you sure you want to deactivate?', function (confirmed) {
                    if (confirmed) {
                        userPhotoService.deleteUserPhoto($scope.userID, userPhotoId, isMyProfile).then(function (userPhotoThumbnails) {
                            if (userPhotoThumbnails.ResultCode === 0) {
                                alertService.success('Photo has been deleted successfully.');
                                var userPhotos = angular.copy($scope.userPhotos);
                                var filteredUserThumbnails = $filter('filter')(userPhotos, { UserPhotoID: '!' + userPhotoId });
                                $scope.userPhotos = filteredUserThumbnails;
                                $scope.updateHeader();
                                if ($scope.inProfile) {
                                    if ($scope.profilePicture && $scope.profilePicture.UserPhotoID == userPhotoId)
                                        angular.element("#userPhoto").attr("ng-src", defaultImagePath);
                                }
                                resetForm();
                            } else {
                                alertService.error('Unable to delete photo.');
                            }
                        });
                    }
                });
            };

            $scope.setPreviewAsPhoto = function () {
                savePhoto().then(function (userPhotoId) {
                    setAsPhoto(userPhotoId).then(function () {
                        alertService.success('Photo set as profile picture.');
                        $scope.isSaving = false;

                        angular.forEach($scope.userPhotos, function (thumbnail) {
                            thumbnail.IsPrimary = false;
                        });

                        $scope.getUserPhotoById(userPhotoId);
                    });
                });
            };

            $scope.setAsPhoto = function (userPhotoId) {
                setAsPhoto(userPhotoId).then(function () {
                    alertService.success('Photo set as profile picture.');
                    $scope.isSaving = false;

                    angular.forEach($scope.userPhotos, function (thumbnail) {
                        thumbnail.IsPrimary = false;
                    });
                  
                    $scope.getUserPhotoById(userPhotoId);
                });
            };

            $scope.donotUsePhoto = function () {
                // Get all userPhoto by userId
                userPhotoService.getUserPhoto($scope.userID, isMyProfile).then(function (userPhotoResponse) {
                    var userPhotos = userPhotoResponse.DataItems;
                    // Get primary userPhoto & set IsPrimary = false
                    var primaryUserPhoto = $filter('filter')(userPhotos, {
                        IsPrimary: true
                    })[0];

                    if (primaryUserPhoto != undefined) {
                        primaryUserPhoto.IsPrimary = false;
                        userPhotoService.updateUserPhoto(primaryUserPhoto, isMyProfile).then(function (response) {
                            alertService.success('No photo is set as profile picture.');
                            $scope.picture = null;
                            $scope.getUserPhotos();
                            $scope.updateHeader();
                            if ($scope.inProfile) {
                                angular.element("#userPhoto").attr("ng-src", defaultImagePath);
                            }
                        });
                    }
                });
            }

            var savePhoto = function () {
                if ($scope.isSaving)
                    return $scope.promiseNoOp();

                if (!$scope.picture || $.isEmptyObject($scope.picture)) {
                    alertService.error('Select or take picture to save.');
                    return $scope.promiseNoOp();
                }

                $scope.picture.TakenBy = $scope.userID;
                $scope.picture.TakenTime = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY HH:mm');

                $scope.isSaving = true;
                if ($scope.picture.PhotoID != undefined && $scope.picture.PhotoID != 0) {
                    return photoService.updatePhoto($scope.picture).then(function () {
                        return $scope.picture.UserPhotoID;
                    });
                }
                else {
                    return photoService.addPhoto($scope.picture).then(function (response) {
                        var userPhoto = {
                            UserID: $scope.userID,
                            PhotoID: response.data.ID
                        };

                        return userPhotoService.addUserPhoto(userPhoto, isMyProfile).then(function (userPhotoResponse) {
                            return userPhotoResponse.ID;
                        });
                    });
                }
            }

            var setAsPhoto = function (userPhotoId) {
                // Get all userPhoto by userId
                return userPhotoService.getUserPhoto($scope.userID, isMyProfile).then(function (userPhotoResponse) {
                    var userPhotos = userPhotoResponse.DataItems;

                    // Get primary userPhoto & set IsPrimary = false
                    var primaryUserPhoto = $filter('filter')(userPhotos, {
                        IsPrimary: true
                    })[0];

                    // Get userPhoto by userPhotoId & set IsPrimary = true
                    var userPhotoToPromary = $filter('filter')(userPhotos, {
                        UserPhotoID: userPhotoId
                    })[0];

                    if (userPhotoToPromary != undefined) {
                        userPhotoToPromary.IsPrimary = true;
                        return userPhotoService.updateUserPhoto(userPhotoToPromary, isMyProfile).then(function (response) {
                            if (primaryUserPhoto != undefined && primaryUserPhoto.UserPhotoID !== userPhotoToPromary.UserPhotoID) {
                                primaryUserPhoto.IsPrimary = false;
                                return userPhotoService.updateUserPhoto(primaryUserPhoto, isMyProfile);
                            }
                        });
                    }
                });
            }

            //call on load
            $scope.init();
        }
    ]);