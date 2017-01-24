(function () {
    angular.module('xenatixApp')
    .controller('servicesController', ['$state', '$scope', 'lookupService', 'helperService', 'serviceDefinitionService',
        function ($state, $scope, lookupService, helperService, serviceDefinitionService) {
            
            var serviceDefinitionState = "servicedefinition"
            var servicesTable = $("#servicesTable");
            
            var init = function () {
                initializeBootstrapTable();
                $scope.getServices();
            }
            
            var formattedDate = function (date, defaultValue) {
                return helperService.getFormattedDate(date, defaultValue);
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
                            field: 'EffectiveDate',
                            title: 'EFF. DATE',
                            formatter: function (value) {
                                return formattedDate(value);
                            },
                            sortable: true
                        },
                        {
                            field: 'ServiceName',
                            title: 'SERVICE',
                            sortable:true
                        },
                        {
                            field: 'ServiceCode',
                            title: 'SERVICE CODE',
                            sortable: true
                            
                        },
                        {
                            field: 'ServiceConfigServiceTypeID',
                            title: 'SERVICE TYPE',
                            formatter: function (value) {
                                 if (value)
                                    return lookupService.getText(LOOKUPTYPE.ServiceConfigType, value);
                                else
                                    return "";
                            },
                            sortable: true

                        },

                    
                        {
                            field: 'ExpirationDate',
                            title: 'EXP. DATE',
                            formatter: function (value) {
                                 return formattedDate(value);
                            },
                            sortable: true
                        },
                        {
                            field: 'CreatedBy',
                            title: 'Created By',
                            formatter: function (value, row) {
                                if (value)
                                    return lookupService.getText(LOOKUPTYPE.Users, value);
                                else
                                    return "";
                            },
                            sortable: true
                        },
                        {
                            field: 'ModifiedBy',
                            title: 'Modified By',
                            formatter: function (value, row) {
                                if (value)
                                    return lookupService.getText(LOOKUPTYPE.Users, value);
                                else
                                    return "";
                            },
                            sortable: true
                        },
                        {
                            field: 'ModifiedOn',
                            title: 'Modified Date',
                            formatter: function (value, row, index) {
                                return formattedDate(value);
                            },
                            sortable: true
                        },
                         {
                             field: 'EncounterReportable',
                             title: 'ENCOUNTER REPORTABLE',
                             formatter: function (value, row, index) {
                                 return value ? "Yes" : "No";
                             },
                             sortable: true
                         },
                        {
                            field: 'ServicesID',
                            title: '',
                            formatter: function (value, row, index) {
                                return '<span class="text-nowrap">' +
                                     '<a href="javascript:void(0)" data-default-action id="editService" name="editService" data-toggle="modal"  ng-click="edit(' + value +   ')" title="Edit" security permission-key="{{permissionKey}}" permission="update" space-key-press><i class="fa fa-pencil fa-fw border-left margin-left padding-left-small padding-right-small" /></a>'
                                     
                            }
                        }
                    ]
                };
            }

            $scope.permissionKey = $state.current.data.permissionKey;
            $scope.searchText = "";

            $scope.getServices = function (searchText) {
                return serviceDefinitionService.getServices(searchText).then(function (data) {
                   if (hasData(data)) {
                      servicesTable.bootstrapTable('load', data.DataItems);
                    }
                   else {
                       servicesTable.bootstrapTable('removeAll');
                   }
                })
            }

            $scope.edit = function (servicesID) {
                $scope.Goto(serviceDefinitionState, { ServicesID: servicesID });
            }

            
            init();
        }]);
}());