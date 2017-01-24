angular.module('xenatixApp')
    .controller('eSignatureController', ['$scope', 'alertService', function ($scope, alertService) {

        $scope.Sign = function () {
            var canvas = document.getElementById('SignatureArea').getContext('2d');
            SigWebSetDisplayTarget(canvas);
            setInterval(SigWebRefresh, 50);
            SetDisplayXSize(500);
            SetDisplayYSize(100);
            SetJustifyMode(0);
            SetTabletState(1);
            KeyPadClearHotSpotList();            
            ClearSigWindow(1);
            ClearTablet();
        };

        $scope.Init = function () {
            ResetParameters();
        };

        $scope.Clear = function() {
            ClearSigWindow(1);
            ClearTablet();
        }

        function sigImageCallback(str) {
            //what do with the saved base64 string
            var tmp = str;
            alert(str);
        }

        $scope.Submit = function () {
            var tabletPoints = NumberOfTabletPoints();
            if (tabletPoints == 0) {
                alertService.error("Please sign before submitting the form");
            }
            
            SetTabletState(0);
            SetSigCompressionMode(1);

            //Get BMP byte array and convert to a Base64 string
            SetImageXSize(500);
            SetImageYSize(100);
            SetImagePenWidth(5);
            GetSigImageB64(sigImageCallback);               
        }
        
        $scope.Init();
    }]);