angular.module('xenatixApp')
.directive('auditOn', ['auditService', function (auditService)
{
    return {
        restrict: 'A',
        link: function ($scope, $element, $attrs)
        {
            var auditData = {};
            if ($attrs.auditWatch != undefined && $attrs.auditWatch != null)
            {
                $attrs.$observe('auditWatch', function (value)
                {
                    if (value !== undefined && value != 0 && value != '') {
                        auditData.AuditKey = $attrs.auditKey;
                        auditData.AuditValue = $attrs.auditValue;
                        auditData.CreatedOn = moment.utc();
                        auditService.addAudit(auditData);
                   }
                });
            }

            var eventType = $attrs.auditOn;

            if (eventType != undefined && eventType != '') {
                angular.element($element[0]).bind(eventType, function ($event) {
                    if ($attrs.auditValue != undefined) {
                        auditData.AuditKey = $attrs.auditKey;
                        auditData.AuditValue = $attrs.auditValue;
                        auditData.CreatedOn = moment.utc();
                        auditService.addAudit(auditData);
                    }
                })
            }
        }
    };
}]);