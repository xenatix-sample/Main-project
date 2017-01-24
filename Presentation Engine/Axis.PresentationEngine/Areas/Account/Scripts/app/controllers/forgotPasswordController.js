angular.module('xenatixApp')
    .controller('forgotPasswordController', ['$scope', '$timeout', 'forgotPasswordService', 'alertService', 'settings', function ($scope, $timeout, forgotPasswordService, alertService, settings) {
        $scope.email = '';
        $scope.securityQuestions = [];

        $scope.init = function () {
            $scope.resetPassword = {};
        };

        $scope.sendResetLink = function () {
            //Disable button for 10 sec. TFS#6585
            $timeout(function () {
                $scope.initiateResetPasswordForm.$invalid = false;
            },10000);
            forgotPasswordService.sendResetLink($scope.email).then(function (data) {
                if (data.ResultCode == 0) {
                    if (data.DataItems && data.DataItems.length > 0 && data.DataItems[0].ADFlag) {
                        alertService.info(data.DataItems[0].ADUserPasswordResetMessage);
                    } else {
                    alertService.success('Reset password link has been sent to your email.');
                }
    
                } else {
                    alertService.error(data.ResultMessage);
                }
            },
            function(errorSttaus) {
                alertService.error('OOPS! Something went wrong');
            });
            $scope.initiateResetPasswordForm.$invalid = true;
        };

        $scope.verifyResetIdentifier = function () {
            $scope.resetPassword.ResetIdentifier = getQueryStringParams('resetIdentifier');

            forgotPasswordService.verifyResetIdentifier($scope.resetPassword).then(function (data) {
                if (data.ResultCode == 0) {
                    $scope.getSecurityQuestions();
                }
                else {
                    alertService.error(data.ResultMessage);
                }
            },
            function (errorSttaus) {
                alertService.error('OOPS! Something went wrong');
            });
        };

        $scope.verifySecurityDetails = function () {
            $scope.resetPassword.ResetIdentifier = getQueryStringParams('resetIdentifier');

            forgotPasswordService.verifySecurityDetails($scope.resetPassword).then(function (data) {
                if (data.ResultCode == 0) {
                    window.location.href = '/Account/ForgotPassword/ResetPassword?resetIdentifier=' + $scope.resetPassword.ResetIdentifier;
                }
                else {
                    alertService.error(data.ResultMessage);
                }
            },
            function (errorSttaus) {
                alertService.error('OOPS! Something went wrong');
            });
        };

        $scope.updatePassword = function () {
            $scope.resetPassword.ResetIdentifier = getQueryStringParams('resetIdentifier');

            forgotPasswordService.resetPassword($scope.resetPassword).then(function (data) {
                if (data.ResultCode == 0) {
                    alertService.success('Password has been changed successfully. Please wait while we transfer you to the login page...');
                    $timeout(function () {
                        window.location.href = '/Account/Account';
                    }, 3000);
                }
                else {
                    alertService.error(data.ResultMessage);
                }
            },
            function (errorSttaus) {
                alertService.error('OOPS! Something went wrong');
            });
        };

        $scope.getSecurityQuestions = function () {
            forgotPasswordService.getSecurityQuestions().then(function (data) {
                $scope.securityQuestions = data.DataItems;
            },
            function (errorSttaus) {
                alertService.error('OOPS! Something went wrong');
            });
        };

        $scope.init();
    }]);