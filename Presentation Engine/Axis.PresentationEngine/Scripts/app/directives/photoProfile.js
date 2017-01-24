angular.module('xenatixApp')
.directive("photoProfile", ['$q', '$timeout', '$filter', 'alertService', '$compile', '$state', '$injector', 'roleSecurityService', 'formService', function ($q, $timeout, $filter, alertService, $compile, $state, $injector, roleSecurityService, formService) {
    return {
        restrict: "E",
        scope: {
            captureMessage: '@',
            countdown: '@',
            flashFallbackUrl: '@',
            overlayUrl: '@',
            outputHeight: '@',
            outputWidth: '@',
            shutterUrl: '@',
            viewerHeight: '@',
            viewerWidth: '@',
            cropHeight: '@',
            cropWidth: '@',
            imageFormat: '@',
            jpegQuality: '@',
            module: '@',
            picture: '=',
            photos: '=',
            preview: '=',
            onDelete: "&",
            onSetAsPhoto: "&",
            onSetPreviewAsPhoto: "&",
            onDonotUsePhoto: "&"
        },
        link: function (scope, element, attrs) {
            /**
             * Set default variables
             */
            var mode = {
                Upload: 0,
                Camera: 1
            };
            if ($state.current.data)
                scope.permissionKey = $state.current.data.permissionKey;
            scope.PERMISSION = PERMISSION;
            scope.libraryLoaded = false;
            scope.cameraLive = false;
            scope.activeCountdown = false;
            scope.cameraMode = false;
            scope.preview = false;
            scope.donotUsePhoto = true;
            scope.removeditemIndex = 0;
            var maxZoomInLimit = 0.5904900000000002; //1*0.9*0.9*0.9*0.9*0.9  5X
            var maxZoomOutLimit = 1.6105100000000008; //1*1.1*1.1*1.1*1.1*1.1 5X
            var rotation = 0, ratio = 1;
            var CanvasCrop = $.CanvasCrop({
                cropBox: ".imageBox",
                imgSrc: "/Images/profile_male.svg",
                limitOver: 2
            });

            /**
             * Set dimensions
             */
            if (scope.viewerHeight === undefined) {
                scope.viewerHeight = 'auto';
            }
            if (scope.viewerWidth === undefined) {
                scope.viewerWidth = 'auto';
            }
            if (scope.outputHeight === undefined) {
                scope.outputHeight = scope.viewerHeight;
            }
            if (scope.outputWidth === undefined) {
                scope.outputWidth = scope.viewerWidth;
            }
            if (getBrowserName() == 'ie') {
                scope.outputHeight = "185";
                scope.outputWidth = "275";
                scope.viewerHeight = "185";
                scope.viewerWidth = "275";
            }
            /**
             * Disable cropping if one or the two params are undefined
             */
            if (scope.cropHeight === undefined || scope.cropWidth === undefined) {
                scope.cropHeight = false;
                scope.cropWith = false;
            }

            /**
             * Set configuration parameters
             * @type {object}
             */
            Webcam.set({
                width: scope.viewerWidth,
                height: scope.viewerHeight,
                dest_width: scope.outputWidth,
                dest_height: scope.outputHeight,
                crop_width: scope.cropWidth,
                crop_height: scope.cropHeight,
                image_format: scope.imageFormat,
                jpeg_quality: scope.jpegQuality,
                force_flash: false
            });
            if (scope.flashFallbackUrl !== 'undefined') {
                Webcam.setSWFLocation(scope.flashFallbackUrl);
            }

            /**
             * Register WebcamJS events
             */
            Webcam.on('load', function () {
                scope.$apply(function () {
                    scope.libraryLoaded = true;
                });
            });
            Webcam.on('live', function () {
                scope.$apply(function () {
                    scope.cameraLive = true;
                    scope.cameraMode = true;
                });
            });

            Webcam.on('error', function (error) {
                scope.$apply(function () {
                    scope.cameraMode = false;
                    scope.preview = false;
                    if (error.indexOf('DevicesNotFoundError') > 0) {
                        alertService.error('No camera detected.');
                    }
                });
            });

            /**
             * Preload the shutter sound
             */
            if (scope.shutterUrl !== undefined) {
                scope.shutter = new Audio();
                scope.shutter.autoplay = false;
                if (navigator.userAgent.match(/Firefox/)) {
                    scope.shutter.src = scope.shutterUrl.split('.')[0] + '.ogg';
                } else {
                    scope.shutter.src = scope.shutterUrl;
                }
            }

            /**
             * Set countdown
             */
            if (scope.countdown !== undefined) {
                scope.countdownTime = parseInt(scope.countdown) * 1000;
                scope.countdownText = parseInt(scope.countdown);
            }
            scope.countdownStart = function () {
                scope.activeCountdown = true;
                scope.countdownPromise = $q.defer();
                scope.countdownTick = setInterval(function () {
                    return scope.$apply(function () {
                        var nextTick;
                        nextTick = parseInt(scope.countdownText) - 1;
                        if (nextTick === 0) {
                            scope.countdownText = scope.captureMessage != null ? scope.captureMessage : 'GO!';
                            clearInterval(scope.countdownTick);
                            scope.countdownPromise.resolve();
                        } else {
                            scope.countdownText = nextTick;
                        }
                    });
                }, 1000);
            };

            /**
             * Get snapshot
             */
            scope.getSnapshot = function () {
                formService.initForm(true);
                if (scope.countdown !== undefined) {
                    scope.countdownStart();
                    scope.countdownPromise.promise.then(function () {
                        $timeout(function () {
                            scope.activeCountdown = false;
                            scope.countdownText = parseInt(scope.countdown);
                        }, 2000);

                        if (scope.shutterUrl !== undefined) {
                            scope.shutter.play();
                        }

                        Webcam.snap(function (data_uri) {
                            resetPicture();
                            setProfilePicture(data_uri, null);
                            scope.cameraMode = false;
                            scope.stopCamera();
                            scope.preview = true;
                        });
                    });
                } else {
                    if (scope.shutterUrl !== undefined) {
                        scope.shutter.play();
                    }

                    Webcam.snap(function (data_uri) {
                        resetPicture();
                        setProfilePicture(data_uri, null);
                        setImageEditor(data_uri);

                        scope.cameraMode = false;
                        scope.stopCamera();
                        scope.preview = true;
                    });
                }
            };

            scope.$on('$destroy', function () {
                Webcam.reset();
            });

            scope.startCamera = function () {
                scope.cameraMode = true;
                scope.preview = true;

                if (!Webcam.live) {
                    Webcam.attach('#ng-camera-feed');
                }
            };

            scope.stopCamera = function () {
                scope.cameraMode = false;
                scope.preview = false;
                Webcam.reset()
            };

            scope.isNotUsingPhoto = function () {
                return scope.donotUsePhoto && scope.photos && scope.photos.length > 0;
            };

            scope.$watchCollection('photos', function (newThumbnailImages, oldThumbnailImages) {
                if (newThumbnailImages !== oldThumbnailImages) {
                    if (oldThumbnailImages) {
                        var removedItem = oldThumbnailImages.filter(function (item) {
                            return newThumbnailImages.filter(function (newItem) {
                                return newItem[scope.module + "PhotoID"] == item[scope.module + "PhotoID"];
                            }).length == 0
                        });
                        if (removedItem.length > 0) {
                            scope.removeditemIndex = oldThumbnailImages.indexOf(removedItem[0]);
                        }
                    }

                    if (newThumbnailImages != null) {
                        initOwlCarousel(newThumbnailImages.length);
                    }

                    scope.profilePicture = $filter('filter')(scope.photos, { IsPrimary: true })[0];

                    if (scope.profilePicture != undefined && scope.profilePicture != null) {
                        setImageEditor("data:image/jpeg;base64," + scope.profilePicture.ThumbnailBLOB);
                        scope.picture = angular.copy(scope.profilePicture);
                        scope.preview = true;
                        scope.donotUsePhoto = false;
                    }
                    else {
                        setImageEditor("/Images/profile_male.svg");
                        scope.preview = false;
                        scope.picture = null;
                        scope.donotUsePhoto = true;
                    }
                }
            });

            // ************* Image Builder ******************
            angular.element("#photoFile").on('click', function () {
                angular.element("#photoFile").val('');
            });

            angular.element("#photoFile").on('change', function () {
                formService.initForm(true);
                scope.stopCamera();
                resetPicture();
                if (!isValidFile()) {
                    alertService.error('Invalid image format.');
                    formService.reset();
                    return;
                }
                var imgSize = (angular.element("#photoFile")[0].files[0].size / 1024);
                if (parseInt(imgSize) > 4 * 1024) {
                    alertService.error('Photo size can not be more than 4 MB.');
                    formService.reset();
                    return;
                }

                var reader = new FileReader();
                reader.onload = function (e) {
                    var newImage = new Image();
                    newImage.src = e.srcElement.result;
                    newImage.onerror = function () {
                        alertService.error('Invalid image.');
                        formService.reset();
                    };
                    newImage.onload = function () {
                        setImageEditor(e.srcElement.result);

                        rotation = 0;
                        ratio = 1;

                        scope.$apply(function () {
                            scope.cameraMode = false;
                            setProfilePicture(e.srcElement.result, null);
                        });
                    };
                }
                reader.readAsDataURL(this.files[0]);
                this.files = [];
            });

            angular.element("#rotateLeft").on("click", function () {
                formService.initForm(true);
                rotation -= 90;
                rotation = rotation < 0 ? 270 : rotation;
                CanvasCrop.rotate(rotation);

                setPicture();
            });

            angular.element("#rotateRight").on("click", function () {
                formService.initForm(true);
                rotation += 90;
                rotation = rotation > 360 ? 90 : rotation;
                CanvasCrop.rotate(rotation);

                setPicture();
            });

            angular.element("#zoomIn").on("click", function () {
                if (ratio < maxZoomInLimit) {
                    alertService.warning('Maximum Zoom in limit reached');
                    return;
                }
                formService.initForm(true);
                ratio = ratio * 0.9;
                CanvasCrop.scale(ratio);

                setPicture();
            });

            angular.element("#zoomOut").on("click", function () {
                formService.initForm(true);
                if (ratio > maxZoomOutLimit) {
                    alertService.warning('Maximum Zoom out limit reached');
                    return;
                }
                ratio = ratio * 1.1;
                CanvasCrop.scale(ratio);

                setPicture();
            });

            angular.element("#crop").on("click", function () {
                formService.initForm(true);
                var src = CanvasCrop.getDataURL("jpeg");
                setImageEditor(src);
                ratio = 1; //to reset ratio after crop
                scope.$apply(function () {
                    setProfilePicture(null, src);
                });
            });

            scope.removePicture = function () {
                bootbox.confirm('Are you sure you want to deactivate?', function (confirmed) {
                    if (confirmed) {
                        scope.$apply(function () {
                            setProfilePicture(null, null);
                            setImageEditor("/Images/profile_male.svg");
                            angular.element("#photoFile").val('');
                            scope.preview = false;
                        });
                        formService.reset();
                    }
                });
            };

            scope.editPhoto = function (photoID) {
                if ($injector.has('roleSecurityService')) {
                    var hasPermission = true;
                    if (scope.permissionKey)
                        hasPermission = roleSecurityService.hasPermission(scope.permissionKey, scope.PERMISSION.UPDATE)
                    if (hasPermission) {
                        var picture = scope.photos.filter(function (obj) {
                            return obj[scope.module + "PhotoID"] == photoID;
                        });
                        scope.picture = angular.copy(picture[0]);
                        setImageEditor("data:image/jpeg;base64," + scope.picture.ThumbnailBLOB);
                        scope.preview = true;
                    }
                }
            }

            function setPicture() {
                scope.$apply(function () {
                    var src = CanvasCrop.getActualDataURL("jpeg");
                    setProfilePicture(src, src);
                });
            }

            function setProfilePicture(image, thumbnail) {
                scope.picture = scope.picture || {};

                // Clear profile/preview picture
                if (image == null && thumbnail == null) {
                    resetPicture();
                    return;
                }

                scope.preview = true;
                if (image != null) {
                    var imageContent = dataURItoBlob(image);
                    scope.picture.PhotoBLOB = imageContent.data;
                    scope.picture.ThumbnailBLOB = imageContent.data;
                }
                if (thumbnail != null) {
                    var imageContent = dataURItoBlob(thumbnail);
                    scope.picture.ThumbnailBLOB = imageContent.data;
                }
            }

            function resetPicture() {
                $timeout(function () {
                    scope.picture = {};
                    scope.preview = false;
                });
            }

            function setImageEditor(src) {
                CanvasCrop = $.CanvasCrop({
                    cropBox: ".imageBox",
                    imgSrc: src,
                    limitOver: 2
                });
            };

            function isValidFile() {
                var input = angular.element("#photoFile")[0];
                if (input.files && input.files[0]) {
                    var ftype = input.files[0].type;
                    switch (ftype) {
                        case 'image/png':
                        case 'image/gif':
                        case 'image/jpeg':
                        case 'image/pjpeg':
                            return true;
                        default:
                            return false;
                    }
                }
                else
                    return true;
            };

            function createCarouselAll() {
                var owl = angular.element("#photoLibrary");
                angular.forEach(scope.photos, function (thumbnail, index) {
                    var itemContent = createCarousel(thumbnail);
                    owl.append(itemContent);
                });
            };

            function createCarousel(thumbnail) {
                var itemContent = '<div class="item">'
                                + '<a href="javascript:void(0)" class="delete-photo" ng-click="onDelete({' + scope.module.toLowerCase() + 'PhotoId:' + thumbnail[scope.module + "PhotoID"] + '})" security permission-key="{{permissionKey}}" permission="delete" title="Delete Photo"><i class="fa fa-trash"><span class="sr-only">delete photo</span></i></a>'
                                + '<a href="javascript:void(0)" class="set-photo" ng-click="onSetAsPhoto({' + scope.module.toLowerCase() + 'PhotoId:' + thumbnail[scope.module + "PhotoID"] + '})" security permission-key="{{permissionKey}}" permission="update" title="Set as photo">Set as photo</a>'
                                + '<img src="" ng-click="editPhoto(' + thumbnail[scope.module + "PhotoID"] + ')" />'
                                + '</div>';
                itemContent = $compile(itemContent)(scope);
                itemContent.find('img').attr("src", "data:image/jpeg;base64," + thumbnail.ThumbnailBLOB);
                return itemContent;
            };

            // ******** *VIEW PHOTOS CAROUSEL **********
            function initOwlCarousel(length) {
                var owlCarouselLength = angular.element("#photoLibrary .item").length;
                var owl = angular.element("#photoLibrary");
                if (owl.data('owlCarousel') != undefined) {
                    if (length > owlCarouselLength) {
                        var lastElementContent = createCarousel(scope.photos[length - 1]);
                        owl.data('owlCarousel').addItem(lastElementContent);
                    } else if (length < owlCarouselLength) {
                        owl.data('owlCarousel').removeItem(scope.removeditemIndex);
                    }
                    onAfterUpdate();
                } else if (length > 0) {
                    createCarouselAll();
                    var noOfItem = scope.module == 'Contact' ? 2 : 3;
                    $timeout(function () {
                        owl.owlCarousel({
                            rewindNav: false,
                            itemsCustom: [
                            [0, noOfItem],
                            [401, noOfItem],
                            [1189, noOfItem]
                            ],
                            navigation: true,
                            navigationText: [
                            "<i class='fa fa-caret-left'><span class='sr-only'>previous</span></i>",
                            "<i class='fa fa-caret-right'><span class='sr-only'>next</span></i>"
                            ],
                            pagination: false,
                            afterUpdate: onAfterUpdate
                        });
                    });
                    owlCarouselLength = length;
                }
            }

            function onAfterUpdate(el) {
                $('.owl-item').each(function () {
                    $(this).css('width', '95px');
                });
            }
        },
        templateUrl: '/Photo/PhotoProfile'
    }
}]);
