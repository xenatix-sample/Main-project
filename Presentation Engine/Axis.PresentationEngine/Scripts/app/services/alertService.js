angular.module('xenatixApp')
    .value("toastr", window.toastr)
    .factory('alertService', ['toastr', function (toastr) {
        toastr.options = {
            "closeButton": true,
            "newestOnTop": true,
            "positionClass": "toast-top-center",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };

        function success(msg) {
            toastr.success(msg);
        };

        function error(msg) {
            toastr.error(msg);
        };

        function warning(msg) {
            toastr.warning(msg);
        };

        function info(msg) {
            toastr.info(msg);
        };

        return {
            success: success,
            error: error,
            warning: warning,
            info: info
        };
    }]);