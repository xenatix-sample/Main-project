angular.module('xenatixApp')
    .directive("topazSignature", ['$compile', function ($compile) {
        var intervalVar;

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
            } catch (e) {
                scope.topazModel.DeviceMessage = 'The signature device software has not been installed!';
            }
        };

        var TopazSign = function (scope) {
            scope.topazModel.hideSignatureImage = true;
            scope.topazModel.hideSignatureCanvas = false;
            scope.topazModel.hideTopazModel = false;
            var canvas = document.getElementById(scope.topazModel.name).getContext('2d');
            SigWebSetDisplayTarget(canvas);
            intervalVar = setInterval(SigWebRefresh, 50);
            SetDisplayXSize(500);
            SetDisplayYSize(100);
            SetJustifyMode(0);
            SetTabletState(1);
            KeyPadClearHotSpotList();
            ClearSigWindow(1);
            ClearTablet();
            scope.topazModel.DisableSignature = false;
        };

        var TopazClear = function (scope) {
            scope.topazModel.hideSignatureImage = true;
            scope.topazModel.hideSignatureCanvas = false;
            scope.topazModel.hideTopazModel = false;
            ClearSigWindow(1);
            ClearTablet();
        };

        var TopazSubmit = function (scope) {
            var tabletPoints = NumberOfTabletPoints();
            if (tabletPoints == 0) {
                SetTabletState(0);
                scope.topazModel.NoPointsCallback();
            }
            else {
                SetSigCompressionMode(1);
                SetImageXSize(500);
                SetImageYSize(100);
                SetImagePenWidth(5);
                GetSigImageB64(scope.topazModel.ImageCallback);
                clearInterval(intervalVar);
            }
        };

        return {
            restrict: 'E',
            scope: {
                topazModel: '=',
                topazReady: '=',
                modulePermission: '@',
                featurePermission: '@',
                actionPermission: '@'
            },
            //templateUrl: '/Plugins/ESignature/SignatureCanvas',
            template: '' +
                //'<keypress-events on-save="topazModel.Submit()" on-sign="topazModel.Sign()" on-clear="topazModel.Clear()"></keypress-events>' +
                '<div class="col-md-12">' +
                '<canvas ng-hide="topazModel.hideSignatureCanvas" id="{{ topazModel.name }}" class="ESigCanvas" width="500" height="100"></canvas>' +
                '<br />' +
                '<div ng-hide="topazModel.hideSignatureBtns"><button type="button" class="btn btn-default btn-lg margin-right-small" id="btnSign" name="btnSign" ng-click="topazModel.Sign()">SIGN</button>' +
                '<button type="button" class="btn btn-default btn-lg margin-right" id="btnClear" name="btnClear" ng-click="topazModel.Clear()">CLEAR</button>' +
                '<button type="button" class="btn btn-default btn-lg" id="btnSubmit" name="btnSubmit" ng-click="topazModel.Submit()" ng-disabled="topazModel.DisableSignature">SAVE</button>' +
                '</div></div>',
            link: function (scope, element, attrs) {
                scope.topazModel.modelNumber = '';
                scope.topazModel.name = scope.topazModel.name || 'SignatureArea';
                scope.topazModel.b64ImageData = null;
                scope.topazModel.DisplayForm = false;
                scope.topazModel.DeviceMessage = 'The signature device could not be located!';
                scope.topazModel.DisableSignature = true;
                scope.topazModel.TabletDetected = false;
                scope.topazModel.Sign = function () { TopazSign(scope); };
                scope.topazModel.Init = function () { TopazInit(scope); };
                scope.topazModel.Clear = function () { TopazClear(scope); };
                scope.topazModel.Submit = function () { TopazSubmit(scope); };
                scope.topazReady = true;
            }
        };
    }]);