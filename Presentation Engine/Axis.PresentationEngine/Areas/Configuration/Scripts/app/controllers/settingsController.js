angular.module('xenatixApp')
    .controller('settingsController', ['$scope', '$modal', 'settingsService', 'alertService', '$rootScope', '$filter', 'formService', function ($scope, $modal, settingsService, alertService, $rootScope, $filter, formService) {
    		$scope.isLoading = true;
    		$scope.settings = [];

    		var resetForm = function () {
    				$rootScope.formReset($scope.ctrl.settingsForm);
    		};
    		var settingsTable = $("#settingsTable");

    		$scope.initializeBootstrapTable = function () {
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
																field: 'SettingName',
																title: 'Name'
														},
														{
																field: 'SettingValue',
																title: 'Value'
														},
														{
																field: 'SettingType',
																title: 'Type'
														},
														{
																field: 'SettingID',
																title: '',
																formatter: function (value, row, index) {
																		return '<a href="#" id="editsetting" data-default-action name="editsetting" data-toggle="modal" ng-click=edit(' + value + ') title="Edit Setting" security permission-key="SiteAdministration-Settings-Settings" permission="update" space-key-press><i class="fa fa-pencil fa-fw" /></a>';

																}
														}
    						]
    				};
    		};

    		$scope.get = function () {
    				$scope.isLoading = true;
    				settingsService.get().then(function (data) {
    						if (data.ResultCode !== 0) {
    								alertService.error('Error while loading the settings');
    						}
    						$scope.settings = data.DataItems;
    						if (data.DataItems != null) {
    								settingsTable.bootstrapTable('load', data.DataItems);
    								$rootScope.setFocusToGrid('settingsTable');
    						} else {
    								settingsTable.bootstrapTable('removeAll');
    						}
    						$scope.isLoading = false;
    						resetForm();
    				},
								function (errorStatus) {
										$scope.isLoading = false;
										alertService.error('Error while loading the settings');
								});
    		};

    		$scope.initSetting = function () {
    				$scope.newSetting = {
    						SettingID: '',
    						SettingName: '',
    						SettingValue: '',
    						SettingTypeID: '',
    						SettingType: '',
    						SettingValuesID: ''
    				};
    		};

    		$scope.edit = function (SettingID) {
    				var setting = angular.copy($filter('filter')($scope.settings, { SettingID: SettingID })[0]);
    				$scope.newSetting = setting;
    				$('#settingModel').modal('show');
    				$('#settingModel').on('shown.bs.modal', function () {
    						setTimeout(function () {
    								$('#settingvalue').focus();
    						}, 500);
    				});
    				resetForm();
    		};

    		$scope.cancel = function () {
    				$rootScope.onCancel(function (result) {
    						if (result) {
    								$('#settingModel').modal('hide');
    						}
    				});


    		};

    		$scope.save = function () {
    				var isDirty = formService.isDirty();
    				if (isDirty) {
    						settingsService.update($scope.newSetting).then(function (data) {
    								$('#settingModel').modal('hide');
    								if (data.ResultCode !== 0) {
    										alertService.error(data.ResultMessage);
    								}
    								else {
    										alertService.success('Setting has been updated.');
    										$scope.get();
    								}
    						}, function (errorStatus) {
    								alertService.error('Error while saving the setting');
    						});
    				}
    		};

    		$scope.get();
    		$scope.initializeBootstrapTable();
    }]);