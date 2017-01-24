(function () {
    angular.module('xenatixApp')
    .controller('serviceDetailsController', ['$filter', 'alertService', '$stateParams', '$state', '$timeout', 'formService', '$scope', '$q', '$rootScope', 'lookupService', 'helperService', 'serviceDetailsService', 'serviceDefinitionService', '_',
        function ($filter, alertService, $stateParams, $state, $timeout, formService, $scope, $q, $rootScope, lookupService, helperService, serviceDetailsService, serviceDefinitionService, _) {
            var serviceWorkflowTable = $("#serviceWorkflowTable");
            var checkFormStatus = function (state) {
                $timeout(function () {
                    var stateDetail = { stateName: "servicedetails", validationState: state.valid ? 'valid' : 'invalid' };
                    $rootScope.$broadcast('rightNavigationServicesHandler', stateDetail);
                })
            };

            var getLookups = function (typeName) {
                return lookupService.getLookupsByType(typeName);
            }

            var deliveryMethods = getLookups('DeliveryMethod');
            var serviceLocation = getLookups('ServiceLocation');
            var recipients = getLookups('RecipientCode');
            var serviceStatus = getLookups('ServiceStatus');
            var attendanceStatus = getLookups('AttendanceStatus');
            var trackingField = getLookups('TrackingField');
            var credentials = getLookups('Credential');
            var savedWorkflows;

            $scope.serviceWorkflowData = lookupService.getLookupsByType('ServiceWorkflowType');
            var copyOfserviceWorkflowData = angular.copy($scope.serviceWorkflowData);
            var init = function () {
                initializeBootstrapTable();
                $scope.initServiceDetails();
                var promise = [];
                promise.push(getServiceDefinition());
                promise.push(getServiceWorkflow());
                $q.all(promise).then(function () {
                    resetForm();
                });
                checkFormStatus({ valid: true });
            }


            var getServiceDefinition = function () {
                serviceDefinitionService.getServiceDefinitionByID($stateParams.ServicesID).then(function (data) {
                    if (hasData(data)) {
                        $scope.serviceDefinition = data.DataItems[0];
                        $scope.serviceDefinition.ServiceType = lookupService.getText(LOOKUPTYPE.ServiceConfigType, $scope.serviceDefinition.ServiceConfigServiceTypeID);
                    }
                })
            }

            var getServiceWorkflow = function () {
                return serviceDetailsService.getServiceWorkflows($stateParams.ServicesID).then(function (data) {
                    if (hasData(data)) {
                        savedWorkflows =$.map(data.DataItems,function(obj){
                            return { ModuleComponentID: obj.ModuleComponentID };
                    });
                        serviceWorkflowTable.bootstrapTable('load', data.DataItems);
                        serviceWorkflowFilter();
                    }
                    else {
                        serviceWorkflowTable.bootstrapTable('removeAll');
                    }
                })
            }

            $scope.isServiceWorkflowDisabled = function () {
                if ((savedWorkflows && savedWorkflows.length == 4) || $scope.serviceDetails.ServicesModuleComponentID) {
                    return true;
                }
            }

            var resetForm = function () {
                if ($scope.serviceDetailsForm)
                    $rootScope.formReset($scope.serviceDetailsForm);
            }

            $scope.editServiceDetails = function (servicesModuleComponentID, serviceID, moduleComponentID) {
                if (formService.isDirty($scope.serviceDetailsForm.$name)) {
                    bootbox.confirm("Any unsaved data will be lost. Do you want to continue?", function (result) {
                        if (result == true) {
                            onEdit(servicesModuleComponentID, serviceID, moduleComponentID);
                        }
                    })
                }
                else {
                    onEdit(servicesModuleComponentID, serviceID, moduleComponentID);
                }
            }

            var onEdit = function (servicesModuleComponentID, serviceID, moduleComponentID) {
                serviceDetailsService.getServiceDetails(serviceID, moduleComponentID).then(function (data) {
                    if (hasData(data)) {
                        $scope.initServiceDetails();
                        $scope.serviceDetails = data.DataItems[0];
                        $scope.serviceDetails.ServicesModuleComponentID = servicesModuleComponentID;
                        serviceWorkflowFilter();
                        resetForm();
                    }
                })
            }


            var serviceWorkflowFilter = function () {
                if ($scope.serviceDetails.ServicesModuleComponentID) {
                     $scope.serviceWorkflowData=$filter('filter')(copyOfserviceWorkflowData, function (item) { return item.ID == $scope.serviceDetails.ModuleComponentID });
                }
                else if (savedWorkflows && savedWorkflows.length > 0) {
                    $scope.serviceWorkflowData = $.grep(copyOfserviceWorkflowData, function (item, indx) {
                        var result = _.find(savedWorkflows, {
                            ModuleComponentID: item.ID
                        })
                        if (!result) {
                            return item;
                        }
                    })
                }
            }



            var initializeBootstrapTable = function () {

                $scope.tableoptions = {
                    pagination: true,
                    pageSize: 10,
                    pageList: [10, 25, 50, 100],
                    search: false,
                    showColumns: true,
                    data: [],
                    undefinedText: '',
                    columns: [
                {
                    field: 'ServiceWorkflow',
                    title: 'Service Workflow',
                    sortable: true
                },
                {
                    field: 'DeliveryMethod',
                    title: 'Delivery Method',
                    sortable: true
                },
                {
                    field: 'PlaceOfService',
                    title: 'Place of Service',
                    sortable: true
                },
                {
                    field: 'Recipient',
                    title: 'Recipient',
                    sortable: true
                },
                {
                    field: 'ServiceStatus',
                    title: 'Service Status',
                    sortable: true
                },
                {
                    field: 'AttendanceStatus',
                    title: 'Attendance Status',
                    sortable: true
                },
                {
                    field: 'TrackingField',
                    title: 'Tracking Field',
                    sortable: true
                },
                {
                    field: 'Credentials',
                    title: 'Credentials',
                    sortable: true
                },
                {
                    field: 'ServicesModuleComponentID',
                    title: '',
                    formatter: function (value, row) {
                        return '<span class="text-nowrap">' +
                             '<a href="javascript:void(0)" data-default-action id="editService" name="editService" data-toggle="modal"  ng-click="editServiceDetails(' + value + ',' + row.ServicesID + ',' + row.ModuleComponentID + ')" title="Edit" security permission-key="{{permissionKey}}" permission="update" space-key-press><i class="fa fa-pencil fa-fw border-left margin-left padding-left-small padding-right-small" /></a>'

                    }
                }
                    ]
                };
            }

            $scope.initServiceDetails = function () {
                $scope.pageSecurity = 0;
                $scope.serviceDetails = {
                    ServicesID: $stateParams.ServicesID,
                    ModuleComponentID: null,
                    ServicesModuleComponentID: 0,
                    DeliveryMethods: [],
                    ServiceLocation: [],
                    Recipients: [],
                    ServiceStatus: [],
                    AttendanceStatus: [],
                    TrackingField: [],
                    Credentials: []
                }
                $scope.deliveryMethods = angular.copy(deliveryMethods);
                $scope.serviceLocation = angular.copy(serviceLocation);
                $scope.recipients = angular.copy(recipients);
                $scope.serviceStatus = angular.copy(serviceStatus);
                $scope.attendanceStatus = angular.copy(attendanceStatus);
                $scope.trackingField = angular.copy(trackingField);
                $scope.credentials = angular.copy(credentials);
                serviceWorkflowFilter();
                resetForm();
            };

           
            $scope.save = function (hasErrors) {
                if (!hasErrors && formService.isDirty($scope.serviceDetailsForm.$name)) {
                    serviceDetailsService.saveServiceDetails($scope.serviceDetails).then(function (data) {
                        if (data.data.ResultCode == 0) {
                            alertService.success('Service Details has been saved.');
                            getServiceWorkflow();
                            $scope.initServiceDetails();
                        }
                    })
                }
            }

            init();
        }]);
})();