angular.module('xenatixApp')
    .directive("eSignature", ['$compile', '$timeout', '$rootScope', 'httpLoaderInterceptor', function ($compile, $timeout, $rootScope, httpLoaderInterceptor) {
        var intervalVar;
        var webRefresh = 50;
        var xSize = 500;
        var ySize = 100;
        var bitYes = 1;
        var bitNo = 0;
        var imagePenWidth = 5;
        var TopazInit = function (scope) {
            try {
                ResetParameters();
                SetTabletState(1);

                if (GetTabletState() != 0) {
                    scope.topazModel.TabletDetected = true;

                    var modNum = -1;
                    modNum = TabletModelNumber();

                    if (modNum == 0) {
                        scope.topazModel.modelNumber = "Model: T-S460";
                    } else if (modNum == 11 || modNum == 12) {
                        scope.topazModel.modelNumber = "Model: T-L462";
                    } else if (modNum == 1) {
                        scope.topazModel.modelNumber = "Model: T-LBK766";
                    } else if (modNum == 15) {
                        scope.topazModel.modelNumber = "Model: T-LBK460";
                    } else if (modNum == 57) {
                        scope.topazModel.modelNumber = "Model: T-LBK57GC"; 
                    }
                }

                SetTabletState(0);

                //Show the JSignature Plugin in case the Model number is not detected
                //if (scope.topazModel.modelNumber == '') {
                    initializeSignature(scope);
                //}
            } catch (e) {
                scope.topazModel.DeviceMessage = 'The signature device software has not been installed!';
                //Show the JSignature Plugin in case any Error is detected
                initializeSignature(scope);
            }
        };

        //Call the jSignature contructor to make sure that the DOM is loaded
        var initializeSignature = function (scope) {
            $timeout(function () {
                if (scope.topazModel.modelNumber == '') {
                    initializeJSignature(scope);
                    enablejSignatureCtrl(scope, false);
                }
                if (scope.topazModel.imageBLOB) {//if we got blob image before the directive renders, like assessment engine.
                    scope.topazModel.b64ImageData = scope.topazModel.imageBLOB;
                }
            });
        };

        //Initialize the jSignature Contructor
        var initializeJSignature = function (scope) {
            //Get the jSignature DIV Element
            var jSignatureElemObj = angular.element('#' + scope.topazModel.name);

            //Initialize the jSignature
            if (jSignatureElemObj != undefined && jSignatureElemObj != null) {
                var dvMouseSignature = jSignatureElemObj.jSignature({ 'UndoButton': false });

                //Assign the jSignature Element to the topazModel to handle reset/getData methods
                scope.topazModel.mouseSignature = dvMouseSignature;

                //Track the jSignature change event
                dvMouseSignature.bind('change', function (e) {
                    scope.topazModel.clickClear = scope.topazModel.DisableClear = false;
                    if (scope.topazModel.DisableSignature) {
                        scope.$apply(function () {
                            scope.topazModel.DisableSignature = false;
                        });
                    }
                })
            }
        };
        var TopazSignOrClear = function (scope) {
            if (scope.topazModel.TabletDetected) {
                NumPointsLastTime = 1;
                scope.topazModel.hideSignatureImage = true;
                scope.topazModel.hideSignatureCanvas = false;
                scope.topazModel.hideTopazModel = false;
                var canvas = document.getElementById(scope.topazModel.name).getContext('2d');
                SigWebSetDisplayTarget(canvas);
                intervalVar = setInterval(SigWebRefresh, webRefresh);
                SetDisplayXSize(xSize);
                SetDisplayYSize(ySize);
                SetJustifyMode(bitNo);
                SetTabletState(bitYes);
                KeyPadClearHotSpotList();
                ClearSigWindow(bitYes);
                ClearTablet();
            }
            else {
                clearjSignatureImage(scope);
                enablejSignatureCtrl(scope, true);
                setjSignatureBackgroundColor(scope.topazModel.name, '#FFFFFF');
            }
            scope.topazModel.b64ImageData = '';
        };

        var TopazSign = function (scope) {
            TopazSignOrClear(scope);
            if (scope.topazModel.TabletDetected) {
                scope.topazModel.DisableSignature = scope.topazModel.DisableClear = false;
            }
        }

        var TopazClear = function (scope) {
            TopazSignOrClear(scope);
            scope.topazModel.clickClear = true;
            //On clear call the callback function to clear the signature
            if (scope.topazModel.ClearCallback) {
                scope.topazModel.ClearCallback(null);
            }
            scope.topazModel.DisableSignature = true;

        };
        var TopazReset = function (scope) {
            if (scope.topazModel.TabletDetected) {
                TopazInit(scope);
                var canvas = document.getElementById(scope.topazModel.name).getContext('2d');
                canvas.clearRect(0, 0, xSize, ySize);
            }
            else {
                scope.topazModel.mouseSignature.jSignature('reset');
                enablejSignatureCtrl(scope, false);
            }
        };
        var TopazSubmit = function (scope) {
            httpLoaderInterceptor.setLoading(true);
            $rootScope.$emit('triggerLoader', { method: 'post', isLoading: true });
            if (scope.topazModel.TabletDetected) {
                var tabletPoints = NumberOfTabletPoints();
                if (tabletPoints == 0) {
                    SetTabletState(bitNo);
                    scope.topazModel.NoPointsCallback();
                    imgCallback(scope);
                }
                else {
                    SetSigCompressionMode(bitYes);
                    SetImageXSize(xSize);
                    SetImageYSize(ySize);
                    SetImagePenWidth(imagePenWidth);
                    GetSigImageB64(function (resp) {
                        imgCallback(scope, resp);
                    });
                    clearInterval(intervalVar);
                }
            }
            else {
                //Get the Base64 data
                var b64String = scope.topazModel.mouseSignature.jSignature("getData");
                var loc = b64String.search("base64,");
                var retstring = !scope.topazModel.clickClear ? b64String.slice(loc + 7, b64String.length) : null;
                //Call the Callback function
                if (scope.topazModel.ImageCallback) {
                    imgCallback(scope, retstring);
                }
                enablejSignatureCtrl(scope, false);
            }
            scope.topazModel.DisableSignature = scope.topazModel.DisableClear = true;
        };

        var imgCallback = function (scope, blob) {
            if (blob)
                scope.topazModel.ImageCallback(blob);
            httpLoaderInterceptor.ignore(true);
            scope.topazModel.TabletDetected && scope.$apply();
        }
        //Set the jSignature Backgroung color to white
        var setjSignatureBackgroundColor = function (dvSignatureName, color) {
            var canvas = $('#' + dvSignatureName).find('canvas');

            if (canvas != undefined && canvas != null && canvas.length > 0) {
                var ctx = canvas[0].getContext('2d');
                if (ctx != undefined && ctx != null) {
                    ctx.rect(0, 0, canvas[0].width, canvas[0].height);
                    ctx.fillStyle = color;
                    ctx.fill();
                }
            }
        };

        //Clear the jSignature Image
        var clearjSignatureImage = function (scope) {
            if (!scope.topazModel.DisableSignature) {
                scope.topazModel.mouseSignature.jSignature('reset');
                setjSignatureBackgroundColor(scope.topazModel.name, '#FFFFFF');
                scope.topazModel.DisableSignature = scope.topazModel.DisableClear = true;
            }
        }

        //Enable/Disable the jSignature Element control
        var enablejSignatureCtrl = function (scope, isEnable) {
            scope.topazModel.mouseSignature.jSignature(isEnable ? 'enable' : 'disable');
        }

        return {
            restrict: 'E',
            scope: {
                topazModel: '=',
                topazReady: '=',
                modulePermission: '@',
                featurePermission: '@',
                actionPermission: '@',
                signButtonText: '@',
                signatureRequired: '=',
                validationName: '@',
                onPreSignature: '&?'
            },
            template:
            '<div class="col-md-12 padding-bottom-small">' +
                 '<div class="row">' +
                    '<div class="col-md-12 col-sm-12">' +
                        '<img ng-if="topazModel.b64ImageData.length > 1" ng-src="data:image/png;base64,{{topazModel.b64ImageData}}" alt="signature" style="vertical-align: middle; padding-bottom:50px;" />' +
                    '</div>' +
                '</div>' +
                '<div class="row" ng-show="!topazModel.b64ImageData || topazModel.b64ImageData === \'\'">' +
                    '<div class="col-md-7 col-sm-12">' +
                        '<strong ng-bind="topazModel.modelNumber" ng-if="false"></strong>' +
                        '<div class="row padding-bottom-small">' +
                            '<div class="col-md-12">' +
                                '<canvas ng-if="topazModel.TabletDetected" id="{{ topazModel.name }}" class="ESigCanvas" width="500" height="100"></canvas>' +
                                '<div ng-if="!topazModel.TabletDetected" id="{{ topazModel.name }}" width="500" height="100"></div>' +
                                '<br />' +
                            '</div>' +
                        '</div>' +
                    '</div>' +
                '</div>' +
                 '<div ng-hide="topazModel.hideSignatureBtns">' +
                                    '<button type="button" class="btn btn-default btn-lg margin-right-small" id="btnSign" name="btnSign" ng-click="topazModel.Sign()">{{ signbuttontext }} </button>' +
                                    '<button type="button" class="btn btn-default btn-lg margin-right" id="btnClear" name="btnClear" ng-click="topazModel.Clear()" ng-disabled="topazModel.b64ImageData ? !topazModel.b64ImageData : topazModel.DisableClear">CLEAR</button>' +
                                    '<button type="button" class="btn btn-default btn-lg" id="btnSubmit" name="btnSubmit" validate-method="" ng-click="topazModel.Submit()" ng-disabled="topazModel.b64ImageData ? topazModel.b64ImageData : topazModel.DisableSignature">SAVE</button>' +
                                '</div>' +
                '<div class="row" ng-if="topazModel.TabletDetected && topazModel.modelNumber === \'\' && (topazModel.b64ImageData === \'\' || topazModel.b64ImageData === null)">' +
                    '<div class="col-md-4 col-sm-4">' +
                        '<span class="error-block" ng-bind="topazModel.DeviceMessage"></span>' +
                    '</div>' +
                '</div>' +
                '<div> <input type="hidden" name="{{validationName}}" data-validation-name="{{validationName}}" ng-model="topazModel.b64ImageData" ng-required="signatureRequired" ></div>' +
            '</div>',
            link: function (scope, element, attrs) {
                scope.signbuttontext = (attrs.signButtonText ? attrs.signButtonText : 'SIGN');
                scope.topazModel.modelNumber = '';
                scope.topazModel.name = scope.topazModel.name || 'SignatureArea';
                //scope.topazModel.b64ImageData = null;
                scope.topazModel.DisplayForm = false;
                scope.topazModel.DeviceMessage = 'The signature device could not be located!';
                scope.topazModel.DisableSignature = scope.topazModel.DisableClear = true;
                scope.topazModel.TabletDetected = false;
                scope.topazModel.Sign = function () { TopazSign(scope); };
                scope.topazModel.Init = function () { TopazInit(scope); };
                scope.topazModel.Clear = function () { TopazClear(scope); };
                scope.topazModel.Reset = function () { TopazReset(scope); };
                scope.topazModel.Submit = function () {
                    if (scope.onPreSignature) {
                        scope.onPreSignature().then(function (result) {
                            TopazSubmit(scope);
                        });
                    }
                    else {
                        TopazSubmit(scope);
                    }
                };
                scope.topazReady = true;
            }
        };
    }]);
